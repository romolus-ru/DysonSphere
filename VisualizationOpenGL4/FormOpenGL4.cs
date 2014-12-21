using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenGL4NET;

namespace VisualizationOpenGL4
{
	public partial class FormOpenGL4 : Form
	{
		public OpenGL4NET.RenderingContext rc;

		public FormOpenGL4()
		{
			InitializeComponent();
			rc = RenderingContext.CreateContext(this);
			//SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}

		void Render()
		{
			//gl.Clear(GL.COLOR_BUFFER_BIT);
			//Line(0, 0, 100, 100);
			// here is the right place to draw all your scene
			//rc.SwapBuffers();
		}

		//protected override void OnSizeChanged(EventArgs e)
		//{
		//	gl.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
		//	// change projection matrix etc.
		//}

		//protected override void WndProc(ref Message m)
		//{
		//	switch (m.Msg) {
		//		case Windows.WM_PAINT: Render(); break;
		//		default: base.WndProc(ref m); break;
		//	}
		//}



		public void Line(int x1, int y1, int x2, int y2)
		{
			gl.Disable(GL.BLEND);
			gl.Enable(GL.LINE_SMOOTH);
			gl.Disable(GL.TEXTURE_2D);
			gl.Begin(GL.LINES);
			gl.Vertex2f(x1, y1);
			gl.Vertex2f(x2, y2);
			gl.End();
		}

	}
}
