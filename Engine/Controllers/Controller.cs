using System;
using System.Collections.Generic;
using System.Diagnostics;
using Engine.Controllers.Events;
using Engine.Utils;

namespace Engine.Controllers
{
	/// <summary>
	/// Один из трёх основных классов
	/// </summary>
	public class Controller
	{
		#region список объектов, к которым может предоставлять доступ контроллер
		// дополнительные возможности
		// GetSettings
		// GetCollector
		// SendError

		public Collector GetCollector()
		{
			var eventArgs = new GetCollectorEventArgs();
			StartEvent("GetCollectorObject", null, eventArgs);
			return eventArgs.Collector;
		}

		/// <summary>
		/// Признак что система активирована и нужно отправить события дальше
		/// </summary>
		private Boolean _systemStarted = false;
		/// <summary>
		/// буфер сообщений об ошибках, сохраняемый в контроллере до активации системы
		/// </summary>
		private List<String> _errorMessages = new List<string>();

		/// <summary>
		/// буфер информационных сообщений, сохраняемый в контроллере до активации системы
		/// </summary>
		private List<String> _textMessages = new List<string>();

		/// <summary>
		/// Отправить сообщение об ошибке
		/// </summary>
		/// <param name="message"></param>
		public void SendError(String message)
		{
			if (_systemStarted) {
				var ea = MessageEventArgs.Msg(message);
				StartEvent("SendError", null, ea);
			} else { 
				// сохраняем сообщения локально
				_errorMessages.Add(message);
			}
		}

		/// <summary>
		/// Отправить информационное сообщение
		/// </summary>
		/// <param name="message"></param>
		public void SendText(String message)
		{
			if (_systemStarted) {
				var ea = MessageEventArgs.Msg(message);
				StartEvent("SendText", null, ea);
			} else {
				// сохраняем сообщения локально
				_textMessages.Add(message);
			}
		}

		// TODO KeyboardClear удалить этот метод
		/// <summary>
		/// Заблокировать получение новых нажатий на некоторое время
		/// </summary>
		/// <remarks>
		/// Возможно, пришётся удалить это событие - оно было создано для нейтрализации дополнительных нажатий, 
		/// но подсоединенные события все получают его и вынуждены его обрабатывать. но как с этим бороться - 
		/// я пока не знаю
		/// Но в целом если организовать перехват событий, или для некоторых кнопок нужно заблокировать.. 
		/// хотя не, для отдельных кнопок не получится. а вот для всех сразу - без проблем</remarks>
		public void KeyboardClear()
		{
			StartEvent("KeyboardClear", this, EventArgs.Empty);
		}

		#endregion

		private ControllerStoredEvents _controllerStoredEvents;

		/// <summary>
		/// Словарь контроллеров
		/// </summary>
		private Dictionary<String, ControllerEvent> _controllers =
			new Dictionary<String, ControllerEvent>();

		/// <summary>
		/// Сохранённые события, которые не запускаются сразу а ждут следующего цикла обработки
		/// </summary>
		private List<StoredEventEventArgs> _storedEvent = new List<StoredEventEventArgs>();

		/// <summary>
		/// Конструктор
		/// </summary>
		public Controller()
		{
			// регистрируем обработчик события сохранения события (StoreEvent)
			AddEventHandler("StoreEvent", (o, args) => AddToStore(o, args as StoredEventEventArgs));
			AddEventHandler("BeginLoop", RunStoredEvent);
			AddEventHandler("SystemStarted", SystemStartedEH);
			_controllerStoredEvents = new ControllerStoredEvents(this);
		}

		/// <summary>
		/// Получаем событие запуска системы (Application создал все нужные объекты)
		/// </summary>
		public void SystemStartedEH(object sender, EventArgs eventArgs)
		{
			_systemStarted = true;
			RemoveEventHandler("SystemStarted", SystemStartedEH);
			
			foreach (var item in _errorMessages) {SendError(item);}
			_errorMessages = null;// удаляем, буфер больше не нужен
			
			foreach (var item in _textMessages) {SendText(item);}
			_textMessages = null;// удаляем, буфер больше не нужен
		}

