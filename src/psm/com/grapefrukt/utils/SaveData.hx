package com.grapefrukt.utils;

/**
 * ...
 * @author Martin Jonasson, m@grapefrukt.com
 */

extern class SaveData {
	public function new():Void;
	public function load():Void;
	public function save():Void;
	public function set(field:String, data:String, compress:Bool = false):Void;
	public function get(field:String, decompress:Bool = false):String;
}