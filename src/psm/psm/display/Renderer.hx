package psm.display;
import psm.geom.Point;
import psm.text.TextField;

/**
 * ...
 * @author Martin Jonasson, m@grapefrukt.com
 */

extern class Renderer {
	public function new(sprite:Sprite):Void;
	public function invalidateTransform():Void;
	public function invalidateBuffer():Void;
	
	public function localToGlobal(p:Point):Point;
	public function globalToLocal(p:Point):Point;
	
	public function clearGraphics():Void;
	public function drawFilledRect(color:Int, alpha:Float, x:Float, y:Float, width:Float, height:Float):Void;
	public function drawPolygon(color:Int, alpha:Float, vertices:Array<Float>):Void;
	
	public function drawTexturedRect(texture:String, x:Int, y:Int, width:Int, height:Int):Void;
	private function drawTexturedRectOffset(texture:String, x:Int, y:Int, width:Int, height:Int, offsetX:Int, offsetY:Int):Void;
	public function drawTexturedRectFixedSize(texture:String, x:Int, y:Int):Void;
	
	public function drawFrame(texture:String, x:Float, y:Float, frameX:Int):Void;
	
	public function getMouseX():Float;
	public function getMouseY():Float;
}

extern class RendererUniversal extends Renderer {
	public function new(sprite:Sprite):Void;
	public static function defineSprite(key:String, x:Int, y:Int, w:Int, h:Int, frameW:Int, frameH:Int, offsetX:Int, offsetY:Int, sheetDataW:Int, sheetDataH:Int, doubled:Bool):Void;
}

extern class RendererTextfield extends Renderer {
	public function new(sprite:TextField):Void;
	
	public function getTextWidth():Float;
	public function getTextHeight():Float;
}