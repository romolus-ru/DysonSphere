using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers.Events;
using Tao.DevIl;
using Tao.Platform.Windows;
using OpenGL4NET;

namespace VisualizationOpenGL4
{
	// TODO удалить лишнее. причесать и может быть что-нибудь можно будет добавить
	class VisualizationOpenGL4 : VisualizationProvider
	{
		private FormOpenGL4 _formOpenGl;

		protected override void InitVisualization2()
		{
			// пригодится в дальнейшем.
			//if (w <= h) {
			//    glOrtho(-nRange, nRange, -nRange * h / w, nRange * h / w, -nRange, nRange);
			//} else {
			//    glOrtho(-nRange * w / h, nRange * w / h, -nRange, nRange, -nRange, nRange);
			//}

			_formOpenGl = new FormOpenGL4();
			_formOpenGl.Size = new Size(CanvasHeight, CanvasWidth);
			_formOpenGl.KeyPreview = true;

			//_formOpenGl.oglView.AccumBits = 0;
			//_formOpenGl.oglView.AutoCheckErrors = false;
			//_formOpenGl.oglView.AutoFinish = false;
			//_formOpenGl.oglView.AutoMakeCurrent = true;
			//_formOpenGl.oglView.AutoSwapBuffers = true;
			//_formOpenGl.oglView.ColorBits = 32;
			//_formOpenGl.oglView.DepthBits = 16;
			//_formOpenGl.oglView.Location = new Point(0, 0);
			//_formOpenGl.oglView.Name = "oglView";
			//_formOpenGl.oglView.StencilBits = 0;
			//_formOpenGl.oglView.TabIndex = 0;
			//_formOpenGl.oglView.InitializeContexts();
			_formOpenGl.Text = @"OpenGL4";//"OpenGL";

			_formOpenGl.MouseWheel += mouseWheel;
			_formOpenGl.Focus();
			_formOpenGl.BringToFront();

			Il.ilInit();
			Il.ilEnable(Il.IL_ORIGIN_SET);
			//Glut.glutInit();
			//Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
			// очиcтка окна
			gl.ClearDepth(1.0f);		// Depth Buffer Setup
			gl.Disable(GL.DEPTH_TEST);	// Disable Depth Buffer было enable, но почему то из-за этого не выводились текстуры
			gl.DepthFunc(GL.LESS);		// The Type Of Depth Test To Do
			gl.Enable(GL.ALPHA_TEST);
			ResizeGlScene(_formOpenGl.Width, _formOpenGl.Height);
			LoadFont("Consolas", 14);
			//LoadFont("Calibri", 12);
			_controller.AddEventHandler("setHeader", (o, args) => SetHeader(o, args as MessageEventArgs));
			_controller.AddEventHandler("systemExit", (o, args) => Exit(o, args as EventArgs));
			LoadTexture("WTBGRound", "Resources/round.png");
			LoadTexture("WTCursor", "Resources/cursor.png");
			//LoadTextureModify("clear", "Resources/clear256x256.tga", new TPTRounded(), Color.Empty, Color.Empty);
		}

		private void Exit(object sender, EventArgs eventArgs)
		{
			_formOpenGl.Close();
		}

		private void nextEnc1(object sender, EventArgs e)
		{
			getNextEnc();
		}

		/// <summary>
		/// Отлавливаем колёсико мыши
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mouseWheel(object sender, MouseEventArgs e)
		{
			//_controller.StartEvent("ViewStringListAdd", this, MessageEventArgs.Msg("Колесо мыши " + e.Delta));
			_controller.StartEvent("CursorDelta", this, IntegerEventArgs.Send(e.Delta));
		}

		/// <summary>
		/// Получаем координаты курсора из других источников
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void CursorCoordinates(object sender, PointEventArgs e)
		{
			var p = _formOpenGl.PointToClient(e.Pt);
			e.SetCoord(p);// меняем переданные координаты на новые
		}

		private void ResizeGlScene(int width, int height)
		{
			// задаётся размер экрана, влияет на искажение вида, поэтому  надо пересчитать размеры
			gl.Viewport(0, 0, width, height);
			gl.MatrixMode(GL.PROJECTION);
			gl.LoadIdentity();

			//Glu.gluOrtho2D(0, width, height, 0);
			gl.Ortho(0, width, height, 0, -1, 1);
			gl.MatrixMode(GL.MODELVIEW);
			CanvasHeight = height;
			CanvasWidth = width;
		}

		private void SetHeader(object sender, MessageEventArgs e)
		{
			if (e != null)
			{
				_formOpenGl.Text = e.Message;
			}
		}

		private float colorR = 0;
		private float colorG = 0;
		private float colorB = 0;
		private float colorA = 0;

		/// <summary>
		/// OpenGL установка цвета
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public override void SetColor(int r, int g, int b, int a)
		{
			colorR = (float)r / 255;
			colorG = (float)g / 255;
			colorB = (float)b / 255;
			colorA = (float)a / 255;
			gl.Color4f(colorR, colorG, colorB, colorA);
		}

