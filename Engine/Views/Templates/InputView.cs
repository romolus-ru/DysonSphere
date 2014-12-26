using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;

// TODO класс не испытывался

namespace Engine.Views.Templates
{
	/// <summary>
	/// Элемент ввода данных с клавиатуры
	/// </summary>
	/// <remarks>
	/// для ввода данных нужно кликнуть на строке, это будет сигналом к началу перехвата команд
	/// По идее если пользователь нажал за пределами поля редактирования то это может тоже означать завершение редактирования
	/// </remarks>
	public class InputView:ViewComponent
	{
		/// <summary>
		/// Редактируемый текст
		/// </summary>
		public string Text {get; private set;}

		/// <summary>
		/// Для блокирования одновременного нажатия многих кнопок
		/// </summary>
		private Dictionary<Keys, StateOne> blockers;

		/// <summary>
		/// Сохраняемый текст, если пользователь нажмёт esc он заменит редактируемый пользователем
		/// </summary>
		private string TextOriginal;

		/// <summary>
		/// Признак активирования элемента
		/// </summary>
		private Boolean _active;

		///// <summary>
		///// Текущая позиция курсора
		///// </summary>
		//private int _currentPos = 99;

		/// <summary>
		/// Событие, вызываемое по нажатию enter - завершение редактирования
		/// </summary>
		public String _editSave;

		/// <summary>
		/// Событие, вызываемое по нажатию esc - отмена редактирования
		/// </summary>
		public String _editCalcel;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		public InputView(Controller controller,ViewComponent parent): base(controller,parent)
		{
			blockers.Add(Keys.LButton, StateOne.Init());
			blockers.Add(Keys.Escape, StateOne.Init());
			blockers.Add(Keys.Enter, StateOne.Init());
			for (Keys i = Keys.A; i < Keys.Z; i++){
				blockers.Add(i, StateOneTime.Init(View.Pause));
			}

			Controller.AddEventHandler("InputActivate", ActivateInput);
		}

		/// <summary>
		/// Активировать строку ввода
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void ActivateInput(object sender, EventArgs e)
		{
			if (sender==this)_active=true;
			else _active=false;
		}

		public override void Keyboard(object sender, InputEventArgs e)
		{
			if (!_active) return;
			if (!CanDraw) return;

			var endEdit=false;

			var sEsc = GetCheck(e,Keys.Escape);
			var sLButton = GetCheck(e, Keys.LButton);
			var sEnter = GetCheck(e, Keys.Enter);
			
			// нажата отмена - восстанавливаем текст и потом вызываем команду на завершение редактирования
			if (sEsc==StatesEnum.On) {
				Text=TextOriginal;
				endEdit=true;
			}

			// нажата мышка за пределами поля редактирования или нажат enter - сохраняем
			if ((sLButton==StatesEnum.On && !CursorOver) || sEnter==StatesEnum.On) {
				endEdit=true;
			}

			if (endEdit) {
				Controller.StartEvent("InputActivate", null, EventArgs.Empty);
				Controller.StartEvent(_editSave, this, EventArgs.Empty);
				return;
			}

			for (Keys i = Keys.A; i < Keys.Z; i++){
				var skey = GetCheck(e, i);
				if (skey==StatesEnum.On){
					Text+=(char)i;
				}
			}

			// (если нажата кнопка мыши и мышка находится над кнопкой)
			if (sLButton == StatesEnum.On && CursorOver){
				Controller.StartEvent("InputActivate", this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Проверка и изменения состояния кнопки
		/// </summary>
		/// <param name="inputEA"></param>
		/// <param name="key">проверяемая кнопка</param>
		/// <returns></returns>
		private StatesEnum GetCheck(InputEventArgs inputEA, Keys key)
		{
			if (blockers.ContainsKey(key))return blockers[key].Check(inputEA.IsKeyPressed(key));
			return StatesEnum.Neutral;
		}

		/// <summary>
		/// Инициализация поля ввода
		/// </summary>
		/// <param name="text">Редактируемый текст</param>
		/// <param name="editSave">Событие запускаемое при завершении редактирования</param>
		/// <param name="editCancel">Событие запускаемое при отмене редактирования</param>
		public void Init(String text, String editSave, String editCancel)
		{
			_active = false;
			Text = text;
			TextOriginal = text;
			_editSave = editSave;
			_editCalcel = editCancel;
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			String txt = Text;
			if (CursorOver)	visualizationProvider.SetColor(Color.Yellow);
			else			visualizationProvider.SetColor(Color.Green);
			visualizationProvider.RotateReset();
			visualizationProvider.Rectangle(X, Y, Width, Height);
			if (_active) visualizationProvider.SetColor(Color.Yellow);
			else		 visualizationProvider.SetColor(Color.Gray);
			visualizationProvider.Print(X + 10, Y + 10, txt);
		}

	}
}
