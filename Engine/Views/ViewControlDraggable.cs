using System;
using System.Drawing;
using System.Net.NetworkInformation;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Views
{
	/// <summary>
	/// Контрол, который умеет обрабатывать перемещение
	/// </summary>
	public class ViewControlDraggable:ViewControl
	{
		public ViewDraggable Draggable;
		public ViewControlDraggable(Controller controller, ViewComponent parent)
			: base(controller,parent)
		{
			//IsCanStartDrag = false;// надо активировать режим извне, что бы отлавливать перемещение
			Draggable = new ViewDraggable(Controller);
			Draggable.OnDragEnd += DragEnd;
			Draggable.OnDragStart += DragStart;
			Draggable.OnMouseClick += MouseClick;
			Draggable.OnMouseMove += MouseMove;
		}

		/// <summary>
		/// Координата курсора
		/// </summary>
		protected Point CursorPoint {
			set { Draggable.CursorPoint = value; }
			get { return Draggable.CursorPoint; }
		}

		/// <summary>
		/// Начальная точка перемещения
		/// </summary>
		protected Point CursorPointFrom {
			set { Draggable.CursorPointFrom = value; }
			get { return Draggable.CursorPointFrom; }
		}

		/// <summary>
		/// Private! Режим определения относительного перемещения
		/// </summary>
		protected Boolean DragStarted {
			set {Draggable.DragStarted = value;}
			get {return Draggable.DragStarted;} 
		}

		/// <summary>
		/// Разрешение на включение режима определения относительного перемещения
		/// </summary>
		protected Boolean IsCanStartDrag{
			set{Draggable.IsCanStartDrag = value;}
			get{return Draggable.IsCanStartDrag;}
		}

		/// <summary>
		/// Сохраняемая координата курсора
		/// </summary>
		private int _cursorX;

		/// <summary>
		/// Сохраняемая координата курсора
		/// </summary>
		private int _cursorY;

		/// <summary>
		/// Определяем то что зависит от перемещения курсора
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="point"></param>
		/// <remarks>На выходе получаем события, которые нужно переопределить - перемещение, дельта, координаты и т.п.</remarks>
		public override void Cursor(object sender, PointEventArgs point)
		{
			var cOver = false;// что бы заблокировать cousorOver для 
			if (!Draggable.DragStarted){
				base.Cursor(sender, point); // код взят из ViewControlSystem
				_cursorX = point.Pt.X - X;
				_cursorY = point.Pt.Y - Y;
				if (Parent != null){
					foreach (var component in Components){
						if (!component.CanDraw) continue; // компонент скрыт
						var c = component as ViewControl;
						if (c == null) continue; // компонент уровня контрол и умеет передавать событие курсора вложенным компонентам
						var cpoint = PointEventArgs.Set(_cursorX, _cursorY);
						c.DeliverCursorEH(sender, cpoint);
						if (c.CursorOver) cOver = true;
					}
				}
			}
			if (!cOver)Draggable.Cursor(sender, point);
		}

		public override void Keyboard(object sender, InputEventArgs e)
		{
			base.Keyboard(sender, e);// на всякий случай вызываем этот метод
			var cOver = false;// что бы заблокировать событие "клавиатуры" для контрола перемещения
			foreach (var component in Components)
			{
				if (e.Handled) break; // если событие было обработано - выходим
				if (!component.CanDraw) continue; // компонент скрыт
				if (component.CursorOver) cOver = true;
				component.Keyboard(sender, e);
			}
			if (!cOver && !e.Handled && InRange(e.cursorX, e.cursorY)) Draggable.Keyboard(sender, e);
		}

		/// <summary>
		/// Начало перемещения после включения режима
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		protected virtual void DragStart(int x, int y)
		{
			
		}

		/// <summary>
		/// Нажатие на кнопку мыши
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="dragStarted">Запущено ли перемещение</param>
		protected virtual void MouseClick(int x, int y,Boolean dragStarted)
		{
			
		}

		/// <summary>
		/// Перемещение мыши по слою
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		protected virtual void MouseMove(int x, int y)
		{
		}

		/// <summary>
		/// Объект перемещён на эти относительные координаты
		/// </summary>
		/// <param name="relX"></param>
		/// <param name="relY"></param>
		protected virtual void DragEnd(int relX,int relY){}

		/// <summary>
		/// Отменяем событие перемещения
		/// </summary>
		protected void DragCancel()
		{
			DragStarted = false;
		}

		public override bool InRange(int x, int y)
		{
			if (DragStarted) return true;// в процессе перемещения объект занимает весь объем
			return base.InRange(x, y);
		}

	}
}
