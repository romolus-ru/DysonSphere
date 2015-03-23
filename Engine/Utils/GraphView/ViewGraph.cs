using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;
using Button = Engine.Views.Templates.Button;

namespace Engine.Utils.GraphView
{
	/// <summary>
	/// Класс для визуализации массивов данных
	/// </summary>
	public class ViewGraph:ViewModal
	{
		public ViewGraph(Controller controller, ViewComponent parent, string outEvent, string destroyEvent) : 
			base(controller, parent, outEvent, destroyEvent)
		{}

		private int beginX = 100;
		private int beginY = 100;
		private int ScreenH = 768;
		private int RayLenght = 30;
		private List<GraphicLine> _gls = new List<GraphicLine>();

		private int countbutton = 0;
		/// <summary>
		/// Список точек, над которыми находится курсор
		/// </summary>
		private List<Tuple<int, GraphicLine, PointF>> _selected = new List<Tuple<int, GraphicLine, PointF>>();

		private Point _cursor;

		/// <summary>
		/// Текущая линия, с которой работаем
		/// </summary>
		private GraphicLine _currentLine = null;

		/// <summary>
		/// Добавить новую линию
		/// </summary>
		/// <param name="lineName"></param>
		/// <param name="color"></param>
		public GraphicLine AddGraphLine(String lineName, Color color)
		{
			var l = GetLine(lineName);
			l.Color = color;
			return l;
		}

		/// <summary>
		/// Добавить новою линию и точки для неё
		/// </summary>
		/// <param name="lineName"></param>
		/// <param name="color"></param>
		/// <param name="points"></param>
		public void AddGraphLine(String lineName, Color color, List<PointF> points)
		{
			var l = AddGraphLine(lineName, color);
			foreach (var point in points){
				l.Add(point);
			}
			SetParams();
		}

		/// <summary>
		/// Добавить новую линию и 
		/// </summary>
		/// <param name="lineName"></param>
		/// <param name="color"></param>
		/// <param name="points"></param>
		public void AddGraphLine(String lineName, Color color, List<float> points)
		{
			var l = AddGraphLine(lineName, color);
			foreach (var point in points){
				l.AddValue(point);
			}
			SetParams();
		}

		/// <summary>
		/// Добавить точку к линии
		/// </summary>
		/// <param name="lineName"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void AddPoint(string lineName, float x, float y)
		{
			AddPoint(lineName, new PointF(x, y));
		}

		/// <summary>
		/// Добавить точку к линии
		/// </summary>
		/// <param name="lineName"></param>
		/// <param name="point"></param>
		public void AddPoint(string lineName, PointF point)
		{
			if (_currentLine == null) _currentLine = GetLine(lineName);
			if (_currentLine.LineName != lineName) _currentLine = GetLine(lineName);
			_currentLine.Add(point);
			SetParams();
		}

		/// <summary>
		/// Добавить точку к линии
		/// </summary>
		/// <param name="lineName"></param>
		/// <param name="value"></param>
		public void AddPointValue(string lineName, int value)
		{
			if (_currentLine == null) _currentLine = GetLine(lineName);
			if (_currentLine.LineName != lineName) _currentLine = GetLine(lineName);
			_currentLine.AddValue(value);
			SetParams();
		}