		/// <summary>
		/// Добавить событие к событиям с заданным временем срабатывания
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		public void AddToStore(Object sender, StoredEventEventArgs eventArgs)
		{
			_controllerStoredEvents.AddEvent(eventArgs);
		}

		/// <summary>
		/// Добавить событие к событиям, которые сработают на следующем цикле
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		public void AddToOperativeStore(Object sender, StoredEventEventArgs eventArgs)
		{
			_storedEvent.Add(eventArgs);
		}

		/// <summary>
		/// Добавить событие к событиям, которые сработают на следующем цикле - не нужно создавать StoredEventEventArgs
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		public void AddToOperativeStore(String eventName, Object sender, EventArgs eventArgs)
		{
			Debug.WriteLine("addToOperativeStore " + eventName);
			AddToOperativeStore(sender, StoredEventEventArgs.Stored(eventName, sender, eventArgs));
		}

		/// <summary>
		/// Проверить, есть ли в оперативном хранилище событие с таким именем
		/// </summary>
		/// <param name="eventName"></param>
		/// <returns></returns>
		/// <remarks>Бывают ситуации когда посылается несколько одинаковых событий, но
		/// объекты не знают друг о друге, а событие нужно одно</remarks>
		public Boolean ExistedInOperativeStore(string eventName)
		{
			Boolean ret = false;
			foreach (var argse in _storedEvent)
			{
				if (argse.EventName != eventName) continue;
				ret = true; break;
			}
			return ret;
		}

		/// <summary>
		/// Регистрация контроллера
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="controllerEvent"></param>
		/// <remarks>вынес на всякий случай для централизации</remarks>
		private void RegisterEventController(String eventName, ControllerEvent controllerEvent)
		{
			_controllers.Add(eventName, controllerEvent);
		}

		/// <summary>
		/// Получить контроллер конкретного события. Если нету - создаётся
		/// </summary>
		/// <param name="eventName"></param>
		/// <returns></returns>
		public ControllerEvent GetControllerEvent(String eventName)
		{
			// нету контроллера события, создаём его
			if (!_controllers.ContainsKey(eventName)){
				// создаём событие и добавляем его к контроллерам
				RegisterEventController(eventName, new ControllerEvent());
			}
			return _controllers[eventName];
		}

		/// <summary>
		/// Добавить обработчик события
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="eventHandler"></param>
		public void AddEventHandler(String eventName, EventHandler<EventArgs> eventHandler)
		{
			if (!_controllers.ContainsKey(eventName)){
				// так как такого имени нету, то создаём его
				RegisterEventController(eventName, new ControllerEvent());
			}
			_controllers[eventName].AddEventHandler(eventHandler);
		}

		/// <summary>
		/// Удалить обработчик события
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="eventHandler"></param>
		public void RemoveEventHandler(String eventName, EventHandler<EventArgs> eventHandler)
		{
			if (_controllers.ContainsKey(eventName)){
				_controllers[eventName].RemoveEventHandler(eventHandler);
				// проверяем количество обработчиков этого события.  если обработчиков 0, то удаляем имя события
				int i = _controllers[eventName].GetHandlersCount();
				if (i == 0)
				{
					RemoveEvent(eventName);
				}
			}
		}

		#region Работа со стеком

		/// <summary>
		/// Затолкать текущие обработчики события в стек
		/// </summary>
		/// <param name="eventName"></param>
		public void PushEventHandlers(String eventName)
		{
			if (_controllers.ContainsKey(eventName))
			{
				_controllers[eventName].PushEventHandlers();
			}
		}

		/// <summary>
		/// Забрать обработчики события из стека, сделав их текущими. предыдущие обработчики удаляются безвозвратно
		/// </summary>
		/// <param name="eventName"></param>
		public void PopEventHandlers(String eventName)
		{
			if (_controllers.ContainsKey(eventName))
			{
				_controllers[eventName].PopEventHandlers();
			}
		}

