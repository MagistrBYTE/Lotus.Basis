﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема атрибутов
// Подраздел: Атрибуты для управления и оформления числовых свойств/полей
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusAttributeNumberMin.cs
*		Атрибут для определения минимального значения величины.
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
        /// Атрибут для определения минимального значения величины
        /// </summary>
        /// <remarks>
        /// В зависимости от способа задания значение распространяется либо на весь тип, либо к каждому экземпляру
        /// </remarks>
        //-------------------------------------------------------------------------------------------------------------
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class LotusMinValueAttribute : Attribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly System.Object mMinValue;
			internal readonly String mMemberName;
			internal readonly TInspectorMemberType mMemberType;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Минимальное значение величины
			/// </summary>
			public System.Object MinValue
			{
				get { return mMinValue; }
			}

			/// <summary>
			/// Имя члена объекта содержащие минимальное значение
			/// </summary>
			public String MemberName
			{
				get { return mMemberName; }
			}

			/// <summary>
			/// Тип члена объекта
			/// </summary>
			public TInspectorMemberType MemberType
			{
				get { return mMemberType; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="minValue">Минимальное значение величины</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusMinValueAttribute(System.Object minValue)
			{
				mMinValue = minValue;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="memberName">Имя члена объекта содержащие минимальное значение</param>
			/// <param name="memberType">Тип члена объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusMinValueAttribute(String memberName, TInspectorMemberType memberType = TInspectorMemberType.Method)
			{
				mMemberName = memberName;
				mMemberType = memberType;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="type">Тип содержащие минимальное значение</param>
			/// <param name="memberName">Имя члена типа содержащие минимальное значение</param>
			/// <param name="memberType">Тип члена типа</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusMinValueAttribute(Type type, String memberName, TInspectorMemberType memberType = TInspectorMemberType.Method)
			{
				mMinValue = type;
				mMemberName = memberName;
				mMemberType = memberType;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================