﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема атрибутов
// Подраздел: Атрибуты для управления и оформления числовых свойств/полей
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusAttributeNumberFormat.cs
*		Атрибут для определения форматирование значения числовой величины.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
        //-------------------------------------------------------------------------------------------------------------
        /** \addtogroup CoreAttribute
		*@{*/
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Атрибут для определения форматирование значения числовой величины
        /// </summary>
        /// <remarks>
        /// Применяется стандартное форматирование строки, значение передается в качестве первого аргумента
        /// </remarks>
        //-------------------------------------------------------------------------------------------------------------
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class LotusNumberFormatAttribute : Attribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly String mFormatValue;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Формат отображения значения числовой величины
			/// </summary>
			public String FormatValue
			{
				get { return mFormatValue; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="formatValue">Формат отображения значения числовой величины</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusNumberFormatAttribute(String formatValue)
			{
				mFormatValue = formatValue;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================