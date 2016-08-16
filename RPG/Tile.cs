using System;
using System.Drawing;
using OpenTK;

namespace RPG
{
	public class Tile
	{

		public const string WATER_TEX = "./res/tiles/water.png";
		public const string ROAD_TEX = "./res/tiles/road.png";
		public const string SAND_TEX = "./res/tiles/sand.png";

		public const int TILE_SIZE = 64;
		private Vector2 _position;
		private Tile_Type _type;
		private Bitmap _texture;

		public Tile(int x, int y, Tile_Type t, Bitmap texture)
		{
			_position = new Vector2(x, y);
			_type = t;
			_texture = texture;
		}

		public Vector2 Position
		{
			get { return _position;}
			set { _position = value;}
		}

		public Tile_Type T_Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public Bitmap Texture
		{
			get { return _texture; }
			set { _texture = value; }
		}

		public void Render()
		{
			
		}
	}

	public enum Tile_Type
	{
		GRASS,
		ROAD,
		WALL
	}


}

