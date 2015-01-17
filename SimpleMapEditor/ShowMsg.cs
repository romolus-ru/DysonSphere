using System;
using System.Drawing;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;

namespace SimpleMapEditor
{
	class ShowMsg:ViewComponent
	{
		private String _msg = "Сообщение";
		private byte _alpha;
		public ShowMsg(Controller controller, ViewComponent parent = null) : base(controller, parent)
		{
			_alpha=20;
		}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			visualizationProvider.LoadTexture("ShowMsg", @"..\Resources\msg01.jpg");
			SetCoordinates(0, 0, 0);// занимаем всё пространство
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		protected override void HandlersAdder()
		{
			base.HandlersAdder();
			Controller.AddEventHandler("ShowMsg",ShowMsgEH);
		}

		protected override void HandlersRemover()
		{
			Controller.RemoveEventHandler("ShowMsg", ShowMsgEH);
			base.HandlersRemover();
		}

		private void ShowMsgEH(object sender, EventArgs e)
		{
			MessageEventArgs m = e as MessageEventArgs;
			if (m == null) return;
			_alpha = 100;
			_msg = m.Message;
		}


		public override bool InRange(int x, int y)
		{
			return false;// не реагируем совсем
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			if (_alpha == 0) { return; }
			_alpha--;

			var mx = visualizationProvider.CanvasWidth/2;
			var my = visualizationProvider.CanvasHeight/2;
			visualizationProvider.SetColor(Color.White,_alpha);
			visualizationProvider.DrawTexture(mx, my, "ShowMsg");
			var l = visualizationProvider.TextLength(_msg)/2;
			visualizationProvider.Print(mx - l, my-14, _msg);
			//visualizationProvider.SetColor(Color.White);
			//visualizationProvider.DrawTexturePart(100, 100, "ShowMsg", 2, 3, 100, 30);
		}
	}
}
