using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Engine.Utils.Settings
{
	// сделать клаcс для работы с настройками и статический класс для чтения стандартных настроек
	// TODO апробировать и сделать что бы была работа с разными файлами
	
	/// <summary>
	/// Работа с настройками, сохранение и загрузка
	/// </summary>
	public class Settings
	{
		/// <summary>
		/// Расширение файла настроек
		/// </summary>
		public static string FileExt = ".settings";

		/// <summary>
		/// Настройки движка
		/// </summary>
		public static Settings EngineSettings = Load("engineSettings");

		/// <summary>
		/// Все настройки
		/// </summary>
		public List<SettingsRow> Values = new List<SettingsRow>();

		/// <summary>
		/// получить значение переменной по имени
		/// </summary>
		/// <param name="section">Секция в которой определена переменная</param>
		/// <param name="name">Имя переменной. Могут повторяться в разных секциях!</param>
		/// <returns></returns>
		public string GetValue(string section, string name)
		{
			foreach (var settingsRow in Values)
			{
				if (settingsRow.Section != section) continue;
				if (settingsRow.Name != name) continue;
				return settingsRow.Value;// выходим отсюда и возвращаем результат
			}
			return "";
		}

		/// <summary>
		/// получить значения переменных по имени секции
		/// </summary>
		/// <param name="section">Секция в которой определены переменные</param>
		/// <returns></returns>
		public List<SettingsRow> GetValues(string section)
		{
			var list = new List<SettingsRow>();
			foreach (var settingsRow in Values) {
				if (settingsRow.Section != section) continue;
				list.Add(settingsRow);
			}
			return list;// выходим отсюда и возвращаем результат;
		}

		/// <summary>
		/// Установить значение переменной
		/// </summary>
		/// <param name="section">Секция в которой определена переменная</param>
		/// <param name="name">Имя переменной. Могут повторяться в разных секциях!</param>
		/// <param name="value">Новое значение переменной</param>
		/// <returns></returns>
		public void SetValue(string section, string name, string value)
		{
			var row = SearchRow(section,name);     // ищем такое же
			if (row == null) return;        // если нету то дальше ищем
			// нашли - присваиваем value новое
			row.Value = value;
		}

		/// <summary>
		/// удалить значение
		/// </summary>
		/// <param name="section">Секция в которой определена переменная</param>
		/// <param name="name">Имя переменной. Могут повторяться в разных секциях!</param>
		/// <returns></returns>
		public void RemoveValue(string section, string name)
		{
			var row = SearchRow(section,name);     // ищем такое же
			if (row == null) return;        // если нету то дальше ищем
			// нашли - удаляем
			RemoveValue(row);
		}

		/// <summary>
		/// Удалиь значение
		/// </summary>
		/// <param name="row"></param>
		public void RemoveValue(SettingsRow row)
		{
			Values.Remove(row);
		}

		/// <summary>
		/// установить значения по умолчанию
		/// </summary>
		public void Init()
		{
			AddValue("", "", "100", "test");
			AddValue("MainSettings", "settingsFile", "settings.xml", "Местоположение файла настроек");
			AddValue("WindowOptions", "windowWidth", "1024", "Ширина окна");
			AddValue("WindowOptions", "windowHeight", "768", "Высота окна");
		}

		/// <summary>
		/// добавляем строку настройки. Ручное добавление
		/// </summary>
		/// <param name="section">раздел к которому относится переменная</param>
		/// <param name="name">имя переменной</param>
		/// <param name="value">начальное значение</param>
		/// <param name="hint">Подсказка</param>
		public void AddValue(string section, string name, string value, string hint)
		{
			var row = SearchRow(section,name);
			if (row != null){row.Value = value;row.Hint = hint;}
			else Values.Add(new SettingsRow(section, name, value, hint));
		}
		
		/// <summary>
		/// добавляем строку настройки
		/// </summary>
		public void AddValue(SettingsRow row)
		{
			AddValue(row.Section, row.Name, row.Value, row.Hint);
		}


		/// <summary>
		/// сохраняем файл настроек
		/// </summary>
		public void Save(String fileName)
		{
			fileName = ModifyFileName(fileName);
			// массив для сохранения. сохраняем только те значения, которые отличаются от значений по умолчанию
			var toSave = Values;
			//Values.Where(settingsRow => settingsRow.Value != settingsRow.DefaultValue).ToList();
			// стандартное сохранение
			var serializer = new XmlSerializer(typeof(List<SettingsRow>));
			TextWriter writer = new StreamWriter(fileName);
			serializer.Serialize(writer, toSave);
			writer.Close();
		}

		/// <summary>
		/// Проверяет расширение файла. если нету - добавляет
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		private static String ModifyFileName(String fileName)
		{
			var e = System.IO.Path.GetExtension(fileName);
			if (String.IsNullOrEmpty(e))
			{
				fileName += FileExt;
			}
			return fileName;
		}

		/// <summary>
		/// Ищем запись по параметрам и узнаём её номер
		/// </summary>
		/// <param name="sectionName"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public SettingsRow SearchRow(String sectionName, String name)
		{
			foreach (var value in Values){
				if (value.Section != sectionName) continue;
				if (value.Name != name) continue;
				return value;
			}
			return null;
		}

		/// <summary>
		/// Ищем запись, похожую на переданную
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public SettingsRow SearchRow(SettingsRow r)
		{
			return SearchRow(r.Section, r.Name);
		}

		/// <summary>
		/// загружаем файл настроек
		/// </summary>
		public static Settings Load(String fileName)
		{
			fileName = ModifyFileName(fileName);
			var settings = new Settings();
			// массив для сохранения
			// var fromLoad = new List<settingsRow>();
			var serializer = new XmlSerializer(typeof(List<SettingsRow>));
			try
			{
				StreamReader reader = new StreamReader(fileName);
				var fromLoad = (List<SettingsRow>)serializer.Deserialize(reader);
				reader.Close();
				settings.Values = fromLoad;
			}
			catch (Exception)
			{
				settings.AddValue("", "", "100", "test");
				settings.AddValue("MainSettings", "settingsFile", "settings.xml", "Местоположение файла настроек");
				settings.AddValue("WindowOptions", "windowWidth", "1024", "Ширина окна");
				settings.AddValue("WindowOptions", "windowHeight", "768", "Высота окна");
			}
			return settings;
		}

		public void ClearSection(string sectionName)
		{
			// копия основного списка
			var settingsRows = new List<SettingsRow>(Values);
			foreach (var settingsRow in settingsRows){
				if (settingsRow.Section==sectionName){// удаляем эту секцию
					Values.Remove(settingsRow);
				}
			}
		}

		public void Sort1()
		{
			Values.Sort(StandartCompare);
		}

		private int StandartCompare(SettingsRow x, SettingsRow y)
		{
			var r1 = x.Section.CompareTo(y.Section);
			if (r1 != 0) return r1;
			var r2 = x.Name.CompareTo(y.Name);
			return r2;
		}
	}
}
