using psm.display;
using psm.text;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using System.Collections.Generic;
using System;

namespace psm.display
{
	public class RendererTextfield : RendererTextured
	{
		private static Dictionary<int, Font> fonts;
		private const float OFFSET_X = 2;
		private const float OFFSET_Y = 2;
		private const int OFFSET_SIZE = 1;
		
		private TextField textfield;
		private Texture2D texture;
		private VertexBuffer vertexBuffer;
		private Font font;
		
		private float textWidth;
		private float textHeight;
		private string[] textLines;

		public RendererTextfield (TextField textfield) : base((Sprite)textfield){
			this.textfield = textfield;
			font = getFont(20);
		}
		
		new public static void init() {
			fonts = new Dictionary<int, Font>();
		}
		
		private static Font getFont(int size){
			Font font = null;
			if (fonts.TryGetValue(size, out font)) return font;
			
			font = new Sce.PlayStation.Core.Imaging.Font("/Application/assets/fonts/helvetica.ttf", size, FontStyle.Regular);
			fonts.Add(size, font);
			
			Console.WriteLine("added font of size " + size);
			
			return font;
		}
		
		public float getTextWidth(){
			if (dirtyBuffer) updateSize();
			return textWidth;
		}
		
		public float getTextHeight(){
			if (dirtyBuffer) updateSize();
			return textHeight;
		}

		private void updateSize(){
			if (font.Size - OFFSET_SIZE != textfield.defaultTextFormat.size) {
				font = getFont(textfield.defaultTextFormat.size + OFFSET_SIZE);
			}
			textLines = textfield.text.Split('\n');
			textWidth = 0;
			foreach ( string line in textLines) {
				textWidth = (float) System.Math.Max(textWidth, font.GetTextWidth(line));
			}
			textHeight = font.Metrics.Height * textLines.Length;
		}
		
		override public void validateBuffer(){
			if (texture != null){
				texture.Dispose();
				texture = null;
			}
			
			if (vertexBuffer != null){
				vertexBuffer.Dispose();
				vertexBuffer = null;
			}
			
			if (textfield.text == null || textfield.text.Length == 0){
				dirtyBuffer = false;
				return;
			}
			
			updateSize();
			
			float[] verts = new float[8];
			float[] uvs = new float[8];
			
			int maxWidth = (int) textWidth;
			if (textfield.defaultTextFormat.align == "right"){
				maxWidth = textfield.width;
			}

			verts[0] = (float) OFFSET_X;
			verts[1] = (float) OFFSET_Y;

			verts[2] = (float) OFFSET_X;
			verts[3] = (float) OFFSET_Y + textHeight;

			verts[6] = (float) OFFSET_X + maxWidth;
			verts[7] = (float) OFFSET_Y;

			verts[4] = (float) OFFSET_X + maxWidth;
			verts[5] = (float) OFFSET_Y + textHeight;

			uvs[0] = 0;
			uvs[1] = 0;

			uvs[2] = 0;
			uvs[3] = 1;

			uvs[6] = 1;
			uvs[7] = 0;

			uvs[4] = 1;
			uvs[5] = 1;
		
			vertexBuffer = new VertexBuffer(4, VertexFormat.Float2, VertexFormat.Float2);
			vertexBuffer.SetVertices(0, verts);
			vertexBuffer.SetVertices(1, uvs);

			int argb = textfield.defaultTextFormat.color;

			var image = new Image(	ImageMode.Rgba,
									new ImageSize((int)maxWidth, (int)textHeight),
									new ImageColor(0, 0, 0, 0));
			
			ImagePosition pos = new ImagePosition(0, 0);
			for (int i = 0; i < textLines.Length; i++){
				pos.X = 0;
				pos.Y = i * font.Metrics.Height;
				
				if (textfield.defaultTextFormat.align == "right"){
					pos.X = (int) maxWidth - font.GetTextWidth(textLines[i]);
				}
				
				image.DrawText(	
					textLines[i],
					new ImageColor((int)((argb >> 16) & 0xff),
					(int)((argb >> 8) & 0xff),
					(int)((argb >> 0) & 0xff),
					255),
					font, 
				    pos
				);
			}
			
			texture = new Texture2D((int)maxWidth, (int)textHeight, false, PixelFormat.Rgba);
			texture.SetPixels(0, image.ToBuffer());
			texture.SetFilter(TextureFilterMode.Disabled);
			texture.SetWrap(TextureWrapMode.ClampToEdge);
			texture.SetMaxAnisotropy(0);
			image.Dispose();

			dirtyBuffer = false;

			//Console.WriteLine("buffer validated: " + renderable.commands.length);
		}
		
		public override void render(GraphicsContext context){
			if (dirtyBuffer) validateBuffer();
			if (vertexBuffer == null) return;

			program.SetUniformValue(0, ref sceneTransform);
			program.SetUniformValue(1, ref screenMatrix);
			program.SetUniformValue(2, (float) sceneAlpha);

			context.SetShaderProgram(program);
			context.SetVertexBuffer(0, vertexBuffer);
			context.SetTexture(0, texture);
			context.DrawArrays(DrawMode.TriangleFan, 0, vertexBuffer.VertexCount);
			context.SetTexture(0, null);
		}
	}
}

