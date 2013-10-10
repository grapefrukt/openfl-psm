using psm.display;
using Sce.PlayStation.Core.Graphics;
using System.Collections.Generic;
using System;

namespace psm.display
{
	public class RendererUniversal : Renderer
	{
		
		private ShaderProgram program;
		protected static ShaderProgram shaderUniversal;
		protected static ShaderProgram shaderColored;
		
		private VertexBuffer vertexBuffer;
		private static Texture2D texture;
		private static Dictionary<string, SpriteData> sprites;
		private static int sheetW; 
		private static int sheetH;
		private static SpriteData white;
		
		private System.Collections.Generic.List<float> verts;
		private System.Collections.Generic.List<float> colors;
		private System.Collections.Generic.List<float> uvs;
		
		public RendererUniversal (Sprite sprite) : base(sprite)
		{
			verts =	 new System.Collections.Generic.List<float>();
			colors = new System.Collections.Generic.List<float>();
			uvs = 	 new System.Collections.Generic.List<float>();
		}
		
		public static void init() {
			if (shaderUniversal == null){
				shaderUniversal = new ShaderProgram("/Application/shaders/Universal.cgx");
				
				shaderUniversal.SetAttributeBinding(0, "a_Position");
				shaderUniversal.SetAttributeBinding(1, "a_VertexColor");
				shaderUniversal.SetAttributeBinding(2, "a_TexCoord");
				
				shaderUniversal.SetUniformBinding(0, "u_SceneMatrix");
				shaderUniversal.SetUniformBinding(1, "u_ScreenMatrix");
				shaderUniversal.SetUniformBinding(2, "u_Alpha");
				
				texture = new Texture2D("/Application/assets/texturepack/rymdkapsel-hd.png", false);
				texture.SetFilter(TextureFilterMode.Disabled);
				texture.SetWrap(TextureWrapMode.ClampToEdge);
				texture.SetMaxAnisotropy(0);
			}
			
			if (shaderColored == null){
				shaderColored = new ShaderProgram("/Application/shaders/Colored.cgx");
				
				shaderColored.SetAttributeBinding(0, "a_Position");
				shaderColored.SetAttributeBinding(1, "a_VertexColor");
				
				shaderColored.SetUniformBinding(0, "u_SceneMatrix");
				shaderColored.SetUniformBinding(1, "u_ScreenMatrix");
				shaderColored.SetUniformBinding(2, "u_Alpha");
			}
		}
		
		override public void clearGraphics(){
			verts.Clear();
			colors.Clear();
			uvs.Clear();
			dirtyBuffer = true;
		}
		
		override public void drawFilledRect(int color, double alpha, double x, double y, double width, double height){
			verts.Add((float) x); 			verts.Add((float) y);				// 0
			verts.Add((float) (x + width)); verts.Add((float) y);				// 1
			verts.Add((float) x); 			verts.Add((float) (y + height));	// 2
			
			verts.Add((float) (x + width)); verts.Add((float) (y + height));	// 3
			verts.Add((float) (x + width)); verts.Add((float) y);				// 1
			verts.Add((float) x); 			verts.Add((float) (y + height));	// 2
			
			float r = ((color >> 16) & 255) / 255f;
			float g = ((color >> 8) & 255) / 255f;
			float b = (color & 255) / 255f;
			float a = (float) alpha;
			
			colors.Add(r); colors.Add(g); colors.Add(b); colors.Add(a);
			colors.Add(r); colors.Add(g); colors.Add(b); colors.Add(a);
			colors.Add(r); colors.Add(g); colors.Add(b); colors.Add(a);
			
			colors.Add(r); colors.Add(g); colors.Add(b); colors.Add(a);
			colors.Add(r); colors.Add(g); colors.Add(b); colors.Add(a);
			colors.Add(r); colors.Add(g); colors.Add(b); colors.Add(a);
			
			uvs.Add((float) (white.x + white.width / 2) / sheetW); uvs.Add((float) (white.y + white.height / 2) / sheetH);
			uvs.Add((float) (white.x + white.width / 2) / sheetW); uvs.Add((float) (white.y + white.height / 2) / sheetH);
			uvs.Add((float) (white.x + white.width / 2) / sheetW); uvs.Add((float) (white.y + white.height / 2) / sheetH);
			
			uvs.Add((float) (white.x + white.width / 2) / sheetW); uvs.Add((float) (white.y + white.height / 2) / sheetH);
			uvs.Add((float) (white.x + white.width / 2) / sheetW); uvs.Add((float) (white.y + white.height / 2) / sheetH);
			uvs.Add((float) (white.x + white.width / 2) / sheetW); uvs.Add((float) (white.y + white.height / 2) / sheetH);
		
			dirtyBuffer = true;
		}
		
		override public void drawPolygon(int color, double alpha, Array<double> vertices) {
			float r = ((color >> 16) & 255) / 255f;
			float g = ((color >> 8) & 255) / 255f;
			float b = (color & 255) / 255f;
			float a = (float) alpha;
			
			for (int i = 4; i < vertices.length; i+=2){
				verts.Add((float) vertices[i - 4]); verts.Add((float) vertices[i - 4 + 1]);
				verts.Add((float) vertices[i - 2]); verts.Add((float) vertices[i - 2 + 1]);
				verts.Add((float) vertices[i - 0]); verts.Add((float) vertices[i - 0 + 1]);
				
				colors.Add(r); colors.Add(g); colors.Add(b); colors.Add(a);
				colors.Add(r); colors.Add(g); colors.Add(b); colors.Add(a);
				colors.Add(r); colors.Add(g); colors.Add(b); colors.Add(a);

				uvs.Add((float) (white.x + white.width / 2) / sheetW); uvs.Add((float) (white.y + white.height / 2) / sheetH);
				uvs.Add((float) (white.x + white.width / 2) / sheetW); uvs.Add((float) (white.y + white.height / 2) / sheetH);
				uvs.Add((float) (white.x + white.width / 2) / sheetW); uvs.Add((float) (white.y + white.height / 2) / sheetH);
			}
			
			dirtyBuffer = true;
		}
		
