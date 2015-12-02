using Foundation;
using UIKit;
using Parse;
using System;
using Xamarin;

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

			ParseObject.RegisterSubclass<House>();
			ParseClient.Initialize("Sv0daWT1jgQ4pdSFvgbqkThjXRtZlFhUW47LMGqx", "LN6CeKOoC0SZtKmyHVizMGyzl80suzV7dgWkYsdw");

			// Register for Push Notitications
//			UIUserNotificationType notificationTypes = (UIUserNotificationType.Alert |
//				UIUserNotificationType.Badge |
//				UIUserNotificationType.Sound);
//			var settings = UIUserNotificationSettings.GetSettingsForTypes(notificationTypes,
//				new NSSet(new string[] { }));
//			UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
//			UIApplication.SharedApplication.RegisterForRemoteNotifications();
//
//			// Handle Push Notifications
//			ParsePush.ParsePushNotificationReceived += (object sender, ParsePushNotificationEventArgs args) => {
//				// Process Push Notification payload here.
//				Console.WriteLine("Push Received");
//			};
//
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


