namespace VisualizationDefault
{
	partial class FormDefault
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.lbText = new System.Windows.Forms.ListBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.lbDebug = new System.Windows.Forms.ListBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.btnDeleteSoundEventHandlers = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnRefreshEventList = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.lbEvents = new System.Windows.Forms.ListBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.lbText);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(258, 481);
			this.panel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Вывод сообщений";
			// 
			// lbText
			// 
			this.lbText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbText.FormattingEnabled = true;
			this.lbText.Location = new System.Drawing.Point(3, 35);
			this.lbText.Name = "lbText";
			this.lbText.Size = new System.Drawing.Size(252, 446);
			this.lbText.TabIndex = 1;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.lbDebug);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(242, 481);
			this.panel2.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(129, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Отладочные сообщения";
			// 
			// lbDebug
			// 
			this.lbDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbDebug.FormattingEnabled = true;
			this.lbDebug.Location = new System.Drawing.Point(3, 36);
			this.lbDebug.Name = "lbDebug";
			this.lbDebug.Size = new System.Drawing.Size(236, 446);
			this.lbDebug.TabIndex = 1;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.btnDeleteSoundEventHandlers);
			this.panel3.Controls.Add(this.btnStart);
			this.panel3.Controls.Add(this.btnRefreshEventList);
			this.panel3.Controls.Add(this.label3);
			this.panel3.Controls.Add(this.lbEvents);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(268, 481);
			this.panel3.TabIndex = 1;
			// 
			// btnDeleteSoundEventHandlers
			// 
			this.btnDeleteSoundEventHandlers.Location = new System.Drawing.Point(150, 2);
			this.btnDeleteSoundEventHandlers.Name = "btnDeleteSoundEventHandlers";
			this.btnDeleteSoundEventHandlers.Size = new System.Drawing.Size(15, 23);
			this.btnDeleteSoundEventHandlers.TabIndex = 5;
			this.btnDeleteSoundEventHandlers.Text = "Start";
			this.btnDeleteSoundEventHandlers.UseVisualStyleBackColor = true;
			this.btnDeleteSoundEventHandlers.Click += new System.EventHandler(this.btnDeleteSoundEventHandlers_Click);
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(129, 3);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(15, 23);
			this.btnStart.TabIndex = 4;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnRefreshEventList
			// 
			this.btnRefreshEventList.Location = new System.Drawing.Point(108, 2);
			this.btnRefreshEventList.Name = "btnRefreshEventList";
			this.btnRefreshEventList.Size = new System.Drawing.Size(15, 23);
			this.btnRefreshEventList.TabIndex = 3;
			this.btnRefreshEventList.Text = "button1";
			this.btnRefreshEventList.UseVisualStyleBackColor = true;
			this.btnRefreshEventList.Click += new System.EventHandler(this.btnRefreshEventList_Click);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(13, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(89, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Запуск событий";
			// 
			// lbEvents
			// 
			this.lbEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbEvents.FormattingEnabled = true;
			this.lbEvents.Location = new System.Drawing.Point(3, 35);
			this.lbEvents.Name = "lbEvents";
			this.lbEvents.Size = new System.Drawing.Size(262, 446);
			this.lbEvents.TabIndex = 2;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.panel1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(776, 481);
			this.splitContainer1.SplitterDistance = 258;
			this.splitContainer1.TabIndex = 3;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.panel2);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.panel3);
			this.splitContainer2.Size = new System.Drawing.Size(514, 481);
			this.splitContainer2.SplitterDistance = 242;
			this.splitContainer2.TabIndex = 2;
			// 
			// FormDefault
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(776, 481);
			this.Controls.Add(this.splitContainer1);
			this.Name = "FormDefault";
			this.Text = "Form1";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.ListBox lbText;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.ListBox lbDebug;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button btnDeleteSoundEventHandlers;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnRefreshEventList;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.ListBox lbEvents;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
	}
}

