using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace RPG
{
	class MainClass
	{
		GameWindow window;
		Level[] levels;
		Level Active;
		Shader s;

		Matrix4 projection;
		public static Matrix4 modelview;



		public MainClass(int width, int height, string title)
		{
			window = new GameWindow(width, height, GraphicsMode.Default, title);
			window.VSync = VSyncMode.Off;
			window.RenderFrame += Render;
			window.UpdateFrame += Update;
			window.Resize += OnResize;
			window.Load += Init;
			window.Run(60);
		}

		private void Init(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, window.Width, window.Height);
			levels = new Level[1];
			levels[0] = Level.BuildNewLevel("./res/levels/test.png");
			Active = levels[0];
			ShaderInit();
			s.Use();
		
			Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, -1, 1, out projection);

			GL.UniformMatrix4(s.GetUniformLocation("projection"), false, ref projection);
			GL.UniformMatrix4(s.GetUniformLocation("model_view"), false, ref modelview);

			GL.ClearColor(Color4.AliceBlue);
			Console.WriteLine("Done");

		}

		private void OnResize(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, window.Width, window.Height);

			Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, -1, 1, out projection);

			GL.UniformMatrix4(s.GetUniformLocation("projection"), false, ref projection);

		}


		private void Update(object sender, EventArgs e)
		{
			Active.Update();
			GL.UniformMatrix4(s.GetUniformLocation("model_view"), false, ref modelview);
		}

		private void Render(object sender, EventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
	
			Active.Render();

			//must be last
			window.SwapBuffers();

		}

		private void ShaderInit()
		{
			s = new Shader("basic");
			s.Compile();

		}

		public static void Main (string[] args)
		{
#pragma warning disable RECS0026 // Possible unassigned object created by 'new'
			new MainClass(800, 600, "Dicks");
#pragma warning restore RECS0026 // Possible unassigned object created by 'new'
		}


	}
}
