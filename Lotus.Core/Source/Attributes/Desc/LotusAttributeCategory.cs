//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема атрибутов
// Подраздел: Атрибуты дополнительного описания поля/свойства объекта
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusAttributeCategory.cs
*		Атрибут для определения группы свойств/полей.
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
		//! \addtogroup CoreAttribute
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для определения группы свойств/полей
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class LotusCategoryAttribute : Attribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly String mName;
			internal readonly Int32 mOrder;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя группы
			/// </summary>
			public String Name
			{
				get { return mName; }
			}

			/// <summary>
			/// Порядок отображения группы свойств
			/// </summary>
			public Int32 Order
			{
				get { return mOrder; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя группы</param>
			/// <param name="order">Порядок отображения группы свойств</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusCategoryAttribute(String name, Int32 order)
			{
				mName = name;
				mOrder = order;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================