using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Parse;

namespace ChristmasLightsFinder.IOS
{
	partial class AddNewHouseViewController : UITableViewController
	{
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

			this.NavigationItem.RightBarButtonItem = saveBtn;
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
