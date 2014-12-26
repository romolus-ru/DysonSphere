using System;
using System.Collections.Generic;
using System.Drawing;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Views
{
	/// <summary>
	/// Компонент умеет работать с вложенными компонентами и определяет возможности работы с клавиатурой и мышкой
	/// </summary>
	/// <remarks>События обрабатываются в прямом порядке объектов в списке Components, а прорисовывается всё в обратном</remarks>
	public class ViewComponent:ViewObject
	{
		/// <summary>
		/// Активное окно (для перемещения)
		/// </summary>
		public Boolean Focused = false;

		/// <summary>
		/// Для инициализации компонентов
		/// </summary>
		protected VisualizationProvider VisualizationProvider;

		// TODO может быть имеет смысл упростить разрешение (до ViewObject) а где нужно проверять - если это VicewComponent то копать глубже
		/// <summary>
		/// Компоненты. извне недоступны. по возможности убрать и для предков
		/// </summary>
		/// <remarks>При добавлении объекта к компонентам происходит его инициализация, поэтому это поле защищено</remarks>
		protected List<ViewComponent> Components=new List<ViewComponent>();

		/// <summary>
		/// Предок компонента в иерархии. Если нету предка - то этот объект должен контролироваться пользователем, система просто так его не выведет
		/// </summary>
		public ViewComponent Parent { get; protected set; }

		public override void Init(VisualizationProvider visualizationProvider)
		{
			base.Init(visualizationProvider);
			VisualizationProvider = visualizationProvider;// сохраняем для будущего использования на всякий случай
		}

		/// <summary>
		/// Добавить объект к списку компонентов
		/// </summary>
		/// <param name="component"></param>
		public void AddComponent(ViewComponent component)
		{
			Components.Add(component);
			component.Parent = this;
			component.HandlersAdd(); HandlersAdd() надо сделать обёртку над методом, который будет предотвращать повторное выполнение 
				// в remove точно нужно вызывать, потому что объект могут изъять из одного компонента и подсадить к другому, поэтому независимо их нужно будет вызывать
			component.Show();
			component.Init(VisualizationProvider);
		}

		/// <summary>
		/// Удалить объект
		/// </summary>
		/// <param name="component"></param>
		public void Remove(ViewComponent component)
		{
			component.HandlersRemove();
			component.Hide();
			Components.Remove(component);
		}

		public ViewComponent(Controller controller, ViewComponent parent=null) : base(controller)
		{
			Parent = parent;
			if (parent != null){
				parent.AddComponent(this);
			}
		}

		/// <summary>
		/// Переместить объект на передний план
		/// </summary>
		/// <param name="topObject"></param>
		public void BringToFront(ViewComponent topObject)
		{
			if (Components.Contains(topObject)){
				Components.Remove(topObject);
				Components.Insert(0,topObject);
			}else{
				foreach (var component in Components){
					component.BringToFront(topObject);// каждый компонент который может содержать другие объекты 
				}
			}
		}

		/// <summary>
		/// Переместить объект на задний план
		/// </summary>
		/// <param name="topObject"></param>
		public void SendToBack(ViewComponent topObject)
		{
			if (Components.Contains(topObject))
			{
				Components.Remove(topObject);
				Components.Add(topObject);
			}else{
				foreach (var component in Components){
					component.SendToBack(topObject);// каждый компонент который может содержать другие объекты 
				}
			}
		}

		/// <summary>
		/// В данном случае надо показать и компоненты
		/// </summary>
		public override void Show()
		{
			base.Show();
			foreach (var component in Components) { component.Show(); }
		}

		/// <summary>
		/// В данном случае надо скрыть и компоненты
		/// </summary>
		public override void Hide()
		{
			foreach (var component in Components) { component.Hide(); }
			base.Hide();
		}

		/// <summary>
		/// Обработка события курсора
		/// </summary>
		/// <param name="o"></param>
		/// <param name="args"></param>
		public virtual void Cursor(object o, PointEventArgs args){}

		/// <summary>
		/// Обработка события клавиатуры
		/// </summary>
		/// <param name="o"></param>
		/// <param name="args"></param>
		public virtual void Keyboard(object o, InputEventArgs args){}

		/// <summary>
		/// Флаг, находится ли курсор над компонентом
		/// </summary>
		public Boolean CursorOver { get; set; }

		/// <summary>
		/// Для блокировки дополнительных вызовов dispose
		/// </summary>
		private Boolean _disposed = !true;

		public override void Dispose()
		{
			base.Dispose();
			if (!_disposed){// обработчики основного класса удаляются в viewObject
				foreach (var component in Components) { component.Dispose(); }
				Components.Clear();
				Components = null;
				_disposed = true;
				X = -99000;
				Y = -99000;
				Width = 0;
				Height = 0;
			}
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			DrawComponentBackground(visualizationProvider);
			DrawComponents(visualizationProvider);
		}

		/// <summary>
		/// Прорисовать фон компонент, если нужно (можно сделать что бы это была простая панелька)
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			if (CursorOver)	visualizationProvider.SetColor(Color.DodgerBlue, 20);
			else			visualizationProvider.SetColor(Color.DimGray, 50);
			visualizationProvider.Box(X, Y, Width, Height);
		}

		/// <summary>
		/// Перерисовать подчиненные компоненты, которые не должны себя прорисовывать сами
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void DrawComponents(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.OffsetAdd(X, Y);// смещаем и рисуем компоненты независимо от их настроек
			for (int index = Components.Count-1; index>=0; index--){
				var component = Components[index];
				component.Draw(visualizationProvider);
			}
			visualizationProvider.OffsetRemove();// восстанавливаем смещение			
		}

		/// <summary>
		/// Покинул ли курсор прерделы объекта (что бы лишний раз не сбрасывать состояние)
		/// </summary>
		protected Boolean CursorOverOffed;

		/// <summary>
		/// Сбрасываем CursorOver, в том числе и у всех вложенных компонентов
		/// </summary>
		protected void CursorOverOff()
		{
			if (CursorOverOffed) return;
			CursorOverOffed = true;
			CursorOver = false;
			foreach (var component in Components)
			{
				component.CursorOverOff();
			}
		}

	}
}
