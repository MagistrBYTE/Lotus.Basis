﻿//=====================================================================================================================
// Проект: Модуль единиц измерения
// Раздел: Единицы измерения
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUnitMeasurementDescriptor.cs
*		Определение дескриптора для описания единицы измерения.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//---------------------------------------------------------------------------------------------------------------------
#nullable disable
//=====================================================================================================================
namespace Lotus
{
	namespace UnitMeasurement
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup UnitMeasurement
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Дескриптор для описания единицы измерения
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CUnitDescriptor
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Коэффициент для преобразования в базовую единицу измерения
			/// </summary>
			public Double CoeffToBase { get; set; }

			/// <summary>
			/// Коэффициент для преобразования в текущую единицу измерения из базовой единицы измерения
			/// </summary>
			public Double CoeffToCurrent 
			{
				get { return 1 / CoeffToBase; }
			}

			/// <summary>
			/// Международное название
			/// </summary>
			public String InternationalName { get; set; }

			/// <summary>
			/// Русское название
			/// </summary>
			public String RusName { get; set; }

			/// <summary>
			/// Международная аббревиатура
			/// </summary>
			public String InternationalAbbv { get; set; }

			/// <summary>
			/// Русская аббревиатура
			/// </summary>
			public String RusAbbv { get; set; }
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными параметрами
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CUnitDescriptor()
			{
				CoeffToBase = 1.0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="сoeffToBase">Коэффициент для преобразования в базовую единицу измерения</param>
			//---------------------------------------------------------------------------------------------------------
			public CUnitDescriptor(Double сoeffToBase)
			{
				CoeffToBase = сoeffToBase;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="сoeffToBase">Коэффициент для преобразования в базовую единицу измерения</param>
			/// <param name="internationalAbbv">Международная аббревиатура</param>
			/// <param name="rusAbbv">Русская аббревиатура</param>
			//---------------------------------------------------------------------------------------------------------
			public CUnitDescriptor(Double сoeffToBase, String internationalAbbv, String rusAbbv)
			{
				CoeffToBase = сoeffToBase;
				InternationalAbbv = internationalAbbv;
				RusAbbv = rusAbbv;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Дескриптор для описания единицы измерения
		/// </summary>
		/// <typeparam name="TUnit">Единица измерения</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class CUnitDescriptor<TUnit> : CUnitDescriptor where TUnit : Enum
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Единица измерения
			/// </summary>
			public TUnit Unit { get; set; }
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными параметрами
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CUnitDescriptor()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="unit">Единица измерения</param>
			/// <param name="сoeffToBase">Коэффициент для преобразования в базовую единицу измерения</param>
			//---------------------------------------------------------------------------------------------------------
			public CUnitDescriptor(TUnit unit, Double сoeffToBase)
				: base(сoeffToBase)
			{
				Unit = unit;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="unit">Единица измерения</param>
			/// <param name="сoeffToBase">Коэффициент для преобразования в базовую единицу измерения</param>
			/// <param name="internationalAbbv">Международная аббревиатура</param>
			/// <param name="rusAbbv">Русская аббревиатура</param>
			//---------------------------------------------------------------------------------------------------------
			public CUnitDescriptor(TUnit unit, Double сoeffToBase, String internationalAbbv, String rusAbbv)
				: base(сoeffToBase, internationalAbbv, rusAbbv)
			{
				Unit = unit;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================