using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils
{
	/// <summary>
	/// Состояния состояния
	/// </summary>
	public enum StatesEnum
	{
		/// <summary>
		/// Состояние не изменилось
		/// </summary>
		Neutral,
		/// <summary>
		/// Состояние "Нажато"
		/// </summary>
		On,
		/// <summary>
		/// Состояние "Ненажато"
		/// </summary>
		Off
	}
}
