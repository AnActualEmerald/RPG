using System;
using System.Drawing;
namespace RPG
{
	public class AABB
	{
		private RectangleF _rect;

		public AABB(float x, float y, float width, float height)
		{
			_rect = new RectangleF(x, y, width, height);
		}

		public AABB(RectangleF r)
		{
			_rect = r;
		}

		public bool CheckCollision(AABB other)
		{
			return _rect.IntersectsWith(other.Rect);
		}

		#region members
		public RectangleF Rect{
			get { return _rect; }
			set { _rect = value; }
		}

		public float X
		{
			get { return _rect.X; }
			set { _rect.X = value; }
		}

		public float Y
		{
			get { return _rect.Y; }
			set { _rect.Y = value; }
		}
		#endregion
	}
}

