using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Engine.Utils.ExtensionMethods;

namespace GMTubes
{
	/// <summary>
	/// Поле
	/// </summary>
	class Field
	{
		public const int MaxWidth = 30;
		public const int MaxHeight = 20;

		public int MaxW;
		public int MaxH;

		public FieldPoint[,] Data=null;
		/// <summary>
		/// Выводим ошибку сюда при создании поля
		/// </summary>
		public string Error = "";

		private Random _rnd = new Random();

		public Field()
		{
			Data = GenerateField(10, 7, 5, 10, 2);
		}

		/// <summary>
		/// Генерация поля
		/// </summary>
		/// <param name="maxWidth">ширина поля</param>
		/// <param name="maxHeight">высота поля</param>
		/// <param name="count4">количество элементов с четыремя связями</param>
		/// <param name="count3">количество элементов с тремя связями</param>
		/// <param name="holes">Количество дырок на поле</param>
		public FieldPoint[,] GenerateField(int maxWidth, int maxHeight, int count4, int count3, int holes)
		{
			MaxW = maxWidth;
			MaxH = maxHeight;
			if (maxWidth > MaxWidth) return null;
			if (maxHeight > MaxHeight) return null;

			var f = new FieldPoint[MaxWidth, MaxHeight];
			// заполняем всё поле элементами и устанавливаем связи и корректируем граничные элементы
			Fill1(f, maxWidth, maxHeight);
			// добавляем дырки
			//for (int i = 0; i < holes; i++){CreateHole(f);}
			// выбираем 4х связные точки и сохраняем их состояние
			for (int i = 0; i < count4;i++){Create4xFieldPoint(f); }
			// проходим по всем объектам и если это 4х связная точка и ещё не статичная - уменьшаем ей количество связей
			ReduceCount4x(f);
			// выбираем 3х связные точки и сохраняем их состояние
			for (int i = 0; i < count3; i++) { Create3xFieldPoint(f); }
			// проходим по всем объектам и уменьшаем количество связей если возможно до 2х
			ReduceCount3x(f);
			// избавляемся от разрывов, что бы можно было перейти от всех объектов ко всем остальным
			ReduceGap(f, maxWidth, maxHeight, holes);
			InitInfo(f);
			return f;
		}

		private void ReduceGap(FieldPoint[,] fieldPoints, int maxWidth, int maxHeight, int holes)
		{
			var h = 0;// сейчас дырки не делаем, поэтому и не надо их учитывать
			int count;
			do{// рекурсивная функция, отмечает все точки с которыми связана переданная
				var f = new Boolean[MaxWidth, MaxHeight];
				ReduceGap1(f, fieldPoints, fieldPoints[0, 0]);
				count = 0;
				for (int i = 0; i < MaxWidth; i++){
					for (int j = 0; j < MaxHeight; j++){
						if (f[i, j]) count++;
					}
				}
				// если количество отмеченных точек меньше общего количества точек, значит не до всех точек можно дойти и надо установить связь
				if (count < maxWidth*maxHeight - h) 
					ReduceGapAddLink(f, fieldPoints, maxWidth, maxHeight);
				else break; // раз равно - прерываем цикл
			} while	(count > 0);
		}

