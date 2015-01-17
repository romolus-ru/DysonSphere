using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine.Controllers;
using Engine.Views;
using Button = Engine.Views.Templates.Button;

namespace SimpleMapEditor
{
	/// <summary>
	/// Тестовое окно с кнопкой
	/// </summary>
	class SimpleWindow:ViewWindow
	{
		public SimpleWindow(Controller controller, ViewComponent parentSystem = null) : base(controller, parentSystem)
		{
			 //TODO --->>>> ControlDraggable получает курсор, но передаёт его только viewDraggable. нужно это как то исправить
			AddComponent(Button.CreateButton(controller, 10, 10, 20, 20, "UU", "Кнопка", "S", Keys.U, "UU"));
			this.Name = "SW";
		}


	}
}
