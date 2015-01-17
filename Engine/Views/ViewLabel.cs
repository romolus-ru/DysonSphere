using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;

namespace Engine.Views
{
	/// <summary>
	/// Текст на экране, без украшательств
	/// </summary>
	public class ViewLabel:ViewComponent
	{
		protected String txt;

		public ViewLabel(Controller controller,String text) : base(controller)
		{txt = text;}

		protected override void DrawObject(VisualizationProvider vp)
		{
			vp.SetColor(Color.AntiqueWhite, 50);
			vp.Print(X, Y, txt);
		}

		public override bool InRange(int x, int y)
		{
			return false;
		}
	}
}
