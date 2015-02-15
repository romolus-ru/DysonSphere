using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;
using Engine.Views.Templates;

namespace GMTubes
{
	class ViewButtonCoords:Button
	{
		public ViewButtonCoords(Controller controller, ViewComponent parent) : base(controller, parent)
		{}

		public int Selected = -1;

		/// <summary>
		/// Минимальное значение координаты
		/// </summary>
		public int b1 = 5;

		/// <summary>
		/// Максимальное значение координаты
		/// </summary>
		public int b2 = 10;

		public override void Cursor(object o, PointEventArgs args)
		{
			base.Cursor(o, args);
			int x = b1 + (args.Pt.X - X)*(b2 - b1+1)/Width;
			Selected = x;
			//int y = args.Pt.Y - this.Y;
			this.Caption = "" + x;
			this.Hint = "Запустить "+x+"й уровень сложности";
		}
	}
}
