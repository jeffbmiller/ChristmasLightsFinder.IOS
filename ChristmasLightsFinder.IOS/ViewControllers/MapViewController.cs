using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using MapKit;
using CoreLocation;
using System.Linq;

namespace ChristmasLightsFinder.IOS
{
	partial class MapViewController : UIViewController
	{
		private CLGeocoder geocoder;
		private readonly HouseService houseService;

		public MapViewController (IntPtr handle) : base (handle)
		{
			geocoder = new CoreLocation.CLGeocoder ();
			houseService = new HouseService ();

			this.NavigationItem.BackBarButtonItem = new UIBarButtonItem ("Back", UIBarButtonItemStyle.Plain,null);
		}

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.mapView.Delegate = new MapDelegate (this);
			Center ();
		}

		private void Center()
		{
			var coords = new CLLocationCoordinate2D(49.8422096,-99.940394);
			var span = new MKCoordinateSpan(0.5, 0.5);
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
					annotationView = new MKPinAnnotationView(annotation, annotationIdentifier);
				else // if we did dequeue one for reuse, assign the annotation to it
					annotationView.Annotation = annotation;

				// configure our annotation view properties
				annotationView.CanShowCallout = true;
				(annotationView as MKPinAnnotationView).AnimatesDrop = true;
				(annotationView as MKPinAnnotationView).PinColor = MKPinAnnotationColor.Green;
				annotationView.Selected = true;

				// you can add an accessory view, in this case, we'll add a button on the right, and an image on the left
				detailButton = UIButton.FromType(UIButtonType.DetailDisclosure);

				detailButton.TouchUpInside += (s, e) => { 
					Console.WriteLine ("Clicked");
					var detailViewController = UIStoryboard.FromName ("MainStoryboard", null).InstantiateViewController("HouseDetailsViewController") as HouseDetailsViewController;
					detailViewController.House = (annotation as HouseMapAnnotation).House;
					this.parent.NavigationController.PushViewController(detailViewController,true);
				};
				annotationView.RightCalloutAccessoryView = detailButton;

//				annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromBundle("29_icon.png"));

				return annotationView;
			}

			// as an optimization, you should override this method to add or remove annotations as the 
			// map zooms in or out.
			public override void RegionChanged (MKMapView mapView, bool animated) {}
		}
	}

}
