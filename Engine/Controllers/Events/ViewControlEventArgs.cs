using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Views2;

namespace Engine.Controllers.Events
{
	/// <summary>
	/// Для передачи объектов для View
	/// </summary>
	class ViewControlEventArgs:EngineEventArgs
	{
		public ViewControl ViewControl;

		public static ViewControlEventArgs Send(ViewControl viewControl)
		{
			var a = new ViewControlEventArgs();
			a.ViewControl = viewControl;
			return a;
		}
	}
}
