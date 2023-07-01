﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема поддержки инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusInspectorBase.cs
*		Определение базовой концепции атрибута характеристики/модификации отображаемого члена объекта инспектором свойств.
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
		/**
         * \defgroup CoreInspector Подсистема поддержки инспектора свойств
         * \ingroup Core
         * \brief Подсистема поддержки инспектора свойств обеспечивает расширенное описание и управление свойствами/полями объекта.
         * \details Инспектор свойств (или инспектор объектов) представляет собой элемент управления, который позволяет управлять
			объектом посредством изменения его свойств (и не только свойств).

			При этом этот элемент управления используется как в режиме разработки приложения, так и может использоваться
			в готовом приложении.
			
			Данная подсистема прежде всего направлена на расширение возможностей инспектора свойств Unity и инспектора свойств Lotus.
			Поддержка стандартного инспектора свойств IDE при разработке обычных приложений не предусмотрена.
         * @{
         */
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Тип члена объекта для атрибутов поддержки инспектора свойств
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TInspectorMemberType
		{
			/// <summary>
			/// Поле
			/// </summary>
			Field,

			/// <summary>
			/// Свойство
			/// </summary>
			Property,

			/// <summary>
			/// Метод
			/// </summary>
			Method
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================