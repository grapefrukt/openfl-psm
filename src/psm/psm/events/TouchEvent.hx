package psm.events;

/**
 * ...
 * @author Martin Jonasson, m@grapefrukt.com
 */

class TouchEvent extends MouseEvent
{
	
	public static var TOUCH_BEGIN	:String = "touchBegin";
	public static var TOUCH_END		:String = "touchEnd";
	public static var TOUCH_MOVE	:String = "touchMove";
	
	public var touchPointID(default, null):Int;
	public var stageX(default, null):Float;
	public var stageY(default, null):Float;
	
	public function new(type:String, touchPointID:Int, stageX:Float, stageY:Float){
		super(type);
		this.touchPointID = touchPointID;
		this.stageX = stageX;
		this.stageY = stageY;
	}
	
}