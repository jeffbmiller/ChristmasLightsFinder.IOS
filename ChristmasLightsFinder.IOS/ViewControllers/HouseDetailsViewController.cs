using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Parse;
using System.Net.Http;
using System.Collections.Generic;
using MapKit;
using System.IO;
using System.Linq;

namespace ChristmasLightsFinder.IOS
{
	public partial class HouseDetailsViewController : UIViewController
	{
		private List<string> housesLiked;
		private string housesLikeFilename;
		private readonly HouseImageCacheRepository imageCacheRepo;

		public HouseDetailsViewController (IntPtr handle) : base (handle)
		{
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			var library = Path.Combine (documents, "..", "Library");
			housesLikeFilename = Path.Combine (library, "HousesLiked.txt");
			if (File.Exists (housesLikeFilename))
				housesLiked = File.ReadAllText (housesLikeFilename).Split (';').ToList ();
			else
				housesLiked = new List<string>();

            //imageCacheRepo = new HouseImageCacheRepository ();

		}

		public HouseMapAnnotation Annotation {get;set;}

		public bool CanLike {get {return !housesLiked.Contains (Annotation.House.Id); }}

		public async override void ViewDidLoad ()
		{
			this.activityIndicator.Hidden = true;
			base.ViewDidLoad ();
			this.Title = Annotation.House.Address;

			BindData ();


			this.activityIndicator.Hidden = false;
			this.activityIndicator.StartAnimating ();
			var imageRef = Firebase.Storage.Storage.DefaultInstance.GetReferenceFromUrl(Annotation.House.ImagePath);

			// Download in memory with a maximum allowed size of 1MB (1 * 1024 * 1024 bytes)
			imageRef.GetData(1 * 1024 * 1024, (data, error) =>
			{
                this.activityIndicator.Hidden = true;
				this.activityIndicator.StopAnimating();
				if (error != null)
				{
					// Uh-oh, an error occurred!

				}
				else
				{
					// Data for "images/island.jpg" is returned
					var image = UIImage.LoadFromData(data);
                    this.houseImage.Image = image;
				}

			});

			
		}

		async partial void likeButton_TouchUpInside (UIButton sender)
		{

			
			var paramsDictionary = new Dictionary<string, object>();
            paramsDictionary.Add("objectId",Annotation.House.Id);
			try {
				var result = await Parse.ParseCloud.CallFunctionAsync<int>("likeHouse",paramsDictionary);
				this.Annotation.House.Likes = result;
				Annotation.RefreshAnnotationView();

				//Update local cache to reflect house was liked
				housesLiked.Add(Annotation.House.Id);
				var houseLikedString = string.Join(";",housesLiked);
				File.WriteAllText(housesLikeFilename, houseLikedString );

				BindData ();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

//		partial void navigateButton_TouchUpInside (UIButton sender)
//		{
//			var item = new MKMapItem(new MKPlacemark(Annotation.Coordinate,new MKPlacemarkAddress(){
//				City = Annotation.House.City, 
//				Street = Annotation.House.Address, 
//				State = Annotation.House.Province,
//				Country = Annotation.House.Country
//			}));
//					
//			MKCoordinateRegion region = new MKCoordinateRegion(Annotation.Coordinate, new MKCoordinateSpan(MapHelper.MilesToLatitudeDegrees(2), MapHelper.MilesToLongitudeDegrees(2, Annotation.Coordinate.Latitude)));
//			
//
//			item.OpenInMaps(new MKLaunchOptions(){
//				MapCenter = region.Center,
//				MapSpan = region.Span
//			});
//		}

		private void BindData()
		{
			this.dateAddedLabel.Text = string.Format("Added: {0}", Annotation.House.CreatedAt.ToLocalTime().ToShortDateString ());
			this.addressLabel.Text = Annotation.House.Address;
			this.cityProvLabel.Text = string.Format ("{0}, {1}", Annotation.House.City, Annotation.House.Province);
			this.likesLabel.Text = string.Format ("({0}) Likes", Annotation.House.Likes);
			this.likeButton.Enabled = CanLike;
			this.likeButton.BackgroundColor = CanLike ? UIColor.Red : UIColor.Gray;
		}
	}
}
