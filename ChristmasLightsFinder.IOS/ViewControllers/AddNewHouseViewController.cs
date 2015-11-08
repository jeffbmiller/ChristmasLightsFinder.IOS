using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

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
				addPhoto ();
			}));

			var saveBtn = new UIBarButtonItem (UIBarButtonSystemItem.Save);
			saveBtn.Clicked += async delegate {
				var house = new House(){
					Address = addressTextField.Text,
					City = cityTextField.Text,
					Province = provinceTextField.Text,
					Country = "Canada"
				};

				await house.SaveAsync();
				this.NavigationController.PopViewController(true);
			};

			this.NavigationItem.RightBarButtonItem = saveBtn;
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
