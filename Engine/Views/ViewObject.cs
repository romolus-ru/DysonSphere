using System;
using System.Collections.Generic;
using System.Drawing;
using Engine.Controllers;

namespace Engine.Views
{
	/// <summary>
	/// Объект визуализации. основной класс
	/// </summary>
	/// <remarks>Прописывает себя в </remarks>
	public class ViewObject:IViewObject// интерфейс потом надо добавить и переделать
	{
		/// <summary>
		/// Ссылка на контроллер
		/// </summary>
		public Controller Controller { get; private set; }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller">Контроллер</param>
		public ViewObject(Controller controller)
		{
			// работа с событиями, создание и подключение
			Controller = controller;
			CanDraw = true;
		}

		// TODO создать объект движка в котором будут интегрированы эти и возможно другие интерфейсы что бы точно было единообразно
		// и заодно будет видно как будет вести себя наследование интерфейсов и наследование классов
		private Boolean HandlersAdded = false;
		private Boolean HandlersRemoved = false;

		/// <summary>
		/// Добавить обработчики
		/// </summary>
		/// <remarks>Можно вызывать много раз - добавится всё равно только 1</remarks>
		public void HandlersAddThis()
		{
			if (HandlersAdded) return;// проверка
			HandlersAdder();// добавляем
			HandlersAdded = true;
			HandlersRemoved = false;// разрешаем удалить
		}

		/// <summary>
		/// Удалить обработчики
		/// </summary>
		/// <remarks>Можно вызывать много раз - удалится всё равно только 1</remarks>
		public void HandlersRemoveThis()
		{
			if (HandlersRemoved) return;//проверка
			HandlersRemover();//удаляем
			HandlersRemoved = true;
			HandlersAdded = false;//разрешаем добавить
		}
		/// <summary>
		/// Добавить обработчики
		/// </summary>
		/// <remarks>Предназначения для переопределения пользователем</remarks>
		protected virtual void HandlersAdder()
		{
		}
	
		/// <summary>
		/// Удалить обработчики
		/// </summary>
		/// <remarks>Предназначена для переопределения пользователем</remarks>
		protected virtual void HandlersRemover()
		{
		}

		/// <summary>
		/// Прорисовка объекта для текстуры
		/// </summary>
		/// <param name="visualizationProvider">Объект-визуализатор</param>
		public void DrawToTexture(VisualizationProvider visualizationProvider)
		{
			if (CanDraw)
			{
				DrawObjectToTexture(visualizationProvider);
			}
		}


		/// <summary>
		/// Прорисовка объекта
		/// </summary>
		/// <param name="visualizationProvider">Объект-визуализатор</param>
		public void Draw(VisualizationProvider visualizationProvider)
		{
			// видимость слоя проверяется когда всё выводится. и там 1 раз проверяется слой прежде чем выводить его составляющие
			// проверяем должен ли объект выводиться
			if (CanDraw){
				DrawObject(visualizationProvider);
			}
		}

		/// <summary>
		/// Инифиализация с учётом текущих настроек визуализации
		/// </summary>
		/// <param name="visualizationProvider"></param>
		public virtual void Init(VisualizationProvider visualizationProvider)
		{
			HandlersAddThis();
			Show();// по умолчанию объект показываем
		}

		/// <summary>
		/// Прорисовка объекта для текстуры. Без проверки на необходимость вывода на экран
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void DrawObjectToTexture(VisualizationProvider visualizationProvider)
		{

		}

		/// <summary>
		/// Прорисовка объекта. Без проверки на необходимость вывода на экран
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void DrawObject(VisualizationProvider visualizationProvider)
		{

		}

		/// <summary>
		/// Имя объекта
		/// </summary>
		public String Name { get; set; }

		#region Управление видимостью

		/// <summary>
		/// Выводить ли этот объект на экран
		/// </summary>
		public Boolean CanDraw { get; protected set; }

		/// <summary>
		/// Скрыть объект
		/// </summary>
		public virtual void Hide()
		{
			CanDraw = false;
		}

		/// <summary>
		/// Показать объект
		/// </summary>
		public virtual void Show()
		{
			CanDraw = true;
		}

		#endregion

		#region Координаты объекта

