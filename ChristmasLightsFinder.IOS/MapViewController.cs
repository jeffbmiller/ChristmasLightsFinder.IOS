using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using MapKit;
using CoreLocation;

namespace ChristmasLightsFinder.IOS
{
	partial class MapViewController : UIViewController
	{
		private CLGeocoder geocoder;

		public MapViewController (IntPtr handle) : base (handle)
		{
			geocoder = new CoreLocation.CLGeocoder ();
		}

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var result = await geocoder.GeocodeAddressAsync ("3514 Rosser Ave Brandon MB");

			var location = result [0];

			var anno = new MKPointAnnotation () {
				Coordinate = location.Location.Coordinate
			};

			this.mapView.AddAnnotation (anno);

			MKMapRect zoomRect = new MKMapRect ();
			foreach (var a in this.mapView.Annotations) {
				var apoint = MKMapPoint.FromCoordinate (a.Coordinate);
				var pointRect = new MKMapRect (apoint.X, apoint.Y, 0.1, 0.1);
				zoomRect = MKMapRect.Union (zoomRect, pointRect);
			}

			this.mapView.SetVisibleMapRect (zoomRect, true);
		}
	}
}
