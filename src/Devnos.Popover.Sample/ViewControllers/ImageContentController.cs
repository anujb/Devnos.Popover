using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Devnos.Popover.Sample
{
	public class ImageContentController : UIViewController
	{
		UIImageView _ImageView;
		
		public ImageContentController()
		{
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			this.View.BackgroundColor = UIColor.Magenta;
			_ImageView = new UIImageView(UIImage.FromFile(NSBundle.MainBundle.PathForResource("Images/dave", "png")));
			_ImageView.Frame = this.View.Frame;
			this.View.AddSubview(_ImageView);
		}
	}
}

