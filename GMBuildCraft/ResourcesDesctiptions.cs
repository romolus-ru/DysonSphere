using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMBuildCraft
{
	/// <summary>
	/// Описания ресурсов
	/// </summary>
	class ResourcesDesctiptions
	{
		public List<ResourcesDescription> info = new List<ResourcesDescription>();

		public ResourcesDesctiptions() { Init();}

		public void Init()
		{
			info.Add(new ResourcesDescription{
				ResourceEnum = ResourceEnum.Sepulki,
				Name="Сепульки",
				Description = "Сепульки",
				TexAddress = @"..\Resources\gmBuildCraft\res_sep.png",
				TexName = "GMBuildCraftResSep",
				Color=Color.Tomato,
				Complexity = 2000,
				OverComplexity = 50,
			});

			info.Add(new ResourcesDescription
			{
				ResourceEnum = ResourceEnum.Markwi,
				Name = "Маркви",
				Description = "Маркви",
				TexAddress = @"..\Resources\gmBuildCraft\res_mar.png",
				TexName = "GMBuildCraftResMar",
				Color = Color.SpringGreen,
				Complexity = 2000,
				OverComplexity = 50,
			});

			info.Add(new ResourcesDescription
			{
				ResourceEnum = ResourceEnum.Pchmy,
				Name = "Пчмы",
				Description = "Пчмы",
				TexAddress = @"..\Resources\gmBuildCraft\res_pch.png",
				TexName = "GMBuildCraftResPch",
				Color = Color.SandyBrown,
				Complexity = 1700,
				OverComplexity = 40,
			});

			info.Add(new ResourcesDescription
			{
				ResourceEnum = ResourceEnum.Iiont,
				Name = "Иионт",
				Description = "Иионт",
				TexAddress = @"..\Resources\gmBuildCraft\res_iio.png",
				TexName = "GMBuildCraftResIio",
				Color = Color.RoyalBlue,
				Complexity = 1400,
				OverComplexity = 35,

			});

			info.Add(new ResourcesDescription
			{
				ResourceEnum = ResourceEnum.Technologies,
				Name = "Технологии",
				Description = "Технологии",
				TexAddress = @"..\Resources\gmBuildCraft\res_tech.png",
				TexName = "GMBuildCraftResTech",
				Color = Color.Wheat,
				Complexity = 100,
				OverComplexity = 2,
			});

			info.Add(new ResourcesDescription
			{
				ResourceEnum = ResourceEnum.Artefacts,
				Name = "Артефакты",
				Description = "Артефакты",
				TexAddress = @"..\Resources\gmBuildCraft\res_artf.png",
				TexName = "GMBuildCraftResArtf",
				Color = Color.LightSalmon,
				Complexity = 50,
				OverComplexity = 1,
			});

		}

		/// <summary>
		/// Получить описание ресурса
		/// </summary>
		/// <param name="resourceEnum"></param>
		/// <returns></returns>
		public ResourcesDescription GetInfo(ResourceEnum resourceEnum)
		{
			ResourcesDescription ret = null;
			foreach (var resourcesDescription in info){
				if (resourcesDescription.ResourceEnum == resourceEnum){
					ret = resourcesDescription;
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
