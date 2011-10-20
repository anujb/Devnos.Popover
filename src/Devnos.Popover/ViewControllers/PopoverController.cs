using System;
using System.Linq;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using System.Collections.Generic;

namespace Devnos.Popover
{
	public class PopoverController : NSObject, IPopoverController
	{
		public static float FadeDuration = 0.25f;
		
		public UIViewController ContentViewController { get; set; }
		public TouchableView BackgroundView { get; set; }
		
		public bool IsPopoverVisible { get; private set; }
		public UIPopoverArrowDirection ArrowDirection { get; private set; }
		public SizeF ContentSize { get; set; }
		public object Context { get; set; }
		
		IEnumerable<UIView> _PassthroughViews;
		public IEnumerable<UIView> PassthroughViews 
		{
			get { return _PassthroughViews; }
			set {
				_PassthroughViews = value;
				UpdateBackgroundPassthroughViews();
			}
		}
			
		
		public Action<IPopoverController> DidDismiss { get; set; }
		public Func<IPopoverController, bool> ShouldDismiss { get; set; }
		
		UIView View;
		PopoverContainerModel Properties;
		
		public PopoverController()
			: base()
		{
			
		}
		
		public PopoverController(UIViewController contentViewController)
			: base()
		{
			this.ContentViewController = contentViewController;
		}
		
		public void PresentPopover(RectangleF rect, UIView inView, UIPopoverArrowDirection arrowDirection, bool animated)
		{
			this.DismissPopover(false);
			
			//obj-C selector calls [vc getView] which calls loadView
			if(this.ContentViewController.View == null) { }
			
			if(this.ContentSize == SizeF.Empty) {
				this.ContentSize = this.ContentViewController.ContentSizeForViewInPopover;	
			}
			
			var displayArea = CalculateDisplayArea(inView);
			var properties = this.Properties ?? DefaultContainerModel;
			var containerView = new PopoverContainerView(this.ContentSize, rect, displayArea, arrowDirection, properties);
			this.ArrowDirection = containerView.PopoverArrowDirection;
			
			var keyView = this.GetKeyView();
			
			BackgroundView = new TouchableView() { Frame = keyView.Bounds };
			BackgroundView.ContentMode = UIViewContentMode.ScaleToFill;
			BackgroundView.BackgroundColor = UIColor.Clear;
			BackgroundView.ViewWasTouched = this.ViewWasTouched;
			BackgroundView.AutoresizingMask = 
					UIViewAutoresizing.FlexibleLeftMargin |
					UIViewAutoresizing.FlexibleWidth |
					UIViewAutoresizing.FlexibleRightMargin |
					UIViewAutoresizing.FlexibleTopMargin |
					UIViewAutoresizing.FlexibleHeight |
					UIViewAutoresizing.FlexibleBottomMargin;
			
			keyView.AddSubview(containerView);
			containerView.ContentView = ContentViewController.View;
			containerView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
			
			this.View = containerView;
			this.UpdateBackgroundPassthroughViews();
			this.ContentViewController.ViewWillAppear(animated);
			
			this.View.BecomeFirstResponder();
			
			if(animated) {
				this.View.Alpha = 0.0f;
				
				UIView.BeginAnimations("FadeIn");
				UIView.SetAnimationDuration(FadeDuration);
				UIView.AnimationWillEnd += HandleAnimationWillEnd_FadeIn;
				
				this.View.Alpha = 1.0f;
				
				UIView.CommitAnimations();
			}
			else {
				IsPopoverVisible = true;
				this.ContentViewController.ViewDidAppear(animated);
			}
		}
		
		public void ViewWasTouched(TouchableView view)
		{
			if (IsPopoverVisible) {
				if(ShouldDismiss != null)
					if(ShouldDismiss(this))
						DismissPopover(true, true);
			}
		}
		
