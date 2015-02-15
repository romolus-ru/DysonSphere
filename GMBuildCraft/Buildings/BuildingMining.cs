using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMBuildCraft.Buildings
{
	/// <summary>
	/// Добывающее здание
	/// </summary>
	class BuildingMining:Building
	{

		public ResourceEnum MiningResource;

		public int MiningCount;

		public BuildingMining(ResourceEnum miningResource)
		{
			MiningResource = miningResource;
		}

		public override void SetValuеsDefault()
		{
			MiningCount = 100;// потом это значение прогоняется через апгрейды и подобное
			base.SetValuеsDefault();
		}

		public override void Process(ResourcesPackets availableResources)
		{
			base.Process(availableResources);
			var rp = new ResourcePacket(MiningResource, MiningCount);
			if (!availableResources.Available(rp)){// ресурсов не достаточно, получаем актуальное количество ресурсов
				var a = availableResources.GetValue(MiningResource).Count;
				rp = new ResourcePacket(MiningResource, a);
			}
			availableResources.Minus(rp);
			Stored.Add(rp);
		}
	}
}
