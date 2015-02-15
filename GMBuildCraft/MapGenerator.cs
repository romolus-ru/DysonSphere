using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utils.Path;
using GMBuildCraft.BaseClasses;

namespace GMBuildCraft
{
	/// <summary>
	/// Генерируется список точек по указанным условиям
	/// </summary>
	public static class MapGenerator
	{
		/// <summary>
		/// Создать дополнительную точку на основе уже существующих
		/// </summary>
		/// <param name="a"></param>
		/// <param name="points"></param>
		/// <returns></returns>
		private static Point GeneratePoint(double a, List<Point> points, int dMin, int dMax, int cX, int cY)
		{
			Point ret = null;
			int c = 0;
			int dr = (int) ((dMax - dMin)/2.0*1.5);
			do{
				c++;
				int r = c*dr;
				int x = (int) (r*Math.Cos(a)) + cX;
				int y = (int) (r*Math.Sin(a)) + cY; // смещаем к центру масс ЗС
				var p = new Point(x, y);
				int i = SearchNearest(p, points, dMin, dMax);
				if (i >= 0){// нашли ближайший склад, смотрим какое расстояние
					int d = Distance(p, points[i]);
					if (d < dMax){// укладываемся в диапазон - сохраняем и выходим
						ret = p;
						break;
					}
				}
			} while ((c < 1000) || (ret != null));
			if (ret == null){
				throw new Exception("Дополнительная точка не создана");
			}
			return ret;
		}


		public static List<Point> Generate(int countPoints, int width, int height, int minDistance, int maxDistance,
			Random rnd)
		{
			var points = new List<Point>();

			int cX = width/2;
			int cY = height/2; // добавляем первый элемент без всяких условий в центр
			points.Add(new Point(cX, cY));
			points.Add(new Point(10, 10));


			for (int i = 1; i < countPoints; i++){
				var a = rnd.Next(360);
				var p = GeneratePoint(a, points, minDistance, maxDistance, cX, cY);
				// тут может быть ошибка что p=null но она отлавливается при генерации точки
				points.Add(p);
			}
			return points;
		}

		/// <summary>
		/// Дистанция между двумя точками
		/// </summary>
		/// <param name="x1">Точка 1</param>
		/// <param name="y1">Точка 1</param>
		/// <param name="x2">Точка 2</param>
		/// <param name="y2">Точка 2</param>
		/// <returns></returns>
		public static int Distance(int x1, int y1, int x2, int y2)
		{
			int dx = x2 - x1;
			int dy = y2 - y1;
			var dist = Math.Sqrt(dx * dx + dy * dy);
			return (int)dist;
		}

		/// <summary>
		/// Дистанция между двумя точками
		/// </summary>
		/// <param name="p1">Точка 1</param>
		/// <param name="x2">Точка 2</param>
		/// <param name="y2">Точка 2</param>
		/// <returns></returns>
		public static int Distance(Point p1, int x2, int y2)
		{
			return Distance(p1.X, p1.Y, x2, y2);
		}

		/// <summary>
		/// Дистанция между двумя точками
		/// </summary>
		/// <param name="p1">Точка 1</param>
		/// <param name="p2">Точка 2</param>
		/// <returns></returns>
		public static int Distance(Point p1, Point p2)
		{
			return Distance(p1.X, p1.Y, p2.X, p2.Y);
		}

		/// <summary>
		/// Дистанция между двумя точками
		/// </summary>
		/// <param name="p1">Точка 1</param>
		/// <param name="p2">Точка 2</param>
		/// <returns></returns>
		public static int Distance(NodePoint p1, NodePoint p2)
		{
			return Distance(p1.Point, p2.Point);
		}

		/// <summary>
		/// Проверяем пересечение 2х отрезков, заданных каждый вдумя точками
		/// </summary>
		/// <param name="a1"></param>
		/// <param name="a2"></param>
		/// <param name="b1"></param>
		/// <param name="b2"></param>
		/// <returns></returns>
		public static int CheckIntersection(Point a1, Point a2, Point b1, Point b2)
		{
			/* returns original http://delphid.dax.ru/www/exampl34.htm
			   1 if there is one intersection point "c"
			   0 if chunks ar on parallel lines
			 -1 if there are no intersection points
			 */
			const float eps = 0.000001f;
			float d, da, db, ta, tb;
			d = (a1.X - a2.X)*(b2.Y - b1.Y) - (a1.Y - a2.Y)*(b2.X - b1.X);
			da = (a1.X - b1.X)*(b2.Y - b1.Y) - (a1.Y - b1.Y)*(b2.X - b1.X);
			db = (a1.X - a2.X)*(a1.Y - b1.Y) - (a1.Y - a2.Y)*(a1.X - b1.X);
			if (Math.Abs(d) < eps)return 0;// параллельны
			ta = da/d;
			tb = db/d;
			if ((0 <= ta) && (ta <= 1) && (0 <= tb) && (tb <= 1)){
				Point c = new Point((int) (a1.X + ta*(a2.X - a1.X)), (int) (a1.Y + ta*(a2.Y - a1.Y)));
				return 1;// пересекаются
			}
			return -1;// не пересекаются
		}

		public static Boolean Intersect(Road r1, NodePoint r2np1, NodePoint r2np2)
		{
			if (r1.NodePoint1 == r2np1) return false;// если точки совпадают, то значит у них общие узловые точки
			if (r1.NodePoint1 == r2np2) return false;// а это означает что просто пути идут из одной точки
			if (r1.NodePoint2 == r2np1) return false;// и это значит что они не могут пересекаться
			if (r1.NodePoint2 == r2np2) return false;
			var a = CheckIntersection(r1.NodePoint1.Point, r1.NodePoint2.Point, r2np1.Point, r2np2.Point);
			return a == 1;
		}

		public static Boolean Intersect(Road r1, Road r2)
		{
			return Intersect(r1, r2.NodePoint1, r2.NodePoint2);
		}

		/// <summary>
		/// Найти ближайшую точку (для списка из узловых точек)
		/// </summary>
		/// <param name="p"></param>
		/// <param name="pts"></param>
		/// <returns></returns>
		public static int SearchNearest(Point p, List<NodePoint> pts, int distMin, int distMax)
		{
			int id = -1;
			int dist = Int32.MaxValue;
			for (int i = 0; i < pts.Count; i++){
				int d = Distance(pts[i].Point, p);
				if (d < distMin){id = -1;break;}// слишком близко
				if (d > distMax) continue;// далеко, но может быть найдём поближе что-нибудь
				if (d < dist){dist = d;id = i;}
			}
			return id;
		}

		/// <summary>
		/// Найти ближайшую точку (для списка из точек)
		/// </summary>
		/// <param name="p"></param>
		/// <param name="pts"></param>
		/// <returns></returns>
		public static int SearchNearest(Point p, List<Point> pts, int distMin, int distMax)
		{
			int id = -1;
			int dist = Int32.MaxValue;
			for (int i = 0; i < pts.Count; i++){
				int d = Distance(pts[i], p);
				if (d < distMin) { id = -1; break; }// слишком близко
				if (d > distMax) continue;// далеко, но может быть найдём поближе что-нибудь
				if (d < dist) { dist = d; id = i; }
			}
			return id;
		}

	}
}
