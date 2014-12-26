using System;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Views
{
	/// <summary>
	/// Прячет клавиатуру и мышь, что бы работать с ними единолично
	/// </summary>
	/// <remarks>Несамостоятельный объект. При создании необходимо передать созданный модальный объект управляющему объекту
	/// А после - запустить метод удаления объекта</remarks>
	public class ViewModal : ViewControl
	{
		/// <summary>
		/// Результат работы функции
		/// </summary>
		public int ModalResult = 0;

		/// <summary>
		/// Событие, которое сработает при закрытии модального окна
		/// </summary>
		protected String OutEvent;
		/// <summary>
		/// Событие, в котором надо удалить объект 
		/// </summary>
		protected String DestroyEvent;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="parent">Объект, к которому присоединяется модальный объект</param>
		/// <param name="outEvent">Событие, генерируемое при выходе или закрытии модального объекта</param>
		/// <param name="destroyEvent">Событие для удаления модального объекта</param>
		public ViewModal(Controller controller, ViewComponent parent, String outEvent, String destroyEvent): base(controller,parent)
		{
			OutEvent = outEvent;
			DestroyEvent = destroyEvent;
		}

		protected override void HandlersAdder()
		{
			base.HandlersAdder();
			Controller.PushEventHandlers("Keyboard");
			Controller.PushEventHandlers("Cursor");

			Controller.AddEventHandler("Keyboard", KeyboardEH);
			Controller.AddEventHandler("Cursor", CursorEH);
		}

		protected override void HandlersRemover()
		{
			Controller.PopEventHandlers("Keyboard");
			Controller.PopEventHandlers("Cursor");

			base.HandlersRemover();
		}

		private void CursorEH(object o, EventArgs args)
		{
			DeliverCursorEH(o, args as PointEventArgs);
		}

		private void KeyboardEH(object o, EventArgs args)
		{
			DeliverKeyboardEH(o, args);
		}
	}
}
