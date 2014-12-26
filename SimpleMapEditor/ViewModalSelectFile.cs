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

		public ViewModalSelectFile(Controller controller, ViewComponent parent, string outEvent,string destroyEvent)
			: base(controller,parent, outEvent,destroyEvent)
		{
			
		}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			SetCoordinates(200, 200, 0);
			SetSize(500, 100);
			EscButton = Button.CreateButton(Controller, 280, 10, 100, 20, OutEvent, "Закрыть", "Закрыть", Keys.None, "");
			AddComponent(EscButton);
			for (int i = 1; i < 10; i++)
			{
				var btn = Button.CreateButton(Controller, 10 + i * 42, 40, 40, 40, "mP", "" + i, "" + i, Keys.None, ""+i);
				AddComponent(btn);
			}
		}

		/// <summary>
		/// Модальное окно выдало результат - значит надо его запустить
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OutEventStarted(object sender, EventArgs e)
		{
			// возможно, надо будет как то разделять события от разных модальных систем
			//if (sender != this) return;// это событие должно генерироваться тут
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
			Controller.AddEventHandler("mP", ModalPressed);
		}

		protected override void HandlersRemover()
		{
			Controller.RemoveEventHandler(OutEvent, OutEventStarted);
			Controller.RemoveEventHandler("mP", ModalPressed);
			base.HandlersRemover();
		}

		/// <summary>
		/// Для остановки повторного нажатия на кнопки
		/// </summary>
		//private Boolean _stopRepeat = false;

		private void ModalPressed(object sender, EventArgs e)
		{
			//TODO --->>>>>> проверить почему нажимается 2 раза
			var s = sender as ViewObject;
			if (s!=null){
				name = s.Name;
				ModalResult = Convert.ToInt32(name);
				//Controller.AddToOperativeStore(OutEvent, this, EventArgs.Empty);
				EscButton.Press();
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
