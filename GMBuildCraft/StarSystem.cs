using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utils.Path;
using GMBuildCraft.BaseClasses;
using GMBuildCraft.Buildings;

namespace GMBuildCraft
{
	/// <summary>
	/// Звёздная система. содержит связывающие ссылки на все нужные глобальные объекты
	/// </summary>
	public class StarSystem:SystemPoint
	{
		/// <summary>
		/// Список дорог
		/// </summary>
		public List<Road>  Roads=new List<Road>();

		public StarSystem(Point point) : base(point)
		{}

		/// <summary>
		/// Инициализируем систему переданными координатами точек
		/// </summary>
		/// <param name="pts"></param>
		/// <param name="dMax">Максимальная длина дороги</param>
		public void Init(List<Point> pts, int dMax)
		{
			foreach (var pt in pts){
				var np = new StarPoint(pt, this);
				Points.Add(np);
			}
			// добавляем дороги
			for (int i = 0; i < Points.Count - 1; i++){
				for (int j = 1; j < Points.Count; j++){
					if (i == j) continue;
					var d = MapGenerator.Distance(Points[i], Points[j]);
					if (d>dMax)continue;
					var r = new Road(Points[i], Points[j]);
					r.CreatePath(Points[i].Point, Points[j].Point);
					var intersect = false;
					var oneway = true;
					foreach (var road in Roads){
						if (MapGenerator.Intersect(road, r)){// пересеклись
							intersect = true;
							break;
						}
						// проверка на oneway
						if (road.NodePoint1 == Points[i] && road.NodePoint2 == Points[j]) oneway = false;
						if (road.NodePoint1 == Points[j] && road.NodePoint2 == Points[i]) oneway = false;
					}
					if (!oneway) continue;
					if (!intersect) {
						Roads.Add(r);// нету пересечений - добавляем дорогу
						r.NodePoint1.roads.Add(r);// добавляем дороги к точке пути
						r.NodePoint2.roads.Add(r);
					}
				}
			}
		}

		/// <summary>
		/// Создать узловую точку, на которой будет расположено здание для связи между зёздными системами
		/// </summary>
		public NodePoint CreateLinkNodePoint(Point p, int dMax)
		{
			var np = new StarPoint(p, this);
			np.BuildingType = BuildingType.StorageUniverse;
			np.Building = new Building();
			// добавляем дороги
			for (int i = 0; i < Points.Count; i++){
				var d = MapGenerator.Distance(Points[i], np);
				if (d > dMax) continue;
				var r = new Road(Points[i], np);
				r.CreatePath(Points[i].Point, np.Point);

				var intersect = false;
				foreach (var road in Roads){
					if (MapGenerator.Intersect(road, r)){// пересеклись
						intersect = true;
						break;
					}
				}
				if (!intersect) Roads.Add(r); // нету пересечений - добавляем дорогу
			}
			Points.Add(np);
			return np;
		}

		public override void DoWork()
		{
			base.DoWork();
			foreach (var point in Points){
				point.DoWork();
			}
			foreach (var road in Roads){
				road.Process();
			}
		}
	}
}
