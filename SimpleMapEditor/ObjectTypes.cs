using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMapEditor
{
	public enum ObjectTypes
	{
		/// <summary>Для удаления или когда объект не заполнен</summary>
		Empty,
		/// <summary>Стартовый блок</summary>
		Start,
		/// <summary>Завершающий блок</summary>
		Finish,
		/// <summary>Стена</summary>
		Wall1,
		/// <summary>Стена</summary>
		Wall2,
		/// <summary>Стена</summary>
		Wall3,
		/// <summary>Стена</summary>
		Wall4,
		/// <summary>Стена</summary>
		Wall5,
		/// <summary>Подвижный блок</summary>
		Moved,
		/// <summary>Вражеский блок</summary>
		Enemy,
		/// <summary>Сдвигающий блок</summary>
		Pusher,
		/// <summary>Знак секретный блок, для редактора</summary>
		Secret,
		/// <summary>Блок телепорта</summary>
		Teleport,
		/// <summary>Знак Мигающий блок, для редактора</summary>
		Flasher,
		/// <summary>Прозрачный декоративный блок 1</summary>
		Decorative1,
		/// <summary>Прозрачный декоративный блок 1</summary>
		Decorative2,
		/// <summary>Переключатель</summary>
		Switcher,
		/// <summary>Переключаемый блок Включен</summary>
		SwitchableOn,
		/// <summary>Переключаемый блок Выключен</summary>
		SwitchableOff,
		/// <summary>Путь</summary>
		Path,
		/// <summary>Пуля</summary>
		Bullet,
		/// <summary>Импульсный блок</summary>
		SpeedUp,
		/// <summary>Часть корабля 1</summary>
		ShipPart1,
		/// <summary>Часть корабля 2</summary>
		ShipPart2,

	}
}
