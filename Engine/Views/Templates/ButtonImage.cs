using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;

namespace Engine.Views.Templates
{
	public class ButtonImage:Button
	{

		private string _btnTexture = "";
		private string _btnTextureOver = "";

		public ButtonImage(Controller controller, ViewComponent parent, String btnTexture, String btnTextureOver)
			: base(controller, parent)
		{
			_btnTexture = btnTexture;
			_btnTextureOver = btnTextureOver;
		}

		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			var x = X + Width/2;
			var y = Y + Height/2;
			visualizationProvider.SetColor(Color.White);
			if (CursorOver) visualizationProvider.DrawTexture(x,y,_btnTextureOver);
			else visualizationProvider.DrawTexture(x, y, _btnTexture);
		}
	}
}
