using System;
using System.Runtime.InteropServices;

namespace Lotus.Maths
{
    /** \addtogroup MathGeometry2D
	*@{*/
    /// <summary>
    /// Структура луча в двухмерном пространстве.
    /// </summary>
    /// <remarks>
    /// Луч представляют собой точку и направление.
    /// </remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray2Df : IEquatable<Ray2Df>, IComparable<Ray2Df>
    {
        #region Static fields
        /// <summary>
        /// Текстовый формат отображения параметров луча.
        /// </summary>
        public static string ToStringFormat = "Pos = {0:0.00}, {1:0.00}; Dir = {2:0.00}, {3:0.00}";
        #endregion

        #region Fields
        /// <summary>
        /// Позиция луча.
        /// </summary>
        public Vector2Df Position;

        /// <summary>
        /// Направление луча.
        /// </summary>
        public Vector2Df Direction;
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор инициализирует луч указанными параметрами.
        /// </summary>
        /// <param name="pos">Позиция луча.</param>
        /// <param name="dir">Направление луча.</param>
        public Ray2Df(Vector2Df pos, Vector2Df dir)
        {
            Position = pos;
            Direction = dir;
        }

        /// <summary>
        /// Конструктор инициализирует луч указанным лучом.
        /// </summary>
        /// <param name="source">Луч.</param>
        public Ray2Df(Ray2Df source)
        {
            Position = source.Position;
            Direction = source.Direction;
        }

#if UNITY_2017_1_OR_NEWER
		/// <summary>
		/// Конструктор инициализирует луч указанными параметрами.
		/// </summary>
		/// <param name="pos">Позиция луча.</param>
		/// <param name="dir">Направление луча.</param>
		public Ray2Df(UnityEngine.Vector2 pos, UnityEngine.Vector2 dir)
		{
			Position = new Vector2Df(pos.x, pos.y);
			Direction = new Vector2Df(dir.x, dir.y);
		}

		/// <summary>
		/// Конструктор инициализирует луч указанным лучом.
		/// </summary>
		/// <param name="source">Луч.</param>
		public Ray2Df(UnityEngine.Ray2D source)
		{
			Position = new Vector2Df(source.origin.x, source.origin.y);
			Direction = new Vector2Df(source.direction.x, source.direction.y);
		}
#endif
        #endregion

        #region System methods
        /// <summary>
        /// Проверяет равен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="obj">Сравниваемый объект.</param>
        /// <returns>Статус равенства объектов.</returns>
        public override readonly bool Equals(object? obj)
        {
            if (obj is Ray2Df ray)
            {
                return Equals(ray);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Проверка равенства лучей по значению.
        /// </summary>
        /// <param name="other">Сравниваемый луч.</param>
        /// <returns>Статус равенства лучей.</returns>
        public readonly bool Equals(Ray2Df other)
        {
            return this == other;
        }

        /// <summary>
        /// Сравнение лучей для упорядочивания.
        /// </summary>
        /// <param name="other">Сравниваемый луч.</param>
        /// <returns>Статус сравнения лучей.</returns>
        public readonly int CompareTo(Ray2Df other)
        {
            if (Position > other.Position)
            {
                return 1;
            }
            else
            {
                if (Position == other.Position && Direction > other.Direction)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Получение хеш-кода луча.
        /// </summary>
        /// <returns>Хеш-код луча.</returns>
        public override readonly int GetHashCode()
        {
            return Position.GetHashCode() ^ Direction.GetHashCode();
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <returns>Текстовое представление луча с указанием значений.</returns>
        public override readonly string ToString()
        {
            return string.Format(ToStringFormat, Position.X, Position.Y, Direction.X, Direction.Y);
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <param name="format">Формат отображения.</param>
        /// <returns>Текстовое представление луча с указанием значений.</returns>
        public readonly string ToString(string format)
        {
            return "Pos = " + Position.ToString(format) + "; Dir = " + Direction.ToString(format);
        }
        #endregion

        #region Operators
        /// <summary>
        /// Сравнение лучей на равенство.
        /// </summary>
        /// <param name="left">Первый луч.</param>
        /// <param name="right">Второй луч.</param>
        /// <returns>Статус равенства лучей.</returns>
        public static bool operator ==(Ray2Df left, Ray2Df right)
        {
            return left.Position == right.Position && left.Direction == right.Direction;
        }

        /// <summary>
        /// Сравнение лучей на неравенство.
        /// </summary>
        /// <param name="left">Первый луч.</param>
        /// <param name="right">Второй луч.</param>
        /// <returns>Статус неравенства лучей.</returns>
        public static bool operator !=(Ray2Df left, Ray2Df right)
        {
            return left.Position != right.Position || left.Direction != right.Direction;
        }

        /// <summary>
        /// Обратный луч.
        /// </summary>
        /// <param name="ray">Исходный луч.</param>
        /// <returns>Обратный луч.</returns>
        public static Ray2Df operator -(Ray2Df ray)
        {
            return new Ray2Df(ray.Position, -ray.Direction);
        }
        #endregion

        #region Operators conversion
#if UNITY_2017_1_OR_NEWER
		/// <summary>
		/// Неявное преобразование в объект типа <see cref="UnityEngine.Ray2D"/>.
		/// </summary>
		/// <param name="ray">Луч.</param>
		/// <returns>Объект <see cref="UnityEngine.Ray2D"/>.</returns>
		public static implicit operator UnityEngine.Ray2D(Ray2Df ray)
		{
			return new UnityEngine.Ray2D(ray.Position, ray.Direction);
		}
#endif
        #endregion

        #region Main methods
        /// <summary>
        /// Получение точки на луче.
        /// </summary>
        /// <param name="position">Позиция точки от начала луча.</param>
        /// <returns>Точка на луче.</returns>
        public readonly Vector2Df GetPoint(float position)
        {
            return Position + (Direction * position);
        }

        /// <summary>
        /// Установка параметров луча.
        /// </summary>
        /// <param name="startPoint">Начальная точка.</param>
        /// <param name="endPoint">Конечная точка.</param>
        public void SetFromPoint(in Vector2Df startPoint, in Vector2Df endPoint)
        {
            Position = startPoint;
            Direction = (endPoint - startPoint).Normalized;
        }

        /// <summary>
        /// Проверка на пересечение с лучом.
        /// </summary>
        /// <param name="ray">Луч.</param>
        /// <returns>Тип пересечения.</returns>
        public readonly TIntersectType2D IntersectRay(Ray2Df ray)
        {
            return XIntersect2D.RayToRay(in Position, in Direction, in ray.Position, in ray.Direction, out _);
        }
        #endregion
    }
    /**@}*/
}