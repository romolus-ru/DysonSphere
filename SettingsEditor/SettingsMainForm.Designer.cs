namespace SettingsEditor
{
	partial class SettingsMainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label4 = new System.Windows.Forms.Label();
			this.cbInput = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbSound = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbVisualization = new System.Windows.Forms.ComboBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnScan = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnNewElem = new System.Windows.Forms.Button();
			this.btnDelElem = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cbRunModule = new System.Windows.Forms.ComboBox();
			this.lblName = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnLoad = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(293, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 13);
			this.label4.TabIndex = 33;
			this.label4.Text = "Устройство ввода";
			// 
			// cbInput
			// 
			this.cbInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbInput.FormattingEnabled = true;
			this.cbInput.Location = new System.Drawing.Point(397, 93);
			this.cbInput.Name = "cbInput";
			this.cbInput.Size = new System.Drawing.Size(247, 21);
			this.cbInput.TabIndex = 32;
			this.cbInput.SelectedIndexChanged += new System.EventHandler(this.cbInput_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(358, 69);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(31, 13);
			this.label3.TabIndex = 31;
			this.label3.Text = "Звук";
			// 
			// cbSound
			// 
			this.cbSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSound.FormattingEnabled = true;
			this.cbSound.Location = new System.Drawing.Point(397, 66);
			this.cbSound.Name = "cbSound";
			this.cbSound.Size = new System.Drawing.Size(247, 21);
			this.cbSound.TabIndex = 30;
			this.cbSound.SelectedIndexChanged += new System.EventHandler(this.cbSound_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(312, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 13);
			this.label2.TabIndex = 29;
			this.label2.Text = "Визуализация";
			// 
			// cbVisualization
			// 
			this.cbVisualization.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbVisualization.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVisualization.FormattingEnabled = true;
			this.cbVisualization.Location = new System.Drawing.Point(397, 39);
			this.cbVisualization.Name = "cbVisualization";
			this.cbVisualization.Size = new System.Drawing.Size(247, 21);
			this.cbVisualization.TabIndex = 28;
			this.cbVisualization.SelectedIndexChanged += new System.EventHandler(this.cbVisualization_SelectedIndexChanged);
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(13, 120);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(631, 337);
			this.listView1.TabIndex = 27;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Секция";
			this.columnHeader1.Width = 103;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Название";
			this.columnHeader2.Width = 125;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Значение";
			this.columnHeader3.Width = 127;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Подсказка";
			this.columnHeader4.Width = 202;
			// 
			// btnScan
			// 
			this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnScan.Location = new System.Drawing.Point(14, 82);
			this.btnScan.Name = "btnScan";
			this.btnScan.Size = new System.Drawing.Size(272, 31);
			this.btnScan.TabIndex = 26;
			this.btnScan.Text = "сканировать расширения";
			this.btnScan.UseVisualStyleBackColor = true;
			this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Location = new System.Drawing.Point(93, 54);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(101, 23);
			this.btnEdit.TabIndex = 25;
			this.btnEdit.Text = "Редактировать";
			this.btnEdit.UseVisualStyleBackColor = true;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnNewElem
			// 
			this.btnNewElem.Location = new System.Drawing.Point(12, 54);
			this.btnNewElem.Name = "btnNewElem";
			this.btnNewElem.Size = new System.Drawing.Size(75, 23);
			this.btnNewElem.TabIndex = 24;
			this.btnNewElem.Text = "Добавить";
			this.btnNewElem.UseVisualStyleBackColor = true;
			this.btnNewElem.Click += new System.EventHandler(this.btnNewElem_Click);
			// 
			// btnDelElem
			// 
			this.btnDelElem.Enabled = false;
			this.btnDelElem.Location = new System.Drawing.Point(200, 54);
			this.btnDelElem.Name = "btnDelElem";
			this.btnDelElem.Size = new System.Drawing.Size(75, 23);
			this.btnDelElem.TabIndex = 23;
			this.btnDelElem.Text = "Удалить";
			this.btnDelElem.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(274, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(117, 13);
			this.label1.TabIndex = 22;
			this.label1.Text = "Запускаемый модуль";
			// 
			// cbRunModule
			// 
			this.cbRunModule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbRunModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbRunModule.FormattingEnabled = true;
			this.cbRunModule.Location = new System.Drawing.Point(397, 12);
			this.cbRunModule.Name = "cbRunModule";
			this.cbRunModule.Size = new System.Drawing.Size(247, 21);
			this.cbRunModule.TabIndex = 21;
			this.cbRunModule.SelectedIndexChanged += new System.EventHandler(this.cbRunModule_SelectedIndexChanged);
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(12, 38);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(112, 13);
			this.lblName.TabIndex = 20;
			this.lblName.Text = "имя файла настроек";
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(93, 12);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 19;
			this.btnSave.Text = "Сохранить";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(12, 12);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(75, 23);
			this.btnLoad.TabIndex = 18;
			this.btnLoad.Text = "Загрузить";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Title = "Открыть файл настроек";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Title = "Сохранить настройки";
			// 
			// SettingsMainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(656, 469);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cbInput);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cbSound);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cbVisualization);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.btnScan);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnNewElem);
			this.Controls.Add(this.btnDelElem);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbRunModule);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnLoad);
			this.Name = "SettingsMainForm";
			this.Text = "Редактор настроек";
			this.Load += new System.EventHandler(this.SettingsMainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbInput;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbSound;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbVisualization;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.Button btnScan;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnNewElem;
		private System.Windows.Forms.Button btnDelElem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbRunModule;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
	}
}

