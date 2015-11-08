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
		}

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
//
//
//
//			var result = await geocoder.GeocodeAddressAsync ("3514 Rosser Ave Brandon MB");
//
//			var location = result [0];
//
//			var anno = new MKPointAnnotation () {
//				Coordinate = location.Location.Coordinate
//			};
//
//			this.mapView.AddAnnotation (anno);
//
//			MKMapRect zoomRect = new MKMapRect ();
//			foreach (var a in this.mapView.Annotations) {
//				var apoint = MKMapPoint.FromCoordinate (a.Coordinate);
//				var pointRect = new MKMapRect (apoint.X, apoint.Y, 0.1, 0.1);
//				zoomRect = MKMapRect.Union (zoomRect, pointRect);
//			}
//
//			this.mapView.SetVisibleMapRect (zoomRect, true);
		}

		public override async void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			var houses = await houseService.GetHousesAsync ();

			foreach (var house in houses) {
				var geoAddress = await geocoder.GeocodeAddressAsync (house.FullAddress);

				var annotation = new MKPointAnnotation (){
					Coordinate = geoAddress[0].Location.Coordinate
				};

				var existing = this.mapView.Annotations.ToList ();
				if (existing.Any (x => x.Coordinate.Latitude == annotation.Coordinate.Latitude &&
					x.Coordinate.Longitude == annotation.Coordinate.Longitude))
					continue;
				this.mapView.AddAnnotation (annotation);
			}
		}
	}
}
