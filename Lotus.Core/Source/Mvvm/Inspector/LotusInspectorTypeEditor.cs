﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема поддержки инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusInspectorTypeEditor.cs
*		Атрибут для определения редактора для свойства.
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
		/** \addtogroup CoreInspector
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для определения редактора для свойства
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
		public class LotusInspectorTypeEditor : Attribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected internal Type mEditorType;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Тип редактора для свойства
			/// </summary>
			public Type EditorType
			{
				get { return mEditorType; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="editorType">Тип редактора для свойства</param>
			//---------------------------------------------------------------------------------------------------------
			public LotusInspectorTypeEditor(Type editorType)
			{
				mEditorType = editorType;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================