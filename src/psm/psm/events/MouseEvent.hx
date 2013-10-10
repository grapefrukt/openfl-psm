package psm.events;
import psm.events.Event;

/**
 * ...
 * @author Martin Jonasson, m@grapefrukt.com
 */

class MouseEvent extends Event {
	
	public static var CLICK			:String = "mouseevent_click";
	public static var MOUSE_DOWN	:String = "mouseevent_mouse_down";
	public static var MOUSE_MOVE	:String = "mouseevent_mouse_move";
	public static var MOUSE_UP		:String = "mouseevent_mouse_up";
	
	public function new(type:String) {
		super(type);
	}
	
}