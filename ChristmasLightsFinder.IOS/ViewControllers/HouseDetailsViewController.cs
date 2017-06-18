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

            var rootNode = Firebase.Storage.Storage.DefaultInstance.GetRootReference();
			// Create a reference to the file you want to download
            var imagesNode = rootNode.GetChild($"images/{Annotation.House.Id}.jpg");

			// Download in memory with a maximum allowed size of 1MB (1 * 1024 * 1024 bytes)
			imagesNode.GetData(1 * 1024 * 1024, (data, error) =>
			{
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

			//if (Annotation.House.ImagePath != null) {
				//try {
					//this.activityIndicator.Hidden = false;
					//this.activityIndicator.StartAnimating ();


					//UIImage image;

				//	//Check Cache First
				//	var houseImageCache = await imageCacheRepo.GetHouseImagesFor(Annotation.House.Id);
				//	if (houseImageCache == null)
				//	{
				//		//If Not in Cache set cache;
				//		var byteArray = await new HttpClient ().GetByteArrayAsync (Annotation.House.Image.Url);
				//		await imageCacheRepo.SaveHouseImages(new HouseImages(){ObjectId = Annotation.House.ObjectId, Image= byteArray});
				//		image = UIImage.LoadFromData (NSData.FromArray (byteArray));

				//	}
				//	else
				//	{
				//		if (houseImageCache.Image != null)
				//		{
				//			image = UIImage.LoadFromData (NSData.FromArray (houseImageCache.Image));

				//		}
				//		else {
				//			var byteArray = await new HttpClient ().GetByteArrayAsync (Annotation.House.Image.Url);
				//			houseImageCache.Image = byteArray;
				//			await imageCacheRepo.UpdateHouseImages(houseImageCache);
				//			image = UIImage.LoadFromData (NSData.FromArray (byteArray));

				//		}
				//	}

				//	this.houseImage.Image = image;

				//} catch (Exception e) {
				//	Console.WriteLine ("Error Retrieving Image. {0}", e.Message);
				//} finally {
				//	this.activityIndicator.StopAnimating ();
				//	this.activityIndicator.Hidden = true;
				//}
			//}
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
