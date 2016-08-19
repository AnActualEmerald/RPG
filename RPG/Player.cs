using System;
using System.Drawing;
namespace RPG
{
	public class Player : IUpdateable, IRenderable
	{
		private float x, y;
		private AABB bounds;
		private Texture tex;
		private VAO ply_vao;

		public Player(float x, float y, Bitmap texture, Level lev)
		{
			bounds = new AABB(x, y, texture.Width, texture.Height);
			tex = new Texture(texture);
			ply_vao = new VAO(texture.Width, texture.Height, false);
		}

		public void Render()
		{
			tex.Bind();
			ply_vao.Render();
			tex.UnBind();
		}

		public void Update()
		{
			ply_vao.UpdateXY((int)x, (int)y);
		}

		public void Input()
		{

		}
	}
}

