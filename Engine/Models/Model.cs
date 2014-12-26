using System.Collections.Generic;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Models
{
	/// <summary>
	/// Модель. Один из трёх главных классов
	/// </summary>
	public class Model
	{
		/// <summary>
		/// Список математических объектов
		/// </summary>
		private List<IModelObject> _modelObjects = new List<IModelObject>();

		/// <summary>
		/// Контроллер
		/// </summary>
		private Controller _controller;

		/// <summary>
		/// Конструктор
		/// </summary>
		public Model(Controller controller)
		{
			_controller = controller;
			// одинаковые. потом удалить одну
			_controller.AddEventHandler("ModelDelObject", (o, args) => EHDelObject(o, args as ModelObjectEventArgs));

			_controller.AddEventHandler("ModelAddObject", (o, args) => EHAddObject(o, args as ModelObjectEventArgs));

		}

		private void EHDelObject(object sender, ModelObjectEventArgs modelObjectEventArgs)
		{
			modelObjectEventArgs.ModelObject.ClearLinks();
			_modelObjects.Remove(modelObjectEventArgs.ModelObject);
		}

		/// <summary>
		/// Добавить переданный объект к модели
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="modelObjectEventArgs"></param>
		/// <remarks>Нету обработки уникального номера, подразумевается или что у можели уже есть номер
		/// или что он не нужен или придётся переделывать</remarks> 
		private void EHAddObject(object sender, ModelObjectEventArgs modelObjectEventArgs)
		{
			_modelObjects.Add(modelObjectEventArgs.ModelObject);
		}

		/// <summary>
		/// Удалить объект
		/// </summary>
		/// <param name="modelObject"></param>
		public void RemoveObject(IModelObject modelObject)
		{
			_modelObjects.Remove(modelObject);
		}


	}
}
