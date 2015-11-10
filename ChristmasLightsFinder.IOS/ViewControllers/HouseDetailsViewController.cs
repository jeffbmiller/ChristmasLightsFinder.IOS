using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Parse;
using System.Net.Http;
using System.Collections.Generic;

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
			BindData ();

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

		async partial void likeButton_TouchUpInside (UIButton sender)
		{
			var paramsDictionary = new Dictionary<string, object>();
			paramsDictionary.Add("objectId",House.ObjectId);
			try {
				var result = await Parse.ParseCloud.CallFunctionAsync<House>("likeHouse",paramsDictionary);
				this.House = result;
				BindData ();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private void BindData()
		{
			this.addressLabel.Text = House.Address;
			this.likesLabel.Text = string.Format ("({0}) Likes", House.Likes);
		}
	}
}
