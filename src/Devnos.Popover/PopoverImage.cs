using System;
using MonoTouch.Foundation;

namespace Devnos.Popover
{
	public class PopoverImage
	{
		public static string BackgroundImage = NSBundle.MainBundle.PathForResource("Images/popoverBg", "png");
		public static string UpArrowImage = NSBundle.MainBundle.PathForResource("Images/popoverArrowUp", "png");
		public static string DownArrowImage = NSBundle.MainBundle.PathForResource("Images/popoverArrowDown", "png");
		public static string LeftArrowImage = NSBundle.MainBundle.PathForResource("Images/popoverArrowLeft", "png");
		public static string RightArrowImage = NSBundle.MainBundle.PathForResource("Images/popoverArrowRight", "png");
	}
}

