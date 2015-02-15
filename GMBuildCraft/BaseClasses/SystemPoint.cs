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
	/// Точка системы. хранит NodePoint и организует иерархию
	/// </summary>
	public class SystemPoint:NodePoint
	{
		public SystemPoint(Point point) : base(point)
		{}

		/// <summary>
		/// Точки, входящие в состав точки системы
		/// </summary>
		public List<NodePoint> Points = new List<NodePoint>();

	}
}
