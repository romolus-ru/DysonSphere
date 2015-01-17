using System;

namespace Engine.Utils.Path
{
	/// <summary>
	/// Точка с двумя координатами
	/// </summary>
	public class Point
	{
		protected bool Equals(Point other)
		{
			return X == other.X && Y == other.Y;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Point) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (X*397) ^ Y;
			}
		}

		public int X;
		public int Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Перегрузка оператора ==
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		public static Boolean operator ==(Point p1, Point p2)
		{
			// If both are null, or both are same instance, return true.
			if (System.Object.ReferenceEquals(p1, p2)){
				return true;
			}

			// If one is null, but not both, return false.
			if (((object)p1 == null) || ((object)p2 == null)){
				return false;
			} 
			if (p1.X == p2.X)
			{
				if (p1.Y == p2.Y) return true;
			}
			return false;
		}

		/// <summary>
		/// Перегрузка оператора !=
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		public static bool operator !=(Point p1, Point p2)
		{
			return !(p1 == p2);
		}
	}
}
