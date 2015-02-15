using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMBuildCraft
{
	/// <summary>
	/// Хранение одного ресурса
	/// </summary>
	public class ResourcePacket
	{
		/// <summary>
		/// Предел, после которого происходит присвоение целой части
		/// </summary>
		protected int CountMax = 1000000;

		/// <summary>
		/// Тип ресурса
		/// </summary>
		public ResourceEnum Res { get; protected set; }

		/// <summary>
		/// Количество ресурса (частичное)
		/// </summary>
		public Decimal Count { get; protected set; }

		public ResourcePacket(ResourceEnum res, Decimal count)
		{
			Res = res;
			Count = count;
		}

		public static ResourcePacket operator +(ResourcePacket a, ResourcePacket b)
		{
			if (a.Res != b.Res) return a;// разные ресурсы - выходим
			var c1 = a.Count + b.Count;
			return new ResourcePacket(a.Res, c1);
		}

		public static ResourcePacket operator -(ResourcePacket a, ResourcePacket b)
		{
			if (a.Res != b.Res) return a;// разные ресурсы - выходим
			var c1 = a.Count - b.Count;
			return new ResourcePacket(a.Res, c1);
		}

		public static Boolean operator <=(ResourcePacket a, ResourcePacket b)
		{
			if (a.Res != b.Res) return false;
			return a.Count <= b.Count;
		}

		public static Boolean operator >=(ResourcePacket a, ResourcePacket b)
		{
			if (a.Res != b.Res) return false;
			return a.Count >= b.Count;
		}
		/// <summary>
		/// Добавить ресурс
		/// </summary>
		/// <param name="a"></param>
		public void Add(ResourcePacket a)
		{
			if (a.Res != this.Res) return;
			Count += a.Count;
		}
	
		/// <summary>
		/// Отнять ресурс
		/// </summary>
		/// <param name="a"></param>
		public void Minus(ResourcePacket a)
		{
			if (a.Res != this.Res) return;
			Count -= a.Count;
		}

		public void Clear()
		{
			this.Count = 0;
		}
	}
}
