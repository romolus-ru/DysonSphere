using Engine.Controllers;
using Engine.Controllers.Events;
using System;

namespace Engine.Views
{
	/// <summary>
	/// Содержит другие контролы, настраивается на мышку и клавиатуру и передаёт нужное вложенным контролам
	/// </summary>
	public class ViewControl:ViewComponent
	{
		public ViewControl(Controller controller, ViewComponent parent): base(controller,parent) {}

		private void CursorEH(object o, EventArgs args)
		{
			DeliverCursorEH(o, args);
		}

		/// <summary>
		/// Доставить событие курсора всем компонентам
		/// </summary>
		/// <param name="o"></param>
		/// <param name="args"></param>
		public void DeliverCursorEH(object o, EventArgs args)
		{
			//определять возможность передачи события компонентам по видимости и по InRange
			if (!CanDraw){return;}
			var a = args as PointEventArgs;
			if (a == null){return;}
			if (!InRange(a.Pt.X, a.Pt.Y)){
				CursorOverOff(); // сбрасываем выделение, в том числе и у вложенных контролов
				return;
			}
			CursorOver = true;
			Cursor(o, a);
			if (Components != null){
				CursorOverOffed = false;
				foreach (var component in Components){
					component.CursorOver = false;
					if (!component.InRange(a.Pt.X - X, a.Pt.Y - Y)) continue; // компонент не в точке нажатия
					component.CursorOver = true;
					// TODO там может быть неправильная обработка - компонент содержит свои координаты относительно предыдущего объекта
					component.Cursor(o, a);
				}
			}
		}

		private void KeyboardEH(object o, EventArgs args)
		{
			DeliverKeyboardEH(o, args);
		}

		/// <summary>
		/// Доставить событие клавиатуры компонентам
		/// </summary>
		/// <param name="o"></param>
		/// <param name="args"></param>
		public void DeliverKeyboardEH(object o, EventArgs args)
		{
			if (!CanDraw){return;} // если компонент скрыт, то не обрабатываем события
			var a = args as InputEventArgs;
			if (a == null){return;}
			Keyboard(o, a);
			if (Components != null){
				var componentsLocal = Components.ToArray();// что бы небыло ошибки что список компонентов изменен
				foreach (var component in componentsLocal){
					if (a.Handled) break; // если событие было обработано - выходим
					// все объекты получат событие клавиатуры, даже если курсор не над ними
					// компонент сам должен решить реагировать или нет
					//if (!component.CursorOver)continue;// курсор не над объектом
					if (!component.CanDraw) continue; // компонент скрыт
					component.Keyboard(o, a);
				}
			}

		}

		protected override void HandlersAdder()
		{
			base.HandlersAdder();
			if (Parent == null){
				Controller.AddEventHandler("Cursor", CursorEH);
				Controller.AddEventHandler("Keyboard", KeyboardEH);
			}
		}

		protected override void HandlersRemover()
		{
			if (Parent == null){
				Controller.RemoveEventHandler("Cursor", CursorEH);
				Controller.RemoveEventHandler("Keyboard", KeyboardEH);
			}
			base.HandlersRemover();
		}

	}
}
