using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;

namespace GMTubes.Controllers
{
	public class ElapsedTimeEventArgs:EngineEventArgs
	{
		public int Elapsed;

		public static ElapsedTimeEventArgs Send(int value)
		{
			var a = new ElapsedTimeEventArgs();
			a.Elapsed = value;
			return a;
		}
		public static ElapsedTimeEventArgs Send(double value)
		{
			return Send((int) value);
		}
	}
}
