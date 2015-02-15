using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMBuildCraft.Limiters
{
	/// <summary>
	/// Ограничитель. блокирует любые операции на узловой точке
	/// </summary>
	class Limiter
	{
		public ResourceEnum ResourceEnum;
		public Decimal ResourceCount;
		public StarPoint StarPoint;
		private ResourcePacket _resourcePacket;

		/// <summary>
		/// Блокирован ли ограничитель
		/// </summary>
		public Boolean Blocked{get; private set;}

		public Limiter(StarPoint starPoint, ResourceEnum resourceEnum, decimal count)
		{
			StarPoint = starPoint;
			ResourceEnum = resourceEnum;
			ResourceCount = count;
			_resourcePacket = new ResourcePacket(ResourceEnum, ResourceCount);
		}
		
		/// <summary>
		/// Проверить, есть ли нужное количество ресурсов на складах, соединенных дорогой
		/// </summary>
		/// <returns></returns>
		protected virtual Boolean VerifyMe()
		{
			// проверяем у ближайших узловых точек наличие ресурсов
			foreach (var road in StarPoint.roads){
				StarPoint p = (StarPoint) road.NodePoint1;
				if (p == StarPoint) p = (StarPoint) road.NodePoint2;// получаем противоположную точку дороги
				if (p.Building.Stored.Available(_resourcePacket)){
					p.Building.Stored.Minus(_resourcePacket);
					return false;
				}
			}
			return true;
		}

		public Boolean Verify()
		{
			// если закрыт - не проверяем
			if (Blocked)
				Blocked = VerifyMe();
			return Blocked;
		}

	}
}
