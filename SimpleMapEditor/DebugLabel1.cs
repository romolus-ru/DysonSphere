using System;
using System.Drawing;
using System.Text;
using Engine;
using Engine.Controllers;
using Engine.Views;

namespace SimpleMapEditor
{
	class DebugLabel1:ViewLabel
	{
		protected String txtCount;

		public DebugLabel1(Controller controller, string text) : base(controller, "")
		{
			var p = text.LastIndexOf(' ');
			txt = text.Substring(0, p);
			txtCount = text.Substring(p + 1);
		}

		protected override void DrawObject(VisualizationProvider vp)
		{
			vp.SetColor(Color.AntiqueWhite, 50);
			vp.Print(X, Y, txtCount);
			vp.SetColor(Color.AntiqueWhite, 50);
			vp.Print(X + 25, Y, txt);
		}
	}
}
