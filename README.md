<h1>Devnos.Popover</h1>

A generic popover re-implementation for MonoTouch inspired by the good folks at [WEPopover] (https://github.com/werner77/WEPopover)

## Installation

Simply compile the project or "Add Existing Project..." to a valid MonoTouch 4.x / 5.x + solution using MonoDevelop 2.8+

##Usage

	public class MyViewController : UIViewController
	{
		UIButton _Button;
		UIViewController _MyContentController;
		PopoverController _PopoverController;

		public override void ViewDidLoad()
		{
			//.....
			//	Initialization goes here
			//.....

			_Button.TouchUpInside += () => {
				_PopoverController.ContentViewController = _MyContentController;
				_PopoverController.PresentFromRect(_Button.Frame, this.View, UIPopoverArrowDirection.Any, true);
			};
		}
	}


## Feedback

Open an issue! Accepting pull requests daily!

