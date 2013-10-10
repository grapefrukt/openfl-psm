using System;
using psm.text;
using Sce.PlayStation.Core.Graphics;

namespace psm.display
{
	public class RendererTextured : Renderer
	{
		protected static ShaderProgram program;
		
		public RendererTextured (Sprite sprite) : base(sprite){
			
		}
			
		public static void init(){
			if (program == null){
				program = new ShaderProgram("/Application/shaders/Textured.cgx");
				program.SetUniformBinding(0, "u_SceneMatrix");
				program.SetUniformBinding(1, "u_ScreenMatrix");
				program.SetUniformBinding(2, "u_Alpha");
				program.SetAttributeBinding(0, "a_Position");
				program.SetAttributeBinding(1, "a_TexCoord");
			}
		}
		
	}
}

