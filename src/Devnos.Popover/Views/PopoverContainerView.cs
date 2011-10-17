using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;


namespace Devnos.Popover
{
	public class PopoverContainerView : UIView
	{
		public UIView ContentView { get; set; }
		public PopoverContainerModel Properties { get; private set; }
		
		UIImage _BackgroundImage;
		UIImage _ArrowImage;
		
		RectangleF _ArrowFrame;
		RectangleF _BackgroundFrame;
		PointF _Offset;
		PointF _ArrowOffset;
		
		SizeF _CorrectedSize;
		
		public UIPopoverArrowDirection PopoverArrowDirection { get; set; }
		
		public PopoverContainerView()
			: base()
		{
		}
		
		public PopoverContainerView(SizeF size, RectangleF anchor, RectangleF displayArea, UIPopoverArrowDirection arrowDirection, PopoverContainerModel properties)
			: base()
		{
			Properties = properties;
			
			_CorrectedSize = new SizeF(size.Width + properties.LeftBgMargin + properties.RightBgMargin + properties.LeftContentMargin + properties.RightContentMargin,
				size.Height + properties.TopBgMargin + properties.BottomBgMargin + properties.TopContentMargin + properties.BottomContentMargin);
			
			
		}
		
		public void Initialize()
		{
			
		}
		