		private float backgroundColorR = 0;
		private float backgroundColorG = 0;
		private float backgroundColorB = 0;
		private float backgroundColorA = 0;

		/// <summary>
		/// OpenGL установка фонового цвета
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public override void SetBackgroundColor(int r, int g, int b, int a)
		{
			backgroundColorR = (float)r / 255;
			backgroundColorG = (float)g / 255;
			backgroundColorB = (float)b / 255;
			backgroundColorA = (float)a / 255;
			// устанавливается он в начале цикла, т.е. в данном случае в следующий раз при начале прорисовки
			//Gl.glColor4f(colorR, colorG, colorB, colorA);
		}

		protected override void _Line(int x1, int y1, int x2, int y2)
		{
			gl.Disable(GL.BLEND);
			gl.Enable(GL.LINE_SMOOTH);
			gl.Disable(GL.TEXTURE_2D); // Turn off textures
			gl.Enable(GL.BLEND);
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

			gl.Begin(GL.LINES);
			gl.Vertex2f(x1, y1);
			gl.Vertex2f(x2, y2);
			gl.End();
		}

		protected override void _Rectangle(int x, int y, int width, int height)
		{
			Line(x, y, x + width, y);
			Line(x + width, y, x + width, y + height);
			Line(x + width, y + height, x, y + height);
			Line(x, y + height, x, y);
		}

		protected override void _Box(int x, int y, int width, int height)
		{
			gl.Disable(GL.BLEND);
			gl.Enable(GL.LINE_SMOOTH);
			gl.Disable(GL.TEXTURE_2D); // Turn off textures
			gl.Enable(GL.BLEND);
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

			gl.Begin(GL.QUADS);
			gl.Vertex2f(x, y);
			gl.Vertex2f(x + width, y);
			gl.Vertex2f(x + width, y + height);
			gl.Vertex2f(x, y + height);
			gl.End();
		}

		private int num_segments = 36;

		public override void Circle(int cx, int cy, int radius)
		{
			double theta = 2 * Math.PI / num_segments;
			double tangetial_factor = Math.Tan(theta);//calculate the tangential factor 
			double radial_factor = Math.Cos(theta);//calculate the radial factor 
			double x = radius;//we start at angle = 0 
			double y = 0;
			gl.Disable(GL.TEXTURE_2D);
			gl.Begin(GL.LINE_LOOP);
			for (int ii = 0; ii < num_segments; ii++)
			{
				gl.Vertex2d(x + cx, y + cy); //output vertex 

				//calculate the tangential vector 
				//remember, the radial vector is (x, y) 
				//to get the tangential vector we flip those coordinates and negate one of them 

				double tx = -y;
				double ty = x;

				//add the tangential vector 

				x += tx * tangetial_factor;
				y += ty * tangetial_factor;

				//correct using the radial factor 

				x *= radial_factor;
				y *= radial_factor;
			}
			gl.End();
		}

		public override void Run()
		{
			base.Run();
			_formOpenGl.BringToFront();
			//_formOpenGl.TopLevel = true;
			//_formOpenGl.TopMost = true;
			_formOpenGl.Focus();
			_formOpenGl.ShowDialog();
		}

		public override bool LoadTextureModify(string textureName, string fileName, ProgramTexture prog, Color colorFrom, Color colorTo)
		{
			// если текстура уже есть то выходим
			// но перед выходом неплохо бы посмотреть количество ссылок и изменить их количество
			if (_textures.ContainsKey(textureName)) return false;

			bool opacity = true;
			// идентификатор текстуры
			int imageId = 0;

			bool r = false;
			// создаем изображение с идентификатором imageId
			Il.ilGenImages(1, out imageId);
			// делаем изображение текущим
			Il.ilBindImage(imageId);

			// адрес изображения
			string url = fileName;

			// пробуем загрузить изображение);
			if (Il.ilLoadImage(url))
			{

				Il.ilSetAlpha(255);// преобразовываем в RGBA
				// если загрузка прошла успешно
				// сохраняем размеры изображения
				int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
				int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);

				// определяем число бит на пиксель
				int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);

				if (prog != null)
				{
					IntPtr ptr = Il.ilGetData();
					prog.Modify(ptr, width, height, bitspp, colorFrom, colorTo);
				}

				TexStruct mGlTextureObject = new TexStruct();
				switch (bitspp) // в зависимости от полученного результата
				{
					// создаем текстуру используя режим GL_RGB или GL_RGBA
					case 24:
						//case 8:
						//case 16:
						mGlTextureObject.Num = MakeGlTexture(GL.RGB, Il.ilGetData(), width, height);
						break;
					case 32:
						mGlTextureObject.Num = MakeGlTexture(GL.RGBA, Il.ilGetData(), width, height);
						break;
				}
				mGlTextureObject.Height = height;
				mGlTextureObject.Width = width;
				mGlTextureObject.BlendParam = opacity ? GL.SRC_ALPHA : GL.ONE;
				mGlTextureObject.refs = 1;// временно! для подсчета количества ссылок на текстуру
				_textures.Add(textureName, mGlTextureObject);

