//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема поддержки инспектора свойств
// Подраздел: Атрибуты для инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusInspectorCollectionReorderable.cs
*		Атрибут для расширенного отображения и управления элементами стандартных коллекций и коллекциями Lotus.
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
		/// Атрибут для расширенного отображения и управления элементами стандартных коллекций и коллекциями Lotus
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class LotusReorderableAttribute : Attribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal String mTitleFieldName;

			// Методы изменения коллекции
			internal String mItemsChangedMethodName;
			internal String mContextMenuMethodName;

			// Методы переупорядочивания коллекции
			internal String mSortAscendingMethodName;
			internal String mSortDescendingMethodName;
			internal String mReorderItemChangedMethodName;

			// Методы для рисования элементов коллекции
			internal String mDrawItemMethodName;
			internal String mHeightItemMethodName;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя поля дочернего свойства выступающее в качестве заголовка для сложных свойств
			/// </summary>
			public String TitleFieldName
			{
				get { return mTitleFieldName; }
				set { mTitleFieldName = value; }
			}

			//
			// МЕТОДЫ ИЗМЕНЕНИЯ КОЛЛЕКЦИИ
			//
			/// <summary>
			/// Имя метода вызываемого после изменения количества элементов списка (Добавления / удаления элементов)
			/// </summary>
			/// <remarks>
			/// Метод не должен принимать аргументов.
			/// Пустое значение означает что метод не используется
			/// </remarks>
			public String ItemsChangedMethodName
			{
				get { return mItemsChangedMethodName; }
				set { mItemsChangedMethodName = value; }
			}

			/// <summary>
			/// Имя метода вызываемого для формирования контекстного меню
			/// </summary>
			/// <remarks>
			/// Метод должен возвращать список элементов соответствующего типа.
			/// Пустое значение означает что метод не используется
			/// </remarks>
			public String ContextMenuMethodName
			{
				get { return mContextMenuMethodName; }
				set { mContextMenuMethodName = value; }
			}

			//
			// МЕТОДЫ ПЕРЕУПОРЯДОЧИВАНИЯ КОЛЛЕКЦИИ
			//
			/// <summary>
			/// Имя метода для сортировки элементов коллекции по возрастанию
			/// </summary>
			/// <remarks>
			/// Метод не должен принимать аргументов.
			/// Пустое значение означает что сортировка не используется
			/// </remarks>
			public String SortAscendingMethodName
			{
				get { return mSortAscendingMethodName; }
				set { mSortAscendingMethodName = value; }
			}

			/// <summary>
			/// Имя метода для сортировки элементов коллекции по убыванию
			/// </summary>
			/// <remarks>
			/// Метод не должен принимать аргументов.
			/// Пустое значение означает что сортировка не используется
			/// </remarks>
			public String SortDescendingMethodName
			{
				get { return mSortDescendingMethodName; }
				set { mSortDescendingMethodName = value; }
			}

			/// <summary>
			/// Имя метода вызываемого после изменения порядка элементов (Перемещение элемента, сортировка)
			/// </summary>
			/// <remarks>
			/// Метод должен принимать два аргумента:
			/// 1. Целый тип - индекс предыдущей позиции
			/// 2. Целый тип - индекс новой позиции
			/// Пустое значение означает что метод не используется
			/// </remarks>
			public String ReorderItemChangedMethodName
			{
				get { return mReorderItemChangedMethodName; }
				set { mReorderItemChangedMethodName = value; }
			}

			//
			// МЕТОДЫ ДЛЯ РИСОВАНИЯ ЭЛЕМЕНТОВ КОЛЛЕКЦИИ
			//
			/// <summary>
			/// Имя метода для рисование элемента коллекции
			/// </summary>
			/// <remarks>
			/// Метод должен принимать два аргумента:
			/// 1. Тип прямоугольник - область отображения элемента
			/// 2. Целый тип - индекс текущего элемента
			/// </remarks>
			public String DrawItemMethodName
			{
				get { return mDrawItemMethodName; }
				set { mDrawItemMethodName = value; }
			}

			/// <summary>
			/// Имя метода для вычисление высоты элемента
			/// </summary>
			/// <remarks>
			/// Метод должен принимать один аргумент целого типа обозначающий индекс элемента и возвращать высоту элемента
			/// Пустое значение обозначение что будет принята высота одного контрола
			/// </remarks>
			public String HeightItemMethodName
			{
				get { return mHeightItemMethodName; }
				set { mHeightItemMethodName = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public LotusReorderableAttribute()
			{
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================