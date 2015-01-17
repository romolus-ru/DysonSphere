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
	class LayerSimpleEditableObjectMap : Layer<SimpleEditableObject>
	{
		public LayerSimpleEditableObjectMap(Controller controller, string layerName,
			Dictionary<int, SimpleEditableObject> data,ViewComponent parent) : base(controller, layerName, parent)
		{
			Data = data;
			//AddButton(210, 100, 200, 20, "ExitFullView", "exit", "Выйти из режима просмотра", Keys.Escape);
			// обработка события должна быть в более общем классе
		}

		public override void Keyboard(object sender, InputEventArgs e)
		{
			base.Keyboard(sender, e);
			if (e.IsKeyPressed(Keys.LButton)){
				Controller.StartEvent("MapChangeMapPos", this, PointEventArgs.Set(MapX - e.cursorX, MapY - e.cursorY));
				Controller.StartEvent("ExitFullView");
			}
		}

		/// <summary>
		/// Размер блока
		/// </summary>
		protected const int blockH = 1;
		/// <summary>
		/// Размер блока
		/// </summary>
		protected const int blockW = 1;

		protected override void DrawObject(VisualizationProvider vp)
		{
			vp.SetColor(Color.Green);
			vp.Print(900, 350, "Режим");
			vp.SetColor(Color.Yellow);
			vp.Print(900, 365, "");
			vp.Print(900, 380, " M(" + MapX+","+MapY+")");
			vp.Print(900, 395, " C(" + CursorPoint.X + "," + CursorPoint.Y + ")");
			foreach (var d in Data)
			{
				var o = d.Value;
				int x1 = o.X/16 + MapX;
				int y1 = o.Y/16 + MapY;
				if (x1 > 800) continue;
				if (y1 > 600) continue;
				vp.SetColor(Color.White);
				vp.Rectangle(x1, y1, 1, 1);
			}
			vp.Rectangle(CursorPoint.X-20,CursorPoint.Y-15,40,30);
			base.DrawObject(vp);
		}
	}
}
