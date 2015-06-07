			//от сервера должно сообщение приходить всем
			//от клиента должно приходить сообщение только серверу, и не отсылаться обратно
			//а вот присланное сообщение сервер может послать всем, но это уже другая историяusing System;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;
using Engine.Utils.ExtensionMethods;

namespace Engine.Controllers
{
	/// <summary>
	/// Делегат для обмена строками
	/// </summary>
	public delegate void GetStringDelegate(string str);

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
	public class DataSender:IDisposable
	{
		private readonly Controller _controller;
		
		/// <summary>
		/// Получаемые события, которые надо отправить, например "FromServerToClient"
		/// </summary>
		protected string AcceptedEvent;

		public DataSender(Controller controller,String acceptedEvent)
		{
			_controller = controller;
			AcceptedEvent = acceptedEvent;
			_controller.AddEventHandler(AcceptedEvent, AcceptEvent);
		}

		protected void AcceptEvent(object sender, EventArgs e)
		{
			var m = e as MessageEventArgs;
			if (m != null){
				PrintNetDebug(this.GetType().FullName);
			}
			Send(e as DataRecieveEventArgs);
		}

		/// <summary>
		/// Отправляем событие
		/// </summary>
		/// <param name="dr"></param>
		public virtual void Send(DataRecieveEventArgs dr)
		{
			// в данном случае - получаем событие и сразу же отправляем его дальше
			// и обходимся без десериализации, до recieve дело не доходит
			PrintNetDebug("DATASender send " + dr.EventName + "+" + dr.DataString);
			Recieve(dr.EventName+"+"+ dr.DataString);
			//_controller.StartEvent(dr.EventName,null,MessageEventArgs.Msg(dr.DataString));
		}

		/// <summary>
		/// Получаем событие из сети и сразу же его отправляем в местную систему
		/// </summary>
		public virtual void Recieve(string data)
		{
			var eventName = "PrintNetDebug2";
			var eventData = data;
			var p = data.IndexOf('+');
			if (p >= 0){
				eventName = data.Substring(0, p);
				eventData = data.Substring(p + 1);
			}
			//PrintNetDebug(this.GetType().FullName + " Recieve " + eventName + " " + data);
			_controller.StartEvent(eventName, null, MessageEventArgs.Msg(eventData));
		}

		public virtual void Dispose()
		{
			_controller.RemoveEventHandler(AcceptedEvent, AcceptEvent);
		}

		public void PrintNetDebug(string str)
		{
			_controller.StartEvent("PrintNetDebug", null, MessageEventArgs.Msg(str));
			//Debug.Print("> "+str);
		}
	}
}
