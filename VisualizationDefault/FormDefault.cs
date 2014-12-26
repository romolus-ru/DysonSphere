using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace VisualizationDefault
{
	public partial class FormDefault : Form
	{
		public FormDefault()
		{
			InitializeComponent();
		}

		private Controller _controller;

		public void setup(Controller controller)
		{
			_controller = controller;
			// подключаемся 
			_controller.AddEventHandler("SendText", (o, args) => SendText(o, (MessageEventArgs)args));
			_controller.AddEventHandler("SendDebug", (o, args) => SendDebug(o, (MessageEventArgs)args));
			_controller.AddEventHandler("RefreshEventsList", btnRefreshEventList_Click);
			_controller.AddEventHandler("SendError", (o, args) => SendDebug(o, (MessageEventArgs)args));
			_controller.AddToOperativeStore(null, StoredEventEventArgs.Stored("RefreshEventsList", null, EventArgs.Empty));
			//btnRefreshEventsList_Click(this, EventArgs.Empty);
		}

		/// <summary>
		/// Получить отправленный текст и вывести его на экран
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SendText(object sender, MessageEventArgs e)
		{
			lbText.Items.Add(e.Message);
		}

		/// <summary>
		/// Получить отправленный текст и вывести его на экран
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SendDebug(object sender, MessageEventArgs e)
		{
			lbDebug.Items.Add(e.Message);
		}

		private void btnRefreshEventList_Click(object sender, EventArgs e)
		{
			lbEvents.Items.Clear();
			var events = _controller.GetInfo();
			foreach (var item in events)
			{
				lbEvents.Items.Add(item);
			}
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			if (lbEvents.SelectedIndex < 0){
				MessageBox.Show(@"выберите элемент списка");
				return;}
			var a = lbEvents.Items[lbEvents.SelectedIndex];
			var en = a.ToString().Split(' ')[0];
			lbText.Items.Add(a.ToString().Split(' ')[0] + " событие запущено");
			_controller.StartEvent(en, this, EventArgs.Empty);
		}

		private void btnDeleteSoundEventHandlers_Click(object sender, EventArgs e)
		{
			_controller.RemoveEvent("SoundStart");
			_controller.RemoveEvent("SoundStop");
			_controller.RemoveEvent("SoundLoad");
			_controller.RemoveEvent("SoundUnload");
			btnRefreshEventList.PerformClick();
			_controller.SendText("События для звуков удалены");
			_controller.StartEvent("sdvsdvsdvsdbetbhtbh");
		}
	}
}
