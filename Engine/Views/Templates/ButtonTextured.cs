using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;

namespace Engine.Views.Templates
{
	class ButtonTextured:Button
	{
		private string _textureName;
		/// <summary>
		/// Кнопка с текстурой (загружать текстуру надо отдельно)
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="parent"></param>
		public ButtonTextured(Controller controller, ViewComponent parent, string textureName) : base(controller, parent)
		{
			_textureName = textureName;
		}


	}
}
