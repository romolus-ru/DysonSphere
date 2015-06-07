using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Engine.Controllers.Net
{
	class Client
	{
		public Socket socket;

		// но по возможности сделать private - нечего ему быть в паблике
		private SocketAsyncEventArgs SockAsyncEventArgs; // объект для асинхронной операции на сокете
		private string _address;
		private int _port;

		private byte[] buff; // буфер обмена
		public GetStringDelegate GetRecieved;
		public GetStringDelegate PrintNetDebug;

		private void Init()
		{
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			buff = new byte[1024];
			SockAsyncEventArgs = new SocketAsyncEventArgs();
			// подписываемся на завершение асинхронного соединения
			SockAsyncEventArgs.Completed += SockAsyncArgs_Completed;
		}

		/// <summary>
		/// обработка последней асинхронной операции
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SockAsyncArgs_Completed(object sender, SocketAsyncEventArgs e)
		{
			switch (e.LastOperation){
				case SocketAsyncOperation.Connect:
					ProcessConnect(e);
					break;
				case SocketAsyncOperation.Receive:
					ProcessReceive(e);
					break;

			}
		}

		/// <summary>
		/// Подключаемся к серверу
		/// </summary>
		/// <param name="address">адрес подключения</param>
		/// <param name="port">порт для подключения</param>
		public void Start(string address, int port)
		{
			_address = address;
			_port = port;
			Init();
			SockAsyncEventArgs.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
			ConnectAsync(SockAsyncEventArgs);
			AcceptCompleted();
		}

		private void ConnectAsync(SocketAsyncEventArgs e)
		{
			bool willRaiseEvent = socket.ConnectAsync(e);
			if (!willRaiseEvent)
				ProcessConnect(e);
		}

		/// <summary>
		/// если нет ошибок, задаем буфер
		/// </summary>
		/// <param name="e"></param>
		private void ProcessConnect(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
				SockAsyncEventArgs.SetBuffer(buff, 0, buff.Length);
			else
				PrintNetDebug("Потеряно соединение с " + e.RemoteEndPoint);
		}

		/// <summary>
		/// отправка сообщений
		/// </summary>
		/// <param name="data"></param>
		public void SendAsync(string data)
		{
			if (socket.Connected && data.Length > 0){
				byte[] buff = Encoding.UTF8.GetBytes(data);
				SocketAsyncEventArgs e = new SocketAsyncEventArgs();
				e.SetBuffer(buff, 0, buff.Length);
				e.Completed += SockAsyncArgs_Completed;
				bool willRaiseEvent = socket.SendAsync(e);
			}
		}

		/// <summary>
		/// Прием сообщений
		/// </summary>
		/// <param name="e"></param>
		private void ProcessSend(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success){
				ReceiveAsync(SockAsyncEventArgs);
			}
			else{
				PrintNetDebug("Не отправлено");
			}
		}

		/// <summary>
		/// Прием сообщений по таймеру
		/// </summary>
		public void ProcessSendTick()
		{
			ProcessSend(SockAsyncEventArgs);
		}



		private void ReceiveAsync(SocketAsyncEventArgs e)
		{
			bool willRaiseEvent = socket.ReceiveAsync(e);
			if (!willRaiseEvent)
				ProcessReceive(e);
		}

		/// <summary>
		/// выводим на экран
		/// </summary>
		/// <param name="e"></param>
		private void ProcessReceive(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success){
				if (e.BytesTransferred != 0){
					string str = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
					GetRecieved(str);
					//PrintNetDebug("client recieved "+str);
				}
			}
			else{
				PrintNetDebug("Не отправлено");
			}
		}

		private void AcceptCompleted()
		{
			byte[] data = new byte[1024];
			int recv = socket.Receive(data, data.Length, SocketFlags.None);

			if (recv != 0){
				string message = Encoding.UTF8.GetString(data, 0, recv);
				var str = message + " Client " + "acceptComplete ";
				GetRecieved(str);
				//PrintNetDebug(str);
			}
		}

	}
}
