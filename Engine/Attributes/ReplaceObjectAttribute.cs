using System;

namespace Engine.Attributes
{
	/// <summary>
	/// Атрибут переопределения типа класса в коллекторе. true - заменить 
	/// </summary>
	/// <remarks>Если флаг не установлен, а имя совпадает - это должно вызвать ошибку</remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	class ReplaceObjectAttribute : Attribute
	{
		private readonly Boolean _replaceObject;

		/// <summary>
		/// конструктор
		/// </summary>
		/// <param name="replaceObject"></param>
		public ReplaceObjectAttribute(Boolean replaceObject)
		{
			_replaceObject = replaceObject;
		}

		/// <summary>
		/// Заменить ли объект в колекторе этим
		/// </summary>
		public Boolean ReplaceObject
		{
			get { return _replaceObject; }
		}
	}
}
