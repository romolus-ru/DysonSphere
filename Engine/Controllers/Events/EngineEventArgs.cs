using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Controllers.Events
{
	/// <summary>
	/// EventArgs, обогащающий стандартный eventArgs управляющими возможностями
	/// </summary>
	public class EngineEventArgs:EventArgs
	{
		/// <summary>
		/// Признак обработанности события. Если тру - событие больше не обрабатывается
		/// </summary>
		public Boolean Handled = false;
	}
}
