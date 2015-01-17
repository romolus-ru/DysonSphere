// TODO посмотреть что с circle - они не поддерживают прозрачность. или слишком много накладывается линий
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
using Button = Engine.Views.Templates.Button;

namespace SimpleMapEditor
{
	class LayerSimpleEditableObject:Layer<SimpleEditableObject>
	{
		#region Переменные

		/// <summary>
		/// Пауза что бы некоторые обработчики сразу не срабатывали
		/// </summary>
		private StateOneTime _StartPause = StateOneTime.Init(3);

		/// <summary>Размер блока</summary>
		public const int blockH = 32;
		/// <summary>Размер блока</summary>
		public const int blockW = 32;
		/// <summary>Количество блоков</summary>
		public const int countBlocks = 23;

		private Button _addModeButton;
		private Button _paintModeButton;
		private Button _getInfoButton;
		private Button _moveObjectButton;

		private SimpleEditableObject _targeted;// выделенный объект на экране
		private SimpleEditableObject _brush;// для копирования свойств

		private enum Modes
		{
			None,
			AddObject,
			PaintObject,
			GetInfo,
			MoveObject,// режим по умолчанию
		}

		private Modes _mode = Modes.None;

		private int x1OldCreated = -9473;
		private int y1OldCreated = -14748;
		private ObjectTypes textureNum = ObjectTypes.Empty;// номер текстуры
		private bool _dragProcess;// перемещение при включенном режиме перемещения 

		#endregion

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="layerName"></param>
		/// <param name="data">Данные передаются извне - где то будет централизованное хранилище</param>
		/// <param name="parent"></param>
		public LayerSimpleEditableObject(Controller controller, string layerName, Dictionary<int, SimpleEditableObject> data,ViewComponent parent)
			: base(controller, layerName, parent)
		{
			Data = data;
			// добавляем кнопки (некоторые сохраняем, их нужно менять в процессе работы)
			_addModeButton = AddButton(10, 020, 100, 20, "addNewObject", "Добавить",
				"Переключение режима добавления объекта", Keys.A);
			_paintModeButton = AddButton(110, 020, 100, 20, "paintObject", "Красить",
				"Переключение режима раскрашивания объектов", Keys.P);
			_getInfoButton = AddButton(210, 020, 100, 20, "getInfoObject", "Копировать",
				"Режим копирования свойств объекта", Keys.C);
			_moveObjectButton = AddButton(310, 020, 100, 20, "moveObject", "Переместить",
							"Режим перемещения объекта", Keys.X);
			AddButton(410, 020, 50, 20, "TexNumPrev", "<=", "Предыдущая текстурка", Keys.D);
			AddButton(460, 020, 50, 20, "TexNumNext", "=>", "Следующая текстурка", Keys.F);
			
			// добавляем обработчики для событий. так как это простые кнопки, то события чаще всего тоже почти стандартные
			Controller.AddEventHandler("addNewObject", AddNewObject);
			Controller.AddEventHandler("paintObject", PaintObject);
			Controller.AddEventHandler("getInfoObject", GetInfoObject);
			Controller.AddEventHandler("moveObject", MoveObject);

			Controller.AddEventHandler("TexNumPrev", TexNumPrev);
			Controller.AddEventHandler("TexNumNext", TexNumNext);
			Controller.AddEventHandler("MapChangeMapPos", MapChangeMapPos);
		}
		
		/// <summary>
		/// Изменить положение центра карты по переданным координатам
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MapChangeMapPos(object sender, EventArgs e)
		{
			var ea = e as PointEventArgs;
			if (ea != null){
				MapX = ea.Pt.X*blockH+400;
				MapY = ea.Pt.Y*blockW+300;
			}
		}

		private void TexNumNext(object sender, EventArgs e)
		{
			textureNum++;
			if ((int)textureNum > countBlocks){
				textureNum = 0;
			}
		}

