using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine.Utils;
using Engine.Utils.Settings;

namespace SettingsEditor
{
	public partial class SettingsEditForm : Form
	{
		public SettingsEditForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Редактировать 
		/// </summary>
		public static SettingsRow Edit(SettingsRow row)
		{
			var f = new SettingsEditForm();
			f.FillForm(row);
			f.ShowDialog();
			if (f.DialogResult == DialogResult.OK)
			{
				f.SaveForm();
			}
			return f._row;
		}

		private SettingsRow _row;

		private void SaveForm()
		{
			_row.Section = tbSection.Text;
			_row.Name = tbName.Text;
			_row.Value = tbValue.Text;
			_row.Hint = tbHint.Text;
		}
		private void FillForm(SettingsRow row)
		{
			_row = row;
			tbSection.Text = _row.Section;
			tbName.Text = _row.Name;
			tbValue.Text = _row.Value;
			tbHint.Text = _row.Hint;
		}

		private void SettingsEditForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) btnOk.PerformClick();
		}
	}
}
