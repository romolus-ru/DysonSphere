using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Utils;
using Engine.Utils.Settings;
using Application = System.Windows.Forms.Application;

namespace SettingsEditor
{
	public partial class SettingsMainForm : Form
	{
		public SettingsMainForm()
		{
			InitializeComponent();
		}

		private String _editedFile = "";
		private String EditedFile
		{
			set
			{
				_editedFile = value;
				lblName.Text = @"редактируемый файл " + _editedFile;
				if (_editedFile == "EngineSettings")
				{// разрешаем некоторые опции если основной файл настроек
					cbRunModule.Enabled = true;
					btnScan.Enabled = true;
				}
				else
				{// запрещаем дополнительные опции если файл не основных настроек
					cbRunModule.Enabled = true;
					btnScan.Enabled = true;
				}
			}
			get { return _editedFile; }
		}

		private Settings _currentSettings = null;

		private void SettingsMainForm_Load(object sender, EventArgs e)
		{
			openFileDialog1.InitialDirectory = Application.StartupPath;
			openFileDialog1.Filter = @"файл настроек|*" + Settings.FileExt;
			saveFileDialog1.InitialDirectory = Application.StartupPath;
			saveFileDialog1.Filter = @"файл настроек|*" + Settings.FileExt;
			EditedFile = "EngineSettings" + Settings.FileExt;
			_currentSettings = Settings.Load(EditedFile);
			FillListView();
		}

		private void btnNewElem_Click(object sender, EventArgs e)
		{
			Settings.EngineSettings.AddValue("section", "name", "value", "hint");
		}

		private void btnScan_Click(object sender, EventArgs e)
		{
			if (_currentSettings == null) { return; }
			var controller = new Controller();
			var collector = new Collector(controller);
			var typesForSearch = new List<Type> { typeof(VisualizationProvider), typeof(Module), typeof(Sound), typeof(Input) };
			// получить файлы из текущей директории и записать их в файл настроек (желательно только в EngineSettings)
			_currentSettings.ClearSection("files");// удаляем старые файлы
			var dir = Application.StartupPath;
			var files = Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".exe") || s.EndsWith(".dll"));
			//var files = Directory.GetFiles(dir, "*.exe");// dll
			foreach (var file in files){// добавляем новые файлы
				var f = Path.GetFileName(file);// получаем только имя и если оно с "vshost" то пропускаем этот файл
				if (f.EndsWith(".vshost.exe")) { continue; }
				// делаем относительный путь, удаляя начальный путь к приложению
				var file1 = file.Substring(Environment.CurrentDirectory.Length + 1);
				// если в сборке есть нужные типы классов, то сохраняем
				if (collector.FindObjectsInAssembly(f, typesForSearch))
					_currentSettings.AddValue("assembly", f, "", "" + file1);
			}

			// получаем нужные данные и добавляем к списку выбора
			cbRunModule.Items.Clear();
			foreach (var module in collector.GetObjects(typeof(Module)))
			{
				cbRunModule.Items.Add(module.Key);// + " " + module.Value);
			}
			cbRunModule.SelectedIndex = cbRunModule.Items.IndexOf(_currentSettings.GetValue("Default", "Module"));

			// получаем нужные данные и добавляем к списку выбора
			cbVisualization.Items.Clear();
			foreach (var viProvider in collector.GetObjects(typeof(VisualizationProvider)))
			{
				cbVisualization.Items.Add(viProvider.Key);// + " " + viProvider.Value);
			}
			cbVisualization.SelectedIndex = cbVisualization.Items.IndexOf(_currentSettings.GetValue("Default", "Visualization"));

			// получаем нужные данные и добавляем к списку выбора
			cbInput.Items.Clear();
			foreach (var inp in collector.GetObjects(typeof(Input)))
			{
				cbInput.Items.Add(inp.Key);// + " " + viProvider.Value);
			}
			cbInput.SelectedIndex = cbInput.Items.IndexOf(_currentSettings.GetValue("Default", "Input"));

			// получаем нужные данные и добавляем к списку выбора
			cbSound.Items.Clear();
			foreach (var snd in collector.GetObjects(typeof(Sound)))
			{
				cbSound.Items.Add(snd.Key);// + " " + viProvider.Value);
			}
			cbSound.SelectedIndex = cbSound.Items.IndexOf(_currentSettings.GetValue("Default", "Sound"));
		}

		/// <summary>
		/// Заполнить список текущими настройками
		/// </summary>
		private void FillListView()
		{
			listView1.Items.Clear();
			_currentSettings.Sort1();
			foreach (var row in _currentSettings.Values)
			{
				var lvi = ListViewItemSettingsRow.Get(row);
				listView1.Items.Add(lvi);
			}
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count < 1)
			{
				MessageBox.Show(this,@"Надо выбрать строку настройки для редактирования", @"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			var a = listView1.SelectedItems[0];
			if (a == null) return;
			var b = _currentSettings.SearchRow(a.Text, a.SubItems[1].Text);
			if (b == null) return;
			_currentSettings.AddValue(SettingsEditForm.Edit(b));
			FillListView();
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			DialogResult result = openFileDialog1.ShowDialog();
			if (result == DialogResult.OK)
			{
				_currentSettings = Settings.Load(openFileDialog1.FileName);
				FillListView();
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			DialogResult result = saveFileDialog1.ShowDialog();
			if (result == DialogResult.OK)
			{
				_currentSettings.Save(saveFileDialog1.FileName);
			}
		}

		private void cbVisualization_SelectedIndexChanged(object sender, EventArgs e)
		{
			var b = _currentSettings.SearchRow("Default", "Visualization");
			var a = cbVisualization.SelectedItem;
			if (a == null) return;
			if (b == null)
			{
				b = new SettingsRow("Default", "Visualization", "", "");
				_currentSettings.AddValue(b);
			}
			b.Value = a.ToString();
			FillListView();
		}

		private void cbRunModule_SelectedIndexChanged(object sender, EventArgs e)
		{
			var b = _currentSettings.SearchRow("Default", "Module");
			var a = cbRunModule.SelectedItem;
			if (a == null) return;
			if (b == null)
			{
				b = new SettingsRow("Default", "Module", "", "");
				_currentSettings.AddValue(b);
			}
			b.Value = a.ToString();
			FillListView();
		}

		private void cbSound_SelectedIndexChanged(object sender, EventArgs e)
		{
			return;// не работаем пока со звуком
			var b = _currentSettings.SearchRow("Default", "Sound");
			var a = cbSound.SelectedItem;
			if (a == null) return;
			if (b == null)
			{
				b = new SettingsRow("Default", "Sound", "", "");
				_currentSettings.AddValue(b);
			}
			b.Value = a.ToString();
			FillListView();
		}

		private void cbInput_SelectedIndexChanged(object sender, EventArgs e)
		{
			var b = _currentSettings.SearchRow("Default", "Input");
			var a = cbInput.SelectedItem;
			if (a == null) return;
			if (b == null)
			{
				b = new SettingsRow("Default", "Input", "", "");
				_currentSettings.AddValue(b);
			}
			b.Value = a.ToString();
			FillListView();
		}

		private void listView1_DoubleClick(object sender, EventArgs e)
		{
			btnEdit.PerformClick();
		}
	}
}
