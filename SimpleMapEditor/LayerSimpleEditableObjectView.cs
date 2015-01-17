using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils.Editor;
using Engine.Views;

namespace SimpleMapEditor
{
	/// <summary>
	/// Установка отображаемой текстуры для секретных подвижных и мигающих блоков
	/// </summary>
	class LayerSimpleEditableObjectView:Layer<SimpleEditableObject>
	{
		#region Переменные

		private SimpleEditableObject _targeted;// выделенный объект на экране

		private ObjectTypes _objType = ObjectTypes.Empty;// номер текстуры

		#endregion

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="layerName"></param>
		/// <param name="data">Данные передаются извне - где то будет централизованное хранилище</param>
		/// <param name="parent"></param>
		public LayerSimpleEditableObjectView(Controller controller, string layerName, Dictionary<int, SimpleEditableObject> data, ViewComponent parent)
			: base(controller, layerName, parent)
		{
			Data = data;
			// добавляем кнопки (некоторые сохраняем, их нужно менять в процессе работы)
			AddButton(410, 020, 50, 20, "TexNumPrev", "<=", "Предыдущая текстурка", Keys.D);
			AddButton(460, 020, 50, 20, "TexNumNext", "=>", "Следующая текстурка", Keys.F);
			
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
				MapX = ea.Pt.X*LayerSimpleEditableObject.blockH+400;
				MapY = ea.Pt.Y*LayerSimpleEditableObject.blockW+300;
			}
		}

		private void TexNumNext(object sender, EventArgs e)
		{
			_objType++;
			if ((int)_objType > LayerSimpleEditableObject.countBlocks){
				_objType = 0;
			}
		}

		private void TexNumPrev(object sender, EventArgs e)
		{
			_objType--;
			if (_objType<0){
				_objType = (ObjectTypes)LayerSimpleEditableObject.countBlocks;
			}
		}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			SetCoordinates(0,0,0);
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		protected override void DrawObject(VisualizationProvider vp)
		{
			vp.SetColor(Color.AntiqueWhite);
			vp.Print(900, 380, " M(" + MapX+","+MapY+")");
			vp.Print(900, 395, " C(" + CursorPoint.X + "," + CursorPoint.Y + ")");
			vp.Print(900, 410, "CF(" + CursorPointFrom.X + "," + CursorPointFrom.Y + ")");
			var num = ObjectTypeAtlas.GetTextureNum(_objType);
			vp.Print(810, 455, "Тип " + num + " " + _objType);
			vp.Print(810, 470, "Название " + ObjectTypeAtlas.GetDescription(_objType));
			foreach (var d in Data){
				var o = d.Value;
				int x1 = o.X + MapX;
				int y1 = o.Y + MapY;
				if (x1 < 0) continue;
				if (y1 < 0) continue;
				if (x1 > 800) continue;
				if (y1 > 600) continue;
				DrawObject(vp, x1, y1, o);
			}
			if (_targeted != null){
				vp.SetColor(Color.BurlyWood);
				vp.Circle(_targeted.X + MapX, _targeted.Y + MapY, 38);
			}
			// выводим текущую текстуру
			vp.DrawTexturePart(900, 320, "mainEdit", 32, 32, ObjectTypeAtlas.GetTextureNum(_objType));
			base.DrawObject(vp);
		}

		private void DrawObject(VisualizationProvider vp, int x1, int y1, SimpleEditableObject o)
		{
			var num1 = ObjectTypeAtlas.GetTextureNum(o.ObjType);
			vp.SetColor(Color.White, 30);
			if ((o.ObjType == ObjectTypes.Secret) || (o.ObjType == ObjectTypes.Flasher) || (o.ObjType == ObjectTypes.Moved))
			{
				vp.SetColor(Color.White);
				var num2 = ObjectTypeAtlas.GetTextureNum(o.ObjTypeView);
				vp.DrawTexturePart(x1, y1, "mainEdit", 32, 32, num2);
			}
			vp.DrawTexturePart(x1, y1, "mainEdit", 32, 32, num1);
		}

		protected override void MouseClick(int x, int y,Boolean dragStarted)
		{
			if (_targeted != null) ChangeObjectView(_targeted);
		}

		protected override void MouseMove(int x, int y)
		{
			_targeted = FindNearest(x - MapX, y - MapY);
		}

		/// <summary>
		/// Вычисляем расстояние между координатами
		/// </summary>
		/// <returns></returns>
		public float Distance(int x, int y, int x1, int y1)
		{
			var dx = x - x1;
			var dy = y - y1;
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
		private void ChangeObjectView(SimpleEditableObject target)
		{
			target.ObjTypeView = _objType;
		}

		/// <summary>Округлить координаты по блокам</summary>
		/// <param name="x"></param>
		/// <returns></returns>
		protected int RoundX(int x){return ((x - MapX + LayerSimpleEditableObject.blockW / 2) / LayerSimpleEditableObject.blockH) * LayerSimpleEditableObject.blockW;}

		/// <summary>Округлить координаты по блокам</summary>
		/// <param name="y"></param>
		/// <returns></returns>
		protected int RoundY(int y){return ((y - MapY + LayerSimpleEditableObject.blockH / 2) / LayerSimpleEditableObject.blockW) * LayerSimpleEditableObject.blockH;}

	}
}