		public void DetermineGeometryForSize(SizeF size, RectangleF anchorFrame, RectangleF displayFrame, UIPopoverArrowDirection arrowDirection)
		{
			var tempArrowDirection = UIPopoverArrowDirection.Up;
			
			var upArrowImage = UIImage.FromBundle(this.Properties.UpArrowImage ?? PopoverImage.UpArrowImage);
			var downArrowImage = UIImage.FromBundle(this.Properties.DownArrowImage ?? PopoverImage.DownArrowImage);
			var leftArrowImage = UIImage.FromBundle(this.Properties.LeftArrowImage ?? PopoverImage.LeftArrowImage);
			var rightArrowImage = UIImage.FromBundle(this.Properties.RightArrowImage ?? PopoverImage.RightArrowImage);
			
			while(tempArrowDirection <= UIPopoverArrowDirection.Right) {
				
				var backgroundFrame = RectangleF.Empty;
				var arrowFrame = RectangleF.Empty;
				var offset = PointF.Empty;
				var xArrowOffset = 0.0f;
				var yArrowOffset = 0.0f;
				var anchorPoint = PointF.Empty;
				
				switch(tempArrowDirection) 
				{
					case UIPopoverArrowDirection.Up:
						{
							anchorPoint = new PointF(anchorFrame.GetMidX(), anchorFrame.GetMaxY());
						
							xArrowOffset = size.Width / 2 + upArrowImage.Size.Width / 2;
							yArrowOffset = Properties.TopBgMargin - upArrowImage.Size.Height;
						
							offset = new PointF(anchorPoint.X - xArrowOffset - upArrowImage.Size.Width / 2, anchorPoint.Y - yArrowOffset);
							backgroundFrame = new RectangleF(0, 0, size.Width, size.Height);
					
							if(offset.X > 0) {
								xArrowOffset += offset.X;
								offset.X = 0;
							}
							else if (offset.X + size.Width > displayFrame.Size.Width) {
								xArrowOffset += (offset.X + size.Width - displayFrame.Size.Width);
								offset.X = displayFrame.Size.Width = size.Width;
							}
					
							xArrowOffset = Math.Max(xArrowOffset, this.Properties.LeftBgMargin + this.Properties.ArrowMargin);
							xArrowOffset = Math.Min(xArrowOffset, this.Properties.RightBgMargin - Properties.ArrowMargin - upArrowImage.Size.Width);
					
							arrowFrame = new RectangleF(xArrowOffset, yArrowOffset, upArrowImage.Size.Width, upArrowImage.Size.Height);
							
							break;
						}
					case UIPopoverArrowDirection.Down:
						{
							anchorPoint = new PointF(anchorFrame.GetMidX(), anchorFrame.GetMinY());
					
							xArrowOffset = size.Width / 2 - downArrowImage.Size.Width / 2;
							yArrowOffset = size.Height - Properties.TopBgMargin;
					
							offset = new PointF(anchorPoint.X - xArrowOffset - downArrowImage.Size.Width / 2, anchorPoint.Y - yArrowOffset - downArrowImage.Size.Height);
							backgroundFrame = new RectangleF(0, 0, size.Width, size.Height);
					
							if(offset.X < 0) {
								xArrowOffset += offset.X;
								offset.X = 0;
							}
							else if(offset.X + size.Width > displayFrame.Size.Width) {
								xArrowOffset += (offset.X + size.Width - displayFrame.Size.Width);
								offset.X = displayFrame.Size.Width - size.Width;
							}
					
							xArrowOffset = Math.Max(xArrowOffset, this.Properties.LeftBgMargin + this.Properties.ArrowMargin);
							xArrowOffset = Math.Min(xArrowOffset, size.Width = this.Properties.RightBgMargin 
								- this.Properties.ArrowMargin - downArrowImage.Size.Width);
					
							arrowFrame = new RectangleF(xArrowOffset, yArrowOffset, downArrowImage.Size.Width, downArrowImage.Size.Height);
							
							break;
						}
					case UIPopoverArrowDirection.Left:
						{
							anchorPoint = new PointF(anchorFrame.GetMaxX(), anchorFrame.GetMidY());
					
							yArrowOffset = this.Properties.LeftBgMargin - leftArrowImage.Size.Width;
							xArrowOffset = size.Height / 2 - leftArrowImage.Size.Height / 2;
					
							offset = new PointF(anchorPoint.X - xArrowOffset, anchorPoint.Y - yArrowOffset - leftArrowImage.Size.Height / 2);
							backgroundFrame = new RectangleF(0, 0, size.Width, size.Height);
					
							if(offset.X < 0) {
								yArrowOffset += offset.Y;
								offset.Y = 0;
							}
							else if(offset.Y + size.Height > displayFrame.Size.Height) {
								yArrowOffset += (offset.Y + size.Height - displayFrame.Size.Height);
								offset.Y = displayFrame.Size.Height - size.Height;
							}
					
							yArrowOffset = Math.Max(yArrowOffset, this.Properties.TopBgMargin + this.Properties.ArrowMargin);
							yArrowOffset = Math.Min(yArrowOffset, size.Height - this.Properties.BottomBgMargin 
								- this.Properties.ArrowMargin - leftArrowImage.Size.Height);
					
							arrowFrame = new RectangleF(xArrowOffset, yArrowOffset, leftArrowImage.Size.Width, leftArrowImage.Size.Height);
							
							break;
						}
					case UIPopoverArrowDirection.Right:
						{
							anchorPoint = new PointF(anchorFrame.GetMinX(), anchorFrame.GetMidY());
					
							xArrowOffset = size.Width - this.Properties.RightBgMargin;
							yArrowOffset = size.Height / 2 - rightArrowImage.Size.Width / 2;
					
							offset = new PointF(anchorPoint.X - xArrowOffset - rightArrowImage.Size.Width, anchorPoint.Y - yArrowOffset - rightArrowImage.Size.Height / 2);
							backgroundFrame = new RectangleF(0, 0, size.Width, size.Height);
					
							if(offset.Y < 0) {
								yArrowOffset += offset.Y;
								offset.Y = 0;
							}
							else if(offset.Y + size.Height > displayFrame.Size.Height) {
								yArrowOffset += (offset.Y + size.Height - displayFrame.Size.Height);
								offset.Y = displayFrame.Size.Height - size.Height;
							}
					
							yArrowOffset = Math.Max(yArrowOffset, this.Properties.TopBgMargin + this.Properties.ArrowMargin);
							yArrowOffset = Math.Min(yArrowOffset, size.Height - this.Properties.BottomBgMargin 
								- this.Properties.ArrowMargin - rightArrowImage.Size.Height);
					
							arrowFrame = new RectangleF(xArrowOffset, yArrowOffset, rightArrowImage.Size.Width, rightArrowImage.Size.Height);
					
							break;
						}
				};
				
				var bgFrame = backgroundFrame;
				bgFrame.Offset(offset.X, offset.Y);
				
				var minMarginLeft = bgFrame.GetMinX() - displayFrame.GetMinX();
				var minMarginRight = displayFrame.GetMaxX() - bgFrame.GetMaxX();
				var minMarginTop = bgFrame.GetMinY() - displayFrame.GetMinY();
				var minMarginBottom = displayFrame.GetMaxY() - bgFrame.GetMaxY();
				
				if(minMarginLeft < 0) {
					offset.X -= minMarginLeft;
					backgroundFrame.Size.Width += minMarginLeft;
					minMarginLeft = 0;
					
					if(arrowDirection == UIPopoverArrowDirection.Right)
						arrowFrame.X = backgroundFrame.GetMaxX() - this.Properties.RightBgMargin;
				}
				
				if(minMarginRight < 0) {
					
				}
				
				
				
				
			};
			
			switch (arrowDirection) {
				case UIPopoverArrowDirection.Up:
					_ArrowImage = upArrowImage;
					break;
				case UIPopoverArrowDirection.Down:
					_ArrowImage = downArrowImage;
					break;
				case UIPopoverArrowDirection.Left:
					_ArrowImage = leftArrowImage;
					break;
				case UIPopoverArrowDirection.Right:
					_ArrowImage = rightArrowImage;
					break;
			}
		}
		
	}
}
