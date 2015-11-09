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
	[Register ("AddNewHouseViewController")]
	partial class AddNewHouseViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField addressTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch animationSwitch { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField cityTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView houseImageView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch musicSwitch { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField provinceTextField { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (addressTextField != null) {
				addressTextField.Dispose ();
				addressTextField = null;
			}
			if (animationSwitch != null) {
				animationSwitch.Dispose ();
				animationSwitch = null;
			}
			if (cityTextField != null) {
				cityTextField.Dispose ();
				cityTextField = null;
			}
			if (houseImageView != null) {
				houseImageView.Dispose ();
				houseImageView = null;
			}
			if (musicSwitch != null) {
				musicSwitch.Dispose ();
				musicSwitch = null;
			}
			if (provinceTextField != null) {
				provinceTextField.Dispose ();
				provinceTextField = null;
			}
		}
	}
}
