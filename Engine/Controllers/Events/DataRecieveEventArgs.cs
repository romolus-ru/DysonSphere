using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Engine.Utils.ExtensionMethods;

namespace Engine.Controllers.Events
{
	/// <summary>
	/// Отправить данные
	/// </summary>
	[Serializable]
	public class DataRecieveEventArgs:EngineEventArgs
	{
		/// <summary>
		/// Имя отправляемого события
		/// </summary>
		public String EventName = "";
		/// <summary>
		/// Отправляемые данные
		/// </summary>
		public String DataString = "";
		/// <summary>
		/// Сериализовать
		/// </summary>
		/// <returns></returns>
		public virtual String Serialize()
		{
			/*XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());}
			StringWriter textWriter = new StringWriter();
			xmlSerializer.Serialize(textWriter, this);
			return textWriter.ToString();*/
			return this.SerializeObject();
		}
		/// <summary>
		/// Десериализовать
		/// </summary>
		/// <returns></returns>
		public virtual DataRecieveEventArgs Deserialize(String serialized)
		{
			/*XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
			StringReader textReader = new StringReader(serialized);
			return (DataRecieveEventArgs)xmlSerializer.Deserialize(textReader);*/
			return serialized.DeserializeObject<DataRecieveEventArgs>();
		}

		public static DataRecieveEventArgs Send(String eventName, String dataString)
		{
			var r = new DataRecieveEventArgs();
			r.EventName = eventName;
			r.DataString = dataString;
			return r;
		}

	}
}
