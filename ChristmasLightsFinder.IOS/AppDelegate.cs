using Foundation;
using UIKit;
using System;
using Xamarin;
using Firebase.Analytics;

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
			#if DEBUG
			Insights.Initialize(Insights.DebugModeKey);
			#else
			Insights.Initialize("7fb2de5e612b5ddda616a9ed6a87022039c84805");
			#endif

			// create a new window instance based on the screen size
			Window = new UIWindow (UIScreen.MainScreen.Bounds);

			UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes {
				TextColor = UIColor.White
			});
			UIBarButtonItem.Appearance.TintColor = UIColor.White;
			UINavigationBar.Appearance.TintColor = UIColor.White;
			UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, false);

            App.Configure();
            Firebase.Database.Database.DefaultInstance.PersistenceEnabled = true;

			Window.RootViewController = UIStoryboard.FromName ("MainStoryboard", null).InstantiateInitialViewController();

			// make the window visible
			Window.MakeKeyAndVisible ();

			return true;
		}

		public override void WillEnterForeground (UIApplication application)
		{
			NSNotificationCenter.DefaultCenter.PostNotificationName("ReloadMap", this);
		}

//
//		public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings) {
//			application.RegisterForRemoteNotifications();
//		}
//
//		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken) {
//			ParseInstallation installation = ParseInstallation.CurrentInstallation;
//			installation.SetDeviceTokenFromData(deviceToken);
//
//			installation.SaveAsync();
//		}
//
//		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo) {
//			// We need this to fire userInfo into ParsePushNotificationReceived.
//			ParsePush.HandlePush(userInfo);
//		}

	}
}


