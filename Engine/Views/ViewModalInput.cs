using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Views
{
	public class ViewModalInput:ViewModal
	{
		private String _text;
		private String _value;

		public ViewModalInput(Controller controller, string outEvent, string value) : base(controller, outEvent)
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
