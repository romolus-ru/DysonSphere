using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Engine;
using Engine.Controllers;
using Engine.Models;
using Engine.Views;
using Engine.Views.Templates;
using Button = Engine.Views.Templates.Button;

namespace PathTester
{
	class PathTesterModule : Module
	{
		private PathView _pv1;

		protected override void SetUpView(Engine.Views.View view, Controller controller)
		{

			var w1 = new ViewWindowPath(controller);
			w1.SetCoordinates(100, 100, 0);
			w1.SetSize(100, 100);
			view.AddObject(w1);

			var w2 = new ViewWindowPath(controller);
			w2.SetCoordinates(300, 100, 0);
			w2.SetSize(100, 100);
			view.AddObject(w2);

			var b = Button.CreateButton(controller, 950, 0, 74, 20, "systemExit", "Выход", "Esc", Keys.Escape, "btn1");
			view.AddObject(b);
			b = Button.CreateButton(controller, 750, 100, 74, 20, "", "TEST", "", Keys.HangulMode, "btn2");
			view.AddObject(b);


			// создаём объект визуализации
			_pv1 = new PathView(controller, null);
			_pv1.SetSize(200, 200);
			_pv1.Show();
			view.AddObject(_pv1);

			var background = new Background(controller);
			view.AddObject(background);

		}

	}
}