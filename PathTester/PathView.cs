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

		public PathView(Controller controller, ViewComponent parent) : base(controller, parent)
		{}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			StateOneTime = StateOneTime.Init(5);
			Width = visualizationProvider.CanvasWidth;
			Height = visualizationProvider.CanvasHeight;
		}

		private pt cPoint = new pt(10, 10);

		/// <summary>
		/// Отслеживаем перемещение курсора
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="point"></param>
		private void CursorMoved(object sender, PointEventArgs point)
		{
			cPoint = point.Pt;// точка где счас находится курсор
		}

		/// <summary>
		/// Отслеживаем перемещение курсора
		/// </summary>
		/// <param name="o"></param>
		/// <param name="args"></param>
		public override void Cursor(object o, PointEventArgs args)
		{
			base.Cursor(o, args);
			cPoint = args.Pt;// точка где счас находится курсор
		}

		public override void Keyboard(object sender, InputEventArgs e)
		{
			if (e.IsKeyPressed(Keys.RButton))
			{
				p1 = new Point(cPoint.X, cPoint.Y);
			}
			var slb = StateOneTime.Check(e.IsKeyPressed(Keys.LButton));
			if (slb==StatesEnum.On){
				p4 = new Point(cPoint.X, cPoint.Y);
				var p = new Path();
				var pts=new List<Point>();
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
			foreach (var path in paths){
				if (pause == 0){path.num++;}
				Point pt1 = null;
				foreach (Point p in path.Points()){
					if (pt1 == null){pt1 = p;continue;}
					visualizationProvider.SetColor(Color.Bisque, 100);
					visualizationProvider.Line(pt1.X, pt1.Y, p.X, p.Y);
					pt1 = p;
				}
			}
		}

	}
}
