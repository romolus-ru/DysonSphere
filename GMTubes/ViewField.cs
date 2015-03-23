using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;
using Engine.Views;
using GMTubes.Model;

namespace GMTubes
{
	/// <summary>
	/// Просмотр поля
	/// </summary>
	class ViewField:ViewControl
	{
		private Field F;
			public void SetF(Field f)
			{
				F = f;
				offsetX = (VisualizationProvider.CanvasWidth - F.MaxW * 32) / 2;
				offsetY = (VisualizationProvider.CanvasHeight - F.MaxH * 32) / 2;
				fieldAssebmled = false;
			}
		
		private int offsetX;
		private int offsetY;
		private int cX;
		private int cY;
		private int cX1;
		private int cY1;
		private float scale = 0;
		/// <summary>
		/// Признак что уровень собран игроком, блокирует дальнейшие изменения
		/// </summary>
		private Boolean fieldAssebmled = false;

		private FieldPoint _target = null;

		private StateOne _keyCkick1 = StateOne.Init();

		public byte Alpha=0;

		private String _verifyResult = "";

		private Boolean showVerify;
		private bool _viewOrig;

		public ViewField(Controller controller, ViewComponent parent = null) : base(controller, parent)
		{}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
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
			if (scale < 2){
				visualizationProvider.SetColor(Color.Red);
				var b = (byte)(100 - (scale - 1) * 100);
				if (scale > 1) visualizationProvider.SetColor(Color.Red, b);
				scale += 0.09f;
				//visualizationProvider.DrawTexture(cX1, cY1, "GMTubesClick", scale);
			}

			var f = F.VerifyCurrentGetField();
			if (!showVerify){
				for (int i = 0; i < Field.MaxWidth; i++){
					for (int j = 0; j < Field.MaxHeight; j++){
						var f1 = F.Data[i, j];
						if (f1 == null) continue;
						int x1 = offsetX + i * 32;
						int y1 = offsetY + j * 32;
						int texnum = f1.texnum; // номер текстуры
						int angle = f1.currentAngle; // угол поворота
						var angle1 = angle * 90+f1.AddRotate();
						visualizationProvider.Rotate(angle1);
						visualizationProvider.SetColor(Color.White, 80);
						visualizationProvider.DrawTexturePart(x1, y1, "GMTubes", 32, 32, texnum);
		
						visualizationProvider.SetColor(Color.Yellow, 80);
						visualizationProvider.Circle(x1, y1, 5);
						if (f1.Broken){
							visualizationProvider.SetColor(Color.Red);
							visualizationProvider.Circle(x1, y1, 15);
						}
						/*visualizationProvider.SetColor(Color.Red);
						if (f1.LinkDown != null) visualizationProvider.Line(x1, y1, x1, y1 + 10);
						if (f1.LinkUp != null) visualizationProvider.Line(x1, y1, x1, y1 - 10);
						if (f1.LinkLeft != null) visualizationProvider.Line(x1, y1, x1 - 10, y1);
						if (f1.LinkRight != null) visualizationProvider.Line(x1, y1, x1 + 10, y1);*/

						visualizationProvider.RotateReset();
					}
				}
			}else{
				
				//for (int i = 0; i < Field.MaxWidth; i++){
				//	for (int j = 0; j < Field.MaxHeight; j++){
				//		var f1 = f[i, j];
				//		if (f1 == null) continue;
				//		int texnum = f1.texnum; // номер текстуры
				//		int angle = f1.angle; // угол поворота
				//		visualizationProvider.SetColor(Color.MistyRose, Alpha);
				//		visualizationProvider.Rotate(angle * 90);
				//		int x1 = offsetX + i * 32;
				//		int y1 = offsetY + j * 32;
				//		visualizationProvider.DrawTexturePart(x1, y1, "GMTubes", 32, 32, texnum);
				//		visualizationProvider.RotateReset();
				//	}
				//}
				for (int i = 0; i < Field.MaxWidth; i++){
					for (int j = 0; j < Field.MaxHeight; j++){
						var f1 = f[i, j];
						if (f1 == null) continue;
						int x1 = offsetX + i * 32;
						int y1 = offsetY + j * 32;
						//if (i == 0 && j == 0) visualizationProvider.Circle(x1, y1, 15);
						visualizationProvider.SetColor(Color.White, 50);
						visualizationProvider.Circle(x1, y1, 5);
						visualizationProvider.SetColor(Color.White);
						if (f1.LinkDown != null) visualizationProvider.Line(x1, y1, x1, y1 + 10);
						if (f1.LinkUp != null) visualizationProvider.Line(x1, y1, x1, y1 - 10);
						if (f1.LinkLeft != null) visualizationProvider.Line(x1, y1, x1 - 10, y1);
						if (f1.LinkRight != null) visualizationProvider.Line(x1, y1, x1 + 10, y1);
					}
				}
			}
			visualizationProvider.SetColor(Color.White);
			//var l = _verifyResult.Length;if (l > 60) l = 60;
			//var s = _verifyResult.Substring(0, l);
			//visualizationProvider.Print(10, 10, s);
			//if (_target!=null){int x1 = 25 + _target.i * 32;int y1 = 50 + _target.j * 32;
			//	visualizationProvider.SetColor(Color.SpringGreen,40);visualizationProvider.Circle(x1, y1, 20);}
			DrawTimeProgress(visualizationProvider);
			base.DrawObject(visualizationProvider);
		}

