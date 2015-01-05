using System;
using System.Drawing;
using System.Windows.Forms;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine
{
	/// <summary>
	/// Класс для получения координат курсора и нажатых клавиш
	/// </summary>
	/// <remarks>
	/// Предоставляет возможность узнать координаты курсора и нажатых клавиш
	/// Сложный как оказалось класс. Использует VisualizationProvider для 
	/// получения координат мыши и вращения колёсика
	/// </remarks> 
	public class Input
	{

		/// <summary>
		/// Сохраняем для посылки сообщения о нажатии клавиш или перемещении мышки
		/// </summary>
		protected Controller _controller;

		/// <summary>
		/// Координата курсора X. Клиентская
		/// </summary>
		public int cursorX { get; protected set; }

		/// <summary>
		/// Координата курсора Y. Клиентская
		/// </summary>
		public int cursorY { get; protected set; }

		// координаты курсора полученные от системы
		protected int curOldX = 0;// для сравнения для метода SetCursor
		protected int curOldY = 0;// для сравнения для метода SetCursor

		/// <summary>
		/// Смещение колеса мыши
		/// </summary>
		public int cursorDelta { get; protected set; }

		private int cursorDeltaState = 0;
		/// <summary>
		/// флаг очистки событий клавиатуры. работает некоторое время, потом отменяется
		/// </summary>
		public bool keyboardCleared { get; protected set; }

		/// <summary>
		/// Для определения отпускания кнопки. кнопки не нажаты, но перед этим были нажаты и генерируется событие, что что то нажато
		/// </summary>
		/// <remarks></remarks>
		private Boolean _lastKeyPressed = false;

		public Input() { }

		/// <summary>
		/// Инициализация, сохранение ссылки на контроллер
		/// </summary>
		/// <param name="controller"></param>
		public void Init(Controller controller)
		{
			_controller = controller;
			// только этот обработчик. Остальные - передача состояния клавиатуры и мыши - передаётся автоматически
			_controller.AddEventHandler("CursorGet", (o, args) => CursorGet(o, args as PointEventArgs));
			_controller.AddEventHandler("CursorDelta", (o, args) => CursorDelta(o, args as IntegerEventArgs));
			_controller.AddEventHandler("KeyboardClearEnd", KeyboardClearEnd);
			_controller.AddEventHandler("KeyboardClear", KeyboardClear);
		}

		/// <summary>
		/// Завершение блокировки клавиатуры
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void KeyboardClearEnd(object sender, EventArgs e)
		{
			keyboardCleared = false;
		}

		/// <summary>
		/// Заблокировать на небольшое время события от клавиатуры
		/// </summary>
		/// <remarks>Нужно часто при нажатии на кнопки мыши, что бы событие дальше не распространялось некоторое время</remarks>
		public void KeyboardClear(object sender, EventArgs e)
		{
			keyboardCleared = true;
			_controller.AddToStore(this, StoredEventEventArgs.StoredMilliseconds(100, "KeyboardClearEnd", this, EventArgs.Empty));
		}

		/// <summary>
		/// Получаем вращение колеса мыши
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="integerEventArgs"></param>
		private void CursorDelta(object sender, IntegerEventArgs integerEventArgs)
		{
			// сохраняем значение вращения колеса
			cursorDelta = integerEventArgs.I;
			cursorDeltaState = 1;
		}

		/// <summary>
		/// Передаём координаты курсора по запросу
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="pointEventArgs"></param>
		private void CursorGet(object sender, PointEventArgs pointEventArgs)
		{
			// меняем точку на текущую, независимо от того что передали
			pointEventArgs.SetCoord(new Point(cursorX, cursorY));
		}

		/// <summary>
		/// получить информацию о клавишах и мышке
		/// </summary>
		public virtual void GetInput()
		{
			var keyNew = SetKeyboard();// устанавливаем состояния устройств ввода
			if (!keyNew){// если кнопка не нажата
				if (!_lastKeyPressed){// если до этого что то нажимали
					_lastKeyPressed = true;// запоминаем что теперь это было последнее нажатие, пользователь отпустил все кнопки
					keyNew = true;// устанавливаем флаг принудительно, что бы отправить событие без нажатий
				}
			}
			else{_lastKeyPressed = false;}// фиксируем что пользователь ещё нажимает какую то кнопку

			var curNew = SetCursor();

			if (cursorDeltaState != 0)
			{
				if (cursorDeltaState == 2){// состояние 2
					cursorDeltaState = 0;
					cursorDelta = 0;// сбрасываем всё
				}
				if (cursorDeltaState == 1){// ничего не делаем, но переходим в состояние 2
					cursorDeltaState = 2;
					// активируем событие от клавиатуры, 
					//чтоб обработчики получили вращение колеса
					keyNew = true;
				}
			}

			// TODO главное что бы тут было всё ок
			if (curNew){// запускаем событие обработки изменения положения курсора
				_controller.StartEvent("Cursor", this, PointEventArgs.Set(cursorX, cursorY));
			}

			if (keyNew){// запускаем событие обработки клавиатуры и мышки
				if (!keyboardCleared){
					_controller.StartEvent("Keyboard", this, InputEventArgs.Input(this));
				}
			}
		}

		/// <summary>
		/// Установить состояние кнопок клавиатуры и мыши
		/// </summary>
		/// <returns></returns>
		protected virtual Boolean SetKeyboard() { return false; }

		/// <summary>
		/// Установить положение курсора
		/// </summary>
		/// <returns></returns>
		protected virtual Boolean SetCursor() { return false; }


		/// <summary>
		/// Проверить, нажата ли данная клавиша клавиатуры или мышки
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public virtual bool isKeyPressed(Keys key) { return false; }

		/// <summary>
		/// Преобразовывает код нажатой клавиши на клавиатуре в код с учётом текущей раскладки клавиатуры
		/// </summary>
		/// <returns></returns>
		public virtual String KeysToUnicode()
		{
			return String.Empty;
		}
	}
}
