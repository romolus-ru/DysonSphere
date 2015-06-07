using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;
using Engine.Utils.ExtensionMethods;
using Button = Engine.Views.Templates.Button;

namespace ZChatTest
{
	class View1:ViewControlDraggable// ViewControl
	{

		private int angle;
		
		public View1(Controller controller, ViewComponent parent): base(controller, parent)
		{
			AddB(Controller, 1, "ChatCreateClient", "Создать клиента");
			AddB(Controller, 2, "ChatCreateServer", "Создать сервер");
			AddB(Controller, 3, "ChatVerifyServer", "Проверить запущен ли сервер");
			AddB(Controller, 4, "ChatSendToServer1", "Послать сообщение серверу");
			AddB(Controller, 5, "ChatSendToClient1", "Послать сообщение клиенту");

			Controller.AddEventHandler("ChatCreateClient", ChatCreateClientEH);
			Controller.AddEventHandler("ChatCreateServer", ChatCreateServerEH);
			Controller.AddEventHandler("PrintNetDebug", PrintNetDebugEH);
			Controller.AddEventHandler("PrintNetDebug2", PrintNetDebug2EH);
			Controller.AddEventHandler("ChatSendToServer1", ChatSendToServer1EH);
			Controller.AddEventHandler("ChatSendToClient1", ChatSendToClient1EH);
			Controller.AddEventHandler("AngleFromServer", AngleFromServerEH);
		}

		private void AngleFromServerEH(object sender, EventArgs e)
		{
			var m = e as MessageEventArgs;
			var mv = m.Message;
			var a = m.Message.DeserializeObject<IntegerEventArgs>();
			angle = a.I;
			var v = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + "Синхронизировано";
			_datas.Add(v);
			if (_datas.Count > 50) _datas.RemoveAt(0);
		}

		private void ChatSendToClient1EH(object sender, EventArgs e)
		{
			Controller.SendToViewCommand("PrintNetDebug2", MessageEventArgs.Msg("SendToClient1"));
		}

		private void ChatSendToServer1EH(object sender, EventArgs e)
		{
			Controller.SendToModelCommand("PrintNetDebug2", MessageEventArgs.Msg("SendToServer1"));
		}

		private void AddB(Controller controller, int i, string eventName, string caption)
		{
			Button a = Button.CreateButton(controller, 10, i*30 - 20, 190, 20, eventName, caption, "", Keys.None, "btnChat" + i);
			AddComponent(a);
		}

		private List<String> _datas = new List<string>();

		private void PrintNetDebugEH(object sender, EventArgs e)
		{
			var m = e as MessageEventArgs;
			var mv = m.Message;
			if (mv.StartsWith("<?xml")){
				
				var a = mv.DeserializeObject<MessageEventArgs>();
				mv = a.Message;
				mv = "X "+mv;
			}
			var v = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + mv;
			_datas.Add(v);
			Debug.Print(v);
			if (_datas.Count > 50) _datas.RemoveAt(0);
		}

		private void PrintNetDebug2EH(object sender, EventArgs e)
		{
			var m = e as MessageEventArgs;
			var mv = m.Message;
			if (mv.StartsWith("<?xml"))
			{
				var a = mv.DeserializeObject<MessageEventArgs>();
				mv = a.Message;
				mv = "X " + mv;
			}
			if (mv == null){mv = "null";}
			var v = "[" + DateTime.Now.ToString("HH:mm:ss") + "]2 " + mv;
			_datas.Add(v);
			Debug.Print(v);
			if (_datas.Count > 50) _datas.RemoveAt(0);
		}

		//объекты не обрабатывают cursorover

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			SetCoordinates(0, 0);
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			
		}

		private void ChatCreateClientEH(object sender, EventArgs e)
		{
			Controller.StartEvent("StartClient", this, EventArgs.Empty);
			if (sender is Button) (sender as Button).Hide();
		}

		private void ChatCreateServerEH(object sender, EventArgs e)
		{
			Controller.StartEvent("StartServer", this, EventArgs.Empty);
			if (sender is Button)(sender as Button).Hide();
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);

			visualizationProvider.SetColor(Color.PowderBlue);
			int i = 0;
			var ds = new List<string>(_datas);
			foreach (var d in ds){
				visualizationProvider.Print(300, 50 + i*15, d);
				i++;
			}

			//angle++;
			//if (angle > 360) angle = 0;
			var a2 = angle*Math.PI/180;
			int x = (int) Math.Round(50*Math.Cos(a2));
			int y = (int) Math.Round(50*Math.Sin(a2));

			visualizationProvider.SetColor(Color.Thistle);
			visualizationProvider.Line(200, 400, 200 + x, 400 + y);

		}
	}
}
