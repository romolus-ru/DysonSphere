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
			}
		
		private int offsetX;
		private int offsetY;

		private FieldPoint _target = null;

		private StateOne _keyCkick1 = StateOne.Init();
		
		public ViewField(Controller controller, ViewComponent parent = null) : base(controller, parent)
		{}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			visualizationProvider.LoadTexture("GMTubes", @"..\Resources\gmTubes1a.jpg");// грузим основную текстуру
			SetCoordinates(0, 0, 0);
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			for (int i = 0; i < Field.MaxWidth; i++){
				for (int j = 0; j < Field.MaxHeight; j++){
					var f1 = F.Data[i, j];
					if (f1 == null) continue;
					int texnum = f1.texnum;// номер текстуры
					int angle = f1.currentAngle;// угол поворота
					visualizationProvider.SetColor(Color.White);
					visualizationProvider.Rotate(angle*90);
					int x1 = offsetX + i*32;
					int y1 = offsetY + j*32;
					visualizationProvider.DrawTexturePart(x1, y1, "GMTubes", 32, 32, texnum);
					visualizationProvider.RotateReset();
				}
			}
			//if (_target!=null){int x1 = 25 + _target.i * 32;int y1 = 50 + _target.j * 32;
			//	visualizationProvider.SetColor(Color.SpringGreen,40);visualizationProvider.Circle(x1, y1, 20);}
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
			if (args.IsKeyPressed(Keys.Escape)) {Controller.StartEvent("GMTubesPause", this, EventArgs.Empty);
				return;
			}
			var st = _keyCkick1.Check(args.IsKeyPressed(Keys.LButton));
			if (st != StatesEnum.On) return;
			if (_target != null) 
				_target.Rotate();
		}
	}
}
