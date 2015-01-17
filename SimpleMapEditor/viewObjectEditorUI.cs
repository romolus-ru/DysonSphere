// TODO отдельная информационная панель, в целом для отображения текущих показателей
// нужно для настройки элемента и применения его свойств к существующему объекту или создание на основе данных нового объекта
// TODO режим пипетки - получение данных с блока для последующего изменения или создания копии
// TODO несколько дополнительных UI для редактирования и отображения более специфических вещей
// например отображение блоков с траекториями и изменение параметров движения блоков

using System;
using System.Collections;
using System.Drawing;
using Engine;
using Engine.Controllers;
using Engine.Utils;
using Engine.Utils.Editor;
using Engine.Views;
using Engine.Controllers.Events;
using System.Windows.Forms;

namespace SimpleMapEditor
{
	class viewObjectEditorUI : ViewObject
	{

		public editor Editor;
		public ObjectTypes ObjType;
		public Boolean NewObjClick;// для создания нового объекта при клике на карте
		public Boolean PaintObjClick;// для перекрашивания объекта при клике на карте
		public Boolean GetInfoObjectClick;// для получения информации об объекте

		/// <summary>
		/// Размер блока
		/// </summary>
		private const int blockH = 16;
		/// <summary>
		/// Размер блока
		/// </summary>
		private const int blockW = 16;

		private Controller _controller;
		private Point cPoint = new Point(10, 10);
		private Boolean moveOperation = false;
		private SimpleEditableObject moveObjNum = null;
		private SimpleEditableObject moveObjNumCurrent = null;
		private SimpleEditableObject sample = null;
		private Point moveStart;
		private Point moveCurrent;
		private Boolean movePressed;
		private int x1OldCreated=-9473;
		private int y1OldCreated=-14748;
		public int centerX = 0;// для перемещения изображения экрана
		public int centerY = 0;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		public viewObjectEditorUI(Controller controller): base(controller)
		{
			this._controller = controller;
			//_controller.AddEventHandler("Cursor", CursorMovedEH);
			//_controller.AddEventHandler("Keyboard", KeyboardEH);
		}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			visualizationProvider.LoadTexture("main", @"..\Resources\defanceLabirinth.jpg");
			sample=new SimpleEditableObject();
		}

		/// <summary>
		/// Вычисляем расстояние между координатами
		/// </summary>
		/// <returns></returns>
		public float Distance(int x, int y, int X, int Y)
		{
			var dx = x - X;
			var dy = y - Y;
			return (float)Math.Sqrt(dx * dx + dy * dy);
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
			if (e.IsKeyPressed(Keys.E)){// применить к текущему выделенному элементу новый тип текстуры
				// лучше заменить номер на ссылку на настоящий объект
				if (moveObjNumCurrent!=null){// элемент выделен
					//var p=(SimpleEditableObject)Editor.GetObject(moveObjNumCurrent);// SetParam(moveObjNumCurrent, "TexNum", textureNum.ToString());
					//if (p!=null){
						moveObjNumCurrent.ObjType = ObjType;
					//}
				}
			}
			//if ((moveObjNumCurrent == -1)) return;// установлен когда курсор мыши близко к любому объекту и не происходит операции перемещения
			if (e.IsKeyPressed(Keys.LButton)){
				if (NewObjClick){
					AddNewObject(e);
					e.KeyboardClear();
					return;
				}
				movePressed = true;// нажатие мыши
				moveCurrent = new Point(e.cursorX, e.cursorY);
				if (!moveOperation){
					if (moveObjNumCurrent == null) return;
					moveObjNum = moveObjNumCurrent;
					moveOperation = true;// признак операции перемещения
					moveStart = new Point(e.cursorX, e.cursorY); ;
				}
			}
			if (e.IsKeyPressed(Keys.Left)) centerX += blockW;
			if (e.IsKeyPressed(Keys.Right)) centerX -= blockW;
			if (e.IsKeyPressed(Keys.Up)) centerY += blockH;
			if (e.IsKeyPressed(Keys.Down)) centerY -= blockH;
		}

