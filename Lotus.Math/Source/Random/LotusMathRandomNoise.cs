﻿//=====================================================================================================================
// Проект: Модуль математической системы
// Раздел: Подсистема генерации псевдослучайных значений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusMathRandomNoise.cs
*		Набор различных методов для генерации шума.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
using System.Collections.Generic;
//=====================================================================================================================
namespace Lotus
{
	namespace Maths
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup MathRandom
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий различных методы генерации шума
		/// </summary>
		/// <remarks>
		/// Реализованы различные методы генерации шума взятые из различных источников
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XGenerationNoise
		{
			#region ====================================== ДАННЫЕ =====================================================
			/// <summary>
			/// Случайное число источника
			/// </summary>
			public static Int32 Seed = 16;

			/// <summary>
			/// Целочисленная шумовая функция в одномерном пространстве
			/// </summary>
			public static Func<Int32, Int32> NoiseInteger1D = NoiseInteger1DV1;

			/// <summary>
			/// Целочисленная шумовая функция в двухмерном пространстве
			/// </summary>
			public static Func<Int32, Int32, Int32> NoiseInteger2D = NoiseInteger2DV1;

			/// <summary>
			/// Целочисленная шумовая функция в трехмерном пространстве
			/// </summary>
			public static Func<Int32, Int32, Int32, Int32>? NoiseInteger3D;

			/// <summary>
			/// Вещественная шумовая функция в одномерном пространстве
			/// </summary>
			public static Func<Single, Single>? NoiseSingle1D;

			/// <summary>
			/// Вещественная шумовая функция в двухмерном пространстве
			/// </summary>
			public static Func<Single, Single, Single> NoiseSingle2D = NoiseSingle2DV1;

			/// <summary>
			/// Вещественная шумовая функция в трехмерном пространстве
			/// </summary>
			public static Func<Single, Single, Single, Single>? NoiseSingle3D;
			#endregion

			#region ====================================== ОБЩИЕ МЕТОДЫ ===============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление значение сглаженного шума по указанным координатам
			/// </summary>
			/// <param name="x">Координата X</param>
			/// <param name="y">Координата Y</param>
			/// <returns>Сглаженное значение шума</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single SmoothNoiseSingle2D(Single x, Single y)
			{
				var corners = (NoiseSingle2D(x - 1, y - 1) +
					NoiseSingle2D(x + 1, y - 1) +
					NoiseSingle2D(x - 1, y + 1) +
					NoiseSingle2D(x + 1, y + 1)) / 16;
				var sides = (NoiseSingle2D(x - 1, y) +
					NoiseSingle2D(x + 1, y) +
					NoiseSingle2D(x, y - 1) +
					NoiseSingle2D(x, y + 1)) / 8;
				var center = NoiseSingle2D(x, y) / 4;

				return corners + sides + center;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление значение интерполированного шума по указанным координатам
			/// </summary>
			/// <param name="x">Координата X</param>
			/// <param name="y">Координата Y</param>
			/// <returns>Интерполированное значение шума</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single InterpolatedNoiseSingle2D(Single x, Single y)
			{
				// Вычисляем целую и дробную часть по X
				var integer_x = (Int32)x;
				var fractional_x = x - integer_x;

				// Вычисляем целую и дробную часть по Y
				var integer_y = (Int32)y;
				var fractional_y = y - integer_y;

				var integer_x1 = integer_x + 1;
				var integer_y1 = integer_y + 1;

				// Получаем 4 сглаженных значения
				var v1 = SmoothNoiseSingle2D(integer_x, integer_y);
				var v2 = SmoothNoiseSingle2D(integer_x1, integer_y);
				var v3 = SmoothNoiseSingle2D(integer_x, integer_y1);
				var v4 = SmoothNoiseSingle2D(integer_x1, integer_y1);

				// Интерполируем значения 1 и 2 пары и производим интерполяцию между ними
				var i1 = XMathInterpolation.Lerp(v1, v2, fractional_x);
				var i2 = XMathInterpolation.Lerp(v3, v4, fractional_x);

				return XMathInterpolation.Lerp(i1, i2, fractional_y);
			}
			#endregion

			#region ====================================== 1D =========================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Целочисленная шумовая функция в одномерном пространстве
			/// </summary>
			/// <remarks>
			/// Получает равномерную случайную величину, постоянную для конкретных параметров
			/// <see href="http://www.gamedev.ru/code/forum/?id=215866"/>
			/// </remarks>
			/// <param name="value">Значение</param>
			/// <returns>Случайная зависимая величина</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 NoiseInteger1DV1(Int32 value)
			{
				var m = value;
				m = (m >> 13) ^ m;
				var nn = ((m * ((m * m * 60493) + 19990303)) + 1376312589) & 0x7fffffff;
				return nn;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Целочисленная шумовая функция в одномерном пространстве
			/// </summary>
			/// <remarks>
			/// Получает равномерную случайную величину, постоянную для конкретных параметров
			/// <see href="http://www.gamedev.ru/code/forum/?id=215866"/>
			/// </remarks>
			/// <param name="value">Значение</param>
			/// <returns>Случайная зависимая величина</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 NoiseInteger1DV2(Int32 value)
			{
				var state = (UInt64)value;
				state = state * state;
				state = (state * 6364136223846793005UL) + 1442695040888963407UL;
				var xorshifted = (Int64)(((state >> 18) ^ state) >> 27);
				var rot = (Int32)(state >> 59);
				var v1 = xorshifted >> rot;
				var v2 = xorshifted << (-rot & 31);
				return (Int32)(v1 | v2);
			}
			#endregion

			#region ====================================== 2D =========================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Целочисленная шумовая функция в двухмерном пространстве
			/// </summary>
			/// <remarks>
			/// Получает равномерную случайную величину, постоянную для конкретных параметров
			/// </remarks>
			/// <param name="x">Координата X</param>
			/// <param name="y">Координата Y</param>
			/// <returns>Случайная зависимая величина</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 NoiseInteger2DV1(Int32 x, Int32 y)
			{
				var m_w = 43;//x * 7 + y * 17 + x * y + 1; //x * 43 + 1;    /* must not be zero, nor 0x464fffff */
				var m_z = ((x * y * 57) + y) ^ (2 + (x * 7) + 1);    /* must not be zero, nor 0x9068ffff */

				m_z = (36969 * (m_z & 65535)) + (m_z >> 16);
				m_w = (18000 * (m_w & 65535)) + (m_w >> 16);
				return (m_z << 16) + m_w;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Целочисленная шумовая функция в двухмерном пространстве
			/// </summary>
			/// <remarks>
			/// Получает равномерную случайную величину, постоянную для конкретных параметров
			/// </remarks>
			/// <param name="x">Координата X</param>
			/// <param name="y">Координата Y</param>
			/// <returns>Случайная зависимая величина</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 NoiseInteger2DV2(Int32 x, Int32 y)
			{
				const Int32 generator_noise_x = 1619;
				const Int32 generator_noise_y = 31337;
				var n = ((generator_noise_x * x) + (generator_noise_y * y) + Seed) & 0x7fffffff;
				n = (n >> 13) ^ n;
				return ((n * ((n * n * 60493) + 19990303)) + 1376312589) & 0x7fffffff;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вещественная шумовая функция в двухмерном пространстве
			/// </summary>
			/// <param name="x">Координата X</param>
			/// <param name="y">Координата Y</param>
			/// <returns>Случайная зависимая величина</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single NoiseSingle2DV1(Single x, Single y)
			{
				var u = (UInt32)NoiseInteger2DV1((Int32)x, (Int32)y);
				return (u + 1.0f) * 2.328306435454494e-10f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вещественная шумовая функция в двухмерном пространстве
			/// </summary>
			/// <param name="x">Координата X</param>
			/// <param name="y">Координата Y</param>
			/// <returns>Случайная зависимая величина</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single NoiseSingle2DV2(Int32 x, Int32 y)
			{
				return 1.0f - (NoiseInteger2DV2(x, y) / 1073741824.0f);
			}
			#endregion

		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================