using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Parse;
using System.Net.Http;
using System.Collections.Generic;
using MapKit;

namespace ChristmasLightsFinder.IOS
{
	public partial class HouseDetailsViewController : UIViewController
	{
		public HouseDetailsViewController (IntPtr handle) : base (handle)
		{
			
		}

		public HouseMapAnnotation Annotation {get;set;}

		public async override void ViewDidLoad ()
		{
			this.activityIndicator.Hidden = true;
			base.ViewDidLoad ();
			this.Title = Annotation.House.Address;
			BindData ();

			if (Annotation.House.Image != null) {
				try {
					this.activityIndicator.Hidden = false;
					this.activityIndicator.StartAnimating ();
					var byteArray = await new HttpClient ().GetByteArrayAsync (Annotation.House.Image.Url);

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
			paramsDictionary.Add("objectId",Annotation.House.ObjectId);
			try {
				var result = await Parse.ParseCloud.CallFunctionAsync<int>("likeHouse",paramsDictionary);
				this.Annotation.House.Likes = result;
				Annotation.RefreshAnnotationView();
				BindData ();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		partial void navigateButton_TouchUpInside (UIButton sender)
		{
			var item = new MKMapItem(new MKPlacemark(Annotation.Coordinate,new MKPlacemarkAddress(){
				City = Annotation.House.City, 
				Street = Annotation.House.Address, 
				State = Annotation.House.Province,
				Country = Annotation.House.Country
			}));
					
			MKCoordinateRegion region = new MKCoordinateRegion(Annotation.Coordinate, new MKCoordinateSpan(MapHelper.MilesToLatitudeDegrees(2), MapHelper.MilesToLongitudeDegrees(2, Annotation.Coordinate.Latitude)));
			

			item.OpenInMaps(new MKLaunchOptions(){
				MapCenter = region.Center,
				MapSpan = region.Span
			});
		}

		private void BindData()
		{
			this.addressLabel.Text = Annotation.House.Address;
			this.cityProvLabel.Text = string.Format ("{0}, {1}", Annotation.House.City, Annotation.House.Province);
			this.likesLabel.Text = string.Format ("({0}) Likes", Annotation.House.Likes);
		}
	}
}
