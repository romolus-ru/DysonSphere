using System;

namespace Engine.Attributes
{
	/// <summary>
	/// Скрыть этот объект от коллектора
	/// </summary>
	/// <remarks>Этот класс не будет сохранен в коллекторе
	/// Этот объект скрывается от движка</remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	class HideThisClassAttribute : Attribute
	{
		private readonly Boolean _hideThisObject;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="hideThisObject"></param>
		public HideThisClassAttribute(Boolean hideThisObject)
		{
			_hideThisObject = hideThisObject;
		}

		/// <summary>
		/// Спрятать этот объект
		/// </summary>
		public Boolean HideThisObject
		{
			get { return _hideThisObject; }
		}
	}
}
