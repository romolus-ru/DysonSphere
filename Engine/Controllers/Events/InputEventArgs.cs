using System.Windows.Forms;

namespace Engine.Controllers.Events
{
	/// <summary>
	/// Для передачи объекта Input (устройства ввода)
	/// </summary>
	public class InputEventArgs : EngineEventArgs
	{
		private Input input { get; set; }

		public bool IsKeyPressed(Keys key)
		{
			if (input.keyboardCleared)return false;
			return input.isKeyPressed(key);
		}

		/// <summary>
		/// Очистить клавиатуру от событий. Аналог есть у контроллера
		/// </summary>
		public void KeyboardClear()
		{
			input.KeyboardClear(this, Empty);
		}

		#region Координаты курсора. Экранные. Возможно, неправильное использование - Координаты берутся из другого источника

		/// <summary>
		/// Координата курсора X
		/// </summary>
		public int cursorX { get { return input.cursorX; } }

		/// <summary>
		/// Координата курсора Y
		/// </summary>
		public int cursorY { get { return input.cursorY; } }

		/// <summary>
		/// Координата курсора Y
		/// </summary>
		public int cursorDelta { get { return input.cursorDelta; } }

		#endregion

		/// <summary>
		/// Устройство ввода
		/// </summary>
		/// <param name="Input"></param>
		/// <returns></returns>
		static public InputEventArgs Input(Input Input)
		{
			var i = new InputEventArgs();
			i.input = Input;
			return i;
		}
	}
}
