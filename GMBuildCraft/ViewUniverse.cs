using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;
using Engine.Views;
using Point = Engine.Utils.Path.Point;

namespace GMBuildCraft
{
	class ViewUniverse:ViewControlDraggable
	{
		// TODO тут
		// при генерации путей безье происходит потеря точности, причём довольно таки сильная
		// попробовать найти лучший алгоритм генерации точек безье
		//надо ещё выводить части путей, соединяющие вселенские склады между собой
		//выделение путей и картинки зданий,  в том числе галактических. картинки ресурсов
		//сделать вывод текстуры с растяжением по одной из осей (DrawTextureStretch)
		public Universe Universe;
		private Boolean _viewStarSystem;// что бы разделять когда двигать вьюху от ЗС, а когда от вселенной
		private int _viewUX;// для вселенной
		private int _viewUY;
		private int _scaleU;
		private int _viewSX;// для звёздной системы
		private int _viewSY;
		private int _scaleS;
		private Random rnd;
		private Boolean _dragged = false;
		private StateOne _stateInc = new StateOne();
		private StateOne _stateDec = new StateOne();
		private float _koeff = 0.0f;
		private int _nearestStarSystem = -1;// необходимо найти ближайшие и систему и путь к ней что бы показать информацию
		private int _nearestStarSystemWay = -1;// система выбрана постоянно, но если курсор ближе к пути то вывести информацию о пути
		private int _nearestStarPoint = -1;// необходимо выбирать и путь и узловую точку
		private int _nearestStarPointRoad = -1;// и обоих выводить и выводить вспомогательные окна
		private Point _cursorUniverse = new Point(0, 0);

		public ViewUniverse(Controller controller, ViewComponent parent = null) : base(controller, parent)
		{
			rnd=new Random();
			_scaleS = 1;
			_scaleU = 1;
			IsCanStartDrag = true;
		}

		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			SetCoordinates(0, 0, 0);
			SetSize(1024, 768);
		}

		protected override void DrawObject(VisualizationProvider vp)
		{
			base.DrawObject(vp);
			if (Universe == null) return;
			if (rnd == null) return;
			vp.SetColor(Color.Silver, 70);
			vp.Line(1024/2 + 20, 768/2, 1024/2 - 20, 768/2);
			vp.Line(1024/2, 768/2 + 20, 1024/2, 768/2 - 20);
			int vx = _viewUX;
			int vy = _viewUY;
			if (_dragged){
				vx -= (CursorPointFrom.X - CursorPoint.X);
				vy -= (CursorPointFrom.Y - CursorPoint.Y);
			}
			int vsx = _viewSX;
			int vsy = _viewSY;
			if (_dragged)
			{
				vsx -= (CursorPointFrom.X - CursorPoint.X);
				vsy -= (CursorPointFrom.Y - CursorPoint.Y);
			} 
			vp.SetColor(Color.White);
			vp.Print(10, 10, " view  " + vx + ", " + vy);
			vp.Print(10, 20, " view  " + vsx + ", " + vsy);
			vp.Print(10, 30, "Cursor " + CursorPoint.ToString());
			vp.Print(10, 40, "CursorF"+CursorPointFrom.ToString());
			vp.Print(10, 50, "ScaleU  " + _scaleU.ToString());
			vp.Print(10, 60, "ScaleS  " + _scaleS.ToString());
			vp.Print(10, 70, "koeff  " + _koeff.ToString());
			vp.Print(10, 80, "  cU   " + _cursorUniverse.X + ", " + _cursorUniverse.Y);
			if (!_viewStarSystem){// выводим вселенную
				byte alphaZoom = (byte) ((10 - _scaleU)*10);
				foreach (var point in Universe.Points){
					//if (rnd.Next(10) < 8) continue;
					var x = point.Point.X*_scaleU + vx;
					var y = point.Point.Y*_scaleU + vy;
					vp.SetColor(Color.White, alphaZoom);
					vp.Circle(x, y, 2*_scaleU);
					vp.SetColor(Color.Tomato, (byte) (alphaZoom/2));
					vp.Circle(x, y, 30*_scaleU);
					vp.SetColor(Color.Tan, (byte) (alphaZoom/2));
					vp.Circle(x, y, 60*_scaleU);
				}

				if (_nearestStarSystem != -1){
					var p = Universe.Points[_nearestStarSystem];
					var x = p.Point.X*_scaleU + vx;
					var y = p.Point.Y*_scaleU + vy;
					vp.SetColor(Color.White);
					vp.Circle(x, y, 4*_scaleU);
					vp.SetColor(Color.Red);
					vp.Line(x, y, CursorPoint.X, CursorPoint.Y);
				}

				foreach (var way in Universe.Ways){
					DrawWay(way, vp, vx, vy, _scaleU);
				}
			}
			else{// работает с одной звёздной системой
				var starSystem = Universe.Points[_nearestStarSystem] as StarSystem;
				foreach (var point in starSystem.Points){
					var x = point.Point.X*_scaleS+vsx;
					var y = point.Point.Y*_scaleS+vsy;
					vp.SetColor(Color.White);
					vp.Circle(x, y, 2 * _scaleS);
					vp.SetColor(Color.Tomato, 50);
					vp.Circle(x, y, 30 * _scaleS);
					vp.SetColor(Color.Tan, 50);
					vp.Circle(x, y, 60 * _scaleS);
				}
				if (_nearestStarPoint != -1){
					var p = starSystem.Points[_nearestStarPoint];
					var x = p.Point.X*_scaleS+vsx;
					var y = p.Point.Y*_scaleS+vsy;
					vp.SetColor(Color.White);
					vp.Circle(x, y, 4 * _scaleS);
					vp.SetColor(Color.Red);
					vp.Line(x, y, CursorPoint.X, CursorPoint.Y);
				}
				foreach (var road in starSystem.Roads){
					DrawRoad(road, vp, vsx, vsy, _scaleS);
				}
			}
		}

