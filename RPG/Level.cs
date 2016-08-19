using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Runtime.InteropServices;

namespace RPG
{

	public class Level : IRenderable, IUpdateable
	{
		private List<Tile> Tiles;
		private Bitmap LevTexture;
		private Texture tex;
		private VAO lev_vao;

		public Level()
		{
			Tiles = new List<Tile>();
		}

		public void AddTile(Tile t)
		{
			Tiles.Add(t);
		}

		int ind_id;
		public void InitTexture()
		{

			tex = new Texture(LevTexture);

			lev_vao = new VAO(LevTexture.Width, LevTexture.Height, true);

			Console.WriteLine("Done");

			ErrorCode e = GL.GetError();
			Console.WriteLine(e);
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
			nl.InitTexture();
			return nl;
		}

		public void Render()
		{
			tex.Bind();
			lev_vao.Render();
			tex.UnBind();
		}

		public void Update()
		{
			
		}
	}
}