		/// <summary>
		/// Собираем разные данные и объединяем их в этом методе
		/// </summary>
		private void MoveProcess()
		{
			if (!moveOperation) return;
			if (movePressed){// операции перемещения
			}
			else{// сохранение перемещения и сброс всех переключателей
				var p = moveObjNum;
				var x1 = p.X + (moveCurrent.X - moveStart.X);
				var y1 = p.Y + (moveCurrent.Y - moveStart.Y);
				x1 = ((x1 + blockW/2)/blockW)*blockW;
				y1 = ((y1 + blockH/2)/blockH)*blockH;
				p.X = x1;// меняем параметры у объекта
				p.Y = y1;
				moveOperation = false;
				moveObjNum = null;
				moveObjNumCurrent = null;
			}
		}

		

		private void AddNewObject(InputEventArgs iea)
		{
			if (!NewObjClick) return;
			// проверяем на координаты, что бы не добавлялось много объектов 
			var x1 = ((iea.cursorX - centerX + blockW/2)/blockH)*blockW;
			var y1 = ((iea.cursorY - centerY + blockH/2)/blockW)*blockH;
			if ((x1 == x1OldCreated) && (y1 == y1OldCreated)) return;
			x1OldCreated = x1;
			y1OldCreated = y1;
			//следующий этап проверки - что бы по указанным координатам небыло ниодного объекта
			var founded = false;
			foreach (SimpleEditableObject p in Editor.Objects()){
				if (p.X == x1){if (p.Y == y1){founded = true;break;}}
			}
			if (!founded){// если такой точки не найдено - добавляем
				var seo = new SimpleEditableObject();
				Editor.AddNewObject(seo);
				seo.X=x1;
				seo.Y=y1;
			}
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.BurlyWood);
			visualizationProvider.Print(200, 100, " EditorUI");
			visualizationProvider.Line(000, 000, 100, 100);
			visualizationProvider.Line(000, 000, 800, 000);
			visualizationProvider.Line(800, 000, 800, 600);
			visualizationProvider.Line(800, 600, 000, 600);
			visualizationProvider.Line(000, 600, 000, 000);
			visualizationProvider.SetColor(Color.Aquamarine);

			var countPoints = 0;
			foreach (SimpleEditableObject p in Editor.Objects())
			{
				int x1 = p.X + centerX;
				int y1 = p.Y + centerY;
				if (x1 > 800) continue;
				if (y1 > 600) continue;
				visualizationProvider.DrawTexturePart(x1, y1, "main", 16, 16, ObjectTypeAtlas.GetTextureNum(p.ObjType));
				countPoints++;
			}

			visualizationProvider.Print(30, visualizationProvider.CanvasHeight - 35, "Количество объектов " + countPoints);
			var n = SearchNearest(Editor.Objects(),cPoint.X - centerX, cPoint.Y - centerY);
			if ((n!=null)&(!moveOperation)){
				// сбрасываем выбранный элемент, если он выходит за границы окна
				if ((n.X + centerX > 800) || (n.Y + centerY > 600)) n = null;
				if (n !=null){
					visualizationProvider.SetColor(Color.IndianRed);
					visualizationProvider.Line(cPoint.X, cPoint.Y, n.X + centerX, n.Y + centerY);
					visualizationProvider.SetColor(Color.Chartreuse);
					visualizationProvider.Circle(n.X + centerX, n.Y + centerY, blockH);
					
				}
			}
			moveObjNumCurrent = n;// сохраняем для последующего использования
			DrawObjectInfo(visualizationProvider, moveObjNumCurrent);