		public override void Cursor(object sender, PointEventArgs point)
		{
			base.Cursor(sender, point);

			_cursorUniverse = new Point(
					point.Pt.X/_scaleU - _viewUX/_scaleU, 
					point.Pt.Y/_scaleU - _viewUY/_scaleU);
			if (!_viewStarSystem){
				_nearestStarSystem = MapGenerator.SearchNearest(_cursorUniverse, Universe.Points, 0, 100);
			}
			else
			{
				if (_nearestStarSystem >= 0){
					var _cursorUniverseLocal = new Point(
						point.Pt.X / _scaleS - _viewSX / _scaleS,
						point.Pt.Y / _scaleS - _viewSY / _scaleS);
					var starSystem = Universe.Points[_nearestStarSystem] as StarSystem;
					_nearestStarPoint = MapGenerator.SearchNearest(_cursorUniverseLocal, starSystem.Points, 0, 10000);
				}
			}
		}

		public override void Keyboard(object sender, InputEventArgs e)
		{
			base.Keyboard(sender, e);
			if (_stateInc.CurrentState != StatesEnum.On){
				var sInc = _stateInc.Check(e.cursorDelta < 0);
				if (sInc == StatesEnum.On) DecScale(e.cursorX, e.cursorY);
			}
			else{_stateInc.Check(false);} // сбрасываем состояние, что бы срабатывало каждый второй раз

			if (_stateDec.CurrentState != StatesEnum.On){
				var sDec = _stateDec.Check(e.cursorDelta > 0);
				if (sDec == StatesEnum.On) IncScale(e.cursorX, e.cursorY);
			}
			else{_stateDec.Check(false);}
		}

