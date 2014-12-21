using System;
using System.Drawing;

namespace Engine.Controllers.Events
{
	/// <summary>
	/// Передача координаты
	/// </summary>
	public class PointEventArgs : EventArgs
	{
		public Point Pt { get; protected set; }

		/// <summary>
		/// Установить координаты курсора. Делает открытым координаты
		/// </summary>
		/// <param name="pt"></param>
		/// <remarks>Надеюсь, временная мера. Если не временная то надо переопределить этот класс и открыть всё</remarks>
		public void SetCoord(Point pt)
		{
			Pt = pt;
		}

		/// <summary>
		/// Установить координаты курсора. Делает открытым координаты
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <remarks>Надеюсь, временная мера. Если не временная то надо переопределить этот класс и открыть всё</remarks>
		public void SetCoord(int x, int y)
		{
			SetCoord(new Point(x, y));
		}

		/// <summary>
		/// Создание аргументов по переданным координатам
		/// </summary>
		/// <param name="x">Координата X</param>
		/// <param name="y">Координата Y</param>
		/// <returns></returns>
		public static PointEventArgs Set(int x, int y)
		{
			Point p = new Point(x, y);
			return Set(p);
		}

		/// <summary>
		/// Создание аргументов по переданной точке
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public static PointEventArgs Set(Point point)
		{
			PointEventArgs pea = new PointEventArgs();
			pea.Pt = point;
			return pea;
		}

		/// <summary>
		/// Создание аргументов с нулевыми координатами или координатами по умолчанию
		/// </summary>
		/// <returns></returns>
		public static PointEventArgs Set()
		{
			return Set(0, 0);
		}


	}
}
