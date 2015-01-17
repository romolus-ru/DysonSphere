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
		/// <summary>Тип объекта</summary>
		public ObjectTypes ObjType;
		/// <summary>Тип объекта, который будет отображаться на экране, нужен только для некоторых типов</summary>
		public ObjectTypes ObjTypeView;
		/// <summary>скорость движения и подобное</summary>
		public int Int1;
		/// <summary>Начальное временное смещение и подобное</summary>
		public int Int2;
		/// <summary>Направление движения и т.п.</summary>
		public int Int3;
		/// <summary>Длина пути и подобное</summary>
		public int Int4;
		public Boolean Boolean;

		
		public int Num { get; set; }
		
		public Dictionary<string, string> Save()
		{
			var d = new Dictionary<String, String>();
			d.Add("X", X.ToString());
			d.Add("Y", Y.ToString());
			d.Add("ObjType", ObjType.ToString());
			// если визуальный тип отличается и если визуальный тип не равен пустому значению, то записываем его
			if ((ObjType != ObjTypeView) && (ObjTypeView != ObjectTypes.Empty))
			{
				d.Add("ObjTypeView", ObjTypeView.ToString());
			}
			if (Int1 != 0) { d.Add("Int1", Int1.ToString()); }
			if (Int2 != 0) { d.Add("Int2", Int2.ToString()); }
			if (Int3 != 0) { d.Add("Int3", Int3.ToString()); }
			if (Int4 != 0) { d.Add("Int4", Int4.ToString()); }
			if (!Boolean) { d.Add("Boolean", Boolean.ToString()); }
			return d;
		}

		public void Load(Dictionary<string, string> data)
		{
			string s;
			X = Convert.ToInt32(data["X"]);
			Y = Convert.ToInt32(data["Y"]);

			ObjType = ObjectTypes.Wall2;
			s = data["ObjType"];
			Enum.TryParse(s, out ObjType);

			ObjTypeView = ObjectTypes.Empty;
			if (data.ContainsKey("ObjTypeView")){
				s = data["ObjTypeView"];
				Enum.TryParse(s, out ObjType);
			}
			if (data.ContainsKey("Int1")){Int1 = Convert.ToInt32(data["Int1"]);}else Int1 = 0;
			if (data.ContainsKey("Int2")){Int2 = Convert.ToInt32(data["Int2"]);}else Int2 = 0;
			if (data.ContainsKey("Int3")){Int3 = Convert.ToInt32(data["Int3"]);}else Int3 = 0;
			if (data.ContainsKey("Int4")){Int4 = Convert.ToInt32(data["Int4"]);}else Int4 = 0;
			if (data.ContainsKey("Boolean")) {
				Boolean = Convert.ToBoolean(data["Boolean"]);
			}else Boolean = true;
		}
	}
}