		private void IncScale(int cursorX, int cursorY)
		{
			if ((_scaleU==10)&&(_nearestStarSystem==-1))return;// не позволяем уменьшить масштаб если не выбрана звёздная система
			float koeff;
			_scaleU++;
			if (_scaleU > 10){
				_scaleU = 10;
				_viewStarSystem = true;
				_scaleS++;
				if (_scaleS > 10){
					_scaleS = 10;
				}else{// дополнительно обрабатывает локальное перемещение
					if (_scaleS > 1){
						koeff = _scaleS/(_scaleS - 1.0f);
						_viewSX = (int) (((_viewSX)*koeff) - cursorX*koeff/_scaleS);
						_viewSY = (int) (((_viewSY)*koeff) - cursorY*koeff/_scaleS);
					}
				}
				return;
			}
			// если значение поменялось то надо пересчитать
			koeff = _scaleU/(_scaleU - 1.0f);
			_viewUX = (int)(((_viewUX) * koeff) - cursorX * koeff / _scaleU);
			_viewUY = (int)(((_viewUY) * koeff) - cursorY * koeff / _scaleU);
			_viewSX = 1024/8;
			_viewSY = 768/8;
			_viewStarSystem = false;
			_scaleS = 0;
		}

		private void DecScale(int cursorX, int cursorY)
		{
			_scaleS--;
			if (_scaleS > 0){
				_viewStarSystem = true;
				float koeff = _scaleS / (_scaleS + 1.0f);
				_viewSX = (int)(((_viewSX) * koeff) + cursorX * koeff / _scaleS);
				_viewSY = (int)(((_viewSY) * koeff) + cursorY * koeff / _scaleS);
				return;
			}
			_viewStarSystem = false;
			_scaleS = 0;
			_scaleU--;
			if (_scaleU < 1) _scaleU = 1;
			else{
				float koeff = _scaleU/(_scaleU + 1.0f);
				_viewUX = (int) (((_viewUX)*koeff) + cursorX*koeff/_scaleU);
				_viewUY = (int) (((_viewUY)*koeff) + cursorY*koeff/_scaleU);
				_viewSX = 1024/8;
				_viewSY = 768/8;
			}
		}

		protected override void DragStart(int x, int y)
		{
			base.DragStart(x, y);
			_dragged = true;
		}

		protected override void DragEnd(int relX, int relY)
		{
			_dragged = false;
			if (_viewStarSystem){
				_viewSX -= relX;
				_viewSY -= relY;
			}
			else{
				_viewUX -= relX;
				_viewUY -= relY;
			}
			base.DragEnd(relX, relY);
		}

		private void DrawWay(Road way, VisualizationProvider vp, int vx, int vy, int scale)
		{
			var p1 = (way.NodePoint1 as StarPoint).Parent.Point;
			var p2 = (way.NodePoint2 as StarPoint).Parent.Point;
			var x1 = p1.X*scale + vx;
			var y1 = p1.Y*scale + vy;
			var x2 = p2.X*scale + vx;
			var y2 = p2.Y*scale + vy;
			var dx = (x2 - x1) / 6;
			var dy = (y2 - y1) / 6;
			vp.SetColor(Color.Turquoise, 40);
			vp.Line(x1 + dx, y1 + dy, x2 - dx, y2 - dy);
			vp.SetColor(Color.Pink);
			var p = way.Path;
			for (int i = 1; i < p.Count; i++){
				vp.Line((int) (p[i - 1].X*scale + vx), (int) (p[i - 1].Y*scale + vy), (int) (p[i].X*scale + vx), (int) (p[i].Y*scale + vy));
			}
		}

		private void DrawRoad(Road road, VisualizationProvider vp, int vsx, int vsy, int scale)
		{
			var p1 = (road.NodePoint1 as StarPoint).Point;
			var p2 = (road.NodePoint2 as StarPoint).Point;
			var x1 = p1.X * scale + vsx;
			var y1 = p1.Y * scale + vsy;
			var x2 = p2.X * scale + vsx;
			var y2 = p2.Y * scale + vsy;
			var dx = (x2 - x1) / 6;
			var dy = (y2 - y1) / 6;
			vp.SetColor(Color.Turquoise, 40);
			vp.Line(x1 + dx, y1 + dy, x2 - dx, y2 - dy);
			vp.SetColor(Color.Pink);
			var p = road.Path;
			for (int i = 1; i < p.Count; i++)
			{
				vp.Line((int) (p[i - 1].X * scale + vsx), (int) (p[i - 1].Y * scale + vsy), (int) (p[i].X * scale + vsx), (int) (p[i].Y * scale + vsy));
			}
		}
	}
}
