using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMBuildCraft.Buildings;

namespace GMBuildCraft.Upgrades
{
	/// <summary>
	/// Основа всех улучшений
	/// </summary>
	public class Upgrade
	{
		/// <summary>
		/// Поставить улучшение
		/// </summary>
		/// <param name="building"></param>
		public virtual void SetUpdrage(Building building) { }

		public UpgradeTypes type;
		
		/// <summary>
		/// Доступно ли для строительства
		/// </summary>
		public Boolean Opened;

		public ResourcesPackets Cost { get; protected set; }

		public Upgrade()
		{
			Cost = new ResourcesPackets();// ничего не стоит по умолчанию
		}
	}
}
