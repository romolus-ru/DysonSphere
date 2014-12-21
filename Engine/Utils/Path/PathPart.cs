using System.Collections.Generic;

namespace Engine.Utils.Path
{
	// TODO возможно, лишний класс.
	/// <summary>
	/// Часть пути, определяемая двумя точками и генератором пути
	/// </summary>
	class PathPart
	{
		public List<Point> BasePoints { get; protected set; }

		public int Count { get; protected set; }

		public PathGenerator PathGenerator { get; protected set; }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="basePoints">Опорные точки</param>
		/// <param name="count">Количество требуемых точек на выходе</param>
		/// <param name="pathGenerator">Генератор пути</param>
		public PathPart(List<Point> basePoints, int count, PathGenerator pathGenerator)
		{
			BasePoints = basePoints;
			Count = count;
			PathGenerator = pathGenerator;
		}

	}
}