			MoveProcess();
			var s = "";
			if (moveObjNum != null) s += " N " + moveObjNum.Num;
			if (moveObjNumCurrent != null) s += " NC " + moveObjNumCurrent;
			s += " " + moveStart;
			s += "=>" + moveCurrent + "";
			s += " mo " + (moveOperation ? "перемещение" : "нет перемещения");
			s += " mouse " + (movePressed ? "нажато" : "не нажато");
			visualizationProvider.SetColor(Color.WhiteSmoke);
			visualizationProvider.Print(30, visualizationProvider.CanvasHeight - 75, s);
			movePressed = false;// для отлова отпускания кнопки мыши

			// выводим круг при перемещении
			if (moveOperation){// выводим объект в новом месте
				var p = moveObjNum;
				var x1 = p.X + (moveCurrent.X - moveStart.X) + centerX;
				var y1 = p.Y + (moveCurrent.Y - moveStart.Y) + centerY;
				visualizationProvider.SetColor(Color.OrangeRed);
				visualizationProvider.Circle(x1, y1, 25);
				visualizationProvider.DrawTexturePart(x1, y1, "main", 16, 16, ObjectTypeAtlas.GetTextureNum(p.ObjType));
			}

			visualizationProvider.SetColor(Color.WhiteSmoke);
			s = " номер " + ObjType;
			s += " Центр " + centerX + " " + centerY;
			visualizationProvider.Print(70, visualizationProvider.CanvasHeight - 95, s);
			visualizationProvider.DrawTexturePart(30, 600, "main", 16, 16, ObjectTypeAtlas.GetTextureNum(ObjType));
		}

		private SimpleEditableObject SearchNearest(IEnumerable items, int x, int y)
		{
			const int maxdist = 100;// максимальная дистанция
			float dist = maxdist;// устанавливаем сразу "максимальную" дальность
			SimpleEditableObject obj = null;
			foreach (SimpleEditableObject item in items){
				var dist1 = Distance(x, y, item.X, item.Y);
				if (dist1 < dist) { dist = dist1;obj = item;}
			}
			return obj;
		}

		/// <summary>
		/// Обработка раскраски
		/// </summary>
		/// <param name="vp"></param>
		private void DrawObjectInfo(VisualizationProvider vp, SimpleEditableObject viewS)
		{
			//TODO копирование свойств с выделенного объекта
			//TODO попробовать переделать всё на текстуры 32х32
			SimpleEditableObject view = null;
			String sInfo = "";
			if (GetInfoObjectClick){
// выводим информацию о текущем блоке
				view = sample;
				sInfo = "Sample";
			}
			else{
				view = viewS;
				sInfo = "LIVE";
			}
			if (view!=null){
				var y = 400;
				vp.SetColor(Color.Brown);
				vp.Print(840, y-16, sInfo);
				y = DrawObjectInfoOne(vp, y, "Num", view.Num.ToString());
				y = DrawObjectInfoOne(vp, y, "X", view.X.ToString());
				y = DrawObjectInfoOne(vp, y, "Y", view.Y.ToString());
				vp.DrawTexturePart(860, y+16/2+3, "main", 16, 16, ObjectTypeAtlas.GetTextureNum(view.ObjType));
				y = DrawObjectInfoOne(vp, y, "Tex", view.ObjType.ToString());
				
			}
		}

		private static int DrawObjectInfoOne(VisualizationProvider vp, int y, String name, String value)
		{
			vp.SetColor(Color.Chocolate);
			vp.Print(820, y, name);
			vp.SetColor(Color.Wheat);
			vp.Print(880, y, value);
			return y + 16;
		}

		public void SetHandlers()
		{
			if (CanDraw){
				_controller.AddEventHandler("Cursor", CursorMovedEH);
				_controller.AddEventHandler("Keyboard", KeyboardEH);
			}
			else{
				_controller.RemoveEventHandler("Cursor", CursorMovedEH);
				_controller.RemoveEventHandler("Keyboard", KeyboardEH);
			}
		}

	}
}
