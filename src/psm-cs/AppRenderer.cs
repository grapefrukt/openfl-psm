using System;
using com.grapefrukt.games.spaceblocks;
using Sce.PlayStation.Core.Graphics;
using psm.display;
using Sce.PlayStation.Core;
using psm;

namespace spaceblocks
{
	public class AppRenderer
	{
		private static GraphicsContext context;
		private static RendererPerformanceBar bar;
		
		private static RendererStartup startupRenderer;
		
		public AppRenderer (){
			int dspWidth = GraphicsContext.ScreenSizes[0].Width;
			int dspHeight = GraphicsContext.ScreenSizes[0].Height;
			
			//trace("Display claims to be: " + dspWidth + "x" + dspHeight);
			
			context = new GraphicsContext (dspWidth, dspHeight, PixelFormat.Rgba, PixelFormat.None, MultiSampleMode.None);
			
			Settings.STAGE_W = dspWidth;
			Settings.STAGE_H = dspHeight;
			
			context.SetViewport(0, 0, dspWidth, dspHeight);
			context.SetScissor(0, 0, dspWidth, dspHeight);
			context.Disable(EnableMode.All);
			
			context.Enable(EnableMode.ScissorTest);
			context.Enable(EnableMode.Blend);
			context.SetBlendFunc(BlendFuncMode.Add, BlendFuncFactor.SrcAlpha, BlendFuncFactor.OneMinusSrcAlpha);
						
			context.SetClearColor(9/255f, 11f/255f, 15f/255f, 1f);
			Renderer.screenMatrix = Matrix4.Ortho(0, dspWidth, dspHeight, 0, 0.0f, 32768.0f);
		}
		
		
		public void Render(Game game) {
			context.Clear();
			RenderSprite(game);		
			context.SwapBuffers();
		}
		
		public void RenderProgress(){
			if (startupRenderer == null){
				startupRenderer = new RendererStartup(Lib.current);
			}
			context.Clear();
			startupRenderer.render(context);
			context.SwapBuffers();
		}

		private void RenderSprite(Sprite sprite){
			if(!sprite.visible) return;
			if (sprite.renderer.dirtyTransform) sprite.renderer.validateTransform();
			if (sprite.renderer.sceneAlpha == 0) return;

			if (sprite is Renderable) sprite.renderer.render(context);
			
			for (int i = 0; i < sprite._children.length; i++){
				RenderSprite((Sprite) sprite._children[i]);
			}
		}
	}
}