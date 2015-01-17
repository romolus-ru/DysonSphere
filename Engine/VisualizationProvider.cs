using System;
using System.Collections.Generic;
using System.Drawing;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;
using Engine.Utils.Settings;
using Engine.Views;

namespace Engine
{
	// TODO Попробовать переделать под interface
	// но основной класс нужно оставить - и для defaultVisualization подойдёт, да и для уже сделанных

	/// <summary>
	/// Базовый класс для различных видов визуализации
	/// </summary>
	/// <remarks>Умеет только рисовать. вспомогательный класс для <see cref="View"/></remarks>
	public class VisualizationProvider
	{

		protected Controller _controller;

		public Version Version = new Version(0, 0);

		/// <summary>
		/// Ширина окна визуализации
		/// </summary>
		public int CanvasWidth { get; protected set; }

		/// <summary>
		/// Высота окна визуализации
		/// </summary>
		public int CanvasHeight { get; protected set; }

		/// <summary>
		/// Конструктор без параметров
		/// </summary>
		/// <remarks>Ничего не должен содержать, всё передаётся при инициализации</remarks>
		public VisualizationProvider()
		{
			Version = new Version(1, 0);
		}

		/// <summary>
		/// Инициализация визуализации. Создание формы и установка нужных размеров
		/// </summary>
		/// <remarks>
		/// Возможно, что тут нужно передать TView
		/// Метод сам по себе не вызывается, нужно вызывать метод родителя принудительно
		/// </remarks>
		public virtual void InitVisualization(Controller controller)
		{
			_controller = controller;
			// пока получается что каждый вид визуализации должен сам преобразовывать координаты из экранных
			_controller.AddEventHandler("VisualizationCursorGet", (o, args) => CursorCoordinates(o, args as PointEventArgs));
			_controller.AddEventHandler("LoadTexture", LoadTextureEH);

			if (_controller != null){
				//if (Settings != null)
				{
					Settings.EngineSettings.GetValue("","Window");
					//CanvasHeight = Settings.GetValueInt("Height");
					if (CanvasHeight == 0) CanvasHeight = 1024;		// 0 значит что в настройках не записано значение
					//CanvasWidth = settings.GetValueInt("Width");
					if (CanvasWidth == 0) CanvasWidth = 768;		// 0 значит что в настройках не записано значение
				}
			}else{
				CanvasHeight = 800;
				CanvasWidth = 600;
			}
			InitVisualization2();
		}

		/// <summary>
		/// Продолжение инициализации, переопределяемое потомками
		/// </summary>
		protected virtual void InitVisualization2()
		{

		}

		/// <summary>
		/// Получаем координаты курсора из других источников
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Заменяется в потомках. Переопределяет координаты курсора из экранных в клиентские</remarks>
		protected virtual void CursorCoordinates(object sender, PointEventArgs e)
		{
			// тут ничего не делаем. в потомках  надо переопределить 
		}


		#region Установка цвета

		/// <summary>
		/// Текущий цвет
		/// </summary>
		public System.Drawing.Color Color { get; private set; }

		/// <summary>
		/// Фоновый цвет
		/// </summary>
		public System.Drawing.Color BackgroundColor { get; private set; }

		/// <summary>
		/// Установить цвет
		/// </summary>
		/// <param name="color"></param>
		public void SetColor(Color color)
		{
			Color = color;
			SetColor(Color.R, Color.G, Color.B, Color.A);
		}

		/// <summary>
		/// Установить цвет c прозрачностью
		/// </summary>
		/// <param name="color"></param>
		/// <param name="alphaPercent"></param>
		public void SetColor(Color color, byte alphaPercent)
		{
			if (alphaPercent > 100){alphaPercent = 100;}// обрезаем до 100
			Color = Color.FromArgb(255*alphaPercent/100, color);// растягиваем до 255
			SetColor(Color.R, Color.G, Color.B, Color.A);
		}

		public virtual void SetColor(int r, int g, int b, int a)
		{

		}

		/// <summary>
		/// Установить фоновый цвет
		/// </summary>
		/// <param name="color"></param>
		public void SetBackgroundColor(Color color)
		{
			BackgroundColor = color;
			SetBackgroundColor(color.R, color.G, color.B, color.A);
		}

		/// <summary>
		/// Установить фоновый цвет c прозрачностью
		/// </summary>
		/// <param name="color"></param>
		/// <param name="alpha"></param>
		public void SetBackgroundColor(Color color, byte alpha)
		{
			BackgroundColor = Color.FromArgb(alpha, color);
			SetBackgroundColor(color.R, color.G, color.B, color.A);
		}

		public virtual void SetBackgroundColor(int r, int g, int b, int a)
		{

		}

		#endregion

		#region Линия, прямоугольник и круг

