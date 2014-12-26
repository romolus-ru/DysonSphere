using System;
using Engine.Controllers;

namespace Engine.Views
{
	/// <summary>
	/// Интерфейс объекта визуализации, для унификации
	/// </summary>
	interface IViewObject:IEngineObject
	{
		/// <summary>
		/// Ссылка на контроллер
		/// </summary>
		Controller Controller { get; }

		void Draw(VisualizationProvider visualizationProvider);

		/// <summary>
		/// Инициализация визуализации. В частности получение размеров экрана
		/// </summary>
		/// <param name="visualizationProvider">Визуализиация, определяет многие параметры визуализации</param>
		void Init(VisualizationProvider visualizationProvider);

		String Name { get;}

		/// <summary>
		/// Выводить ли этот объект на экран
		/// </summary>
		Boolean CanDraw { get;}

		/// <summary>
		/// Скрыть объект
		/// </summary>
		void Hide();

		/// <summary>
		/// Показать объект
		/// </summary>
		void Show();

		/// <summary>
		/// Координата X объекта
		/// </summary>
		int X {get;}

		/// <summary>
		/// Координата Y объекта
		/// </summary>
		int Y { get;}

		/// <summary>
		/// Координата Z объекта
		/// </summary>
		int Z { get;}

		/// <summary>
		/// Установить координаты объекта
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		void SetCoordinates(int x, int y, int z);

		/// <summary>
		/// Установить относительные координаты объекта
		/// </summary>
		/// <param name="rx"></param>
		/// <param name="ry"></param>
		/// <param name="rz"></param>
		void SetCoordinatesRelative(int rx, int ry, int rz);

		/// <summary>
		/// Высота объекта
		/// </summary>
		int Height { get;}

		/// <summary>
		/// Ширина объекта
		/// </summary>
		int Width { get;}

		/// <summary>
		/// Установить размеры объекта
		/// </summary>
		/// <param name="width">Ширина</param>
		/// <param name="height">Высота</param>
		void SetSize(int width, int height);


	}
}