		override protected void drawTexturedRectOffset(string texture, double x, double y, double width, double height, double offsetX) {
			SpriteData data;
			sprites.TryGetValue(texture, out data);
			
			if (width == -1) width = data.width;
			if (height == -1) height = data.height;			
			
			verts.Add((float) x); 			verts.Add((float) y);				// 0
			verts.Add((float) (x + width)); verts.Add((float) y);				// 1
			verts.Add((float) x); 			verts.Add((float) (y + height));	// 2
			
			verts.Add((float) (x + width)); verts.Add((float) (y + height));	// 3
			verts.Add((float) (x + width)); verts.Add((float) y);				// 1
			verts.Add((float) x); 			verts.Add((float) (y + height));	// 2
						
			colors.Add(1); colors.Add(1); colors.Add(1); colors.Add(1);
			colors.Add(1); colors.Add(1); colors.Add(1); colors.Add(1);
			colors.Add(1); colors.Add(1); colors.Add(1); colors.Add(1);
			colors.Add(1); colors.Add(1); colors.Add(1); colors.Add(1);
			colors.Add(1); colors.Add(1); colors.Add(1); colors.Add(1);
			colors.Add(1); colors.Add(1); colors.Add(1); colors.Add(1);
			
			float doubled = data.doubled ? 2 : 1;
			offsetX *= doubled;
			width *= doubled;
			height *= doubled;
			
			uvs.Add((float) (data.x + offsetX) / sheetW);				uvs.Add((float) (data.y) / sheetH);
			uvs.Add((float) (data.x + width + offsetX) / sheetW);		uvs.Add((float) (data.y) / sheetH);
			uvs.Add((float) (data.x + offsetX) / sheetW);				uvs.Add((float) (data.y + height) / sheetH);
			
			uvs.Add((float) (data.x + width + offsetX) / sheetW);		uvs.Add((float) (data.y + height) / sheetH);
			uvs.Add((float) (data.x + width + offsetX) / sheetW);		uvs.Add((float) (data.y) / sheetH);
			uvs.Add((float) (data.x + offsetX) / sheetW);				uvs.Add((float) (data.y + height) / sheetH);
			
			dirtyBuffer = true;
		}
		
		public override void drawFrame (string texture, double x, double y, int frameX)
		{
			SpriteData data;
			sprites.TryGetValue(texture, out data);
			
			drawTexturedRectOffset(texture, x + data.offsetX, y + data.offsetY, data.frameW, data.frameH, data.frameW * frameX);
		}
		
		override public void validateBuffer(){
			dirtyBuffer = false;
			
			if (vertexBuffer != null){
				vertexBuffer.Dispose();
				vertexBuffer = null;
			}
			
			if (verts.Count == 0) return;
			
						
			//if (renderable.usesTextures) {
				vertexBuffer = new VertexBuffer(verts.Count / 2, VertexFormat.Float2, VertexFormat.Float4, VertexFormat.Float2);
				vertexBuffer.SetVertices(0, verts.ToArray());
				vertexBuffer.SetVertices(1, colors.ToArray());
				vertexBuffer.SetVertices(2, uvs.ToArray());
				program = shaderUniversal;
			/*} else {
				vertexBuffer = new VertexBuffer(verts.Count / 2, VertexFormat.Float2, VertexFormat.Float4);
				vertexBuffer.SetVertices(0, verts.ToArray());
				vertexBuffer.SetVertices(1, colors.ToArray());
				program = shaderColored;
			}*/
			
			
			
			//Console.WriteLine("buffer validated " + renderable.numPolyVerts + " commands");
		}
		
		override public void render(GraphicsContext context){
			if (dirtyBuffer) {
				validateBuffer();
			}
			if (vertexBuffer == null) return;
			
			program.SetUniformValue(0, ref sceneTransform);
			program.SetUniformValue(1, ref screenMatrix);
			program.SetUniformValue(2, (float) sceneAlpha);
				
			context.SetShaderProgram(program);
			context.SetVertexBuffer(0, vertexBuffer);
			if (program == shaderUniversal) context.SetTexture(0, texture);
			
			context.DrawArrays(DrawMode.Triangles, 0, vertexBuffer.VertexCount);
			
			if (program == shaderUniversal) context.SetTexture(0, null);
		}
		
		public static void defineSprite(string name, int x, int y, int width, int height, int frameW, int frameH, int offsetX, int offsetY, int sheetW, int sheetH, bool doubled) {
			if (sprites == null) sprites = new Dictionary<string, SpriteData>();
			SpriteData data = new SpriteData(x, y, width, height, frameW, frameH, offsetX, offsetY, doubled);
			sprites.Add(name, data);
			RendererUniversal.sheetW = sheetW;
			RendererUniversal.sheetH = sheetH;
			
			if (name == "white") white = data;
		}
	}
}

struct SpriteData {
	
	public int x;
	public int y; 
	public int width;
	public int height;
	public int frameW;
	public int frameH;
	public int offsetX;
	public int offsetY;
	public bool doubled;
			
	public SpriteData(int x, int y, int width, int height, int frameW, int frameH, int offsetX, int offsetY, bool doubled){
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
		this.frameW = frameW;
		this.frameH = frameH;
		this.offsetX = offsetX;
		this.offsetY = offsetY;
		this.doubled = doubled;
	}
}
