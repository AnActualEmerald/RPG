using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;


namespace RPG
{
	class MainClass
	{
		GameWindow window;
		Level[] levels;
		Level Active;
		Shader s;

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
			Console.WriteLine("Done");

		}

		private void OnResize(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, window.Width, window.Height);
		}

		private void Update(object sender, EventArgs e)
		{
			Active.Update();
		}

		private void Render(object sender, EventArgs e)
		{



			Active.Render();
			//must be last
			window.SwapBuffers();
			GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		private void ShaderInit()
		{
			s = new Shader("basic");
			s.Compile();

		}

		public static void Main (string[] args)
		{
#pragma warning disable RECS0026 // Possible unassigned object created by 'new'
			new MainClass(1280, 720, "Dicks");
#pragma warning restore RECS0026 // Possible unassigned object created by 'new'
		}


	}
}
