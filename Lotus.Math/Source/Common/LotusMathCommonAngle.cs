//=====================================================================================================================
// Проект: Модуль математической системы
// Раздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusMathCommonAngle.cs
*		Работа с углами и их единицами.
*		Реализация методов работы с углами и различным единицами их представления.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
//=====================================================================================================================
namespace Lotus
{
	namespace Maths
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup MathCommon Общая подсистема
		//! Общая математическая подсистема реализует работу с основными, общими структурами данных.
		//! Сюда входит работа с углами, методы расширений для потоков данных применительно к математическим структурам,
		//! математические вычисления основанные на предварительно вычисленных(кэшированных) данных.
		//! \ingroup Math
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методов работы с углами и различным единицами их представления
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XMathAngle
		{
			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Нормализация угла в пределах от 0 до 360
			/// </summary>
			/// <param name="angle">Угол, задается в градусах</param>
			/// <returns>Нормализованный угол в пределах от 0 до 360</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Double NormalizationFull(Double angle)
			{
				Double degree_norm = angle;
				if (angle >= 360.0 || angle < 0.0)
				{
					degree_norm -= Math.Floor(angle / 360.0) * 360.0;
				}
				return degree_norm;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Нормализация угла в пределах от 0 до 360
			/// </summary>
			/// <param name="angle">Угол, задается в градусах</param>
			/// <returns>Нормализованный угол в пределах от 0 до 360</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single NormalizationFull(Single angle)
			{
				Single degree_norm = angle;
				if (angle >= 360.0f || angle < 0.0f)
				{
					degree_norm -= (Single)Math.Floor(angle / 360.0f) * 360.0f;
				}
				return degree_norm;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Нормализация угла в пределах от -180 до 180
			/// </summary>
			/// <param name="angle">Угол, задается в градусах</param>
			/// <returns>Нормализованный угол в пределах от -180 до 180</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Double NormalizationHalf(Double angle)
			{
				Double degree_norm = angle;
				if (angle >= 360.0 || angle < 0.0)
				{
					degree_norm -= Math.Floor(angle / 360.0) * 360.0;
				}
				if (degree_norm > 180.0)
				{
					degree_norm -= 360.0;
				}
				return degree_norm;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Нормализация угла в пределах от -180 до 180
			/// </summary>
			/// <param name="angle">Угол, задается в градусах</param>
			/// <returns>Нормализованный угол в пределах от -180 до 180</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single NormalizationHalf(Single angle)
			{
				Single degree_norm = angle;
				if (angle >= 360.0f || angle < 0.0f)
				{
					degree_norm -= (Single)Math.Floor(angle / 360.0f) * 360.0f;
				}
				if (degree_norm > 180.0f)
				{
					degree_norm -= 360.0f;
				}
				return degree_norm;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Ограничение угла
			/// </summary>
			/// <param name="angle">Угол, задается в градусах</param>
			/// <param name="min">Минимальный угол</param>
			/// <param name="max">Максимальный угол</param>
			/// <returns>Нормализованный угол</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Double Clamp(Double angle, Double min, Double max)
			{
				if (angle < -360)
					angle += 360;
				if (angle > 360)
					angle -= 360;

				if (angle > max)
				{
					return max;
				}
				if (angle < min)
				{
					return min;
				}

				return angle;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Ограничение угла
			/// </summary>
			/// <param name="angle">Угол, задается в градусах</param>
			/// <param name="min">Минимальный угол</param>
			/// <param name="max">Максимальный угол</param>
			/// <returns>Нормализованный угол</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single Clamp(Single angle, Single min, Single max)
			{
				if (angle < -360)
					angle += 360;
				if (angle > 360)
					angle -= 360;

				if (angle > max)
				{
					return max;
				}
				if (angle < min)
				{
					return min;
				}

				return angle;
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ЕДИНИЦАМИ УГЛОВ ===========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование количества оборотов в градусы
			/// </summary>
			/// <param name="revolution">Количество оборотов</param>
			/// <returns>Количество градусов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Double RevolutionsToDegrees(Double revolution)
			{
				return revolution * 360.0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование количества оборотов в градусы
			/// </summary>
			/// <param name="revolution">Количество оборотов</param>
			/// <returns>Количество градусов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single RevolutionsToDegrees(Single revolution)
			{
				return revolution * 360.0f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование количества оборотов в радианы
			/// </summary>
			/// <param name="revolution">Количество оборотов</param>
			/// <returns>Количество радиан</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Double RevolutionsToRadians(Double revolution)
			{
				return revolution * XMath.PI2d;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование количества оборотов в радианы
			/// </summary>
			/// <param name="revolution">Количество оборотов</param>
			/// <returns>Количество радиан</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single RevolutionsToRadians(Single revolution)
			{
				return revolution * XMath.PI2f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование количества оборотов в грады
			/// </summary>
			/// <remarks>
			/// Град — сотая часть прямого угла
			/// </remarks>
			/// <param name="revolution">Количество оборотов</param>
			/// <returns>Количество град</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Double RevolutionsToGradians(Double revolution)
			{
				return revolution * 400.0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование количества оборотов в грады
			/// </summary>
			/// <remarks>
			/// Град — сотая часть прямого угла
			/// </remarks>
			/// <param name="revolution">Количество оборотов</param>
			/// <returns>Количество град</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single RevolutionsToGradians(Single revolution)
			{
				return revolution * 400.0f;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================