		/// <summary>
		/// Получить линию. если такой линии нету - создать 
		/// </summary>
		/// <param name="lineName"></param>
		/// <returns></returns>
		private GraphicLine GetLine(string lineName)
		{
			var l=_gls.FirstOrDefault(obj=>obj.LineName==lineName);
			if (l == null){// ненашли такую строку - создаём новую
				l = new GraphicLine();
				l.LineName = lineName;
				_gls.Add(l);
				AddHideButton(l);
			}
			return l;
		}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			this.SetCoordinates(0, 0, 0);
			this.SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.Box(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		/// <summary>
		/// Устанавливаем параметры - группировку областей, масштаб для каждой линии и подобное
		/// </summary>
		private void SetParams()
		{
			if (_gls.Count < 1) return;
			// вычислить для каждой линии масштаб
			foreach (var gl in _gls){// приводим все объекты к масштабу 800х600(сам график должен вписаться в эти размеры)
				var w = (gl.maxX - gl.minX); if (w < 0.1f) w = 1;
				var h = (gl.maxY - gl.minY); if (h < 0.1f) h = 1;
				var scaleX1 = 800/w;
				if (scaleX1 < 0.01f) scaleX1 = 1;
				var scaleY1 = 600/h;
				float scaleAll = 0f;
				if (scaleY1 > scaleX1) scaleAll = scaleX1;
				else scaleAll = scaleY1;
				gl.Scale = scaleAll;

				// рассчитываем точки которые теперь будут выводиться
				gl.PointsView.Clear();
				foreach (var point in gl.Points){
					var x1 = (int)(beginX + (point.X-gl.minX) * gl.Scale);
					var y1 = ScreenH-(int)(beginY + (point.Y-gl.minY) * gl.Scale);
					gl.PointsView.Add(new PointF(x1, y1));
				}
			}
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			int n = 0;

			visualizationProvider.SetColor(Color.DarkSlateBlue);
			visualizationProvider.Rectangle(beginX, ScreenH-beginY, 800, -600);

			foreach (var gl in _gls){
				n++;
				if (!gl.Visible) continue;
				DrawAxis(visualizationProvider, beginX, beginY, n, gl);
				Boolean first = true;
				int x1old=0;
				int y1old=0;
				foreach (var point in gl.PointsView){
					var x1 = (int)point.X;
					var y1 = (int)point.Y;
					DrawRayX(visualizationProvider, gl, x1, y1);
					if (first){// первый ход цикла - сохраняем текущую точку, потом будет рисовать линию до неё
						first = false;
						x1old = x1;
						y1old = y1;
						continue;
					}
					visualizationProvider.SetColor(gl.Color);
					visualizationProvider.Line(x1, y1, x1old, y1old);
					x1old = x1;
					y1old = y1;
				}
			}
			n = 0;
			foreach (var t in _selected){
				n++;
				visualizationProvider.SetColor(t.Item2.Color);
				visualizationProvider.Print(_cursor.X, _cursor.Y + n*15,
					"" + t.Item2.LineName + " (" + t.Item3.X + "," + t.Item3.Y + ")");
			}
		}

		/// <summary>
		/// Выводим оси текущей точки
		/// </summary>
		/// <param name="visualizationProvider"></param>
		/// <param name="gl"></param>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		private void DrawRayX(VisualizationProvider visualizationProvider, GraphicLine gl, int x1, int y1)
		{
			visualizationProvider.SetColor(gl.Color, 20);
			visualizationProvider.Circle(x1, y1, 10);
			visualizationProvider.Line(x1 - RayLenght, y1, x1 + RayLenght, y1);
			visualizationProvider.Line(x1, y1 - RayLenght, x1, y1 + RayLenght);
		}

		private void DrawAxis(VisualizationProvider visualizationProvider, int bX, int bY, int num, GraphicLine gl)
		{
			visualizationProvider.SetColor(gl.Color);
			int x1 = bX - num*10;
			int x2 = x1;
			int y1 = ScreenH-(int) (bY + (gl.maxY - gl.minY)*gl.Scale);
			int y2 = ScreenH-bY;
			visualizationProvider.Line(x1, y1, x2, y2);
			x1 = (int) (bX + (gl.maxX - gl.minX)*gl.Scale);
			x2 = bX;
			y1 = ScreenH-(bY - num * 10);
			y2 = y1;
			visualizationProvider.Line(x1, y1, x2, y2);
			visualizationProvider.Print(x2,y1-5,gl.LineName);

		}

		private void AddHideButton(GraphicLine gl)
		{
			countbutton++;
			var b = Button.CreateButton(Controller, 900, countbutton * 20, 150, 15, "ViewGraphButton", gl.LineName + " видима", 
				"hint", Keys.None, "btn" + (countbutton - 1));
			//b.SetCaption(gl.LineName+" видима");
			this.AddComponent(b);
		}

		public override void Cursor(object o, PointEventArgs args)
		{
			base.Cursor(o, args);
			_cursor = args.Pt;

			_selected.Clear();
			foreach (var gl in _gls){
				if (!gl.Visible) continue;
				int c = 0;
				for (int i = 0; i < gl.Points.Count; i++){
					var p = gl.PointsView[i];
					if (Distance(p.X, p.Y, _cursor.X, _cursor.Y) < 30){
						c++;// вычисляем по экранным кородинатам, но сохраняем реальные, как они записаны в списке
						_selected.Add(new Tuple<int, GraphicLine, PointF>(c, gl, gl.Points[i]));
						if (c > 4) break; // не больше 5 точек на линию
					}
				}
			}
		}

		/// <summary>
		/// Вычисляем расстояние между координатами
		/// </summary>
		/// <returns></returns>
		public float Distance(float x, float y, int X, int Y)
		{
			var dx = x - X;
			var dy = y - Y;
			return (float)Math.Sqrt(dx * dx + dy * dy);
		}

		protected override void HandlersAdder()
		{
			base.HandlersAdder();
			Controller.AddEventHandler("ViewGraphButton", ViewGraphButton);
		}

		protected override void HandlersRemover()
		{
			Controller.RemoveEventHandler("ViewGraphButton", ViewGraphButton);
			base.HandlersRemover();
		}

		private void ViewGraphButton(object sender, EventArgs e)
		{
			var a = sender as Button;
			if (a == null) return;
			var s = a.Name.Substring(3);
			int n;
			int.TryParse(s, out n);
			if (n >= 0)
				_gls[n].Visible = !_gls[n].Visible;
			s = _gls[n].LineName+" ";
			if (_gls[n].Visible) s += "видима";
			else s += "скрыта";
			a.SetCaption(s);
			SetParams();
		}
	}
}
