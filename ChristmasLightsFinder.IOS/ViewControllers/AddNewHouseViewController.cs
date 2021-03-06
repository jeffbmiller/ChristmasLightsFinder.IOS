using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Parse;
using CoreLocation;
using AddressBook;

namespace ChristmasLightsFinder.IOS
{
	partial class AddNewHouseViewController : UITableViewController
	{
		CLLocationManager locationManager;

		public AddNewHouseViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.houseImageView.AddGestureRecognizer (new UITapGestureRecognizer (x => {
				DismissKeyboards();
				addPhoto ();
			}));

			var myLocationBtn = new UIBarButtonItem (UIImage.FromFile ("ic_my_location_white.png"), UIBarButtonItemStyle.Plain, (object sender, EventArgs e) => {
				if (locationManager == null)
				{
					// Set a movement threshold for new events.
					locationManager = new CLLocationManager();
					locationManager.DistanceFilter = 500f;
					locationManager.LocationsUpdated += async (object s, CLLocationsUpdatedEventArgs args) =>
					{

						var geocoder = new CoreLocation.CLGeocoder();
						try
						{
							var result = await geocoder.ReverseGeocodeLocationAsync(new CLLocation(locationManager.Location.Coordinate.Latitude, locationManager.Location.Coordinate.Longitude));

							addressTextField.Text = result[0].AddressDictionary.ValueForKey(ABPersonAddressKey.Street).ToString();
							cityTextField.Text = result[0].AddressDictionary.ValueForKey(ABPersonAddressKey.City).ToString();
							provinceTextField.Text = result[0].AddressDictionary.ValueForKey(ABPersonAddressKey.State).ToString();
							locationManager.StopUpdatingLocation();

						}
						catch (Exception ex)
						{
							new UIAlertView("Error", "Unable to determine current location.", null, "OK", null).Show();
							locationManager.StopUpdatingLocation();
						}
					};
				}

				if (CLLocationManager.Status == CLAuthorizationStatus.NotDetermined){
					locationManager.RequestWhenInUseAuthorization();
					locationManager.StartUpdatingLocation();
					return;
				}

				if (CLLocationManager.Status != CLAuthorizationStatus.AuthorizedWhenInUse){

					new UIAlertView("Turn On Location Services For \"Christmas Lights Finder\" To Determine Your Locaiton.","Go to Settings -> Location Services -> Christmas Lights Finder to turn on.",null,"Close",null).Show();
				}
				else {
					locationManager.StartUpdatingLocation();
				}
				
			});

			var saveBtn = new UIBarButtonItem (UIBarButtonSystemItem.Save);
			saveBtn.Clicked += delegate {

				DismissKeyboards();
				if (addressTextField.Text == null || cityTextField.Text == null && provinceTextField.Text == null)
				{
					new UIAlertView("Could Not Save","Please fill in all fields",null,"Close",null).Show();
					return;
				}

               uploadImage();
				
			};

			this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ saveBtn, myLocationBtn };
		}

        private void uploadImage()
        {
			var housesRef = Firebase.Database.Database.DefaultInstance.GetRootReference().GetChild("Houses").GetChildByAutoId();

			var imagesNode = Firebase.Storage.Storage.DefaultInstance.GetRootReference().GetChild($"images\\{housesRef.Key}\\main_image.jpg");

			BigTed.BTProgressHUD.Show("Saving...");
			// Upload the file to the path "images/rivers.jpg"
			imagesNode.PutData(NSData.FromStream(houseImageView.Image.AsJPEG(0.2f).AsStream()), null, (metadata, storageError) =>
			{
				if (storageError != null)
				{
						// Uh-oh, an error occurred!
						BigTed.BTProgressHUD.Dismiss();
				}
				else
				{
					saveHouse(housesRef, metadata.DownloadUrl.AbsoluteString);
				}
			});
        }

        private void saveHouse(Firebase.Database.DatabaseReference housesRef, string imagePath){

            var house = new House()
            {
                Address = addressTextField.Text,
                City = cityTextField.Text,
                Province = provinceTextField.Text,
                ImagePath = imagePath
            };

            if (locationManager != null && locationManager.Location != null)
            {
                house.Longitude = locationManager.Location.Coordinate.Longitude;
                house.Latitude = locationManager.Location.Coordinate.Latitude;
            }

            object[] keys = { "Address", "City", "Province", "ImagePath", "Longitude", "Latitude" };
            object[] values = { house.Address, house.City, house.Province, house.ImagePath ?? "", house.Longitude, house.Latitude };
            var data = NSDictionary.FromObjectsAndKeys(values, keys, keys.Length);

            
            housesRef.SetValue<NSDictionary>(data, (error, reference) =>
            {
                BigTed.BTProgressHUD.Dismiss();
                if (error != null)
                {
                    new UIAlertView("Error Saving Image", error.ToString(), null, "Close", null).Show();
                    return;
                }
            this.NavigationController.PopViewController(true);
            });

        }

		private void DismissKeyboards()
		{
			addressTextField.ResignFirstResponder ();
			cityTextField.ResignFirstResponder ();
			provinceTextField.ResignFirstResponder ();
		}

		private void addPhoto()
		{
			
			var imagePicker = new UIImagePickerController();
			imagePicker.FinishedPickingMedia += (object ip, UIImagePickerMediaPickedEventArgs e) => {

				this.houseImageView.Image = e.OriginalImage;

				imagePicker.DismissViewController(true,null);
			};

			imagePicker.Canceled += (object sender, EventArgs e) => {
				imagePicker.DismissViewController(true,null);
			};

			var sourceActionSheet = new UIActionSheet("Select Photo",null,"Cancel",null,new string []{"From Camera", "From Saved Photos"});

			sourceActionSheet.Dismissed += (object actionSheet, UIButtonEventArgs e) => {
				Console.WriteLine(e.ButtonIndex);
				if (e.ButtonIndex == sourceActionSheet.CancelButtonIndex) return;
				if (e.ButtonIndex == 0){
					//Camera
					imagePicker.SourceType = UIImagePickerControllerSourceType.Camera;
					imagePicker.CameraFlashMode = UIImagePickerControllerCameraFlashMode.Off;
					this.PresentViewController(imagePicker,true,null);
				}
				if (e.ButtonIndex == 1) {
					//Photo Library
					imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;

					this.PresentViewController(imagePicker,true,null);
				}

			};


			sourceActionSheet.ShowInView(this.View);

		}
	}
}
