﻿using Engine.Controllers;

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
			//_controller = controller;
			_viewMainObjects = new ViewControlSystem(controller);
			_viewMainObjects.Init(_visualizationProvider);
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

	}
}