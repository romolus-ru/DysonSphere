using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils.Editor;
using Engine.Utils.ExtensionMethods;
using Engine.Views;

namespace SimpleMapEditor
{
	/// <summary>
	/// Установка параметров движения и мигания для блоков
	/// </summary>
	class LayerSimpleEditableObjectMove:Layer<SimpleEditableObject>
	{
		#region Переменные

		private SimpleEditableObject _targeted;// выделенный объект на экране

		private ObjectTypes _objType = ObjectTypes.Empty;// номер текстуры

		private int int1Speed;// на какое расстояние передвигаемся
		private int int2Offset;// скорость перемещения
		private int int3Direction;// направление
		private int int4MaxLenght;// длина пути

		#endregion

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="layerName"></param>
		/// <param name="data">Данные передаются извне - где то будет централизованное хранилище</param>
		/// <param name="parent"></param>
		public LayerSimpleEditableObjectMove(Controller controller, string layerName, Dictionary<int, SimpleEditableObject> data, ViewComponent parent)
			: base(controller, layerName, parent)
		{
			Data = data;
			AddButton(410, 020, 50, 20, "TexNumPrev", "<=", "Предыдущая текстурка", Keys.D);
			AddButton(460, 020, 50, 20, "TexNumNext", "=>", "Следующая текстурка", Keys.F);
			AddButton(110, 020, 50, 20, "1P", "1+", "+", Keys.Y);
			AddButton(165, 020, 50, 20, "1M", "1-", "-", Keys.H);
			AddButton(110, 040, 50, 20, "2P", "2+", "+", Keys.U);
			AddButton(165, 040, 50, 20, "2M", "2-", "-", Keys.J);
			AddButton(110, 060, 50, 20, "3P", "3+", "+", Keys.I);
			AddButton(165, 060, 50, 20, "3M", "3-", "-", Keys.K);
			AddButton(110, 080, 50, 20, "4P", "4+", "+", Keys.O);
			AddButton(165, 080, 50, 20, "4M", "4-", "-", Keys.L);

			Controller.AddEventHandler("TexNumPrev", TexNumPrev);
			Controller.AddEventHandler("TexNumNext", TexNumNext);
			Controller.AddEventHandler("1P", I1P);
			Controller.AddEventHandler("1M", I1M);
			Controller.AddEventHandler("2P", I2P);
			Controller.AddEventHandler("2M", I2M);
			Controller.AddEventHandler("3P", I3P);
			Controller.AddEventHandler("3M", I3M);
			Controller.AddEventHandler("4P", I4P);
			Controller.AddEventHandler("4M", I4M);
			Controller.AddEventHandler("MapChangeMapPos", MapChangeMapPos);
		}

		private void I1P(object sender, EventArgs e)
		{
			_targeted = null;
			int1Speed++;
		}

		private void I1M(object sender, EventArgs e)
		{
			_targeted = null;
			int1Speed--;
		}

		private void I2P(object sender, EventArgs e)
		{
			_targeted = null;
			int2Offset++;
		}

		private void I2M(object sender, EventArgs e)
		{
			_targeted = null;
			int2Offset--;
		}
		private void I3P(object sender, EventArgs e)
		{
			_targeted = null;
			int3Direction++;
			if (int3Direction > 3) int3Direction = 0;
		}

		private void I3M(object sender, EventArgs e)
		{
			_targeted = null;
			int3Direction--;
			if (int3Direction < 0) int3Direction = 3;
		}
		private void I4P(object sender, EventArgs e)
		{
			_targeted = null;
			int4MaxLenght++;
		}

		private void I4M(object sender, EventArgs e)
		{
			_targeted = null;
			int4MaxLenght--;
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
			vp.Print(810, 410, "Тип " + num + " " + _objType);
			vp.Print(810, 425, "Название " + ObjectTypeAtlas.GetDescription(_objType));

			vp.Print(810, 440, "Настройки  | Объект");
			vp.Print(810, 455, "Sp " + int1Speed.ToString().PadLeft(7) + "|" + _targeted.Return(t => t.Int1, -1));
			vp.Print(810, 470, "Of " + int2Offset.ToString().PadLeft(7) + "|" + _targeted.Return(t => t.Int2, -1));
			vp.Print(810, 485, "Di " + GetDirection(int3Direction).PadLeft(7) + "|" + GetDirection(_targeted.Return(t => t.Int3, -1)));
			vp.Print(810, 500, "Ln " + int4MaxLenght.ToString().PadLeft(7) + "|" + _targeted.Return(t => t.Int4, -1));
			vp.Print(810, 515, "____________");

			foreach (var d in Data)
			{
				var o = d.Value;
				int x1 = o.X + MapX;
				int y1 = o.Y + MapY;
				if (x1 < 0) continue;
				if (y1 < 0) continue;
				if (x1 > 800) continue;
				if (y1 > 600) continue;
				if (o.ObjType == ObjectTypes.Moved)
				{
					DrawObjectMoved(vp, x1, y1, o);
					continue;
				}
				DrawObject(vp, x1, y1, o);
			}
			if (_targeted != null){
				vp.SetColor(Color.BurlyWood);
				vp.Circle(_targeted.X + MapX, _targeted.Y + MapY, 30);
			}
			// выводим текущую текстуру
			vp.SetColor(Color.White);
			vp.DrawTexturePart(900, 320, "mainEdit", 32, 32, ObjectTypeAtlas.GetTextureNum(_objType));
			base.DrawObject(vp);
		}

