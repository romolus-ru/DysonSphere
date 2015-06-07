using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;

namespace Engine.Controllers.Net
{
	/// <summary>
	/// Отправка событий клиентам
	/// </summary>
	/// <remarks>По примеру видно, что сервер работает постоянно, не требует поддержки соединения</remarks>
	class DataSenderServer:DataSender
	{
		private Server _server;
		private Controller _controller;

		public DataSenderServer(Controller controller, string acceptedEvent, int port) : base(controller, acceptedEvent)
		{
			_controller = controller;
			_server=new Server();
			_server.GetRecieved += Recieve;
			_server.PrintNetDebug = PrintNetDebug;
			_server.Start(port);
		}

		public override void Dispose()
		{
			base.Dispose();
			_server.Stop();
		}

		public override void Send(DataRecieveEventArgs dr)
		{
			// отправляем клиентам
			//PrintNetDebug("Server send " + dr.EventName + "+" + dr.DataString);
			_server.SendToAll(dr.EventName+"+"+dr.DataString);
		}

		//public override void Recieve(string data)
		//{
		//	base.Recieve(data);// вызов ничем не отличается - получили строку и надо её разделить
		//}
	}
}
