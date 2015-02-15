using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMBuildCraft
{
	/// <summary>
	/// Набор ресурсов. На складе или при перемещении
	/// </summary>
	public class ResourcesPackets
	{
		/// <summary>
		/// Список ресурсов
		/// </summary>
		public List<ResourcePacket> Resources { get; protected set; }

		public ResourcesPackets()
		{
			Resources=new List<ResourcePacket>();
			Resources.Add(new ResourcePacket(ResourceEnum.Wood, 0));
			Resources.Add(new ResourcePacket(ResourceEnum.Metal, 0));
			Resources.Add(new ResourcePacket(ResourceEnum.Sand, 0));
		}

		/// <summary>
		/// Установить количество ресурса. в основном для ограничителей и т.п.
		/// </summary>
		/// <param name="resourceEnum"></param>
		/// <param name="count"></param>
		public void SetValue(ResourceEnum resourceEnum, decimal count)
		{
			var rs = Resources.FirstOrDefault(r => r.Res == resourceEnum);
			if (rs == null) return;
			rs.Clear();
			rs.Add(new ResourcePacket(resourceEnum, count));
		}

		/// <summary>
		/// Получить пакет указанного типа ресурсов
		/// </summary>
		/// <param name="resourceEnum"></param>
		public ResourcePacket GetValue(ResourceEnum resourceEnum)
		{
			var rs = Resources.FirstOrDefault(r => r.Res == resourceEnum);
			return rs;
		}

		/// <summary>
		/// Проверить, есть ли в этих ресурсах требуемое количество ресурсов
		/// </summary>
		/// <param name="resourcesVerify"></param>
		/// <returns></returns>
		public Boolean Available(ResourcesPackets resourcesVerify)
		{
			foreach (var resourcePacket in resourcesVerify.Resources){
				if (!Available(resourcePacket))return false;
			}
			return true;
		}

		public Boolean Available(ResourcePacket resourcePacket)
		{
			var rs = Resources.FirstOrDefault(r => r.Res == resourcePacket.Res);
			if (rs == null) return false;
			if (rs <= resourcePacket) return false;
			return true;
		}

		/// <summary>
		/// Отнять переданное количество ресурсов
		/// </summary>
		/// <param name="value"></param>
		public void Minus(ResourcesPackets value)
		{
			foreach (var resource in value.Resources){
				Minus(resource);
			}
		}

		/// <summary>
		/// Отнять переданное количество ресурсов
		/// </summary>
		/// <param name="value"></param>
		public void Minus(ResourcePacket value)
		{
			var rs = Resources.FirstOrDefault(r => r.Res == value.Res);
			if (rs == null) return;
			rs.Minus(value);
		}

		/// <summary>
		/// Отнять переданное количество ресурсов
		/// </summary>
		/// <param name="value"></param>
		public void Add(ResourcesPackets value)
		{
			foreach (var resource in value.Resources)
			{
				Add(resource);
			}
		}

		/// <summary>
		/// Отнять переданное количество ресурсов
		/// </summary>
		/// <param name="value"></param>
		public void Add(ResourcePacket value)
		{
			var rs = Resources.FirstOrDefault(r => r.Res == value.Res);
			if (rs == null) return;
			rs.Add(value);
		}

	}
}
