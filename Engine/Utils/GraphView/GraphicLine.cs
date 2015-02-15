using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace Engine.Utils.GraphView
{
	/// <summary>
	/// Строка графика
	/// </summary>
	public class GraphicLine
	{
		public GraphicLine()
		{
			Points = new List<PointF>();
		}

		/// <summary>
		/// Масштаб определяет коэффициент умножения для линии
		/// </summary>
		public float Scale = 1;

		/// <summary>
		/// Масштаб определяет принадлежность к группе и максимальное и минимальное значение у шкалы
		/// </summary>
		public float scaleTopX = 1;
		public float scaleTopY = 1;
		public float scaleBtmX = 0;
		public float scaleBtmY = 0;

		/// <summary>
		/// Цвет линии. по умолчанию белый
		/// </summary>
		public Color Color = Color.White;

		/// <summary>
		/// Видимость линии
		/// </summary>
		public Boolean Visible = true;
		/// <summary>
		/// Группа линии. ГРУППИРОВКУ ПОКА НЕ ИСПОЛЬЗОВАТЬ! в общем случае в группу должны входить линии у которых границы графика примерно одинаковые
		/// </summary>
		/// <remarks>
		/// если группа одинаковая, то она выводится вместе с другими в соответствии с её размерами 
		/// если группа другая то выводится с учетом масштаба и с отдельной шкалой
		/// </remarks>
		public int Group = 0;
		public String LineName = "Unknown";

		public float minX;
		public float minY;
		public float maxX;
		public float maxY;

		public List<PointF> Points { get; protected set; }
		
		/// <summary>
		/// Точки пересчитанные для вывода на экран
		/// </summary>
		public List<PointF> PointsView = new List<PointF>();

		public void Add(PointF point)
		{
			Points.Add(point);// добавляем и сразу сортируем
			Points = Points.OrderBy(obj => obj.X).ToList();
			SetScale(point);
		}

		public void AddValue(float value)
		{
			var p = new PointF(Points.Count, value);
			Points.Add(p);
			var pts = Points.OrderBy(obj => obj.Y);
			Points=new List<PointF>(pts);
			SetScale(p);
		}

		private void SetScale(PointF point)
		{
			if (Points.Count == 1){
				minX = point.X;
				maxX = point.X;
				minY = point.Y;
				maxY = point.Y;
				return;
			}
			if (point.X < minX) minX = point.X;
			if (point.X > maxX) maxX = point.X;
			if (point.Y < minY) minY = point.Y;
			if (point.Y > maxY) maxY = point.Y;
		}
	}
}