		private Boolean ReduceGapAddLink(bool[,] f, FieldPoint[,] fieldPoints, int maxWidth, int maxHeight)
		{
			int c=0;
			var r = false;// ненашли
			do{
				c++;if (c > 100) break;

				var x = _rnd.Next(maxWidth);
				var y = _rnd.Next(maxHeight);
				var fb = f[x, y];
				if (fb == false) continue;// точка не отмечена - ищем дальше отмеченную
				var fp = fieldPoints[x, y];
				if (fp == null) continue;
				if (fp.countLinksAvailable == 4) continue; // все связи уже есть, выходим

				var l = false;
				FieldPoint fp2 = null;
				if (!l&&fp.LinkDown == null && fp.j+1<=maxHeight && !f[fp.i, fp.j + 1]){// есть неотмеченная область - создаём связь
					fp2 = fieldPoints[fp.i, fp.j + 1];
					if (fp2!=null&& fp2.countLinksAvailable != 4){// если можно создать связь - создаём
						fp.LinkDown = fp2;
						fp.countLinksAvailable++;
						fp2.LinkUp = fp;
						fp2.countLinksAvailable++;
						l = true;
					}
				}
				if (!l&&fp.LinkUp == null &&(fp.j - 1>=0)&& !f[fp.i, fp.j - 1]){// есть неотмеченная область - создаём связь
					fp2 = fieldPoints[fp.i, fp.j - 1];
					if (fp2!=null&& fp2.countLinksAvailable != 4){// если можно создать связь - создаём
						fp.LinkUp = fp2;
						fp.countLinksAvailable++;
						fp2.LinkDown = fp;
						fp2.countLinksAvailable++;
						l = true;
					}
				}
				if (!l&&fp.LinkLeft == null && (fp.i+1<=maxWidth) && !f[fp.i+1, fp.j]){// есть неотмеченная область - создаём связь
					fp2 = fieldPoints[fp.i+1, fp.j];
					if (fp2!=null&&fp2.countLinksAvailable != 4){// если можно создать связь - создаём
						fp.LinkLeft = fp2;
						fp.countLinksAvailable++;
						fp2.LinkRight = fp;
						fp2.countLinksAvailable++;
						l = true;
					}
				}
				if (!l&&fp.LinkRight == null &&(fp.i-1>=0)&& !f[fp.i-1, fp.j]){// есть неотмеченная область - создаём связь
					fp2 = fieldPoints[fp.i-1, fp.j];
					if (fp2!=null&&fp2.countLinksAvailable != 4){// если можно создать связь - создаём
						fp.LinkRight = fp2;
						fp.countLinksAvailable++;
						fp2.LinkLeft = fp;
						fp2.countLinksAvailable++;
						l = true;
					}
				}

				if (!l) continue;
				r = true;
			} while (!r);
			return r;
		}

		private void ReduceGap1(bool[,] f, FieldPoint[,] fieldPoints, FieldPoint fp)
		{
			if (f[fp.i, fp.j]) return;// точка уже отмечена - выходим
			f[fp.i, fp.j] = true;// отммечаем точку. раз добрались до неё значит путь есть
			if (fp.LinkLeft != null) { ReduceGap1(f, fieldPoints, fieldPoints[fp.i + 1, fp.j]); }
			if (fp.LinkRight != null) { ReduceGap1(f, fieldPoints, fieldPoints[fp.i - 1, fp.j]); }
			if (fp.LinkUp != null){ ReduceGap1(f, fieldPoints, fieldPoints[fp.i, fp.j - 1]); }
			if (fp.LinkDown != null){ ReduceGap1(f, fieldPoints, fieldPoints[fp.i, fp.j + 1]); }
		}

		private void InitInfo(FieldPoint[,] fieldPoints)
		{
			for (int i = 0; i < MaxWidth; i++){
				for (int j = 0; j < MaxHeight; j++){
					var f = fieldPoints[i, j];
					if (f==null)continue;
					f.InitInfo();
					for (int k = 0; k < _rnd.Next(4); k++){
						f.Rotate();
					}
				}
			}
		}

		private void ReduceCount3x(FieldPoint[,] f)
		{
			for (int i = 0; i < MaxWidth; i++){
				for (int j = 0; j < MaxHeight; j++){
					var f1 = f[i, j];
					if (f1 == null) continue;
					if (f1.IsStatic) continue;
					var cla1 = f1.countLinksAvailable;
					if (cla1 < 3) continue;// не статичная, связей 3 или 4 - уменьшаем количество связей
					var nonStaticLink = 0;// проверяем количество нестатичных связей, которые можно будет удалить
					if (f1.LinkDown != null && f1.LinkDown.IsStatic == false) nonStaticLink++;
					if (f1.LinkUp != null && f1.LinkUp.IsStatic == false) nonStaticLink++;
					if (f1.LinkLeft != null && f1.LinkLeft.IsStatic == false) nonStaticLink++;
					if (f1.LinkRight != null && f1.LinkRight.IsStatic == false) nonStaticLink++;
					if (nonStaticLink < 1) continue;// нету нестатичных связей - ничего не меняем
					while (f1.countLinksAvailable == cla1)
					{
						ClearLink(f1, _rnd.Next(4));
					}
				}
			}
		}

