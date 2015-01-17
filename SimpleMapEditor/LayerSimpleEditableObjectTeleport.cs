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
	/// <summary>
	/// Слой для настройки телепортов. int1 и int2 содержат относительные координаты
	/// </summary>
	class LayerSimpleEditableObjectTeleport:Layer<SimpleEditableObject>
	{
		#region Переменные

		private SimpleEditableObject _targeted;// выделенный объект на экране

		private bool _dragProcess;// перемещение при включенном режиме перемещения 

		#endregion

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="layerName"></param>
		/// <param name="data">Данные передаются извне - где то будет централизованное хранилище</param>
		/// <param name="parent"></param>
		public LayerSimpleEditableObjectTeleport(Controller controller, string layerName, Dictionary<int, SimpleEditableObject> data, ViewComponent parent)
			: base(controller, layerName, parent)
		{
			Data = data;
			Controller.AddEventHandler("MapChangeMapPos", MapChangeMapPos);
			IsCanStartDrag = true;
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
				MapX = ea.Pt.X*LayerSimpleEditableObject.blockH + 400;
				MapY = ea.Pt.Y*LayerSimpleEditableObject.blockW + 300;
			}
		}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			SetCoordinates(0,0,0);
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		public override SimpleEditableObject CreateObject(string objType)
		{
			var r = new SimpleEditableObject();
			return r;
		}

		protected override void DrawObject(VisualizationProvider vp)
		{
			vp.SetColor(Color.AntiqueWhite);
			vp.Print(900, 380, " M(" + MapX+","+MapY+")");
			vp.Print(900, 395, " C(" + CursorPoint.X + "," + CursorPoint.Y + ")");
			vp.Print(900, 410, "CF(" + CursorPointFrom.X + "," + CursorPointFrom.Y + ")");
			foreach (var d in Data)
			{
				var o = d.Value;
				int x1 = o.X + MapX;
				int y1 = o.Y + MapY;
				if (o.ObjType == ObjectTypes.Teleport){
					DrawObject(vp, x1, y1, o);
					continue;
				}
				if (x1 < 0) continue;
				if (y1 < 0) continue; 
				if (x1 > 800) continue;
				if (y1 > 600) continue;
				vp.SetColor(Color.White, 40);
				vp.DrawTexturePart(x1, y1, "mainEdit", 32, 32, ObjectTypeAtlas.GetTextureNum(o.ObjType));
			}
			if (_targeted != null){
				vp.SetColor(Color.BurlyWood);
				vp.Circle(_targeted.X + _targeted.Int1 + MapX, _targeted.Y + _targeted.Int2 + MapY, 14);
			}
			if (_dragProcess&&_targeted!=null){// для перемещения выводим отдельно цель в новых координатах, полупрозрачно
				vp.SetColor(Color.BurlyWood,50);
				int x1 = _targeted.X+_targeted.Int1 + MapX - (CursorPointFrom.X - CursorPoint.X);
				int y1 = _targeted.Y+_targeted.Int2 + MapY - (CursorPointFrom.Y - CursorPoint.Y);
				vp.DrawTexturePart(x1, y1, "mainEdit", 32, 32, ObjectTypeAtlas.GetTextureNum(ObjectTypes.Wall1));
				vp.Circle(x1, y1, 43);
			}
			base.DrawObject(vp);
		}

		private void DrawObject(VisualizationProvider vp, int x1, int y1, SimpleEditableObject o)
		{
			var num1 = ObjectTypeAtlas.GetTextureNum(o.ObjType);
			vp.SetColor(Color.White);
			vp.DrawTexturePart(x1, y1, "mainEdit", 32, 32, num1);
			vp.Line(x1, y1, x1 + o.Int1, y1 + o.Int2);
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
			_targeted.Int1 = RoundX(_targeted.Int1 - relX);
			_targeted.Int2 = RoundY(_targeted.Int2 - relY);
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
				var o = item.Value;
				if (o.ObjType != ObjectTypes.Teleport) continue;
				var dist1 = Distance(x, y, o.X + o.Int1, o.X + o.Int2);
				if (dist1 < dist) { dist = dist1; obj = item.Value; }
			}
			return obj;
		}

		/// <summary>Округлить координаты по блокам</summary>
		/// <param name="x"></param>
		/// <returns></returns>
		protected int RoundX(int x) { return ((x - MapX + LayerSimpleEditableObject.blockW/2) / LayerSimpleEditableObject.blockH) * LayerSimpleEditableObject.blockW; }

		/// <summary>Округлить координаты по блокам</summary>
		/// <param name="y"></param>
		/// <returns></returns>
		protected int RoundY(int y) { return ((y - MapY + LayerSimpleEditableObject.blockH/2) / LayerSimpleEditableObject.blockW) * LayerSimpleEditableObject.blockH; }

	}
}
