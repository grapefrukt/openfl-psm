package psm.events;

class Event
{
	public var target : Dynamic;
	public var type(default,null) : String;

	public static var RESIZE 		= "resize";
	public static var CHANGE 		= "change";
	public static var DEACTIVATE 	= "deactivate";
	public static var CLOSE 		= "close";
	
	public function new(inType : String) {
		type = inType;
	}
}