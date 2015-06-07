using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utils.Path;
using GMBuildCraft.BaseClasses;

namespace GMBuildCraft
{
	class Universe:SystemPoint
	{
		/// <summary>
		/// Пути между звёздными системами
		/// </summary>
		public List<Road> Ways=new List<Road>();

		public Universe() : base(new Point(0,0))
		{}

		/// <summary>
		/// Создаём вселенную
		/// </summary>
		public void GenerateUniverse()
		{
			var rnd = new Random();
			var pts = MapGenerator.Generate(100, 800, 600, 30, 60, rnd);
			foreach (var pt in pts){
				var starSystem = new StarSystem(pt);
				starSystem.Init(MapGenerator.Generate(100, 800, 600, 30, 60, rnd), 60);
				Points.Add(starSystem);
			}

			// добавляем пути между звёздными системами
			for (int i = 0; i < Points.Count - 1; i++){
				for (int j = 1; j < Points.Count; j++){
					if (i == j) continue;
					var d = MapGenerator.Distance(Points[i], Points[j]);
					if (d > 60) continue;
					var intersect = false;
					var oneway = true;
					foreach (var way in Ways){
						if (MapGenerator.Intersect(way, Points[i],Points[j])){// пересеклись
							intersect = true;
							break;
						}
						// проверка на oneway
						if (way.NodePoint1 == Points[i] && way.NodePoint2 == Points[j]) oneway = false;
						if (way.NodePoint1 == Points[j] && way.NodePoint2 == Points[i]) oneway = false;
					}
					if (!oneway)continue;
					if (!intersect){// нету пересечений - добавляем дорогу
						var r = new Road(Points[i], Points[j]);
						Ways.Add(r);
					} 
				}
			}

			// добавляем дополнительные узловые точки для соединения с соседними звёздными системами
			var w = new List<Road>(Ways);// создаём копию
			Ways.Clear();
			foreach (var way in w){
				GenerateWayNodePoints(way);
			}


		}

		private void GenerateWayNodePoints(Road way)
		{
			// надо вычислить угол между точками и по этому вектору создать склад
			double x = way.NodePoint1.Point.X - way.NodePoint2.Point.X;
			double y = way.NodePoint1.Point.Y - way.NodePoint2.Point.Y;
			var a = Math.Atan2(y, x);

			var point1 = way.NodePoint1 as StarSystem;
			NodePoint np1 = GenerateWayNodePoints1(-a, point1);
			var point2 = way.NodePoint2 as StarSystem;
			NodePoint np2 = GenerateWayNodePoints1(a, point2);
			if (np1==null||np2==null){throw new Exception("Нету возможности создать дорогу");}
			// обе точки в обоих звёздных системах созданы - теперь формируем путь
			var w = new Road(np1, np2);// точки разделены между собой очень большим расстоянием!
			w.IsGlobal = true;
			w.CreatePath(point1.Point, point2.Point);
			Ways.Add(w);// но сам путь принадлежит вселенной, поэтому при выводе проблем не должно быть
			np1.roads.Add(w);// добавляем дороги, нужно для вывода стрелок и т.п.
			np2.roads.Add(w);
		}

		private static NodePoint GenerateWayNodePoints1(double a, StarSystem point1)
		{
			if (point1 == null) return null;
			int cX = 800/2; // предполагалось что это будет арифметический центр точек
			int cY = 600/2;
			NodePoint np1 = null;
			int c = 0;
			do{
				c++;
				int r = c*10;
				int wX = (int) (r*Math.Cos(a)) + cX;
				int wY = (int) (r*Math.Sin(a)) + cY; // смещаем к центру масс ЗС
				var wP = new Point(wX, wY);
				int i = MapGenerator.SearchNearest(wP, point1.Points, 30, 70);
				if (i >= 0){// нашли ближайший склад, смотрим какое расстояние
					int d = MapGenerator.Distance(wP, point1.Points[i].Point);
					if (d < 70){// укладываемся в диапазон - сохраняем и выходим
						np1 = point1.CreateLinkNodePoint(wP, 70);
						break;
					}
				}
			} while ((c < 1000) || (np1 != null));

			if (np1 == null){
				throw new Exception("Дополнительная узловая точка не создана");
			}
			return np1;
		}

		public override void DoWork()
		{
			base.DoWork();
			foreach (var point in Points){
				point.DoWork();
			}
			foreach (var way in Ways){
				way.Process();
			}
		}
	}
}
