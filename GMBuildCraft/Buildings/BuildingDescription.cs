using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMBuildCraft.Buildings
{
	class BuildingDescription
	{
		public BuildingType BuildingType;
		public String Name;
		public String Description;
		public String TexName;
		public String TexAddress;
		public ResourceEnum MiningResource;
		public ResourceEnum FactoryResource;
		public ResourceEnum ConvertedResource1;
		public ResourceEnum ConvertedResource2;
		public Boolean CanUpgrade;
	}
}
