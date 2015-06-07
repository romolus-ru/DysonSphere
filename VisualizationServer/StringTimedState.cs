using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizationServer
{
	enum StringTimedState
	{
		/// <summary>
		/// Не то
		/// </summary>
		None,
		/// <summary>
		/// То, но по времени не подходит
		/// </summary>
		Found,
		/// <summary>
		/// Найдена и обновлена
		/// </summary>
		Updated
	}
}
