using System;
using System.Drawing;

namespace MonoTouch.CoreGraphics
{
	public static class RectangleFExtensions
	{
		public static RectangleF RectOffset(this RectangleF rect, float x, float y)
		{
			rect.Offset(x, y);
			return rect;
		}
		
		public static RectangleF RectOffset(this RectangleF rect, PointF pos)
		{
			rect.Offset(pos);
			return rect;
		}
	}
}

