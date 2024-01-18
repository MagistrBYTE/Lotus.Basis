﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема коллекций
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusCollectionPriorityQueue.cs
*		Очередь с приоритетом основе массива.
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
        /** \addtogroup CoreCollections
		*@{*/
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Очередь с приоритетом
        /// </summary>
        /// <remarks>
        /// Реализация очереди с приоритетом на основе массива, с полной поддержкой функциональности <see cref="ListArray{TItem}"/>
        /// с учетом особенности очереди с приоритетом
        /// </remarks>
        /// <typeparam name="TItem">Тип элемента очереди</typeparam>
        //-------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class PriorityQueue<TItem> : ListArray<TItem>
		{
			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные очереди предустановленными данными
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public PriorityQueue()
				: base(INIT_MAX_COUNT)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные очереди указанными данными
			/// </summary>
			/// <param name="maxCount">Максимальное количество элементов</param>
			//---------------------------------------------------------------------------------------------------------
			public PriorityQueue(Int32 maxCount)
				: base(maxCount)
			{

			}
			#endregion

			#region ======================================= ИНДЕКСАТОР ================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индексация элементов очереди
			/// </summary>
			/// <param name="index">Индекс элемента</param>
			/// <returns>Элемент очереди</returns>
			//---------------------------------------------------------------------------------------------------------
			public new TItem? this[Int32 index]
			{
				get { return _arrayOfItems[index]; }
				set
				{
					_arrayOfItems[index] = value;
					Update(index);
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вставка элемента в очередь
			/// </summary>
			/// <param name="item">Элемент</param>
			/// <returns>Индекс в очереди где находится элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 Push(in TItem? item)
			{
				var count = Count;
				Int32 middle;
				Add(item);
				do
				{
					if (count == 0)
					{
						break;
					}
					middle = (count - 1) / 2;
					if (ComparerDefault.Compare(_arrayOfItems[count], _arrayOfItems[middle]) < 0)
					{
						Swap(count, middle);
						count = middle;
					}
					else
					{
						break;
					}
				}
				while (true);

				return count;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выбрать элемент с самым низким приоритетом и вытолкнуть(удалить) его
			/// </summary>
			/// <returns>Элемент с самым низким приоритетом</returns>
			//---------------------------------------------------------------------------------------------------------
			public TItem? Pop()
			{
				TItem? result = _arrayOfItems[0];
				Int32 p = 0, p1, p2, pn;
				_arrayOfItems[0] = _arrayOfItems[Count - 1];
				RemoveAt(Count - 1);
				do
				{
					pn = p;
					p1 = (2 * p) + 1;
					p2 = (2 * p) + 2;
					if (Count > p1 && ComparerDefault.Compare(_arrayOfItems[p], _arrayOfItems[p1]) > 0)
					{
						p = p1;
					}
					if (Count > p2 && ComparerDefault.Compare(_arrayOfItems[p], _arrayOfItems[p2]) > 0)
					{
						p = p2;
					}
					if (p == pn)
					{
						break;
					}
					Swap(p, pn);
				}
				while (true);

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выбрать элемент с самым низким приоритетом, но не выталкивать(удалять) его
			/// </summary>
			/// <returns>Элемент с самым низким приоритетом</returns>
			//---------------------------------------------------------------------------------------------------------
			public TItem? Peek()
			{
				if (Count > 0)
				{
					return _arrayOfItems[0];
				}
				return default;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновить позицию элемента в очереди по указанному индексу
			/// </summary>
			/// <remarks>
			/// Применяется в тех случаях когда изменился приоритет элемента
			/// </remarks>
			/// <param name="index">Индекс элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public void Update(Int32 index)
			{
				Int32 p = index, pn;
				Int32 p1, p2;
				do
				{
					if (p == 0)
					{
						break;
					}
					p2 = (p - 1) / 2;
					if (ComparerDefault.Compare(_arrayOfItems[p], _arrayOfItems[p2]) < 0)
					{
						Swap(p, p2);
						p = p2;
					}
					else
					{
						break;
					}
				}
				while (true);

				if (p < index)
				{
					return;
				}
				do
				{
					pn = p;
					p1 = (2 * p) + 1;
					p2 = (2 * p) + 2;
					if (Count > p1 && ComparerDefault.Compare(_arrayOfItems[p], _arrayOfItems[p1]) > 0)
					{
						p = p1;
					}
					if (Count > p2 && ComparerDefault.Compare(_arrayOfItems[p], _arrayOfItems[p2]) > 0)
					{
						p = p2;
					}
					if (p == pn)
					{
						break;
					}
					Swap(p, pn);
				}
				while (true);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================