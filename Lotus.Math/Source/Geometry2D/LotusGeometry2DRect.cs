using System;
using System.Runtime.InteropServices;

namespace Lotus.Maths
{
    /** \addtogroup MathGeometry2D
    *@{*/
    /// <summary>
    /// Структура прямоугольника в двухмерном пространстве.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect2D : IEquatable<Rect2D>, IComparable<Rect2D>
    {
        #region Const
        /// <summary>
        /// Пустой прямоугольник.
        /// </summary>
        public static readonly Rect2D Empty = new(0, 0, 0, 0);

        /// <summary>
        /// Прямоугольник по умолчанию.
        /// </summary>
        public static readonly Rect2D Default = new(0, 0, 100, 100);
        #endregion

        #region Static fields
        /// <summary>
        /// Текстовый формат отображения параметров прямоугольника.
        /// </summary>
        public static string ToStringFormat = "X = {0:0.00}; Y = {1:0.00}; W = {2:0.00}; H = {3:0.00}";
        #endregion

        #region Static methods
        /// <summary>
        /// Определение пересечения двух прямоугольников.
        /// </summary>
        /// <param name="a">Первый прямоугольник.</param>
        /// <param name="b">Второй прямоугольник.</param>
        /// <returns>Прямоугольник полученный в результате пересечения.</returns>
        public static Rect2D IntersectRect(in Rect2D a, in Rect2D b)
        {
            var x1 = Math.Max(a.X, b.X);
            var x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            var y1 = Math.Max(a.Y, b.Y);
            var y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new Rect2D(x1, y1, x2 - x1, y2 - y1);
            }

            return new Rect2D();
        }

