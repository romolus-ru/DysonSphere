using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Controllers.Events
{
	/// <summary>
	/// Сохранённое событие
	/// </summary>
	/// <remarks>Может быть стоит сделать это основным событием, и уже потом отправлять 
	/// куда надо, а не передавать отдельно имя отправителя и аргументы</remarks>
	public class StoredEventEventArgs : EngineEventArgs
	{
		/// <summary>Время срабатывания</summary>
		public TimeSpan EventTime { get; set; }
		/// <summary>Имя события</summary>
		public String EventName { get; private set; }
		/// <summary>Отправитель события</summary>
		public Object EventSender { get; private set; }
		/// <summary>Аргументы события</summary>
		public EventArgs EventArguments { get; private set; }

		/// <summary>
		/// Cоздание сохранённого события
		/// </summary>
		/// <returns></returns>
		public static StoredEventEventArgs Stored(String eventName, Object eventSender, EventArgs eventArguments)
		{
			var st = new StoredEventEventArgs();
			st.EventTime = TimeSpan.Zero;
			st.EventName = eventName;
			st.EventSender = eventSender;
			st.EventArguments = eventArguments;
			return st;
		}

		/// <summary>
		/// Cоздание сохранённого события с учётом времени
		/// </summary>
		/// <returns></returns>
		public static StoredEventEventArgs Stored(TimeSpan eventTime, String eventName, Object eventSender, EventArgs eventArguments)
		{
			var st = new StoredEventEventArgs();
			st.EventTime = eventTime;
			st.EventName = eventName;
			st.EventSender = eventSender;
			st.EventArguments = eventArguments;
			return st;
		}

		/// <summary>
		/// Cоздание сохранённого события с заданным временем в секундах
		/// </summary>
		/// <returns></returns>
		public static StoredEventEventArgs StoredMilliseconds(int eventTime, String eventName, Object eventSender, EventArgs eventArguments)
		{
			return Stored(TimeSpan.FromMilliseconds(eventTime), eventName, eventSender, eventArguments);
		}

		/// <summary>
		/// Cоздание сохранённого события с заданным временем в секундах
		/// </summary>
		/// <returns></returns>
		public static StoredEventEventArgs StoredSeconds(int eventTime, String eventName, Object eventSender, EventArgs eventArguments)
		{
			return Stored(TimeSpan.FromSeconds(eventTime), eventName, eventSender, eventArguments);
		}

		/// <summary>
		/// Cоздание сохранённого события с заданным временем в минутах
		/// </summary>
		/// <returns></returns>
		public static StoredEventEventArgs StoredMinutes(int eventTime, String eventName, Object eventSender, EventArgs eventArguments)
		{
			return Stored(TimeSpan.FromMinutes(eventTime), eventName, eventSender, eventArguments);
		}

	}
}
