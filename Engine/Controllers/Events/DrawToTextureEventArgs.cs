using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Views;

namespace Engine.Controllers.Events
{
	class DrawToTextureEventArgs:EngineEventArgs
	{
		public String TextureName;
		public IViewObject ViewObject;

		public static DrawToTextureEventArgs Set(IViewObject viewObject, string textureName)
		{
			var r = new DrawToTextureEventArgs();
			r.ViewObject = viewObject;
			r.TextureName = textureName;
			return r;
		}
	}
}
