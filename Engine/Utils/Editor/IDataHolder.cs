using System;
using System.Collections.Generic;

namespace Engine.Utils.Editor
{
	/// <summary>
	/// Для поддержки возможности сохранения и чтения данных из Dictionary string,string
	/// </summary>
	/// <remarks>В основном планируется использовать в редакторе. 
	/// интерфейс требует реализации сохранения данных в словарь и чтение из словаря
	/// получается дольше чем другим способом, но зато весь контроль сохранения остаётся за классом редактора
	/// </remarks>
	public interface IDataHolder
	{
		/// <summary>
		/// Номер объекта
		/// </summary>
		int Num { get; set; }

		/// <summary>
		/// Сохранить данные в словарь
		/// </summary>
		/// <returns></returns>
		Dictionary<String, String> Save();

		/// <summary>
		/// Загрузить данные из словаря
		/// </summary>
		/// <param name="data"></param>
		void Load(Dictionary<String, String> data);

	}
}
