// осторожно! может влиять на viewOBjectEditorUI
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;
using Engine.Utils.Editor;
using Engine.Views;
using View = Engine.Views.View;

namespace SimpleMapEditor
{
	class viewObjectFullView:ViewObject
	{
		public editor Editor;
		public int textureNum;

		private Controller _controller;
		public Point cPoint = new Point(10, 10);// курсор
		private int centerX;// для перемещения изображения экрана
		private int centerY;

		public int ClickX = -1;
		public int ClickY = -1;
		public Boolean Clicked = false;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		public viewObjectFullView(Controller controller): base(controller)
		{
			_controller = controller;
		}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			visualizationProvider.LoadTexture("main", @"..\Resources\defanceLabirinth.jpg");
		}

		private void CursorMovedEH(object o, EventArgs args)
		{
			CursorMoved(o, args as PointEventArgs);
		}

		private void KeyboardEH(object o, EventArgs args)
		{
			Keyboard(o, args as InputEventArgs);
		}
		
		/// <summary>
		/// Отслеживаем перемещение курсора
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="point"></param>
		private void CursorMoved(object sender, PointEventArgs point)
		{
			cPoint = point.Pt;// точка где счас находится курсор
		}

		private void Keyboard(object sender, InputEventArgs e)
		{
			if (e.IsKeyPressed(Keys.Left)) centerX += 50;
			if (e.IsKeyPressed(Keys.Right)) centerX -= 50;
			if (e.IsKeyPressed(Keys.Up)) centerY += 50;
			if (e.IsKeyPressed(Keys.Down)) centerY -= 50;
			if (e.IsKeyPressed(Keys.LButton)){
				if (e.cursorX<900){
					if (e.cursorY<650){// курсор в пределах карты, значит надо определить координаты нажатия
						Clicked = true;
						ClickX = e.cursorX - centerX;
						ClickY = e.cursorY - centerY;
						_controller.KeyboardClear();
						_controller.StartEvent("MapView", this);
						
					}
				}
			}
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.CornflowerBlue);
			visualizationProvider.Circle(cPoint.X, cPoint.Y, 40);
			visualizationProvider.SetColor(Color.Chartreuse);
			visualizationProvider.Print(50, 690, "> " + cPoint.X + " " + cPoint.Y + "  + " + centerX + " " + centerY);
			if (Editor==null){
				return;
			}
			foreach (IDataHolder dh in Editor.Objects()){
				SimpleEditableObject seo = (SimpleEditableObject) dh;
				int x1 = seo.X/16 + centerX;
				int y1 = seo.Y/16 + centerY;
				visualizationProvider.Rectangle(x1,y1,1,1);
			}
		}

		public void SetHandlers()
		{
			if (CanDraw){
				_controller.AddEventHandler("Cursor", CursorMovedEH);
				_controller.AddEventHandler("Keyboard", KeyboardEH);
				int minX = 0;
				int minY = 0;
				int maxX = 0;
				int maxY = 0;
				foreach (SimpleEditableObject p in Editor.Objects())
				{
					minX = p.X;minY = p.Y;maxX = p.X;maxY = p.Y;
					break; // получаем начальные данные и прерываем
				}
				foreach (SimpleEditableObject p in Editor.Objects())
				{
					if (minX > p.X) minX = p.X;
					if (minY > p.Y) minY = p.Y;
					if (maxX < p.X) maxX = p.X;
					if (maxY < p.Y) maxY = p.Y;
				}
				centerX = (maxX - minX)/2/16;
				centerY = (maxX - minY)/2/16;
			}
			else{
				_controller.RemoveEventHandler("Cursor", CursorMovedEH);
				_controller.RemoveEventHandler("Keyboard", KeyboardEH);
			}
			ClickX = -1;
			ClickY = -1;
			Clicked = false;
		}

	}
}
