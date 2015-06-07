using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMBuildCraft.Upgrades;
using Engine.Attributes;

namespace GMBuildCraft.Buildings
{
	/// <summary>
	/// Основа для всех зданий
	/// </summary>
	/// <remarks>Сделан отдельно для безболезненной замены здания у узловой точки</remarks>
	[LibraryClass("Building","1.0")]
	[DebuggerDisplay("IsConstucted={IsConstucted}")]
	public class Building
	{
		/// <summary>
		/// В процессе постройки здание только собирает ресурсы и может показать сколько ресурсов уже собрано
		/// </summary>
		public Boolean IsConstucted = true;

		/// <summary>
		/// Количество ресурсов на складе
		/// </summary>
		public ResourcesPackets Stored;

		/// <summary>
		/// Предел склада, для разных зданий может меняться, в том числе и улучшениями
		/// </summary>
		public ResourcesPackets StoredMax;

		/// <summary>
		/// Максимальное количество улучшений, определяется уровнем узловой точки
		/// </summary>
		public int MaxUpgrades;
		
		/// <summary>
		/// Список улучшений здания
		/// </summary>
		public List<Upgrade> Upgrades;

		/// <summary>
		/// Список строящихся улучшений
		/// </summary>
		public List<Upgrade> UpgradesToBuild;

		public Building()
		{
			Stored=new ResourcesPackets();
			Stored.SetValue(ResourceEnum.Sepulki, 0);
			Stored.SetValue(ResourceEnum.Markwi, 0);
			Stored.SetValue(ResourceEnum.Pchmy, 0);
			Stored.SetValue(ResourceEnum.Iiont, 0);
			Stored.SetValue(ResourceEnum.Technologies, 0);
			Stored.SetValue(ResourceEnum.Artefacts, 0);
		}

		/// <summary>
		/// Установить значения с учётом улучшений
		/// </summary>
		public void SetValues()
		{
			SetValuеsDefault();
			BuyUpgrades();
			foreach (var upgrade in Upgrades){
				upgrade.SetUpdrage(this);
			}
		}

		/// <summary>
		/// Покупаем улучшения
		/// </summary>
		private void BuyUpgrades()
		{
			if (UpgradesToBuild.Count==0)return;
			var buyed = new List<Upgrade>();// купленные улучшения
			foreach (var upgrade in UpgradesToBuild){
				if (!upgrade.Opened) continue;
				if (this.Stored.Available(upgrade.Cost)){
					Stored.Minus(upgrade.Cost);
					buyed.Add(upgrade);
				}
			}
			foreach (var upgrade in buyed){
				UpgradesToBuild.Remove(upgrade);
				Upgrades.Add(upgrade);
			}
		}

		/// <summary>
		/// Каждое здание устанавливает свои начальные установки
		/// </summary>
		public virtual void SetValuеsDefault()
		{
			
		}

		/// <summary>
		/// Работа здания. по умолчанию ничего не делаем. У наследников это означает производство товаров, добыча и т.п.
		/// </summary>
		/// <param name="availableResources"></param>
		public virtual void Process(ResourcesPackets availableResources)
		{
			
		}
	}
}
