using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTubes.View
{
	/// <summary>
	/// Одна точка поля
	/// </summary>
	class ViewFieldPoint
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
		/// угол вращения от взаимодействия с игроком
		/// </summary>
		public int RotateAngle;
		/// <summary>
		/// текущий угол на который повернута фигура
		/// </summary>
		public int CurrentAngle;

		private float _addToRotate = 0;

		public ViewFieldPoint(int textureNum, int x, int y,Boolean isVisible, int currentAngle)
		{
			X = x;
			Y = y;
			TextureNum = textureNum;
			IsVisible = isVisible;
			RotateAngle = 0;
			CurrentAngle = currentAngle;
		}

		public void Rotate()
		{
			_addToRotate -= 90;// поворачиваем назад на 90 градусов, что бы фигура "повернулась" на 90 градусов
		}

		public int AddRotate()
		{
			if (_addToRotate == 0) return 0;
			var n = _addToRotate / 18;
			if (_addToRotate < -10) n *= 3;
			_addToRotate -= n;
			if (_addToRotate > -2) _addToRotate = 0;
			return (int)_addToRotate;
		}


		public static ViewFieldPoint Create(int num, int x, int y,Boolean isVisible, int currentAngle)
		{
			return new ViewFieldPoint(num, x, y, isVisible, currentAngle);
		}

	}
}
