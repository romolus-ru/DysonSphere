﻿using System;
using Engine.Controllers;
using Engine.Models;
using Engine.Views;

namespace Engine
{
	/// <summary>
	/// Модуль. Определение основных установок для запуска внешнего модуля
	/// </summary>
	/// <remarks>на текущий момент (20141217) предполагается что запускаемый модуль один
	/// Но всё должно быть готово к тому что модуль может быть вызван, запущен и удален много раз во время работы программы</remarks>
	public class Module:IEngineObject
	{
		/// <summary>
		/// Хранение ссылки  на контроллер
		/// </summary>
		/// <remarks>Так как этот контроллер сильно нужен каждому модулю - надо сохранить ссылку на него</remarks>
		protected Controller Controller;

		/// <summary>
		/// Тип модуля
		/// </summary>
		public int ModuleType { get; protected set; }

		/// <summary>
		/// Конструктор. Обязательно без параметров, создаётся через коллектор и через активатор
		/// </summary>
		public Module()
		{
			ModuleType = 0;
		}

		/// <summary>
		/// Установки
		/// </summary>
		/// <param name="model"></param>
		/// <param name="view"></param>
		public void Init(Model model, View view,Controller controller)
		{
			Controller = controller;
			SetUpModel(model, Controller);
			SetUpView(view, Controller);
			HandlersAdd();
		}

		/// <summary>
		/// Установки для модели
		/// </summary>
		/// <param name="model"></param>
		/// <param name="controller"></param>
		protected virtual void SetUpModel(Model model, Controller controller)
		{

		}

		/// <summary>
		/// Установки для вида
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		/// <remarks>Настройки слоёв для визуализации.</remarks>
		protected virtual void SetUpView(View view, Controller controller)
		{

		}

		/// <summary>
		/// Для блокировки дополнительных вызовов dispose
		/// </summary>
		private Boolean _disposed = false;

		public virtual void Dispose()
		{
			if (!_disposed){
				HandlersRemove();
				Controller = null;
				_disposed = true;
			}		
		}

		public virtual void HandlersAdd()
		{
			
		}

		public virtual void HandlersRemove()
		{
			
		}
		
		/// <summary>
		/// Деструктор
		/// </summary>
		~Module()
		{
			Dispose();
		}
	}
}