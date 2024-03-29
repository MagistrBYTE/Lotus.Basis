using System;

using Lotus.Core;
using Lotus.Maths;

namespace Lotus.Algorithm
{
    /**
     * \defgroup AlgorithmSnap Алгоритмы привязки пространственных данных
     * \ingroup Algorithm
     * \brief Подсистема алгоритмов для привязки пространственных данных.
	 * \details Алгоритмы привязки пространственных данных обеспечивают привязку объекта к определенному узлу, 
		то есть позволяют найти ближайший соответствующий объект в зависимости от различных критериев.
     * @{
     */
    /// <summary>
    /// Точка привязки в двухмерном пространстве.
    /// </summary>
    public struct TSnapPoint2D : IEquatable<TSnapPoint2D>, IComparable<TSnapPoint2D>
    {
        #region Static fields
        /// <summary>
        /// Текстовый формат отображения параметров точки.
        /// </summary>
        public static string ToStringFormat = "X = {0:0.00}; Y = {1:0.00}";

        /// <summary>
        /// Текстовый формат отображения только значений параметров точки.
        /// </summary>
        public static string ToStringFormatValue = "{0:0.00}; {1:0.00}";
        #endregion

        #region Fields
        /// <summary>
        /// Опорная точка.
        /// </summary>
        public Vector2Df Point;

        /// <summary>
        /// Дистанция до этой точки.
        /// </summary>
        public float Distance;
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор инициализирует точку привязки указанными параметрами.
        /// </summary>
        /// <param name="x">X - координата.</param>
        /// <param name="y">Y - координата.</param>
        public TSnapPoint2D(float x, float y)
        {
            Point.X = x;
            Point.Y = y;
            Distance = 0;
        }

        /// <summary>
        /// Конструктор инициализирует точку привязки указанными параметрами.
        /// </summary>
        /// <param name="point">Точка.</param>
        public TSnapPoint2D(Vector2Df point)
        {
            Point = point;
            Distance = 0;
        }

#if UNITY_2017_1_OR_NEWER
		/// <summary>
		/// Конструктор инициализирует точку привязки указанными параметрами.
		/// </summary>
		/// <param name="vector">Вектор.</param>
		public TSnapPoint2D(UnityEngine.Vector2 vector)
		{
			Point.X = vector.x;
			Point.Y = vector.y;
			Distance = 0;
		}
#endif

        /// <summary>
        /// Конструктор инициализирует точку привязки указанными параметрами.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="distance">Дистанция до этой точки.</param>
        public TSnapPoint2D(Vector2Df point, float distance)
        {
            Point = point;
            Distance = distance;
        }
        #endregion

