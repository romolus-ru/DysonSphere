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
			AddButton(310, 250, 200, 40, "GMTubesContinue", "Продолжить", "Продолжить", Keys.Escape);
			AddButton(310, 300, 200, 40, "GMTubesStart", "Старт", "Старт", Keys.N);
			AddButton(310, 350, 200, 40, "GMTubesNewPlayer", "Сбросить успехи", "Сбросить успехи", Keys.U);
			AddButton(310, 400, 200, 40, "GMTubesExit", "Выход", "Выход", Keys.Space);
			AddButton(310, 450, 200, 40, "GMTubesSet1", "Сложность Режим 1", "Выставить сложность режим 1", Keys.D1);
			AddButton(310, 500, 200, 40, "GMTubesSet2", "Сложность Режим 2", "Выставить сложность режим 2", Keys.D2);
			btnContinue.Hide();
		}

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			SetCoordinates(0,0,0);
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		public Button AddButton(int x, int y, int width, int height, string eventName,
			string caption, string hint, Keys key)
		{
			var btn = Button.CreateButton(Controller, x, y, width, height, eventName, caption, hint, key, caption);
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
