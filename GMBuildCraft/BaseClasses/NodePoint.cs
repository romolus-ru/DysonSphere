using System.Collections.Generic;
using Engine.Utils.Path;
using GMBuildCraft.Buildings;

namespace GMBuildCraft.BaseClasses
{
	/// <summary>
	/// Узловая точка. позволяет строить здания и склады, хранит статистическую информацию
	/// </summary>
	public class NodePoint
	{
		/// <summary>
		/// Ссылка на более крупное образование
		/// </summary>
		protected SystemPoint SystemPoint;

		/// <summary>
		/// Местоположение узловой точки
		/// </summary>
		public Point Point;

		/// <summary>
		/// Уровень узловой точки, влияет на доступность многих улучшений
		/// </summary>
		public int NodePointLevel;

		/// <summary>
		/// Уоличество доступных ресурсов у этой узловой точки
		/// </summary>
		public ResourcesPackets AvailableResources;

		/// <summary>
		/// Пути, связанные с этой узловой точкой
		/// </summary>
		public List<Road> roads;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="point"></param>
		public NodePoint(Point point)
		{
			Point = point;
			roads = new List<Road>();
		}

		/// <summary>
		/// Проделываемая точкой работа - производство ресурсов и отправка ресурсов по путям
		/// </summary>
		public virtual void DoWork()
		{
		}

		/// <summary>
		/// Установить направление
		/// </summary>
		/// <param name="resourceEnum">Ресурс</param>
		/// <param name="npTo">Куда</param>
		public void SetDirection(ResourceEnum resourceEnum, NodePoint npTo)
		{
			// пройтись по всем дорогам и если пути направлены не к точке то обнулить такой путь
			foreach (var road in roads){
				road.SetDirectionBreak(resourceEnum, npTo);// если путь содержит точку и к ней идут ресурсы - то их остановят
				road.SetDirection(resourceEnum, this, npTo);// если точки не совпадут то пропускаем, поэтому установится только у одной точки
			}
		}

	}
}
