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
		/// <param name="controller"></param>
		/// <param name="sender"></param>
		/// <param name="viewObject"></param>
		public static void ViewAddObjectCommand(this Controller controller, Object sender, ViewObject viewObject)
		{
			var eventArgs = ViewObjectEventArgs.vObj(viewObject);
			controller.StartEvent("ViewAddObject", sender, eventArgs);
		}
		
		/// <summary>
		/// Удалить объект из визуализации
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="sender"></param>
		/// <param name="viewObject"></param>
		public static void ViewDelObjectCommand(this Controller controller, Object sender, ViewObject viewObject)
		{
			var eventArgs = ViewObjectEventArgs.vObj(viewObject);
			controller.StartEvent("ViewDelObject", sender, eventArgs);
		}
	
		/// <summary>
		/// Завершить работу
		/// </summary>
		/// <param name="controller"></param>
		public static void SystemExitCommand(this Controller controller)
		{
			controller.StartEvent("systemExit");
		}

		/// <summary>
		/// Отправить данные модели (на сервер)
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="dr"></param>
		public static void SendToModelCommand(this Controller controller, DataRecieveEventArgs dr)
		{
			controller.StartEvent("SendToModel", null, dr);
		}

		/// <summary>
		/// Отправить данные виду (клиенту)
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="dr"></param>
		public static void SendToViewCommand(this Controller controller, DataRecieveEventArgs dr)
		{
			controller.StartEvent("SendToView", null, dr);
		}

		/// <summary>
		/// Инициализировать рендер в текстуру
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="viewObject"></param>
		/// <param name="textureName"></param>
		public static void DrawToTexture(this Controller controller, IViewObject viewObject, String textureName)
		{
			controller.StartEvent("DrawToTexture", null, DrawToTextureEventArgs.Set(viewObject, textureName));
		}

	}
}
