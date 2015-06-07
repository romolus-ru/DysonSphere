using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;

namespace Engine.Views2
{
	/// <summary>
	/// Основной класс визуализации. Умеет только рисовать. постараться избавиться от этого класса - иерархия пусть лучше начинается с контрола, 
	/// умеющего работать с вложенными контролами
	/// </summary>
	[Obsolete]
	class ViewObject
	{
		protected Controller Controller;

		public ViewObject(Controller controller)
		{
			Controller = controller;
		}

		/// <summary>
		/// Инициализация объекта для текущей визуализации (размер экрана и т.п.)
		/// </summary>
		/// <param name="visualizationProvider"></param>
		public void Init(VisualizationProvider visualizationProvider)
		{
			
		}
		
		protected virtual void DrawObject(VisualizationProvider visualizationProvider) { }

	}
}
