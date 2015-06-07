using System;
using System.Collections.Generic;
using Engine;
using Engine.Controllers.Events;
using Engine.Controllers;
using Engine.Models;
using Engine.Utils;
using Engine.Utils.Settings;
using Engine.Views;
using Timer = System.Windows.Forms.Timer;

namespace Server
{
	public class Application
	{

		#region Основные переменные

		private Settings _serverSettings;
		private const string EngineSettingsServer = "EngineSettingsServer";

		/// <summary>
		/// Главный Таймер
		/// </summary>
		private readonly Timer _mainTimer;

		/// <summary>
		/// Модель
		/// </summary>
		private readonly Model _model;

		/// <summary>
		/// Вид
		/// </summary>
		private readonly View _view;

		/// <summary>
		/// Контроллер
		/// </summary>
		private readonly Controller _controller;

		/// <summary>
		/// Визуализатор
		/// </summary>
		private readonly VisualizationProvider _visualizationProvider;

		/// <summary>
		/// Устройства ввода
		/// </summary>
		private readonly Input _input;

		/// <summary>
		/// Звук
		/// </summary>
		private readonly Sound _sound;

		/// <summary>
		/// Собиратель объектов из сборок
		/// </summary>
		private readonly Collector _collector;

		///// <summary>
		///// Параметры для запуска модулей. каждый модуль может хранить свои параметры
		///// </summary>
		//private ModuleParams parameters;

		/// <summary>
		/// Отправка данных клиенту
		/// </summary>
		private readonly DataSender _toModelDataSender;

		private readonly DataSender _toViewDataSender;


		#endregion

		/// <summary>
		/// Конструктор
		/// </summary>
		public Application()
		{
			_serverSettings=Settings.Load(EngineSettingsServer);
			// создание контроллера
			_controller = new Controller();

			// регистрируем метод для получения главного таймера
			_controller.AddEventHandler("GetMainTimerRun", (o, args) => GetMainTimerRun(o, (GetHandlerEventArgs)args));

			// создание коллектора
			_collector = new Collector(_controller);

			#region заполнение массива typesForSearch классами и интерфейсами, которые надо искать в сборках

			var typesForSearch = new List<Type>();
			typesForSearch.Add(typeof(VisualizationProvider)); // визуализация
			typesForSearch.Add(typeof(IModelObject)); // объект модели
			typesForSearch.Add(typeof(ControllerEvent)); // Событие контроллера
			//typesForSearch.Add(typeof(ControllerElement)); // Элемент контроллера
			typesForSearch.Add(typeof(Module)); // Модуль
			typesForSearch.Add(typeof(Input)); // Устройства ввода
			typesForSearch.Add(typeof(ISound)); // Вывод звука

			#endregion

			// создаём таймер и настраиваем его на более менее оптимальную работу
			// было последним, но так как нужна работа инициализации визуализации,
			// которая убирает таймер, его лучше создать сразу
			_mainTimer = new Timer();
			_mainTimer.Interval = TimerInterval;
			_mainTimer.Tick += MainTimerRun;

			// чтение из настроек сборок, которые надо сканировать
			var assemblies = new List<string>();
			foreach (var sr in Settings.EngineSettings.GetValues("assembly"))
			{
				assemblies.Add(sr.Hint);
			}
			// сканирование сборок);
			foreach (var assembly in assemblies)
			{
				_collector.FindObjectsInAssembly(assembly, typesForSearch);
			}

			// если ошибок не возникло то надо создавать визуализацию
			// хотя можно создать визуализацию через контрол
			//_controller.StartEvent("VisualizationStart", null, EventArgs.Empty);
			String visualizationName = Settings.EngineSettings.GetValue("Default", "Visualization");
			if (visualizationName == "") { throw new Exception(" не указана система визуализации в настройках"); }
			_visualizationProvider = (VisualizationProvider)_collector.Create(
				typeof(VisualizationProvider), visualizationName);
			if (_visualizationProvider == null) { throw new Exception("визуализатор не создан"); }
			// визуализация запускается в методе run
			_visualizationProvider.InitVisualization(_controller);

			// создание модели и вида (контроллер создаётся в начале, он особо не требователен)
			_model = new Model(_controller);
			_view = new View(_controller, _visualizationProvider);

			String inputName = Settings.EngineSettings.GetValue("Default", "Input");
			if (inputName == "") { inputName = "Engine.Input"; }// базовый класс, заготовка
			_input = (Input)_collector.Create(typeof(Input), inputName);
			if (_input == null)
			{
				_controller.SendError("Устройство ввода отсутствует " + inputName);
				_input = new Input();
			}
			_input.Init(_controller);

			// пока предполагаем что звук отдельно. но в общем случае звук может быть встроен в визуализацию
			String soundName = Settings.EngineSettings.GetValue("Default", "Sound");
			if (soundName == "") { soundName = "Engine.ISound"; }// базовый класс, заготовка
			var isound = (ISound)_collector.Create(typeof(ISound), soundName);
			if (isound == null)
			{
				_controller.SendError("Звуковое устройство отсутствует " + soundName);
				// и оставляем его null раз его всё равно нету
			}
			_sound = new Sound(isound);
			_sound.Init(_controller);

			_toModelDataSender = new DataSender(_controller, "SendToModel");// создаём отправителя сообщений Модели (фактически серверу)
			_toViewDataSender = new DataSender(_controller, "SendToView");// создаём отправителя сообщений Виду (фактически клиенту)

			// запуск одного модуля из настроек
			String moduleName = Settings.EngineSettings.GetValue("Default", "Module");
			if (moduleName == "") { throw new Exception(" не указан запускаемый модуль в настройках"); }// модуль обязательно нужен
			var module = (Module)_collector.Create(typeof(Module), moduleName);
			if (_input == null) { _controller.SendError("Запускаемый модуль не обнаружен в подключенных сборках " + moduleName); }
			module.InitServer(_model, _controller);

			_controller.SendError("Создание объекта Application завершено");
			_controller.StartEvent("SystemStarted");// передаём все сообщения объектам
		}

		/// <summary>
		/// Вспомогательная функция. Получает метод основного цикла событий и отключает таймер
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void GetMainTimerRun(object sender, GetHandlerEventArgs args)
		{
			if (_mainTimer != null)
			{
				_mainTimer.Enabled = false; // отключаем таймер
			}
			args.Set(MainTimerRun);		// передаём главную функцию
		}

		/// <summary>
		/// Запуск Главного Таймера
		/// </summary>
		public void Run()
		{
			_mainTimer.Start();		// сначала запускаем таймер
			_visualizationProvider.Run();	// запускаем модальное диалоговое окно
			Settings.EngineSettings.Save(EngineSettingsServer);// после закрытия модального окна должен сработать этот оператор
			_sound.ClearLinks();    // прерываем все звуки
		}

		/// <summary>
		/// Значение времени для рассчёта количества кадров в секунду
		/// </summary>
		public static int TimerInterval = 1000 / 60;

		/// <summary>
		/// Основной цикл. по таймеру
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void MainTimerRun(Object sender, EventArgs eventArgs)
		{
			// Неплохо бы определять сколько времени прошло для рассчета и рисования. 
			// и в зависимости от этого пропускать циклы рисования или пару лишних раз проводить рассчеты

			_controller.StartEvent("BeginLoop", null, EventArgs.Empty);
			_input.GetInput();// обработка устройств ввода

			_model.Execute();

			_view.Draw();

			_controller.StartEvent("EndLoop", null, EventArgs.Empty);
		}
	}
}
