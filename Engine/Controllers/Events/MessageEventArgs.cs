using System;

namespace Engine.Controllers.Events
{
	/// <summary>
	/// Простое сообщение в виде строки текста
	/// </summary>
	public class MessageEventArgs : EngineEventArgs
	{
		/// <summary>
		/// Сообщение
		/// </summary>
		public String Message;
		/// <summary>
		/// Время просмотра. по уолчанию не задаётся
		/// </summary>
		public int viewTime = 0;

		/// <summary>
		/// Отправить сообщение
		/// </summary>
		/// <param name="message">сообщение</param>
		/// <param name="time">время просмотра</param>
		/// <returns></returns>
		static public MessageEventArgs MsgTime(String message, int time)
		{
			var m = new MessageEventArgs();
			m.Message = message;
			m.viewTime = time;
			return m;
		}

		/// <summary>
		/// Отправить сообщение
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		static public MessageEventArgs Msg(String message)
		{
			return MsgTime(message, 0);
		}
	}
}
