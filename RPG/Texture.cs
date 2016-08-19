using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace RPG
{
	public class Texture
	{

		private int tex_id;

		/// <summary>
		/// Fully initializes the texture with the given data and generates an ID
		/// </summary>
		/// <param name="data">Data.</param>
		public Texture(Bitmap data)
		{
			BitmapData bits = data.LockBits(new Rectangle(0, 0, data.Width, data.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
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

			data.UnlockBits(bits);
		}

		public void Bind()
		{
			GL.BindTexture(TextureTarget.Texture2D, tex_id);
		}

		public void UnBind()
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}


		public int ID
		{
			get { return tex_id;}
		}
	}

	public class VAO : IRenderable
	{
		private int array_id;
		private int ind_id;
		private int vert_buffer;
		private int num_verts;
		private uint[] ind;
		private int width, height;

		private Vector3[] buff;

		/// <summary>
		/// Creates complete VAO for any rectangular entity at 0,0
		/// </summary>
		public VAO(int Width, int Height, bool is_static)
		{
			width = Width;
			height = Height;

			buff = new Vector3[]{
				new Vector3(0, 0, 0),
				new Vector3(0, Width, 0),
				new Vector3(Height, Width, 0),
				new Vector3(Height, 0, 0)};

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

			BufferUsageHint hint = is_static ? BufferUsageHint.StaticDraw : BufferUsageHint.DynamicDraw;

			GL.BindBuffer(BufferTarget.ArrayBuffer, vert_buffer);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * buff.Length), buff, hint);

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
		}

		/// <summary>
		/// Use this if you need to change the x and/or y of the VAO
		/// </summary>
		public void UpdateXY(int x, int y)
		{
			buff[0] = new Vector3(x, y, 0);
			buff[1] = new Vector3(x, y + height, 0);
			buff[2] = new Vector3(x + width, y + height, 0);
			buff[3] = new Vector3(x + width, y, 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vert_buffer);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * buff.Length), buff, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public void Render()
		{
			GL.BindVertexArray(array_id);

			GL.DrawElements(PrimitiveType.Triangles, ind.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindVertexArray(0);
		}

		public int Width
		{
			get { return width;}
			set { width = value;}
		}

		public int Height
		{
			get { return height; }
			set { height = value; }
		}
	}
}