		/// <summary>
		/// Рисовать линию
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		public void Line(int x1, int y1, int x2, int y2)
		{
			_Line(x1 + curOffsetX, y1 + curOffsetY, x2 + curOffsetX, y2 + curOffsetY);
		}

		protected virtual void _Line(int x1, int y1, int x2, int y2) { }

		/// <summary>
		/// Нарисовать прямоугольник
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Rectangle(int x, int y, int width, int height)
		{
			//_Rectangle(x + curOffsetX, y + curOffsetY, width, height);
			// TODO раньше был curOffset, но без него работает лучше
			_Rectangle(x, y, width, height);
		}

		protected virtual void _Rectangle(int x, int y, int width, int height)
		{

		}

		/// <summary>
		/// Нарисовать закрашенный прямоугольник
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Box(int x, int y, int width, int height)
		{_Box(x + curOffsetX, y + curOffsetY, width, height);}

		protected virtual void _Box(int x, int y, int width, int height)
		{
			
		}

		/// <summary>
		/// Нарисовать круг
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="radius"></param>
		public virtual void Circle(int x, int y, int radius)
		{_Circle(x + curOffsetX, y + curOffsetY, radius);}

		protected virtual void _Circle(int x, int y, int radius) { }

		#endregion

		/// <summary>
		/// Запустить объект визуализации (окно) в модальном режиме
		/// </summary>
		public virtual void Run() { }

		/// <summary>
		/// Загрузить текстуру из файла и модифицировать её
		/// </summary>
		/// <param name="textureName">Имя текстуры</param>
		/// <param name="fileName">Имя файла</param>
		/// <param name="prog"></param>
		/// <param name="colorFrom"></param>
		/// <param name="colorTo"></param>
		/// <returns></returns>
		/// <remarks>Так же нужна процедура, которая делает прозрачную процедуру. Грузит текстуру из файла и вычисляя серый цвет пишет его в альфу</remarks>
		public virtual Boolean LoadTextureModify(string textureName, string fileName, ProgramTexture prog, Color colorFrom, Color colorTo)
		{ return false; }

		/// <summary>
		/// Загрузка текстуры и создание в ней альфа-канала на основе серого цвета(сумма RGBсоставляющих цвета каждой отдельной точки)
		/// </summary>
		/// <param name="textureName">Имя текстуры</param>
		/// <param name="fileName">Имя файла</param>
		/// <param name="mode">режим. для будущего использования, чтоб можно было грузить альфу из других файлов (например *Alpha.jpg)</param>
		/// <returns></returns>
		public virtual Boolean LoadTextureAlpha(string textureName, string fileName, int mode = 0)
		{ return false; }

		/// <summary>
		/// Обработчик события загрузки текстуры
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>имя_текстуры запятая имя файла</remarks>
		private void LoadTextureEH(object sender, EventArgs e)
		{
			var e1 = e as MessageEventArgs;
			if (e1 != null)
			{
				var s = e1.Message.Split(',');
				if (s.Length > 1)
				{
					LoadTexture(s[0], s[1]);
				}
			}
		}

		/// <summary>
		/// Загрузить текстуру из файла. Фактически текстуры грузится 24х битная, без альфа-канала
		/// </summary>
		/// <param name="textureName">Имя текстуры</param>
		/// <param name="fileName">Имя файла</param>
		/// <returns></returns>
		public virtual Boolean LoadTexture(string textureName, string fileName)
		{ return false; }

		/// <summary>
		/// Вывести на экран текстуру
		/// </summary>
		/// <param name="x">Координата Х</param>
		/// <param name="y">Координата У</param>
		/// <param name="textureName">Имя текстуры</param>
		/// <param name="scale">Увеличение размера текстуры</param>
		public void DrawTexture(int x, int y, String textureName, float scale = 1)
		{_DrawTexture(x + curOffsetX, y + curOffsetY, textureName, scale);}
		
		protected virtual void _DrawTexture(int x, int y, String textureName, float scale = 1) { }

		/// <summary>
		/// Вывести на экран часть разбитой на блоки текстуры
		/// </summary>
		/// <param name="x">Координата X</param>
		/// <param name="y">Координата Y</param>
		/// <param name="textureName">Имя текстуры</param>
		/// <param name="blockWidth">Ширина одного блока в текстуре</param>
		/// <param name="blockHeight">Высота одного блока в текстуре</param>
		/// <param name="num">Номер с нуля блока, выводимого на экран</param>
		public void DrawTexturePart(int x, int y, String textureName, int blockWidth, int blockHeight, int num)
		{_DrawTexturePart(x + curOffsetX, y + curOffsetY, textureName, blockWidth, blockHeight, num);}

		protected virtual void _DrawTexturePart(int x, int y, String textureName, int blockWidth, int blockHeight, int num) { }


