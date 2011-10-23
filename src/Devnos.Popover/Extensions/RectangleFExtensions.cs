using System;
using System.Drawing;

namespace MonoTouch.CoreGraphics
{
	public static class RectangleFExtensions
	{
		public static RectangleF Offset(RectangleF rect, float x, float y)
		{
			var foo =rect;
			foo.X += x;
			foo.Y += y;
			return foo;
		}
		
		public static RectangleF Offset( RectangleF rect, PointF pos)
		{
			return rect.RectOffset(pos);
		}
		
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

