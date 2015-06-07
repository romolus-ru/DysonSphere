using System;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Views
{
	public class ViewModalInput:ViewModal
	{
		protected String _text;
		protected String _value;

		public ViewModalInput(Controller controller, ViewComponent parent, string outEvent, string destroyEvent, string value)
			: base(controller, parent, outEvent,destroyEvent)
		{
			_text = value;
			_value = value;
		}

		public String GetResult(){return _text;}

		public override void Keyboard(object o, InputEventArgs inputEventArgs)
		{
			base.Keyboard(o, inputEventArgs);
		}
	}
}
