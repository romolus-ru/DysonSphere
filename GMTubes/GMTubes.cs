using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;
using Engine.Utils.Editor;
using Engine.Views.Templates;
using Button = Engine.Views.Templates.Button;

namespace GMTubes
{
	/// <summary>
	/// GameModule Tubes / Игровой модуль Трубы
	/// </summary>
	/// <remarks>
	/// На поле расположены трубы. надо их повернуть так, что бы образовался замкнутый контур
	/// </remarks>
	public class GMTubes:Module
	{
		private ViewControlSystem _sys;
		private ViewField _vf;
		private Field _f;
		private ViewMenu _menu;

		protected override void SetUpView(Engine.Views.View view, Controller controller)
		{
			_sys = new ViewControlSystem(Controller);
			_sys.Show();
			view.AddObject(_sys);

			_menu = new ViewMenu(controller, _sys);

			//var b1 = Button.CreateButton(controller, 95, 0, 74, 20, "systemExit", "Выход", "Esc", Keys.Escape, "");
			//_sys.AddComponent(b1);

			_f = new Field();
			_vf = new ViewField(controller, _sys);
			_vf.Hide();
			_vf.SetF(_f);

			// сначала создаётся и запускается меню. из него выбираются параметры запуска
			// при нажатии "старт" в модуле прячется меню и запускается игра
			// если нажать esc то откроется меню - там можно будет выбрать прохолжить, начать снова, параметры или выход

			Controller.AddEventHandler("GMTubesPause", GMTubesPause);
			Controller.AddEventHandler("GMTubesContinue", GMTubesContinue);
			Controller.AddEventHandler("GMTubesStart", GMTubesStart);
			Controller.AddEventHandler("GMTubesSet1", GMTubesSet1);
			Controller.AddEventHandler("GMTubesSet2", GMTubesSet2);

		}

		private void GMTubesPause(object sender, EventArgs e)
		{
			_menu.btnContinue.Show();
			_menu.Show();
			_vf.Hide();
		}

		private void GMTubesSet1(object sender, EventArgs e)
		{
			
		}

		private void GMTubesSet2(object sender, EventArgs e)
		{
			
		}


		private void GMTubesStart(object sender, EventArgs e)
		{
			_menu.Hide();
			_f = new Field();
			_vf.SetF(_f);
			_vf.Show();
		}

		private void GMTubesContinue(object sender, EventArgs e)
		{
			_menu.Hide();
			_vf.Show();	
		}
	}
}
