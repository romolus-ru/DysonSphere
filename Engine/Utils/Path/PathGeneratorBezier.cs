using System.Collections.Generic;

namespace Engine.Utils.Path
{
	/// <summary>
	/// Генератор пути безье
	/// </summary>
	public class PathGeneratorBezier:PathGenerator
	{
		public override List<Point> Generate(List<Point> basePoints, int count)
		{
			if (basePoints.Count != 4) return base.Generate(basePoints, count);
			return GenerateBezierPath(basePoints[0], basePoints[1], basePoints[2], basePoints[3], count);
		}

		/// <summary>
		/// Генератору пути безье требуется 4 опорные точки
		/// </summary>
		/// <returns></returns>
		public override int CountBasePoints()
		{
			return 4;
		}

		/// <summary>
		/// Генерация пути безье по четырём опорным точкам
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="p3"></param>
		/// <param name="p4"></param>
		/// <param name="count"></param>
		public List<Point> GenerateBezierPath(Point p1, Point p2, Point p3, Point p4,int count)
		{
			List<Point> _points = new List<Point>();
			float t = 0;
			float dt = (float)(1.0 / count);
			for (int i = 0; i <= count; i++){
				var x = (int)bezier_spline(t, p1.X, p2.X, p3.X, p4.X);
				var y = (int)bezier_spline(t, p1.Y, p2.Y, p3.Y, p4.Y);
				_points.Add(new Point(x, y));
				t += dt;
			}
			return _points;
		}

		private float bezier_spline(float t, int p1, int p2, int p3, int p4)
		{
			var mu = t;
			var mum1 = 1 - mu;
			var mum13 = mum1 * mum1 * mum1;
			var mu3 = mu * mu * mu;

			var q = mum13 * p1 + 3 * mu * mum1 * mum1 * p2 + 3 * mu * mu * mum1 * p3 + mu3 * p4;
			return q;
		}

	}
}
