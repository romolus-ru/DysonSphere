using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;
using Engine.Utils.ExtensionMethods;

namespace Engine.Controllers
{
	/// <summary>
	/// Отправитель данных
	/// </summary>
	/// <remarks>
	/// Отправляются текстовые данные. В datarecieveEventArgs предусмотрена возможность сериализовать данные, что бы меньше передавать
	/// Но пока от этого отказался - во первых tcp зипуется сам (или это можно включить) во вторых данных пока передаётся не так много
	/// От клиента к северу точно будет мало идти данных
	/// Отправляет полученное сообщение. 1 объект работает за сервер, второй - за клиента
	/// Или разделение такое - ViewDataSender - отправляет события с view на model
	/// ModelDataSender - отправляет события с model на view
	/// В общем случае каждый из них отправляет данные через сеть
	/// 
	/// кое какая информация там https://msdn.microsoft.com/en-us/library/w89fhyex(v=vs.110).aspx
	/// </remarks>
	class DataSender
	{
		private readonly Controller _controller;
		
		/// <summary>
		/// Получаемые события, которые надо отправить, например "FromServerToClient"
		/// </summary>
		private string _acceptedEvent;

		public DataSender(Controller controller,String acceptedEvent)
		{
			_controller = controller;
			_acceptedEvent = acceptedEvent;
			_controller.AddEventHandler(_acceptedEvent, AcceptEvent);
		}

		private void AcceptEvent(object sender, EventArgs e)
		{
			Send(e as DataRecieveEventArgs);
		}

		public void Send(DataRecieveEventArgs dr)
		{
			// в данном случае - получаем событие и сразу же отправляем его дальше
			// и обходимся без десериализации
			_controller.StartEvent(dr.EventName,null,MessageEventArgs.Msg(dr.DataString));
		}

	}
}
