﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема поддержки инспектора свойств
// Подраздел: Атрибуты для инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusInspectorDecorationBoxing.cs
*		Атрибут для определения фонового бокса элемента инспектора свойств.
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
		/** \addtogroup CoreInspectorAttribute
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для определения фонового бокса элемента инспектора свойств
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
#if UNITY_2017_1_OR_NEWER
		public sealed class LotusBoxingAttribute : UnityEngine.PropertyAttribute
#else
		public sealed class LotusBoxingAttribute : Attribute
#endif
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly Int32 mOffsetLeft;
			internal readonly Int32 mOffsetTop;
			internal readonly Int32 mOffsetRight;
			internal readonly Int32 mOffsetBottom;
			internal String _backgroundStyleName;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Смещение слева
			/// </summary>
			public Int32 OffsetLeft
			{
				get { return mOffsetLeft; }
			}

			/// <summary>
			/// Смещение сверху
			/// </summary>
			public Int32 OffsetTop
			{
				get { return mOffsetTop; }
			}

			/// <summary>
			/// Смещение справа
			/// </summary>
			public Int32 OffsetRight
			{
				get { return mOffsetRight; }
			}

			/// <summary>
			/// Смещение снизу
			/// </summary>
			public Int32 OffsetBottom
			{
				get { return mOffsetBottom; }
			}

			/// <summary>
			/// Имя визуального стиля для фонового поля
			/// </summary>
			/// <remarks>
			/// При необходимости может использоваться и для элементов
			/// </remarks>
			public String BackgroundStyleName
			{
				get { return _backgroundStyleName; }
				set { _backgroundStyleName = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="offsetLeft">Красная компонента цвета</param>
			/// <param name="offsetTop">Зеленая компонента цвета</param>
			/// <param name="offsetRight">Синяя компонента цвета</param>
			/// <param name="offsetBottom">Альфа компонента цвета</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusBoxingAttribute(Int32 offsetLeft, Int32 offsetTop = 0, Int32 offsetRight = 0, 
				Int32 offsetBottom = 0)
			{
				mOffsetLeft = offsetLeft;
				mOffsetTop = offsetTop;
				mOffsetRight = offsetRight;
				mOffsetBottom = offsetBottom;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================