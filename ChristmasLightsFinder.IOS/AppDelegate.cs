using Foundation;
using UIKit;
using Parse;

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

			ParseObject.RegisterSubclass<House>();
			ParseClient.Initialize("Sv0daWT1jgQ4pdSFvgbqkThjXRtZlFhUW47LMGqx", "LN6CeKOoC0SZtKmyHVizMGyzl80suzV7dgWkYsdw");

			// If you have defined a root view controller, set it here:
			Window.RootViewController = UIStoryboard.FromName ("MainStoryboard", null).InstantiateInitialViewController();

			// make the window visible
			Window.MakeKeyAndVisible ();

			return true;
		}

	}
}


