using System;

namespace Devnos.Popover
{
	public class PopoverContainerModel
	{
		public string BackgroundImage { get; set; }
		public string UpArrowImage { get; set; }
		public string DownArrowImage { get; set; }
		public string LeftArrowImage { get; set; }
		public string RightArrowImage { get; set; }
		
		public float LeftBgMargin { get; set; }
		public float RightBgMargin { get; set; }
		public float TopBgMargin { get; set; }
		public float BottomBgMargin { get; set; }
		
		public float LeftContentMargin { get; set; }
		public float RightContentMargin { get; set; }
		public float TopContentMargin { get; set; }
		public float BottomContentMargin { get; set; }
		
		public int TopBgCapSize { get; set; }
		public int LeftBgCapSize { get; set; }
		
		public float ArrowMargin { get; set; }
	}
}

