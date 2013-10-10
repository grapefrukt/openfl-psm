using System;
using Sce.PlayStation.Core.Graphics;
using com.grapefrukt.games.spaceblocks;

namespace psm.display
{
	public class RendererStartup : RendererTextured
	{
		
		private VertexBuffer vertexBuffer;
		private Texture2D texture;
		
		public RendererStartup (Sprite sprite) : base(sprite){
			float[] verts = new float[8];
			float[] uvs = new float[8];
			
			texture = new Texture2D("/Application/assets/bootscreen/Vita.png", false);
			texture.SetFilter(TextureFilterMode.Disabled);
			texture.SetWrap(TextureWrapMode.ClampToEdge);
			texture.SetMaxAnisotropy(0);
			
			int offsetX = (Settings.STAGE_W - texture.Width) / 2;
			int offsetY = (Settings.STAGE_H - texture.Height) / 2;

			verts[0] = (float) offsetX;
			verts[1] = (float) offsetY;

			verts[2] = (float) offsetX;
			verts[3] = (float) offsetY + texture.Height;

			verts[6] = (float) offsetX + texture.Width;
			verts[7] = (float) offsetY;

			verts[4] = (float) offsetX + texture.Width;
			verts[5] = (float) offsetY + texture.Height;

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
			
		}
		
		public override void render(GraphicsContext context){
			if (vertexBuffer == null) return;

			program.SetUniformValue(0, ref sceneTransform);
			program.SetUniformValue(1, ref screenMatrix);
			program.SetUniformValue(2, 1f);

			context.SetShaderProgram(program);
			context.SetVertexBuffer(0, vertexBuffer);
			context.SetTexture(0, texture);
			context.DrawArrays(DrawMode.TriangleFan, 0, vertexBuffer.VertexCount);
			context.SetTexture(0, null);
		}
	}
}

