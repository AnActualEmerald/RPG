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
	struct Vertex
	{
		Vector3 position;
		public Vertex(Vector3 pos)
		{
			position = pos;
		}
	}


	public class Level : IRenderable, IUpdateable
	{
		private List<Tile> Tiles;
		private Bitmap LevTexture;
		private int tex_id;
		private int array_id;
		private int vert_buffer;
		private int num_verts;
		private uint[] ind;

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
			BitmapData bits = LevTexture.LockBits(new Rectangle(0, 0, LevTexture.Width, LevTexture.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			tex_id = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, tex_id);

			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);


			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, bits.Width, bits.Height, 0,
			              OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bits.Scan0);


			GL.BindTexture(TextureTarget.Texture2D, 0);

			LevTexture.UnlockBits(bits);

			Vector3[] buff = {
				new Vector3(0, 0, 0),
				new Vector3(0, LevTexture.Width, 0),
				new Vector3(LevTexture.Height, LevTexture.Width, 0),
				new Vector3(LevTexture.Height, 0, 0)};

			ind = new uint[]{
				0, 1, 2, 0, 3, 2};


			int tex_coord;
			Vector2[] coord = {
				new Vector2(0, 0),
				new Vector2(0, 1),
				new Vector2(1, 1),
				new Vector2(1, 0)};

			num_verts = buff.Length;


			Console.WriteLine("Beginning Buffer Creation");

			GL.GenVertexArrays(1, out array_id);
			GL.GenBuffers(1, out vert_buffer);
			GL.GenBuffers(1, out ind_id);
			GL.GenBuffers(1, out tex_coord);



			GL.BindBuffer(BufferTarget.ArrayBuffer, vert_buffer);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * buff.Length), buff, BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, tex_coord);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vector2.SizeInBytes * coord.Length), coord, BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ind_id);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * ind.Length), ind, BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);


			GL.BindVertexArray(array_id);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vert_buffer);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, tex_coord);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ind_id);

			GL.BindVertexArray(0);

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

			//GL.EnableClientState(ArrayCap.VertexArray);
			//GL.EnableClientState(ArrayCap.IndexArray);
			GL.EnableClientState(ArrayCap.TextureCoordArray);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, tex_id);
			GL.BindVertexArray(array_id);

			GL.DrawElements(PrimitiveType.Triangles, ind.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindVertexArray(0);
			GL.BindTexture(TextureTarget.Texture2D, 0);
			//GL.DisableClientState(ArrayCap.VertexArray);
			//GL.DisableClientState(ArrayCap.IndexArray);

		}

		public void Update()
		{
			
		}
	}
}

