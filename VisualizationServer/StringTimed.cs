using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizationServer
{
	class StringTimed
	{
		public DateTime DeadLine;
		public TimeSpan DeltaTime;
		public string Text;
		/// <summary>
		/// Идентификатор строки, что бы переопределять конкретную строку, несмотря на разные значения текста
		/// </summary>
		public int stringID;

		public Boolean Updated;

		public static StringTimed Create(int strID, DateTime currTime, TimeSpan delta, string initText)
		{
			var r = new StringTimed();
			r.stringID = strID;
			r.DeltaTime = delta;
			r.DeadLine = currTime + delta;
			r.Text = initText;
			r.Updated=true;
			return r;
		}

		/// <summary>
		/// Обновить текст, если возможно
		/// </summary>
		/// <remarks>Основное назначение - фиьтрация одинаковых строк, что бы строка была в одном экземпляре и не дублировалась</remarks>
		/// <returns>Обновлена ли строка у этого объекта</returns>
		public Boolean AddText(int strID,DateTime currTime, string txt)
		{
			var ret = false;
			if (strID != -1){// задан идентификатор
				if (strID == stringID){// Идентификатор совпадает, надо обрабатывать
					ret = true;
					if (currTime >= DeadLine){// если время подошло
						DeadLine = DateTime.Now + DeltaTime;
						Text = txt;
						Updated = true;
					}
				}
			}else{// если ИД не задан
				if (Text == txt){// если текст такой же то обновляем таймер
					ret = true;
					if (currTime >= DeadLine){// если время подошло
						DeadLine = DateTime.Now + DeltaTime;
						Updated = true;
					}
				}
			}
			return ret;
		}
	}
}
