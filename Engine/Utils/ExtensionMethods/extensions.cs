using System;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;

namespace Engine.Utils.ExtensionMethods
{
	// TODO переделать - события будут подключать объект к viewMainObjects и делать обшие операции. со временем можно расширить список
	public static class Systems
	{
		/// <summary>
		/// Добавить объект к визуализации
		/// </summary>
		/// <param name="_controller"></param>
		/// <param name="sender"></param>
		/// <param name="viewObject"></param>
		public static void ViewAddObjectCommand(this Controller _controller, Object sender, ViewObject viewObject)
		{
			var eventArgs = ViewObjectEventArgs.vObj(viewObject);
			_controller.StartEvent("ViewAddObject", sender, eventArgs);
		}
		
		/// <summary>
		/// Удалить объект из визуализации
		/// </summary>
		/// <param name="_controller"></param>
		/// <param name="sender"></param>
		/// <param name="viewObject"></param>
		public static void ViewDelObjectCommand(this Controller _controller, Object sender, ViewObject viewObject)
		{
			var eventArgs = ViewObjectEventArgs.vObj(viewObject);
			_controller.StartEvent("ViewDelObject", sender, eventArgs);
		}
	
		/// <summary>
		/// Завершить работу
		/// </summary>
		/// <param name="_controller"></param>
		public static void systemExitCommand(this Controller _controller)
		{
			_controller.StartEvent("systemExit");
		}

	}
}
