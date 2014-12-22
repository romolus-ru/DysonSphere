using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Controllers;
using Engine.Views;

namespace PathTester
{
	class ViewWindowPath:ViewWindow
	{
		public ViewWindowPath(Controller controller) : base(controller, null)
		{}

		protected override void DrawComponentBackground(Engine.VisualizationProvider visualizationProvider)
		{
			if (CursorOver) visualizationProvider.SetColor(Color.SlateGray, 100);
			else visualizationProvider.SetColor(Color.Goldenrod, 30);
			visualizationProvider.Box(X, Y, Width, Height);
		}


		protected override void DrawObject(VisualizationProvider vp)
		{
			DrawComponentBackground(vp);
			DrawComponents(vp); vp.SetColor(Color.FloralWhite, 100);
			var x = X;
			var y = Y;
			vp.SetColor(Color.LightCyan, 70);
			vp.Box(x, y, Width, HeaderHeight);

			if (CursorOver)
			{
				vp.SetColor(Color.FloralWhite);
				vp.Rectangle(x + 1, y + 1, Width - 2, Height - 2);
			}
		}
	}
}
