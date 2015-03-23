using System;

namespace GMTubes.Model
{
	/// <summary>
	/// Точка на поле
	/// </summary>
	class FieldPoint
	{
		/// <summary>
		/// Окончательное ли значение у точки
		/// </summary>
		public Boolean IsStatic { get; set; }

		public Boolean Broken = false;

		private float _addToRotate = 0;

		/// <summary>
		/// Ссылки на соседние ячейки
		/// </summary>
		public FieldPoint LinkUp = null;
		public FieldPoint LinkDown = null;
		public FieldPoint LinkLeft = null;
		public FieldPoint LinkRight = null;

		public int i;
		public int j;

		/// <summary>
		/// Количество установленных связей
		/// </summary>
		public int countLinks = 0;
		/// <summary>
		/// Количество доступных для установления связей
		/// </summary>
		public int countLinksAvailable = 4;

		private void SetIsStatic()
		{
			if (IsStatic || countLinksAvailable > 2)return;
			IsStatic=true;
		}

		public Boolean LinkUpNull()
		{
			if (IsStatic) return false;
			if (LinkUp == null) return false;
			countLinksAvailable--;
			LinkUp = null;
			SetIsStatic();
			return true;
		}

		public Boolean LinkDownNull()
		{
			if (IsStatic) return false;
			if (LinkDown == null) return false;
			countLinksAvailable--;
			LinkDown = null;
			SetIsStatic();
			return true;
		}
		public Boolean LinkLeftNull()
		{
			if (IsStatic) return false;
			if (LinkLeft == null) return false;
			countLinksAvailable--;
			LinkLeft = null;
			SetIsStatic();
			return true;
		}
		public Boolean LinkRightNull()
		{
			if (IsStatic) return false;
			if (LinkRight == null) return false;
			countLinksAvailable--;
			LinkRight = null;
			SetIsStatic();
			return true;
		}

		/// <summary>
		/// По умолчанию поворот = 0 и это положение решения (но потом это может быть будет изменено)
		/// </summary>
		public int angle = 0;

		public int currentAngle = 0;

		/// <summary>
		/// Номер текстуры
		/// </summary>
		public int texnum = -1;

		/// <summary>
		/// Установка начальных значений
		/// </summary>
		public void InitInfo()
		{
			var num = countLinksAvailable;
			if (num == 4) texnum = 1;
			if (num == 3){// нужно доопределить на какой угол поворачивать
				texnum = 2;angle = 0;
				if (LinkUp == null) angle = 1;
				if (LinkRight == null) angle = 2;
				if (LinkDown == null) angle = 3;
			}
			if (num == 2){// тут сложнее - надо определиться, прямая труба или изогнутая
				if (LinkDown != null && LinkUp != null) texnum = 3;
				else if (LinkLeft != null && LinkRight != null){
					texnum = 3;
					angle = 1;
				}else{
					texnum = 4; angle = 3;
					if (LinkLeft == null && LinkDown == null) angle = 0;
					if (LinkUp == null && LinkLeft == null) angle = 1;
					if (LinkUp == null && LinkRight == null) angle = 2;
				}
			}
			currentAngle = angle;
		}

		/// <summary>
		/// Вращаем точку
		/// </summary>
		public void Rotate()
		{
			_addToRotate -= (90 /*+ 360*/);
			if (texnum == 1) return;// объект с четыремя связями не вращается
			currentAngle++;
			if (texnum == 3){
				if (currentAngle>1)currentAngle = 0;
				return;
			}
			if (currentAngle > 3) currentAngle = 0;
		}

		public int AddRotate()
		{
			if (_addToRotate == 0) return 0;
			var n = _addToRotate/18;
			if (_addToRotate < -10) n *= 3;
			_addToRotate -= n;
			if (_addToRotate >- 2) _addToRotate = 0;
			return (int)_addToRotate;
		}

	}
}
