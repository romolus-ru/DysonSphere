using System;
using Engine.Controllers;

namespace Engine.Models
{
	public class ModelObject : IModelObject
	{
		/// <summary>
		/// Выполнить шаг в алгоритме мат. объекта
		/// </summary>
		public virtual void Execute()
		{
			
		}

		/// <summary>
		/// Ссылка на контроллер
		/// </summary>
		public Controller Controller { get; private set; }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller">Контроллер</param>
		public ModelObject(Controller controller)
		{
			// работа с событиями, создание и подключение
			Controller = controller;
		}

		private Boolean HandlersAdded = false;
		private Boolean HandlersRemoved = false;

		/// <summary>
		/// Добавить обработчики
		/// </summary>
		/// <remarks>Можно вызывать много раз - добавится всё равно только 1</remarks>
		public void HandlersAddThis()
		{
			if (HandlersAdded) return;// проверка
			HandlersAdder();// добавляем
			HandlersAdded = true;
			HandlersRemoved = false;// разрешаем удалить
		}

		/// <summary>
		/// Удалить обработчики
		/// </summary>
		/// <remarks>Можно вызывать много раз - удалится всё равно только 1</remarks>
		public void HandlersRemoveThis()
		{
			if (HandlersRemoved) return;//проверка
			HandlersRemover();//удаляем
			HandlersRemoved = true;
			HandlersAdded = false;//разрешаем добавить
		}
		/// <summary>
		/// Добавить обработчики
		/// </summary>
		/// <remarks>Предназначения для переопределения пользователем</remarks>
		protected virtual void HandlersAdder()
		{
		}
	
		/// <summary>
		/// Удалить обработчики
		/// </summary>
		/// <remarks>Предназначена для переопределения пользователем</remarks>
		protected virtual void HandlersRemover()
		{
		}
		
		/// <summary>
		/// Имя объекта
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// Для блокировки дополнительных вызовов dispose
		/// </summary>
		private Boolean _disposed = false;

		/// <summary>
		/// Удаление, можно дополнить у потомков
		/// </summary>
		public virtual void Dispose()
		{
			if (!_disposed)
			{
				HandlersRemoveThis();
				Controller = null;
				_disposed = true;
			}
		}

		/// <summary>
		/// Деструктор
		/// </summary>
		~ModelObject()
		{
			Dispose();
		}
	}
}
















