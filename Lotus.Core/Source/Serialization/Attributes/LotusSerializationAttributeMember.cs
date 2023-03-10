//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема сериализации
// Подраздел: Атрибуты подсистема сериализации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSerializationAttributeMember.cs
*		Атрибут для определения сериализации свойства/поля.
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
		//! \addtogroup CoreSerialization
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для определения сериализации свойства/поля
		/// </summary>
		/// <remarks>
		/// Реализация атрибута для определения возможности сериализации свойства/поля.
		/// Соответственно определяет возможность сериализации на уровне типа объекта.
		/// Также атрибут позволяет определить под каким именем будет записано поле или свойство объекта в поток данных.
		/// Так как имя образует имя элемента/атрибута в формате XML то он должен подчинятся правилам использования 
		/// допустимых символов
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
		public sealed class LotusSerializeMemberAttribute : Attribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal String mName;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя для сериализации члена типа
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public LotusSerializeMemberAttribute()
			{
				mName = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя для сериализации члена типа</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusSerializeMemberAttribute(String name)
			{
				mName = name;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================