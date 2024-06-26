using System;
using System.Runtime.InteropServices;

namespace Lotus.Maths
{
    /** \addtogroup MathGeometry3D
	*@{*/
    /// <summary>
    /// Трехмерный вектор.
    /// </summary>
    /// <remarks>
    /// Реализация трехмерного вектора, представляющего собой базовую математическую сущность в трехмерном пространстве.
    /// </remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3D : IEquatable<Vector3D>, IComparable<Vector3D>
    {
        #region Const
        /// <summary>
        /// Единичный вектор.
        /// </summary>
        public static readonly Vector3D One = new(1, 1, 1);

        /// <summary>
        /// Вектор - право.
        /// </summary>
        public static readonly Vector3D Right = new(1, 0, 0);

        /// <summary>
        /// Вектор - влево.
        /// </summary>
        public static readonly Vector3D Left = new(-1, 0, 0);

        /// <summary>
        /// Вектор - вверх.
        /// </summary>
        public static readonly Vector3D Up = new(0, 1, 0);

        /// <summary>
        /// Вектор - вниз.
        /// </summary>
        public static readonly Vector3D Down = new(0, -1, 0);

        /// <summary>
        /// Вектор - вперед.
        /// </summary>
        public static readonly Vector3D Forward = new(0, 0, 1);

        /// <summary>
        /// Вектор - назад.
        /// </summary>
        public static readonly Vector3D Back = new(0, 0, -1);

        /// <summary>
        /// Нулевой вектор.
        /// </summary>
        public static readonly Vector3D Zero = new(0, 0, 0);
        #endregion

        #region Static fields
        /// <summary>
        /// Текстовый формат отображения параметров вектора.
        /// </summary>
        public static string ToStringFormat = "X = {0:0.00}; Y = {1:0.00}; Z = {2:0.00}";

        /// <summary>
        /// Текстовый формат отображения только значений параметров вектора.
        /// </summary>
        public static string ToStringFormatValue = "{0:0.00}; {1:0.00}; {2:0.00}";
        #endregion

        #region Static methods
        /// <summary>
        /// Сложение векторов.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        /// <param name="result">Результирующий вектор.</param>
        public static void Add(in Vector3D a, in Vector3D b, out Vector3D result)
        {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Z = a.Z + b.Z;
        }

        /// <summary>
        /// Разность векторов.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        /// <param name="result">Результирующий вектор.</param>
        public static void Subtract(in Vector3D a, in Vector3D b, out Vector3D result)
        {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
            result.Z = a.Z - b.Z;
        }

        /// <summary>
        /// Косинус угла между векторами.
        /// </summary>
        /// <param name="from">Начальный вектор.</param>
        /// <param name="to">Конечный вектор.</param>
        /// <returns>Косинус угла.</returns>
        public static double Cos(in Vector3D from, in Vector3D to)
        {
            var dot = (from.X * to.X) + (from.Y * to.Y) + (from.Z * to.Z);
            var ll = from.Length * to.Length;
            return dot / ll;
        }

        /// <summary>
        /// Угол между двумя векторами (в градусах).
        /// </summary>
        /// <param name="from">Начальный вектор.</param>
        /// <param name="to">Конечные вектор.</param>
        /// <returns>Угол в градусах.</returns>
        public static double Angle(in Vector3D from, in Vector3D to)
        {
            var dot = (from.X * to.X) + (from.Y * to.Y) + (from.Z * to.Z);
            var ll = from.Length * to.Length;
            var csv = dot / ll;
            return XMathAngle.NormalizationFull(Math.Acos(csv) * XMath.RadianToDegree_D);
        }

        /// <summary>
        /// Расстояние между двумя векторами.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        /// <returns>Расстояние между двумя векторами.</returns>
        public static double Distance(in Vector3D a, in Vector3D b)
        {
            var x = b.X - a.X;
            var y = b.Y - a.Y;
            var z = b.Z - a.Z;

            return Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        /// <summary>
        /// Скалярное произведение векторов.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        /// <returns>Скаляр.</returns>
        public static double Dot(in Vector3D a, in Vector3D b)
        {
            return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
        }

        /// <summary>
        /// Векторное произведение векторов.
        /// </summary>
        /// <param name="left">Левый вектор.</param>
        /// <param name="right">Правый вектор.</param>
        /// <returns>Вектор, перпендикулярный обоим векторам.</returns>
        public static Vector3D Cross(in Vector3D left, in Vector3D right)
        {
            return new Vector3D((left.Y * right.Z) - (left.Z * right.Y),
                (left.Z * right.X) - (left.X * right.Z),
                (left.X * right.Y) - (left.Y * right.X));
        }

        /// <summary>
        /// Линейная интерполяция векторов.
        /// </summary>
        /// <param name="from">Начальный вектор.</param>
        /// <param name="to">Конечный вектор.</param>
        /// <param name="time">Время от 0 до 1.</param>
        /// <returns>Интерполированный вектор.</returns>
        public static Vector3D Lerp(in Vector3D from, in Vector3D to, double time)
        {
            Vector3D vector;
            vector.X = from.X + ((to.X - from.X) * time);
            vector.Y = from.Y + ((to.Y - from.Y) * time);
            vector.Z = from.Z + ((to.Z - from.Z) * time);
            return vector;
        }

        /// <summary>
        /// Десереализация трехмерного вектора из строки.
        /// </summary>
        /// <param name="data">Строка данных.</param>
        /// <returns>Трехмерный вектор.</returns>
        public static Vector3D DeserializeFromString(string data)
        {
            var vector = new Vector3D();
            var vector_data = data.Split(';');
            vector.X = XNumberHelper.ParseDouble(vector_data[0]);
            vector.Y = XNumberHelper.ParseDouble(vector_data[1]);
            vector.Z = XNumberHelper.ParseDouble(vector_data[2]);
            return vector;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Координата X.
        /// </summary>
        public double X;

        /// <summary>
        /// Координата Y.
        /// </summary>
        public double Y;

        /// <summary>
        /// Координата Z.
        /// </summary>
        public double Z;
        #endregion

        #region Properties
        /// <summary>
        /// Квадрат длины вектора.
        /// </summary>
        public readonly double SqrLength
        {
            get { return (X * X) + (Y * Y) + (Z * Z); }
        }

        /// <summary>
        /// Длина вектора.
        /// </summary>
        public readonly double Length
        {
            get { return Math.Sqrt((X * X) + (Y * Y) + (Z * Z)); }
        }

        /// <summary>
        /// Нормализованный вектор.
        /// </summary>
        public readonly Vector3D Normalized
        {
            get
            {
                var inv_lentgh = XMath.InvSqrt((X * X) + (Y * Y) + (Z * Z));
                return new Vector3D(X * inv_lentgh, Y * inv_lentgh, Z * inv_lentgh);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор инициализирует вектор указанными параметрами.
        /// </summary>
        /// <param name="x">X - координата.</param>
        /// <param name="y">Y - координата.</param>
        /// <param name="z">Z - координата.</param>
        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Конструктор инициализирует вектор указанным вектором.
        /// </summary>
        /// <param name="source">Вектор.</param>
        public Vector3D(Vector3D source)
        {
            X = source.X;
            Y = source.Y;
            Z = source.Z;
        }

#if USE_WINDOWS
		/// <summary>
		/// Конструктор инициализирует вектор указанным вектором WPF.
		/// </summary>
		/// <param name="source">Вектор WPF.</param>
		public Vector3D(System.Windows.Media.Media3D.Vector3D source)
		{
			X = source.X;
			Y = source.Y;
			Z = source.Z;
		}

		/// <summary>
		/// Конструктор инициализирует вектор указанной точкой WPF.
		/// </summary>
		/// <param name="source">Точка WPF.</param>
		public Vector3D(System.Windows.Media.Media3D.Point3D source)
		{
			X = source.X;
			Y = source.Y;
			Z = source.Z;
		}

		/// <summary>
		/// Конструктор инициализирует вектор указанным размером WPF.
		/// </summary>
		/// <param name="source">Размер WPF.</param>
		public Vector3D(System.Windows.Media.Media3D.Size3D source)
		{
			X = source.X;
			Y = source.Y;
			Z = source.Z;
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
            if (obj is Vector3D vector)
            {
                return Equals(vector);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Проверка равенства векторов по значению.
        /// </summary>
        /// <param name="other">Сравниваемый вектор.</param>
        /// <returns>Статус равенства векторов.</returns>
        public readonly bool Equals(Vector3D other)
        {
            return this == other;
        }

        /// <summary>
        /// Сравнение векторов для упорядочивания.
        /// </summary>
        /// <param name="other">Вектор.</param>
        /// <returns>Статус сравнения векторов.</returns>
        public readonly int CompareTo(Vector3D other)
        {
            if (X > other.X)
            {
                return 1;
            }
            else
            {
                if (X == other.X && Y > other.Y)
                {
                    return 1;
                }
                else
                {
                    if (X == other.X && Y == other.Y && Z > other.Z)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// Получение хеш-кода вектора.
        /// </summary>
        /// <returns>Хеш-код вектора.</returns>
        public override readonly int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <returns>Текстовое представление вектора с указанием значений координат.</returns>
        public override readonly string ToString()
        {
            return string.Format(ToStringFormat, X, Y, Z);
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <param name="format">Формат отображения компонентов вектора.</param>
        /// <returns>Текстовое представление вектора с указанием значений координат.</returns>
        public readonly string ToString(string format)
        {
            return string.Format(ToStringFormat.Replace("0.00", format), X, Y, Z);
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <returns>Текстовое представление вектора с указанием значений координат.</returns>
        public readonly string ToStringValue()
        {
            return string.Format(ToStringFormatValue, X, Y, Z);
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <param name="format">Формат отображения компонентов вектора.</param>
        /// <returns>Текстовое представление вектора с указанием значений координат.</returns>
        public readonly string ToStringValue(string format)
        {
            return string.Format(ToStringFormatValue.Replace("0.00", format), X, Y, Z);
        }
        #endregion

        #region Operators
        /// <summary>
        /// Сложение векторов.
        /// </summary>
        /// <param name="left">Первый вектор.</param>
        /// <param name="right">Второй вектор.</param>
        /// <returns>Сумма векторов.</returns>
        public static Vector3D operator +(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <summary>
        /// Вычитание векторов.
        /// </summary>
        /// <param name="left">Первый вектор.</param>
        /// <param name="right">Второй вектор.</param>
        /// <returns>Разность векторов.</returns>
        public static Vector3D operator -(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        /// <summary>
        /// Умножение вектора на скаляр.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="scalar">Скаляр.</param>
        /// <returns>Масштабированный вектор.</returns>
        public static Vector3D operator *(Vector3D vector, double scalar)
        {
            return new Vector3D(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        /// <summary>
        /// Деление вектора на скаляр.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="scalar">Скаляр.</param>
        /// <returns>Масштабированный вектор.</returns>
        public static Vector3D operator /(Vector3D vector, double scalar)
        {
            scalar = 1 / scalar;
            return new Vector3D(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        /// <summary>
        /// Умножение вектора на вектор. Скалярное произведение векторов.
        /// </summary>
        /// <param name="left">Первый вектор.</param>
        /// <param name="right">Второй вектор.</param>
        /// <returns>Скаляр.</returns>
        public static double operator *(Vector3D left, Vector3D right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        /// <summary>
        /// Умножение вектора на вектор. Векторное произведение векторов.
        /// </summary>
        /// <param name="left">Левый вектор.</param>
        /// <param name="right">Правый вектор.</param>
        /// <returns>Вектор.</returns>
        public static Vector3D operator ^(Vector3D left, Vector3D right)
        {
            return new Vector3D((left.Y * right.Z) - (left.Z * right.Y),
                (left.Z * right.X) - (left.X * right.Z),
                (left.X * right.Y) - (left.Y * right.X));
        }

        /// <summary>
        /// Умножение вектора на матрицу трансформации.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="matrix">Матрица трансформации.</param>
        /// <returns>Трансформированный вектор.</returns>
        public static Vector3D operator *(Vector3D vector, Matrix3Dx3 matrix)
        {
            return new Vector3D((vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31),
                (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32),
                (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33));
        }

        /// <summary>
        /// Умножение вектора на матрицу трансформации.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="matrix">Матрица трансформации.</param>
        /// <returns>Трансформированный вектор.</returns>
        public static Vector3D operator *(Vector3D vector, Matrix4Dx4 matrix)
        {
            return new Vector3D((vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31) + matrix.M41,
                (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32) + matrix.M42,
                (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33) + matrix.M43);
        }

        /// <summary>
        /// Сравнение векторов на равенство.
        /// </summary>
        /// <param name="left">Первый вектор.</param>
        /// <param name="right">Второй вектор.</param>
        /// <returns>Статус равенства векторов.</returns>
        public static bool operator ==(Vector3D left, Vector3D right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }

        /// <summary>
        /// Сравнение векторов на неравенство.
        /// </summary>
        /// <param name="left">Первый вектор.</param>
        /// <param name="right">Второй вектор.</param>
        /// <returns>Статус неравенства векторов.</returns>
        public static bool operator !=(Vector3D left, Vector3D right)
        {
            return left.X != right.X || left.Y != right.Y || left.Z != right.Z;
        }

        /// <summary>
        /// Реализация лексикографического порядка отношений векторов.
        /// </summary>
        /// <param name="left">Левый вектор.</param>
        /// <param name="right">Правый вектор.</param>
        /// <returns>Статус меньше.</returns>
        public static bool operator <(Vector3D left, Vector3D right)
        {
            return left.X < right.X || (left.X == right.X && left.Y < right.Y)
                                      || (left.X == right.X && left.Y == right.Y && left.Z < right.Z);
        }

        /// <summary>
        /// Реализация лексикографического порядка отношений векторов.
        /// </summary>
        /// <param name="left">Левый вектор.</param>
        /// <param name="right">Правый вектор.</param>
        /// <returns>Статус больше.</returns>
        public static bool operator >(Vector3D left, Vector3D right)
        {
            return left.X > right.X || (left.X == right.X && left.Y > right.Y)
                                      || (left.X == right.X && left.Y == right.Y && left.Z > right.Z);
        }

        /// <summary>
        /// Обратный вектор.
        /// </summary>
        /// <param name="vector">Исходный вектор.</param>
        /// <returns>Обратный вектор.</returns>
        public static Vector3D operator -(Vector3D vector)
        {
            return new Vector3D(-vector.X, -vector.Y, -vector.Z);
        }
        #endregion

        #region Operators conversion
        /// <summary>
        /// Неявное преобразование в объект типа <see cref="Vector3Df"/>.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <returns>Объект <see cref="Vector3Df"/> .</returns>
        public static implicit operator Vector3Df(Vector3D vector)
        {
            return new Vector3Df((float)vector.X, (float)vector.Y, (float)vector.Z);
        }

#if UNITY_2017_1_OR_NEWER
		/// <summary>
		/// Неявное преобразование в объект типа <see cref="UnityEngine.Vector3"/>.
		/// </summary>
		/// <param name="vector">Вектор.</param>
		/// <returns>Объект <see cref="UnityEngine.Vector3"/>.</returns>
		public static implicit operator UnityEngine.Vector3(Vector3D vector)
		{
			return new UnityEngine.Vector3((Single)vector.X, (Single)vector.Y, (Single)vector.Z);
		}
#endif
        #endregion

        #region Indexer
        /// <summary>
        /// Индексация компонентов вектора на основе индекса.
        /// </summary>
        /// <param name="index">Индекс компонента.</param>
        /// <returns>Компонента вектора.</returns>
        public double this[int index]
        {
            readonly get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    default:
                        return Z;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    default:
                        Z = value;
                        break;
                }
            }
        }
        #endregion

        #region Main methods
        /// <summary>
        /// Нормализация вектора.
        /// </summary>
        public void Normalize()
        {
            var inv_lentgh = XMath.InvSqrt((X * X) + (Y * Y) + (Z * Z));
            X *= inv_lentgh;
            Y *= inv_lentgh;
            Z *= inv_lentgh;
        }

        /// <summary>
        /// Вычисление расстояние до вектора.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <returns>Расстояние до вектора.</returns>
        public readonly double Distance(in Vector3D vector)
        {
            var x = vector.X - X;
            var y = vector.Y - Y;
            var z = vector.Z - Z;

            return Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        /// <summary>
        /// Вычисление скалярного произведения векторов.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <returns>Скалярное произведение векторов.</returns>
        public readonly double Dot(in Vector3D vector)
        {
            return (X * vector.X) + (Y * vector.Y) + (Z * vector.Z);
        }

        /// <summary>
        /// Установка компонентов вектора из наибольших компонентов двух векторов.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        public void SetMaximize(in Vector3D a, in Vector3D b)
        {
            X = a.X > b.X ? a.X : b.X;
            Y = a.Y > b.Y ? a.Y : b.Y;
            Z = a.Z > b.Z ? a.Z : b.Z;
        }

        /// <summary>
        /// Установка компонентов вектора из наименьших компонентов двух векторов.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        public void SetMinimize(in Vector3D a, in Vector3D b)
        {
            X = a.X < b.X ? a.X : b.X;
            Y = a.Y < b.Y ? a.Y : b.Y;
            Z = a.Z < b.Z ? a.Z : b.Z;
        }

        /// <summary>
        /// Векторное произведение c нормализацией результата.
        /// </summary>
        /// <param name="left">Левый вектор.</param>
        /// <param name="right">Правый вектор.</param>
        public void CrossNormalize(in Vector3D left, in Vector3D right)
        {
            X = (left.Y * right.Z) - (left.Z * right.Y);
            Y = (left.Z * right.X) - (left.X * right.Z);
            Z = (left.X * right.Y) - (left.Y * right.X);
            var inv_length = XMath.InvSqrt((X * X) + (Y * Y) + (Z * Z));
            X *= inv_length;
            Y *= inv_length;
            Z *= inv_length;
        }

        /// <summary>
        /// Трансформация вектора как точки.
        /// </summary>
        /// <param name="matrix">Матрица трансформации.</param>
        public void TransformAsPoint(in Matrix4Dx4 matrix)
        {
            this = new Vector3D((X * matrix.M11) + (Y * matrix.M21) + (Z * matrix.M31) + matrix.M41,
                                (X * matrix.M12) + (Y * matrix.M22) + (Z * matrix.M32) + matrix.M42,
                                (X * matrix.M13) + (Y * matrix.M23) + (Z * matrix.M33) + matrix.M43);
        }

        /// <summary>
        /// Трансформация вектора как вектора.
        /// </summary>
        /// <param name="matrix">Матрица трансформации.</param>
        public void TransformAsVector(in Matrix4Dx4 matrix)
        {
            this = new Vector3D((X * matrix.M11) + (Y * matrix.M21) + (Z * matrix.M31),
                                (X * matrix.M12) + (Y * matrix.M22) + (Z * matrix.M32),
                                (X * matrix.M13) + (Y * matrix.M23) + (Z * matrix.M33));
        }

        /// <summary>
        /// Сериализация вектора в строку.
        /// </summary>
        /// <returns>Строка данных.</returns>
        public readonly string SerializeToString()
        {
            return string.Format("{0};{1};{2}", X, Y, Z);
        }
        #endregion

        #region Convert methods
        /// <summary>
        /// Преобразование в вектор нулевой X компонентой.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector2D ToVector2X()
        {
            return new Vector2D(X, 0);
        }

        /// <summary>
        /// Преобразование в вектор нулевой Y компонентой.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector2D ToVector2Y()
        {
            return new Vector2D(0, Y);
        }

        /// <summary>
        /// Преобразование в двухмерных вектор плоскости XY.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector2D ToVector2XY()
        {
            return new Vector2D(X, Y);
        }

        /// <summary>
        /// Преобразование в двухмерных вектор плоскости YZ.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector2D ToVector2XZ()
        {
            return new Vector2D(X, Z);
        }

        /// <summary>
        /// Преобразование в двухмерных вектор плоскости YZ.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector2D ToVector2YZ()
        {
            return new Vector2D(Y, Z);
        }

        /// <summary>
        /// Преобразование в трехмерный вектор только с компонентой X.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector3D ToVector3X()
        {
            return new Vector3D(X, 0, 0);
        }

        /// <summary>
        /// Преобразование в трехмерный вектор только с компонентой Y.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector3D ToVector3Y()
        {
            return new Vector3D(0, Y, 0);
        }

        /// <summary>
        /// Преобразование в трехмерный вектор только с компонентой Z.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector3D ToVector3Z()
        {
            return new Vector3D(0, 0, Z);
        }

        /// <summary>
        /// Преобразование в трехмерный вектор плоскости XY.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector3D ToVector3XY()
        {
            return new Vector3D(X, Y, 0);
        }

        /// <summary>
        /// Преобразование в трехмерный вектор плоскости YZ.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector3D ToVector3XZ()
        {
            return new Vector3D(X, 0, Z);
        }

        /// <summary>
        /// Преобразование в трехмерный вектор плоскости YZ.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector3D ToVector3YZ()
        {
            return new Vector3D(0, Y, Z);
        }
        #endregion
    }

    /// <summary>
    /// Трехмерный вектор.
    /// </summary>
    /// <remarks>
    /// Реализация трехмерного вектора, представляющего собой базовую математическую сущность в трехмерном пространстве.
    /// </remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vector3Df : IEquatable<Vector3Df>, IComparable<Vector3Df>
    {
        #region Const
        /// <summary>
        /// Единичный вектор.
        /// </summary>
        public static readonly Vector3Df One = new(1, 1, 1);

        /// <summary>
        /// Вектор - право.
        /// </summary>
        public static readonly Vector3Df Right = new(1, 0, 0);

        /// <summary>
        /// Вектор - влево.
        /// </summary>
        public static readonly Vector3Df Left = new(-1, 0, 0);

        /// <summary>
        /// Вектор - вверх.
        /// </summary>
        public static readonly Vector3Df Up = new(0, 1, 0);

        /// <summary>
        /// Вектор - вниз.
        /// </summary>
        public static readonly Vector3Df Down = new(0, -1, 0);

        /// <summary>
        /// Вектор - вперед.
        /// </summary>
        public static readonly Vector3Df Forward = new(0, 0, 1);

        /// <summary>
        /// Вектор - назад.
        /// </summary>
        public static readonly Vector3Df Back = new(0, 0, -1);

        /// <summary>
        /// Нулевой вектор.
        /// </summary>
        public static readonly Vector3Df Zero = new(0, 0, 0);
        #endregion

        #region Static fields
        /// <summary>
        /// Текстовый формат отображения параметров вектора.
        /// </summary>
        public static string ToStringFormat = "X = {0:0.000}; Y = {1:0.000}; Z = {2:0.000}";
        #endregion

        #region Static methods
        /// <summary>
        /// Сложение векторов.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        /// <param name="result">Результирующий вектор.</param>
        public static void Add(in Vector3Df a, in Vector3Df b, out Vector3Df result)
        {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Z = a.Z + b.Z;
        }

        /// <summary>
        /// Разность векторов.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        /// <param name="result">Результирующий вектор.</param>
        public static void Subtract(in Vector3Df a, in Vector3Df b, out Vector3Df result)
        {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
            result.Z = a.Z - b.Z;
        }

        /// <summary>
        /// Масштабирование вектора.
        /// </summary>
        /// <param name="vector">Исходный вектор.</param>
        /// <param name="scale">Масштаб.</param>
        /// <returns>Результирующий вектор.</returns>
        public static Vector3Df Scale(Vector3Df vector, Vector3Df scale)
        {
            return new Vector3Df(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z);
        }

        /// <summary>
        /// Косинус угла между векторами.
        /// </summary>
        /// <param name="from">Начальный вектор.</param>
        /// <param name="to">Конечный вектор.</param>
        /// <returns>Косинус угла.</returns>
        public static float Cos(in Vector3Df from, in Vector3Df to)
        {
            var dot = (from.X * to.X) + (from.Y * to.Y) + (from.Z * to.Z);
            var ll = from.Length * to.Length;
            return dot / ll;
        }

        /// <summary>
        /// Угол между двумя векторами (в градусах).
        /// </summary>
        /// <param name="from">Начальный вектор.</param>
        /// <param name="to">Конечные вектор.</param>
        /// <returns>Угол в градусах.</returns>
        public static float Angle(in Vector3Df from, in Vector3Df to)
        {
            var dot = (from.X * to.X) + (from.Y * to.Y) + (from.Z * to.Z);
            var ll = from.Length * to.Length;
            var csv = dot / ll;
            return (float)XMathAngle.NormalizationFull(Math.Acos(csv) * XMath.RadianToDegree_D);
        }

        /// <summary>
        /// Расстояние между двумя векторами.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        /// <returns>Расстояние между двумя векторами.</returns>
        public static float Distance(in Vector3Df a, in Vector3Df b)
        {
            var x = b.X - a.X;
            var y = b.Y - a.Y;
            var z = b.Z - a.Z;

            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        /// <summary>
        /// Нормализация вектора.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <returns>Нормализованный вектор.</returns>
        public static Vector3Df Normalize(in Vector3Df vector)
        {
            var inv_lentgh = XMath.InvSqrt((vector.X * vector.X) + (vector.Y * vector.Y) + (vector.Z * vector.Z));
            return new Vector3Df(vector.X * inv_lentgh, vector.Y * inv_lentgh, vector.Z * inv_lentgh);
        }

        /// <summary>
        /// Скалярное произведение векторов.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        /// <returns>Скаляр.</returns>
        public static float Dot(in Vector3Df a, in Vector3Df b)
        {
            return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
        }

        /// <summary>
        /// Векторное произведение векторов.
        /// </summary>
        /// <param name="left">Левый вектор.</param>
        /// <param name="right">Правый вектор.</param>
        /// <returns>Вектор, перпендикулярный обоим векторам.</returns>
        public static Vector3Df Cross(in Vector3Df left, in Vector3Df right)
        {
            Cross(in left, in right, out var result);
            return result;
        }

        /// <summary>
        /// Векторное произведение векторов.
        /// </summary>
        /// <param name="left">Левый вектор.</param>
        /// <param name="right">Правый вектор.</param>
        /// <param name="result">Вектор, перпендикулярный обоим векторам.</param>
        public static void Cross(in Vector3Df left, in Vector3Df right, out Vector3Df result)
        {
            result.X = (left.Y * right.Z) - (left.Z * right.Y);
            result.Y = (left.Z * right.X) - (left.X * right.Z);
            result.Z = (left.X * right.Y) - (left.Y * right.X);
        }

        /// <summary>
        /// Аппроксимация равенства значений векторов.
        /// </summary>
        /// <param name="left">Левый вектор.</param>
        /// <param name="right">Правый вектор.</param>
        /// <param name="epsilon">Погрешность.</param>
        /// <returns>Статус равенства значений векторов.</returns>
        public static bool Approximately(in Vector3Df left, in Vector3Df right, float epsilon = 0.01f)
        {
            if (Math.Abs(left.X - right.X) < epsilon &&
                Math.Abs(left.Y - right.Y) < epsilon &&
                Math.Abs(left.Z - right.Z) < epsilon)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Линейная интерполяция векторов.
        /// </summary>
        /// <param name="from">Начальный вектор.</param>
        /// <param name="to">Конечный вектор.</param>
        /// <param name="time">Время от 0 до 1.</param>
        /// <returns>Интерполированный вектор.</returns>
        public static Vector3Df Lerp(in Vector3Df from, in Vector3Df to, float time)
        {
            Vector3Df vector;
            vector.X = from.X + ((to.X - from.X) * time);
            vector.Y = from.Y + ((to.Y - from.Y) * time);
            vector.Z = from.Z + ((to.Z - from.Z) * time);
            return vector;
        }

        /// <summary>
        /// Получение вектора из сферических координат.
        /// </summary>
        /// <remarks>
        /// <para>
        /// https://ru.wikipedia.org/wiki/Сферическая_система_координат
        /// </para>
        /// <para>
        /// Экватор расположен в плоскости XZ, высота по координате Y
        /// В плоскости экватора при взгляде сверху
        /// Вертикальная ось - координата Z
        /// Горизонтальна ось - координата X
        /// </para>
        /// </remarks>
        /// <param name="radius">Радиус.</param>
        /// <param name="theta">Угол между осью Y и отрезком, соединяющим начало координат и точку в пределах [0, 180].</param>
        /// <param name="phi">Угол между осью Z и проекцией отрезка, соединяющего начало координат с точкой P, на плоскость XZ в пределах [0, 359].</param>
        /// <returns>Вектор.</returns>
        public static Vector3Df FromSpherical(float radius, float theta, float phi)
        {
            var radian_theta = theta * XMath.DegreeToRadian_F;
            var radian_phi = phi * XMath.DegreeToRadian_F;

            var z = (float)(Math.Sin(radian_theta) * Math.Cos(radian_phi)) * radius;
            var x = (float)(Math.Sin(radian_theta) * Math.Sin(radian_phi)) * radius;
            var y = (float)Math.Cos(radian_theta) * radius;

            return new Vector3Df(x, y, z);
        }

        /// <summary>
        /// Получение вектора из сферических координат.
        /// </summary>
        /// <remarks>
        /// <para>
        /// https://ru.wikipedia.org/wiki/Сферическая_система_координат
        /// </para>
        /// <para>
        /// Долгота(λ) - угол в горизонтальной плоскости между в пределах [0, 359]
        /// Широта(φ) - угол в вертикальной плоскости в пределах [0, 180]
        /// </para>
        /// <para>
        /// Экватор расположен в плоскости XZ, высота по координате Y
        /// В плоскости экватора при взгляде сверху
        /// Вертикальная ось - координата Z
        /// Горизонтальна ось - координата X
        /// </para>
        /// </remarks>
        /// <param name="radius">Радиус.</param>
        /// <param name="latitude">Широта в градусах в пределах [0, 180].</param>
        /// <param name="longitude">Долгота в градусах в пределах [0, 359].</param>
        /// <returns>Вектор.</returns>
        public static Vector3Df FromGeographicCoordSystem(float radius, float latitude, float longitude)
        {
            var theta = latitude * XMath.DegreeToRadian_F;
            var phi = longitude * XMath.DegreeToRadian_F;

            var z = (float)(Math.Sin(theta) * Math.Cos(phi)) * radius;
            var x = (float)(Math.Sin(theta) * Math.Sin(phi)) * radius;
            var y = (float)Math.Cos(theta) * radius;

            return new Vector3Df(x, y, z);
        }

        /// <summary>
        /// Получение вектора перпендикулярного указанной плоскости.
        /// </summary>
        /// <param name="planeType">Плоскость.</param>
        /// <returns>Вектор.</returns>
        public static Vector3Df GetPerpendicularPlane(TDimensionPlane planeType)
        {
            if (planeType == TDimensionPlane.XZ)
            {
                return Vector3Df.Up;
            }
            else
            {
                if (planeType == TDimensionPlane.ZY)
                {
                    return Vector3Df.Right;
                }
                else
                {
                    return Vector3Df.Forward;
                }
            }
        }

        /// <summary>
        /// Десереализация трехмерного вектора из строки.
        /// </summary>
        /// <param name="data">Строка данных.</param>
        /// <returns>Двухмерный вектор.</returns>
        public static Vector3Df DeserializeFromString(string data)
        {
            var vector = new Vector3Df();
            var vector_data = data.Split(';');
            vector.X = XNumberHelper.ParseSingle(vector_data[0]);
            vector.Y = XNumberHelper.ParseSingle(vector_data[1]);
            vector.Z = XNumberHelper.ParseSingle(vector_data[2]);
            return vector;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Координата X.
        /// </summary>
        public float X;

        /// <summary>
        /// Координата Y.
        /// </summary>
        public float Y;

        /// <summary>
        /// Координата Z.
        /// </summary>
        public float Z;
        #endregion

        #region Properties
        /// <summary>
        /// Квадрат длины вектора.
        /// </summary>
        public readonly float SqrLength
        {
            get { return (X * X) + (Y * Y) + (Z * Z); }
        }

        /// <summary>
        /// Длина вектора.
        /// </summary>
        public readonly float Length
        {
            get { return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z)); }
        }

        /// <summary>
        /// Нормализованный вектор.
        /// </summary>
        public readonly Vector3Df Normalized
        {
            get
            {
                var inv_lentgh = XMath.InvSqrt((X * X) + (Y * Y) + (Z * Z));
                return new Vector3Df(X * inv_lentgh, Y * inv_lentgh, Z * inv_lentgh);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор инициализирует вектор указанными параметрами.
        /// </summary>
        /// <param name="x">X - координата.</param>
        /// <param name="y">Y - координата.</param>
        /// <param name="z">Z - координата.</param>
        public Vector3Df(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Конструктор инициализирует вектор указанным вектором.
        /// </summary>
        /// <param name="source">Вектор.</param>
        public Vector3Df(Vector3Df source)
        {
            X = source.X;
            Y = source.Y;
            Z = source.Z;
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
            if (obj is Vector3Df vector)
            {
                return Equals(vector);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Проверка равенства векторов по значению.
        /// </summary>
        /// <param name="other">Сравниваемый вектор.</param>
        /// <returns>Статус равенства векторов.</returns>
        public readonly bool Equals(Vector3Df other)
        {
            return this == other;
        }

        /// <summary>
        /// Сравнение векторов для упорядочивания.
        /// </summary>
        /// <param name="other">Вектор.</param>
        /// <returns>Статус сравнения векторов.</returns>
        public readonly int CompareTo(Vector3Df other)
        {
            if (X > other.X)
            {
                return 1;
            }
            else
            {
                if (X == other.X && Y > other.Y)
                {
                    return 1;
                }
                else
                {
                    if (X == other.X && Y == other.Y && Z > other.Z)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// Получение хеш-кода вектора.
        /// </summary>
        /// <returns>Хеш-код вектора.</returns>
        public override readonly int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <returns>Текстовое представление вектора с указанием значений координат.</returns>
        public override readonly string ToString()
        {
            return string.Format(ToStringFormat, X, Y, Z);
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <param name="format">Формат отображения компонентов вектора.</param>
        /// <returns>Текстовое представление вектора с указанием значений координат.</returns>
        public readonly string ToString(string format)
        {
            return string.Format(format, X, Y, Z);
        }
        #endregion

        #region Operators
        /// <summary>
        /// Сложение векторов.
        /// </summary>
        /// <param name="left">Первый вектор.</param>
        /// <param name="right">Второй вектор.</param>
        /// <returns>Сумма векторов.</returns>
        public static Vector3Df operator +(Vector3Df left, Vector3Df right)
        {
            return new Vector3Df(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <summary>
        /// Вычитание векторов.
        /// </summary>
        /// <param name="left">Первый вектор.</param>
        /// <param name="right">Второй вектор.</param>
        /// <returns>Разность векторов.</returns>
        public static Vector3Df operator -(Vector3Df left, Vector3Df right)
        {
            return new Vector3Df(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        /// <summary>
        /// Умножение вектора на скаляр.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="scalar">Скаляр.</param>
        /// <returns>Масштабированный вектор.</returns>
        public static Vector3Df operator *(float scalar, Vector3Df vector)
        {
            return new Vector3Df(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        /// <summary>
        /// Умножение вектора на скаляр.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="scalar">Скаляр.</param>
        /// <returns>Масштабированный вектор.</returns>
        public static Vector3Df operator *(Vector3Df vector, float scalar)
        {
            return new Vector3Df(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        /// <summary>
        /// Деление вектора на скаляр.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="scalar">Скаляр.</param>
        /// <returns>Масштабированный вектор.</returns>
        public static Vector3Df operator /(float scalar, Vector3Df vector)
        {
            scalar = 1 / scalar;
            return new Vector3Df(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        /// <summary>
        /// Деление вектора на скаляр.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="scalar">Скаляр.</param>
        /// <returns>Масштабированный вектор.</returns>
        public static Vector3Df operator /(Vector3Df vector, float scalar)
        {
            scalar = 1 / scalar;
            return new Vector3Df(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        /// <summary>
        /// Умножение вектора на вектор. Скалярное произведение векторов.
        /// </summary>
        /// <param name="left">Первый вектор.</param>
        /// <param name="right">Второй вектор.</param>
        /// <returns>Скаляр.</returns>
        public static float operator *(Vector3Df left, Vector3Df right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        /// <summary>
        /// Умножение вектора на вектор. Векторное произведение векторов.
        /// </summary>
        /// <param name="left">Левый вектор.</param>
        /// <param name="right">Правый вектор.</param>
        /// <returns>Вектор.</returns>
        public static Vector3Df operator ^(Vector3Df left, Vector3Df right)
        {
            return new Vector3Df((left.Y * right.Z) - (left.Z * right.Y),
                (left.Z * right.X) - (left.X * right.Z),
                (left.X * right.Y) - (left.Y * right.X));
        }

        /// <summary>
        /// Умножение вектора на матрицу трансформации.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="matrix">Матрица трансформации.</param>
        /// <returns>Трансформированный вектор.</returns>
        public static Vector3Df operator *(Vector3Df vector, Matrix3Dx3f matrix)
        {
            return new Vector3Df((vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31),
                (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32),
                (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33));
        }

        /// <summary>
        /// Умножение вектора на матрицу трансформации.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="matrix">Матрица трансформации.</param>
        /// <returns>Трансформированный вектор.</returns>
        public static Vector3Df operator *(Vector3Df vector, Matrix4Dx4 matrix)
        {
            var x = (float)((vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31) + matrix.M41);
            var y = (float)((vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32) + matrix.M42);
            var z = (float)((vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33) + matrix.M43);
            return new Vector3Df(x, y, z);
        }

        /// <summary>
        /// Сравнение векторов на равенство.
        /// </summary>
        /// <param name="left">Первый вектор.</param>
        /// <param name="right">Второй вектор.</param>
        /// <returns>Статус равенства векторов.</returns>
        public static bool operator ==(Vector3Df left, Vector3Df right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }

        /// <summary>
        /// Сравнение векторов на неравенство.
        /// </summary>
        /// <param name="left">Первый вектор.</param>
        /// <param name="right">Второй вектор.</param>
        /// <returns>Статус неравенства векторов.</returns>
        public static bool operator !=(Vector3Df left, Vector3Df right)
        {
            return left.X != right.X || left.Y != right.Y || left.Z != right.Z;
        }

        /// <summary>
        /// Реализация лексикографического порядка отношений векторов.
        /// </summary>
        /// <param name="left">Левый вектор.</param>
        /// <param name="right">Правый вектор.</param>
        /// <returns>Статус меньше.</returns>
        public static bool operator <(Vector3Df left, Vector3Df right)
        {
            return left.X < right.X || (left.X == right.X && left.Y < right.Y)
                || (left.X == right.X && left.Y == right.Y && left.Z < right.Z);
        }

        /// <summary>
        /// Реализация лексикографического порядка отношений векторов.
        /// </summary>
        /// <param name="left">Левый вектор.</param>
        /// <param name="right">Правый вектор.</param>
        /// <returns>Статус больше.</returns>
        public static bool operator >(Vector3Df left, Vector3Df right)
        {
            return left.X > right.X || (left.X == right.X && left.Y > right.Y)
                || (left.X == right.X && left.Y == right.Y && left.Z > right.Z);
        }

        /// <summary>
        /// Обратный вектор.
        /// </summary>
        /// <param name="vector">Исходный вектор.</param>
        /// <returns>Обратный вектор.</returns>
        public static Vector3Df operator -(Vector3Df vector)
        {
            return new Vector3Df(-vector.X, -vector.Y, -vector.Z);
        }
        #endregion

        #region Operators conversion
#if UNITY_2017_1_OR_NEWER
		/// <summary>
		/// Неявное преобразование в объект типа <see cref="UnityEngine.Vector3"/>.
		/// </summary>
		/// <param name="vector">Вектор.</param>
		/// <returns>Объект <see cref="UnityEngine.Vector3"/>.</returns>
		public static implicit operator UnityEngine.Vector3(Vector3Df vector)
		{
			return new UnityEngine.Vector3(vector.X, vector.Y, vector.Z);
		}
#endif
        #endregion

        #region Indexer
        /// <summary>
        /// Индексация компонентов вектора на основе индекса.
        /// </summary>
        /// <param name="index">Индекс компонента.</param>
        /// <returns>Компонента вектора.</returns>
        public float this[int index]
        {
            readonly get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    default:
                        return Z;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    default:
                        Z = value;
                        break;
                }
            }
        }
        #endregion

        #region Main methods
        /// <summary>
        /// Нормализация вектора.
        /// </summary>
        public void Normalize()
        {
            var inv_lentgh = XMath.InvSqrt((X * X) + (Y * Y) + (Z * Z));
            X *= inv_lentgh;
            Y *= inv_lentgh;
            Z *= inv_lentgh;
        }

        /// <summary>
        /// Вычисление расстояние до вектора.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <returns>Расстояние до вектора.</returns>
        public readonly float Distance(in Vector3Df vector)
        {
            var x = vector.X - X;
            var y = vector.Y - Y;
            var z = vector.Z - Z;

            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        /// <summary>
        /// Вычисление скалярного произведения векторов.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <returns>Скалярное произведение векторов.</returns>
        public readonly float Dot(in Vector3Df vector)
        {
            return (X * vector.X) + (Y * vector.Y) + (Z * vector.Z);
        }

        /// <summary>
        /// Установка компонентов вектора из наибольших компонентов двух векторов.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        public void SetMaximize(in Vector3Df a, in Vector3Df b)
        {
            X = a.X > b.X ? a.X : b.X;
            Y = a.Y > b.Y ? a.Y : b.Y;
            Z = a.Z > b.Z ? a.Z : b.Z;
        }

        /// <summary>
        /// Установка компонентов вектора из наименьших компонентов двух векторов.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        public void SetMinimize(in Vector3Df a, in Vector3Df b)
        {
            X = a.X < b.X ? a.X : b.X;
            Y = a.Y < b.Y ? a.Y : b.Y;
            Z = a.Z < b.Z ? a.Z : b.Z;
        }

        /// <summary>
        /// Векторное произведение c нормализацией результата.
        /// </summary>
        /// <param name="left">Левый вектор.</param>
        /// <param name="right">Правый вектор.</param>
        public void CrossNormalize(in Vector3Df left, in Vector3Df right)
        {
            X = (left.Y * right.Z) - (left.Z * right.Y);
            Y = (left.Z * right.X) - (left.X * right.Z);
            Z = (left.X * right.Y) - (left.Y * right.X);
            var inv_length = XMath.InvSqrt((X * X) + (Y * Y) + (Z * Z));
            X *= inv_length;
            Y *= inv_length;
            Z *= inv_length;
        }

        /// <summary>
        /// Трансформация вектора как точки.
        /// </summary>
        /// <param name="matrix">Матрица трансформации.</param>
        public void TransformAsPoint(in Matrix4Dx4 matrix)
        {
            var x = (float)((X * matrix.M11) + (Y * matrix.M21) + (Z * matrix.M31) + matrix.M41);
            var y = (float)((X * matrix.M12) + (Y * matrix.M22) + (Z * matrix.M32) + matrix.M42);
            var z = (float)((X * matrix.M13) + (Y * matrix.M23) + (Z * matrix.M33) + matrix.M43);
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Трансформация вектора как вектора.
        /// </summary>
        /// <param name="matrix">Матрица трансформации.</param>
        public void TransformAsVector(in Matrix4Dx4 matrix)
        {
            var x = (float)((X * matrix.M11) + (Y * matrix.M21) + (Z * matrix.M31));
            var y = (float)((X * matrix.M12) + (Y * matrix.M22) + (Z * matrix.M32));
            var z = (float)((X * matrix.M13) + (Y * matrix.M23) + (Z * matrix.M33));
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Аппроксимация равенства значений векторов.
        /// </summary>
        /// <param name="other">Вектор.</param>
        /// <param name="epsilon">Погрешность.</param>
        /// <returns>Статус равенства значений векторов.</returns>
        public readonly bool Approximately(in Vector3Df other, float epsilon = 0.01f)
        {
            if (Math.Abs(X - other.X) < epsilon &&
                Math.Abs(Y - other.Y) < epsilon &&
                Math.Abs(Z - other.Z) < epsilon)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Сериализация вектора в строку.
        /// </summary>
        /// <returns>Строка данных.</returns>
        public readonly string SerializeToString()
        {
            return string.Format("{0};{1};{2}", X, Y, Z);
        }
        #endregion

        #region Convert methods
        /// <summary>
        /// Преобразование в вектор нулевой X компонентой.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector2Df ToVector2X()
        {
            return new Vector2Df(X, 0);
        }

        /// <summary>
        /// Преобразование в вектор нулевой Y компонентой.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector2Df ToVector2Y()
        {
            return new Vector2Df(0, Y);
        }

        /// <summary>
        /// Преобразование в двухмерных вектор плоскости XY.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector2Df ToVector2XY()
        {
            return new Vector2Df(X, Y);
        }

        /// <summary>
        /// Преобразование в двухмерных вектор плоскости YZ.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector2Df ToVector2XZ()
        {
            return new Vector2Df(X, Z);
        }

        /// <summary>
        /// Преобразование в двухмерных вектор плоскости YZ.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector2Df ToVector2YZ()
        {
            return new Vector2Df(Y, Z);
        }

        /// <summary>
        /// Преобразование в трехмерный вектор только с компонентой X.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector3Df ToVector3X()
        {
            return new Vector3Df(X, 0, 0);
        }

        /// <summary>
        /// Преобразование в трехмерный вектор только с компонентой Y.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector3Df ToVector3Y()
        {
            return new Vector3Df(0, Y, 0);
        }

        /// <summary>
        /// Преобразование в трехмерный вектор только с компонентой Z.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector3Df ToVector3Z()
        {
            return new Vector3Df(0, 0, Z);
        }

        /// <summary>
        /// Преобразование в трехмерный вектор плоскости XY.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector3Df ToVector3XY()
        {
            return new Vector3Df(X, Y, 0);
        }

        /// <summary>
        /// Преобразование в трехмерный вектор плоскости YZ.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector3Df ToVector3XZ()
        {
            return new Vector3Df(X, 0, Z);
        }

        /// <summary>
        /// Преобразование в трехмерный вектор плоскости YZ.
        /// </summary>
        /// <returns>Вектор.</returns>
        public readonly Vector3Df ToVector3YZ()
        {
            return new Vector3Df(0, Y, Z);
        }
        #endregion
    }
    /**@}*/
}