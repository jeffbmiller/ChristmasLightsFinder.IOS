using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Parse;
using System.Net.Http;

namespace ChristmasLightsFinder.IOS
{
	public partial class HouseDetailsViewController : UIViewController
	{
		public HouseDetailsViewController (IntPtr handle) : base (handle)
		{
			
		}

		public House House {get;set;}

		public async override void ViewDidLoad ()
		{
			this.activityIndicator.Hidden = true;
			base.ViewDidLoad ();
			this.Title = House.Address;
			this.addressLabel.Text = House.Address;

			if (House.Image != null) {
				try {
					this.activityIndicator.Hidden = false;
					this.activityIndicator.StartAnimating ();
					var byteArray = await new HttpClient ().GetByteArrayAsync (House.Image.Url);

					var image = UIImage.LoadFromData (NSData.FromArray (byteArray));
					this.houseImage.Image = image;

				} catch (Exception e) {
					Console.WriteLine ("Error Retrieving Image. {0}", e.Message);
				} finally {
					this.activityIndicator.StopAnimating ();
					this.activityIndicator.Hidden = true;
				}
			}
		}
	}
}
