using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMapEditor
{
	/// <summary>
	/// Соответствие и некоторая информация об объекте по его типу
	/// </summary>
	public static class ObjectTypeAtlas
	{
		private static Dictionary<ObjectTypes, int> _textureParts = new Dictionary<ObjectTypes, int>();
		private static Dictionary<ObjectTypes, string> _textDescription = new Dictionary<ObjectTypes, string>();

		public static int GetTextureNum(ObjectTypes type)
		{
			var ret = -1;
			if (_textureParts.ContainsKey(type))
			{
				ret = _textureParts[type];
			}
			return ret;
		}

		public static String GetDescription(ObjectTypes type)
		{
			var ret = "none";
			if (_textureParts.ContainsKey(type))
			{
				ret = _textDescription[type];
			}
			return ret;
		}

		private static void Add(ObjectTypes type, int num, string description)
		{
			_textureParts.Add(type, num);
			_textDescription.Add(type, description);
		}

		static ObjectTypeAtlas()
		{
			Add(ObjectTypes.Empty, 0, "Пустой блок");
			Add(ObjectTypes.Start, 1, "Стартовый блок");
			Add(ObjectTypes.Finish, 2, "Финишный блок");
			Add(ObjectTypes.Wall1, 3, "Стена1");
			Add(ObjectTypes.Wall2, 4, "Стена2");
			Add(ObjectTypes.Wall3, 5, "Стена3");
			Add(ObjectTypes.Wall4, 6, "Стена4");
			Add(ObjectTypes.Wall5, 7, "Стена5");
			Add(ObjectTypes.Moved, 8, "Подвижный блок");
			Add(ObjectTypes.Enemy, 9, "Враг");
			Add(ObjectTypes.Pusher, 10, "Смещающий блок");
			Add(ObjectTypes.Secret, 11, "Секретный блок");
			Add(ObjectTypes.Teleport, 12, "Телепорт");
			Add(ObjectTypes.Flasher, 13, "Мигающий блок");
			Add(ObjectTypes.Decorative1, 14, "Декоративный блок 1");
			Add(ObjectTypes.Decorative2, 15, "Декоративный блок 2");
			Add(ObjectTypes.Switcher, 16, "Переключатель");
			Add(ObjectTypes.SwitchableOn, 17, "Включенный Переключаемый блок");
			Add(ObjectTypes.SwitchableOff, 18, "Выключенный Переключаемый блок");
			Add(ObjectTypes.Path, 19, "Точка пути");
			Add(ObjectTypes.Bullet, 20, "Пуля");
			Add(ObjectTypes.SpeedUp, 21, "Импульсный блок");
			Add(ObjectTypes.ShipPart1, 35, "Часть корабля 1");
			Add(ObjectTypes.ShipPart2, 43, "Часть корабля 2");
		}
	}
}
