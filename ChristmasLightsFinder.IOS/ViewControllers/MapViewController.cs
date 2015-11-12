using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using MapKit;
using CoreLocation;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace ChristmasLightsFinder.IOS
{
	partial class MapViewController : UIViewController
	{
		private CLGeocoder geocoder;
		private readonly HouseService houseService;
		private bool isAdmin;

		public MapViewController (IntPtr handle) : base (handle)
		{
			geocoder = new CoreLocation.CLGeocoder ();
			houseService = new HouseService ();

			this.NavigationItem.BackBarButtonItem = new UIBarButtonItem ("Back", UIBarButtonItemStyle.Plain,null);
			isAdmin =  NSBundle.MainBundle.ObjectForInfoDictionary("AllowAddNew").ToString() == "1";
		}

		public async override void ViewDidLoad ()
		{
			// If Not Admin Remove Add button
			if (!isAdmin) {
				this.NavigationItem.RightBarButtonItem = null;
			}
			base.ViewDidLoad ();
			this.mapView.Delegate = new MapDelegate (this);
//			this.filterBarButton.Clicked += (object sender, EventArgs e) => {
//
//				var actionSheet = new UIActionSheet("Filter By",null,"Cancel",null,new string[]{"All", "With Music", "With Animation"});
//
//				actionSheet.Dismissed += (object aSheet, UIButtonEventArgs args) => {
//					if (args.ButtonIndex == actionSheet.CancelButtonIndex)
//						return;
//					
//				};
//
//				actionSheet.ShowInView(this.View);
//			};

			Center ();
		}

		private void Center()
		{
			var coords = new CLLocationCoordinate2D(49.8423203,-99.9584185);
			var span = new MKCoordinateSpan(0.1, 0.1);
			mapView.Region = new MKCoordinateRegion(coords, span);
		}

		public override async void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			try {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
				var houses = await houseService.GetHousesAsync ();
				foreach (var house in houses) {
					var geoAddress = await geocoder.GeocodeAddressAsync (house.FullAddress);

					var annotation = new HouseMapAnnotation (geoAddress [0].Location.Coordinate, house.Address, house);
				
					var existing = this.mapView.Annotations.ToList ();
					if (existing.Any (x => x.Coordinate.Latitude == annotation.Coordinate.Latitude &&
						x.Coordinate.Longitude == annotation.Coordinate.Longitude))
						continue;
					this.mapView.AddAnnotation (annotation);
				}
			}
			catch(Exception e) {
				Console.WriteLine ("Error communicating with server. {0}",e.Message);
			}
			finally{
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			}
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

			/// <summary>
			/// This is very much like the GetCell method on the table delegate
			/// </summary>
			public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, IMKAnnotation annotation)
			{
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
				annotationView.Image = LightMapPointStyleKit.ImageOfLightMapPoint ((float)r,(float)g,(float)b,(float)a);
				annotationView.Selected = true;

				// you can add an accessory view, in this case, we'll add a button on the right, and an image on the left
				detailButton = UIButton.FromType(UIButtonType.DetailDisclosure);

				detailButton.TouchUpInside += (s, e) => { 
					Console.WriteLine ("Clicked");
					var detailViewController = UIStoryboard.FromName ("MainStoryboard", null).InstantiateViewController("HouseDetailsViewController") as HouseDetailsViewController;
					detailViewController.Annotation = annotation as HouseMapAnnotation;
					mapView.DeselectAnnotation(annotation,true);
					this.parent.NavigationController.PushViewController(detailViewController,true);
				};
				annotationView.RightCalloutAccessoryView = detailButton;

				FetchImageAsync(annotationView,(annotation as HouseMapAnnotation).House);
				return annotationView;
			}

			private async void FetchImageAsync(MKAnnotationView annotationView, House house)
			{
				try {
					if (house.Thumbnail == null) return;
					var byteArray = await new HttpClient ().GetByteArrayAsync (house.Thumbnail.Url);

					var image = UIImage.LoadFromData (NSData.FromArray (byteArray));
					annotationView.LeftCalloutAccessoryView = new UIImageView(image);

				} catch (Exception e) {
					Console.WriteLine ("Error Retrieving Image. {0}", e.Message);
				}

			}

			// as an optimization, you should override this method to add or remove annotations as the 
			// map zooms in or out.
			public override void RegionChanged (MKMapView mapView, bool animated) {}
		}
	}

}
