using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMBuildCraft
{
	/// <summary>
	/// Описание одного ресурса
	/// </summary>
	class ResourcesDescription
	{
		public ResourceEnum ResourceEnum;
		public String Name;
		public String Description;
		public String TexName;
		public String TexAddress;
		public Color Color;
		/// <summary>
		/// Сложность добычи ресурса
		/// </summary>
		public int Complexity;
		/// <summary>
		/// Сложность выработки после добычи всех официальных запасов
		/// </summary>
		public int OverComplexity;
	}
}
