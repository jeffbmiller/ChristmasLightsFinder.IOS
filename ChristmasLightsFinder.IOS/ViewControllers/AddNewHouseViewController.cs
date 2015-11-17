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
				}

				locationManager.RequestWhenInUseAuthorization();
				locationManager.StartUpdatingLocation();

				locationManager.LocationsUpdated += async (object s, CLLocationsUpdatedEventArgs args) => {

					var geocoder = new CoreLocation.CLGeocoder ();
					var result = await geocoder.ReverseGeocodeLocationAsync(new CLLocation(locationManager.Location.Coordinate.Latitude, locationManager.Location.Coordinate.Longitude));

					addressTextField.Text = result[0].AddressDictionary.ValueForKey(ABPersonAddressKey.Street).ToString();
					cityTextField.Text =  result[0].AddressDictionary.ValueForKey(ABPersonAddressKey.City).ToString();
					provinceTextField.Text = result[0].AddressDictionary.ValueForKey(ABPersonAddressKey.State).ToString();
					locationManager.StopUpdatingLocation();
				};
			});

			var saveBtn = new UIBarButtonItem (UIBarButtonSystemItem.Save);
			saveBtn.Clicked += async delegate {

				DismissKeyboards();
				if (addressTextField.Text == null || cityTextField.Text == null && provinceTextField.Text == null)
				{
					new UIAlertView("Could Not Save","Please fill in all fields",null,"Close",null).Show();
					return;
				}

				var house = new House(){
					Address = addressTextField.Text,
					City = cityTextField.Text,
					Province = provinceTextField.Text,
					Country = "Canada",

					Music = musicSwitch.On,
					Animation = animationSwitch.On
				};

				if (locationManager.Location != null)
				{
					house.Longitude = locationManager.Location.Coordinate.Longitude;
					house.Latitude = locationManager.Location.Coordinate.Latitude;
				}

				//Save Parse File
				if (houseImageView.Image != null){
					try {
						var imageFile = new ParseFile(string.Format("{0}.jpg",house.FullAddress),houseImageView.Image.AsJPEG(0.2f).AsStream());
						await imageFile.SaveAsync();
						house.Image = imageFile;
					}
					catch (Exception e)
					{
						new UIAlertView("Error Saving Image",e.Message,null,"Close",null).Show();
						return;
					}
				}

				//Save House Parse object
				try {
					await house.SaveAsync();
				}
				catch (Exception e){
					
					new UIAlertView("Error Saving",e.Message,null,"Close",null).Show();
					return;
				}

				this.NavigationController.PopViewController(true);
			};

			this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ saveBtn, myLocationBtn };
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

			var sourceActionSheet = new UIActionSheet("Select Photo",null,"Cancel",null,new string []{"From Camera", "From Saved Photos"});

			sourceActionSheet.Dismissed += (object actionSheet, UIButtonEventArgs e) => {
				Console.WriteLine(e.ButtonIndex);
				if (e.ButtonIndex == sourceActionSheet.CancelButtonIndex) return;
				if (e.ButtonIndex == 0){
					//Camera
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
