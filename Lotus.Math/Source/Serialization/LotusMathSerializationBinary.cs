using System.Collections.Generic;
using System.IO;

namespace Lotus.Maths
{
    /** \addtogroup MathSerialization
	*@{*/
    /// <summary>
    /// Статический класс реализующий методы расширения для сериализации математических типов в бинарный поток.
    /// </summary>
    /// <remarks>
    /// Реализация методов расширений потоков чтения и записи бинарных данных для сериализации математических типов.
    /// </remarks>
    public static class XMathSerializationBinary
    {
        #region Const
        /// <summary>
        /// Нулевые данные по значению в контексте записи/чтения ссылочных объектов бинарного потока.
        /// </summary>
        public const int ZERO_DATA = -1;

        /// <summary>
        /// Существующие данные по значению в контексте записи/чтения ссылочных объектов бинарного потока.
        /// </summary>
        public const int EXISTING_DATA = 1;

        /// <summary>
        /// Метка успешности.
        /// </summary>
        public const int SUCCESS_LABEL = 198418;
        #endregion

        #region Write methods
        /// <summary>
        /// Запись данных двухмерного вектора.
        /// </summary>
        /// <param name="writer">Бинарный поток открытый для записи.</param>
        /// <param name="vector">Двухмерный вектор.</param>
        public static void Write(this BinaryWriter writer, Vector2D vector)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
        }

        /// <summary>
        /// Запись данных двухмерного вектора, оптимизированная версия.
        /// </summary>
        /// <param name="writer">Бинарный поток открытый для записи.</param>
        /// <param name="vector">Двухмерный вектор.</param>
        public static void Write(this BinaryWriter writer, ref Vector2D vector)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
        }

        /// <summary>
        /// Запись данных списка двухмерных векторов.
        /// </summary>
        /// <param name="writer">Бинарный поток открытый для записи.</param>
        /// <param name="vectors">Список двухмерных векторов.</param>
        public static void Write(this BinaryWriter writer, IList<Vector2D> vectors)
        {
            // Проверка против нулевых значений
            if (vectors == null || vectors.Count == 0)
            {
                writer.Write(ZERO_DATA);
            }
            else
            {
                // Записываем длину
                writer.Write(vectors.Count);

                // Записываем данные по порядку
                for (var i = 0; i < vectors.Count; i++)
                {
                    writer.Write(vectors[i].X);
                    writer.Write(vectors[i].Y);
                }
            }
        }

        /// <summary>
        /// Запись данных прямоугольника.
        /// </summary>
        /// <param name="writer">Бинарный поток открытый для записи.</param>
        /// <param name="rect">Прямоугольник.</param>
        public static void Write(this BinaryWriter writer, Rect2D rect)
        {
            writer.Write(rect.X);
            writer.Write(rect.Y);
            writer.Write(rect.Width);
            writer.Write(rect.Height);
        }

        /// <summary>
        /// Запись данных прямоугольника, оптимизированная версия.
        /// </summary>
        /// <param name="writer">Бинарный поток открытый для записи.</param>
        /// <param name="rect">Прямоугольник.</param>
        public static void Write(this BinaryWriter writer, ref Rect2D rect)
        {
            writer.Write(rect.X);
            writer.Write(rect.Y);
            writer.Write(rect.Width);
            writer.Write(rect.Height);
        }

        /// <summary>
        /// Запись данных списка прямоугольников.
        /// </summary>
        /// <param name="writer">Бинарный поток открытый для записи.</param>
        /// <param name="rects">Список прямоугольников.</param>
        public static void Write(this BinaryWriter writer, IList<Rect2D> rects)
        {
            // Проверка против нулевых значений
            if (rects == null || rects.Count == 0)
            {
                writer.Write(ZERO_DATA);
            }
            else
            {
                // Записываем длину
                writer.Write(rects.Count);

                // Записываем данные по порядку
                for (var i = 0; i < rects.Count; i++)
                {
                    writer.Write(rects[i].X);
                    writer.Write(rects[i].Y);
                    writer.Write(rects[i].Width);
                    writer.Write(rects[i].Height);
                }
            }
        }
        #endregion

        #region Read methods 
        /// <summary>
        /// Чтение данных двухмерного вектора, оптимизированная версия.
        /// </summary>
        /// <param name="reader">Бинарный поток открытый для чтения.</param>
        /// <param name="vector">Двухмерный вектор.</param>
        public static void Read(this BinaryReader reader, ref Vector2D vector)
        {
            vector.X = reader.ReadDouble();
            vector.Y = reader.ReadDouble();
        }

        /// <summary>
        /// Чтение данных двухмерного вектора.
        /// </summary>
        /// <param name="reader">Бинарный поток открытый для чтения.</param>
        /// <returns>Двухмерный вектор.</returns>
        public static Vector2D ReadMathVector2D(this BinaryReader reader)
        {
            var vector = new Vector2D(reader.ReadDouble(), reader.ReadDouble());
            return vector;
        }

        /// <summary>
        /// Чтение данных массива двухмерных векторов.
        /// </summary>
        /// <param name="reader">Бинарный поток открытый для чтения.</param>
        /// <returns>Массив двухмерных векторов.</returns>
        public static Vector2D[]? ReadMathVectors2D(this BinaryReader reader)
        {
            // Чтение количество элементов массива
            var count = reader.ReadInt32();

            // Проверка нулевых данных
            if (count == ZERO_DATA)
            {
                return null;
            }
            else
            {
                // Создаем массив
                var vectors = new Vector2D[count];

                // Читаем данные по порядку
                for (var i = 0; i < count; i++)
                {
                    vectors[i].X = reader.ReadDouble();
                    vectors[i].Y = reader.ReadDouble();
                }

                return vectors;
            }
        }

        /// <summary>
        /// Чтение данных прямоугольника, оптимизированная версия.
        /// </summary>
        /// <param name="reader">Бинарный поток открытый для чтения.</param>
        /// <param name="rect">Прямоугольник.</param>
        public static void Read(this BinaryReader reader, ref Rect2D rect)
        {
            rect.X = reader.ReadDouble();
            rect.Y = reader.ReadDouble();
            rect.Width = reader.ReadDouble();
            rect.Height = reader.ReadDouble();
        }

        /// <summary>
        /// Чтение данных прямоугольника.
        /// </summary>
        /// <param name="reader">Бинарный поток открытый для чтения.</param>
        /// <returns>Прямоугольник.</returns>
        public static Rect2D ReadMathRect2D(this BinaryReader reader)
        {
            var rect = new Rect2D(reader.ReadDouble(),
                                 reader.ReadDouble(),
                                 reader.ReadDouble(),
                                 reader.ReadDouble());
            return rect;
        }

        /// <summary>
        /// Чтение данных массива прямоугольников.
        /// </summary>
        /// <param name="reader">Бинарный поток открытый для чтения.</param>
        /// <returns>Массив прямоугольников.</returns>
        public static Rect2D[]? ReadMathRects2D(this BinaryReader reader)
        {
            // Чтение количество элементов массива
            var count = reader.ReadInt32();

            // Проверка нулевых данных
            if (count == ZERO_DATA)
            {
                return null;
            }
            else
            {
                // Создаем массив
                var rects = new Rect2D[count];

                // Читаем данные по порядку
                for (var i = 0; i < count; i++)
                {
                    rects[i].X = reader.ReadDouble();
                    rects[i].Y = reader.ReadDouble();
                    rects[i].Width = reader.ReadDouble();
                    rects[i].Height = reader.ReadDouble();
                }

                return rects;
            }
        }
        #endregion
    }
    /**@}*/
}