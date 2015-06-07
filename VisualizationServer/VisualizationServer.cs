using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers.Events;

namespace VisualizationServer
{
	// TODO удалить лишнее. причесать и может быть что-нибудь можно будет добавить
	class VisualizationServer : VisualizationProvider
	{
		private FormMain _formMain;

		protected override void InitVisualization2()
		{
			_formMain = new FormMain();
			_formMain.KeyPreview = true;

			_formMain.Text = @"Server";

			_formMain.Focus();
			//_formMain.BringToFront();

			_controller.AddEventHandler("systemExit", Exit);
			// дополнительные события для сервера
		}

		private void Exit(object sender, EventArgs eventArgs)
		{
			_formMain.Close();
		}

		/// <summary>
		/// OpenGL установка цвета
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public override void SetColor(int r, int g, int b, int a)
		{}

		/// <summary>
		/// OpenGL установка фонового цвета
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public override void SetBackgroundColor(int r, int g, int b, int a){}

		protected override void _Line(int x1, int y1, int x2, int y2){}

		protected override void _Rectangle(int x, int y, int width, int height){}

		protected override void _Box(int x, int y, int width, int height){}

		public override void Circle(int cx, int cy, int radius){}

		public override void Run(){
			base.Run();
			_formMain.BringToFront();
			_formMain.Focus();
			_formMain.ShowDialog();
		}

		public override bool LoadTextureModify(string textureName, string fileName, ProgramTexture prog, Color colorFrom, Color colorTo){return false;}

		public override bool LoadTexture(string textureName, string fileName){return false;}

		protected override void _DrawTexture(int x, int y, string textureName, float scale = 1)
		{
		}

		protected override void _DrawTexturePart(int x, int y, string textureName, int blockWidth, int blockHeight, int num){}

		protected override void _DrawTexturePart(int x, int y, String textureName, int xtex, int ytex, int width, int height){}

		protected override void _DrawTextureMasked(int x, int y, string textureName, string textureMaskName){}

		public override void LoadFont(string fontName, int fontHeight = 12){}

		public override void LoadFontTexture(string textureName){}

		public override int TextLength(string text){return text.Length;}

		private List<StringTimed> strings = new List<StringTimed>();

		protected override void PrintOnly(int x, int y, string text)
		{
			var dt = DateTime.Now;
			StringTimed s1 = null;
			foreach (var s in strings){
				if (s.AddText(-1, dt, text)){s1 = s;break;}
			}
			if (s1 == null){// ненашли такую строку, значит создаём
				s1=StringTimed.Create(-1,dt,TimeSpan.FromSeconds(3),text);
				strings.Add(s1);
			}
			if (s1.Updated) { _formMain.listBox1.Items.Insert(0,text);}
		}

		public override void BeginDraw(){}

		public override void FlushDrawing()
		{
		}

		public override void Rotate(int angle){}

		public override void RotateReset()
		{
		}

		protected override void _CopyToTexture(string textureName){}

		protected override void _DeleteTexture(string textureName){}
	}

}
