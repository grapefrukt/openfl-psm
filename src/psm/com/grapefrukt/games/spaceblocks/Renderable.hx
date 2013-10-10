package com.grapefrukt.games.spaceblocks;
import psm.display.Sprite;

/**
 * ...
 * @author Martin Jonasson, m@grapefrukt.com
 */

class Renderable extends Sprite {

	public var clickBoxX:Float;
	public var clickBoxY:Float;
	public var clickBoxW:Float;
	public var clickBoxH:Float;
	
	public function new() {
		super();
		clickBoxX = clickBoxY = clickBoxW = clickBoxH = 0;
	}
	
	public function setClickBox(x:Float, y:Float, width:Float, height:Float):Void {
		clickBoxX = x;
		clickBoxY = y;
		clickBoxW = width;
		clickBoxH = height;
	}
	
}