using System;
using System.Collections.Generic;
using Engine.Utils.Editor;

namespace SimpleMapEditor
{
	/// <summary>
	/// Пример простого редактируемого объекта
	/// </summary>
	class SimpleEditableObject:IDataHolder
	{
		public int X;
		public int Y;
		public int TextureNum;
		
		public int Num { get; set; }
		
		public Dictionary<string, string> Save()
		{
			var d = new Dictionary<String, String>();
			d.Add("X", X.ToString());
			d.Add("Y", Y.ToString());
			d.Add("TextureNum", TextureNum.ToString());
			return d;
		}

		public void Load(Dictionary<string, string> data)
		{
			X = Convert.ToInt32(data["X"]);
			Y = Convert.ToInt32(data["Y"]);
			TextureNum = Convert.ToInt32(data["TextureNum"]);
		}
	}
}