		/// <summary>
		/// Координата X объекта
		/// </summary>
		public int X {get; protected set;}

		/// <summary>
		/// Координата Y объекта
		/// </summary>
		public int Y { get; protected set; }

		/// <summary>
		/// Координата Z объекта
		/// </summary>
		public int Z { get; protected set; }
		
		/// <summary>
		/// Установить координаты объекта
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void SetCoordinates(int x, int y, int z=0)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Установить относительные координаты объекта
		/// </summary>
		/// <param name="rx"></param>
		/// <param name="ry"></param>
		/// <param name="rz"></param>
		public void SetCoordinatesRelative(int rx, int ry, int rz)
		{
			X += rx;
			Y += ry;
			Z += rz;
		}

		#endregion

		#region Углы поворота объекта

		/// <summary>
		/// Углы поворота
		/// </summary>
		protected int Ax, Ay, Az;

		/// <summary>
		/// Установить углы поворота
		/// </summary>
		/// <param name="ax"></param>
		/// <param name="ay"></param>
		/// <param name="az"></param>
		public void SetAngles(int ax, int ay, int az)
		{
			Ax = ax;
			Ay = ay;
			Az = az;
		}

		#endregion

		#region Строки и цвета

		/// <summary>
		/// Строки для отображения на экране
		/// </summary>
		private readonly List<String> _strings = new List<string>();

		private readonly List<Color> _colors = new List<Color>();

		/// <summary>
		/// Добавить строку
		/// </summary>
		/// <param name="s"></param>
		public void StringsAdd(String s)
		{
			_strings.Add(s);
		}

		/// <summary>
		/// Добавить строку с цветом
		/// </summary>
		public void StringsAddWithColor(Color color, String s)
		{
			StringsAdd(s);
			ColorsAdd(color);
		}

		/// <summary>
		/// Очистить список строк
		/// </summary>
		public void StringsClear()
		{
			_strings.Clear();
		}

		/// <summary>
		/// очистить список строк
		/// </summary>
		public void ColorsClear()
		{
			_colors.Clear();
		}

		public void ColorsAdd(Color color)
		{
			_colors.Add(color);
		}

		/// <summary>
		/// Установить цвет
		/// </summary>
		/// <param name="color"></param>
		/// <param name="num"></param>
		public void SetColor(Color color, int num = 0)
		{
			_colors[num] = color;
		}

		#endregion

		#region Текстуры

		/// <summary>
		/// список текстур
		/// </summary>
		private readonly List<String> _texturesNames = new List<string>();

		/// <summary>
		/// Добавить имя тектуры
		/// </summary>
		/// <param name="texnureName"></param>
		public void TexturesAdd(String texnureName)
		{
			_texturesNames.Add(texnureName);
		}

		/// <summary>
		/// Очистить список текстур
		/// </summary>
		public void TexturesClear()
		{
			_texturesNames.Clear();
		}

		#endregion

		#region Размеры объекта

		/// <summary>
		/// Высота объекта
		/// </summary>
		public int Height { get; protected set; }

		/// <summary>
		/// Ширина объекта
		/// </summary>
		public int Width { get; protected set; }

		/// <summary>
		/// Установить размеры объекта
		/// </summary>
		/// <param name="width">Ширина</param>
		/// <param name="height">Высота</param>
		public void SetSize(int width, int height)
		{
			Height = height;
			Width = width;
		}

		#endregion

		/// <summary>
		/// Для блокировки дополнительных вызовов dispose
		/// </summary>
		private Boolean _disposed = false;

		/// <summary>
		/// Проверяем, находятся ли переданные координаты внутри объекта
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public virtual Boolean InRange(int x, int y)
		{
			if (!CanDraw) return false;// компонент не рисуется - значит не проверяем дальше
			if ((X < x) && (x < X + Width))
			{
				if ((Y < y) && (y < Y + Height))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Удаление, можно дополнить у потомков
		/// </summary>
		public virtual void Dispose()
		{
			if (!_disposed)
			{
				HandlersRemoveThis();
				Controller = null;
				_disposed = true;
			}
		}

		/// <summary>
		/// Деструктор
		/// </summary>
		~ViewObject()
		{
			Dispose();
		}
	}
}
