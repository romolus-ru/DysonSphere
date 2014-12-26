using System;
using System.Collections.Generic;

namespace Engine.Controllers
{
	/// <summary>
	/// Событие контроллера
	/// </summary>
	public class ControllerEvent
	{

		/// <summary>
		/// Флаг приоритета
		/// </summary>
		private int Priority = 0;

		/// <summary>
		/// Список делегатов, организуют стек
		/// </summary>
		private Stack<EventHandler<EventArgs>> _handlersStack = new Stack<EventHandler<EventArgs>>();

		/// <summary>
		/// Текущий делегат
		/// </summary>
		private EventHandler<EventArgs> _handler = null;

		#region Блокировка

		/// <summary>
		/// Флаг блокировки запуска событий
		/// </summary>
		private Boolean _eventBlocked = false;

		/// <summary>
		/// Заблокировать событие
		/// </summary>
		public void BlockEventHandlers()
		{
			_eventBlocked = true;
		}

		/// <summary>
		/// Разблокировать событие
		/// </summary>
		public void UnBlockEventHandlers()
		{
			_eventBlocked = false;
		}

		#endregion

		/// <summary>
		/// Конструктор
		/// </summary>
		public ControllerEvent()
		{
			// Свежесозданный контроллер не подключен ещё ни к какому объекту, поэтому надо его заблокировать
			BlockEventHandlers();
		}

		public void PopEventHandlers() {
			_handler = _handlersStack.Pop();
		}

		public void PushEventHandlers(){
			_handlersStack.Push(_handler);
			_handler = null;
		}

		/// <summary>
		/// Добавить обработчик
		/// </summary>
		/// <param name="eventHandler"></param>
		public void AddEventHandler(EventHandler<EventArgs> eventHandler)
		{
			if (_handler == null) _handler = eventHandler;
							else _handler += eventHandler;
		}

		/// <summary>
		/// Отнять обработчик
		/// </summary>
		/// <param name="eventHandler"></param>
		public void RemoveEventHandler(EventHandler<EventArgs> eventHandler)
		{
			_handler -= eventHandler;
		}

		/// <summary>
		/// Запустить событие и все подключенные события
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		public Boolean StartEvent(Object sender, EventArgs eventArgs)
		{
			if ((!_eventBlocked) || (Priority == 0)){
				var ehl = _handler; // проверяем, есть ли обработчики. редко, но бывает что и нету
				if (ehl != null){
					ehl(sender, eventArgs); // запускаем событие
				}else return false;// нету обработчиков
			}else return false;// событие заблокировано
			return true;
		}

		/// <summary>
		/// Обнулить обработчики события. Уровень не меняется
		/// </summary>
		public void ClearEventHandlers()
		{
			// очищаем обработчики
			_handler = null;
		}

		/// <summary>
		/// получить количество обработчиков у данного события
		/// </summary>
		/// <remarks>Вспомогательный метод</remarks>
		public int GetHandlersCount()
		{
			int ret = 0;
			var e = _handler;
			if (e != null)
			{
				var l = e.GetInvocationList();
				//if (l != null) 
				ret = l.Length;
			}
			return ret;
		}


	}
}
