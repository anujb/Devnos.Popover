using System;
using System.Linq;
using MonoTouch.UIKit;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Devnos.Popover
{
	public class TouchableView : UIView
	{
		public bool IsTouchForwardingDisabled { get; set; }
		public IEnumerable<UIView> PassthroughViews { get; set; }
		public Action<TouchableView> ViewWasTouched { get; set; }
		
		bool ShouldTestHits;
		
		public TouchableView()
			: base()
		{
			
		}
		
		public override UIView HitTest(PointF point, UIEvent uievent)
		{	
			if (ShouldTestHits) {
				return null;
			}
			else if (IsTouchForwardingDisabled) {
				return this;
			}
			else {
				var hitView = base.HitTest(point, uievent);
				
				if(hitView == this) {
					ShouldTestHits = true;
					var superHitView = this.Superview.HitTest(point, uievent);
					ShouldTestHits = false;
					
					if (IsPassThroughView(superHitView)) {
						hitView = superHitView;
					}
				}
					
				return hitView;
			}
		}
		
		public override void TouchesBegan(MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			if(ViewWasTouched != null) {
				ViewWasTouched(this);
			}
		}
					
		public bool IsPassThroughView(UIView view)
		{
			if (view == null) {
				return false;
			}
			if (PassthroughViews.Contains(view)) {
				return true;
			}
			else {
				return IsPassThroughView(view.Superview);
			}
		}
						
		
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				
			}
			base.Dispose(disposing);
		}
		
	}
}

