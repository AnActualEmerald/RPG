using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace RPG
{
	public class Shader
	{
		byte[] frag;
		byte[] vert;
		int prog;

		public Shader(string name)
		{
			FileStream stream = new FileStream("./res/shaders/" + name + ".vt", FileMode.Open);
			vert = new byte[stream.Length];
			stream.Read(vert, 0, (int)stream.Length);
			stream.Close();

			stream = new FileStream("./res/shaders/" + name + ".fg", FileMode.Open);
			frag = new byte[stream.Length];
			stream.Read(frag, 0, (int)stream.Length);
			stream.Close();
		}

		public int UniformLocation(string name)
		{
			return GL.GetUniformLocation(prog, name);
		}

		public Shader Compile()
		{
			int frg = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(frg, Encoding.ASCII.GetString(frag));
			GL.CompileShader(frg);

			Console.WriteLine(GL.GetShaderInfoLog(frg));

			int vrt = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vrt, Encoding.ASCII.GetString(vert));
			GL.CompileShader(vrt);

			Console.WriteLine(GL.GetShaderInfoLog(vrt));

			prog = GL.CreateProgram();
			GL.AttachShader(prog, vrt);
			GL.AttachShader(prog, frg);
			GL.LinkProgram(prog);

			GL.DeleteShader(vrt);
			GL.DeleteShader(frg);

			return this;
		}

		public void Use()
		{
			GL.UseProgram(prog);
		}
	}
}

