using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMBuildCraft.Buildings
{
	/// <summary>
	/// Склад
	/// </summary>
	class BuildingStore:Building
	{
		public override void SetValuеsDefault()
		{
			base.SetValuеsDefault();
			StoredMax.SetValue(ResourceEnum.Metal,10000);
		}

	}
}
