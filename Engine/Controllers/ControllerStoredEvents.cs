using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Controllers.Events;

namespace Engine.Controllers
{
	/// <summary>
	/// Обработка сохранённых событий
	/// </summary>
	public class ControllerStoredEvents
	{
		/// <summary>
		/// Список сохранённых событий
		/// </summary>
		private List<StoredEventEventArgs> StoredEventsList = new List<StoredEventEventArgs>();

		/// <summary>
		/// ссылка на контроллер
		/// </summary>
		private Controller _controller;

		/// <summary>
		/// Конструктор
		/// </summary>
		public ControllerStoredEvents(Controller controller)
		{
			_controller = controller;
			_controller.AddEventHandler("EndLoop", ReOrderEvents);
			_controller.AddEventHandler("ControllerPause", Pause);
			_controller.AddEventHandler("ControllerUnPause", UnPause);
			currentTime = DateTime.Now;
		}

		private Boolean Paused = false;

		/// <summary>
		/// Снятие событий с паузы
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Слишком топорно, возможно потом надо будет изменить реализацию
		/// особенно для онлайновой версии</remarks>
		private void UnPause(object sender, EventArgs e)
		{
			Paused = false;
		}

		/// <summary>
		/// Постановка событий на паузу
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Pause(object sender, EventArgs e)
		{
			Paused = true;
		}

		/// <summary>
		/// Добавить событие на хранение
		/// </summary>
		/// <param name="eventToStore"></param>
		public void AddEvent(StoredEventEventArgs eventToStore)
		{
			StoredEventsList.Add(eventToStore);
		}

		/// <summary>
		/// Очистить список
		/// </summary>
		public void Clear()
		{
			StoredEventsList.Clear();
		}

		/// <summary>
		/// текущее сохранённое время для контроля относительного времени с момента последнего запуска
		/// </summary>
		private DateTime currentTime;

		/// <summary>
		/// Меняем время и смотрим, запускать ли событие или ещё рано
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ReOrderEvents(object sender, EventArgs e)
		{
			if (Paused) return;
			DateTime t = DateTime.Now;
			TimeSpan sp = t - currentTime;// прошло миллисекунд со времени последнего запуска
			// смотрим что надо запускать, запускаем и удаляем
			// создаём копию массива, потому что в основном надо будет удалять элементы
			var localStoredEvents = new List<StoredEventEventArgs>(StoredEventsList);
			foreach (var stored in localStoredEvents)
			{
				stored.EventTime -= sp;// отнимаем время
				if (stored.EventTime > TimeSpan.Zero) continue;
				// добавляем к контроллеру для следующего запуска
				_controller.AddToOperativeStore(null, stored);
				// удаляем запись, она уже отработала
				StoredEventsList.Remove(stored);
			}
			// сохраняем текущее время
			currentTime = t;
		}

		/// <summary>
		/// Удаление события по имени
		/// </summary>
		/// <param name="eventName"></param>
		public Boolean RemoveEvent(String eventName)
		{
			var ret = false;
			StoredEventEventArgs d = null;
			foreach (var se in StoredEventsList)
				if (se.EventName == eventName) { d = se; break; }
			if (d != null)
			{
				StoredEventsList.Remove(d);
				ret = true;
			}
			return ret;
		}
	}
}
