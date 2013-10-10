package psm.text;
import com.grapefrukt.games.spaceblocks.Renderable;
import psm.display.Sprite;
import psm.display.Renderer;

/**
 * ...
 * @author Martin Jonasson, m@grapefrukt.com
 */

class TextField extends Renderable {

	public var text(default, set):String;
	public var htmlText(get, set):String;
	public var textWidth(get, never):Float;
	public var textHeight(get, never):Float;
	public var defaultTextFormat(default, set):TextFormat;
	public var width(default, set):Int;
	
	private var rendererTextField:RendererTextfield;
	
	public function new() {
		super();
		renderer = rendererTextField = new RendererTextfield(this);
	}
	
	private function set_text(value:String):String {
		if (text == value) return text;
		renderer.invalidateBuffer();
		return text = value;
	}
	
	private function get_textWidth():Float {
		return rendererTextField.getTextWidth();
	}
	
	private function get_textHeight():Float {
		return rendererTextField.getTextHeight();
	}
	
	private function set_defaultTextFormat(value:TextFormat):TextFormat {
		renderer.invalidateBuffer();
		return defaultTextFormat = value;
	}
	
	private function get_htmlText():String {
		return text;
	}
	
	private function set_htmlText(value:String):String {
		return set_text(value);
	}
	
	private function set_width(value:Int):Int {
		width = value;
		renderer.invalidateBuffer();
		return width;
	}
}