        /// <summary>
        /// Объединение двух прямоугольников.
        /// </summary>
        /// <param name="a">Первый прямоугольник.</param>
        /// <param name="b">Второй прямоугольник.</param>
        /// <returns>Прямоугольник полученный в результате объединения.</returns>
        public static Rect2D UnionRect(in Rect2D a, in Rect2D b)
        {
            var x1 = Math.Min(a.X, b.X);
            var x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            var y1 = Math.Min(a.Y, b.Y);
            var y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new Rect2D(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Десереализация двухмерного прямоугольника из строки.
        /// </summary>
        /// <param name="data">Строка данных.</param>
        /// <returns>Прямоугольник.</returns>
        public static Rect2D DeserializeFromString(string data)
        {
            var rect = new Rect2D();
            var rect_data = data.Split(';');
            rect.X = XNumberHelper.ParseDouble(rect_data[0]);
            rect.Y = XNumberHelper.ParseDouble(rect_data[1]);
            rect.Width = XNumberHelper.ParseDouble(rect_data[2]);
            rect.Height = XNumberHelper.ParseDouble(rect_data[3]);
            return rect;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Позиция по X.
        /// </summary>
        public double X;

        /// <summary>
        /// Позиция по Y.
        /// </summary>
        public double Y;

        /// <summary>
        /// Ширина.
        /// </summary>
        public double Width;

        /// <summary>
        /// Высота.
        /// </summary>
        public double Height;
        #endregion

        #region Properties
        /// <summary>
        /// Статус пустого прямоугольника.
        /// </summary>
        public double Right
        {
            readonly get { return X + Width; }
            set
            {
                if (value > X)
                {
                    Width = value - X;
                }
            }
        }

        /// <summary>
        /// Статус пустого прямоугольника.
        /// </summary>
        public double Bottom
        {
            readonly get { return Y + Height; }
            set
            {
                if (value > Y)
                {
                    Height = value - Y;
                }
            }
        }

        /// <summary>
        /// Статус пустого прямоугольника.
        /// </summary>
        public readonly bool IsEmpty
        {
            get { return Width == 0 && Height == 0; }
        }

        /// <summary>
        /// Площадь.
        /// </summary>
        public readonly double Area
        {
            get { return Width * Height; }
        }

        /// <summary>
        /// Диагональ.
        /// </summary>
        public readonly double Diagonal
        {
            get { return Math.Sqrt((Width * Width) + (Height * Height)); }
        }

        /// <summary>
        /// Верхняя левая точка прямоугольника.
        /// </summary>
        public readonly Vector2D PointTopLeft
        {
            get { return new Vector2D(X, Y); }
        }

        /// <summary>
        /// Верхняя правая точка прямоугольника.
        /// </summary>
        public readonly Vector2D PointTopRight
        {
            get { return new Vector2D(X + Width, Y); }
        }

        /// <summary>
        /// Нижняя левая точка прямоугольника.
        /// </summary>
        public readonly Vector2D PointBottomLeft
        {
            get { return new Vector2D(X, Y + Height); }
        }

        /// <summary>
        /// Нижняя правая точка прямоугольника.
        /// </summary>
        public readonly Vector2D PointBottomRight
        {
            get { return new Vector2D(X + Width, Y + Height); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор инициализирует прямоугольник указанными параметрами.
        /// </summary>
        /// <param name="x">Позиция по X.</param>
        /// <param name="y">Позиция по Y.</param>
        /// <param name="width">Ширина.</param>
        /// <param name="height">Высота.</param>
        public Rect2D(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Конструктор инициализирует прямоугольник указанным прямоугольником.
        /// </summary>
        /// <param name="source">Прямоугольник.</param>
        public Rect2D(Rect2D source)
        {
            X = source.X;
            Y = source.Y;
            Width = source.Width;
            Height = source.Height;
        }

#if USE_WINDOWS
		/// <summary>
		/// Конструктор инициализирует прямоугольник указанным прямоугольником WPF.
		/// </summary>
		/// <param name="source">Прямоугольник WPF.</param>
		public Rect2D(System.Windows.Rect source)
		{
			X = source.X;
			Y = source.Y;
			Width = source.Width;
			Height = source.Height;
		}
#endif
#if USE_SHARPDX
		/// <summary>
		/// Конструктор инициализирует прямоугольник указанным прямоугольником SharpDX.
		/// </summary>
		/// <param name="source">Прямоугольник SharpDX.</param>
		public Rect2D(global::SharpDX.Rectangle source)
		{
			X = source.X;
			Y = source.Y;
			Width = source.Width;
			Height = source.Height;
		}

		/// <summary>
		/// Конструктор инициализирует прямоугольник указанным прямоугольником SharpDX.
		/// </summary>
		/// <param name="source">Прямоугольник SharpDX.</param>
		public Rect2D(global::SharpDX.RectangleF source)
		{
			X = source.X;
			Y = source.Y;
			Width = source.Width;
			Height = source.Height;
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
            if (obj is Rect2D rect)
            {
                return Equals(rect);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Проверка равенства прямоугольников по значению.
        /// </summary>
        /// <param name="other">Сравниваемый прямоугольник.</param>
        /// <returns>Статус равенства прямоугольников.</returns>
        public readonly bool Equals(Rect2D other)
        {
            return this == other;
        }

        /// <summary>
        /// Сравнение прямоугольников для упорядочивания.
        /// </summary>
        /// <param name="other">Прямоугольник.</param>
        /// <returns>Статус сравнения прямоугольников.</returns>
        public readonly int CompareTo(Rect2D other)
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
                    return 0;
                }
            }
        }

        /// <summary>
        /// Получение хеш-кода прямоугольника.
        /// </summary>
        /// <returns>Хеш-код прямоугольника.</returns>
        public override readonly int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <returns>Текстовое представление прямоугольника с указанием значений.</returns>
        public override readonly string ToString()
        {
            return string.Format(ToStringFormat, X, Y, Width, Height);
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <param name="format">Формат отображения.</param>
        /// <returns>Текстовое представление прямоугольника с указанием значений.</returns>
        public readonly string ToString(string format)
        {
            return "X = " + X.ToString(format) + "; Y = " + Y.ToString(format);
        }
        #endregion

        #region Operators
        /// <summary>
        /// Сравнение прямоугольников на равенство.
        /// </summary>
        /// <param name="left">Первый прямоугольник.</param>
        /// <param name="right">Второй прямоугольник.</param>
        /// <returns>Статус равенства прямоугольников.</returns>
        public static bool operator ==(Rect2D left, Rect2D right)
        {
            return left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;
        }

        /// <summary>
        /// Сравнение прямоугольников на неравенство.
        /// </summary>
        /// <param name="left">Первый прямоугольник.</param>
        /// <param name="right">Второй прямоугольник.</param>
        /// <returns>Статус неравенства прямоугольников.</returns>
        public static bool operator !=(Rect2D left, Rect2D right)
        {
            return left.X != right.X || left.Y != right.Y;
        }

        /// <summary>
        /// Реализация лексикографического порядка отношений прямоугольников.
        /// </summary>
        /// <param name="left">Левый прямоугольник.</param>
        /// <param name="right">Правый прямоугольник.</param>
        /// <returns>Статус меньше.</returns>
        public static bool operator <(Rect2D left, Rect2D right)
        {
            return left.X < right.X || (left.X == right.X && left.Y < right.Y);
        }

        /// <summary>
        /// Реализация лексикографического порядка отношений прямоугольников.
        /// </summary>
        /// <param name="left">Левый прямоугольник.</param>
        /// <param name="right">Правый прямоугольник.</param>
        /// <returns>Статус больше.</returns>
        public static bool operator >(Rect2D left, Rect2D right)
        {
            return left.X > right.X || (left.X == right.X && left.Y > right.Y);
        }
        #endregion

        #region Operators conversion
#if USE_WINDOWS
		/// <summary>
		/// Неявное преобразование в объект типа прямоугольника WPF.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>Прямоугольник WPF.</returns>
		public unsafe static implicit operator System.Windows.Rect(Rect2D rect)
		{
			return (*(System.Windows.Rect*)&rect);
		}
#endif
#if USE_SHARPDX
		/// <summary>
		/// Неявное преобразование в объект типа прямоугольника SharpDX.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>Прямоугольник SharpDX.</returns>
		public static implicit operator global::SharpDX.Rectangle(Rect2D rect)
		{
			return (new global::SharpDX.Rectangle((Int32)rect.X, (Int32)rect.Y, (Int32)rect.Width, (Int32)rect.Height));
		}

		/// <summary>
		/// Неявное преобразование в объект типа прямоугольника SharpDX.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>Прямоугольник SharpDX.</returns>
		public static implicit operator global::SharpDX.RectangleF(Rect2D rect)
		{
			return (new global::SharpDX.RectangleF((Single)rect.X, (Single)rect.Y, (Single)rect.Width, (Single)rect.Height));
		}


		/// <summary>
		/// Неявное преобразование в объект типа прямоугольника SharpDX.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>Прямоугольник SharpDX.</returns>
		public static implicit operator global::SharpDX.Mathematics.Interop.RawRectangle(Rect2D rect)
		{
			return (new global::SharpDX.Mathematics.Interop.RawRectangle((Int32)rect.X, (Int32)rect.Y, 
				(Int32)(rect.X + rect.Width), (Int32)(rect.Y + rect.Height)));
		}

		/// <summary>
		/// Неявное преобразование в объект типа прямоугольника SharpDX.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>Прямоугольник SharpDX.</returns>
		public static implicit operator global::SharpDX.Mathematics.Interop.RawRectangleF(Rect2D rect)
		{
			return (new global::SharpDX.Mathematics.Interop.RawRectangleF((Single)rect.X, (Single)rect.Y, 
				(Single)(rect.X + rect.Width), (Single)(rect.Y + rect.Height)));
		}
#endif
        #endregion

        #region Main methods
        /// <summary>
        /// Проверка на попадание точки в область прямоугольника.
        /// </summary>
        /// <param name="point">Проверяемая точка.</param>
        /// <returns>Статус попадания.</returns>
        public readonly bool Contains(in Vector2D point)
        {
            return X <= point.X && X + Width >= point.X && Y <= point.Y && Y + Height >= point.Y;
        }

        /// <summary>
        /// Смещение прямоугольника.
        /// </summary>
        /// <param name="offset">Смещение.</param>
        public void Offset(in Vector2D offset)
        {
            X += offset.X;
            Y += offset.Y;
        }

        /// <summary>
        /// Установка компонентов прямоугольника из наибольших компонентов двух прямоугольников.
        /// </summary>
        /// <param name="a">Первый прямоугольник.</param>
        /// <param name="b">Второй прямоугольник.</param>
        public void SetMaximize(in Rect2D a, in Rect2D b)
        {
            X = a.X > b.X ? a.X : b.X;
            Y = a.Y > b.Y ? a.Y : b.Y;
            Width = a.Width > b.Width ? a.Width : b.Width;
            Height = a.Height > b.Height ? a.Height : b.Height;
        }

        /// <summary>
        /// Установка компонентов прямоугольника из наименьших компонентов двух прямоугольников.
        /// </summary>
        /// <param name="a">Первый прямоугольник.</param>
        /// <param name="b">Второй прямоугольник.</param>
        public void SetMinimize(in Rect2D a, in Rect2D b)
        {
            X = a.X < b.X ? a.X : b.X;
            Y = a.Y < b.Y ? a.Y : b.Y;
            Width = a.Width < b.Width ? a.Width : b.Width;
            Height = a.Height < b.Height ? a.Height : b.Height;
        }

        /// <summary>
        /// Определение пересечения двух прямоугольников.
        /// </summary>
        /// <param name="rect">Прямоугольник.</param>
        public void SetIntersect(in Rect2D rect)
        {
            this = IntersectRect(in this, in rect);
        }

        /// <summary>
        /// Объединение двух прямоугольников.
        /// </summary>
        /// <param name="rect">Прямоугольник.</param>
        public void SetUnion(in Rect2D rect)
        {
            this = UnionRect(in this, in rect);
        }

        /// <summary>
        /// Увеличение размеров прямоугольника из центра на указанные величины.
        /// </summary>
        /// <param name="width">Ширина.</param>
        /// <param name="height">Высота.</param>
        public void Inflate(double width, double height)
        {
            X -= width;
            Y -= height;
            Width += 2 * width;
            Height += 2 * height;
        }

        /// <summary>
        /// Увеличение размеров прямоугольника для вхождения точки.
        /// </summary>
        /// <param name="point">Точка.</param>
        public void InflateInPoint(in Vector2D point)
        {
            if (X > point.X)
            {
                X = point.X;
            }
            else
            {
                if (point.X > X + Width)
                {
                    Width = point.X - X;
                }
            }

            if (Y > point.Y)
            {
                Y = point.Y;
            }
            else
            {
                if (point.Y > Y + Height)
                {
                    Height = point.Y - Y;
                }
            }
        }

        /// <summary>
        /// Сериализация прямоугольника в строку.
        /// </summary>
        /// <returns>Строка данных.</returns>
        public readonly string SerializeToString()
        {
            return string.Format("{0};{1};{2};{3}", X, Y, Width, Height);
        }
        #endregion
    }

    /// <summary>
    /// Структура прямоугольника в двухмерном пространстве.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct Rect2Df : IEquatable<Rect2Df>, IComparable<Rect2Df>
    {
        #region Const
        /// <summary>
        /// Пустой прямоугольник.
        /// </summary>
        public static readonly Rect2Df Empty = new(0, 0, 0, 0);

        /// <summary>
        /// Прямоугольник по умолчанию.
        /// </summary>
        public static readonly Rect2Df Default = new(0, 0, 100, 100);
        #endregion

        #region Static fields
        /// <summary>
        /// Текстовый формат отображения параметров прямоугольника.
        /// </summary>
        public static string ToStringFormat = "X = {0:0.00}; Y = {1:0.00}; W = {2:0.00}; H = {3:0.00}";
        #endregion

        #region Static methods
        /// <summary>
        /// Определение пересечения двух прямоугольников.
        /// </summary>
        /// <param name="a">Первый прямоугольник.</param>
        /// <param name="b">Второй прямоугольник.</param>
        /// <returns>Прямоугольник полученный в результате пересечения.</returns>
        public static Rect2Df IntersectRect(in Rect2Df a, in Rect2Df b)
        {
            IntersectRect(in a, in b, out var result);
            return result;
        }

        /// <summary>
        /// Определение пересечения двух прямоугольников.
        /// </summary>
        /// <param name="a">Первый прямоугольник.</param>
        /// <param name="b">Второй прямоугольник.</param>
        /// <param name="result">Прямоугольник полученный в результате пересечения.</param>
        public static void IntersectRect(in Rect2Df a, in Rect2Df b, out Rect2Df result)
        {
            var x1 = Math.Max(a.X, b.X);
            var x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            var y1 = Math.Max(a.Y, b.Y);
            var y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                result.X = x1;
                result.Y = y1;
                result.Width = x2 - x1;
                result.Height = y2 - y1;
            }

            result = Empty;
        }

        /// <summary>
        /// Объединение двух прямоугольников.
        /// </summary>
        /// <param name="a">Первый прямоугольник.</param>
        /// <param name="b">Второй прямоугольник.</param>
        /// <returns>Прямоугольник полученный в результате объединения.</returns>
        public static Rect2Df UnionRect(in Rect2Df a, in Rect2Df b)
        {
            UnionRect(in a, in b, out var result);
            return result;
        }

        /// <summary>
        /// Объединение двух прямоугольников.
        /// </summary>
        /// <param name="a">Первый прямоугольник.</param>
        /// <param name="b">Второй прямоугольник.</param>
        /// <param name="result">Прямоугольник полученный в результате объединения.</param>
        public static void UnionRect(in Rect2Df a, in Rect2Df b, out Rect2Df result)
        {
            var x1 = Math.Min(a.X, b.X);
            var x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            result.X = x1;
            result.Width = x2 - x1;

            var y1 = Math.Min(a.Y, b.Y);
            var y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);
            result.Y = y1;
            result.Height = y2 - y1;
        }

        /// <summary>
        /// Десереализация прямоугольника из строки.
        /// </summary>
        /// <param name="data">Строка данных.</param>
        /// <returns>Прямоугольник.</returns>
        public static Rect2Df DeserializeFromString(string data)
        {
            var rect = new Rect2Df();
            var rect_data = data.Split(';');
            rect.X = XNumberHelper.ParseSingle(rect_data[0]);
            rect.Y = XNumberHelper.ParseSingle(rect_data[1]);
            rect.Width = XNumberHelper.ParseSingle(rect_data[2]);
            rect.Height = XNumberHelper.ParseSingle(rect_data[3]);
            return rect;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Позиция по X.
        /// </summary>
        public float X;

        /// <summary>
        /// Позиция по Y.
        /// </summary>
        public float Y;

        /// <summary>
        /// Ширина.
        /// </summary>
        public float Width;

        /// <summary>
        /// Высота.
        /// </summary>
        public float Height;
        #endregion

        #region Properties
        /// <summary>
        /// Центр прямоугольника.
        /// </summary>
        public Vector2Df Center
        {
            readonly get { return new Vector2Df(X + (Width / 2), Y + (Height / 2)); }
            set
            {
                X = value.X - (Width / 2);
                Y = value.Y - (Height / 2);
            }
        }

        /// <summary>
        /// Правая сторона прямоугольника.
        /// </summary>
        public float Right
        {
            readonly get { return X + Width; }
            set
            {
                if (value > X)
                {
                    Width = value - X;
                }
            }
        }

        /// <summary>
        /// Нижняя сторона прямоугольника.
        /// </summary>
        public float Bottom
        {
            readonly get { return Y + Height; }
            set
            {
                if (value > Y)
                {
                    Height = value - Y;
                }
            }
        }

        /// <summary>
        /// Статус пустого прямоугольника.
        /// </summary>
        public readonly bool IsEmpty
        {
            get { return Width == 0 && Height == 0; }
        }

        /// <summary>
        /// Площадь прямоугольника.
        /// </summary>
        public readonly float Area
        {
            get { return Width * Height; }
        }

        /// <summary>
        /// Диагональ прямоугольника.
        /// </summary>
        public readonly float Diagonal
        {
            get { return (float)Math.Sqrt((Width * Width) + (Height * Height)); }
        }

        /// <summary>
        /// Верхняя левая точка прямоугольника.
        /// </summary>
        public Vector2Df PointTopLeft
        {
            readonly get { return new Vector2Df(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Верхняя правая точка прямоугольника.
        /// </summary>
        public Vector2Df PointTopRight
        {
            readonly get { return new Vector2Df(X + Width, Y); }
            set
            {
                Width = value.X - X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Нижняя левая точка прямоугольника.
        /// </summary>
        public Vector2Df PointBottomLeft
        {
            readonly get { return new Vector2Df(X, Y + Height); }
            set
            {
                X = value.X;
                Height = value.Y - Y;
            }
        }

        /// <summary>
        /// Нижняя правая точка прямоугольника.
        /// </summary>
        public Vector2Df PointBottomRight
        {
            readonly get { return new Vector2Df(X + Width, Y + Height); }
            set
            {
                Width = value.X - X;
                Height = value.Y - Y;
            }
        }

        /// <summary>
        /// Верхняя левая точка прямоугольника.
        /// </summary>
        public readonly Vector2Df PointTopLeftRightMiddle
        {
            get { return new Vector2Df(X + (Width / 2), Y); }
        }

        /// <summary>
        /// Верхняя левая точка прямоугольника.
        /// </summary>
        public readonly Vector2Df PointBottomLeftRightMiddle
        {
            get { return new Vector2Df(X + (Width / 2), Y + Height); }
        }

        /// <summary>
        /// Верхняя левая точка прямоугольника.
        /// </summary>
        public readonly Vector2Df PointLeftTopBottomMiddle
        {
            get { return new Vector2Df(X, Y + (Height / 2)); }
        }

        /// <summary>
        /// Верхняя левая точка прямоугольника.
        /// </summary>
        public readonly Vector2Df PointRightTopBottomMiddle
        {
            get { return new Vector2Df(X + Width, Y + (Height / 2)); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор инициализирует прямоугольник указанными параметрами.
        /// </summary>
        /// <param name="x">Позиция по X.</param>
        /// <param name="y">Позиция по Y.</param>
        /// <param name="width">Ширина.</param>
        /// <param name="height">Высота.</param>
        public Rect2Df(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Конструктор инициализирует прямоугольник указанным прямоугольником.
        /// </summary>
        /// <param name="source">Прямоугольник.</param>
        public Rect2Df(Rect2Df source)
        {
            X = source.X;
            Y = source.Y;
            Width = source.Width;
            Height = source.Height;
        }

#if USE_WINDOWS
		/// <summary>
		/// Конструктор инициализирует прямоугольник указанным прямоугольником WPF.
		/// </summary>
		/// <param name="source">Прямоугольник WPF.</param>
		public Rect2Df(System.Windows.Rect source)
		{
			X = (Single)source.X;
			Y = (Single)source.Y;
			Width = (Single)source.Width;
			Height = (Single)source.Height;
		}
#endif

#if USE_GDI
		/// <summary>
		/// Конструктор инициализирует прямоугольник указанным прямоугольником System.Drawing.
		/// </summary>
		/// <param name="source">Прямоугольник System.Drawing.</param>
		public Rect2Df(System.Drawing.Rectangle source)
		{
			X = (Single)source.X;
			Y = (Single)source.Y;
			Width = (Single)source.Width;
			Height = (Single)source.Height;
		}

		/// <summary>
		/// Конструктор инициализирует прямоугольник указанным прямоугольником System.Drawing.
		/// </summary>
		/// <param name="source">Прямоугольник System.Drawing.</param>
		public Rect2Df(System.Drawing.RectangleF source)
		{
			X = source.X;
			Y = source.Y;
			Width = source.Width;
			Height = source.Height;
		}
#endif

#if USE_SHARPDX
		/// <summary>
		/// Конструктор инициализирует прямоугольник указанным прямоугольником SharpDX.
		/// </summary>
		/// <param name="source">Прямоугольник SharpDX.</param>
		public Rect2Df(SharpDX.Rectangle source)
		{
			X = source.X;
			Y = source.Y;
			Width = source.Width;
			Height = source.Height;
		}

		/// <summary>
		/// Конструктор инициализирует прямоугольник указанным прямоугольником SharpDX.
		/// </summary>
		/// <param name="source">Прямоугольник SharpDX.</param>
		public Rect2Df(SharpDX.RectangleF source)
		{
			X = source.X;
			Y = source.Y;
			Width = source.Width;
			Height = source.Height;
		}

		/// <summary>
		/// Конструктор инициализирует прямоугольник указанным прямоугольником SharpDX.
		/// </summary>
		/// <param name="source">Прямоугольник SharpDX.</param>
		public Rect2Df(SharpDX.Mathematics.Interop.RawRectangleF source)
		{
			X = source.Left;
			Y = source.Top;
			Width = source.Right - source.Left;
			Height = source.Bottom - source.Top;
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
            if (obj is Rect2Df rect)
            {
                return Equals(rect);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Проверка равенства прямоугольников по значению.
        /// </summary>
        /// <param name="other">Сравниваемый прямоугольник.</param>
        /// <returns>Статус равенства прямоугольников.</returns>
        public readonly bool Equals(Rect2Df other)
        {
            return this == other;
        }

        /// <summary>
        /// Сравнение прямоугольников для упорядочивания.
        /// </summary>
        /// <param name="other">Сравниваемый прямоугольник.</param>
        /// <returns>Статус сравнения прямоугольников.</returns>
        public readonly int CompareTo(Rect2Df other)
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
                    return 0;
                }
            }
        }

        /// <summary>
        /// Получение хеш-кода прямоугольника.
        /// </summary>
        /// <returns>Хеш-код прямоугольника.</returns>
        public override readonly int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <returns>Текстовое представление прямоугольника с указанием значений.</returns>
        public override readonly string ToString()
        {
            return string.Format(ToStringFormat, X, Y, Width, Height);
        }

        /// <summary>
        /// Преобразование к текстовому представлению.
        /// </summary>
        /// <param name="format">Формат отображения.</param>
        /// <returns>Текстовое представление прямоугольника с указанием значений.</returns>
        public readonly string ToString(string format)
        {
            return "X = " + X.ToString(format) + "; Y = " + Y.ToString(format);
        }
        #endregion

        #region Operators
        /// <summary>
        /// Сравнение прямоугольников на равенство.
        /// </summary>
        /// <param name="left">Первый прямоугольник.</param>
        /// <param name="right">Второй прямоугольник.</param>
        /// <returns>Статус равенства прямоугольников.</returns>
        public static bool operator ==(Rect2Df left, Rect2Df right)
        {
            return left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;
        }

        /// <summary>
        /// Сравнение прямоугольников на неравенство.
        /// </summary>
        /// <param name="left">Первый прямоугольник.</param>
        /// <param name="right">Второй прямоугольник.</param>
        /// <returns>Статус неравенства прямоугольников.</returns>
        public static bool operator !=(Rect2Df left, Rect2Df right)
        {
            return left.X != right.X || left.Y != right.Y;
        }

        /// <summary>
        /// Реализация лексикографического порядка отношений прямоугольников.
        /// </summary>
        /// <param name="left">Левый прямоугольник.</param>
        /// <param name="right">Правый прямоугольник.</param>
        /// <returns>Статус меньше.</returns>
        public static bool operator <(Rect2Df left, Rect2Df right)
        {
            return left.X < right.X || (left.X == right.X && left.Y < right.Y);
        }

        /// <summary>
        /// Реализация лексикографического порядка отношений прямоугольников.
        /// </summary>
        /// <param name="left">Левый прямоугольник.</param>
        /// <param name="right">Правый прямоугольник.</param>
        /// <returns>Статус больше.</returns>
        public static bool operator >(Rect2Df left, Rect2Df right)
        {
            return left.X > right.X || (left.X == right.X && left.Y > right.Y);
        }
        #endregion

        #region Operators conversion
#if UNITY_2017_1_OR_NEWER
		/// <summary>
		/// Неявное преобразование в объект типа UnityEngine.Rect.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>UnityEngine.Rect.</returns>
		public static implicit operator UnityEngine.Rect(Rect2Df rect)
		{
			return new UnityEngine.Rect(rect.X, rect.Y, rect.Width, rect.Height);
		}
#endif
#if USE_WINDOWS
		/// <summary>
		/// Неявное преобразование в объект типа прямоугольника WPF.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>Прямоугольник WPF.</returns>
		public static implicit operator System.Windows.Rect(Rect2Df rect)
		{
			return (new System.Windows.Rect(rect.X, rect.Y, rect.Width, rect.Height));
		}
#endif
#if USE_GDI
		/// <summary>
		/// Неявное преобразование в объект типа прямоугольника System.Drawing.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>Прямоугольник System.Drawing.</returns>
		public static implicit operator System.Drawing.Rectangle(Rect2Df rect)
		{
			return (new System.Drawing.Rectangle((Int32)rect.X, (Int32)rect.Y, (Int32)rect.Width, (Int32)rect.Height));
		}

		/// <summary>
		/// Неявное преобразование в объект типа прямоугольника System.Drawing.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>Прямоугольник System.Drawing.</returns>
		public static implicit operator System.Drawing.RectangleF(Rect2Df rect)
		{
			return (new System.Drawing.RectangleF(rect.X, rect.Y, rect.Width, rect.Height));
		}
#endif
#if USE_SHARPDX
		/// <summary>
		/// Неявное преобразование в объект типа прямоугольника SharpDX.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>Прямоугольник SharpDX.</returns>
		public static implicit operator SharpDX.Rectangle(Rect2Df rect)
		{
			return (new SharpDX.Rectangle((Int32)rect.X, (Int32)rect.Y, (Int32)rect.Width, (Int32)rect.Height));
		}

		/// <summary>
		/// Неявное преобразование в объект типа прямоугольника SharpDX.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>Прямоугольник SharpDX.</returns>
		public static implicit operator SharpDX.RectangleF(Rect2Df rect)
		{
			return (new SharpDX.RectangleF(rect.X, rect.Y, rect.Width, rect.Height));
		}

		/// <summary>
		/// Неявное преобразование в объект типа прямоугольника SharpDX.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>Прямоугольник SharpDX.</returns>
		public static implicit operator SharpDX.Mathematics.Interop.RawRectangle(Rect2Df rect)
		{
			return (new SharpDX.Mathematics.Interop.RawRectangle((Int32)rect.X, (Int32)rect.Y,
				(Int32)(rect.X + rect.Width), (Int32)(rect.Y + rect.Height)));
		}

		/// <summary>
		/// Неявное преобразование в объект типа прямоугольника SharpDX.
		/// </summary>
		/// <param name="rect">Прямоугольник.</param>
		/// <returns>Прямоугольник SharpDX.</returns>
		public static implicit operator SharpDX.Mathematics.Interop.RawRectangleF(Rect2Df rect)
		{
			return (new SharpDX.Mathematics.Interop.RawRectangleF(rect.X, rect.Y,
				(rect.X + rect.Width), (rect.Y + rect.Height)));
		}
#endif
        #endregion

        #region Main methods
        /// <summary>
        /// Проверка на попадание точки в область прямоугольника.
        /// </summary>
        /// <param name="point">Проверяемая точка.</param>
        /// <returns>Статус попадания.</returns>
        public readonly bool Contains(in Vector2Df point)
        {
            return X <= point.X && X + Width >= point.X && Y <= point.Y && Y + Height >= point.Y;
        }

        /// <summary>
        /// Смещение прямоугольника.
        /// </summary>
        /// <param name="offset">Смещение.</param>
        public void Offset(in Vector2Df offset)
        {
            X += offset.X;
            Y += offset.Y;
        }

        /// <summary>
        /// Смещение прямоугольника.
        /// </summary>
        /// <param name="x">Смещение по X.</param>
        /// <param name="y">Смещение по Y.</param>
        public void Offset(float x, float y)
        {
            X += x;
            Y += y;
        }

        /// <summary>
        /// Установка компонентов прямоугольника из наибольших компонентов двух прямоугольников.
        /// </summary>
        /// <param name="a">Первый прямоугольник.</param>
        /// <param name="b">Второй прямоугольник.</param>
        public void SetMaximize(in Rect2Df a, in Rect2Df b)
        {
            X = a.X > b.X ? a.X : b.X;
            Y = a.Y > b.Y ? a.Y : b.Y;
            Width = a.Width > b.Width ? a.Width : b.Width;
            Height = a.Height > b.Height ? a.Height : b.Height;
        }

        /// <summary>
        /// Установка компонентов прямоугольника из наименьших компонентов двух прямоугольников.
        /// </summary>
        /// <param name="a">Первый прямоугольник.</param>
        /// <param name="b">Второй прямоугольник.</param>
        public void SetMinimize(in Rect2Df a, in Rect2Df b)
        {
            X = a.X < b.X ? a.X : b.X;
            Y = a.Y < b.Y ? a.Y : b.Y;
            Width = a.Width < b.Width ? a.Width : b.Width;
            Height = a.Height < b.Height ? a.Height : b.Height;
        }

        /// <summary>
        /// Определение пересечения двух прямоугольников.
        /// </summary>
        /// <param name="rect">Прямоугольник.</param>
        public void SetIntersect(in Rect2Df rect)
        {
            IntersectRect(in this, in rect, out this);
        }

        /// <summary>
        /// Объединение двух прямоугольников.
        /// </summary>
        /// <param name="rect">Прямоугольник.</param>
        public void SetUnion(in Rect2Df rect)
        {
            UnionRect(in this, in rect, out this);
        }

        /// <summary>
        /// Объединение двух прямоугольников.
        /// </summary>
        /// <param name="rect">Прямоугольник.</param>
        public readonly void SetBoundsRect(Rect2Df rect)
        {
            //UnionRect(in this, in rect, out this);
        }

        /// <summary>
        /// Увеличение размеров прямоугольника из центра на указанные величины.
        /// </summary>
        /// <param name="width">Ширина.</param>
        /// <param name="height">Высота.</param>
        public void Inflate(float width, float height)
        {
            X -= width;
            Y -= height;
            Width += 2 * width;
            Height += 2 * height;
        }

        /// <summary>
        /// Увеличение размеров прямоугольника для вхождения точки.
        /// </summary>
        /// <param name="point">Точка.</param>
        public void InflateInPoint(in Vector2Df point)
        {
            if (X > point.X)
            {
                X = point.X;
            }
            else
            {
                if (point.X > X + Width)
                {
                    Width = point.X - X;
                }
            }

            if (Y > point.Y)
            {
                Y = point.Y;
            }
            else
            {
                if (point.Y > Y + Height)
                {
                    Height = point.Y - Y;
                }
            }
        }

        /// <summary>
        /// Сериализация прямоугольника в строку.
        /// </summary>
        /// <returns>Строка данных.</returns>
        public readonly string SerializeToString()
        {
            return string.Format("{0};{1};{2};{3}", X, Y, Width, Height);
        }
        #endregion
    }
    /**@}*/
}