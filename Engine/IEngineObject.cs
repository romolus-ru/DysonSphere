using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	/// <summary>
	/// Интерфейс для объединения некоторых особеностей объектов
	/// </summary>
	/// <remarks>Объект движка должен уметь устанавливать и чистить за собой ссылки от контроллера</remarks>
	public interface IEngineObject : IDisposable
	{
		/// <summary>
		/// Добавить обработчики
		/// </summary>
		void HandlersAdd();

		/// <summary>
		/// Удалить обработчики
		/// </summary>
		void HandlersRemove();


	}
}