		private string GetDirection(int direction)
		{
			String ret = "";
			if (direction == 0) { ret = "Вправо"; }
			if (direction == 1) { ret = "Вниз"; }
			if (direction == 2) { ret = "Влево"; }
			if (direction == 3) { ret = "Вверх"; }
			return ret;
		}

		private void DrawObjectMoved(VisualizationProvider vp, int x1, int y1, SimpleEditableObject o)
		{
			var num1 = ObjectTypeAtlas.GetTextureNum(o.ObjTypeView);
			vp.SetColor(Color.White, 80);
			vp.DrawTexturePart(x1, y1, "mainEdit", 32, 32, num1);
			int dx = 0;
			int dy = 0;
			int bx = LayerSimpleEditableObject.blockW;
			int by = LayerSimpleEditableObject.blockH;
			if (o.Int3 == 0) dx = 1;
			if (o.Int3 == 1) dy = 1;
			if (o.Int3 == 2) dx = -1;
			if (o.Int3 == 3) dy = -1;
			vp.SetColor(Color.GreenYellow);
			vp.Line(x1, y1, x1 + dx*o.Int4*bx - by/2, y1 + dy*o.Int4*by - bx/2);

			vp.SetColor(Color.White, 70);
			vp.DrawTexturePart(x1+dx*o.Int4*bx, y1+dy*o.Int4*by, "mainEdit", 32, 32, num1);
			vp.SetColor(Color.White, 80);
			vp.DrawTexturePart(x1, y1, "mainEdit", 32, 32, num1);
			
		}

		private void DrawObject(VisualizationProvider vp, int x1, int y1, SimpleEditableObject o)
		{
			var num1 = ObjectTypeAtlas.GetTextureNum(o.ObjType);
			vp.SetColor(Color.White, 30);
			vp.DrawTexturePart(x1, y1, "mainEdit", 32, 32, num1);
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
				if (item.Value.ObjType != ObjectTypes.Moved) continue;
				var dist1 = Distance(x, y, item.Value.X, item.Value.Y);
				if (dist1 < dist) { dist = dist1; obj = item.Value; }
			}
			return obj;
		}

		/// <summary>Округлить координаты по блокам</summary>
		/// <param name="x"></param>
		/// <returns></returns>
		protected int RoundX(int x){return ((x - MapX + LayerSimpleEditableObject.blockW / 2) / LayerSimpleEditableObject.blockH) * LayerSimpleEditableObject.blockW;}

		/// <summary>Округлить координаты по блокам</summary>
		/// <param name="y"></param>
		/// <returns></returns>
		protected int RoundY(int y){return ((y - MapY + LayerSimpleEditableObject.blockH / 2) / LayerSimpleEditableObject.blockW) * LayerSimpleEditableObject.blockH;}

		public override void Keyboard(object sender, InputEventArgs e)
		{
			base.Keyboard(sender, e);
			if (e.Handled) return;// без этого происходит и нажатие на кнопки и нажатие на слой
			if (_targeted == null) return;
			if (e.IsKeyPressed(Keys.RButton)){// копируем настройки
				int1Speed = _targeted.Int1;
				int2Offset = _targeted.Int2;
				int3Direction = _targeted.Int3;
				int4MaxLenght = _targeted.Int4;
				return;
			}
			if (e.IsKeyPressed(Keys.LButton)){// переделываем настройки
				_targeted.Int1 = int1Speed;
				_targeted.Int2 = int2Offset;
				_targeted.Int3 = int3Direction;
				_targeted.Int4 = int4MaxLenght;
				return;
			}

		}
	}
}
