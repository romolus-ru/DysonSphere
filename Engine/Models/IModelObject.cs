namespace Engine.Models
{
	/// <summary>
	/// Интерфейс объекта модели, для унификации
	/// </summary>
	public interface IModelObject//:IEngineObject
	{
		// у обоих нужны методы для инициализации событий
		/// <summary>
		/// Выполнить шаг в алгоритме мат. объекта
		/// </summary>
		void Execute();

		/// <summary>
		/// Удалить все связи, мешающие объекту удалиться
		/// </summary>
		/// <remarks>В частности, ссылки на контроллер, ссылки на различные объекты и т.п.</remarks>
		void ClearLinks();
	}
}
