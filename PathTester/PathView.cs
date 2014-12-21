using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils.Path;
using Engine.Views;
using Point = Engine.Utils.Path.Point;
using pt = System.Drawing.Point;
using Engine.Utils;

namespace PathTester
{
	class PathView : ViewComponent
	{

		private Random rnd = new Random();
		private Point p1 = new Point(0, 0);
		private Point p4 = new Point(1024, 768);
		private List<Path> paths = new List<Path>();
		private StateOneTime StateOneTime;

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			visualizationProvider.LoadTexture("hider", @"..\Resources\hider001.jpg");// грузим текстуру основную
			StateOneTime = StateOneTime.Init(5);
		}

		private pt cPoint = new pt(10, 10);

		public PathView(Controller controller, ViewComponent parent)
			: base(controller, parent)
		{
			Controller.AddEventHandler("Keyboard", KeyboardEH);
			Controller.AddEventHandler("Cursor", CursorMovedEH);
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

		public override void Keyboard(object sender, InputEventArgs e)
		{
			if (e.IsKeyPressed(Keys.RButton))
			{
				p1 = new Point(cPoint.X, cPoint.Y);
			}
			var slb = StateOneTime.Check(e.IsKeyPressed(Keys.LButton));
			// TODO ---->>>
			//тут. определить почему пауза между нажатиями не сильно регулируется
			//и кнопка должна полностью гасить событие. может быть не хватает "handled"
			//и опять чувствуется будет проблема с выяснением кто выше и кто должен обработать событие. в данном случае кнопка или компонент
			if (slb==StatesEnum.On){
				p4 = new Point(cPoint.X, cPoint.Y);
				Path p = new Path();
				List<Point> pts=new List<Point>();
				pts.Add(p1);
				pts.Add(new Point(rnd.Next(1024),rnd.Next(768)));
				pts.Add(new Point(rnd.Next(1024), rnd.Next(768)));
				pts.Add(p4);
				p.AddPointsOneSegment(pts, 40, new PathGeneratorBezier());
				paths.Add(p);
			}
		}

		private int pause = 0;

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.White, 80);
			visualizationProvider.DrawTexture(1024 / 2, 768 / 2, "hider");

			foreach (var path in paths)
			{
				if (pause == 0) { path.num++; }
				Point pt1 = null;
				foreach (Point p in path.Points())
				{
					if (pt1 == null) { pt1 = p; continue; }
					visualizationProvider.SetColor(Color.Bisque, 100);
					visualizationProvider.Line(pt1.X, pt1.Y, p.X, p.Y);
					pt1 = p;
				}
			}

		}

	}
}
