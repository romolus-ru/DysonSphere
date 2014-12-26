using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Controllers.Events
{
	/// <summary>
	/// Передаёт целое число
	/// </summary>
	public class IntegerEventArgs : EventArgs
	{
		public int I { get; private set; }

		public void Set(int iNew)
		{
			I = iNew;
		}

		public static IntegerEventArgs Send(int it)
		{
			var ret = new IntegerEventArgs {I = it};
			return ret;
		}
	}
}
