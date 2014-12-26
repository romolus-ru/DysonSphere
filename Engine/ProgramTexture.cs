using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Engine
{
	/// <summary>
	/// Программная текстура
	/// </summary>
	public class ProgramTexture
	{
		/// <summary>
		/// Конструктор по умолчанию. без него не создастся в коллекторе
		/// </summary>
		public ProgramTexture() { }

		/// <summary>
		/// Меняем текстуру
		/// </summary>
		/// <param name="p">указатель на текстуру(или bitmap.scan0)</param>
		/// <param name="width">ширина</param>
		/// <param name="height">высота</param>
		/// <param name="bitspp">битов на пиксель</param>
		/// <param name="colorFrom">Цвет, который надо заменить</param>
		/// <param name="colorTo">Цвет, которым заменяем</param>
		public virtual void Modify(IntPtr p, int width, int height, int bitspp, Color colorFrom, Color colorTo)
		{

		}
	}
}