        #region System methods
        /// <summary>
        /// Проверяет равен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="obj">Сравниваемый объект.</param>
        /// <returns>Статус равенства объектов.</returns>
        public override readonly bool Equals(object? obj)
        {
            if (obj is TSnapPoint2D snap_point)
            {
                return Equals(snap_point);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Проверка равенства точек привязок по значению.
        /// </summary>
        /// <param name="other">Сравниваемая точка привязки.</param>
        /// <returns>Статус равенства точек привязок.</returns>
        public readonly bool Equals(TSnapPoint2D other)
        {
            return Point == other.Point;
        }

        /// <summary>
        /// Сравнение точек привязок для упорядочивания.
        /// </summary>
        /// <param name="other">Точка привязки.</param>
        /// <returns>Статус сравнения точек привязок.</returns>
        public readonly int CompareTo(TSnapPoint2D other)
        {
            return Distance.CompareTo(other.Distance);
        }

        /// <summary>
        /// Получение хеш-кода точки привязки.
        /// </summary>
        /// <returns>Хеш-код точки привязки.</returns>
        public override readonly int GetHashCode()
        {
            return Point.GetHashCode() ^ Distance.GetHashCode();
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <returns>Текстовое представление точки привязки с указанием значений координат.</returns>
        public override readonly string ToString()
        {
            return string.Format(ToStringFormat, Point.X, Point.Y);
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <param name="format">Формат отображения.</param>
        /// <returns>Текстовое представление точки привязки с указанием значений координат.</returns>
        public readonly string ToString(string format)
        {
            return "X = " + Point.X.ToString(format) + "; Y = " + Point.Y.ToString(format);
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <returns>Текстовое представление точки привязки с указанием значений координат.</returns>
        public readonly string ToStringValue()
        {
            return string.Format(ToStringFormatValue, Point.X, Point.Y);
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <param name="format">Формат отображения компонентов точки привязки.</param>
        /// <returns>Текстовое представление точки привязки с указанием значений координат.</returns>
        public readonly string ToStringValue(string format)
        {
            return string.Format(ToStringFormatValue.Replace("0.00", format), Point.X, Point.Y);
        }
        #endregion

        #region Operators 
        /// <summary>
        /// Сравнение объектов на равенство.
        /// </summary>
        /// <param name="left">Первый объект.</param>
        /// <param name="right">Второй объект.</param>
        /// <returns>Статус равенства.</returns>
        public static bool operator ==(TSnapPoint2D left, TSnapPoint2D right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Сравнение объектов на неравенство.
        /// </summary>
        /// <param name="left">Первый объект.</param>
        /// <param name="right">Второй объект.</param>
        /// <returns>Статус неравенство.</returns>
        public static bool operator !=(TSnapPoint2D left, TSnapPoint2D right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Сравнение объектов по операции меньше.
        /// </summary>
        /// <param name="left">Первый объект.</param>
        /// <param name="right">Второй объект.</param>
        /// <returns>Статус операции.</returns>
        public static bool operator <(TSnapPoint2D left, TSnapPoint2D right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Сравнение объектов по операции меньше или равно.
        /// </summary>
        /// <param name="left">Первый объект.</param>
        /// <param name="right">Второй объект.</param>
        /// <returns>Статус операции.</returns>
        public static bool operator <=(TSnapPoint2D left, TSnapPoint2D right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Сравнение объектов по операции больше.
        /// </summary>
        /// <param name="left">Первый объект.</param>
        /// <param name="right">Второй объект.</param>
        /// <returns>Статус операции.</returns>
        public static bool operator >(TSnapPoint2D left, TSnapPoint2D right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Сравнение объектов по операции больше или равно.
        /// </summary>
        /// <param name="left">Первый объект.</param>
        /// <param name="right">Второй объект.</param>
        /// <returns>Статус операции.</returns>
        public static bool operator >=(TSnapPoint2D left, TSnapPoint2D right)
        {
            return left.CompareTo(right) >= 0;
        }
        #endregion

        #region Main methods
        /// <summary>
        /// Вычисление дистанции до указанной точки.
        /// </summary>
        /// <param name="point">Точка.</param>
        public void ComputeDistance(ref Vector2Df point)
        {
            Distance = Vector2Df.Distance(in Point, in point);
        }

#if UNITY_2017_1_OR_NEWER
		/// <summary>
		/// Вычисление дистанции до указанной точки.
		/// </summary>
		/// <param name="vector">Точка.</param>
		public void ComputeDistance(ref UnityEngine.Vector2 vector)
		{
			Single x = Point.X - vector.x;
			Single y = Point.Y - vector.y;

			Distance = UnityEngine.Mathf.Sqrt(x * x + y * y);
		}
#endif

        /// <summary>
        /// Аппроксимация равенства значений точки.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="deltaX">Допуск по координате X.</param>
        /// <param name="deltaY">Допуск по координате Y.</param>
        /// <returns>Статус равенства значений.</returns>
        public readonly bool ApproximatelyPoint(ref Vector2Df point, float deltaX, float deltaY)
        {
            if (Math.Abs(Point.X - point.X) < deltaX && Math.Abs(Point.Y - point.Y) < deltaY)
            {
                return true;
            }

            return false;
        }

#if UNITY_2017_1_OR_NEWER
		/// <summary>
		/// Аппроксимация равенства значений точки.
		/// </summary>
		/// <param name="vector">Точка.</param>
		/// <param name="delta_x">Допуск по координате X.</param>
		/// <param name="delta_y">Допуск по координате Y.</param>
		/// <returns>Статус равенства значений.</returns>
		public Boolean ApproximatelyPoint(ref UnityEngine.Vector2 vector, Single delta_x, Single delta_y)
		{
			if (Math.Abs(Point.X - vector.x) < delta_x && Math.Abs(Point.Y - vector.y) < delta_y)
			{
				return true;
			}

			return false;
		}
#endif

        /// <summary>
        /// Аппроксимация равенства значений точки по координате X.
        /// </summary>
        /// <param name="x">Координата точки по X.</param>
        /// <param name="epsilon">Погрешность.</param>
        /// <returns>Статус равенства значений.</returns>
        public readonly bool ApproximatelyPointX(float x, float epsilon)
        {
            if (Math.Abs(Point.X - x) < epsilon)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Аппроксимация равенства значений точки по координате Y.
        /// </summary>
        /// <param name="y">Координата точки по Y.</param>
        /// <param name="epsilon">Погрешность.</param>
        /// <returns>Статус равенства значений.</returns>
        public readonly bool ApproximatelyPointY(float y, float epsilon)
        {
            if (Math.Abs(Point.Y - y) < epsilon)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Аппроксимация равенства значений дистанции.
        /// </summary>
        /// <param name="distance">Дистанция.</param>
        /// <param name="epsilon">Погрешность.</param>
        /// <returns>Статус равенства значений.</returns>
        public readonly bool ApproximatelyDistance(float distance, float epsilon)
        {
            if (Math.Abs(Distance - distance) < epsilon)
            {
                return true;
            }

            return false;
        }
        #endregion
    }

    /// <summary>
    /// Список точек привязки в двухмерном пространстве.
    /// </summary>
    [Serializable]
    public class ListSnapPoint2D : ListArray<TSnapPoint2D>
    {
        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию инициализирует объект класса предустановленными значениями.
        /// </summary>
        public ListSnapPoint2D()
            : base()
        {

        }

        /// <summary>
        /// Конструктор инициализирует объект класса указанными параметрами.
        /// </summary>
        /// <param name="capacity">Начальная максимальная емкость списка.</param>
        public ListSnapPoint2D(int capacity)
            : base(capacity)
        {

        }
        #endregion

        #region Add methods
        /// <summary>
        /// Добавление точки привязки по указанными параметрами.
        /// </summary>
        /// <param name="x">X - координата.</param>
        /// <param name="y">Y - координата.</param>
        public void Add(float x, float y)
        {
            Add(new TSnapPoint2D(x, y));
        }

        /// <summary>
        /// Добавление точки привязки по указанными параметрами.
        /// </summary>
        /// <param name="point">Точка.</param>
        public void Add(Vector2Df point)
        {
            Add(new TSnapPoint2D(point));
        }

#if UNITY_2017_1_OR_NEWER
		/// <summary>
		/// Конструктор инициализирует точку привязки указанными параметрами.
		/// </summary>
		/// <param name="vector">Вектор.</param>
		public void Add(UnityEngine.Vector2 vector)
		{
			Add(new TSnapPoint2D(vector));
		}
#endif

        /// <summary>
        /// Добавление точки привязки по указанными параметрами.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="distance">Дистанция до этой точки.</param>
        public void Add(Vector2Df point, float distance)
        {
            Add(new TSnapPoint2D(point, distance));
        }
        #endregion

        #region Calc methods
        /// <summary>
        /// Вычисление дистанции до указанной точки.
        /// </summary>
        /// <param name="point">Точка.</param>
        public void ComputeDistance(Vector2Df point)
        {
            for (var i = 0; i < _count; i++)
            {
                _arrayOfItems[i].ComputeDistance(ref point);
            }
        }

#if UNITY_2017_1_OR_NEWER
		/// <summary>
		/// Вычисление дистанции до указанной точки.
		/// </summary>
		/// <param name="vector">Точка.</param>
		public void ComputeDistance(UnityEngine.Vector2 vector)
		{
			Vector2Df point = new Vector2Df(vector.x, vector.y);
			for (Int32 i = 0; i < _count; i++)
			{
				_arrayOfItems[i].ComputeDistance(ref point);
			}
		}
#endif

        /// <summary>
        /// Получение минимальной дистанции точки привязки.
        /// </summary>
        /// <returns>Минимальная дистанция.</returns>
        public float GetMinimumDistance()
        {
            var minimum = float.MaxValue;
            for (var i = 0; i < _count; i++)
            {
                if (_arrayOfItems[i].Distance < minimum)
                {
                    minimum = _arrayOfItems[i].Distance;
                }
            }

            return minimum;
        }

        /// <summary>
        /// Получение индекса точки по минимальной дистанции.
        /// </summary>
        /// <returns>Индекса точки привязки с минимальной дистанцией.</returns>
        public int GetMinimumDistanceIndex()
        {
            var minimum = float.MaxValue;
            var index = 0;
            for (var i = 0; i < _count; i++)
            {
                if (_arrayOfItems[i].Distance < minimum)
                {
                    minimum = _arrayOfItems[i].Distance;
                    index = i;
                }
            }

            return index;
        }
        #endregion

        #region Search methods
        /// <summary>
        /// Поиск индекса ближайшей точки на основании позиции.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="deltaX">Допуск по координате X.</param>
        /// <param name="deltaY">Допуск по координате Y.</param>
        /// <returns>Найденный индекс или -1.</returns>
        public int FindIndexNearestFromPosition(Vector2Df point, float deltaX, float deltaY)
        {
            for (var i = 0; i < _count; i++)
            {
                if (_arrayOfItems[i].ApproximatelyPoint(ref point, deltaX, deltaY))
                {
                    return i;
                }
            }

            return -1;
        }

#if UNITY_2017_1_OR_NEWER
		/// <summary>
		/// Поиск индекса ближайшей точки на основании позиции.
		/// </summary>
		/// <param name="vector">Точка.</param>
		/// <param name="delta_x">Допуск по координате X.</param>
		/// <param name="delta_y">Допуск по координате Y.</param>
		/// <returns>Найденный индекс или -1.</returns>
		public Int32 FindIndexNearestFromPosition(UnityEngine.Vector2 vector, Single delta_x, Single delta_y)
		{
			for (Int32 i = 0; i < _count; i++)
			{
				if (_arrayOfItems[i].ApproximatelyPoint(ref vector, delta_x, delta_y))
				{
					return (i);
				}
			}

			return (-1);
		}
#endif

        /// <summary>
        /// Поиск индекса ближайшей точки на основании позиции по X.
        /// </summary>
        /// <param name="x">Координата точки по X.</param>
        /// <param name="epsilon">Погрешность.</param>
        /// <returns>Найденный индекс или -1.</returns>
        public int FindIndexNearestFromPositionX(float x, float epsilon)
        {
            for (var i = 0; i < _count; i++)
            {
                if (_arrayOfItems[i].ApproximatelyPointX(x, epsilon))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Поиск индекса ближайшей точки на основании позиции по Y.
        /// </summary>
        /// <param name="y">Координата точки по Y.</param>
        /// <param name="epsilon">Погрешность.</param>
        /// <returns>Найденный индекс или -1.</returns>
        public int FindIndexNearestFromPositionY(float y, float epsilon)
        {
            for (var i = 0; i < _count; i++)
            {
                if (_arrayOfItems[i].ApproximatelyPointY(y, epsilon))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Поиск индекса ближайшей точки на основании дистанции.
        /// </summary>
        /// <param name="distance">Дистанция.</param>
        /// <param name="epsilon">Погрешность.</param>
        /// <returns>Найденный индекс или -1.</returns>
        public int FindIndexNearestFromDistance(float distance, float epsilon)
        {
            for (var i = 0; i < _count; i++)
            {
                if (_arrayOfItems[i].ApproximatelyDistance(distance, epsilon))
                {
                    return i;
                }
            }

            return -1;
        }
        #endregion
    }
    /**@}*/
}