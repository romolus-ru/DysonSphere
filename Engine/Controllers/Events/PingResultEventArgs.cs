using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Controllers.Events
{
	class PingResultEventArgs:EngineEventArgs
	{
		public Boolean Ok = false;
		public PingReply Reply;
		public IPStatus Status;

		public static PingResultEventArgs Set(PingReply reply, IPStatus status)
		{
			var r = new PingResultEventArgs();
			r.Reply = reply;
			r.Status = status;
			if (status == IPStatus.Success) r.Ok = true;
			return r;
		}
	}
}
