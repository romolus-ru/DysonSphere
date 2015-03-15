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
using GMBuildCraft.BaseClasses;
using Point = Engine.Utils.Path.Point;

namespace GMBuildCraft
{
	class ViewUniverse:ViewControlDraggable
	{
		// TODO тут
		//http://www.cyberforum.ru/opengl/thread759440.html
		// Объект для передачи данных из модели во вьюшку.
		// генерация дорог возможно содержит ошибки
		// точки должны содержать пути, выводить информацию надо по ним
		// нарисовать белые стрелки, а поверх них будут выводиться кружки нужного цвета или прямоугольники
		// сделать рендер в текстуру. это позволит рендерить 10 текстур для частиц и пользователь почти не заметить разницы. да и в некоторых ситуациях должно помочь. + сделать метод для размытия текстур
		// всех ресурсов на всех планетах должно быть в 3 раза больше чем суммарные требуемые ресурсы (может быть в 4 или даже в 5)
		// потерянные ресурсы тоже надо считать. если потерянных ресурсов будет много, то значит задача выполнена не будет
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
		private ResourcesDesctiptions _rd;
		private Boolean _IsSetDirection = false;// направление
		private int _currDirection;
		public double F;
		private int _rotateResource;

		public ViewUniverse(Controller controller, ViewComponent parent = null) : base(controller, parent)
		{
			rnd=new Random();
			_scaleS = 1;
			_scaleU = 1;
			IsCanStartDrag = true;
			_rotateResource = 0;
		}

		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{}

		public override void Init(VisualizationProvider vp)
		{
			base.Init(vp);
			SetCoordinates(0, 0, 0);
			SetSize(1024, 768);
			_rd = ResourcesDesctiptions.rd;
			ResourcesDescription o ;
			o = _rd.GetInfo(ResourceEnum.Sepulki); vp.LoadTexture(o.TexName, o.TexAddress);
			o = _rd.GetInfo(ResourceEnum.Markwi); vp.LoadTexture(o.TexName, o.TexAddress);
			o = _rd.GetInfo(ResourceEnum.Pchmy); vp.LoadTexture(o.TexName, o.TexAddress);
			o = _rd.GetInfo(ResourceEnum.Iiont); vp.LoadTexture(o.TexName, o.TexAddress);
			o = _rd.GetInfo(ResourceEnum.Technologies); vp.LoadTexture(o.TexName, o.TexAddress);
			o = _rd.GetInfo(ResourceEnum.Artefacts); vp.LoadTexture(o.TexName, o.TexAddress);
			vp.LoadTexture("GMBuildCraftResBgr", @"..\Resources\gmBuildCraft\res_bgr.jpg");
			vp.LoadTexture("GMBuildCraftResEmpty", @"..\Resources\gmBuildCraft\res_empty.jpg");
			vp.LoadTexture("GMBuildCraftStarSystem", @"..\Resources\gmBuildCraft\starSystem.jpg");
			vp.LoadTexture("GMBuildCraftBldBgr", @"..\Resources\gmBuildCraft\bld_bgr.jpg");
			vp.LoadTexture("GMBuildCraftBldNone", @"..\Resources\gmBuildCraft\bld_none.jpg");
			vp.LoadTexture("GMBuildCraftArrowDir", @"..\Resources\gmBuildCraft\arrow_direction.png");
			vp.LoadTexture("GMBuildCraftArrowLine", @"..\Resources\gmBuildCraft\arrow_line.png");
			vp.LoadTexture("GMBuildCraftArrowLineRound", @"..\Resources\gmBuildCraft\arrow_line_rounded.png");
			var g = new RandomNameGenerator();
			for (int i = 0; i < 50; i++){
				n1.Add(g.GenerateRusName());
				n2.Add(g.GenerateRusName());
				n3.Add(g.GenerateRusName());
			}
		}

		private List<string> n1 = new List<string>();
		private List<string> n2 = new List<string>();
		private List<string> n3 = new List<string>();