		private void DrawTimeProgress(VisualizationProvider visualizationProvider)
		{
			var maxW = 600;

			var dtg=TimeSpan.FromSeconds(F.TimeGold);
			var sg = dtg.Minutes.ToString().PadLeft(2, '0') + ":" + dtg.Seconds.ToString().PadLeft(2, '0');
			visualizationProvider.SetColor(Color.Tomato);
			visualizationProvider.Print(20, 620, sg);

			var dts = TimeSpan.FromSeconds(F.TimeSilver);
			var ss = dts.Minutes.ToString().PadLeft(2, '0') + ":" + dts.Seconds.ToString().PadLeft(2, '0');
			visualizationProvider.SetColor(Color.Tomato);
			visualizationProvider.Print(20, 635, ss);

			var dt = F.Time.Elapsed;
			var s = dt.Minutes.ToString().PadLeft(2, '0') + ":" + dt.Seconds.ToString().PadLeft(2, '0');
			visualizationProvider.SetColor(Color.Tomato);
			visualizationProvider.Print(100, 750, s);
			s = "GMTubesVictoryBronze";
			var color = Color.Thistle;
			var percent = 0;
			var t = F.LevelTimeInterval();
			if (t == 1){
				s = "GMTubesVictorySilver";
				color = Color.DarkSalmon;
				percent = (int)((F.TimeSilver - F.Time.Elapsed.TotalSeconds) / (F.TimeSilver-F.TimeGold) * maxW);
			}
			if (t == 2){
				s = "GMTubesVictoryGold";
				color = Color.DarkSeaGreen;
				percent = (int) ((F.TimeGold - F.Time.Elapsed.TotalSeconds)/F.TimeGold*maxW);
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

		protected FieldPoint FindNearest(int x, int y)
		{
			const int maxdist = 100;// максимальная дистанция
			float dist = maxdist;// устанавливаем сразу "максимальную" дальность
			FieldPoint obj = null;
			for (int i = 0; i < Field.MaxWidth; i++){
				for (int j = 0; j < Field.MaxHeight; j++){
					var f1 = F.Data[i, j];
					if (f1 == null) continue;
					int x1 = offsetX + i*32;
					int y1 = offsetY + j*32;
					var dist1 = Distance(x, y, x1, y1);
					if (dist1 < dist){
						dist = dist1;
						obj = f1;
					}
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
			if (fieldAssebmled) return;
			if (args.IsKeyPressed(Keys.Escape))
			{
				Controller.StartEvent("GMTubesPause", this, EventArgs.Empty);
				return;
			}
			if (args.IsKeyPressed(Keys.W)){
				_verifyResult = F.VerifyCurrent();
				return;
			}
			_viewOrig = false;
			if (args.IsKeyPressed(Keys.E)){
				_viewOrig = true;
				return;
			}
			showVerify = false;
			if (args.IsKeyPressed(Keys.Q)){
				showVerify = true;
				return;
			}
			var st = _keyCkick1.Check(args.IsKeyPressed(Keys.LButton));
			if (st != StatesEnum.On) return;
			cX1 = cX;
			cY1 = cY;
			scale = 0;
			if (_target != null) 
				_target.Rotate();
			_verifyResult = F.VerifyCurrent();
			if (_verifyResult == ""){
				F.Time.Stop();
				fieldAssebmled = true;
				Controller.AddToStore(this, StoredEventEventArgs.StoredSeconds(2, "GMTubesVictory", this, EventArgs.Empty));
			}
		}
	}
}