		private void TexNumPrev(object sender, EventArgs e)
		{
			textureNum--;
			if (textureNum<0){
				textureNum = (ObjectTypes)countBlocks;
			}
		}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			visualizationProvider.LoadTexture("mainEdit", @"..\Resources\defanceLabirinth2.jpg");// грузим основную текстуру 
			SetCoordinates(0,0,0);
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		private void ResetButtonNames()
		{
			_addModeButton.SetCaption( _mode==Modes.AddObject ? "Добавляем" : "Добавить");
			_paintModeButton.SetCaption(_mode == Modes.PaintObject ? "Раскрашиваем" : "Раскрасить");
			_getInfoButton.SetCaption(_mode == Modes.GetInfo ? "Копируем" : "Копировать");
			_moveObjectButton.SetCaption(_mode == Modes.MoveObject ? "Перемещаем" : "Переместить");
		}

		/// <summary>
		/// Добавить новый объект на слой
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddNewObject(object sender, EventArgs e)
		{
			// сигналим о том что будет добавлен объект при клике по экрану
			if (_mode == Modes.None){_mode = Modes.AddObject;}
			else{_mode = Modes.None;}
			ResetButtonNames();
		}

		/// <summary>
		/// Режим раскрашивания
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PaintObject(object sender, EventArgs e)
		{
			// сигналим о включении режима раскрашивания при клике по экрану
			if (_mode == Modes.None) _mode = Modes.PaintObject;
			else _mode = Modes.None;
			ResetButtonNames();
		}

		/// <summary>
		/// Режим получения информации об объекте
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GetInfoObject(object sender, EventArgs e)
		{
			// сигналим о включении режима копирования при клике по экрану
			if (_mode == Modes.None) _mode = Modes.GetInfo;
			else _mode = Modes.None;
			ResetButtonNames();
		}

		/// <summary>
		/// Режим перемещения объектов
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MoveObject(object sender, EventArgs e)
		{
			// сигналим о включении режима перемещения
			if (_mode == Modes.None) {_mode = Modes.MoveObject;
				IsCanStartDrag = true;// включаем относительное перемещение
			}
			else {_mode = Modes.None;
				IsCanStartDrag = false;// отключаем относительное перемещение
			}
			ResetButtonNames();
		}
		
		public override SimpleEditableObject CreateObject(string objType)
		{
			var r = new SimpleEditableObject();
			return r;
		}

		protected override void DrawObject(VisualizationProvider vp)
		{
			vp.SetColor(Color.AntiqueWhite);
			vp.Print(900, 365, "" + _mode);
			vp.Print(900, 380, " M(" + MapX+","+MapY+")");
			vp.Print(900, 395, " C(" + CursorPoint.X + "," + CursorPoint.Y + ")");
			vp.Print(900, 410, "CF(" + CursorPointFrom.X + "," + CursorPointFrom.Y + ")");
			vp.Print(900, 425, "" + (IsCanStartDrag ? "Перемещение" : "Запрет перемещения"));
			vp.Print(900, 440, "" + (_dragProcess ? "Перемещаем" : "Не перемещаем"));
			//vp.Print(900, 455, "" + (this.Parent.CanDraw ? "Видимый" : "Не видимый"));
			var objectTypes = (ObjectTypes)textureNum;
			var num = ObjectTypeAtlas.GetTextureNum(objectTypes);
			vp.Print(810, 455, "Тип " + num+" "+objectTypes.ToString());
			vp.Print(810, 470, "Название " + ObjectTypeAtlas.GetDescription(objectTypes));
			foreach (var d in Data)
			{
				var o = d.Value;
				int x1 = o.X + MapX;
				int y1 = o.Y + MapY;
				if (x1 < 0) continue;
				if (y1 < 0) continue; 
				if (x1 > 800) continue;
				if (y1 > 600) continue;
				vp.SetColor(Color.White);
				DrawObject(vp, x1, y1, o);
			}
			if (_targeted != null && _mode == Modes.MoveObject){
				vp.SetColor(Color.BurlyWood);
				vp.Circle(_targeted.X + MapX, _targeted.Y + MapY, 38);
			}
			if (_dragProcess){// для перемещения выводим отдельно цель в новых координатах, полупрозрачно
				vp.SetColor(Color.BurlyWood,50);
				int x1 = _targeted.X + MapX - (CursorPointFrom.X - CursorPoint.X);
				int y1 = _targeted.Y + MapY - (CursorPointFrom.Y - CursorPoint.Y);
				DrawObject(vp, x1, y1, _targeted);
				vp.Circle(x1, y1, 43);
			}
			// выводим текущую текстуру
			vp.DrawTexturePart(900, 320, "mainEdit", 32, 32, ObjectTypeAtlas.GetTextureNum(objectTypes));
			base.DrawObject(vp);
		}

