using System;
using Engine.Models;

namespace Engine.Controllers.Events
{
	public class ModelObjectEventArgs : EventArgs
	{
		public IModelObject ModelObject { get; private set; }
		public static ModelObjectEventArgs Create(IModelObject modelObj)
		{
			var r = new ModelObjectEventArgs();
			r.ModelObject = modelObj;
			return r;
		}
	}
}
