﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема поддержки инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusInspectorSupport.cs
*		Определение интерфейсов для расширенной поддержки просмотра и редактирования объекта через инспектор свойств.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Reflection;
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup CoreInspector
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для определения просмотра общей информации объекта в инспекторе свойств
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusSupportViewInspector
		{
			/// <summary>
			/// Отображаемое имя типа в инспекторе свойств
			/// </summary>
			String InspectorTypeName { get; }

			/// <summary>
			/// Отображаемое имя объекта в инспекторе свойств
			/// </summary>
			String InspectorObjectName { get; }
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для определения расширенной поддержки редактирования объекта в инспекторе свойств
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusSupportEditInspector : ILotusSupportViewInspector
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получить массив описателей свойств объекта
			/// </summary>
			/// <returns>Массив описателей</returns>
			//---------------------------------------------------------------------------------------------------------
			CPropertyDesc[] GetPropertiesDesc();
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================