		private void DrawObject(VisualizationProvider vp, int x1, int y1, SimpleEditableObject o)
		{
			var num1 = ObjectTypeAtlas.GetTextureNum(o.ObjType);
			if ((o.ObjType == ObjectTypes.Secret) || (o.ObjType == ObjectTypes.Flasher) || (o.ObjType == ObjectTypes.Moved))
			{
				var num2 = ObjectTypeAtlas.GetTextureNum(o.ObjTypeView);
				vp.DrawTexturePart(x1, y1, "mainEdit", 32, 32, num2);
			}
			vp.DrawTexturePart(x1, y1, "mainEdit", 32, 32, num1);
		}

		protected override void MouseClick(int x, int y,Boolean dragStarted)
		{
			switch (_mode){
				case Modes.AddObject:// добавляем объект
					AddObjectOnLayer(x, y);
					break;
			}
		}

		protected override void MouseMove(int x, int y)
		{
			if (!_dragProcess){// когда начинается процесс перемещения - перестаём определять перемещение объекта
				_targeted = FindNearest(x - MapX, y - MapY);
			}
		}

		protected override void DragStart(int x, int y)
		{
			if (_targeted==null){// отменяем перемещение
				DragCancel();_dragProcess = false;return;
			}
			_dragProcess = true;// цель есть - перемещаем её
		}

		protected override void DragEnd(int relX, int relY)
		{
			_targeted.X = RoundX(_targeted.X - relX + MapX);
			_targeted.Y = RoundY(_targeted.Y - relY + MapY);
			_dragProcess = false;
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

		protected SimpleEditableObject FindNearest(int x, int y)
		{
			const int maxdist = 100;// максимальная дистанция
			float dist = maxdist;// устанавливаем сразу "максимальную" дальность
			SimpleEditableObject obj = null;
			foreach (var item in Data)
			{
				var dist1 = Distance(x, y, item.Value.X, item.Value.Y);
				if (dist1 < dist) { dist = dist1; obj = item.Value; }
			}
			return obj;
		}

		/// <summary>
		/// Добавить объект на слой
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private void AddObjectOnLayer(int x, int y)
		{
			var x1 = RoundX(x);
			var y1 = RoundY(y);
			if ((x1 == x1OldCreated) && (y1 == y1OldCreated)) return;
			x1OldCreated = x1;
			y1OldCreated = y1;
			//следующий этап проверки - что бы по указанным координатам небыло ниодного объекта
			var founded = false;
			foreach (SimpleEditableObject p in Data.Values)
			{
				if (p.X == x1) { if (p.Y == y1) { founded = true; break; } }
			}
			if (!founded){// если такой точки не найдено - добавляем
				var seo = new SimpleEditableObject();
				AddObject(seo);
				seo.X = x1;
				seo.Y = y1;
				seo.ObjType = textureNum;
				seo.ObjTypeView=ObjectTypes.Empty;
			}
		}

		/// <summary>Округлить координаты по блокам</summary>
		/// <param name="x"></param>
		/// <returns></returns>
		protected int RoundX(int x){return ((x - MapX + blockW / 2) / blockH) * blockW;}

		/// <summary>Округлить координаты по блокам</summary>
		/// <param name="y"></param>
		/// <returns></returns>
		protected int RoundY(int y){return ((y - MapY + blockH / 2) / blockW) * blockH;}

	}
}
