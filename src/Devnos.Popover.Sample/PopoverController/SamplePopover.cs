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
		
		public static Action PopoverDidDismiss { get; set; }
		
		public static UIViewController ContentController { 
			get { return _Popover.ContentViewController; }
			set { _Popover.ContentViewController = value; }
		}
		
		public static SizeF ContentSize { 
			get { return _Popover.ContentSize; }
			set { _Popover.ContentSize = value; }
		}
		
		public static void Initialize()
		{
			if(!IsInitialized) {
				_Popover = new PopoverController();
			}
		}
		
		public static void Initialize(PopoverContainerModel properties)
		{
			if(!IsInitialized) {
				_Popover = new PopoverController();
				_Popover.Properties = properties;
			}
		}
		
		public static void PresentFromRect(RectangleF rect, UIView inView, UIPopoverArrowDirection arrowDirection)
		{
			using(var pool = new NSAutoreleasePool()) {
			pool.BeginInvokeOnMainThread(()=> {
					_Popover.PresentPopover(rect, inView, arrowDirection, false);
				});
			}
		}
	}
}

