using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Models;
using Engine.Views;
using Button = Engine.Views.Templates.Button;

namespace PathTester
{
	class PathTesterModule : Module
	{
		private PathView _pv1;

		protected override void SetUpView(Engine.Views.View view, Controller controller)
		{
			var b = Button.CreateButton(controller, 950, 0, 74, 20, "systemExit", "Выход", "Esc", Keys.Escape, "btn1");
			view.AddObject(b);
			b = Button.CreateButton(controller, 750, 100, 74, 20, "", "TEST", "", Keys.HangulMode, "btn2");
			view.AddObject(b);

			// создаём объект визуализации
			_pv1 = new PathView(controller, null);
			_pv1.Show();
			view.AddObject(_pv1);
		}

	}
}