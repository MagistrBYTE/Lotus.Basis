//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема атрибутов
// Подраздел: Атрибуты связанные с возможностью непосредственно управлять значением поля/свойства объекта
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusAttributeValueDefault.cs
*		Атрибут для определения свойства/поля у которого есть значение по умолчанию.
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
		/// Атрибут для определения свойства/поля у которого есть значение по умолчанию
		/// </summary>
		/// <remarks>
		/// В зависимости от способа задания, значение распространяется либо на весь тип, либо к каждому экземпляру
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
#if UNITY_2017_1_OR_NEWER
		public sealed class LotusDefaultValueAttribute : UnityEngine.PropertyAttribute
#else
		public sealed class LotusDefaultValueAttribute : Attribute
#endif
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly System.Object mDefaultValue;
			internal readonly String mMemberName;
			internal readonly TInspectorMemberType mMemberType;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Значение по умолчанию
			/// </summary>
			public System.Object DefaultValue
			{
				get { return mDefaultValue; }
			}

			/// <summary>
			/// Имя члена объекта содержащие значение по умолчанию
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
			/// <param name="default_value">Значение по умолчанию</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusDefaultValueAttribute(System.Object default_value)
			{
				mDefaultValue = default_value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="member_name">Имя члена объекта содержащий значение по умолчанию</param>
			/// <param name="member_type">Тип члена объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusDefaultValueAttribute(String member_name, TInspectorMemberType member_type)
			{
				mMemberName = member_name;
				mMemberType = member_type;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="type">Тип представляющий шаг содержащий значение по умолчанию</param>
			/// <param name="member_name">Имя члена типа содержащий значение по умолчанию</param>
			/// <param name="member_type">Тип члена типа</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusDefaultValueAttribute(Type type, String member_name, TInspectorMemberType member_type)
			{
				mDefaultValue = type;
				mMemberName = member_name;
				mMemberType = member_type;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================