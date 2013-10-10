using System;
using psm;
using psm.display;
using psm.events;
using Sce.PlayStation.Core.Input;

namespace spaceblocks
{
	public class TouchInputManager
	{

		private int mainTouch = -1;
		private long mainTouchDown = 0;

		public TouchInputManager ()
		{

		}

		public void update(){
			foreach (var touchData in Touch.GetData(0)) {

				// only use new touches as a main touch
				if (mainTouch == -1 && touchData.Status == TouchStatus.Down) {
					mainTouch = touchData.ID;
					mainTouchDown = Lib.getTimerFloat();
				}

				float touchX = touchToStageX(touchData.X);
				float touchY = touchToStageY(touchData.Y);

				if (touchData.ID == mainTouch){
					
					bool moved = Math.round(Lib.current.get_stage()._mouseX) != Math.round(touchX) || Math.round(Lib.current.get_stage()._mouseY) != Math.round(touchY);
					
					Lib.current.get_stage()._mouseX = touchX;
					Lib.current.get_stage()._mouseY = touchY;
	
					if (moved && touchData.Status == TouchStatus.Move){
						dispatch(Lib.current.get_stage(), MouseEvent.MOUSE_MOVE, true);
					}
					
					if (touchData.Status == TouchStatus.Down) {
						dispatch(Lib.current.get_stage(), MouseEvent.MOUSE_DOWN, true);
						dispatch(Lib.current.get_stage(), MouseEvent.CLICK, true);
					}

					if (touchData.Status == TouchStatus.Up || touchData.Status == TouchStatus.Canceled){
						dispatch(Lib.current.get_stage(), MouseEvent.MOUSE_UP, true);
						mainTouch = -1;
					}
				} 
				
				if (touchData.Status == TouchStatus.Down) {
					Lib.current.get_stage().dispatchEvent(new TouchEvent(TouchEvent.TOUCH_BEGIN, touchData.ID, touchX, touchY));
				}

				if (touchData.Status == TouchStatus.Up || touchData.Status == TouchStatus.Canceled){
					Lib.current.get_stage().dispatchEvent(new TouchEvent(TouchEvent.TOUCH_END, touchData.ID, touchX, touchY));
				}
				
				if (touchData.Status == TouchStatus.Move){
					Lib.current.get_stage().dispatchEvent(new TouchEvent(TouchEvent.TOUCH_MOVE, touchData.ID, touchX, touchY));
				}
				
			}
		}
		
		private static Sprite dispatch(Sprite target, String eventName, Boolean dispatchForStage = false){
			if (!target.mouseEnabled) return null;
			if (!target.visible) return null;
			Sprite dispatched = null;

			Renderable renderable;
			if ((renderable = target as Renderable) != null && renderable.clickBoxW > 0 && renderable.clickBoxH > 0){

				float localX = target.renderer.getMouseX();
				float localY = target.renderer.getMouseY();

				if (localX > renderable.clickBoxX &&
				    localY > renderable.clickBoxY &&
				    localX < renderable.clickBoxX + renderable.clickBoxW &&
				    localY < renderable.clickBoxY + renderable.clickBoxH
				){
					// Console.WriteLine("Local space : " + localX + " - " + localY + " box: " + renderable.clickBoxX + " " + renderable.clickBoxY + " " + renderable.clickBoxW + " " + renderable.clickBoxH);
					dispatched = target;
				}
			}

			for (int i = target._children.length - 1; dispatched == null && target.mouseChildren && i >= 0; i--){
				var child = dispatch((Sprite) target._children[i], eventName);
				if (child != null){
					dispatched = child;
					break;
				}
			}
			
			if (dispatchForStage) {
				var e = new MouseEvent(eventName);
				e.target = dispatched;
				
				if( dispatched != null) dispatched.dispatchEvent(e);
				Lib.current.get_stage().dispatchEvent(e);
			}
			
			return dispatched;
		}

		private static float touchToStageX(float pos){
			return ((pos + 0.5f) * Settings.STAGE_W);
		}

		private static float touchToStageY(float pos){
			return ((pos + 0.5f) * Settings.STAGE_H);
		}

	}
}

