using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;

namespace GMTubes.Controllers
{
	/// <summary>
	/// Передача информации о пользователе
	/// </summary>
	public class UserInfoEventArgs:EngineEventArgs
	{
		public String UserName = "";
		public int Exp = 0;
		public int ExpNext = 100;
		public int CurrentLevel = 1;

		public static UserInfoEventArgs Send(String userName,int exp,int expNext, int currentLevel)
		{
			var a = new UserInfoEventArgs();
			a.UserName = userName;
			a.Exp = exp;
			a.ExpNext = expNext;
			a.CurrentLevel = currentLevel;
			return a;
		}
	}
}
