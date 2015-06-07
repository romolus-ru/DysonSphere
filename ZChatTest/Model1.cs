using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Models;
using Engine.Utils.ExtensionMethods;

namespace ZChatTest
{
	class Model1:ModelObject
	{
		private int angle;
		private int freq1;
		private int freq2;

		public Model1(Controller controller) : base(controller)
		{
			angle = 0;
			freq1 = 0;
			freq2 = 0;
		}

		public override void Execute()
		{
			base.Execute();

			freq1++;
			if (freq1 > 4){
				freq1 = 0;
				angle++;
				if (angle > 360) angle = 0;
				freq2++;
				if (freq2 > 4){
					freq2 = 0;
					Controller.SendToViewCommand("AngleFromServer", IntegerEventArgs.Send(angle));
				}
			}



		}
	}
}
