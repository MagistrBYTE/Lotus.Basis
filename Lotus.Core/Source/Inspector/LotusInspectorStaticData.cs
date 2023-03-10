//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема поддержки инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusInspectorStaticData.cs
*		Определение статических данных для инспектора свойств.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreInspector
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс содержащий типовые названия групп свойств для испектора свойств
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XInspectorGroupDesc
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			//
			// Общие данные
			//
			public const String ID = "Идентификация";
			public const String Params = "Основные параметры";
			public const String Data = "Данные";
			public const String Subject = "Предмет";
			public const String Executor = "Исполнители";
			public const String Table = "Параметры таблицы";
			public const String Formats = "Форматирование";
			public const String Calculations = "Расчеты";
			public const String Date = "Сроки";
			public const String LinkPlace = "Ссылки на расположение";

			//
			// Графика
			//
			public const String Graphics = "Графика";
			public const String Pattern = "Параметры образца";
			public const String Stroke = "Контур";
			public const String Fill = "Заливка";
			public const String Size = "Размеры и позиция";

			//
			// Дорога
			//	
			public const String RoadParams = "Характеристики дороги";

			//
			// Финансы
			//
			public const String Financing = "Финансирование";
			public const String FinancialPerformance = "Финансовые показатели";

			//
			// Инвестиции
			//
			public const String InvestmentCommission = "Комиссия";

			//
			// Государственное управление
			//
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================