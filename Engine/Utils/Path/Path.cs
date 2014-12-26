using System;
using System.Collections;
using System.Collections.Generic;

namespace Engine.Utils.Path
{
	/// <summary>
	/// Содержит опорные точки пути
	/// </summary>
	public class Path
	{
		/// <summary>
		/// Точки пути
		/// </summary>
		private List<Point> _points = new List<Point>();

		public int num = 0;
		static public Random rnd = new Random();

		public IEnumerable Points()
		{
			foreach (Point p in _points)
			{
				yield return p;
			}
		}

		public Point this[int i]
		{
			get{
				var n = i;
				while (n>_points.Count){n -= _points.Count;}
				return _points[n];
			}
			set{
				var n = i;
				while (n > _points.Count){n -= _points.Count;}
				_points[n] = value;
			}
		}

		/// <summary>
		/// Нормализуем точки. в данном случае удаляем соседние точки с одинаковыми координатами
		/// </summary>
		public void Normalize()
		{
			// последнюю точку не удаляем. даже если она совпадает с первой - это позволит сделать замкнутый путь
			var c = _points.Count;
			if (c == 0) return;
			for (int i = c-1; i > 0; i--){// удаляем дублирующую точку в списке
				if (_points[i] == _points[i - 1]) _points.Remove(_points[i]);
			}
		}

		/// <summary>
		/// Добавить точки пути. для всех точек одинаковое разбитие и один и тот же генератор
		/// </summary>
		/// <param name="basePoints">Опорные точки</param>
		/// <param name="count">На сколько точек разделять отдельный путь</param>
		/// <param name="pathGenerator">Генератор</param>
		/// <remarks>Конечные точки участков путей должны повторяться</remarks>
		public void AddPoints(List<Point> basePoints, int count, PathGenerator pathGenerator)
		{
			var num = pathGenerator.CountBasePoints();
			int i = 0;
			while (i>=basePoints.Count){
				var pts = basePoints.GetRange(i, num);
				AddPointsOneSegment(pts,count,pathGenerator);
				i += num;
			}
		}

		/// <summary>
		/// Добавить точки пути одного сегмента (что бы обеспечить более гибкую добавку и возможность внешнего редактирования)
		/// </summary>
		/// <param name="basePoints">Опорные точки</param>
		/// <param name="count">На сколько точек разделять отдельный путь</param>
		/// <param name="pathGenerator">Генератор</param>
		public void AddPointsOneSegment(List<Point> basePoints, int count, PathGenerator pathGenerator)
		{
			var points = pathGenerator.Generate(basePoints, count);
			_points.AddRange(points);// сгенерировани и добавляем полученные точки
		}

	}
}
