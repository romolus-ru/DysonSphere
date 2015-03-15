using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Engine.Utils.ExtensionMethods;
using Engine.Utils.Path;
using GMBuildCraft.BaseClasses;
using GMBuildCraft.Buildings;
using GMBuildCraft.Upgrades;
using Point = Engine.Utils.Path.Point;

namespace GMBuildCraft
{
	/// <summary>
	/// Дорога между двумя узловыми точками (в том числе и межзвёздная)
	/// </summary>
	public class Road
	{
		/// <summary>
		/// Объект, к которому принадлежит путь
		/// </summary>
		public SystemPoint Parent;

		/// <summary>
		/// Признак глобального пути (путь соединяет не точки звёздной системы а сами звёздные системы)
		/// </summary>
		public Boolean IsGlobal = false;
		public NodePoint NodePoint1;
		public NodePoint NodePoint2;
		/// <summary>
		/// Угол между точками. нужен для задания направления перемещения груза
		/// </summary>
		public double Angle;
		/// <summary>
		/// Максимальный размер пакета
		/// </summary>
		public int PacketSizeMax;

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
	
		/// <summary>
		/// Передвигающиеся пакеты
		/// </summary>
		public Dictionary<ResourcePacket, int> packets = new Dictionary<ResourcePacket, int>();

		/// <summary>
		/// Направление движения, по умолчанию 0 (может быть неправильно такой большой объект 
		/// использовать для таких целей, может быть переделать на list)
		/// </summary>
		public ResourcesPackets Directions=new ResourcesPackets();

		/// <summary>
		/// Предыдущее направление движения, что бы пакеты могли вернуться обратно
		/// </summary>
		public ResourcesPackets DirectionsOld = new ResourcesPackets();

		public Road(NodePoint np1, NodePoint np2)
		{
			NodePoint1 = np1;
			NodePoint2 = np2;
		}

		private void SetDirection(ResourceEnum resourceEnum, int direction)
		{
			var dOld = Directions.GetValue(resourceEnum).Return(r => r.Count, 0);
			var d = direction;
			if (d < 0) d = -1;
			if (d > 1) d = 1;
			Directions.SetValue(resourceEnum, d);
			if (dOld != 0) { DirectionsOld.SetValue(resourceEnum,dOld);}
		}

		/// <summary>
		/// Установить направление движения
		/// </summary>
		/// <param name="resourceEnum"></param>
		/// <param name="npFrom"></param>
		/// <param name="npTo"></param>
		public void SetDirection(ResourceEnum resourceEnum, NodePoint npFrom, NodePoint npTo)
		{
			if (npFrom != NodePoint1 || npFrom != NodePoint2) return;
			if (npTo != NodePoint1 || npTo != NodePoint2) return;
			var d = 1;
			if (npFrom != NodePoint1) d = -1;
			if (npFrom == npTo) d = 0;// никуда не передаём ресурсы
			SetDirection(resourceEnum, d);
		}

		/// <summary>
		/// Разорвать соединение в этом направлении
		/// </summary>
		/// <param name="resourceEnum"></param>
		/// <param name="npTo"></param>
		public void SetDirectionBreak(ResourceEnum resourceEnum, NodePoint npTo)
		{
			if (npTo != NodePoint1 || npTo != NodePoint2) return;
			var d = 1;
			if (npTo != NodePoint2) d = -1;
			var dOld = Directions.GetValue(resourceEnum).Return(r => r.Count, 0);
			// d - направление куда надо установить, dOld - текущее. по необходимости разрываем соединение
			if (dOld == 0) return;// не двигаемся, не меняем движение
			if (d == dOld) SetDirection(resourceEnum, 0);// раз направления совпадают, т.е. идут к npTo то разрываем путь
		}

