using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;
using Engine.Utils.Settings;

namespace GMTubes.Model
{
	public class UserInfo
	{
		public String UserName = "";
		public int Exp = 0;
		public int ExpNext = 100;
		public int CurrentLevel = 1;

		public UserInfo() { InitUserInfo();}

		public void AddExp(int exp)
		{
			if (CurrentLevel == 10)
			{
				Exp = ExpNext; return;
			}
			Exp += exp;
			AddLevel1();
			SaveUserInfo();
		}

		private void AddLevel1()
		{
			if (ExpNext < Exp)
			{
				Exp -= ExpNext;
				CurrentLevel++;
				ExpNext = (int)(CurrentLevel * 100);
				AddLevel1();
			}
		}

		private void InitUserInfo()
		{
			UserName = "none";
			CurrentLevel = 1;
			Exp = 0;
			ExpNext = 100;
			UserName = Settings.EngineSettings.GetValue("GMTubes", "UserName");
			String s;
			s = Settings.EngineSettings.GetValue("GMTubes", "CurrentLevel");
			if (s != "") { int.TryParse(s, out CurrentLevel); }
			s = Settings.EngineSettings.GetValue("GMTubes", "Exp");
			if (s != "") { int.TryParse(s, out Exp); }
			s = Settings.EngineSettings.GetValue("GMTubes", "ExpNext");
			if (s != "") { int.TryParse(s, out ExpNext); }
			//if (UserName==""){Controller.AddToOperativeStore(this, StoredEventEventArgs.Stored("GMTubesNewPlayer", this, EventArgs.Empty));}
		}

		public void SaveUserInfo()
		{
			Settings.EngineSettings.AddValue("GMTubes", "UserName", UserName, "");
			Settings.EngineSettings.AddValue("GMTubes", "CurrentLevel", CurrentLevel.ToString(), "");
			Settings.EngineSettings.AddValue("GMTubes", "Exp", Exp.ToString(), "");
			Settings.EngineSettings.AddValue("GMTubes", "ExpNext", ExpNext.ToString(), "");
		}

	}
}
