using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Views;

namespace Engine.Controllers.Events
{
	// TODO возможно не нужен
	/// <summary>
	/// Параметр для работы с объектом вида
	/// </summary>
	/// <remarks>В основном содержит ссылку на сам объект-вид 
	/// и информацию о слое (нужно для добавления)</remarks>
	public class ViewObjectEventArgs : EventArgs
	{
		/// <summary>
		/// Сообщение
		/// </summary>
		public ViewObject ViewObject { private set; get; }

		/// <summary>
		/// Отправить объект визуализации
		/// </summary>
		/// <param name="viewObject">Объект визуализации</param>
		/// <returns></returns>
		static public ViewObjectEventArgs vObj(ViewObject viewObject)
		{
			var v = new ViewObjectEventArgs();
			v.ViewObject = viewObject;
			return v;
		}


	}
}
