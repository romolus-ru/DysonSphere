using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Engine.Controllers.Net
{
	/// <summary>
	/// Запись о клиенте на сервере
	/// </summary>
  class ClientConnection
	{
		private int _index;
		public GetStringDelegate PrintNetDebug;

		/// <summary>
		/// индекс подключения
		/// </summary>
		public int Index
		{
			get { return _index; }
		}
		public string name = "Неизвестный";

		private List<ClientConnection> Clients = null;// этот список получается в конструкторе от объекта Server
		private Socket Sock;
		private SocketAsyncEventArgs SockAsyncEventArgs;
		private byte[] buff;
		public GetStringDelegate GetString;

		public ClientConnection(Socket acceptedSocket, List<ClientConnection> listFr, int index)
		{
			this._index = index;
			Clients = listFr;
			buff = new byte[1024];
			this.Sock = acceptedSocket;
			SockAsyncEventArgs = new SocketAsyncEventArgs();
			SockAsyncEventArgs.Completed += SockAsyncEventArgs_Completed;
			SockAsyncEventArgs.SetBuffer(buff, 0, buff.Length);
			ReceiveAsync(SockAsyncEventArgs);
		}

		private void SockAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
		{
			switch (e.LastOperation)
			{
				case SocketAsyncOperation.Receive:
					ProcessReceive(e);
					break;
				case SocketAsyncOperation.Send:
					ProcessSend(e);
					break;
			}
		}

		private void ProcessSend(SocketAsyncEventArgs e)
		{
			if ((bool)SockAsyncEventArgs.UserToken == false)
				if (e.SocketError == SocketError.Success)
					ReceiveAsync(SockAsyncEventArgs);
		}

		private void ProcessReceive(SocketAsyncEventArgs e)
		{
			try{
				if (e.SocketError == SocketError.Success){
					SockAsyncEventArgs.UserToken = false;
					string str = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
					GetString(str);
					//PrintNetDebug("Входящее сообщение от #" + name + ": {" + str + "}");
					//SendAsync("Ok");// может быть и не нужно
				}
				// Если случайно клиент закрыл окно
				if (e.SocketError == SocketError.ConnectionReset){
					string temp = name;
					Clients.Remove(this);
					SendToAll("Вышел из чата  " + temp + "\n");
					//PrintNetDebug("Отключился " + temp);
				}

			}
			catch (Exception ex){
				Console.WriteLine(ex.Message);
			}
		}

		public void ReceiveAsync(SocketAsyncEventArgs e)
		{
			bool willRaiseEvent = Sock.ReceiveAsync(e);
			e.UserToken = true;
			if (!willRaiseEvent)
				ProcessReceive(e);
		}

		// Массовая рассылка
		public void SendToAll(string data)
		{
			foreach (ClientConnection Cl in Clients)
			{
				Cl.SendAsync(data);
			}
		}

		public void SendAsync(string data)
		{
			byte [] buffer = Encoding.UTF8.GetBytes(data);
			var e = new SocketAsyncEventArgs();
			e.Completed += SockAsyncEventArgs_Completed;
			e.SetBuffer(buffer, 0, buffer.Length);
			SendAsync(e);
		}
		public void SendAsync(SocketAsyncEventArgs e)
		{
			bool willRaiseEvent = Sock.SendAsync(e);
			if (!willRaiseEvent)
				ProcessSend(e);
		}


	}
}
