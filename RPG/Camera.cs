using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;


namespace RPG
{
	public class Camera : IUpdateable
	{
		private Vector3 position;

		public Camera (float x, float y, float z) 
		{
			position = new Vector3 (x, y, z);
		}

		public Camera(Vector3 start_pos)
		{
			position = start_pos;
		}

		public void Move(float x, float y, float z)
		{
			position.X += x;
			position.Y += y;
			position.Z += z;
		}

		public void Update()
		{
			MainClass.modelview = Matrix4.CreateTranslation (position);
		}
	}
}

