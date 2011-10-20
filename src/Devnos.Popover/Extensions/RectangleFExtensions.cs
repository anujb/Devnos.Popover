using System;
using System.Drawing;

namespace MonoTouch.CoreGraphics
{
	public static class RectangleFExtensions
	{
		public static RectangleF RectOffset(this RectangleF rect, float x, float y)
		{
			rect.RectOffset(x, y);
			return rect;
		}
		
		public static RectangleF RectOffset(this RectangleF rect, PointF pos)
		{
			rect.RectOffset(pos);
			return rect;
		}
	}
}

