package psm.events;
import psm.Lib;
import psm.Tweenable;

extern class EventDispatcherCSharp {
	public function new():Void;
	public function addEventListener(type:String, inListener:Dynamic):Void;
	public function dispatchEvent(event : Event):Bool;
	public function hasEventListener(type : String):Bool;
	public function removeEventListener(type : String, inListener : Dynamic):Void;
}

class EventDispatcher extends Tweenable {
	
	private var wrap:EventDispatcherCSharp;
	
	public function new():Void {
		
	}

	public function addEventListener(type:String, inListener:Dynamic):Void {
		if (wrap == null) wrap = new EventDispatcherCSharp();
		wrap.addEventListener(type, inListener);
	}

	public function dispatchEvent(event : Event) : Bool {
		if (!hasEventListener(event.type)) return false;
		return wrap.dispatchEvent(event);
	}

	public function hasEventListener(type : String) {
		if (wrap == null) return false;
		return wrap.hasEventListener(type);
	}

	public function removeEventListener(type : String, inListener : Dynamic) : Void	{
		if (wrap == null) return;
		wrap.removeEventListener(type, inListener);
	}
}