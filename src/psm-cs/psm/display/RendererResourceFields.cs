using System;
using Sce.PlayStation.Core.Graphics;
using com.grapefrukt.games.spaceblocks.grid;
using Sce.PlayStation.Core;
using com.grapefrukt.games.spaceblocks;

namespace psm.display
{
	public class RendererResourceFields : RendererUniversal
	{
		private static ShaderProgram program;
		private static VertexBuffer vertexBuffer;
		private System.Collections.Generic.List<CellData> cells;
		
		private const int CELL_SIZE = 45;
		
		public RendererResourceFields (Sprite sprite) : base(sprite) {
			cells = new System.Collections.Generic.List<CellData>();
		}
		
		new public static void init() {
			if(program == null) {
				program = new ShaderProgram("/Application/shaders/Resource.cgx");
				
				program.SetUniformBinding(0, "u_SceneMatrix");
				program.SetUniformBinding(1, "u_ScreenMatrix");
				program.SetUniformBinding(2, "u_Time");
				program.SetUniformBinding(3, "u_Energy");
				program.SetUniformBinding(4, "u_Position");
				
				int[] cellIndices = new int[21];
				for (int i = 0; i < 21; i++) {
					cellIndices[i] = i;
				}
				
				var rnd = new System.Random();
				
				int n = cellIndices.Length;
				while (n > 1) {
					int k = (int)(rnd.NextDouble() * (n--));
					int temp = cellIndices[n];
					cellIndices[n] = cellIndices[k];
					cellIndices[k] = temp;
				}
				
				float[] verts = new float[5 * 5 * 6 * 4];
				int vi = 0;
				float subcellSize = (float) CELL_SIZE / 5f;
				int count = 0;
				
				for (int i = 0; i < 25; i++) {
					if ( i == 0 || i == 4 || i == 20 || i == 24) continue;
					
					float phase = (float) (rnd.NextDouble() * Math.PI * 2);
					int x = i % 5;
					int y = (int) System.Math.Floor((double) i/5);
					
					verts[vi++] = x * subcellSize;
					verts[vi++] = y * subcellSize;
					verts[vi++] = phase;
					verts[vi++] = cellIndices[count];
					
					verts[vi++] = x * subcellSize + subcellSize;
					verts[vi++] = y * subcellSize;
					verts[vi++] = phase;
					verts[vi++] = cellIndices[count];
					
					verts[vi++] = x * subcellSize;
					verts[vi++] = y * subcellSize + subcellSize;
					verts[vi++] = phase;
					verts[vi++] = cellIndices[count];
					
					verts[vi++] = x * subcellSize + subcellSize;
					verts[vi++] = y * subcellSize;
					verts[vi++] = phase;
					verts[vi++] = cellIndices[count];
					
					verts[vi++] = x * subcellSize + subcellSize;
					verts[vi++] = y * subcellSize + subcellSize;
					verts[vi++] = phase;
					verts[vi++] = cellIndices[count];
					
					verts[vi++] = x * subcellSize;
					verts[vi++] = y * subcellSize + subcellSize;
					verts[vi++] = phase;
					verts[vi++] = cellIndices[count];
					
					count++;					
				}
				
				vertexBuffer = new VertexBuffer(5 * 5 * 6, VertexFormat.Float4);
				vertexBuffer.SetVertices(verts);
				
			}	
		}
			
		public void renderCell(int x, int y, int val, double time) {
			cells.Add(new CellData(x, y, val, time));
		}
		
		public void clearCells() {
			cells.Clear();
		}
		
		
		private static Vector2 pos = new Vector2();
		override public void render(GraphicsContext context){
			base.render(context);
			
			program.SetUniformValue(1, ref screenMatrix);
			program.SetUniformValue(0, ref sceneTransform);
				
			context.SetShaderProgram(program);
			context.SetVertexBuffer(0, vertexBuffer);
			
			foreach ( CellData cell in cells ){
				program.SetUniformValue(3, (float) cell.val);
				program.SetUniformValue(2, (float) cell.time);
				pos.X = cell.x * (CELL_SIZE + 5) - CELL_SIZE * .5f;
				pos.Y = cell.y * (CELL_SIZE + 5) - CELL_SIZE * .5f;
				program.SetUniformValue(4, ref pos);
				context.DrawArrays(DrawMode.Triangles, 0, vertexBuffer.VertexCount);
			}
		}
	}
	
	public struct CellData {
		public int x, y, val;
		public double time;
				
		public CellData(int x, int y, int val, double time){
			this.x = x;
			this.y = y;
			this.val = val;
			this.time = time;
		}
	}
}