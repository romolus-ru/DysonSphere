using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;

namespace Engine.Views2
{

	/// <summary>
	/// Позволяет перемещать объект по экрану
	/// </summary>
	class ViewDraggable:ViewControl
	{
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
		protected Boolean DragStarted;

		private int HeaderX;
		private int HeaderY;
		private int HeaderWidth;
		private int HeaderHeight;

		/// <summary>
		/// Разрешение на включение режима определения относительного перемещения
		/// </summary>
		public Boolean IsCanStartDrag;

		public ViewDraggable(Controller controller) : base(controller)
		{
			IsCanStartDrag = true;// надо активировать режим извне, что бы отлавливать перемещение. возможно, уже лишний флаг
		}

		/// <summary>Установить координаты и размеры области, за которую перемещаем</summary>
		/// <param name="headerX"></param>
		/// <param name="headerY"></param>
		/// <param name="headerWidth"></param>
		/// <param name="headerHeight"></param>
		public void SetHeader(int headerX, int headerY, int headerWidth, int headerHeight)
		{
			HeaderX = headerX;
			HeaderY = headerY;
			HeaderWidth = headerWidth;
			HeaderHeight = headerHeight;
		}

		/// <summary>
		/// Определяем то что зависит от перемещения курсора
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="point"></param>
		/// <remarks>На выходе получаем события, которые нужно переопределить - перемещение, дельта, координаты и т.п.</remarks>
		protected override void Cursor(object sender, PointEventArgs point)
		{
			if (CursorPoint.Equals(point.Pt)) return;// выходим, если курсор не сдвинулся
			CursorPoint = point.Pt;
			MouseMove(CursorPoint.X, CursorPoint.Y);
		}

		protected override void Keyboard(object sender, InputEventArgs e)
		{
			var sLButton = _stateLButton.Check(e.IsKeyPressed(Keys.LButton));
			if (DragStarted){// кнопка не нажата, значит формируем сигнал о завершении перемещения
				if (sLButton == StatesEnum.Off){
					var relX = CursorPointFrom.X - e.cursorX;
					var relY = CursorPointFrom.Y - e.cursorY;
					X -= relX;
					Y -= relY;
					DragEnd(relX, relY);
					DragStarted = false;
				}
				e.Handled = true;
				return;
			}
			if (sLButton == StatesEnum.On){
				CursorPointFrom = new Point(e.cursorX, e.cursorY);
				if (InRangeHeader(CursorPointFrom.X, CursorPointFrom.Y)){
					DragStarted = true; // активируем перемещение и сохраняем текущие координаты
					e.Handled = true;
					DragStart(CursorPointFrom.X, CursorPointFrom.Y);
				}
				MouseClick(e.cursorX, e.cursorY, DragStarted);
			}
			if (!DragStarted) _stateLButton.Check(false); // сбрасываем
		}

		/// <summary>
		/// Определяет область, клик на которой запускает перемещение
		/// </summary>
		/// <returns></returns>
		protected virtual Boolean InRangeHeader(int x,int y)
		{
			if (!CursorOver) return false;
			var d = (HeaderX < x) && (x < HeaderX + HeaderWidth) && (HeaderY < y) && (y < HeaderY + HeaderHeight);
			return d;
		}

		/// <summary>
		/// Начало перемещения после включения режима
		/// </summary>
		public virtual void DragStart(int x, int y) { }

		/// <summary>
		/// Нажатие на кнопку мыши
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="dragStarted">Запущено ли перемещение</param>
		public virtual void MouseClick(int x, int y, Boolean dragStarted) { }

		/// <summary>
		/// Перемещение мыши по слою
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public virtual void MouseMove(int x, int y){}

		/// <summary>
		/// Объект перемещён на эти относительные координаты
		/// </summary>
		/// <param name="relX"></param>
		/// <param name="relY"></param>
		public virtual void DragEnd(int relX, int relY) { }

		/// <summary>
		/// Отменяем событие перемещения
		/// </summary>
		protected void DragCancel()
		{
			DragStarted = false;
		}
	}
}
