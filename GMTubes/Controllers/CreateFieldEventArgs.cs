using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;

namespace GMTubes.Controllers
{
	public class CreateFieldEventArgs:EngineEventArgs
	{
		public int UserLevel;

		public static CreateFieldEventArgs Create(int userLevel)
		{
			var a = new CreateFieldEventArgs();
			a.UserLevel = userLevel;
			return a;
		}
	}
}
