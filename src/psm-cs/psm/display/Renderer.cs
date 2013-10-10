using System;
using com.grapefrukt.games.spaceblocks;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using psm.geom;

namespace psm.display
{
	public class Renderer
	{
		public Boolean dirtyTransform = true;
		public Boolean dirtyBuffer = true;
		
		public Matrix4 transform;
		public Matrix4 sceneTransform;
		
		public float sceneAlpha;
		
		protected Sprite sprite;
		protected Renderable renderable;
		
		protected Vector3 translation;
		protected Vector3 scale;
		protected Quaternion rotation;
		
		public static Matrix4 screenMatrix;

		private Matrix4 inverseSceneTransform;
		private Vector2 touchStage;
		private Vector2 touchLocal;

		public Renderer(Sprite sprite) {
			this.sprite = sprite;
			if (sprite is Renderable) renderable = (Renderable) sprite;
			
			transform = Matrix4.Identity;
			sceneTransform = Matrix4.Identity;
			translation = Vector3.Zero;
			scale = Vector3.One;
			rotation = Quaternion.Identity;

			inverseSceneTransform = Matrix4.Identity;
			touchStage = new Vector2();
			touchLocal = new Vector2();
		}
				
		public void invalidateTransform(){
			if (dirtyTransform) return;
			dirtyTransform = true;
			for(int i = 0; i < sprite._children.length; i++){
				((Sprite)sprite._children[i]).renderer.invalidateTransform();
			}
		}
		
		public void validateTransform(){
			if (!dirtyTransform) return;
			
			//Console.WriteLine("validated transform", sprite);
			
			if (sprite.parent != null) {
				sceneAlpha = (float) (sprite.alpha * sprite.parent.renderer.sceneAlpha);
			} else {
				sceneAlpha = (float) sprite.alpha;
			}
			
			translation.X = (float) sprite.x;
			translation.Y = (float) sprite.y;
			
			scale.X = (float) sprite.scaleX;
			scale.Y = (float) sprite.scaleY;
			
			Quaternion.RotationXyz(0f, 0f, (float)sprite.rotation * FMath.DegToRad, out rotation);
			Matrix4.Transformation(ref translation, ref rotation, ref scale, out transform);
			
			if(sprite.parent != null && sprite.parent.renderer.sceneTransform != null){
				Matrix4.Multiply(ref sprite.parent.renderer.sceneTransform, ref transform, out sceneTransform);
			}

			sceneTransform.InverseAffine(out inverseSceneTransform);
						
			dirtyTransform = false;
 		}
		
		private static Vector2 localPoint = new Vector2();
		private static Vector2 globalPoint = new Vector2();
		public Point localToGlobal(Point p){
			localPoint.X = (float) p.x;
			localPoint.Y = (float) p.y;
			sceneTransform.TransformPoint(ref localPoint, out globalPoint);
			return new Point(new haxe.lang.Null<double>(globalPoint.X, true), new haxe.lang.Null<double>(globalPoint.Y, true));
		}
		
		public Point globalToLocal(Point p){
			globalPoint.X = (float) p.x;
			globalPoint.Y = (float) p.y;
			inverseSceneTransform.TransformPoint(ref globalPoint, out localPoint);
			return new Point(new haxe.lang.Null<double>(localPoint.X, true), new haxe.lang.Null<double>(localPoint.Y, true));
		}
		
		public void invalidateBuffer(){
			dirtyBuffer = true;
		}
		
		public virtual void validateBuffer(){
			dirtyBuffer = false;
		}
		
		public virtual void render(GraphicsContext context){
			
		}

		private void updateMousePosition(){
			if (sprite.get_stage() == null) return;
			touchStage.X = (float) sprite.get_stage().get_mouseX();
			touchStage.Y = (float) sprite.get_stage().get_mouseY();
			inverseSceneTransform.TransformPoint(ref touchStage, out touchLocal);
		}

		public float getMouseX(){
			updateMousePosition();
			return touchLocal.X;
		}

		public float getMouseY(){
			updateMousePosition();
			return touchLocal.Y;
		}
		
		public virtual void clearGraphics(){
			
		}
		
		public virtual void drawFilledRect(int color, double alpha, double x, double y, double width, double height){
			
		}
		
		public virtual void drawPolygon(int color, double alpha, Array<double> vertices) {
			
		}
		
		public virtual void drawFrame(string texture, double x, double y, int frameX){
			
		}
		
		public void drawTexturedRect(string texture, double x, double y, double width, double height) {
			drawTexturedRectOffset(texture, x, y, width, height, 0);
		}
		
		protected virtual void drawTexturedRectOffset(string texture, double x, double y, double width, double height, double offsetX) {
			
		}
		
		public void drawTexturedRectFixedSize(string texture, double x, double y) {
			drawTexturedRectOffset(texture, x, y, -1, -1, 0);
		}
	
		
	}
}

