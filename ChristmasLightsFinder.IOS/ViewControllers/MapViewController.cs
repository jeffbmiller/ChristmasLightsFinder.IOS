using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using MapKit;
using CoreLocation;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

namespace ChristmasLightsFinder.IOS
{
	partial class MapViewController : UIViewController
	{
		CLLocationManager locationManager;

		private readonly HouseService houseService;
		private readonly HouseImageCacheRepository imageCacheRepo;
		private bool isAdmin;
		private NSObject observer;
		private bool shouldCenterOnLocation;
		private MapFilter mapFilter;

		private List<House> houses;

		public MapViewController (IntPtr handle) : base (handle)
		{
			houseService = new HouseService ();
			imageCacheRepo = new HouseImageCacheRepository ();
			this.mapFilter = MapFilter.All;
			this.NavigationItem.BackBarButtonItem = new UIBarButtonItem ("Back", UIBarButtonItemStyle.Plain,null);
			isAdmin =  NSBundle.MainBundle.ObjectForInfoDictionary("AllowAddNew").ToString() == "1";
		}

		public string GetRankForHouse(House house)
		{
			var grouping = houses.GroupBy (x => x.Likes).ToList();

			var rank = 1;
			foreach (var group in grouping.OrderByDescending(x=>x.Key)) {
				if (group.Contains (house))
					return group.Count() > 1 ? string.Format("T{0}",rank.ToString()) : rank.ToString ();
				rank++;
			}

			return null;
		}

		public HouseImageCacheRepository ImageCacheRepo { get { return imageCacheRepo; } }

		public async override void ViewDidLoad ()
		{

			if (observer == null){
				observer = NSNotificationCenter.DefaultCenter.AddObserver (new NSString ("ReloadMap"), (x)=>{
					if (!IsBusy)
						Reload(true);
				});
			}
			// If Not Admin Remove Add button
			if (!isAdmin) {
				this.NavigationItem.RightBarButtonItem = null;
			}
			base.ViewDidLoad ();
			this.mapView.Delegate = new MapDelegate (this);

			Center ();

			Reload (true);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			if (!IsBusy)
				Reload (false);
		}

		private void Center()
		{
			var coords = new CLLocationCoordinate2D(49.8423203,-99.9584185);
			var span = new MKCoordinateSpan(0.1, 0.1);
			mapView.Region = new MKCoordinateRegion(coords, span);
		}
			
		private async void Reload(bool redownload)
		{
			try {
				if (redownload)
				{
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
					var results = await houseService.GetHousesAsync (mapFilter);
					houses = results.ToList();
				}
				var existing = this.mapView.Annotations.OfType<HouseMapAnnotation>().ToList ();

//				//If Deleted from Server or filtered remove from map
				foreach (var annotation in existing) {
					if (!houses.Any(x=>x.ObjectId == annotation.House.ObjectId))
						this.mapView.RemoveAnnotation(annotation);
				}

				//Update Annotation from Server
				foreach (var house in houses) {
					var existingAnnotation = existing.FirstOrDefault (x => x.House.ObjectId == house.ObjectId);
					var rank = GetRankForHouse(house);
					if (existingAnnotation != null) 
					{
						
						if (existingAnnotation.House.UpdatedAt < house.UpdatedAt ||  existingAnnotation.Rank != rank)
						{
							this.mapView.RemoveAnnotation(existingAnnotation);
							var annotation = new HouseMapAnnotation (new CLLocationCoordinate2D(house.Latitude,house.Longitude), house.Address, house, rank);
							this.mapView.AddAnnotation(annotation);
						}
					}
					else{

						var annotation = new HouseMapAnnotation (new CLLocationCoordinate2D(house.Latitude,house.Longitude), house.Address, house, rank);

						this.mapView.AddAnnotation (annotation);
					}
				}

			}
			catch(Exception e) {
				BigTed.BTProgressHUD.ShowErrorWithStatus ("Error Communication With Server");
			}
			finally{
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			}
		}

		private bool IsBusy { get {return UIApplication.SharedApplication.NetworkActivityIndicatorVisible; } }

		partial void trackCurrentLocationBtn_Activated (UIBarButtonItem sender)
		{
			if (locationManager == null)
			{
				// Set a movement threshold for new events.
				locationManager = new CLLocationManager();
				locationManager.DistanceFilter = 500f;
			}

			if (CLLocationManager.Status == CLAuthorizationStatus.NotDetermined){
				locationManager.RequestWhenInUseAuthorization();
				mapView.ShowsUserLocation = true;
				return;
			}

			if (CLLocationManager.Status != CLAuthorizationStatus.AuthorizedWhenInUse){

				new UIAlertView("Turn On Location Services For \"Christmas Lights Finder\" To Determine Your Locaiton.","Go to Settings -> Location Services -> Christmas Lights Finder to turn on.",null,"Close",null).Show();
			}
			else {
				//Toggle Show User Location
				if (mapView.ShowsUserLocation == true)
					mapView.ShowsUserLocation = false;
				else 
				{
					locationManager.RequestWhenInUseAuthorization();
					shouldCenterOnLocation = true;
					mapView.ShowsUserLocation = true;
				}
			}
		}

