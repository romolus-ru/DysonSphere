using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTubes.Controllers
{
	/// <summary>
	/// Обмен точками
	/// </summary>
	public class FieldPointExchange
	{
		/// <summary>
		/// Текстура
		/// </summary>
		public int TextureNum;
		public int X;
		public int Y;
		/// <summary>
		/// Виден ли блок - может быть поле будет создаваться целиком и поэтому нужны будут все ячейки
		/// </summary>
		public Boolean IsVisible=false;
		/// <summary>
		/// текущий угол на который повернута фигура
		/// </summary>
		public int CurrentAngle;

		public FieldPointExchange() { }// безпараметрический конструктор, для сериализации
		public FieldPointExchange(int textureNum, int x, int y, Boolean isVisible, int currentAngle)
		{
			X = x;
			Y = y;
			TextureNum = textureNum;
			IsVisible = isVisible;
			CurrentAngle = currentAngle;
		}

		public static FieldPointExchange Create(int num, int x, int y, Boolean isVisible, int currentAngle)
		{
			return new FieldPointExchange(num, x, y, isVisible, currentAngle);
		}


	}
}
