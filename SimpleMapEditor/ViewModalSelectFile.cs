using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Utils;
using Engine.Views;
using Button = Engine.Views.Templates.Button;

namespace SimpleMapEditor
{
	class ViewModalSelectFile:ViewModal
	{
		//TODO сделать визуализатор количества обработчикиов у событий и что бы они выводились справочно (можно было скрыть и переместить иконку, отдельный вспомогательный объект)

		public ViewModalSelectFile(Controller controller, string outEvent)
			: base(controller, outEvent)
		{
			SetCoordinates(200, 200, 0);
			SetSize(500,100);
			EscButton = Button.CreateButton(Controller, 280, 10, 100, 20, OutEvent+"Esc", "Закрыть", "Закрыть", Keys.None, "");
			AddComponent(EscButton);
			for (int i = 1; i < 10; i++){
				var btn = Button.CreateButton(Controller, 10 + i*42, 40, 40, 40, "modalPressed", "" + i, "" + i, Keys.None, "");
				AddComponent(btn);
			}
		}

		/// <summary>
		/// Кнопка закрытия модального окна
		/// </summary>
		protected Button EscButton;


		public override void HandlersAdd()
		{
			base.HandlersAdd();
			Controller.AddEventHandler("modalPressed", modalPressed);
			Controller.AddEventHandler(OutEvent + "Esc", EscPress);
		}

		public override void HandlersRemove()
		{
			Controller.RemoveEventHandler(OutEvent + "Esc", EscPress);
			Controller.RemoveEventHandler("modalPressed", modalPressed);
			base.HandlersRemove();
		}

		/// <summary>
		/// Нажали кнопку Esc значит закрываем диалог
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EscPress(object sender, EventArgs e)
		{
			DeleteObject = true;
		}

		private void modalPressed(object sender, EventArgs e)
		{
			var s = sender as ViewObject;
			if (s!=null){
				name = s.Name;
				ModalResult = Convert.ToInt32(name);
				//EscButton.Press();
				EscPress(this, e);
			}
		}

		private String name = "";

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			if (CursorOver)
				 visualizationProvider.SetColor(Color.FromArgb(50, Color.Chartreuse));
			else visualizationProvider.SetColor(Color.FromArgb(50,Color.Aquamarine));
			visualizationProvider.Box(X, Y, Width, Height);
			base.DrawObject(visualizationProvider);
			visualizationProvider.SetColor(Color.Aquamarine);
			visualizationProvider.Print(X+10, Y+10, "Для выхода из режима нажмите 8 " + name);
			var c = Controller.ToString();
		}
	}
}
