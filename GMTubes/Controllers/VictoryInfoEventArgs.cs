using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;

namespace GMTubes.Controllers
{
	/// <summary>
	/// Информация о выйгрыше
	/// </summary>
	public class VictoryInfoEventArgs:EngineEventArgs
	{
		public Boolean IsVictory = false;
		public int MainExp;
		public int extraExp;// дополнительные очки
		public int SecondsElapsed;// время прохождения
		public int LevelUpdate;// какая прибавка к уровню

	}
}
