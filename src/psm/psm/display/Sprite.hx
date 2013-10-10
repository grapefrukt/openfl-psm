package psm.display;
import psm.events.EventDispatcher;
import psm.display.Stage;
import psm.display.Renderer;
import psm.geom.Point;

/**
 * ...
 * @author Martin Jonasson, m@grapefrukt.com
 */

class Sprite extends EventDispatcher {
	
	public var renderer:Renderer;
	
	public var x(default, set):Float;
	public var y(default, set):Float;
	
	// used in one or two places
	public var rotation(default, set):Float;
	
	// used in three or four places
	public var scaleX(default, set):Float;
	public var scaleY(default, set):Float;
	
	public var alpha(default, set):Float;
	public var visible:Bool;
	
	public var cacheAsBitmap:Bool;
	
	public var ___stage:Stage;
	public var stage(get, never):Stage;
	
	public var buttonMode:Bool;
	public var mouseEnabled:Bool;
	public var mouseChildren:Bool;
	
	public var mouseX(get, never):Float;
	public var mouseY(get, never):Float;
	
	public var numChildren(get, never):Int;
	
	public var parent:Sprite;
	private var _children:Array<Sprite>;
	
	public function new() {
		super();
		_children = [];
		renderer = new RendererUniversal(this);
		x = y = 0;
		scaleX = scaleY = 1;
		alpha = 1;
		visible = true;
		buttonMode = false;
		mouseEnabled = true;
		mouseChildren = true;
	}
	
	public function removeChildAt(index:Int):Sprite {
		var child = _children[index];
		_children.remove(child);
		child.parent = null;
		return child;
	}
	
	public function addChild(child:Sprite):Sprite {
		_children.push(child);
		child.parent = this;
		child.renderer.invalidateTransform();
		return child;
	}
	
	public function removeChild(child:Sprite):Sprite {
		_children.remove(child);
		child.parent = null;
		return child;
	}
	
	public function getChildAt(index:Int):Sprite {
		return _children[index];
	}
	
	public function localToGlobal(p:Point):Point {
		return renderer.localToGlobal(p);
	}
	
	public function globalToLocal(p:Point):Point {
		return renderer.globalToLocal(p);
	}
	
	private function set_x(value:Float):Float {
		renderer.invalidateTransform();
		return x = value;
	}
	
	private function set_y(value:Float):Float {
		renderer.invalidateTransform();
		return y = value;
	}
	
	private function set_rotation(value:Float):Float {
		renderer.invalidateTransform();
		return rotation = value;
	}
	
	private function set_scaleX(value:Float):Float {
		renderer.invalidateTransform();
		return scaleX = value;
	}
	
	private function set_scaleY(value:Float):Float {
		renderer.invalidateTransform();
		return scaleY = value;
	}
	
	private function set_alpha(value:Float):Float {
		renderer.invalidateTransform();
		return alpha = value;
	}
	
	private function get_stage():Stage {
		if (___stage != null) return ___stage;
		if (parent == null) return null;
		return parent.get_stage();
	}
	
	private function get_mouseX():Float {
		return renderer.getMouseX();
	}
	
	private function get_mouseY():Float {
		return renderer.getMouseY();
	}
	
	private function get_numChildren() {
		return _children.length;
	}
	
}