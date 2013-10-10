using motion.actuators;
using psm;
using psm.display;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using System.Threading;
using System;
using psm.events;


namespace spaceblocks
{
	public class AppMain
	{
		private static AppRenderer renderer;
		private static Game game;
		private static TouchInputManager touchInput;
		private static ControllerInputManager controlInput;

		public static long durationRender = 0;
		public static long durationUpdate = 0;
		
		public static void Main (string[] args)
		{
			renderer = new AppRenderer();
			Lib.init();
			
			double start = Lib.getTimerFloat();

			Console.WriteLine("Started init at " + start);
			Console.WriteLine("Threaded startup");
			
			RendererTextured.init();
			renderer.RenderProgress();
			Console.WriteLine("Stuff on screen after " + (Lib.getTimerFloat() - start) + "ms");
						
			Thread threadAssets = new Thread(InitializeGameAssets);
			Thread threadGame = new Thread(InitializeGameSystems);
			
			threadAssets.Start();
			
			RendererUniversal.init();
			renderer.RenderProgress();
			RendererResourceFields.init();
			renderer.RenderProgress();
			RendererTextfield.init();
			renderer.RenderProgress();
			TextureManager.init();
			
			threadGame.Start();
			
			while (!threadAssets.Join(16) || !threadGame.Join(16)){
				SystemEvents.CheckEvents();
				renderer.RenderProgress();
			}
			
			threadAssets = null;
			threadGame = null;
			
			renderer.CompletedProgressRender();
			
			Console.WriteLine("Completed init in " + (Lib.getTimerFloat() - start) + "ms");
			
			Lib.startTimer();
			
			while (true) {
				SystemEvents.CheckEvents();
				Update();
				renderer.Render(game);
			}
		}
		
		public static void InitializeGameSystems() {
			touchInput = new TouchInputManager();
			controlInput = new ControllerInputManager();
			
			game = new Game();
						
			Lib.current.addChild(game);
			
			Lib.current.get_stage().dispatchEvent(new Event(Event.RESIZE));
			
			game.init();
			game.render.set_zoom(1.15);
			
			Lib.current.addChild(game.gui);
			
			Lib.current.get_stage().renderer.validateTransform();
			Lib.current.renderer.validateTransform();
			
			Lib.current.get_stage().dispatchEvent(new Event(Event.RESIZE));
		}
		
		public static void InitializeGameAssets(){
			com.grapefrukt.games.spaceblocks.managers.SoundPlayer.init();
			SoundManager.init();
		}
		
		private static int saveTime = Settings.AUTOSAVE_FREQUENCY / (1000/60) / 10;
		
		public static void Update ()
		{
			durationUpdate = Lib.getTimerFloat();
			SimpleActuator.updateTweens();
			touchInput.update();
			controlInput.update(game.input);
			game.update();
			durationUpdate = Lib.getTimerFloat() - durationUpdate;
			
			saveTime -= 1;
			
			//if(saveTime % 300 == 0) Console.WriteLine("autosave in " + saveTime / 60);
			
			if(saveTime < 0){
				Lib.current.get_stage().dispatchEvent(new Event(Event.DEACTIVATE));
				saveTime = Settings.AUTOSAVE_FREQUENCY / (1000/60) / 10;
			}
		}
	}
}