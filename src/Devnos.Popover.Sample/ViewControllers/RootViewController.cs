using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace Devnos.Popover.Sample
{
	public class RootViewController : UIViewController
	{
		UIButton _RootButton;
		ImageContentController _ImageController;
		
		public RootViewController()
		{
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.View.BackgroundColor = UIColor.Blue;
			
			_ImageController = new ImageContentController();
			
			_RootButton = UIButton.FromType(UIButtonType.RoundedRect);
			_RootButton.Frame = new RectangleF(this.View.Center, new SizeF(200, 50));
			_RootButton.SetTitle("Click Me!", UIControlState.Normal);
			_RootButton.TouchUpInside += (sender, e) => {
				
				SamplePopover.ContentSize = new SizeF(600, 450);
//				SamplePopover.ContentController = _ImageController;
				SamplePopover.ContentController = new UIViewController() { ContentSizeForViewInPopover = SamplePopover.ContentSize };
				SamplePopover.PresentFromRect(_RootButton.Frame, this.View, UIPopoverArrowDirection.Right);
				
			};
			this.View.AddSubview(_RootButton);
		}
		
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}

