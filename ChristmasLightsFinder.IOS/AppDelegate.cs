using Foundation;
using UIKit;
using Parse;
using System;

namespace ChristmasLightsFinder.IOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations

		public override UIWindow Window {
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			// create a new window instance based on the screen size
			Window = new UIWindow (UIScreen.MainScreen.Bounds);

			UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes {
				TextColor = UIColor.White
			});
//			UIBarButtonItem.Appearance.TintColor = UIColor.White;
//			UINavigationBar.Appearance.TintColor = UIColor.White;
			UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, false);

			ParseObject.RegisterSubclass<House>();
			ParseClient.Initialize("Sv0daWT1jgQ4pdSFvgbqkThjXRtZlFhUW47LMGqx", "LN6CeKOoC0SZtKmyHVizMGyzl80suzV7dgWkYsdw");

			// If you have defined a root view controller, set it here:
			Window.RootViewController = UIStoryboard.FromName ("MainStoryboard", null).InstantiateInitialViewController();

			// make the window visible
			Window.MakeKeyAndVisible ();

			return true;
		}

		public override void WillEnterForeground (UIApplication application)
		{
			NSNotificationCenter.DefaultCenter.PostNotificationName("ReloadMap", this);
		}

	}
}


