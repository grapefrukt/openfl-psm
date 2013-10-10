# OpenFL to PlayStation Mobile hack

This most of the compatibility layer I coded to put my game [rymdkapsel](http://rymdkapsel.com), written in [haxe](http://haxe.org/) using [OpenFL](http://www.openfl.org/) onto [PlayStation Mobile](http://playstation.com/psm/). I won't call this a port, since it retains very little of the original code (basically the Point class) and is only the very smallest subset I could get away with. 

As I'm writing this text it's been five months since the game released on PSM and I still haven't felt like I've had time to clean this up and release it. So, under the increasingly apparent reality that I will never have time, I decided to just release it anyway, sans cleanup.

This has been ripped out of my repository for rymdkapsel with little regard for things actually working, but if someone wants to pick it up, you can.

As I was doing this port I did a few things to simplify things for myself:

* No support for the Assets class, especially no support for asset embedding
* Only supports a partial API, in a slightly tweaked version
* Only supports the use of ONE texture, which is supposed to be an atlas. Code to parse and deal with this is not included. 
* Comes with a haxe to C# compatible version of Actuate which unfortunately requires everything tweened to extend Tweenable
* Minimal use of comments (Sorry)

This will likely not compile, but in the unlikely event that you want to try anyway, there's a build.hxml that you can add your game source code folder to. Then, use the command line to compile using that hxml. It will generate a bunch of C# code into a psm-cs-gen folder. Add that folder together with the psm-cs folder to a PSM Studio project and start poking around. 

Best of luck!

Naturally, this comes with no support whatsoever. 