				// активируем флаг, сигнализирующий успешную загрузку текстуры
				r = true;
				// очищаем память
				Il.ilDeleteImages(1, ref imageId);

			}
			return r;
		}


		public override bool LoadTexture(string textureName, string fileName)
		{
			// если текстура уже есть то выходим
			// но перед выходом неплохо бы посмотреть количество ссылок и изменить их количество
			if (_textures.ContainsKey(textureName)) return false;

			bool opacity = true;
			// идентификатор текстуры
			int imageId = 0;

			bool r = false;
			// создаем изображение с идентификатором imageId
			Il.ilGenImages(1, out imageId);
			// делаем изображение текущим
			Il.ilBindImage(imageId);

			// адрес изображения
			string url = fileName;

			// пробуем загрузить изображение);
			if (Il.ilLoadImage(url))
			{
				// если загрузка прошла успешно
				// сохраняем размеры изображения
				int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
				int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);

				// определяем число бит на пиксель
				int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);

				TexStruct mGlTextureObject = new TexStruct();
				switch (bitspp) // в зависимости от полученного результата
				{
					// создаем текстуру используя режим GL_RGB или GL_RGBA
					case 24:
						//case 8:
						//case 16:
						mGlTextureObject.Num = MakeGlTexture(GL.RGB, Il.ilGetData(), width, height);
						break;
					case 32:
						mGlTextureObject.Num = MakeGlTexture(GL.RGBA, Il.ilGetData(), width, height);
						break;
				}
				mGlTextureObject.Height = height;
				mGlTextureObject.Width = width;
				mGlTextureObject.BlendParam = opacity ? GL.SRC_ALPHA : GL.ONE;
				mGlTextureObject.refs = 1;// временно! для подсчета количества ссылок на текстуру
				_textures.Add(textureName, mGlTextureObject);

				// активируем флаг, сигнализирующий успешную загрузку текстуры
				r = true;
				// очищаем память
				Il.ilDeleteImages(1, ref imageId);

			}
			return r;
		}

		protected override void _DrawTexture(int x, int y, string textureName, float scale = 1)
		{
			// проверяем, есть ли текстура. в крайнем случае можно выдать ошибку тут
			if (!_textures.ContainsKey(textureName)) return;
			gl.LoadIdentity();
			int z = 0;
			TexStruct texInfo = _textures[textureName];
			gl.PushAttrib(GL.TEXTURE_2D);
			// включаем режим текстурирования 
			gl.Enable(GL.TEXTURE_2D);
			gl.PushAttrib(GL.BLEND);
			gl.Enable(GL.BLEND);
			//Gl.glBlendFunc(texInfo.BlendParam, Gl.GL_ONE);// (SRC ALPHA или ONE)
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);// (SRC ALPHA или ONE)
			gl.BindTexture(GL.TEXTURE_2D, texInfo.Num);
			int h = (int)(scale * texInfo.Height);// по идее это можно узнать с помощью GL_TEXTURE_WIDTH и HEIGHT
			int w = (int)(scale * texInfo.Width);// но наврядли быстрее - счас без обращения к видеокарте

			// сохраняем состояние матрицы 
			gl.PushMatrix();
			gl.Translated(x, y, 0);
			gl.Rotated(_angle, 0.0f, 0.0f, 1.0f);
			//Gl.glTranslated(0, 0, 0);

			gl.Begin(GL.QUADS);
			// указываем поочередно вершины и текстурные координаты
			gl.TexCoord2f(1, 0); gl.Vertex3d(w / 2, h / 2, z);
			gl.TexCoord2f(1, 1); gl.Vertex3d(w / 2, -h / 2, z);
			gl.TexCoord2f(0, 1); gl.Vertex3d(-w / 2, -h / 2, z);
			gl.TexCoord2f(0, 0); gl.Vertex3d(-w / 2, h / 2, z);

			gl.End();

			gl.Rotated(-_angle, 0.0f, 0.0f, 1.0f);// вращаем всё назад
			// возвращаем матрицу 
			gl.PopMatrix();
			// возвращаем всё в исходное состояние
			gl.PopAttrib();//Gl.GL_BLEND
			gl.PopAttrib();//Gl.GL_TEXTURE_2D
			gl.BlendFunc(GL.ONE, GL.ONE);
		}

		protected override void _DrawTexturePart(int x, int y, string textureName, int blockWidth, int blockHeight, int num)
		{
			if (blockWidth == 0) return;
			if (blockHeight == 0) return;

			// проверяем, есть ли текстура. в крайнем случае можно выдать ошибку тут
			if (!_textures.ContainsKey(textureName)) return;
			gl.LoadIdentity();
			int z = 0;
			TexStruct texInfo = _textures[textureName];
			gl.PushAttrib(GL.TEXTURE_2D);
			// включаем режим текстурирования 
			gl.Enable(GL.TEXTURE_2D);
			gl.PushAttrib(GL.BLEND);
			gl.Enable(GL.BLEND);
			gl.BlendFunc(texInfo.BlendParam, GL.ONE);
			gl.BindTexture(GL.TEXTURE_2D, texInfo.Num);
			int textureHeight = texInfo.Height;// по идее это можно узнать с помощью GL_TEXTURE_WIDTH и HEIGHT
			int textureWidth = texInfo.Width;// но наврядли быстрее - счас без обращения к видеокарте

			// сохраняем состояние матрицы 
			gl.PushMatrix();
			gl.Translated(x, y, 0);
			gl.Rotated(_angle, 0.0f, 0.0f, 1.0f);

			// вычисляем координаты блока
			int cx = textureWidth / blockWidth;// количество блоков по X
			int cy = textureHeight / blockHeight;// количество блоков по Y
			if (cx < 1) return;// если меньше нуля получили 
			if (cy < 1) return;// значит где то что то не то

			int posX = num / cy;
			int posY = num % cy;
			if (posX >= cx) return;// проверяем выходит ли за рамки
			if (posY >= cy) return;// выходит - выходим отсюда

			float x1 = blockWidth * posX;
			float x2 = blockWidth * (posX + 1);
			float y1 = blockHeight * posY;
			float y2 = blockHeight * (posY + 1);

			x1 = x1 / textureWidth;
			x2 = x2 / textureWidth;
			y1 = y1 / textureHeight;
			y2 = y2 / textureHeight;

			gl.Begin(GL.QUADS);
			// указываем поочередно вершины и текстурные координаты
			gl.TexCoord2f(x2, y1); gl.Vertex3d(blockWidth / 2, blockHeight / 2, z);
			gl.TexCoord2f(x2, y2); gl.Vertex3d(blockWidth / 2, -blockHeight / 2, z);
			gl.TexCoord2f(x1, y2); gl.Vertex3d(-blockWidth / 2, -blockHeight / 2, z);
			gl.TexCoord2f(x1, y1); gl.Vertex3d(-blockWidth / 2, blockHeight / 2, z);

			gl.End();

			gl.Rotated(-_angle, 0.0f, 0.0f, 1.0f);// вращаем всё назад
			// возвращаем матрицу 
			gl.PopMatrix();
			// возвращаем всё в исходное состояние
			gl.PopAttrib();//Gl.GL_BLEND
			gl.PopAttrib();//Gl.GL_TEXTURE_2D
			gl.BlendFunc(GL.ONE, GL.ONE);
		}


		protected override void _DrawTextureMasked(int x, int y, string textureName, string textureMaskName)
		{
			// в целом уже ненужна эта функция. потому что PNG прекрасно содержит и прозрачность и почти всё остальное
			TexStruct texInfo = _textures[textureMaskName];
			int h = texInfo.Height;
			int w = texInfo.Width;

			gl.PushAttrib(GL.BLEND);
			gl.PushAttrib(GL.DEPTH_TEST);
			gl.Disable(GL.DEPTH_TEST);
			gl.Enable(GL.BLEND);
			// почти.
			// http://www.opengl.org/archives/resources/faq/technical/transparency.htm
			//Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);		// Blend Screen Color With Zero (Black)
			gl.BlendFunc(GL.DST_COLOR, GL.ZERO);		// Blend Screen Color With Zero (Black)
			gl.BindTexture(GL.TEXTURE_2D, texInfo.Num);	// Select The First Mask Texture
			gl.Begin(GL.QUADS);							// Start Drawing A Textured Quad
			gl.TexCoord2f(1, 0); gl.Vertex3d(x + h, y + w, 0);
			gl.TexCoord2f(1, 1); gl.Vertex3d(x + h, y, 0);
			gl.TexCoord2f(0, 1); gl.Vertex3d(x, y, 0);
			gl.TexCoord2f(0, 0); gl.Vertex3d(x, y + w, 0);
			gl.End();											// Done Drawing The Quad

			texInfo = _textures[textureName];
			h = texInfo.Height;
			w = texInfo.Width;

			gl.BlendFunc(GL.ONE, GL.ONE);				// Copy Image 1 Color To The Screen
			//Gl.glBlendFunc(Gl.GL_ONE, Gl.GL_ONE);				// Copy Image 1 Color To The Screen
			gl.BindTexture(GL.TEXTURE_2D, texInfo.Num);	// Select The First Image Texture
			gl.Begin(GL.QUADS);							// Start Drawing A Textured Quad
			gl.TexCoord2f(1, 0); gl.Vertex3d(x + h, y + w, 0);
			gl.TexCoord2f(1, 1); gl.Vertex3d(x + h, y, 0);
			gl.TexCoord2f(0, 1); gl.Vertex3d(x, y, 0);
			gl.TexCoord2f(0, 0); gl.Vertex3d(x, y + w, 0);
			gl.End();
			gl.PopAttrib();// GL_DEPTH_TEST
			gl.PopAttrib();// GL_BLEND

			/*
			// мультитиnекстурирование
			glActiveTextureARB(GL_TEXTURE0_ARB);
			glEnable(GL_TEXTURE_2D);
			glBindTexture(GL_TEXTURE_2D,texNames[0]);
			glTexEnvi(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
			glMatrixMode(GL_TEXTURE);
			glLoadIdentity();
			glTranslatef(0.5,0.5,0.0);
			glRotatef(45.0,0.0,0.0,1.0);
			glTranslatef(-0.5,-0.5,0.0);
			glMatrixMode(GL_MODELVIEW);
			glActiveTextureARB(GL_TEXTURE1_ARB);
			glEnable(GL_TEXTURE_2D);
			glBindTexture(GL_TEXTURE_2D,texNames[1]);
			glTexEnvi(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
			 
			*/


		}

		public override void LoadFont(string fontName, int fontHeight = 12)
		{
			//LoadTexture(TextureFont, @"..\resources\fonts\TNR_B.tga");
			//LoadTexture(TextureFont, @"TNR_B.tga");
			//LoadTexture(TextureFont, @"TNR_B.png");
			//LoadTextureModify(TextureFont, @"TNR_B.tga", new TPTRounded(), Color.Empty, Color.Empty);
			FontHeight = fontHeight;
			BuildFont(fontName, fontHeight);
		}

		public override void LoadFontTexture(string textureName)
		{
			// или надо переделать или сохранить в архивах и удалить отсюда
			//LoadTexture(TextureFont, textureName);
			//BuildFont();
		}

		private int TextLength(byte[] text)
		{
			float len = text.Sum(с => glyphMetrics[с].gmfCellIncX);
			return (int)(len * FontHeight + 0.5f);
		}

		public override int TextLength(string text)
		{
			// примерно, но в целом пока сойдёт
			//return (text.Length - 1)*FontHeight;
			//float len = text.Sum(с => glyphMetrics[с].gmfCellIncX);
			//return (int) len;
			var w1251Bytes = ConvertEncoding(text);
			return TextLength(w1251Bytes);
		}

		/// <summary>
		/// Вспомогательная функция для конвертирования уникода в другую кодировку и преобразование в массив байтов
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private byte[] ConvertEncoding(string text)
		{
			Encoding w1251 = Encoding.GetEncoding("windows-1251");
			Encoding unicode = Encoding.Unicode;
			byte[] unicodeBytes = unicode.GetBytes(text);
			byte[] w1251Bytes = Encoding.Convert(unicode, w1251, unicodeBytes);
			return w1251Bytes;
		}

		private EncodingInfo enc1 = Encoding.GetEncodings()[0];

		/// <summary>
		/// Вспомогательная функция, для получения нужной кодировки
		/// </summary>
		private void getNextEnc()
		{
			Boolean set = false;
			Boolean set2 = false;
			foreach (EncodingInfo ei in Encoding.GetEncodings())
			{
				if (set)
				{
					enc1 = ei;
					set2 = true;
					break;
				}
				if (ei.Name == enc1.Name)
				{
					set = true;
				}
			}
			// если кодировка не установлена - ставим опять первую
			if (set2 == false && set)
			{
				enc1 = Encoding.GetEncodings()[0];
			}
		}

		protected override void PrintOnly(int x, int y, string text)
		{
			byte[] w1251Bytes = ConvertEncoding(text);

			//Gl.glListBase(_fontBasePtr);
			//Gl.glColor4f(colorR, colorG, colorB, colorA);
			//Gl.glWindowPos2iARB(x, y);
			//Gl.glDisable(Gl.GL_DEPTH_TEST);
			//Gl.glPushAttrib(Gl.GL_BLEND);
			//Gl.glEnable(Gl.GL_BLEND);
			//Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			//Gl.glCallLists(w1251Bytes.Length, Gl.GL_UNSIGNED_BYTE, w1251Bytes);
			//Gl.glDisable(Gl.GL_BLEND);
			//Gl.glEnable(Gl.GL_DEPTH_TEST);
			//Gl.glPopAttrib();

			//return;

			//Gl.glRasterPos2i(x, y);
			//Gl.glColor3b(100,0,100);
			//// в цикле foreach перебираем значения из массива text,
			//// который содержит значение строки для визуализации
			//foreach (var ch in text)
			//{
			//    // визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_HELVETICA_10.
			//    Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_TIMES_ROMAN_24, ch);
			//}

			//return;


			// вывод текста
			gl.PushAttrib(GL.DEPTH_TEST);		// Save the current Depth test settings (Used for blending )
			gl.PushAttrib(GL.TEXTURE_2D);		// Save the current GL_TEXTURE_2D
			gl.PushAttrib(GL.ALPHA_TEST);		// Save the current GL_ALPHA_TEST
			gl.PushAttrib(GL.BLEND);		    // Save the current GL_BLEND
			gl.Disable(GL.DEPTH_TEST);			// Turn off depth testing (otherwise we get no FPS)
			//Gl.glBindTexture(Gl.GL_TEXTURE_2D, _textures[TextureFont].Num);
			gl.Disable(GL.TEXTURE_2D);			// Включаем текстурирование, текстурный текст
			gl.Enable(GL.ALPHA_TEST);
			gl.Enable(GL.BLEND);

			gl.MatrixMode(GL.PROJECTION);		// Switch to the projection matrix
			gl.PushMatrix();						// Save current projection matrix
			gl.LoadIdentity();

			gl.Ortho(0, _formOpenGl.Width, _formOpenGl.Height, 0, -1, 1);
			gl.MatrixMode(GL.MODELVIEW);		// Return to the modelview matrix
			gl.PushMatrix();						// Save the current modelview matrix
			gl.LoadIdentity();

			//Gl.glTranslated(x, y, 0);
			gl.WindowPos2iARB(x, _formOpenGl.Height - y - 16);
			gl.PushAttrib(GL.LIST_BIT);		// Save's the current base list
			//Gl.glEnable(Gl.GL_COLOR_MATERIAL);

			// этот весь код на всякий случай оставить. на будущее 
			//Gl.glEnable(Gl.GL_BLEND);                     // Включаем смешивание
			//Gl.glColor4f(1.0f, 1.0f, 0.0f, 0.5f);
			//Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE);

			//Gl.glColorMaterial(Gl.GL_FRONT_AND_BACK,Gl.GL_SPECULAR);

			//float[] fColor=new float[4]{1.0f,0.0f,0.0f,0.0f};
			//Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE,Gl.GL_BLEND);
			//Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_COLOR,255);

			//Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE);					// Set The Blending Function For Translucency
			//Gl.glEnable(Gl.GL_BLEND);
			//Gl.glShadeModel(Gl.GL_SMOOTH);   // Включение плавного цветового закрашивания
			//Gl.glDisable(Gl.GL_LINE_SMOOTH); // Первоначальное отключение сглаживания линий
			//Gl.glColor3b(100, 0, 0);
			//Gl.glColor4ub(255, 0, 0, 255);
			//colorR, colorG, colorB, colorA);//1.0f, 1.0f, 1.0f, 1.0f);
			//Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_COLOR);
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
			//Gl.glBlendColor(1.0f, 1.0f, 0.0f, 1.0f);
			//Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			//Gl.glColor4f(colorR, colorG, colorB, colorA);

			gl.ListBase((uint)_fontBasePtr);			// Set the base list to our character list
			gl.CallLists(w1251Bytes.Length, GL.UNSIGNED_BYTE, w1251Bytes); // Display the text

			gl.PopAttrib();						// Restore the old base list

			gl.MatrixMode(GL.PROJECTION);		//Switch to projection matrix
			gl.PopMatrix();						// Restore the old projection matrix
			gl.MatrixMode(GL.MODELVIEW);		// Return to modelview matrix
			gl.PopMatrix();	// Restore old modelview matrix
			gl.PopAttrib();	// Restore GL_BLEND
			gl.PopAttrib();	// Restore GL_ALPHA_TEST
			gl.PopAttrib();	// Restore GL_TEXTURE_2D
			gl.PopAttrib();	// Restore GL_DEPTH_TEST
			_textCursorX = x + TextLength(w1251Bytes);
			_textCursorY = y;

		}

		public override void BeginDraw()
		{
			//Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f); 	   // Black Background
			gl.ClearColor(backgroundColorR, backgroundColorG, backgroundColorB, backgroundColorA);
			gl.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);
			gl.LoadIdentity();
			//тут. удостовериться что тут код выполняется.
		}

		public override void FlushDrawing()
		{
			gl.Flush();
			_formOpenGl.rc.SwapBuffers();
			//_formOpenGl.oglView.Invalidate();
		}

		private int _angle = 0;

		/// <summary>
		/// Поворот на угол. в градусах
		/// </summary>
		/// <param name="angle"></param>
		public override void Rotate(int angle)
		{
			_angle += angle;
			//Gl.glRotated(_angle, 0, 0, 1);
		}

		public override void RotateReset()
		{
			//Gl.glRotated(-_angle, 0, 0, 1);
			_angle = 0;
		}

		/// <summary>
		/// указатель на шрифт в памяти, наверное
		/// </summary>
		private int _fontBasePtr = -1;

		/// <summary>
		/// Имя текстуры-шрифта
		/// </summary>
		public const string TextureFont = "Font";

		private int _textCursorX = 0;
		private int _textCursorY = 0;

		// Хранит информацию о шрифте. нужна для вычисления длины текста
		private Gdi.GLYPHMETRICSFLOAT[] glyphMetrics = new Gdi.GLYPHMETRICSFLOAT[256];

		private void BuildFont(string fontName, int fontHeight)
		{
			//IntPtr font;                    
			// Windows Font ID
			//IntPtr oldfont;
			//_fontBasePtr = Gl.glGenLists(257);//Выделим место для 96 символов.

			//font = Gdi.CreateFont(

			//        -24,//Высота фонта.
			//        0,//Ширина фонта.
			//        0,//Угол отношения.
			//        0,//Угол наклона.
			//        Gdi.FW_BOLD,//Ширина шрифта.
			//        false,//Курсив.
			//        false,//Подчеркивание.
			//        false,//Перечеркивание.
			//        Gdi.ANSI_CHARSET,//Идентификатор набора символов.
			//        Gdi.OUT_TT_PRECIS,//Точность вывода.
			//        Gdi.CLIP_DEFAULT_PRECIS,//Точность отсечения.
			//        Gdi.ANTIALIASED_QUALITY,//Качество вывода.
			//        Gdi.FF_DONTCARE | Gdi.DEFAULT_PITCH,//Семейство и шаг.
			//        "Arial"
			//    );//Имя шрифта.
			//IntPtr hDC = Wgl.wglGetCurrentDC();
			//oldfont = Gdi.SelectObject(hDC, font);//Выбрать шрифт, созданный нами.  
			//Wgl.wglUseFontBitmapsW(hDC, 0, 256, _fontBasePtr);                       // Builds 96 Characters Starting At Character 32
			//Gdi.SelectObject(hDC, oldfont);                                     // Selects The Font We Want
			//Gdi.DeleteObject(font);                                             // Delete The Font

			//IntPtr font;
			//_fontBasePtr = Gl.glGenLists(256);
			//font = Gdi.CreateFont(-24, 0, 0, 0, 400, false, false, false, 0, 4, 0, 4, (0 << 4) | 0, "Comic Sans MS");
			//IntPtr dc = Wgl.wglGetCurrentDC();
			//Gdi.SelectObject(dc, font);
			//Wgl.wglUseFontBitmapsW(dc, 32, 256, _fontBasePtr);

			IntPtr font;
			IntPtr oldfont;

			_fontBasePtr = (int)gl.GenLists(256);
			font = Gdi.CreateFont(-fontHeight,
								  0,
								  0,
								  0,
								  Gdi.FW_BOLD,//Gdi.FF_DONTCARE
								  false,
								  false,
								  false,
								  Gdi.DEFAULT_CHARSET,
								  Gdi.OUT_TT_PRECIS,
								  Gdi.CLIP_DEFAULT_PRECIS,
								  Gdi.ANTIALIASED_QUALITY,
								  0,
								  fontName);

			IntPtr dc = Wgl.wglGetCurrentDC();
			oldfont = Gdi.SelectObject(dc, font);
			Wgl.wglUseFontOutlinesA(dc, 0, 256, _fontBasePtr, 0, 0f, Wgl.WGL_FONT_POLYGONS, glyphMetrics);
			Wgl.wglUseFontBitmapsA(dc, 0, 256, _fontBasePtr);

			Gdi.SelectObject(dc, oldfont);
			Gdi.DeleteObject(font);
		}

		private void BuildFont2()
		{
			// если текстура шрифта не задана, значит скорее всего не загружена, значит шрифта не будет
			// или по быстрому можно сварганить обычный шрифт, но без русских букв. но это в будущем
			if (!_textures.ContainsKey(TextureFont)) return;
			float cx; // Holds Our X Character Coord
			float cy; // Holds Our Y Character Coord
			_fontBasePtr = (int)gl.GenLists(256); // Creating 256 Display Lists
			gl.BindTexture(GL.TEXTURE_2D, _textures[TextureFont].Num); // Select Our Font Texture
			for (int loop = 0; loop < 256; loop++) // Loop Through All 256 Lists
			{
				cx = loop % 16 / 16.0f; // X Position Of Current Character
				cy = loop / 16 / 16.0f; // Y Position Of Current Character
				gl.NewList((uint)(_fontBasePtr + loop), GL.COMPILE); // Start Building A List
				gl.Begin(GL.QUADS); // Use A Quad For Each Character
				gl.TexCoord2f(cx, 1 - cy - 0.0625f); // Texture Coord (Bottom Left)
				gl.Vertex2i(0, 16); // Vertex Coord (Bottom Left)
				gl.TexCoord2f(cx + 0.0625f, 1 - cy - 0.0625f); // Texture Coord (Bottom Right)
				gl.Vertex2i(16, 16); // Vertex Coord (Bottom Right)
				gl.TexCoord2f(cx + 0.0625f, 1 - cy); // Texture Coord (Top Right)
				gl.Vertex2i(16, 0); // Vertex Coord (Top Right)
				gl.TexCoord2f(cx, 1 - cy); // Texture Coord (Top Left)
				gl.Vertex2i(0, 0); // Vertex Coord (Top Left)
				gl.End(); // Done Building Our Quad (Character)
				gl.Translated(10, 0, 0); // Move To The Right Of The Character
				gl.EndList(); // Done Building The Display List
			}
		}


		/// <summary>
		/// структура описатель текстуры
		/// </summary>
		private struct TexStruct
		{
			public int Width;
			public int Height;
			/// <summary>
			/// номер от опенГЛ
			/// </summary>
			public uint Num;
			/// <summary>
			/// Смешивание. иногда на что то влияет
			/// </summary>
			public int BlendParam;
			/// <summary>
			/// количество ссылок на текстуру
			/// </summary>
			public int refs;
		}

		/// <summary>
		/// словарь текстур
		/// </summary>
		private readonly Dictionary<String, TexStruct> _textures = new Dictionary<string, TexStruct>();


		// создание текстуры в панями openGL (орфография иногда сохранена 
		// как и у оригинала, ввиду непонятности написанного)
		private static uint MakeGlTexture(int format, IntPtr pixels, int w, int h)
		{
			// идентификатор текстурного объекта 
			uint texObject;
			// генерируем текстурный объект 
			gl.GenTextures(1, out texObject);
			// устанавливаем режим упаковки пикселей 
			gl.PixelStorei(GL.UNPACK_ALIGNMENT, 1);
			// создаем привязку к только что созданной текстуре 
			gl.BindTexture(GL.TEXTURE_2D, texObject);
			// устанавливаем режим фильтрации и повторения текстуры 
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, GL.REPEAT);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, GL.REPEAT);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.LINEAR);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.LINEAR);
			gl.TexEnvf(GL.TEXTURE_ENV, GL.TEXTURE_ENV_MODE, GL.REPLACE);
			// создаем RGB или RGBA текстуру 
			switch (format)
			{
				case GL.RGB:
					gl.TexImage2D(GL.TEXTURE_2D, 0, GL.RGB, w, h, 0, GL.RGB, GL.UNSIGNED_BYTE, pixels);
					break;
				case GL.RGBA:
					gl.TexImage2D(GL.TEXTURE_2D, 0, GL.RGBA, w, h, 0, GL.RGBA, GL.UNSIGNED_BYTE, pixels);
					break;
			}
			// возвращаем идентификатор текстурного объекта 
			return texObject;
		}

		public void DrawCursor(int cursorX, int cursorY)
		{
			gl.MatrixMode(GL.PROJECTION); // Switch to the projection matrix
			gl.PushMatrix(); // Save current projection matrix
			gl.LoadIdentity();
			//Gl.glOrtho(0, TAOWindow.Width, 0, TAOWindow.Height, -1, 1);
			gl.Ortho(0, _formOpenGl.Width, _formOpenGl.Height, 0, -1, 1);
			gl.MatrixMode(GL.MODELVIEW); // Return to the modelview matrix
			gl.PushMatrix(); // Save the current modelview matrix
			gl.LoadIdentity();

			DrawTexture(cursorX, cursorY, "Cursor");
			//ShowMasked(cursorX, cursorY, "Pic1", "Pic1m");

			gl.MatrixMode(GL.PROJECTION); //Switch to projection matrix
			gl.PopMatrix(); // Restore the old projection matrix
			gl.MatrixMode(GL.MODELVIEW); // Return to modelview matrix
			gl.PopMatrix(); // Restore old modelview matrix

			double[] modelview = new double[16]; gl.GetDoublev(GL.MODELVIEW_MATRIX, modelview);
			double[] projection = new double[16]; gl.GetDoublev(GL.PROJECTION_MATRIX, projection);
			int[] viewport = new int[4]; gl.GetIntegerv(GL.VIEWPORT, viewport);

			#region Вычисление Координат курсора в OpenGL
			//double rx;
			//double ry;
			//double rz;
			//Glu.gluUnProject(MouseX, _formOpenGl.oglView.Height - MouseY, 0, modelview, projection, viewport, out rx, out ry, out rz);
			//float x = (float)rx;
			//float y = (float)ry;
			//float z = (float)rz;
			//float curSize = 10;
			//SetColor(Color.White);
			//Line3D(new TPoint3(x - curSize, y - curSize, 1), new TPoint3(x + curSize, y + curSize, 1));
			//Line3D(new TPoint3(x - curSize, y + curSize, 1), new TPoint3(x + curSize, y - curSize, 1));
			//SetColor(Color.LightGray);
			//Print(30, 80, "OpenGL Coordinates ( " + (x) + " , " + y + " , " + z + " ).");
			//SetColor(Color.Gray);
			//Print(30, 100, "Mouse ( " + cursorX + " , " + cursorY + " ).");
			//Print(30, 120, "Window " + "(" + _oglView.Width + " , " + _oglView.Height + ")."); 
			#endregion
		}
	}

}