		public void RepositionPopover(RectangleF rect, UIView inView, UIPopoverArrowDirection arrowDirection)
		{
			var displayArea = CalculateDisplayArea(View);
			var containerView = (PopoverContainerView)this.View;
			
			containerView.UpdatePositionWithAnchorRect(rect, displayArea, arrowDirection);
			containerView.Frame = View.ConvertRectToView(containerView.Frame, this.BackgroundView);
			ArrowDirection = arrowDirection;
		}
		
		public RectangleF CalculateDisplayArea(UIView view)
		{
			var displayArea = RectangleF.Empty;
			
			if(view is IPopoverParentView)
				displayArea = (view as IPopoverParentView).CalculateDisplayArea();
			else
				displayArea = UIApplication.SharedApplication.KeyWindow.ConvertRectToView(UIScreen.MainScreen.ApplicationFrame, view);
			
			return displayArea;
		}
		
		public void DismissPopover(bool animated)
		{
			DismissPopover(animated, false);
		}
		
		public void DismissPopover(bool animated, bool userInitiated)
		{
			if(this.View != null) {
				ContentViewController.DismissViewController(animated, null);
				this.IsPopoverVisible = false;
				this.View.ResignFirstResponder();
				
				if(animated) {
					this.View.UserInteractionEnabled = false;
					UIView.BeginAnimations("FadeOut");
					UIView.AnimationWillEnd += HandleAnimationWillEnd_FadeOut;
					UIView.SetAnimationDuration(FadeDuration);
					
					this.View.Alpha = 0.0f;
					
					UIView.CommitAnimations();
				}
				else {
					this.ContentViewController.ViewDidDisappear(animated);
					this.View.RemoveFromSuperview();
					
					this.View = null;
					BackgroundView.RemoveFromSuperview();
					BackgroundView.Dispose();
					BackgroundView = null;
				}
			}
		}
		
		public void HandleAnimationWillEnd_FadeOut()
		{
			IsPopoverVisible = false;
			this.ContentViewController.ViewDidDisappear(true);
			
			this.View.RemoveFromSuperview();
			this.View.Dispose();
			
			BackgroundView.RemoveFromSuperview();
			BackgroundView.Dispose();
			
			if(this.DidDismiss != null)
				DidDismiss(this);
		}
		
		public void HandleAnimationWillEnd_FadeIn()
		{
			this.View.UserInteractionEnabled = true;
			this.IsPopoverVisible = true;
			this.ContentViewController.ViewDidAppear(true);
		}
		
		public void UpdateBackgroundPassthroughViews()
		{
			BackgroundView.PassthroughViews = this.PassthroughViews;
		}
		
		public UIView GetKeyView()
		{
			var window = UIApplication.SharedApplication.KeyWindow;
			
			if(window.Subviews.Any())
				return window.Subviews[0];
			else 
				return window;
		}
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				UIView.AnimationWillEnd -= HandleAnimationWillEnd_FadeIn;
				UIView.AnimationWillEnd -= HandleAnimationWillEnd_FadeOut;
			}
			base.Dispose(disposing);
		}
		
		public static PopoverContainerModel DefaultContainerModel
		{
			get
			{
				var bgMargin = 6.0f;
				var imageSize = new SizeF(30.0f, 30.0f);
				var contentMargin = 2.0f;
				
				return new PopoverContainerModel()
				{
					LeftBgMargin = bgMargin,
					RightBgMargin = bgMargin,
					TopBgMargin = bgMargin,
					BottomBgMargin = bgMargin,
					LeftBgCapSize = (int)imageSize.Width / 2,
					TopBgCapSize = (int)imageSize.Height / 2,
					LeftContentMargin = contentMargin,
					RightContentMargin = contentMargin,
					BottomContentMargin = contentMargin,
					ArrowMargin = 1.0f,
					BackgroundImage = PopoverImage.BackgroundImage,
					UpArrowImage = PopoverImage.UpArrowImage,
					DownArrowImage = PopoverImage.DownArrowImage,
					LeftArrowImage = PopoverImage.LeftArrowImage,
					RightArrowImage = PopoverImage.RightArrowImage,
				};
			}
		}
	}
}

