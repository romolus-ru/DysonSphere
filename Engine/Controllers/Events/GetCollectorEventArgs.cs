using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utils;

namespace Engine.Controllers.Events
{
	/// <summary>
	/// Получение объекта Collector
	/// </summary>
	class GetCollectorEventArgs : EngineEventArgs
	{
		/// <summary>
		/// Коллектор
		/// </summary>
		public Collector Collector;
	}
}