using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Controllers.Events
{
	/// <summary>
	/// Получить обработчик события
	/// </summary>
	/// <remarks>Используется в Application для получения 
	/// функции главного таймера и для отключения таймера
	/// После вызова подразумевается, что объект, который 
	/// получит метод, будет его запускать сам</remarks>
	public class GetHandlerEventArgs : EngineEventArgs
	{
		/// <summary>
		/// Обработчик передаваемый через событие
		/// </summary>
		public EventHandler EH { get; private set; }

		/// <summary>
		/// Установить обработчик
		/// </summary>
		/// <param name="eh"></param>
		public void Set(EventHandler eh)
		{
			EH = eh;
		}

		/// <summary>
		/// Создаёт пустое событие
		/// </summary>
		/// <returns></returns>
		static public GetHandlerEventArgs Create()
		{
			var tmp = new GetHandlerEventArgs();
			tmp.EH = null;
			return tmp;
		}
	}
}