		/// <summary>
		/// Вывести на экран произвольную часть текстуры, например в качестве прогрессбара
		/// </summary>
		/// <param name="x">Координата вывода на экран</param>
		/// <param name="y"></param>
		/// <param name="textureName">Имя текстуры</param>
		/// <param name="xtex">Координата на текстуре откуда выводить</param>
		/// <param name="ytex">Координата на текстуре откуда выводить</param>
		/// <param name="width">Ширина выводимой области</param>
		/// <param name="height">Высота выводимой области</param>
		public void DrawTexturePart(int x, int y, String textureName, int xtex, int ytex, int width, int height)
		{_DrawTexturePart(x,y,textureName, xtex, ytex, width, height);}

		protected virtual void _DrawTexturePart(int x, int y, String textureName, int xtex, int ytex, int width, int height) { }

		/// <summary>
		/// Вывести на экран текстуру с маской (почти аналог LoadTextureAlpha)
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="textureName"></param>
		/// <param name="textureMaskName"></param>
		public void DrawTextureMasked(int x, int y, String textureName, String textureMaskName)
		{_DrawTextureMasked(x + curOffsetX, y + curOffsetY, textureName, textureMaskName);}

		protected virtual void _DrawTextureMasked(int x, int y, String textureName, String textureMaskName) { }

		///// <summary>
		///// Вывести на экран повёрнутую текстуру
		///// </summary>
		///// <param name="x"></param>
		///// <param name="y"></param>
		///// <param name="textureName"></param>
		//public virtual void DrawTextureRotated(int x, int y, String textureName){}

		#region Операции с текстом

		/// <summary>
		/// Имя текстуры
		/// </summary>
		/// <remarks>Вне зависимости от способа загрузки шрифта должна появиться текстура
		/// Хотя может и не появиться текстура, тогда будет выводиться без неё
		/// (зависит от конкретной реализации объекта визуализации)</remarks>
		protected String FontTexture = "";

		/// <summary>
		/// Высота шрифта. по умолчанию = 16
		/// </summary>
		protected int FontHeight = 16;

		/// <summary>
		/// Загрузить шрифт по имени 
		/// </summary>
		/// <param name="fontName">Системное имя шрифта</param>
		/// <param name="fontHeight">Высота шрифта</param>
		public virtual void LoadFont(String fontName, int fontHeight = 12) { }

		/// <summary>
		/// Загрузить текстуру-шрифт
		/// </summary>
		/// <param name="textureName"></param>
		public virtual void LoadFontTexture(String textureName) { }

		/// <summary>
		/// Вычисление длины текста
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public virtual int TextLength(String text) { return 0; }

		/// <summary>
		/// Координата текста Х
		/// </summary>
		public int CurTxtX { get; protected set; }

		/// <summary>
		/// Координата текста У
		/// </summary>
		public int CurTxtY { get; protected set; }

		/// <summary>
		/// Вывод текста на экран в координатах ХУ
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		public void Print(int x, int y, String text)
		{
			CurTxtX = x;
			CurTxtY = y;
			Print(text);
		}

		/// <summary>
		/// Вывод текста на экран в текущих координатах
		/// </summary>
		/// <param name="text"></param>
		public void Print(String text)
		{
			PrintOnly(CurTxtX+curOffsetX, CurTxtY+curOffsetY, text);
			CurTxtX += TextLength(text);
		}

		/// <summary>
		/// Именно эта функция и  выводит текст
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		/// <remarks>
		/// Переопределено должно быть в потомках
		/// выводит текст в указанных координатах
		/// цвет уже установлен в других функциях, тут это не должно требоваться</remarks>
		protected virtual void PrintOnly(int x, int y, String text) { }

		#endregion

		/// <summary>
		/// Подготовка к началу рисования
		/// </summary>
		public virtual void BeginDraw() { }

		/// <summary>
		/// Окончательный вывод на экран, наподобие swapBuffers
		/// </summary>
		public virtual void FlushDrawing() { }

		/// <summary>
		/// Повернуть
		/// </summary>
		/// <param name="angle"></param>
		public virtual void Rotate(int angle) { }

		/// <summary>
		/// Восстановить повернутое
		/// </summary>
		public virtual void RotateReset() { }

		/// <summary>
		/// Смещения для прорисовки компонентов
		/// </summary>
		private Stack<int> offsets=new Stack<int>();

		public int curOffsetX = 0;
		public int curOffsetY = 0;

		/// <summary>
		/// Добавить смещение
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		public void OffsetAdd(int dx, int dy)
		{
			offsets.Push(curOffsetX);
			offsets.Push(curOffsetY);
			curOffsetX = dx;
			curOffsetY = dy;
		}

		/// <summary>
		/// Удалить смещение, восстановив предыдущее состояние
		/// </summary>
		public void OffsetRemove()
		{
			curOffsetY = offsets.Pop();
			curOffsetX = offsets.Pop();
		}

	}
}
