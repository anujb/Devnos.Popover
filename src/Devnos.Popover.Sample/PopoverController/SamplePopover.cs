using System;
using Devnos.Popover;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Devnos.Popover.Sample
{
	public class SamplePopover
	{
		static bool IsInitialized = false;
		static PopoverController _Popover;
		
		public static UIViewController ContentController { 
			get { return _Popover.ContentViewController; }
			set { _Popover.ContentViewController = value; }
		}
		
		public static SizeF ContentSize { 
			get { return _Popover.ContentSize; }
			set { _Popover.ContentSize = value; }
		}
		
		public static Func<IPopoverController, bool> ShouldDismissAction {
			get { return _Popover.ShouldDismiss; }
			set { _Popover.ShouldDismiss = value; }
		}
		
		public static Action<IPopoverController> PopoverDidDismissAction {
			get { return _Popover.DidDismiss; }
			set { _Popover.DidDismiss = value; }
		}
		
		public static void Initialize()
		{
			if(!IsInitialized) {
				_Popover = new PopoverController();
				_Popover.ShouldDismiss = (controller) => { return true; };
			}
		}
		
		public static void Initialize(PopoverContainerModel properties)
		{
			if(!IsInitialized) {
				_Popover = new PopoverController();
				_Popover.ShouldDismiss = (controller) => { return true; };
				_Popover.Properties = properties;
			}
		}
		
		public static void PresentFromRect(RectangleF rect, UIView inView, UIPopoverArrowDirection arrowDirection)
		{
			if(_Popover.ShouldDismiss == null) {
				_Popover.ShouldDismiss += (controller) => { return true; };
			}
			
			if(_Popover.DidDismiss == null) {
				_Popover.DidDismiss += (controller) => { Console.WriteLine("Popover Did Dismiss"); };
			}
			
			using(var pool = new NSAutoreleasePool()) {
			pool.BeginInvokeOnMainThread(()=> {
					_Popover.PresentPopover(rect, inView, arrowDirection, true);
				});
			}
		}
	}
}

