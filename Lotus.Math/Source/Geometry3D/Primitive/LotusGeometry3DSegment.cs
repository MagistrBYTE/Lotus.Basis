﻿//=====================================================================================================================
// Проект: Модуль математической системы
// Раздел: Подсистема 3D геометрии
// Подраздел: Трехмерные геометрические примитивы
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGeometry3DSegment.cs
*		Структура для представления сегмента в трехмерном пространстве.
*		Реализация структуры для представления сегмента(отрезка) в трехмерном пространстве.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
using System.Runtime.InteropServices;
//=====================================================================================================================
namespace Lotus
{
	namespace Maths
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup MathGeometry3D
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Структура сегмента(отрезка) в трехмерном пространстве
		/// </summary>
		/// <remarks>
		/// Сегмент(отрезок) характеризуется начальной и конечной точкой
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public struct Segment3Df : IEquatable<Segment3Df>, IComparable<Segment3Df>, ICloneable
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			/// <summary>
			/// Текстовый формат отображения параметров сегмента
			/// </summary>
			public static String ToStringFormat = "Start = {0:0.00}, {1:0.00}, {2:0.00}; End = {3:0.00}, {4:0.00}, {5:0.00}";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Начало сегмента
			/// </summary>
			public Vector3Df Start;

			/// <summary>
			/// Окончание сегмента
			/// </summary>
			public Vector3Df End;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Центр сегмента
			/// </summary>
			public readonly Vector3Df Location
			{
				get { return (Start + End) / 2; }
			}

			/// <summary>
			/// Направление сегмента
			/// </summary>
			public readonly Vector3Df Direction
			{
				get { return End - Start; }
			}

			/// <summary>
			/// Единичное направление сегмента
			/// </summary>
			public readonly Vector3Df DirectionUnit
			{
				get { return (End - Start).Normalized; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует сегмент указанными параметрами
			/// </summary>
			/// <param name="start">Начало сегмента</param>
			/// <param name="end">Окончание сегмента</param>
			//---------------------------------------------------------------------------------------------------------
			public Segment3Df(Vector3Df start, Vector3Df end)
			{
				Start = start;
				End = end;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует сегмент указанной линией
			/// </summary>
			/// <param name="source">Сегмент</param>
			//---------------------------------------------------------------------------------------------------------
			public Segment3Df(Segment3Df source)
			{
				Start = source.Start;
				End = source.End;
			}

#if UNITY_2017_1_OR_NEWER
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует сегмент указанными параметрами
			/// </summary>
			/// <param name="start">Начало сегмента</param>
			/// <param name="end">Окончание сегмента</param>
			//---------------------------------------------------------------------------------------------------------
			public Segment3Df(UnityEngine.Vector3 start, UnityEngine.Vector3 end)
			{
				Start = new Vector3Df(start.x, start.y, start.z);
				End = new Vector3Df(end.x, end.y, end.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует сегмент указанным лучом
			/// </summary>
			/// <param name="ray">Луч</param>
			/// <param name="length">Длина сегмента</param>
			//---------------------------------------------------------------------------------------------------------
			public Segment3Df(UnityEngine.Ray ray, Single length)
			{
				Start = new Vector3Df(ray.origin.x, ray.origin.y, ray.origin.z);
				End = new Vector3Df(ray.origin.x + (ray.direction.x * length), 
					ray.origin.y + (ray.direction.y * length),
					ray.origin.z + (ray.direction.z * length));
			}
#endif
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверяет равен ли текущий объект другому объекту того же типа
			/// </summary>
			/// <param name="obj">Сравниваемый объект</param>
			/// <returns>Статус равенства объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public override readonly Boolean Equals(Object obj)
			{
				if (obj != null)
				{
					if (obj is Segment3Df segment)
					{
						return Equals(segment);
					}
				}
				return base.Equals(obj);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка равенства сегментов по значению
			/// </summary>
			/// <param name="other">Сравниваемый сегмент</param>
			/// <returns>Статус равенства сегментов</returns>
			//---------------------------------------------------------------------------------------------------------
			public readonly Boolean Equals(Segment3Df other)
			{
				return this == other;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение сегментов для упорядочивания
			/// </summary>
			/// <param name="other">Сравниваемый сегмент</param>
			/// <returns>Статус сравнения сегментов</returns>
			//---------------------------------------------------------------------------------------------------------
			public readonly Int32 CompareTo(Segment3Df other)
			{
				if (Start > other.Start)
				{
					return 1;
				}
				else
				{
					if (Start == other.Start && End > other.End)
					{
						return 1;
					}
					else
					{
						return 0;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение хеш-кода сегмента
			/// </summary>
			/// <returns>Хеш-код сегмента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override readonly Int32 GetHashCode()
			{
				return Start.GetHashCode() ^ End.GetHashCode();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Полное копирование сегмента
			/// </summary>
			/// <returns>Копия сегмента</returns>
			//---------------------------------------------------------------------------------------------------------
			public readonly Object Clone()
			{
				return MemberwiseClone();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление сегмента с указанием значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public override readonly String ToString()
			{
				return String.Format(ToStringFormat, Start.X, Start.Y, Start.Z, End.X, End.Y, End.Z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <param name="format">Формат отображения</param>
			/// <returns>Текстовое представление сегмента с указанием значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public readonly String ToString(String format)
			{
				return "Start = " + Start.ToString(format) + "; End = " + End.ToString(format);
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ =================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение сегментов на равенство
			/// </summary>
			/// <param name="left">Первый сегмент</param>
			/// <param name="right">Второй сегмент</param>
			/// <returns>Статус равенства сегментов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator ==(Segment3Df left, Segment3Df right)
			{
				return left.Start == right.Start && left.End == right.End;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение сегментов на неравенство
			/// </summary>
			/// <param name="left">Первый сегмент</param>
			/// <param name="right">Второй сегмент</param>
			/// <returns>Статус неравенства сегментов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator !=(Segment3Df left, Segment3Df right)
			{
				return left.Start != right.Start || left.End != right.End;
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ ПРЕОБРАЗОВАНИЯ ==================================
#if UNITY_2017_1_OR_NEWER
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Неявное преобразование в объект типа <see cref="UnityEngine.Ray"/> 
			/// </summary>
			/// <param name="segment">Сегмент</param>
			/// <returns>Объект <see cref="UnityEngine.Ray"/></returns>
			//---------------------------------------------------------------------------------------------------------
			public static implicit operator UnityEngine.Ray(Segment3Df segment)
			{
				return new UnityEngine.Ray(segment.Start, segment.DirectionUnit);
			}
#endif
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на попадание точки на сегмент
			/// </summary>
			/// <param name="point">Проверяемая точка</param>
			/// <returns>Статус попадания</returns>
			//---------------------------------------------------------------------------------------------------------
			public readonly Boolean Contains(in Vector3Df point)
			{
				return XIntersect3D.PointSegment(in point, in Start, in End);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================