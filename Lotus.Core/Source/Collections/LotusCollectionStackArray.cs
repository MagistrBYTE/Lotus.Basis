//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема коллекций
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusCollectionStackArray.cs
*		Стек на основе массива.
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
		//! \addtogroup CoreCollections
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Стек на основе массива
		/// </summary>
		/// <remarks>
		/// Реализация стека на основе массива, с полной поддержкой функциональности <see cref="ListArray{TItem}"/> 
		/// с учетом особенности реализации стека
		/// </remarks>
		/// <typeparam name="TItem">Тип элемента стека</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class StackArray<TItem> : ListArray<TItem>
		{
			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные стека предустановленными данными
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public StackArray()
				: base(INIT_MAX_COUNT)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные стека указанными данными
			/// </summary>
			/// <param name="max_count">Максимальное количество элементов</param>
			//---------------------------------------------------------------------------------------------------------
			public StackArray(Int32 max_count)
				: base(max_count)
			{
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вставка элемента в вершину стека
			/// </summary>
			/// <param name="item">Элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public void Push(in TItem item)
			{
				Add(item);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вытолкнуть(удалить) элемент из вершины стека
			/// </summary>
			/// <returns>Элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public TItem Pop()
			{
				if (mCount > 0)
				{
					TItem item = mArrayOfItems[mCount - 1];
					mCount--;
					mArrayOfItems[mCount] = default(TItem);

					return item;
				}
				else
				{
#if (UNITY_2017_1_OR_NEWER)
					UnityEngine.Debug.LogError("Not element in stack!!!");
#else
					XLogger.LogError("Not element in stack!!!");
#endif
					return default(TItem);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Взять элемент из вершины стека, но не выталкивать его(не удалять)
			/// </summary>
			/// <returns>Элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public TItem Peek()
			{
				if (mCount > 0)
				{
					return mArrayOfItems[mCount - 1];
				}
				else
				{
#if (UNITY_2017_1_OR_NEWER)
					UnityEngine.Debug.LogError("Not element in stack!!!");
#else
					XLogger.LogError("Not element in stack!!!");
#endif
					return default(TItem);
				}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================