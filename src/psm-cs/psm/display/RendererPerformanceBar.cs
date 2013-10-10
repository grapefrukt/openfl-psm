using System;
using Sce.PlayStation.Core.Graphics;
using com.grapefrukt.games.spaceblocks;
using Sce.PlayStation.Core;

namespace psm.display
{
	public class RendererPerformanceBar : RendererUniversal
	{
		
		private VertexBuffer vertexBuffer;
				
		public RendererPerformanceBar (Sprite sprite) : base(sprite) {
			float[] verts = new float[8];
			float[] colors = new float[16];
			
			verts[0] = (float) 0;
			verts[1] = (float) 0;

			verts[2] = (float) 0;
			verts[3] = (float) 1;

			verts[6] = (float) 1;
			verts[7] = (float) 0;

			verts[4] = (float) 1;
			verts[5] = (float) 1;
			
			int i = 0;
			colors[i++] = 1; colors[i++] = 1; colors[i++] = 1; colors[i++] = 1;
			colors[i++] = 1; colors[i++] = 1; colors[i++] = 1; colors[i++] = 1;
			colors[i++] = 1; colors[i++] = 1; colors[i++] = 1; colors[i++] = 1;
			colors[i++] = 1; colors[i++] = 1; colors[i++] = 1; colors[i++] = 1;

			
			vertexBuffer = new VertexBuffer(4, VertexFormat.Float2, VertexFormat.Float4);
			vertexBuffer.SetVertices(0, verts);
			vertexBuffer.SetVertices(1, colors);
		}
		
		private Vector3 barPos = new Vector3(0, 0, 0);
		private Vector3 barScale = new Vector3(1, 2, 1);
		public void renderBar(float time, float offsetX, float offsetY, GraphicsContext context) {
			barScale.X = time;
			barPos.X = offsetX;
			barPos.Y = offsetY;
			sceneTransform = Matrix4.Transformation(barPos, barScale);
			render(context);
		}
		
		public override void render(GraphicsContext context){
			shaderColored.SetUniformValue(0, ref sceneTransform);
			shaderColored.SetUniformValue(1, ref screenMatrix);
			shaderColored.SetUniformValue(2, 1f);

			context.SetShaderProgram(shaderColored);
			context.SetVertexBuffer(0, vertexBuffer);
			context.DrawArrays(DrawMode.TriangleFan, 0, vertexBuffer.VertexCount);
		}
	}
}

