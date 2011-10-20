using System;

namespace Devnos.Popover
{
	public interface IPopoverController
	{
		Action<IPopoverController> DidDismiss { get; set; }
		Func<IPopoverController, bool> ShouldDismiss { get; set; }
	}
}