		/// <summary>
		/// Покупаем улучшения
		/// </summary>
		private void BuyUpgrades()
		{
			if (UpgradesToBuild.Count == 0) return;
			var buyed = new List<Upgrade>();// купленные улучшения
			var stored1 = (NodePoint1 as StarPoint).Building.Stored;
			var stored2 = (NodePoint2 as StarPoint).Building.Stored;
			foreach (var upgrade in UpgradesToBuild)
			{
				if (!upgrade.Opened) continue;
				if (stored1.Available(upgrade.Cost))
				{
					stored1.Minus(upgrade.Cost);
					buyed.Add(upgrade);
				}else{
				if (stored2.Available(upgrade.Cost))
				{
					stored2.Minus(upgrade.Cost);
					buyed.Add(upgrade);
				}}
			}
			foreach (var upgrade in buyed)
			{
				UpgradesToBuild.Remove(upgrade);
				Upgrades.Add(upgrade);
			}
		}

		/// <summary>
		/// Траектория дороги
		/// </summary>
		public List<PointF> Path;

		/// <summary>
		/// Сгенерировать промежуточные точки дороги, для вывода пути
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		public void CreatePath(Point a, Point b)
		{
			var path = new PathGeneratorBezier();
			var pc = new Point((a.X + b.X)/2, (a.Y + b.Y)/2);// средняя точка
			var p1 = new Point((b.X-a.X)/7, (b.Y-a.Y)/7);// "единичный" вектор, для отступа средних точек
			var pa=new Point(pc.X+p1.Y,pc.Y-p1.X);
			var pb=new Point(pc.X-p1.Y,pc.Y+p1.X);
			var r = path.GenerateBezierPathF(a, pa, pb, b, 30);
			Path = r;
			double x = a.X - b.X;
			double y = a.Y - b.Y;
			var a1 = Math.Atan2(y, x);
			Angle = a1;
		}

		/// <summary>
		/// Перемещение пакетов по дороге в соответствии с направлением
		/// </summary>
		public void Process()
		{
			BuyUpgrades();// покупаем апгрейды, если у одной из узловых точек есть необходимое количество ресурсов
			if (packets.Count < 1) return;// только если есть пакеты
			// перемещаем
			foreach (var directionPacket in Directions.Resources){
				var res = directionPacket.Res;
				var direction = directionPacket.Count;
				if (direction == 0){// пробуем получить направление предыдущего состояния
					direction = Directions.GetValue(res).Return(r => r.Count, 0);
				}
				if (direction==0)continue;// пропускаем если направление не указано для этого типа ресурсов
				var toRemove = new List<ResourcePacket>();// удаляемые
				var ps = packets.Where(packet => packet.Key.Res == res);
				
				foreach (var pair in ps){
					packets.Remove(pair.Key);// удаляем пакет и прибавляем его уже смещенным
					var k = pair.Key;
					int v = pair.Value + (int)direction;
					packets.Add(k, v);
					if (v > 100) toRemove.Add(k);// добавляем для удаления
					if (v < 0) toRemove.Add(k);
				}
				
				// определяем откуда брать пакеты и отправляем дополнительные
				var npnum = 0;// по умолчанию первая точка
				var npFrom = NodePoint1 as StarPoint;
				var npTo = NodePoint2 as StarPoint;
				if (direction == -1){npnum = 100;npFrom = NodePoint2 as StarPoint;npTo = NodePoint1 as StarPoint;}
				
				// удаляем пакеты из списка на удаление и добавляем переданное узловой точке
				foreach (var packet in toRemove){
					packets.Remove(packet);
					npTo.Building.Stored.Add(packet);
				}
				
				// по возможности забираем ресурсы и перемещаем их
				var pkt = new ResourcePacket(res, PacketSizeMax);
				if (npFrom.Building.Stored.Available(pkt)){// ресурсы есть - посылаем пакет
					npFrom.Building.Stored.Minus(pkt);
					packets.Add(pkt, npnum);
				}
			}
		}

		public int GetDirection(StarPoint starPoint)
		{
			var ret = 0;
			if (starPoint == NodePoint1) ret = 1;
			if (starPoint == NodePoint2) ret = -1;
			return ret;
		}
	}
}
