using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace RPG
{
	public class Level : IRenderable, IUpdateable
	{
		private List<Tile> Tiles;
		private Bitmap LevTexture;
		private int tex_id;
		private int array_id;

		public Level()
		{
			Tiles = new List<Tile>();
		}

		public void AddTile(Tile t)
		{
			Tiles.Add(t);
		}

		public void InitTexture()
		{
			BitmapData bits = LevTexture.LockBits(new Rectangle(0, 0, LevTexture.Width, LevTexture.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			tex_id = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, tex_id);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, LevTexture.Width, LevTexture.Height, 0,
			              OpenTK.Graphics.OpenGL4.PixelFormat.Rgb, PixelType.UnsignedByte, bits.Scan0);
			GL.BindTexture(TextureTarget.Texture2D, 0);

			float[] buff = new float[]{
				0.0f, 0.0f,
				 LevTexture.Width, 0.0f,
				 LevTexture.Width, LevTexture.Height,
				 0.0f, LevTexture.Height};



			array_id = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, array_id);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)buff.Length, buff, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

		}

		public static Level BuildNewLevel(string path)
		{
			Level nl = new Level();
			Bitmap lev = new Bitmap(path);
			for (int x = 0; x < lev.Width; x++)
				for (int y = 0; y < lev.Height; y++)
				{
					Color r = lev.GetPixel(x, y);
					switch (r.Name)
					{
						case "ff0000ff":
							nl.AddTile(new Tile(x, y, Tile_Type.WALL, new Bitmap(Tile.WATER_TEX)));
							break;
						case "ffffcc00":
							nl.AddTile(new Tile(x, y, Tile_Type.GRASS, new Bitmap(Tile.SAND_TEX)));
							break;
						case "ff00d723":
							nl.AddTile(new Tile(x, y, Tile_Type.GRASS, new Bitmap(Tile.ROAD_TEX)));
							break;
						default:
							Console.Write("Not an expected color");
							break;
					}
				}

			nl.LevTexture = new Bitmap(lev.Width * Tile.TILE_SIZE, lev.Height * Tile.TILE_SIZE);
			Graphics g = Graphics.FromImage(nl.LevTexture);
			foreach (Tile t in nl.Tiles)
			{
				g.DrawImage(t.Texture, t.Position.X*Tile.TILE_SIZE, t.Position.Y*Tile.TILE_SIZE);
			}
			g.Dispose();

			return nl;
		}

		public void Render()
		{
			//GL.BindTexture(TextureTarget.Texture2D, tex_id);
			GL.BindBuffer(BufferTarget.ArrayBuffer, array_id);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public void Update()
		{
			
		}
	}
}

