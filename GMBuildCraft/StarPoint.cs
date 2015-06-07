using Engine.Utils.Path;
using GMBuildCraft.BaseClasses;
using GMBuildCraft.Buildings;

namespace GMBuildCraft
{
	/// <summary>
	/// Узловая точка звёздной системы
	/// </summary>
	public class StarPoint:NodePoint
	{
		/// <summary>
		/// Тип стоящего здания, в том числе для визуализации
		/// </summary>
		public BuildingType BuildingType;

		public Building Building;

		public StarSystem Parent;

		public StarPoint(Point point, StarSystem parent) : base(point)
		{
			Parent = parent;
			AvailableResources=new ResourcesPackets();
			InitResourcesIn();
		}

		/// <summary>
		/// Инициализация ресурсов
		/// </summary>
		public void InitResourcesIn()
		{
			AvailableResources.SetValue(ResourceEnum.Sepulki,10000);
			AvailableResources.SetValue(ResourceEnum.Markwi, 10000);
			AvailableResources.SetValue(ResourceEnum.Pchmy, 10000);
			AvailableResources.SetValue(ResourceEnum.Iiont, 0);
			AvailableResources.SetValue(ResourceEnum.Technologies, 1000);
			AvailableResources.SetValue(ResourceEnum.Artefacts, 1000);
		}


		public override void DoWork()
		{
			base.DoWork();
			Building.Process(AvailableResources);
			// дороги в другом месте будут перемещать ресурсы, а то будет 2 раза перемещать, это не дело
			//foreach (var road in roads){road.Process(this, Building.Stored);}

		}
	}
}
