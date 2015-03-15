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
		/// Большой склад. Больше чем вселенский, потому что вселенский промежуточный, а этот основательный. Но должен быть дорогим
		/// </summary>
		StorageBig,
		/// <summary>
		/// Склад для передачи ресурсов между звёздными системами
		/// </summary>
		StorageUniverse,
		/// <summary>
		/// Технология для апгрейда зданий - игрок выбирает здание и какой апгрейд туда надо установить.
		/// </summary>
		MiningTechnology,
		/// <summary>
		/// Ещё более редкий ресурс для апгрейда зданий
		/// </summary>
		MiningArtefacts,
		/// <summary>
		/// Преобразовывает любой ресурс в любой другой. апгрейдится.
		/// </summary>
		Converter,
		MiningSepulki,
		MiningMarkwi,
		MiningPchmy,
		MiningIiont,
		FactoryTechnologies,
		FactoryArtefacts,
		Factory1,
		Factory2,
		Factory3,
		Factory4,
	}
}
