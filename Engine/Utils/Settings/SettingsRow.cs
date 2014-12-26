using System;

namespace Engine.Utils.Settings
{
	/// <summary>
	/// Строка настройки
	/// </summary>
	[Serializable]
	public class SettingsRow
	{
		/// <summary>
		/// раздел
		/// </summary>
		public String Section;
		/// <summary>
		/// Имя
		/// </summary>
		public String Name;
		/// <summary>
		/// значение
		/// </summary>
		public string Value;
		/// <summary>
		/// Пояснение
		/// </summary>
		public string Hint;

		/// <summary>
		/// Конструктор без параметров
		/// </summary>
		public SettingsRow()
		{
			Section="none";
			Name = "none";
			Value = "";
			Hint = "none";
		}

		/// <summary>
		/// конструктор
		/// </summary>
		/// <param name="section">раздел</param>
		/// <param name="name">имя</param>
		/// <param name="value">Значение</param>
		/// <param name="hint">подсказка</param>
		public SettingsRow(string section, string name, string value, string hint)
		{
			Section = section;
			Name = name;
			Value = value;
			Hint = hint;
		}

		/// <summary>
		/// "конструктор"
		/// </summary>
		/// <param name="section">раздел</param>
		/// <param name="name">имя</param>
		/// <param name="value">Значение</param>
		/// <param name="hint">подсказка</param>
		public static SettingsRow Create(string section, string name, string value, string hint)
		{
			return new SettingsRow(section, name, value, hint);
		}

	}
}
