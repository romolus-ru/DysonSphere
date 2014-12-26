using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;

namespace VisualizationDefault
{
	class VisualizationDefault : VisualizationProvider
	{
		private FormDefault _formDefault;

		/// <summary>
		/// Обрабатываем закрытие формы. Посылаем всем сигнал о том что форма закрылась и нужно всё сохранить
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void formClosed(object sender, FormClosedEventArgs e)
		{
			_controller.StartEvent("ExitProgramm", this, EventArgs.Empty);
		}

		/// <summary>
		/// Сохранить текст и вывести его потом одним махом
		/// </summary>
		/// <param name="text"></param>
		private void SaveText(String text)
		{
			_formDefault.lbText.Items.Add(text);
		}

		#region override

		protected override void InitVisualization2()
		{
			// пригодится в дальнейшем.
			//if (w <= h) {
			//    glOrtho(-nRange, nRange, -nRange * h / w, nRange * h / w, -nRange, nRange);
			//} else {
			//    glOrtho(-nRange * w / h, nRange * w / h, -nRange, nRange, -nRange, nRange);
			//}

			_formDefault = new FormDefault();
			_formDefault.KeyPreview = true;

			//_formOpenGl.KeyDown += _formOpenGl_KeyDown;
			// можт сработает, чтоб не плодить разные дополнительные функции
			//_formOpenGl.KeyDown += (o, args) => _controller.StartEvent("keyboard", o, args);
			_formDefault.Focus();
			_formDefault.BringToFront();
			//_formDefault.TopMost = true;
			_formDefault.FormClosed += formClosed;
			_formDefault.setup(_controller);
		}

		/// <summary>
		/// OpenGL установка цвета
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public override void SetColor(int r, int g, int b, int a) { }

		/// <summary>
		/// OpenGL установка фонового цвета
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public override void SetBackgroundColor(int r, int g, int b, int a) { }

		protected override void _Line(int x1, int y1, int x2, int y2) { }

		protected override void _Rectangle(int x, int y, int width, int height) { }

		protected override void _Circle(int cx, int cy, int radius) { }

		public override void Run()
		{
			base.Run();
			_formDefault.BringToFront();
			//_formOpenGl.TopLevel = true;
			//_formOpenGl.TopMost = true;
			_formDefault.Focus();
			_formDefault.ShowDialog();
		}

		public override bool LoadTextureModify(string textureName, string fileName, ProgramTexture prog, Color colorFrom, Color colorTo) { return false; }

		public override bool LoadTexture(string textureName, string fileName) { return false; }

		protected override void _DrawTexture(int x, int y, string textureName, float scale = 1) { }

		protected override void _DrawTexturePart(int x, int y, string textureName, int blockWidth, int blockHeight, int num) { }

		protected override void _DrawTextureMasked(int x, int y, string textureName, string textureMaskName) { }

		public override void LoadFont(string fontName, int fontHeight = 12) { }

		public override void LoadFontTexture(string textureName) { }

		public override int TextLength(string text) { return 0; }

		protected override void PrintOnly(int x, int y, string text)
		{
			SaveText(text);
		}

		public override void BeginDraw() { }

		public override void FlushDrawing() { }

		public override void Rotate(int angle) { }

		public override void RotateReset() { }

		#endregion

	}
}