		private void ReduceCount4x(FieldPoint[,] f)
		{
			for (int i = 0; i < MaxWidth; i++)
			{
				for (int j = 0; j < MaxHeight; j++)
				{
					var f1 = f[i, j];
					if (f1 == null) continue;
					if (f1.IsStatic) continue;
					var cla1 = f1.countLinksAvailable;
					if (cla1 < 4) continue;// не статичная, связей 4 - уменьшаем количество связей
					while (f1.countLinksAvailable==cla1){
						ClearLink(f1, _rnd.Next(4));
					}
				}
			}
		}

		private bool ClearLink(FieldPoint f1, int numLink)
		{
			var fp = f1;
			if (fp.countLinksAvailable < 3) return false;
			if (numLink == 0){
				if (fp.LinkDown == null) return false;
				if (fp.LinkDown.countLinksAvailable<3) return false;
				if (fp.LinkDown.LinkUp == null) return false;
				fp.LinkDown.LinkUpNull();
				fp.LinkDownNull();
				return true;
			}
			if (numLink == 1){
				if (fp.LinkLeft == null) return false;
				if (fp.LinkLeft.countLinksAvailable<3) return false;
				if (fp.LinkLeft.LinkRight == null) return false;
				fp.LinkLeft.LinkRightNull();
				fp.LinkLeftNull();
				return true;
			}
			if (numLink == 2){
				if (fp.LinkRight == null) return false;
				if (fp.LinkRight.countLinksAvailable<3) return false;
				if (fp.LinkRight.LinkLeft == null) return false;
				fp.LinkRight.LinkLeftNull();
				fp.LinkRightNull();
				return true;
			}
			if (numLink == 3){
				if (fp.LinkUp == null) return false;
				if (fp.LinkUp.countLinksAvailable<3) return false;
				if (fp.LinkUp.LinkDown == null) return false;
				fp.LinkUp.LinkDownNull();
				fp.LinkUpNull();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Текущее количество 4х связных точек на поле
		/// </summary>
		/// <param name="f"></param>
		/// <param name="maxWidth"></param>
		/// <param name="maxHeight"></param>
		/// <returns></returns>
		private int Count4xFieldPoint(FieldPoint[,] f, int maxWidth, int maxHeight)
		{
			var ret = 0;
			for (int i = 0; i < maxWidth; i++){
				for (int j = 0; j < maxHeight; j++){
					var f1 = f[i, j];
					if (f1 == null) continue;
					if (f1.countLinksAvailable<4)continue;
					ret++;
				}
			}
			return ret;
		}

		/// <summary>
		/// Текущее количество 4х связных точек на поле
		/// </summary>
		/// <param name="f"></param>
		/// <param name="maxWidth"></param>
		/// <param name="maxHeight"></param>
		/// <returns></returns>
		private int Count3xFieldPoint(FieldPoint[,] f, int maxWidth, int maxHeight)
		{
			var ret = 0;
			for (int i = 0; i < maxWidth; i++){
				for (int j = 0; j < maxHeight; j++){
					var f1 = f[i, j];
					if (f1 == null) continue;
					if (f1.countLinksAvailable < 3) continue;
					ret++;
				}
			}
			return ret;
		}

		private void Create4xFieldPoint(FieldPoint[,] f)
		{
			var r = false;// ненашли
			do
			{
				var x = _rnd.Next(MaxWidth);
				var y = _rnd.Next(MaxHeight);
				var fp = f[x, y];
				if (fp == null) continue;
				if (fp.IsStatic) continue;
				if (fp.countLinksAvailable < 4) continue; // 
				fp.IsStatic = true;// сохраняем объект как 4х связную точку
				r = true;
			} while (!r);		
		}

		private void Create3xFieldPoint(FieldPoint[,] f)
		{
			var r = false;// ненашли
			do
			{
				var x = _rnd.Next(MaxWidth);
				var y = _rnd.Next(MaxHeight);
				var fp = f[x, y];
				if (fp == null) continue;
				if (fp.IsStatic) continue;
				if (fp.countLinksAvailable !=3) continue; // 
				fp.IsStatic = true;// сохраняем объект как 3х связную точку
				r = true;
			} while (!r);
		}

		private static void Fill1(FieldPoint[,] f, int maxWidth, int maxHeight)
		{
			for (int i = 0; i < maxWidth; i++){
				for (int j = 0; j < maxHeight; j++){
					var fp = new FieldPoint();
					fp.i = i;
					fp.j = j;
					f[i, j] = fp;
				}
			}
			for (int i = 1; i < maxWidth; i++){
				for (int j = 0; j < maxHeight; j++){
					f[i - 1, j].LinkLeft = f[i, j];
				}
			}
			for (int i = 0; i < maxWidth; i++){
				for (int j = 1; j < maxHeight; j++){
					f[i, j-1].LinkDown = f[i, j];
				}
			}
			for (int i = 0; i < maxWidth-1; i++){
				for (int j = 0; j < maxHeight; j++){
					f[i +1, j].LinkRight = f[i, j];
				}
			}
			for (int i = 0; i < maxWidth; i++){
				for (int j = 0; j < maxHeight-1; j++){
					f[i, j+1].LinkUp = f[i, j];
				}
			}
			// уменьшаем количество связей у граничных элементов
			for (int i = 0; i < maxWidth; i++){
				var f1=f[i, 0];
				f1.countLinksAvailable--;
				if (f1.countLinksAvailable == 2) f1.IsStatic = true;
			}
			for (int i = 0; i < maxWidth; i++){
				var f1 = f[i, maxHeight - 1];
				f1.countLinksAvailable--;
				if (f1.countLinksAvailable == 2) f1.IsStatic = true;
			}

			for (int i = 0; i < maxHeight; i++)
			{
				var f1 = f[0, i];
				f1.countLinksAvailable--;
				if (f1.countLinksAvailable == 2) f1.IsStatic = true;
			}
			for (int i = 0; i < maxHeight; i++){
				var f1 = f[maxWidth - 1, i];
				f1.countLinksAvailable--;
				if (f1.countLinksAvailable == 2) f1.IsStatic = true;
			}

		}

		/// <summary>
		/// Создать дырку на поле
		/// </summary>
		/// <param name="f"></param>
		private void CreateHole(FieldPoint[,] f)
		{
			var r = false;// ненашли
			do{
				var x = _rnd.Next(MaxWidth);
				var y = _rnd.Next(MaxHeight);
				var fp = f[x, y];
				var p3 = Count3xFieldPoint(f, MaxWidth, MaxHeight);
				var p4 = Count4xFieldPoint(f, MaxWidth, MaxHeight);
				if (fp == null) continue;
				if (fp.IsStatic) continue;
				if (fp.LinkDown != null && fp.LinkDown.countLinksAvailable < 3) continue; // у прилегающих 
				if (fp.LinkUp != null && fp.LinkUp.countLinksAvailable < 3) continue;
				if (fp.LinkLeft != null && fp.LinkLeft.countLinksAvailable < 3) continue;
				if (fp.LinkRight != null && fp.LinkRight.countLinksAvailable < 3) continue;
				// удаляем связи и удаляем саму точку
				ClearLink(fp, 0);
				ClearLink(fp, 1);
				ClearLink(fp, 2);
				ClearLink(fp, 3);
				f[x, y] = null;
				r = true;
			} while (!r);
		}
	}
}
