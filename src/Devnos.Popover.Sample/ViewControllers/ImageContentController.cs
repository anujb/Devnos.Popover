using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Devnos.Popover.Sample
{
	public class ImageContentController : UIViewController
	{
		public ImageContentController()
		{
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
//			this.View.BackgroundColor = UIColor.Magenta;
			var imageView = new UIImageView(UIImage.FromFile(NSBundle.MainBundle.PathForResource("Images/dave", "png")));
			this.View = imageView;
			this.View.Frame = new System.Drawing.RectangleF(0, 0, 600, 450);
		}
	}
}

