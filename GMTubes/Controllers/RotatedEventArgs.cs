using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;

namespace GMTubes.Controllers
{
	/// <summary>
	/// Событие от модели виду о том, какой объект какое теперь занимает положение
	/// </summary>
	public class RotatedEventArgs:EngineEventArgs
	{
		public int I;
		public int J;
		public int CurrentAngle;

		public static RotatedEventArgs Create(int i, int j,int currAngle)
		{
			var a = new RotatedEventArgs();
			a.I = i;
			a.J = j;
			a.CurrentAngle = currAngle;
			return a;
		}

	}
}
