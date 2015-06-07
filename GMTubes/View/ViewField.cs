using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;
using Engine.Views;
using Engine.Utils.ExtensionMethods;
using GMTubes.Controllers;
using GMTubes.Model;

namespace GMTubes.View
{
	/// <summary>
	/// Просмотр поля
	/// </summary>
	class ViewField:ViewControl
	{
		// показатели получаются из поля
		private List<ViewFieldPoint> _points = new List<ViewFieldPoint>();
		private int FieldWidth;
		private int FieldHeight;
		private int FieldTimeGold;
		private int FieldTimeSilver;
		
		private int offsetX;
		private int offsetY;
		private int cX;
		private int cY;

		private int ScreenW=320;// размеры экрана
		private int ScreenH=240;
		
		private ViewFieldPoint _target = null;

		private StateOne _keyCkick1 = StateOne.Init();

		public byte Alpha=0;

		public TimeSpan TimeElapsed;
		public Stopwatch Time1=new Stopwatch();// для вида, настоящий таймер в модели

		public ViewField(int fieldLevel, Controller controller, ViewComponent parent = null) : base(controller, parent)
		{
			Controller.AddEventHandler("GMTubesFieldInfo", GMTubesFieldInfoEH);
			Controller.AddEventHandler("GMTubesRotated", GMTubesRotatedEH);
			Controller.AddEventHandler("GMTubesTimeElapsed", GMTubesTimeElapsedEH);

			Controller.SendToModelCommand("GMTubesCreateField", CreateFieldEventArgs.Create(fieldLevel));
		}

		private void GMTubesTimeElapsedEH(object sender, EventArgs e)
		{
			var ec = e as MessageEventArgs;
			GMTubesTimeElapsed(sender, ec.Deserialize<ElapsedTimeEventArgs>(e));
		}
		private void GMTubesTimeElapsed(object sender, ElapsedTimeEventArgs e)
		{
			TimeElapsed = TimeSpan.FromSeconds(e.Elapsed);// получаем прошедшее время из модели и перезапускаем видовский таймер
			Time1.Restart();
		}

		private void GMTubesRotatedEH(object sender, EventArgs e)
		{
			var ec = e as MessageEventArgs;
			GMTubesRotated(sender, ec.Deserialize<RotatedEventArgs>(e));
		}

		private void GMTubesRotated(object sender, RotatedEventArgs e)
		{
			foreach (var point in _points){// ищем нужную точку и поворачиваем её
				if (point.X != e.I) continue;
				if (point.Y != e.J) continue;
				point.CurrentAngle = e.CurrentAngle;
				point.Rotate();
				break;
			}
		}

		private void GMTubesFieldInfoEH(object sender, EventArgs e)
		{
			GMTubesFieldInfo(sender, ((EngineEventArgs) e).Deserialize<FieldInfoEventArgs>(e));
		}

		private void GMTubesFieldInfo(object sender, FieldInfoEventArgs e)
		{
			FieldWidth = e.FieldWidth;
			FieldHeight = e.FieldHeight;
			FieldTimeGold = e.FieldTimeGold;
			FieldTimeSilver = e.FieldTimeSilver;
			foreach (var point in e.Points){
				var p = new ViewFieldPoint(point.TextureNum, point.X, point.Y, point.IsVisible, point.CurrentAngle);
				_points.Add(p);
			}
			offsetX = (ScreenW - FieldWidth * 32) / 2;
			offsetY = (ScreenH - FieldHeight * 32) / 2;
		}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			ScreenH = visualizationProvider.CanvasHeight;
			ScreenW = visualizationProvider.CanvasWidth;
			visualizationProvider.LoadTexture("GMTubes", @"..\Resources\gmTubes1a.jpg");// грузим основную текстуру
			visualizationProvider.LoadTexture("GMTubesVictoryGold", @"..\Resources\gmTubesVictoryGold.png");
			visualizationProvider.LoadTexture("GMTubesVictorySilver", @"..\Resources\gmTubesVictorySilver.png");
			visualizationProvider.LoadTexture("GMTubesVictoryBronze", @"..\Resources\gmTubesVictoryBronze.png");
			// отключено. и текстуры с ПНГ не могут быть прозрачными в зависимости от текущего цвета в отличии от JPG
			visualizationProvider.LoadTexture("GMTubesClick", @"..\Resources\gmTubesClick.jpg");
			SetCoordinates(0, 0, 0);
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			Alpha++;
			if (Alpha > 100) Alpha = 100;

