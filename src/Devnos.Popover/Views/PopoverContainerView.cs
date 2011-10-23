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
		public SizeF ContentSize { get { return CalculateContentRect().Size; } }
		
		UIImage _BackgroundImage;
		UIImage _ArrowImage;
		
		RectangleF _ArrowRect;
		RectangleF _BackgroundRect;
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
			
			DetermineGeometry(size, anchor, displayArea, arrowDirection);
			InitializeFrame();
			this.BackgroundColor = UIColor.Clear;
			
			var image = UIImage.FromBundle(properties.BackgroundImage);
			this._BackgroundImage = image.StretchableImage(properties.LeftBgCapSize, properties.TopBgCapSize);
			
			this.ClipsToBounds = true;
			this.UserInteractionEnabled = true;
		}
		
		public void InitializeFrame()
		{
//			var frame = RectangleFExtensions.Offset(RectangleF.Union(_BackgroundRect, _ArrowRect), _Offset.X, _Offset.Y);
			var frame = RectangleF.Union(_BackgroundRect, _ArrowRect);
			frame.X += _Offset.X;
			frame.Y += _Offset.Y;
			
			//If arrow rect origin is < 0 the frame above is extended to include it so we should offset the other rects
			
			_ArrowOffset = new PointF(Math.Max(0, (-_ArrowRect.Location.X)), Math.Max(0, (-_ArrowRect.Location.Y)));
			_BackgroundRect.Offset(_ArrowOffset.X, _ArrowOffset.Y);
			_ArrowRect.Offset(_ArrowOffset.X, _ArrowOffset.Y);
			this.Frame = frame;
		}
		
		public void UpdatePositionWithAnchorRect(RectangleF anchorRect, RectangleF displayArea, UIPopoverArrowDirection arrowDirections)
		{
			DetermineGeometry(_CorrectedSize, anchorRect, displayArea, arrowDirections);
			InitializeFrame();
		}
		
		public override bool PointInside(PointF point, UIEvent uievent)
		{
			return this.CalculateContentRect().Contains(point);
		}
		
		public RectangleF CalculateContentRect()
		{
			var x = Properties.LeftBgMargin + Properties.LeftContentMargin + _ArrowOffset.X;
			var y = Properties.TopBgMargin + Properties.TopContentMargin + _ArrowOffset.Y;
			
			var width = _BackgroundRect.Size.Width - Properties.LeftBgMargin - Properties.RightBgMargin 
				- Properties.LeftContentMargin - Properties.RightContentMargin;
			var height = _BackgroundRect.Size.Height - Properties.TopBgMargin - Properties.BottomBgMargin - Properties.TopContentMargin - Properties.BottomContentMargin;
			
			return new RectangleF(x, y, width, height);
			
		}
		
		public void SetContentView(UIView view)
		{
			if(view != this.ContentView) {
				this.ContentView = null;
				this.ContentView = view;
				this.ContentView.Frame = this.CalculateContentRect();
				this.AddSubview(ContentView);
			}
		}
		
		public override void Draw(RectangleF rect)
		{
			base.Draw(rect);
			_BackgroundImage.Draw(_BackgroundRect, CGBlendMode.Normal, 1.0f);
			_ArrowImage.Draw(_ArrowRect, CGBlendMode.Normal, 1.0f);
		}
		
		public void DetermineGeometry(SizeF size, RectangleF anchorRect, RectangleF displayArea, UIPopoverArrowDirection supportedDirections)
		{
			_Offset = PointF.Empty;
			_BackgroundRect = RectangleF.Empty;
			_ArrowRect = RectangleF.Empty;
			PopoverArrowDirection = UIPopoverArrowDirection.Unknown;
			
			var biggestSurface = 0.0f;
			var currentMinMargin = 0.0f;
			
			var upArrowImage = UIImage.FromBundle(this.Properties.UpArrowImage);
			var downArrowImage = UIImage.FromBundle(this.Properties.DownArrowImage);
			var leftArrowImage = UIImage.FromBundle(this.Properties.LeftArrowImage);
			var rightArrowImage = UIImage.FromBundle(this.Properties.RightArrowImage);
			
			foreach(var direction in (UIPopoverArrowDirection[])Enum.GetValues(typeof(UIPopoverArrowDirection))) {
				
				if(supportedDirections.HasFlag(direction)) {
					
					var bgRect = RectangleF.Empty;
					var arrowRect = RectangleF.Empty;
					var offset = PointF.Empty;
					var xArrowOffset = 0.0f;
					var yArrowOffset = 0.0f;
					var anchorPoint = PointF.Empty;
					
					switch(direction) {
						case UIPopoverArrowDirection.Up: {
							
							anchorPoint = new PointF(anchorRect.GetMidX(), anchorRect.GetMaxY());
							
							xArrowOffset = size.Width / 2 - upArrowImage.Size.Width / 2;
							yArrowOffset = Properties.TopBgMargin - upArrowImage.Size.Height;
							
							offset = new PointF(anchorPoint.X - xArrowOffset - upArrowImage.Size.Width / 2, anchorPoint.Y - yArrowOffset);
							bgRect = new RectangleF(0, 0, size.Width, size.Height);
						
							if(offset.X < 0) {
								xArrowOffset += offset.X;
								offset.X = 0;
							}
							else if(offset.X + size.Width > displayArea.Size.Width) {
								xArrowOffset += (offset.X + size.Width - displayArea.Size.Width);
								offset.X = displayArea.Size.Width - size.Width;
							}
						
							xArrowOffset = Math.Max(xArrowOffset, Properties.LeftBgMargin + Properties.ArrowMargin);
							xArrowOffset = Math.Min(xArrowOffset, size.Width - Properties.RightBgMargin - Properties.ArrowMargin - upArrowImage.Size.Width);
							
							arrowRect = new RectangleF(xArrowOffset, yArrowOffset, upArrowImage.Size.Width, upArrowImage.Size.Height);
						
							break;
						}
						case UIPopoverArrowDirection.Down: {
							anchorPoint = new PointF(anchorRect.GetMidX(), anchorRect.GetMinY());
						
							xArrowOffset = size.Width / 2 - downArrowImage.Size.Width / 2;
							yArrowOffset = size.Height - Properties.BottomBgMargin;
						
							offset = new PointF(anchorPoint.X - xArrowOffset - downArrowImage.Size.Width / 2,
								anchorPoint.Y - yArrowOffset - downArrowImage.Size.Height);
						
							bgRect = new RectangleF(0, 0, size.Width, size.Height);
						
							if(offset.X < 0) {
								xArrowOffset += offset.X;
								offset.X = 0;
							}
							else if(offset.X + size.Width > displayArea.Size.Width) {
								xArrowOffset += (offset.X + size.Width - displayArea.Size.Width);
								offset.X = displayArea.Size.Width - size.Width;
							}
						
							//cap arrow offset;
							xArrowOffset = Math.Max(xArrowOffset, Properties.LeftBgMargin + Properties.ArrowMargin);
							xArrowOffset = Math.Min(xArrowOffset, size.Width - Properties.RightBgMargin - Properties.ArrowMargin - downArrowImage.Size.Width);
							
							arrowRect = new RectangleF(xArrowOffset, yArrowOffset, downArrowImage.Size.Width, downArrowImage.Size.Height);
							
							break;
						}
						case UIPopoverArrowDirection.Left: {
							anchorPoint = new PointF(anchorRect.GetMaxX(), anchorRect.GetMidY());
						
							xArrowOffset = Properties.LeftBgMargin - leftArrowImage.Size.Width;
							yArrowOffset = size.Height / 2 - leftArrowImage.Size.Height / 2;
						
							offset = new PointF(anchorPoint.X - xArrowOffset, anchorPoint.Y - yArrowOffset - leftArrowImage.Size.Height / 2);
							bgRect = new RectangleF(0, 0, size.Width, size.Height);
							
							if(offset.Y < 0) {
								yArrowOffset += offset.Y;
								offset.Y = 0;
							}
							else if(offset.Y + size.Height > displayArea.Size.Height) {
								yArrowOffset += (offset.Y + size.Height) - displayArea.Size.Height;
								offset.Y = displayArea.Size.Height - size.Height;
							}
						
							//cap arrow offset;
							yArrowOffset = Math.Max(yArrowOffset, Properties.TopBgMargin + Properties.ArrowMargin);
							yArrowOffset = Math.Min(yArrowOffset, size.Height - Properties.BottomBgMargin - Properties.ArrowMargin - leftArrowImage.Size.Height);
						
							arrowRect = new RectangleF(xArrowOffset, yArrowOffset, leftArrowImage.Size.Width, leftArrowImage.Size.Height);
						
							break;	
						}
						case UIPopoverArrowDirection.Right: {
							anchorPoint = new PointF(anchorRect.GetMinX(), anchorRect.GetMidY());
							
							xArrowOffset = size.Width - Properties.RightBgMargin;
							yArrowOffset = size.Height / 2 - rightArrowImage.Size.Width / 2;
							
							offset = new PointF(anchorPoint.X - xArrowOffset - rightArrowImage.Size.Width, anchorPoint.Y - yArrowOffset - rightArrowImage.Size.Height / 2);
							bgRect = new RectangleF(0, 0, size.Width, size.Height);
						
							if(offset.Y < 0) {
								yArrowOffset += offset.Y;
								offset.Y = 0;
							}
							else if(offset.Y + size.Height > displayArea.Size.Height) {
								yArrowOffset += (offset.Y + size.Height) - displayArea.Size.Height;
								offset.Y = displayArea.Size.Height - size.Height;
							}
						
							//cap arrow offset;
							yArrowOffset = Math.Max(yArrowOffset, Properties.TopBgMargin + Properties.ArrowMargin);
							yArrowOffset = Math.Min(yArrowOffset, size.Height - Properties.BottomBgMargin - Properties.ArrowMargin - rightArrowImage.Size.Height);
						
							arrowRect = new RectangleF(xArrowOffset, yArrowOffset, rightArrowImage.Size.Width, rightArrowImage.Size.Height);
							
							break;	
						}
					}
					
					//end switch statement
					
//					var bgFrame = bgRect.RectOffset(offset.X, offset.Y);
//					var bgFrame = RectangleFExtensions.Offset(bgRect, offset.X, offset.Y);
					var bgFrame = bgRect;
					bgFrame.X += offset.X;
					bgFrame.Y += offset.Y;
					
					var minMarginLeft = bgFrame.GetMinX() - displayArea.GetMinX();
					var minMarginRight = displayArea.GetMaxX() - bgFrame.GetMaxY();
					var minMarginTop = bgFrame.GetMinY() - displayArea.GetMinY();
					var minMarginBottom = displayArea.GetMaxY() - bgFrame.GetMaxY();
					
					if(minMarginLeft < 0) {
					    // Popover is too wide and clipped on the left; decrease width
			    		// and move it to the right
						
						offset.X -= minMarginLeft;
						bgRect.Size.Width += minMarginLeft;
						minMarginLeft = 0;
						
						if(direction == UIPopoverArrowDirection.Right) {
							arrowRect.X = bgRect.GetMaxX() - Properties.RightBgMargin;
						}
					}
					
					if(minMarginRight < 0) {
						// Popover is too wide and clipped on the right; decrease width.
						
						bgRect.Size.Width += minMarginRight;
						minMarginRight = 0;
						
						if(direction == UIPopoverArrowDirection.Left) {
							arrowRect.X = bgRect.GetMinX() - leftArrowImage.Size.Width + Properties.LeftBgMargin;	
						}
					}
					
					if(minMarginTop < 0) {
						// Popover is too high and clipped at the top; decrease height and move it down
						
						offset.Y -= minMarginTop;
						bgRect.Size.Height += minMarginTop;
						minMarginTop = 0;
						
						if(direction == UIPopoverArrowDirection.Down) {
							arrowRect.Y = bgRect.GetMaxY() - Properties.BottomBgMargin;	
						}
					}

					if(minMarginBottom < 0) {
						// Popover is too high and clipped at the bottom; decrease height.
						
						bgRect.Size.Height += minMarginBottom;
						minMarginBottom = 0;
						
						if(direction == UIPopoverArrowDirection.Up) {
							arrowRect.Y = bgRect.GetMinY() - upArrowImage.Size.Height + Properties.TopBgMargin;	
						}
					}
					
					bgFrame = bgRect.RectOffset(offset.X, offset.Y);
					
					var minMargin = Math.Min(minMarginLeft, minMarginRight);
					minMargin = Math.Min(minMargin, minMarginTop);
					minMargin = Math.Min(minMargin, minMarginBottom);
					
					// Calculate intersection and surface
					var intersection = RectangleF.Intersect(displayArea, bgFrame);
					var surface = intersection.Size.Width * intersection.Size.Height;
					
					if(surface >= biggestSurface && minMargin >= currentMinMargin) {
						
						biggestSurface = surface;
						_Offset = offset;
						_ArrowRect = arrowRect;
						_BackgroundRect = bgRect;
						PopoverArrowDirection = direction;
						currentMinMargin = minMargin;
					}
				} // end if
			} //end foreach
			
			switch(PopoverArrowDirection) {
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
