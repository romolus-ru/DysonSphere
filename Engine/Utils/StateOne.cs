using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils
{
	/// <summary>
	/// Прототип. Содержит внутри переключаемое состояние
	/// </summary>
	/// <remarks>Изначально предназначен для блокировки повторных нажатий клавиатуры
	/// Передаются ссылки на методы которые будут вызваны в случае изменения состояния. Работает без Controller</remarks>
	public class StateOne
	{
		/// <summary>
		/// Текущее состояние
		/// </summary>
		protected StatesEnum CurrentState = StatesEnum.Off;

		/// <summary>
		/// Делегат события включения состояния
		/// </summary>
		public EventHandler StateOn = null;

		/// <summary>
		/// Делегат события выключения состояния
		/// </summary>
		public EventHandler StateOff = null;

		/// <summary>
		/// Проверяем нажатие
		/// </summary> 
		/// <param name="isPressed">Нажата ли нужная кнопка</param> 
		/// <returns>Если состояние изменилось = true</returns>
		/// <remarks>Если кнопка нажата - состояние переключаем на "нажато" и ждём пока кнопку отпустят
		/// Если кнопка отжата - состояние переключаем на отпущено и ждём пока опять нажмут</remarks>
		public virtual StatesEnum Check(Boolean isPressed)
		{
			if (isPressed && CurrentState == StatesEnum.Off){
				CurrentState = StatesEnum.On;
				if (StateOn != null) StateOn(this,EventArgs.Empty);
				return StatesEnum.On;
			}
			if (isPressed==false && CurrentState==StatesEnum.On){
				CurrentState = StatesEnum.Off;
				if (StateOff != null) StateOff(this, EventArgs.Empty);
				return StatesEnum.Off;
			}
			return StatesEnum.Neutral;// состояние не изменилось
		}

		/// <summary>
		/// Обработчики, ждём нажатия/отпускания кнопок
		/// </summary>
		/// <param name="stateOn"></param>
		/// <param name="stateOff"></param>
		/// <returns></returns>
		public static StateOne Init(EventHandler stateOn, EventHandler stateOff)
		{
			var s = new StateOne();
			s.StateOn = stateOn;
			s.StateOff = stateOff;
			return s;
		}
		
		/// <summary>
		/// без обработчиков, ждём нажатия/отпускания кнопок
		/// </summary>
		/// <returns></returns>
		public static StateOne Init()
		{
			return Init(null, null);
		}

	}
}
