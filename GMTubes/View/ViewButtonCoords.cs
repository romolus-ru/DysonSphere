using Controller = Engine.Controllers.Controller;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;
using Engine.Views.Templates;

namespace GMTubes.View
{
	class ViewButtonCoords:ButtonImage
	{
		public int Selected = -1;

		/// <summary>
		/// Минимальное значение координаты
		/// </summary>
		public int b1 = 5;

		/// <summary>
		/// Максимальное значение координаты
		/// </summary>
		public int b2 = 10;

		public ViewButtonCoords(Controller controller, ViewComponent parent, string btnTexture, string btnTextureOver)
			: base(controller, parent, btnTexture, btnTextureOver)
		{}

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
