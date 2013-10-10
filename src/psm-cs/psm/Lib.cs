using System.Diagnostics;
using psm.display;
using Sce.PlayStation.Core;
using System;

namespace psm
{
	public class Lib
	{
		private static Stopwatch stopwatch;
		public static Sprite current;
		private static long timeOffset;
		private static bool started;
		
		public static void init()
		{
			stopwatch = new Stopwatch();
       		stopwatch.Start();
			current = new Sprite();
			current.___stage = new Stage();
			current.get_stage().addChild(current);

			current.get_stage().renderer.transform = Matrix4.Identity;
			current.get_stage().renderer.sceneTransform = Matrix4.Identity;
			
			started = false;
			timeOffset = 0;
		}
		
		public static int getTimer  ()
		{
			if (!started) return 0;	
			return (int) (stopwatch.ElapsedMilliseconds - timeOffset);
		}
		
		public static long getTimerFloat  ()
		{
			return stopwatch.ElapsedMilliseconds - timeOffset;
		}
		
		public static void startTimer()
		{
			started = true;
			timeOffset = getTimerFloat();
		}
		
		public static void trace(string text){
			Console.WriteLine(text);
		}
	}
}

