using System;
using System.Windows.Forms;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Views
{
	/// <summary>
	/// Прячет клавиатуру и мышь, что бы работать с ними единолично
	/// </summary>
	public class ViewModal : ViewControl
	{
		/// <summary>
		/// Результат работы функции
		/// </summary>
		public int ModalResult = 0;

		protected String OutEvent;
		protected Boolean DeleteObject;// флаг удаления объекта

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="outEvent">Событие, генерируемое при выходе или закрытии модального объекта</param>
		public ViewModal(Controller controller, String outEvent): base(controller,null)
		{
			DeleteObject = false;
			OutEvent = outEvent;
			Controller.AddToOperativeStore("ViewAddObject", this, ViewObjectEventArgs.vObj(this));
		}

		public override void HandlersAdd()
		{
			base.HandlersAdd();
			Controller.PushEventHandlers("Keyboard");
			Controller.PushEventHandlers("Cursor");
			Controller.PushEventHandlers("KeyboardInRange");
			Controller.PushEventHandlers("zOrderToTop");

			Controller.AddEventHandler("Keyboard", KeyboardEH);
			Controller.AddEventHandler("Cursor", CursorEH);
		}

		public override void HandlersRemove()
		{
			Controller.PopEventHandlers("Keyboard");
			Controller.PopEventHandlers("Cursor");
			Controller.PushEventHandlers("KeyboardInRange");
			Controller.PushEventHandlers("zOrderToTop");

			base.HandlersRemove();
		}

		private void CursorEH(object o, EventArgs args)
		{
			DeliverCursorEH(o, args as PointEventArgs);
		}

		private void KeyboardEH(object o, EventArgs args)
		{
			DeliverKeyboardEH(o, args);
			//Keyboard(o, args as InputEventArgs);
		}

		public override void Keyboard(object o, InputEventArgs inputEventArgs)
		{
			base.Keyboard(o, inputEventArgs);
			if (inputEventArgs.IsKeyPressed(Keys.D8)){
				// TODO keyboardClear
				inputEventArgs.KeyboardClear();// по идее ещё надо удалить объект из системы
				DeleteObject = true;
			}
			if (DeleteObject){// раз флаг установлен, то надо удалять объект
				DeleteObjectViewModal();
			}
		}

		private void DeleteObjectViewModal()
		{
			Controller.AddToOperativeStore(OutEvent, this, EventArgs.Empty);
			Controller.AddToOperativeStore("ViewDelObject", this, ViewObjectEventArgs.vObj(this));
		}
	}
}
