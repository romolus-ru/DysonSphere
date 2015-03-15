using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMBuildCraft.Buildings
{
	class BuildingDescriptions
	{
		public List<BuildingDescription> info = new List<BuildingDescription>();

		public BuildingDescriptions() { Init();}

		public void Init()
		{
			info.Add(new BuildingDescription{
				BuildingType = BuildingType.None,
				Name="Сепульки",
				Description = "Сепульки",
				TexAddress = @"..\Resources\gmBuildCraft\res_sep.png",
				TexName = "GMBuildCraftResSep",
				ConvertedResource1 = ResourceEnum.Sepulki,
				ConvertedResource2 = ResourceEnum.Sepulki,
				FactoryResource = ResourceEnum.Sepulki,
				MiningResource = ResourceEnum.Sepulki,
				CanUpgrade = false,
			});

			info.Add(new BuildingDescription
			{
				BuildingType = BuildingType.Storage,
				Name = "Мелкий склад",
				Description = "Мелкий склад",
				TexAddress = @"..\Resources\gmBuildCraft\res_sep.png",
				TexName = "GMBuildCraftResSep",
				CanUpgrade = true,
			});

			info.Add(new BuildingDescription
			{
				BuildingType = BuildingType.StorageUniverse,
				Name = "Вселенский склад",
				Description = "Вселенский склад",
				TexAddress = @"..\Resources\gmBuildCraft\res_sep.png",
				TexName = "GMBuildCraftResSep",
				CanUpgrade = false,
			});

			info.Add(new BuildingDescription
			{
				BuildingType = BuildingType.StorageBig,
				Name = "Большой склад",
				Description = "Большой склад",
				TexAddress = @"..\Resources\gmBuildCraft\res_sep.png",
				TexName = "GMBuildCraftResSep",
				CanUpgrade = true,
			});

			info.Add(new BuildingDescription
			{
				BuildingType = BuildingType.MiningTechnology,
				Name = "Технологии",
				Description = "Добываем Технологии",
				TexAddress = @"..\Resources\gmBuildCraft\res_sep.png",
				TexName = "GMBuildCraftResSep",
				MiningResource = ResourceEnum.Technologies,
				CanUpgrade = true,
			});

			info.Add(new BuildingDescription
			{
				BuildingType = BuildingType.MiningArtefacts,
				Name = "Артефакты",
				Description = "Добываем Артефакты",
				TexAddress = @"..\Resources\gmBuildCraft\res_sep.png",
				TexName = "GMBuildCraftResSep",
				MiningResource = ResourceEnum.Artefacts,
				CanUpgrade = true,
			});

			info.Add(new BuildingDescription
			{
				BuildingType = BuildingType.Converter,
				Name = "Преобразователь",
				Description = "Преобразователь",
				TexAddress = @"..\Resources\gmBuildCraft\res_sep.png",
				TexName = "GMBuildCraftResSep",
				MiningResource = ResourceEnum.Technologies,
				CanUpgrade = true,
			});

			info.Add(new BuildingDescription
			{
				BuildingType = BuildingType.MiningSepulki,
				Name = "Сепульки",
				Description = "Добываем Сепульки",
				TexAddress = @"..\Resources\gmBuildCraft\res_sep.png",
				TexName = "GMBuildCraftResSep",
				MiningResource = ResourceEnum.Sepulki,
				CanUpgrade = true,
			});

			info.Add(new BuildingDescription
			{
				BuildingType = BuildingType.MiningMarkwi,
				Name = "Маркви",
				Description = "Добываем Маркви",
				TexAddress = @"..\Resources\gmBuildCraft\res_sep.png",
				TexName = "GMBuildCraftResSep",
				MiningResource = ResourceEnum.Markwi,
				CanUpgrade = true,
			});

			info.Add(new BuildingDescription
			{
				BuildingType = BuildingType.MiningPchmy,
				Name = "Пчмы",
				Description = "Добываем Пчмы",
				TexAddress = @"..\Resources\gmBuildCraft\res_sep.png",
				TexName = "GMBuildCraftResSep",
				MiningResource = ResourceEnum.Pchmy,
				CanUpgrade = true,
			});

			info.Add(new BuildingDescription
			{
				BuildingType = BuildingType.MiningIiont,
				Name = "Иионт",
				Description = "Добываем Иионт",
				TexAddress = @"..\Resources\gmBuildCraft\res_sep.png",
				TexName = "GMBuildCraftResSep",
				MiningResource = ResourceEnum.Iiont,
				CanUpgrade = true,
			});

		
		}

		/// <summary>
		/// Получить описание ресурса
		/// </summary>
		/// <param name="buildingType"></param>
		/// <returns></returns>
		public BuildingDescription GetInfo(BuildingType buildingType)
		{
			BuildingDescription ret = null;
			foreach (var buildingDescription in info){
				if (buildingDescription.BuildingType == buildingType){
					ret = buildingDescription;
					break;
				}
			}
			return ret;
		}

		/// <summary>
		/// Описания ресурсов
		/// </summary>
		public static ResourcesDesctiptions rd = new ResourcesDesctiptions();
	}
}
