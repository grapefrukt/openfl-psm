package com.grapefrukt.games.spaceblocks.managers;
import com.grapefrukt.games.spaceblocks.Settings;
/**
 * ...
 * @author Martin Jonasson, m@grapefrukt.com
 */

extern class SoundPlayer {
	public static function init():Void;
	public static function preload(sound:String):Void;
	public static function toggleMusic():Void;
	public static function startMusic():Void;
	public static function stopMusic():Void;
	public static function play(sound:String, volume:Float):Void;
	public static function stop(sound:String):Void;
	static public function setAmbienceLevel(level:Float):Void;
}