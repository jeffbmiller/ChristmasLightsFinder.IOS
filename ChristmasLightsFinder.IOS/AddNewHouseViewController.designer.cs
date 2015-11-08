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
		UIImageView houseImageView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (houseImageView != null) {
				houseImageView.Dispose ();
				houseImageView = null;
			}
		}
	}
}
