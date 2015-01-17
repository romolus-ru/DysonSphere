using Engine.Controllers;

namespace Engine.Models
{
	public class ModelObject : IModelObject
	{
		/// <summary>
		/// Уникальный номер, присваивается моделью
		/// </summary>
		public int UniqueNum { get; protected set; }

		/// <summary>
		/// Установить необходимые события
		/// </summary>
		/// <param name="controller"></param>
		public virtual void SetEvents(Controller controller)
		{ }

		public ModelObject(){}

		public void SetUniqueNum(int uniqueNum)
		{
			UniqueNum = uniqueNum;
		}

		/// <summary>
		/// Получить уникальный номер объекта
		/// </summary>
		/// <returns></returns> 
		public int GetUniqueNum()
		{
			return UniqueNum;
		}

		/// <summary>
		/// Выполнить шаг в алгоритме мат. объекта
		/// </summary>
		public virtual void Execute()
		{
			
		}

		/// <summary>
		/// Удалить все связи, мешающие объекту удалиться
		/// </summary>
		/// <remarks>В частности, ссылки на контроллер, ссылки на различные объекты и т.п.</remarks>
		public virtual void ClearLinks()
		{

		}

		/// <summary>
		/// получить имя стратегии по умолчанию
		/// </summary>
		/// <returns></returns>
		public virtual string GetDefaultStrategyName()
		{
			return "defaultNone";
		}
	}
}
