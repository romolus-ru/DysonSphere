using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;
using Engine.Views;

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
			SetCoordinates(0, 0, 0);
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			Alpha++;
			if (Alpha > 100) Alpha = 100;
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
			var dt = F.Time.Elapsed;
			var s = dt.Minutes.ToString().PadLeft(2, '0') + ":" + dt.Seconds.ToString().PadLeft(2, '0');
			visualizationProvider.SetColor(Color.Tomato);
			visualizationProvider.Print(100, 750, s);
			s = "GMTubesVictoryBronze";
			var t = F.LevelTimeInterval();
			if (t==1) s = "GMTubesVictorySilver";
			if (t==2) s = "GMTubesVictoryGold";
			visualizationProvider.DrawTexture(70, 730, s);
			base.DrawObject(visualizationProvider);
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
			_target=FindNearest(args.Pt.X, args.Pt.Y);
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
