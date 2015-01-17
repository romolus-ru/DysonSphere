using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils.Path
{
	/// <summary>
	/// Генератор пути. Получает 2 точки (или больше) и выдаёт заданное количество точек соответствующих пути
	/// </summary>
	/// <remarks>В целом можно сделать добавку для создания пути с отступом, такая задача очень может быть востребована</remarks>
	public class PathGenerator
	{
		/// <summary>
		/// Генератор пути
		/// </summary>
		/// <param name="basePoints">список опорных точек</param>
		/// <param name="count">Количество получаемых точек</param>
		/// <returns></returns>
		public virtual List<Point> Generate(List<Point> basePoints, int count)
		{
			return null;
		}

		/// <summary>
		/// Количество точек, требуемых для построения пути этм генератором
		/// </summary>
		/// <returns></returns>
		public virtual int CountBasePoints()
		{
			return 2;
		}
	}
}
