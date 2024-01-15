﻿//=====================================================================================================================
// Проект: Модуль математической системы
// Раздел: Подсистема 2D геометрии
// Подраздел: Двухмерные геометрические примитивы
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGeometry2DCircle.cs
*		Структура для представления окружности в двухмерном пространстве.
*		Реализация структуры для представления окружности в двухмерном пространстве.
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
		/** \addtogroup MathGeometry2D
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Структура окружности в двухмерном пространстве
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public struct Circle2Df : IEquatable<Circle2Df>, IComparable<Circle2Df>, ICloneable
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			/// <summary>
			/// Текстовый формат отображения параметров окружности
			/// </summary>
			public static String ToStringFormat = "Center = {0:0.00}, {1:0.00}; Radius = {3:0.00}";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Центр окружности
			/// </summary>
			public Vector2Df Center;

			/// <summary>
			/// Радиус окружности
			/// </summary>
			public Single Radius;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Длина окружности
			/// </summary>
			public Single Circumference
			{
				readonly get { return Radius * XMath.PI_2_F; }
				set
				{
					Radius = value / XMath.PI_2_F;
				}
			}

			/// <summary>
			/// Диаметр окружности
			/// </summary>
			public Single Diameter
			{
				readonly get { return 2 * Radius; }
				set { Radius = value / 2; }
			}

			/// <summary>
			/// Площадь окружности
			/// </summary>
			public Single Area
			{
				readonly get { return XMath.PI_F * Radius * Radius; }
				set { Radius = XMath.Sqrt(value / XMath.PI_F); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует окружность указанными параметрами
			/// </summary>
			/// <param name="center">Центр окружности</param>
			/// <param name="radius">Радиус окружности</param>
			//---------------------------------------------------------------------------------------------------------
			public Circle2Df(Vector2Df center, Single radius)
			{
				Center = center;
				Radius = radius;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует окружность указанной окружностью
			/// </summary>
			/// <param name="source">Окружность</param>
			//---------------------------------------------------------------------------------------------------------
			public Circle2Df(Circle2Df source)
			{
				Center = source.Center;
				Radius = source.Radius;
			}

#if UNITY_2017_1_OR_NEWER
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует окружность указанными параметрами
			/// </summary>
			/// <param name="center">Центр окружности</param>
			/// <param name="radius">Радиус окружности</param>
			//---------------------------------------------------------------------------------------------------------
			public Circle2Df(UnityEngine.Vector2 center, Single radius)
			{
				Center = new Vector2Df(center.x, center.y);
				Radius = radius;
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
			public override readonly Boolean Equals(System.Object? obj)
			{
				if (obj != null)
				{
					if (obj is Circle2Df circle)
					{
						return Equals(circle);
					}
				}
				return base.Equals(obj);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка равенства окружностей по значению
			/// </summary>
			/// <param name="other">Сравниваемая окружность</param>
			/// <returns>Статус равенства окружностей</returns>
			//---------------------------------------------------------------------------------------------------------
			public readonly Boolean Equals(Circle2Df other)
			{
				return this == other;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение окружностей для упорядочивания
			/// </summary>
			/// <param name="other">Сравниваемый окружность</param>
			/// <returns>Статус сравнения окружностей</returns>
			//---------------------------------------------------------------------------------------------------------
			public readonly Int32 CompareTo(Circle2Df other)
			{
				if (Center > other.Center)
				{
					return 1;
				}
				else
				{
					if (Center == other.Center && Radius > other.Radius)
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
			/// Получение хеш-кода окружности
			/// </summary>
			/// <returns>Хеш-код окружности</returns>
			//---------------------------------------------------------------------------------------------------------
			public override readonly Int32 GetHashCode()
			{
				return Center.GetHashCode() ^ Radius.GetHashCode();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Полное копирование окружности
			/// </summary>
			/// <returns>Копия окружности</returns>
			//---------------------------------------------------------------------------------------------------------
			public readonly Object Clone()
			{
				return MemberwiseClone();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление окружности с указанием значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public override readonly String ToString()
			{
				return String.Format(ToStringFormat, Center.X, Center.Y, Radius);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <param name="format">Формат отображения</param>
			/// <returns>Текстовое представление окружности с указанием значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public readonly String ToString(String format)
			{
				return "Center = " + Center.ToString(format) + "; Radius = " + Radius.ToString(format);
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ =================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение окружностей на равенство
			/// </summary>
			/// <param name="left">Первая окружность</param>
			/// <param name="right">Вторая окружность</param>
			/// <returns>Статус равенства окружностей</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator ==(Circle2Df left, Circle2Df right)
			{
				return left.Center == right.Center && left.Radius == right.Radius;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение окружностей на неравенство
			/// </summary>
			/// <param name="left">Первая окружность</param>
			/// <param name="right">Вторая окружность</param>
			/// <returns>Статус неравенства окружностей</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator !=(Circle2Df left, Circle2Df right)
			{
				return left.Center != right.Center || left.Radius != right.Radius;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на попадание точки в область окружности
			/// </summary>
			/// <param name="point">Проверяемая точка</param>
			/// <returns>Статус попадания</returns>
			//---------------------------------------------------------------------------------------------------------
			public readonly Boolean Contains(in Vector2Df point)
			{
				var d = Vector2Df.Distance(in Center, in point);
				return Math.Abs(d - Radius) < XGeometry2D.Eplsilon_f;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================