		/// <summary>
		/// Удалить текущие обработчики данного события (из стека на текущем уровне)
		/// </summary>
		/// <param name="eventName">Имя события, обработчики которого будут удалены</param>
		/// <remarks>
		/// Похоже что тут делается то же что и removeevent
		/// Посмотреть что с ними не то.
		/// операции со стеком. там их нету. проверить, нужны ли они и где используются
		/// </remarks>
		public void ClearEventHandlers(String eventName)
		{
			if (_controllers.ContainsKey(eventName))
			{
				//_controllers[eventName].ClearEventHandlers();
			}
		}

		#endregion

		#region Блокировка обработчиков событий (если не будет востребована - удалить)

		/// <summary>
		/// Заблокировать событие
		/// </summary>
		/// <param name="eventName"></param>
		public void BlockEventHandlers(String eventName)
		{
			if (_controllers.ContainsKey(eventName))
			{
				//_controllers[eventName].BlockEventHandlers();
			}
		}

		/// <summary>
		/// Разблокировать событие
		/// </summary>
		/// <param name="eventName"></param>
		public void UnBlockEventHandlers(String eventName)
		{
			if (_controllers.ContainsKey(eventName))
			{
				//_controllers[eventName].UnBlockEventHandlers();
			}
		}

		#endregion

		/// <summary>
		/// Запуск обработчиков события
		/// </summary>
		/// <param name="eventName">Имя запускаемого события</param>
		/// <param name="sender">Отправитель запускаемого события</param>
		/// <param name="eventArgs">Аргументы запускаемого события</param>
		public void StartEvent(String eventName, Object sender=null, EventArgs eventArgs=null)
		{
			Debug.WriteLineIf(eventName == "mP", "mp запущен");
			if (_controllers.ContainsKey(eventName)) {
				if (eventArgs == null) {
					eventArgs = EventArgs.Empty;
				}
				var runned = _controllers[eventName].StartEvent(sender, eventArgs);
				if (!runned) {
					//SendError(eventName + " : событие не запустилось");
				}
			} else { SendError(eventName + " : событие не зарегистрировано в системе"); }
		}

		/// <summary>
		/// Получить информацию о существующих контроллерах
		/// </summary>
		/// <returns></returns>
		/// <remarks>Вспомогательный метод</remarks>
		public List<String> GetInfo()
		{
			var ret = new List<String>();
			var all = 0;
			foreach (var controler in _controllers){
				var i = controler.Value.GetHandlersCount();
				all += i;
				ret.Add(controler.Key + " " + i);
			}
			//ret.Add("всего обработчиков " + all);
			return ret;
		}

		/// <summary>
		/// Запуск сохранённых и готовых к запуску событий
		/// </summary>
		public void RunStoredEvent(object sender, EventArgs eventArgs)
		{	// копия сохранённых событий
			List<StoredEventEventArgs> local = new List<StoredEventEventArgs>(_storedEvent);
			_storedEvent.Clear();// очищаем сохранённые события
			foreach (var argse in local){// запускаем копии событий
				StartEvent(argse.EventName, argse.EventSender, argse.EventArguments);
			}
		}

		/// <summary>
		/// Очищает хранилища событий
		/// </summary>
		public void ResetStores()
		{
			_controllerStoredEvents.Clear();
			_storedEvent.Clear();
		}

		/// <summary>
		/// Удалить событие по имени. удаляется из всех трёх списоков событий полностью
		/// </summary>
		/// <param name="eventName"></param>
		/// <returns>Удалилось ли что-нибудь или нет</returns>
		public Boolean RemoveEvent(String eventName)
		{
			// удаляем из сохранённых событий
			var ret = _controllerStoredEvents.RemoveEvent(eventName);
			// удаляем из оперативных событий
			StoredEventEventArgs d = null;
			foreach (var se in _storedEvent)
				if (se.EventName == eventName){
					d = se;
					break;
				}
			if (d != null){
				_storedEvent.Remove(d);
				ret = true;
			}
			// удаляем имя события
			if (_controllers.ContainsKey(eventName)){
				_controllers.Remove(eventName);
				ret = true;
			}
			return ret;
		}
	}
}
