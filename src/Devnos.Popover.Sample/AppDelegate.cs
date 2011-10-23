using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Devnos.Popover.Sample
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow _Window;
		RootViewController _Root;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			_Window = new UIWindow(UIScreen.MainScreen.Bounds);
			
			//Initialize Singleton....Once.
			SamplePopover.Initialize();
			
			_Root = new RootViewController();
			
			_Window.RootViewController = _Root;
			_Window.MakeKeyAndVisible();
			return true;
		}
	}
}

