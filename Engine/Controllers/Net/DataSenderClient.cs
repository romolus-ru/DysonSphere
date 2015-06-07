using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using Engine.Controllers.Events;

namespace Engine.Controllers.Net
{
	/// <summary>
	/// Отправка событий на сервер
	/// </summary>
	class DataSenderClient:DataSender
	{
		private Client _client;
		private Controller _controller;
		private string _address;
		private int _port;

		public DataSenderClient(Controller controller, string acceptedEvent, string address, int port) : base(controller, acceptedEvent)
		{
			_controller = controller;
			_address = address;
			_port = port;
			_client=new Client();
			_client.GetRecieved += Recieve;
			_client.PrintNetDebug += PrintNetDebug;
			_client.Start(_address,_port);
			_controller.AddEventHandler("PingSend", PingSend);
			_controller.AddEventHandler("BeginLoop", ProcessSendRepeat);
		}

		private void ProcessSendRepeat(object sender, EventArgs e)
		{
			if (_client.socket.Available != 0){
				_client.ProcessSendTick();
			}
		}

		public override void Send(DataRecieveEventArgs dr)
		{
			// отправляем клиентам
			//PrintNetDebug("Client send " + dr.EventName + "+" + dr.DataString);
			_client.SendAsync(dr.EventName + "+" + dr.DataString);
		}

		//public override void Recieve(string data)
		//{
		//	base.Recieve(data);// вызов ничем не отличается - получили строку и надо её разделить
		//}

		private void PingSend(string address, int port)
		{
			//отсюда https://msdn.microsoft.com/ru-ru/library/system.net.networkinformation.ping(v=vs.110).aspx
			string who = address+":"+port;
			AutoResetEvent waiter = new AutoResetEvent(false);

			Ping pingSender = new Ping();
			pingSender.PingCompleted += PingCompletedCallback;
			string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";// Create a buffer of 32 bytes of data to be transmitted.
			byte[] buffer = Encoding.ASCII.GetBytes(data);
			int timeout = 12000;// Wait 12 seconds for a reply.
			PingOptions options = new PingOptions(64, true);
			Console.WriteLine("Time to live: {0}", options.Ttl);
			Console.WriteLine("Don't fragment: {0}", options.DontFragment);

			pingSender.SendAsync(who, timeout, buffer, options, waiter);
			//waiter.WaitOne();
		}

		private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
		{
			// If the operation was canceled, display a message to the user.
			if (e.Cancelled){
				Console.WriteLine("Ping canceled.");
				// Let the main thread resume. UserToken is the AutoResetEvent object that the main thread is waiting for.
				((AutoResetEvent)e.UserState).Set();
			}

			// If an error occurred, display the exception to the user.
			if (e.Error != null){
				Console.WriteLine("Ping failed:");
				Console.WriteLine(e.Error.ToString());
				// Let the main thread resume. 
				((AutoResetEvent)e.UserState).Set();
			}

			PingReply reply = e.Reply;
			DisplayReply(reply);
			// Let the main thread resume.
			((AutoResetEvent)e.UserState).Set();
			_controller.StartEvent("PingResult", this, PingResultEventArgs.Set(e.Reply, e.Reply.Status));
		}

		public static void DisplayReply(PingReply reply)
		{
			if (reply == null)
				return;

			Console.WriteLine("ping status: {0}", reply.Status);
			if (reply.Status == IPStatus.Success)
			{
				Console.WriteLine("Address: {0}", reply.Address.ToString());
				Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
				Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
				Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
				Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
			}
		}

		private void PingSend(object sender, EventArgs e)
		{
			PingSend(_address,_port);
		}
	}
	
}
