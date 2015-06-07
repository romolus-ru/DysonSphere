using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Engine.Controllers.Net
{
	class Server
	{
		private Boolean _stopped;
		private Socket Sock;
		private int ConnectedSockets;
		private SocketAsyncEventArgs AcceptAsyncArgs;
		public static List<ClientConnection> Clients = new List<ClientConnection>();
		public GetStringDelegate GetRecieved;
		public GetStringDelegate PrintNetDebug;
		public Server()
		{
			Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			AcceptAsyncArgs = new SocketAsyncEventArgs();
			AcceptAsyncArgs.Completed += AcceptCompleted;
		}

		private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
		{
			if ((e.SocketError == SocketError.Success) && (Clients.Count < 50))
			{
				ClientConnection client = new ClientConnection(e.AcceptSocket, Clients, ConnectedSockets);
				client.name = "N" + ConnectedSockets;
				client.GetString = GetRecieved;
				client.PrintNetDebug = PrintNetDebug;
				Clients.Add(client);
				ConnectedSockets++;
				var data = "Добро пожаловать в чат :) !!";
				//PrintNetDebug(data+" send from server");
				client.SendAsync(data);
				//PrintNetDebug(data + " sended");
			}
			e.AcceptSocket = null;
			if (!_stopped)
				AcceptAsync(AcceptAsyncArgs);
			
		}
		private void AcceptAsync(SocketAsyncEventArgs e)
		{
			bool willRaiseEvent = Sock.AcceptAsync(e);
			if (!willRaiseEvent)
				AcceptCompleted(Sock, e);
		}

		public void Start(int port)
		{
			ConnectedSockets = 0;
			Sock.Bind(new IPEndPoint(IPAddress.Any, port));
			Sock.Listen(ConnectedSockets);
			PrintNetDebug("Сервер готов");
			AcceptAsync(AcceptAsyncArgs);
		}

		public void Stop()
		{
			_stopped = true;// что бы дальше не отправлялось
			Sock.Close();
			Sock.Dispose();
		}

		/// <summary>
		/// Отправить данные всем подключенным клиентам
		/// </summary>
		/// <param name="data"></param>
	public void SendToAll(string data)
		{
			foreach (var cl in Clients){
				cl.SendAsync(data);
			}
		}

	}
}
