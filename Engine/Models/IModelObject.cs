using System;
using Engine.Controllers;

namespace Engine.Models
{
	/// <summary>
	/// Интерфейс объекта модели, для унификации (многое взято у IViewModel)
	/// </summary>
	public interface IModelObject//:IEngineObject
	{
		// у обоих нужны методы для инициализации событий
		/// <summary>
		/// Выполнить шаг в алгоритме мат. объекта
		/// </summary>
		void Execute();

		/// <summary>
		/// Ссылка на контроллер
		/// </summary>
		Controller Controller { get; }

		String Name { get; }

	}
}