			foreach (var point in _points){

				var f1 = point;
				int x1 = offsetX + f1.X*32;
				int y1 = offsetY + f1.Y*32;
				int texnum = f1.TextureNum; // номер текстуры
				int angle = f1.CurrentAngle; // угол поворота
				var angle1 = angle*90 + f1.AddRotate();
				visualizationProvider.Rotate(angle1);
				visualizationProvider.SetColor(Color.White, 80);
				visualizationProvider.DrawTexturePart(x1, y1, "GMTubes", 32, 32, texnum);

				visualizationProvider.SetColor(Color.Yellow, 80);
				visualizationProvider.Circle(x1, y1, 5);

				visualizationProvider.RotateReset();
			}

			visualizationProvider.SetColor(Color.White);
			DrawTimeProgress(visualizationProvider);
			base.DrawObject(visualizationProvider);
		}

		private void DrawTimeProgress(VisualizationProvider visualizationProvider)
		{
			var maxW = 600;

			var dtg=TimeSpan.FromSeconds(FieldTimeGold);
			var sg = dtg.Minutes.ToString().PadLeft(2, '0') + ":" + dtg.Seconds.ToString().PadLeft(2, '0');
			visualizationProvider.SetColor(Color.Tomato);
			visualizationProvider.Print(20, 620, sg);

			var dts = TimeSpan.FromSeconds(FieldTimeSilver);
			var ss = dts.Minutes.ToString().PadLeft(2, '0') + ":" + dts.Seconds.ToString().PadLeft(2, '0');
			visualizationProvider.SetColor(Color.Tomato);
			visualizationProvider.Print(20, 635, ss);

			var dt = TimeElapsed + Time1.Elapsed;
			var s = dt.Minutes.ToString().PadLeft(2, '0') + ":" + dt.Seconds.ToString().PadLeft(2, '0');
			visualizationProvider.SetColor(Color.Tomato);
			visualizationProvider.Print(100, 750, s);
			s = "GMTubesVictoryBronze";
			var color = Color.Thistle;
			var percent = 0;
			var t = LevelTimeInterval(dt);
			if (t == 1){
				s = "GMTubesVictorySilver";
				color = Color.DarkSalmon;
				percent = (int)((FieldTimeSilver - dt.TotalSeconds) / (FieldTimeSilver-FieldTimeGold) * maxW);
			}
			if (t == 2){
				s = "GMTubesVictoryGold";
				color = Color.DarkSeaGreen;
				percent = (int) ((FieldTimeGold - dt.TotalSeconds)/FieldTimeGold*maxW);
			}
			visualizationProvider.DrawTexture(70, 745, s);
			visualizationProvider.SetColor(color);
			visualizationProvider.Box(150, 730, percent, 20);
		}

		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{}

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

		protected ViewFieldPoint FindNearest(int x, int y)
		{
			const int maxdist = 100;// максимальная дистанция
			float dist = maxdist;// устанавливаем сразу "максимальную" дальность
			ViewFieldPoint obj = null;
			foreach (var point in _points){
				int x1 = offsetX + point.X * 32;
				int y1 = offsetY + point.Y * 32;
				var dist1 = Distance(x, y, x1, y1);
				if (dist1 < dist)
				{
					dist = dist1;
					obj = point;
				}
			}
			return obj;
		}

		public override void Cursor(object o, PointEventArgs args)
		{
			base.Cursor(o, args);
			cX = args.Pt.X;
			cY = args.Pt.Y;
			_target = FindNearest(cX, cY);
		}

		public override void Keyboard(object o, InputEventArgs args)
		{
			base.Keyboard(o, args);
			if (args.IsKeyPressed(Keys.Escape)){
				Controller.StartEvent("GMTubesPause", this, EventArgs.Empty);
				return;
			}
			var isKeyPressed = args.IsKeyPressed(Keys.LButton);
			var st = _keyCkick1.Check(isKeyPressed);
			if (st != StatesEnum.On) return;
			if (_target != null)
				Controller.SendToModelCommand("GMTubesRotateClick", RotateClickEventArgs.Create(_target.X,_target.Y));
			//if (_verifyResult == ""){F.Time.Stop();
				//Controller.AddToStore(this, StoredEventEventArgs.StoredSeconds(2, "GMTubesVictory", this, EventArgs.Empty));
			//}
		}

		public void TimeContinue()
		{
			Controller.SendToModelCommand("GMTubesTimeContinue", new EngineEventArgs());
		}

		public void TimePause()
		{
			Time1.Stop();
			Controller.SendToModelCommand("GMTubesTimePause", new EngineEventArgs());
		}

		/// <summary>
		/// 0 бронза, 1 серебро, 2 золото
		/// </summary>
		/// <returns></returns>
		public int LevelTimeInterval(TimeSpan dt)
		{
			var ret = 0;
			if (dt.TotalSeconds < FieldTimeSilver) ret = 1;
			if (dt.TotalSeconds < FieldTimeGold) ret = 2;
			return ret;
		}


	}
}
