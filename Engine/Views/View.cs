using System;
using System.Collections.Generic;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Views
{
	/// <summary>
	/// Один из трёх главных классов
	/// </summary>
	public class View
	{
		/// <summary>
		/// Пауза для запрета обработки кнопки
		/// </summary>
		public static int Pause = 30;

		private List<DrawToTextureEventArgs> _drawToTexture = new List<DrawToTextureEventArgs>();

		/// <summary>
		/// Класс для объекта визуализации
		/// </summary>
		private VisualizationProvider _visualizationProvider;

		/// <summary>
		/// Основной корневой объект визуализации, фоновый
		/// </summary>
		private ViewControlSystem _viewMainObjects = null;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="visualizationProvider"></param>
		public View(Controller controller, VisualizationProvider visualizationProvider)
		{
			// сохраняем всё самое важное из переданного
			_visualizationProvider = visualizationProvider;
			_viewMainObjects = new ViewControlSystem(controller);
			_viewMainObjects.Init(_visualizationProvider);
			controller.AddEventHandler("ViewBringToFront", EHBringToFront);
			controller.AddEventHandler("ViewAddObject", (o, args) => EHAddObject(o, args as ViewObjectEventArgs));
			controller.AddEventHandler("ViewDelObject", (o, args) => EHDelObject(o, args as ViewObjectEventArgs));
			controller.AddEventHandler("ViewDrawToTexture", (o, args) => EHDrawToTexture(o, args as DrawToTextureEventArgs));

		}

		private void EHDrawToTexture(object sender, DrawToTextureEventArgs drawToTextureEventArgs)
		{
			// сохраняем переданные параметры
			_drawToTexture.Add(drawToTextureEventArgs);
		}

		private void EHAddObject(object sender, ViewObjectEventArgs viewObjectEventArgs)
		{
			_viewMainObjects.AddComponent(viewObjectEventArgs.ViewObject as ViewComponent);
		}

		private void EHDelObject(object sender, ViewObjectEventArgs viewObjectEventArgs)
		{
			_viewMainObjects.Remove(viewObjectEventArgs.ViewObject as ViewComponent);
		}

		private void EHBringToFront(object sender, EventArgs eaArgs)
		{
			var c = sender as ViewComponent;
			if (c == null) return;
			_viewMainObjects.BringToFront(c);
		}

		/// <summary>
		/// Добавление объекта
		/// </summary>
		/// <param name="obj"></param>
		public void AddObject(ViewComponent obj)
		{
			_viewMainObjects.AddComponent(obj);
		}

		/// <summary>
		/// Удаление объекта
		/// </summary>
		/// <param name="obj"></param>
		public void DeleteObject(ViewComponent obj)
		{
			_viewMainObjects.Remove(obj);
		}

		/// <summary>
		/// Рисование объектов, с проверкой, кого надо рисовать
		/// </summary>
		public void Draw()
		{
			_visualizationProvider.BeginDraw();
			_viewMainObjects.Draw(_visualizationProvider);
			_visualizationProvider.FlushDrawing();
		}

		/// <summary>
		/// Рисование в текстуру
		/// </summary>
		public void DrawToTexture()
		{
			foreach (var argse in _drawToTexture){
				_visualizationProvider.BeginDraw();
				argse.ViewObject.DrawToTexture(_visualizationProvider);
				_visualizationProvider.CopyToTexture(argse.TextureName);
			}
		}
	}
}
