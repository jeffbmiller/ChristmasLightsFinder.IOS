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
	[Register ("HouseDetailsViewController")]
	partial class HouseDetailsViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIActivityIndicatorView activityIndicator { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel addressLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel cityProvLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView houseImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton likeButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel likesLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton navigateButton { get; set; }

		[Action ("likeButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void likeButton_TouchUpInside (UIButton sender);

		[Action ("navigateButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void navigateButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (activityIndicator != null) {
				activityIndicator.Dispose ();
				activityIndicator = null;
			}
			if (addressLabel != null) {
				addressLabel.Dispose ();
				addressLabel = null;
			}
			if (cityProvLabel != null) {
				cityProvLabel.Dispose ();
				cityProvLabel = null;
			}
			if (houseImage != null) {
				houseImage.Dispose ();
				houseImage = null;
			}
			if (likeButton != null) {
				likeButton.Dispose ();
				likeButton = null;
			}
			if (likesLabel != null) {
				likesLabel.Dispose ();
				likesLabel = null;
			}
			if (navigateButton != null) {
				navigateButton.Dispose ();
				navigateButton = null;
			}
		}
	}
}
