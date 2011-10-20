using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace Devnos.Popover
{
	public interface IPopoverParentView
	{
		RectangleF CalculateDisplayArea();
	}
}

