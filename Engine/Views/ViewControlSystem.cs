using System;
using System.Windows.Forms;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Views
{
	/// <summary>
	/// Обобщённый компонент, штучный, содержит другие компоненты и определяет очередность обработки событий клавиатуры и мыши
	/// </summary>
	public class ViewControlSystem : ViewControl
	{
		/// <summary>
		/// Храним объект, который в текущий момент обрабатывает клавиатуру в монопольном режиме
		/// </summary>
		public ViewComponent KeyboardControlled = null;

		/// <summary>
		/// Сохраняемая координата курсора
		/// </summary>
		private int _cursorX;

		/// <summary>
		/// Сохраняемая координата курсора
		/// </summary>
		private int _cursorY;

		public ViewControlSystem(Controller controller) : base(controller, null){}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			X = 0;
			Y = 0;
			Height = visualizationProvider.CanvasHeight;
			Width = visualizationProvider.CanvasWidth;
		}

		public override bool InRange(int x, int y)
		{
			return true;// этот компонент занимает всё пространство и перехватывает все события независимо от размеров экрана
		}

		public override void Cursor(object o, PointEventArgs args)
		{
			base.Cursor(o, args);
			_cursorX = args.Pt.X;
			_cursorY = args.Pt.Y;
			var cOver = false;
			if (Parent != null){
				foreach (var component in Components){
					if (cOver) break;
					if (!component.CanDraw) continue; // компонент скрыт
					var c = component as ViewControl;
					if (c == null) continue;// компонент уровня контрол и умеет передавать событие курсора вложенным компонентам
					c.DeliverCursorEH(o, args);
					if (c.CursorOver) cOver = true;
				}
			}
		}

		public override void Keyboard(object o, InputEventArgs args)
		{
			//base.Keyboard(o, args);

			bool mouseLPressed = args.IsKeyPressed(Keys.LButton);
			bool mouseRPressed = args.IsKeyPressed(Keys.RButton);
			// если кнопки мышки нажаты или есть управляемый объект (что бы к нему попало событие не нажатия)
			if (mouseLPressed || mouseRPressed || (KeyboardControlled != null)){
				if (mouseLPressed || mouseRPressed){// передаём событие дальше
					foreach (var component in Components){// ищем кому передать
						if (!component.InRange(_cursorX, _cursorY)){continue;
						} // компонент не обрабатывает клик, потому что кликают где то не у компонента
						component.Keyboard(o, args);
						// компонент в пределах досягаемости - передаём событие ему (если это контрол то deliver вызовется, иначе - событие компонента(с обходом всех вложенных))
						KeyboardControlled = component; // сохраняем
						KeyboardControlled.Focused = true;
						break;
					}
				}
				else{
					KeyboardControlled.Keyboard(o,args);// передаём "пустое" событие ("отпускание")
					KeyboardControlled.Focused = false;
					KeyboardControlled = null;
				}
			}
			else{//основной компонент получил событие клавиатуры, отправляем его всем
				// TODO тут. события не синхронизированы. может быть сбой
				// последовательность событий надо уточнить. а то могут работать не в таком порядке и ещё и запускаться много раз
				//DeliverKeyboardEH(o,args);
				// усеченная версия DeliverEH из ViewControl зато не будет посылать событие себе же
				foreach (var component in Components){
					if (args.Handled) break; // если событие было обработано - выходим
					if (!component.CanDraw) continue; // компонент скрыт
					component.Keyboard(o, args);
				}
			}
		}
	}
}
