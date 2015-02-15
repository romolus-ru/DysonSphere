using System;
using System.Windows.Forms;

namespace GMTubes
{
	/// <summary>
	/// Информация о пользователе
	/// </summary>
	public class UserInfo
	{
		public String UserName = "";
		public int Exp = 0;
		public int ExpNext = 100;
		public int CurrentLevel = 1;

		public void AddExp(int exp)
		{
			if (CurrentLevel == 10){
				Exp = ExpNext;return;
			}
			Exp += exp;
			AddLevel1();
		}

		private void AddLevel1()
		{
			if (ExpNext < Exp){
				Exp -= ExpNext;
				CurrentLevel++;
				AddLevel1();
				ExpNext = (int) (CurrentLevel*100);
			}
		}
	}
}
