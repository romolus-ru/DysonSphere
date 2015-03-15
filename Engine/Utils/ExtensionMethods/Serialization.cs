using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.Utils.ExtensionMethods
{
	/// <summary>
	/// Расширение для сериализации в строку и десериализации из строки
	/// </summary>
	public static class SerializationExtension
	{
		public static T DeserializeObject<T>(this string toDeserialize)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			StringReader textReader = new StringReader(toDeserialize);
			return (T)xmlSerializer.Deserialize(textReader);
		}

		public static string SerializeObject<T>(this T toSerialize)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			StringWriter textWriter = new StringWriter();
			xmlSerializer.Serialize(textWriter, toSerialize);
			return textWriter.ToString();
		}

	}
}
