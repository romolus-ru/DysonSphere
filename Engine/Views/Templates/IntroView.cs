using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Controllers;

namespace Engine.Views.Templates
{
	// TODO может быть нужно удалить класс. пока не нужен
	/// <summary>
	/// Вступительное действие
	/// </summary>
	class IntroView:ViewObject
	{
		public IntroView(Controller controller) : base(controller) { }

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
		}
	}
}