		partial void FilterMapButton_Activated (UIBarButtonItem sender)
		{
			var selections = Enum.GetValues(typeof(MapFilter)).Cast<MapFilter>().Select(x=>x.Description()).ToList();
			var actionSheet = new UIActionSheet("Filter Map",null,"Cancel",null,selections.ToArray());
			actionSheet.ShowInView(this.View);

			actionSheet.Dismissed += (object s, UIButtonEventArgs e) => {
				if (actionSheet.CancelButtonIndex != e.ButtonIndex){
					mapFilter = selections.ElementAt((int)e.ButtonIndex).FromDescription<MapFilter>();
					Reload(true);
				}
			};
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			if (observer != null)
				NSNotificationCenter.DefaultCenter.RemoveObserver (observer);
		}

		protected class MapDelegate : MKMapViewDelegate
		{
			protected string annotationIdentifier = "BasicAnnotation";
			UIButton detailButton;
			MapViewController parent;

			public MapDelegate(MapViewController parent)
			{
				this.parent = parent;
			}
				
			public override void DidUpdateUserLocation (MKMapView mapView, MKUserLocation userLocation)
			{
				if (mapView.UserLocation != null && parent.shouldCenterOnLocation) {
					CLLocationCoordinate2D coords = mapView.UserLocation.Coordinate;
					MKCoordinateSpan span = new MKCoordinateSpan(MapHelper.MilesToLatitudeDegrees(0.2), MapHelper.MilesToLongitudeDegrees(0.2, coords.Latitude));
					mapView.Region = new MKCoordinateRegion(coords, span);
					parent.shouldCenterOnLocation = false;
				}
			}

			/// <summary>
			/// This is very much like the GetCell method on the table delegate
			/// </summary>
			public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, IMKAnnotation annotation)
			{
				if(ThisIsTheCurrentLocation(mapView, annotation))
				{
					return null;
				}

			
				// try and dequeue the annotation view
				MKAnnotationView annotationView = mapView.DequeueReusableAnnotation(annotationIdentifier);

				// if we couldn't dequeue one, create a new one
				if (annotationView == null)
					annotationView = new MKAnnotationView(annotation, annotationIdentifier);
				else // if we did dequeue one for reuse, assign the annotation to it
					annotationView.Annotation = annotation;

				// configure our annotation view properties
				annotationView.CanShowCallout = true;
				nfloat r;
				nfloat g;
				nfloat b;
				nfloat a;
				RandomColorHelper.GetRandomColor().GetRGBA(out r, out g, out b, out a);
				var rank = parent.GetRankForHouse ((annotation as HouseMapAnnotation).House);
		
				var fontSize = rank.Length >= 3 ? 9 : 15;

				annotationView.Image = LightMapPointStyleKit.ImageOfLightMapPoint ((float)r,(float)g,(float)b,(float)a,rank, fontSize);
				annotationView.Selected = true;

				// you can add an accessory view, in this case, we'll add a button on the right, and an image on the left
				detailButton = UIButton.FromType(UIButtonType.DetailDisclosure);
				detailButton.TintColor = UIColor.Black;

				detailButton.TouchUpInside += (s, e) => { 
					Console.WriteLine ("Clicked");
					var detailViewController = UIStoryboard.FromName ("MainStoryboard", null).InstantiateViewController("HouseDetailsViewController") as HouseDetailsViewController;
					detailViewController.Annotation = annotation as HouseMapAnnotation;
					mapView.DeselectAnnotation(annotation,true);
					this.parent.NavigationController.PushViewController(detailViewController,true);
				};
				annotationView.RightCalloutAccessoryView = detailButton;

				if (annotation.GetType () == typeof(HouseMapAnnotation))
					FetchImageAsync(annotationView,(annotation as HouseMapAnnotation).House);
				return annotationView;
			}

			private async void FetchImageAsync(MKAnnotationView annotationView, House house)
			{
				try {
					if (house.Thumbnail == null) return;

					UIImage thumbnail;

					//Check Cache First
					var houseImageCache = await parent.imageCacheRepo.GetHouseImagesFor(house.ObjectId);
					if (houseImageCache == null)
					{
						//If Not in Cache set cache;
						var byteArray = await new HttpClient ().GetByteArrayAsync (house.Thumbnail.Url);
						parent.imageCacheRepo.SaveHouseImages(new HouseImages(){ObjectId = house.ObjectId, Thumbnail= byteArray});
						thumbnail = UIImage.LoadFromData (NSData.FromArray (byteArray));

					}
					else
					{
						if (houseImageCache.Thumbnail != null)
						{
							thumbnail = UIImage.LoadFromData (NSData.FromArray (houseImageCache.Thumbnail));

						}
						else {
							var byteArray = await new HttpClient ().GetByteArrayAsync (house.Thumbnail.Url);
							houseImageCache.Thumbnail = byteArray;
							await parent.imageCacheRepo.UpdateHouseImages(houseImageCache);
							thumbnail = UIImage.LoadFromData (NSData.FromArray (byteArray));

						}
					}
					annotationView.LeftCalloutAccessoryView = new UIImageView(thumbnail);

				} catch (Exception e) {
					Console.WriteLine ("Error Retrieving Image. {0}", e.Message);
				}

			}

			private static bool ThisIsTheCurrentLocation(MKMapView mapView, IMKAnnotation annotation)
			{
				var userLocationAnnotation = ObjCRuntime.Runtime.GetNSObject(annotation.Handle) as MKUserLocation;
				if(userLocationAnnotation != null)
				{
					return userLocationAnnotation == mapView.UserLocation;
				}

				return false;
			}

		}
	}

}
