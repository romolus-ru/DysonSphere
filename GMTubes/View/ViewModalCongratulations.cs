using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;
using Button = Engine.Views.Templates.Button;

namespace GMTubes.View
{
	/// <summary>
	/// Окно поздравления игрока с успешным прохождением
	/// </summary>
	class ViewModalCongratulations:ViewModal
	{
		// строки. "B" выводится другим цветом
		public String str1a = "";
		public String str1b = "";
		public String str1c = "";
		public String str2a = "";
		public String str2b = "";
		public String str2c = "";
		public String str3a = "";
		public String str3b = "";
		public String str3c = "";

		public Color c1;
		public Color c2;

		public ViewModalCongratulations(Controller controller, ViewComponent parent, string outEvent, string destroyEvent)
			: base(controller, parent, outEvent, destroyEvent){}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			SetSize(450, 150);
			SetCoordinates(300, 200, 0);
			EscButton = Button.CreateButton(Controller, 10, 100, 120, 30, OutEvent, "Дальше!", "Дальше", Keys.Enter, "");
			AddComponent(EscButton);
		}

		/// <summary>
		/// Модальное окно выдало результат - значит надо его запустить
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OutEventStarted(object sender, EventArgs e)
		{
			Controller.AddToOperativeStore(DestroyEvent, this, EventArgs.Empty);
		}

		/// <summary>
		/// Кнопка закрытия модального окна
		/// </summary>
		protected Button EscButton;


		protected override void HandlersAdder()
		{
			base.HandlersAdder();
			Controller.AddEventHandler(OutEvent, OutEventStarted);
		}

		protected override void HandlersRemover()
		{
			Controller.RemoveEventHandler(OutEvent, OutEventStarted);
			base.HandlersRemover();
		}

		protected override void DrawObject(VisualizationProvider vp)
		{
			base.DrawObject(vp);
			vp.SetColor(Color.SpringGreen);
			vp.SetColor(c1); vp.Print(X + 10, Y + 10, "П о з д р а в л я е м !");
			vp.SetColor(Color.Wheat, 80);
			vp.Rectangle(X, Y, Width, Height);
			vp.SetColor(c1); vp.Print(X + 10, Y + 30, str1a);
			vp.SetColor(c2); vp.Print( str1b);
			vp.SetColor(c1); vp.Print(str1c);
	
			vp.SetColor(c1); vp.Print(X + 10, Y + 50, str2a);
			vp.SetColor(c2); vp.Print(str2b);
			vp.SetColor(c1); vp.Print(str2c);

			vp.SetColor(c1); vp.Print(X + 10, Y + 70, str3a);
			vp.SetColor(c2); vp.Print(str3b);
			vp.SetColor(c1); vp.Print(str3c);
		}

		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.Black, 20);
			visualizationProvider.Box(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		public override void Keyboard(object o, InputEventArgs inputEventArgs)
		{
			base.Keyboard(o, inputEventArgs);
			//if (inputEventArgs.IsKeyPressed(Keys.Enter)){EscButton.Press();}
		}
	}
}

