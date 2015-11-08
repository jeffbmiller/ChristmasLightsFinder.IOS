using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ChristmasLightsFinder.IOS
{
	public partial class HouseDetailsViewController : UIViewController
	{
		public HouseDetailsViewController (IntPtr handle) : base (handle)
		{
			
		}

		public House House {get;set;}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Title = House.Address;

		}
	}
}
