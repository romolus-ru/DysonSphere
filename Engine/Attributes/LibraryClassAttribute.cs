using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Attributes
{
	/// <summary>
	/// Атрибут определения библиотечного класса. Такой класс может быть создан с помощью коллектора
	/// </summary>
	/// <remarks>Класс, помеченный этим атрибутом, может быть создан коллектором</remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class LibraryClassAttribute : Attribute
	{
		private readonly String _name;
		private readonly Version _version;

		/// <summary>
		/// конструктор
		/// </summary>
		/// <param name="name"></param>
		/// <param name="version"></param>
		public LibraryClassAttribute(String name,String version)
		{
			_name = name;
			_version = new Version(version);
		}

		/// <summary>
		/// Имя класса в коллекторе
		/// </summary>
		public String Name
		{
			get { return _name; }
		}

		public Version Version
		{
			get { return _version; }
		}
	}
}
