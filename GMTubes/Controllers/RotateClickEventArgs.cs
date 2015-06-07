using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;

namespace GMTubes.Controllers
{
	/// <summary>
	/// Передаём координаты поля на которые нажал игрок в модель
	/// </summary>
	public class RotateClickEventArgs:EngineEventArgs
	{
		public int I;
		public int J;

		public static RotateClickEventArgs Create(int i, int j)
		{
			var a = new RotateClickEventArgs();
			a.I = i;
			a.J = j;
			return a;
		}
	}
}
