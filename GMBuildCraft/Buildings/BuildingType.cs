using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMBuildCraft
{
	public enum  BuildingType
	{
		/// <summary>
		/// Стандартный мелкий склад, значение по умолчанию
		/// </summary>
		None,
		/// <summary>
		/// Склад
		/// </summary>
		Storage,
		/// <summary>
		/// Большой склад
		/// </summary>
		StorageBig,
		/// <summary>
		/// Склад для передачи ресурсов между звёздными системами
		/// </summary>
		StorageUniverse,
		/// <summary>
		/// Технология для апгрейда зданий - игрок выбирает здание и какой апгрейд туда надо установить.
		/// </summary>
		StarTechnology1,
		/// <summary>
		/// Ещё более редкий ресурс для апгрейда зданий
		/// </summary>
		StarTechnology2,
		/// <summary>
		/// Преобразовывает любой ресурс в любой другой. апгрейдится.
		/// </summary>
		Converter,
		Mining1,
		Mining2,
		Mining3,
		Mining4,
		Mining5,
		Factory1,
		Factory2,
		Factory3,
		Factory4,
		Factory5
	}
}
