using System;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Views
{
	/// <summary>
	/// Класс окна с возможностью выводить информацию и содержащую некоторые предопределённые кнопки
	/// </summary>
	public class ViewWindow : ViewControlDraggable
	{
		/// <summary>
		/// Создаём окно
		/// </summary>
		/// <param name="controller">Контроллер</param>
		/// <param name="parentSystem">ViewControlSystem</param>
		public ViewWindow(Controller controller, ViewComponent parentSystem=null)
			: base(controller, parentSystem)
		{
			IsCanStartDrag = true;// разрешаем двигать окно
			HeaderHeight = 15;
		}

		protected Boolean DragView = false;

		//у окна нету mouseover
		protected override void DrawObject(VisualizationProvider vp)
		{
			base.DrawObject(vp);
			vp.SetColor(Color.FloralWhite, 100);
			var x = X;
			var y = Y;
			if (DragView)
			{// перемещаем форму (можно заменить это просто перетаскиванием рамки)
				x -= (CursorPointFrom.X - CursorPoint.X);
				y -= (CursorPointFrom.Y - CursorPoint.Y);
			}
			//vp.SetColor(Color.DimGray,50);
			//vp.Box(x, y, Width, Height);
			vp.SetColor(Color.DarkRed, 70);
			vp.Box(x, y, Width, HeaderHeight);
			if (DragView)
			{
				vp.SetColor(Color.FloralWhite);
				vp.Rectangle(x, y, Width, Height);
			}

			if (CursorOver)
			{
				vp.SetColor(Color.FloralWhite);
				vp.Rectangle(x + 1, y + 1, Width - 2, Height - 2);
			}

			//vp.OffsetAdd(X, Y);// выводим текст относительно окна
			//vp.SetColor(Color.White);
			//vp.Print(10, 50, " c1 " + CursorPoint);
			//vp.Print(10, 65, " c2 " + CursorPointFrom);
			//vp.Print(10, 80, " dragstart " + draggable.DragStarted);
			//vp.Print(10, 95, " ord " + _zOrder);
			//vp.Print(10, 110, " top " + TopMost);
			//vp.Print(10, 125, " fs " + focused);
			//vp.OffsetRemove();
		}

		protected override void DragStart(int x, int y)
		{
			if (!CursorOver) return;
			base.DragStart(x, y);
			var d = (X < x) && (x < X + Width) && (Y < y) && (y < Y + HeaderHeight);
			if (!d){// отменяем перемещение если не за заголовок перетаскиваем
				DragCancel();
				DragView = false;
			}else{// включаем визуализацию перемещения
				// передаём объекту сигнал о перемещении окна наверх
				Controller.StartEvent("ViewBringToFront", this);
				DragView = true;
			}
		}

		public int HeaderHeight { get; private set; }

		protected override void DragEnd(int relX, int relY)
		{
			if (!CursorOver) return;
			X -= relX;
			Y -= relY;
			DragView = false;
			base.DragEnd(relX, relY);
		}

		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			if (CursorOver) visualizationProvider.SetColor(Color.SlateGray, 100);
			else visualizationProvider.SetColor(Color.DarkBlue, 30);
			visualizationProvider.Box(X, Y, Width, Height);
		}
	}
}