		protected override void DrawObject(VisualizationProvider vp)
		{
			base.DrawObject(vp);
			vp.SetColor(Color.White);
			for (int i = 0; i < 50; i++){
				vp.Print(10, i * 15, n1[i]);
				vp.Print(300, i * 15, n2[i]);
				vp.Print(600, i * 15, n3[i]);
			}

			return;

			_rotateResource++;if (_rotateResource >= 360) _rotateResource = 0;
			//vp.SetColor(Color.Red);vp.Print(921,11,"Universe");
			vp.SetColor(Color.White);vp.Print(920, 10, "Universe");
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
					//vp.SetColor(Color.Tomato, (byte) (alphaZoom/2));vp.Circle(x, y, 30*_scaleU);
					//vp.SetColor(Color.Tan, (byte) (alphaZoom/2));vp.Circle(x, y, 60*_scaleU);
					vp.SetColor(Color.White,30);
					vp.DrawTexture(x, y, "GMBuildCraftStarSystem", (float)_scaleU/6);
				}

				if (_nearestStarSystem != -1){
					var p = Universe.Points[_nearestStarSystem];
					var x = p.Point.X*_scaleU + vx;
					var y = p.Point.Y*_scaleU + vy;
					vp.SetColor(Color.White);
					vp.Circle(x, y, 4*_scaleU);
					vp.SetColor(Color.Red);
					vp.Line(x, y, CursorPoint.X, CursorPoint.Y);
					//DrawAvailableResourcesStarSystem(vp, (StarSystem)p, x, y);
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
					//vp.SetColor(Color.White, 30);
					//vp.Circle(x, y, 2 * _scaleS);
					//vp.SetColor(Color.Tomato, 50);vp.Circle(x, y, 30 * _scaleS);
					//vp.SetColor(Color.Tan, 50);vp.Circle(x, y, 60 * _scaleS);
					//vp.SetColor(Color.White, 70);
					//vp.DrawTexture(x, y, "GMBuildCraftBldBgr", (float)_scaleS / 6);
					vp.SetColor(Color.White, 100);
					vp.DrawTexture(x, y, "GMBuildCraftBldNone", (float)_scaleS / 4);
					vp.SetColor(Color.Yellow);// расстояние с которого можно задать направление пути
					vp.Circle(x, y, 9 * _scaleS);
					if ((point as StarPoint).BuildingType == BuildingType.StorageUniverse)
						vp.Circle(x, y, 9*_scaleS/2);

				}
				if (_nearestStarPoint != -1){
					var p = starSystem.Points[_nearestStarPoint];
					var ps = p as StarPoint;
					var x = p.Point.X*_scaleS+vsx;
					var y = p.Point.Y*_scaleS+vsy;
					vp.SetColor(Color.White);
					vp.Circle(x, y, 4 * _scaleS);
					vp.SetColor(Color.Red);
					vp.Line(x, y, CursorPoint.X, CursorPoint.Y);
					//DrawAvailableResources(vp, ps, x, y);
					if (_IsSetDirection) DrawDirection(vp, CursorPoint, starSystem, ps, x, y, vsx, vsy, _scaleS);
				}
				foreach (var road in starSystem.Roads){
					DrawRoad(road, vp, vsx, vsy, _scaleS);
				}
				foreach (var road in Universe.Ways){
					DrawRoadU(road, vp, vsx, vsy, _scaleS,starSystem);
				}				
			}
			DrawResourcesSigns(vp);
			//DrawRotatedResources(vp,300,300,_scaleU);
		}

		private void DrawRotatedResources(VisualizationProvider vp, int x, int y, int scaleU)
		{
			int r = 60;
			var a = (_rotateResource+90)*Math.PI/180;
			vp.Rotate(_rotateResource+90);
			vp.DrawTexture((int) (x + r * Math.Cos(a)), (int) (y + r * Math.Sin(a)), "GMBuildCraftArrowDir");
			vp.RotateReset();

			vp.Rotate(_rotateResource);
			for (int i = 0; i < 40; i++){
				vp.Rotate(5);
				vp.DrawTexture(x, y, "GMBuildCraftArrowLineRound", scaleU/2f);
			}
			vp.RotateReset();

		}

		private void DrawDirection(VisualizationProvider vp, System.Drawing.Point cursor, StarSystem starSystem, StarPoint starPoint, int x, int y, int vsx, int vsy, int scaleS)
		{
			var p1 = new Point((x - cursor.X) / 7, (y - cursor.Y) / 7);// "единичный" вектор, для отступа средних точек
			var pa1 = new Point(cursor.X + p1.Y, cursor.Y - p1.X);
			var pa2 = new Point(cursor.X - p1.Y, cursor.Y + p1.X);
			var pb1 = new Point(x + p1.Y, y - p1.X);
			var pb2 = new Point(x - p1.Y, y + p1.X);
			vp.SetColor(Color.White);
			vp.Line(pa1.X, pa1.Y, pb1.X, pb1.Y);
			vp.Line(pa2.X, pa2.Y, pb2.X, pb2.Y);
			// теперь рисуем возможные направления
			foreach (var road in starPoint.roads){//starSystem.Roads){// starPoint.Parent.Roads){
				var d = road.GetDirection(starPoint);
				if (d == 0) continue;// дорога не содержит направления - выходим
				
				var p1a = (road.NodePoint1 as StarPoint).Point;
				//var p2a = (road.NodePoint2 as StarPoint).Point;
				var reverse = 1;
				if (road.NodePoint2 == starPoint){
					p1a = (road.NodePoint2 as StarPoint).Point;
					//p2a = (road.NodePoint1 as StarPoint).Point;
					reverse = -1;
				}
				var x1 = p1a.X * scaleS + vsx;
				var y1 = p1a.Y * scaleS + vsy;
				//var x2 = p2a.X * scaleS + vsx;var y2 = p2a.Y * scaleS + vsy;var dx = (x2 - x1) / 6;var dy = (y2 - y1) / 6;
				//vp.SetColor(Color.Gainsboro);vp.Line(x1 + dx, y1 + dy, x2 - dx, y2 - dy);// для рисования линии между точками (полная линия)

				var x2 = (int)(x1 - 140 * Math.Cos(road.Angle)*reverse);
				var y2 = (int)(y1 - 140 * Math.Sin(road.Angle)*reverse);
				vp.SetColor(Color.Gold);
				vp.Line(x1, y1, x2, y2);

			}
		}

		/// <summary>
		/// Вывести 
		/// </summary>
		/// <param name="vp"></param>
		/// <param name="starSystem"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private void DrawAvailableResourcesStarSystem(VisualizationProvider vp, StarSystem starSystem, int x, int y)
		{
			DrawAvailableResourcesStarSystem(vp, _rd.GetInfo(ResourceEnum.Sepulki), starSystem, x, y, 1);
			DrawAvailableResourcesStarSystem(vp, _rd.GetInfo(ResourceEnum.Markwi), starSystem, x, y, 2);
			DrawAvailableResourcesStarSystem(vp, _rd.GetInfo(ResourceEnum.Pchmy), starSystem, x, y, 3);
			DrawAvailableResourcesStarSystem(vp, _rd.GetInfo(ResourceEnum.Iiont), starSystem, x, y, 4);
			DrawAvailableResourcesStarSystem(vp, _rd.GetInfo(ResourceEnum.Technologies), starSystem, x, y, 5);
			DrawAvailableResourcesStarSystem(vp, _rd.GetInfo(ResourceEnum.Artefacts), starSystem, x, y, 6);
		}

		/// <summary>
		/// Вывести 
		/// </summary>
		/// <param name="vp"></param>
		/// <param name="rd1"></param>
		/// <param name="starSystem"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="num"></param>
		private void DrawAvailableResourcesStarSystem(VisualizationProvider vp, ResourcesDescription rd1, StarSystem starSystem, int x, int y, int num)
		{
			vp.SetColor(Color.White);
			decimal resourceA = 0;
			foreach (StarPoint point in starSystem.Points){
				resourceA += point.AvailableResources.GetValue(rd1.ResourceEnum).Count;
			}
			decimal resourceW = 0;
			foreach (StarPoint point in starSystem.Points){
				if (point.Building==null)continue;
				resourceA += point.Building.Stored.GetValue(rd1.ResourceEnum).Count;
			}
			var x1 = x;
			var y1 = y + num * 32;
			vp.DrawTexture(x1, y1 + 2, rd1.TexName);
			vp.SetColor(Color.White, 50);
			vp.DrawTexture(x1, y1, "GMBuildCraftResBgr");
			if (resourceA < 1){
				vp.SetColor(Color.White);
				vp.DrawTexture(x1, y1, "GMBuildCraftResEmpty");
			}
			vp.Print(x1 + 30, y1 - 8, "A:" + resourceA.ToString().PadRight(8));
			if (resourceW != 0)
				vp.Print(x1 + 30, y1 + 8, "W:" + resourceW.ToString().PadRight(8));
		}

		/// <summary>
		/// Вывести 
		/// </summary>
		/// <param name="vp"></param>
		/// <param name="starPoint"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private void DrawAvailableResources(VisualizationProvider vp, StarPoint starPoint, int x, int y)
		{
			DrawAvailableResources(vp, _rd.GetInfo(ResourceEnum.Sepulki), starPoint, x, y, 1);
			DrawAvailableResources(vp, _rd.GetInfo(ResourceEnum.Markwi), starPoint, x, y, 2);
			DrawAvailableResources(vp, _rd.GetInfo(ResourceEnum.Pchmy), starPoint, x, y, 3);
			DrawAvailableResources(vp, _rd.GetInfo(ResourceEnum.Iiont), starPoint, x, y, 4);
			DrawAvailableResources(vp, _rd.GetInfo(ResourceEnum.Technologies), starPoint, x, y, 5);
			DrawAvailableResources(vp, _rd.GetInfo(ResourceEnum.Artefacts), starPoint, x, y, 6);
		}

		/// <summary>
		/// Вывести 
		/// </summary>
		/// <param name="vp"></param>
		/// <param name="rd1"></param>
		/// <param name="starPoint"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="num"></param>
		private void DrawAvailableResources(VisualizationProvider vp, ResourcesDescription rd1, StarPoint starPoint, int x, int y, int num)
		{
			vp.SetColor(Color.White);
			var resourceA = starPoint.AvailableResources.GetValue(rd1.ResourceEnum);
			ResourcePacket resourceW = null;
			if (starPoint.Building != null)
				resourceW = starPoint.Building.Stored.GetValue(rd1.ResourceEnum);
			var x1 = x;
			var y1 = y + num*32;
			vp.DrawTexture(x1, y1 + 2, rd1.TexName);
			vp.SetColor(Color.White, 50);
			vp.DrawTexture(x1, y1, "GMBuildCraftResBgr");
			if (resourceA.Count < 1){
				vp.SetColor(Color.White);
				vp.DrawTexture(x1, y1, "GMBuildCraftResEmpty");
			}
			vp.Print(x1 + 30, y1 - 8, "A:" + resourceA.Count.ToString().PadRight(8));
			if (resourceW != null)
				vp.Print(x1 + 30, y1 + 8, "W:" + resourceW.Count.ToString().PadRight(8) + " " + starPoint.BuildingType);
			else vp.Print(x1 + 30, y1 + 8, "W:нету здания");
		}



		private void DrawResourcesSigns(VisualizationProvider vp)
		{
			DrawResourcesSign(vp,_rd.GetInfo(ResourceEnum.Sepulki),40);
			DrawResourcesSign(vp,_rd.GetInfo(ResourceEnum.Markwi),80);
			DrawResourcesSign(vp,_rd.GetInfo(ResourceEnum.Pchmy),120);
			DrawResourcesSign(vp,_rd.GetInfo(ResourceEnum.Iiont),160);
			DrawResourcesSign(vp,_rd.GetInfo(ResourceEnum.Technologies),200);
			DrawResourcesSign(vp,_rd.GetInfo(ResourceEnum.Artefacts),240);
		}

		private void DrawResourcesSign(VisualizationProvider vp, ResourcesDescription rd1, int dx)
		{
			vp.SetColor(Color.White);
			vp.DrawTexture(20 + dx, 740 + 2, rd1.TexName);
			vp.SetColor(Color.White, 50);
			vp.DrawTexture(20 + dx, 740, "GMBuildCraftResBgr");
			vp.SetColor(rd1.Color);
			vp.Print(20 + dx-20, 740 + 10, "XXXXX");
		}
		public override void Cursor(object sender, PointEventArgs point)
		{
			base.Cursor(sender, point);
			_cursorUniverse = new Point(
					point.Pt.X / _scaleU - _viewUX / _scaleU,
					point.Pt.Y / _scaleU - _viewUY / _scaleU);

			if (_IsSetDirection){// особый режим, вычисляем направление
				return;
			}

			if (_dragged)return;// если запущено перемещение то новые точки не ищем

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
			//_cursorUniverse = new Point(x / _scaleU - _viewUX / _scaleU,y / _scaleU - _viewUY / _scaleU);
			// проверяем, если расстояние между выявленной точкой и переданной небольшое - то включаем режим указания направления
			if (_viewStarSystem){
				if (_nearestStarPoint > -1){
					var cursorUniverseLocal = new Point(
							x / _scaleS - _viewSX / _scaleS,
							y / _scaleS - _viewSY / _scaleS);
					var starSystem = Universe.Points[_nearestStarSystem] as StarSystem;
					var p = starSystem.Points[_nearestStarPoint];
					var r = MapGenerator.Distance(p.Point, cursorUniverseLocal);
					if (r < 10){
						_IsSetDirection = true;
						return;
					}
				}
			}

			_dragged = true;
		}

		protected override void DragEnd(int relX, int relY)
		{
			if (_IsSetDirection){
				_IsSetDirection = false;
				return;
			}
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
			var dx = (x2 - x1) / 60;
			var dy = (y2 - y1) / 60;
			vp.SetColor(Color.Turquoise);
			vp.Line(x1 + dx, y1 + dy, x2 - dx, y2 - dy);

			x2 = (int) (x1 - 140*Math.Cos(way.Angle));
			y2 = (int) (y1 - 140*Math.Sin(way.Angle));
			vp.SetColor(Color.Gold,50);
			vp.Line(x1, y1, x2, y2);

			if (Math.Abs(way.Angle )< 0.0000001){
				// вычисляем ещё раз угол, уже тут

				double x = p1.X - p2.X;
				double y = p1.Y - p2.Y;
				var a1 = Math.Atan2(y, x);
				F = a1;

				vp.Box(x1 - 5, y1 - 5, 10, 10);
			}

			vp.SetColor(Color.Pink);
			var p = way.Path;
			for (int i = 1; i < p.Count; i++){
				//vp.Line((int) (p[i - 1].X*scale + vx), (int) (p[i - 1].Y*scale + vy), (int) (p[i].X*scale + vx), (int) (p[i].Y*scale + vy));
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
			vp.SetColor(Color.Gainsboro,30);
			vp.Line(x1 + dx, y1 + dy, x2 - dx, y2 - dy);

			x2 = (int)(x1 - 140 * Math.Cos(road.Angle));
			y2 = (int)(y1 - 140 * Math.Sin(road.Angle));
			vp.SetColor(Color.Tomato, 40);
			vp.Line(x1, y1, x2, y2);

			vp.SetColor(Color.Pink,20);
			var p = road.Path;
			for (int i = 1; i < p.Count; i++)
			{
				////vp.Line((int) (p[i - 1].X * scale + vsx), (int) (p[i - 1].Y * scale + vsy), (int) (p[i].X * scale + vsx), (int) (p[i].Y * scale + vsy));
			}
		}

		/// <summary>
		/// локальная дорога между звёздными системами
		/// </summary>
		/// <param name="road"></param>
		/// <param name="vp"></param>
		/// <param name="vsx"></param>
		/// <param name="vsy"></param>
		/// <param name="scale"></param>
		/// <param name="starSystem"></param>
		private void DrawRoadU(Road road, VisualizationProvider vp, int vsx, int vsy, int scale, StarSystem starSystem)
		{
			int d = 1;
			if ((road.NodePoint2 as StarPoint).Parent == starSystem) d = -1;
			if ((d!=-1)&&(road.NodePoint1 as StarPoint).Parent!=starSystem)return;// если вторая точка не подходит и первая не подходит - выходим

			var p1a = (road.NodePoint1 as StarPoint).Point;
			if (d==-1){
				p1a = (road.NodePoint2 as StarPoint).Point;
			}
			var x1 = p1a.X * scale + vsx;
			var y1 = p1a.Y * scale + vsy;
			var x2 = (int)(x1 - 140 * Math.Cos(road.Angle) * d);
			var y2 = (int)(y1 - 140 * Math.Sin(road.Angle) * d);
			vp.SetColor(Color.PaleGreen,30);
			vp.Line(x1, y1, x2, y2);

			//var p1 = (road.NodePoint1 as StarPoint).Point;
			//var p2 = (road.NodePoint2 as StarPoint).Point;
			//var x1 = p1.X * scale + vsx;
			//var y1 = p1.Y * scale + vsy;
			//var x2 = p2.X * scale + vsx;
			//var y2 = p2.Y * scale + vsy;
			//var dx = (x2 - x1) / 60;
			//var dy = (y2 - y1) / 60;
			//vp.SetColor(Color.White);
			//vp.Line(x1 + dx, y1 + dy, x2 - dx, y2 - dy);

			//x2 = (int)(x1 - 140 * Math.Cos(road.Angle));
			//y2 = (int)(y1 - 140 * Math.Sin(road.Angle));
			//vp.SetColor(Color.BurlyWood);
			//vp.Line(x1, y1, x2, y2);

		}

	}
}
