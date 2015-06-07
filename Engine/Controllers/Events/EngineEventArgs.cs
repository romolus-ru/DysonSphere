using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utils.ExtensionMethods;

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

		public virtual String Serialize<T>() where T:EngineEventArgs
		{
			return (this as T).SerializeObject<T>();
		}

		public virtual T Deserialize<T>(EventArgs em) where T:EngineEventArgs
		{
			var s = (em as MessageEventArgs).Message;
			return s.DeserializeObject<T>();
		}
	}
}
