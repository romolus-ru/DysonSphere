using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;

namespace GMTubes.Controllers
{
	public class FieldInfoEventArgs:EngineEventArgs
	{
		public List<FieldPointExchange> Points = new List<FieldPointExchange>();
		public int FieldWidth;
		public int FieldHeight;
		public int FieldTimeGold;
		public int FieldTimeSilver;

		public void Add(int textureNum, int x, int y, Boolean isVisible, int currentAngle)
		{
			var a = FieldPointExchange.Create(textureNum, x, y, isVisible, currentAngle);
			Points.Add(a);
		}

		public static FieldInfoEventArgs Create()
		{
			return new FieldInfoEventArgs();
		}
	}
}
