using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMBuildCraft.Limiters
{
	/// <summary>
	/// Список ограничителей, для централизованного управления ими
	/// </summary>
	class LimitersList
	{
		private List<Limiter> _limiters = new List<Limiter>();

		///// <summary>
		///// Название списка ограничителей, например строительство стадиона и т.п.
		///// </summary>
		//public String Name;

		public void Add(Limiter limiter)
		{
			_limiters.Add(limiter);
		}

		public Boolean Blocked()
		{
			var ret = false;
			foreach (var limiter in _limiters){
				// если хоть один ограничитель закрыт, то переменная получит значение закрыт
				if (limiter.Verify()) ret = true;
			}
			return ret;
		}

	}
}
