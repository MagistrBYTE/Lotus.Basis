//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема общих типов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusComparisonOperator.cs
*		Определение основных операторов.
*		Реализация типов которые определяют основные виды операторов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreCommonTypes
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Оператор сравнения
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TComparisonOperator
		{
			/// <summary>
			/// Равно
			/// </summary>
			[LotusAbbreviation("=")]
			Equality,

			/// <summary>
			/// Не равно
			/// </summary>
			[LotusAbbreviation("!=")]
			Inequality,

			/// <summary>
			/// Меньше
			/// </summary>
			[LotusAbbreviation("<")]
			LessThan,

			/// <summary>
			/// Меньше или равно
			/// </summary>
			[LotusAbbreviation("<=")]
			LessThanOrEqual,

			/// <summary>
			/// Больше
			/// </summary>
			[LotusAbbreviation(">")]
			GreaterThan,

			/// <summary>
			/// Больше или равно
			/// </summary>
			[LotusAbbreviation(">=")]
			GreaterThanOrEqual
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений для перечисления <see cref="TComparisonOperator"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XComparisonOperatorExtension
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение текстового представления оператора сравнения
			/// </summary>
			/// <param name="comparison_operator">Оператор сравнения</param>
			/// <returns>Текстовое представление</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetOperatorOfString(this TComparisonOperator comparison_operator)
			{
				String result = "";
				switch (comparison_operator)
				{
					case TComparisonOperator.Equality:
						result = " = ";
						break;
					case TComparisonOperator.Inequality:
						result = " != ";
						break;
					case TComparisonOperator.LessThan:
						result = " < ";
						break;
					case TComparisonOperator.LessThanOrEqual:
						result = " <= ";
						break;
					case TComparisonOperator.GreaterThan:
						result = " > ";
						break;
					case TComparisonOperator.GreaterThanOrEqual:
						result = " >= ";
						break;
					default:
						break;
				}

				return (result);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================