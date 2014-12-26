using System;

namespace Engine.Controllers.Events
{
	public class SoundLinkEventArgs : EventArgs
	{
		/// <summary>
		/// Имя звукового файла
		/// </summary>
		public String FileName { get; private set; }

		/// <summary>
		/// Имя звука
		/// </summary>
		public String SoundName { get; private set; }

		public static SoundLinkEventArgs Link(String soundName, String fileName)
		{
			var ret = new SoundLinkEventArgs();
			ret.SoundName = soundName;
			ret.FileName = fileName;
			return ret;
		}
	}
}
