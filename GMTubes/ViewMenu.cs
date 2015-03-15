using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;
using Engine.Views.Templates;
using Button = Engine.Views.Templates.Button;

namespace GMTubes
{
	/// <summary>
	/// Основное меню
	/// </summary>
	class ViewMenu:ViewControlDraggable
	{
		/// <summary>
		/// Сложность
		/// </summary>
		public int Complexity=0;

		public Button btnContinue;

		public ViewMenu(Controller controller, ViewComponent parent): base(controller, parent)
		{
			// добавляем кнопки
			btnContinue =
			AddButton2A(310, 250, 200, 40, "GMTubesContinue", "Продолжить", "Продолжить", Keys.Escape, new ButtonImage(Controller, null, "GMTubesBtn01", "GMTubesBtn01Over"));
			AddButton2A(310, 300, 200, 40, "GMTubesStart", "Старт", "Старт", Keys.N, new ButtonImage(Controller, null, "GMTubesBtn01", "GMTubesBtn01Over"));
			AddButton2A(310, 350, 200, 40, "GMTubesNewPlayer", "Сбросить успехи", "Сбросить успехи", Keys.U, new ButtonImage(Controller, null, "GMTubesBtn01", "GMTubesBtn01Over"));
			AddButton2A(310, 400, 200, 40, "GMTubesExit", "Выход", "Выход", Keys.Space, new ButtonImage(Controller, null, "GMTubesBtn01", "GMTubesBtn01Over"));
			AddButton2A(100, 010, 200, 40, "GMTubesGraph", "График", "График", Keys.G, new ButtonImage(Controller, null, "GMTubesBtn01", "GMTubesBtn01Over"));
			var b1 = AddButton2(310, 450, 200, 40, "GMTubesSet1", "Выберите сложность", "Выберите сложность запускаемого уровня", Keys.None);
			b1.b1 = 1;
			b1.b2 = 10;
			btnContinue.Hide();
		}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			SetCoordinates(0,0,0);
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
			visualizationProvider.LoadTexture("GMTubesBtn01", @"..\Resources\gmTubesBtn01.png");
			visualizationProvider.LoadTexture("GMTubesBtn01Over", @"..\Resources\gmTubesBtn01over.png");
		}

		public Button AddButton(int x, int y, int width, int height, string eventName,
			string caption, string hint, Keys key)
		{
			var btn = Button.CreateButton(Controller, x, y, width, height, eventName, caption, hint, key, caption);
			AddComponent(btn);
			return btn;
		}

		public ViewButtonCoords AddButton2(int x, int y, int width, int height, string eventName,
			string caption, string hint, Keys key)
		{
			var btn = new ViewButtonCoords(Controller, null, "GMTubesBtn01", "GMTubesBtn01Over");
			Button.InitButton(btn, Controller, x, y, width, height, eventName, caption, hint, key, caption);
			AddComponent(btn);
			return btn;
		}

		public Button AddButton2A(int x, int y, int width, int height, string eventName,
	string caption, string hint, Keys key, Button btn)
		{
			Button.InitButton(btn, Controller, x, y, width, height, eventName, caption, hint, key, caption);
			AddComponent(btn);
			return btn;
		}

		protected override void DrawObject(VisualizationProvider vp)
		{
			base.DrawObject(vp);
			vp.SetColor(Color.White);
			vp.Print(10,10,"Хело! хело!");
			vp.Print(10, 30, "Игрок   " + GMTubes.UserInfo.UserName);
			vp.Print(10, 50, "Уровень " + GMTubes.UserInfo.CurrentLevel);
			vp.Print(10, 70, "Экспы   " + GMTubes.UserInfo.Exp +" из "+ GMTubes.UserInfo.ExpNext);
		}

	}
}
