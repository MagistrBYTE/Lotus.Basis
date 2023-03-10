//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема поддержки инспектора свойств
// Подраздел: Атрибуты для инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusInspectorHeaderGroup.cs
*		Атрибут декоративной отрисовки заголовка группы.
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
		//! \addtogroup CoreInspectorAttribute
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут декоративной отрисовки заголовка группы
		/// </summary>
		/// <remarks>
		/// Реализация декоративной атрибута отрисовки заголовка группы c возможностью задать выравнивания и цвет текста заголовка
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
#if UNITY_2017_1_OR_NEWER
		public sealed class LotusHeaderGroupAttribute : UnityEngine.PropertyAttribute
#else
		public sealed class LotusHeaderGroupAttribute : Attribute
#endif
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal String mName;
			internal TColor mTextColor;
			internal String mTextAlignment = "MiddleLeft";
			internal Int32 mIndent;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя заголовка
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Цвет текста заголовка
			/// </summary>
			public TColor TextColor
			{
				get { return mTextColor; }
				set { mTextColor = value; }
			}

			/// <summary>
			/// Выравнивание текста заголовка
			/// </summary>
			public String TextAlignment
			{
				get { return mTextAlignment; }
				set { mTextAlignment = value; }
			}

			/// <summary>
			/// Уровень смещения
			/// </summary>
			public Int32 Indent
			{
				get { return mIndent; }
				set { mIndent = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public LotusHeaderGroupAttribute()
			{
				mName = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusHeaderGroupAttribute(String name)
			{
				mName = name;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="indent">Уровень смещения</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusHeaderGroupAttribute(String name, Int32 indent)
			{
				mName = name;
				mIndent = indent;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="colorBGRA">Цвет текста заголовка</param>
			/// <param name="text_alignment">Выравнивание текста заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusHeaderGroupAttribute(String name, UInt32 colorBGRA, String text_alignment = "MiddleLeft")
			{
				mName = name;
				mTextColor = TColor.FromBGRA(colorBGRA);
				mTextAlignment = text_alignment;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="colorBGRA">Цвет текста заголовка</param>
			/// <param name="ord">Порядок отображения свойства</param>
			/// <param name="text_alignment">Выравнивание текста заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusHeaderGroupAttribute(String name, UInt32 colorBGRA, Int32 ord, String text_alignment = "MiddleLeft")
			{
				mName = name;
				mTextColor = TColor.FromBGRA(colorBGRA);
				mTextAlignment = text_alignment;
#if UNITY_2017_1_OR_NEWER
				order = ord;
#endif
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================