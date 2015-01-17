using System;
using System.Drawing;
using System.Windows.Forms;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;

namespace Engine.Views
{
	// TODO проверить, можно ли использовать этот класс автономно и как успешно. 
	// Для компонентов есть ViewControlDraggable, в котором используется этот класс, но может быть это излишне и лучше перенести всё туда, упростив

	/// <summary>
	/// Делегат для старта перемещения
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public delegate void DraggableDragStartDelegate(int x, int y);
	
	/// <summary>
	/// Делегат нажатия мышки
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="dragStarted">Запущено ли перемещение</param>
	public delegate void DraggableMouseClickDelegate(int x, int y, Boolean dragStarted);

	/// <summary>
	/// Делегат для перемещения мышки
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public delegate void DraggableMouseMoveDelegate(int x, int y);

	/// <summary>
	/// Делегат на завершение перемещения
	/// </summary>
	/// <param name="relX"></param>
	/// <param name="relY"></param>
	public delegate void DraggableDragEndDelegate(int relX, int relY);

	/// <summary>
	/// Объект визуализации, умеющий обрабатывать события перемещения
	/// </summary>
	public class ViewDraggable:ViewObject
	{
		public ViewDraggable(Controller controller) : base(controller)
		{
			IsCanStartDrag = false;// надо активировать режим извне, что бы отлавливать перемещение
		}

		private StateOne _stateLButton = StateOne.Init();

		/// <summary>
		/// Координата курсора
		/// </summary>
		public Point CursorPoint;

		/// <summary>
		/// Начальная точка перемещения
		/// </summary>
		public Point CursorPointFrom;

		/// <summary>
		/// Private! Режим определения относительного перемещения
		/// </summary>
		public Boolean DragStarted;

		/// <summary>
		/// Разрешение на включение режима определения относительного перемещения
		/// </summary>
		public Boolean IsCanStartDrag;

		/// <summary>
		/// Определяем то что зависит от перемещения курсора
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="point"></param>
		/// <remarks>На выходе получаем события, которые нужно переопределить - перемещение, дельта, координаты и т.п.</remarks>
		public void Cursor(object sender, PointEventArgs point)
		{
			if (CursorPoint.Equals(point.Pt)) return;// выходим, если курсор не сдвинулся
			CursorPoint = point.Pt;
			OnMouseMove(CursorPoint.X, CursorPoint.Y);
		}
		public void Keyboard(object sender, InputEventArgs e)
		{
			var sLButton = _stateLButton.Check(e.IsKeyPressed(Keys.LButton));
			if (DragStarted){// кнопка не нажата, значит формируем сигнал о завершении перемещения
				if (sLButton == StatesEnum.Off){
					OnDragEnd(CursorPointFrom.X - e.cursorX, CursorPointFrom.Y - e.cursorY);
					DragStarted = false;
				}
				e.Handled = true;
				return;
			}
			if (sLButton==StatesEnum.On){
				CursorPointFrom = new Point(e.cursorX, e.cursorY);
				if (IsCanStartDrag){
					DragStarted = true;// активируем перемещение и сохраняем текущие координаты
					e.Handled = true;
					OnDragStart(CursorPointFrom.X, CursorPointFrom.Y);
				}
				OnMouseClick(e.cursorX, e.cursorY,DragStarted);
			}
			if (!DragStarted) _stateLButton.Check(false);// сбрасываем
		}

		/// <summary>
		/// Начало перемещения после включения режима
		/// </summary>
		public DraggableDragStartDelegate OnDragStart;

		/// <summary>
		/// Нажатие на кнопку мыши
		/// </summary>
		public DraggableMouseClickDelegate OnMouseClick;

		/// <summary>
		/// Перемещение мыши по слою
		/// </summary>
		public DraggableMouseMoveDelegate OnMouseMove;

		/// <summary>
		/// Объект перемещён на эти относительные координаты
		/// </summary>
		public DraggableDragEndDelegate OnDragEnd;

		/// <summary>
		/// Отменяем событие перемещения
		/// </summary>
		protected void DragCancel()
		{
			DragStarted = false;
		}

	}
}
