// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ChristmasLightsFinder.IOS
{
	[Register ("MapViewController")]
	partial class MapViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem addNewBarButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		MapKit.MKMapView mapView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem trackCurrentLocationBtn { get; set; }

		[Action ("trackCurrentLocationBtn_Activated:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void trackCurrentLocationBtn_Activated (UIBarButtonItem sender);

		void ReleaseDesignerOutlets ()
		{
			if (addNewBarButton != null) {
				addNewBarButton.Dispose ();
				addNewBarButton = null;
			}
			if (mapView != null) {
				mapView.Dispose ();
				mapView = null;
			}
			if (trackCurrentLocationBtn != null) {
				trackCurrentLocationBtn.Dispose ();
				trackCurrentLocationBtn = null;
			}
		}
	}
}
