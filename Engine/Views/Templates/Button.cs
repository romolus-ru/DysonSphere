using System;
using System.Windows.Forms;
using System.Drawing;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;

namespace Engine.Views.Templates
{
	public class Button:ViewControl
	{
		/// <summary>
		/// Имя запускаемого события
		/// </summary>
		private string _eventName;

		/// <summary>
		/// Заголовок кнопки
		/// </summary>
		protected string Caption;

		/// <summary>
		/// Заголовок кнопки
		/// </summary>
		protected string Hint;

		/// <summary>
		/// Кнопка для кнопки
		/// </summary>
		protected Keys Key;

		protected StateOneTime StateOneKeyboard=StateOneTime.Init(15);// для кнопки клавиатуры
		protected StateOne StateOneMouse = new StateOne();// для кнопки мыши

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="parent"></param>
		public Button(Controller controller, ViewComponent parent) : base(controller, parent){}

		/// <summary>
		/// Жмём на кнопку
		/// </summary>
		public void Press()
		{
			// TODO надо проверить, лучше что бы было расшарено, но в то же время нажатие становится неуправляемым
			if (Controller != null){
				Controller.AddToOperativeStore(_eventName, this, EventArgs.Empty);
			}
		}

		public override void Keyboard(object sender, InputEventArgs e)
		{
			if (!CanDraw) return;
			//if (!CursorOver) return;
			// (если нажата кнопка мыши и мышка находится над кнопкой) или (если нажата кнопка на клавиатуре)
			bool b = e.IsKeyPressed(Keys.LButton);
			bool b2 = e.IsKeyPressed(Key);
			var sk = StateOneKeyboard.Check((b && CursorOver) || b2);
			if (sk==StatesEnum.On){
				Press();
			}
			if (StateOneKeyboard.CurrentState == StatesEnum.On) e.Handled = true;
			base.Keyboard(sender, e);
		}

		public void Init(string eventName,String caption, String hint, Keys key)
		{
			_eventName = eventName;
			Caption = caption;
			Hint = hint;
			Key = key;
			Name = caption;
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.RotateReset();
			DrawComponentBackground(visualizationProvider);
			String txt;
			if (CursorOver){
				txt = "[" + Caption + "]";
				visualizationProvider.SetColor(Color.Yellow);
			} else{
				txt = " " + Caption + " ";
				visualizationProvider.SetColor(Color.Green);
			}
			visualizationProvider.Print(X + 4, Y - 3, txt);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			if (Hint != "" && CursorOver){
				visualizationProvider.Print(X + 10, Y + Height + 5, Hint);
			}
		}

		/// <summary>
		/// Инициализируем кнопку. закрытый метод, может быть удастся от него избавиться и опрерделиться что лучше 
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="eventName"></param>
		/// <param name="caption"></param>
		/// <param name="hint"></param>
		/// <param name="key"></param>
		private static Button CreateButton(Controller controller, int x, int y, int width, int height, 
			string eventName, String caption, String hint, Keys key)
		{
			var btn = new Button(controller,null);
			btn.Show();
			var dx = 0;//rnd.Next(0, 20);
			var dy = 0;//rnd.Next(0, 20);
			btn.SetCoordinates(x+dx, y+dy, 0);
			btn.SetSize(width, height);
			btn.Init(eventName, caption, hint, key);
			return btn;
		}

		/// <summary>
		/// Инициализируем кнопку, с учётом названия
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="eventName"></param>
		/// <param name="caption"></param>
		/// <param name="hint"></param>
		/// <param name="key"></param>
		/// <param name="buttonName"></param>
		public static Button CreateButton(Controller controller, int x, int y, int width, int height, string eventName, String caption, String hint, System.Windows.Forms.Keys key, String buttonName)
		{
			var btn = CreateButton(controller, x, y, width, height, eventName, caption, hint, key);
			btn.Name = buttonName;// по умолчанию берётся из caption, может быть приживётся
			return btn;
		}

		public void SetCaption(String newCaption)
		{
			Caption = newCaption;
		}
	}
}
