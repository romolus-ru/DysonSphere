using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;
using Engine.Views;
using Button = Engine.Views.Templates.Button;

namespace GMTubes.View
{
	/// <summary>
	/// Для ввода имени игрока
	/// </summary>
	/// <remarks>
	/// скопировано из SImpleMapEditor, с небольшими изменениями
	/// </remarks>
	class ViewModalInputName:ViewModalInput
	{
		private StateOneTime _keyTime = StateOneTime.Init(5);
		private int EditCursor = 0;

		public ViewModalInputName(Controller controller, ViewComponent parent, string outEvent, string destroyEvent,
			string value)
			: base(controller, parent, outEvent, destroyEvent, value)
		{
			EditCursor = _text.Length;
			_keyTime.Check(true);// блокируем клики что бы сразу окно не закрылось
			
		}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			SetSize(300, 80);
			SetCoordinates(350, 200, 0);
			EscButton = Button.CreateButton(Controller, -9, -9, -100, -20, OutEvent, "Закрыть", "Закрыть", Keys.None, "");
			AddComponent(EscButton);
			EscButton.Hide();
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
			var cur1 = ":> ";
			vp.SetColor(Color.Wheat, 50);
			vp.Rectangle(X, Y, Width, Height);
			vp.SetColor(Color.Aquamarine);
			vp.Print(X + 10, Y + 20, "Введите имя и нажмите Enter");
			vp.Print(X + 10, Y + 40, cur1 + _text);
			var s = cur1 + _text.Substring(0, EditCursor);
			var l = vp.TextLength(s);
			vp.SetColor(Color.White);
			vp.Print(X + 10 + l, Y + 40, "_");
			vp.SetColor(Color.Silver);
			vp.Print(X + 10, Y + 60, "" + EditCursor);
		}

		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.Black, 40);
			visualizationProvider.Box(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		public override void Keyboard(object o, InputEventArgs inputEventArgs)
		{
			base.Keyboard(o, inputEventArgs);
			var st=_keyTime.Check(true);
			if (st != StatesEnum.On) return;
			// обрабатываем управляющие кнопки
			if (inputEventArgs.IsKeyPressed(Keys.LButton)){
				if (!CursorOver){
					EscButton.Press();
					return;
				}
			}
			if (inputEventArgs.IsKeyPressed(Keys.Back))
			{
				if (EditCursor > 0){
					EditCursor--;
					_text = _text.Remove(EditCursor, 1);
				}
			}
			if (inputEventArgs.IsKeyPressed(Keys.Escape))
			{
				if (_text==_value){
					EscButton.Press();
					return;
				}
				_text = _value;
				EditCursor = _text.Length;
				return;
			}
			if (inputEventArgs.IsKeyPressed(Keys.Enter))
			{
				EscButton.Press();
				return;
			}
			if (inputEventArgs.IsKeyPressed(Keys.Left)){
				if (EditCursor > 0) EditCursor--;
			}
			if (inputEventArgs.IsKeyPressed(Keys.Right))
			{
				if (EditCursor <_text.Length) EditCursor++;
			}
			if (inputEventArgs.IsKeyPressed(Keys.Home)) EditCursor = 0;
			if (inputEventArgs.IsKeyPressed(Keys.End)) EditCursor = _text.Length;

			// кнопки ввода
			//for (Keys i = Keys.A; i < Keys.Z; i++){if (inputEventArgs.IsKeyPressed(i))Text +=inputEventArgs.KeyToUnicode(i);//(char)i;}
			var a = inputEventArgs.KeyToUnicode();
			if (a.Length>0){
				_text = _text.Insert(EditCursor, a);
				EditCursor+=a.Length;
			}
		}
	}
}
