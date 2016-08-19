using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;


namespace RPG
{
	public class Camera : IUpdateable
	{
		private Vector3 position;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:RPG.Camera"/> class. All values multiplied by -1
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="z">The z coordinate.</param>
		public Camera (float x, float y, float z) 
		{
			position = new Vector3 (x * -1, y * -1, z * -1);
		}

		public Camera(Vector3 start_pos)
		{
			position = start_pos;
		}

		/// <summary>
		/// Move the specified amount on x, y and z as if moving the camera not the level.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="z">The z coordinate.</param>
		public void Move(float x, float y, float z)
		{
			position.X -= x;
			position.Y -= y;
			position.Z -= z;
		}

		public void Update()
		{
			MainClass.modelview = Matrix4.CreateTranslation (position);
		}
	}
}

