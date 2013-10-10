package psm.display;

/**
 * ...
 * @author Martin Jonasson, m@grapefrukt.com
 */

class Stage extends Sprite {

	public var _mouseX:Float;
	public var _mouseY:Float;
	
	public function new() {
		super();
	}
	
	override private function get_mouseX():Float {
		return _mouseX;
	}
	
	override private function get_mouseY():Float {
		return _mouseY;
	}
	
}