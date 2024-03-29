using System;

namespace Lotus.Core
{
    /** \addtogroup CoreCollections
	*@{*/
    /// <summary>
    /// Двусторонняя очередь на основе массива.
    /// </summary>
    /// <remarks>
    /// Реализация двусторонней очереди на основе массива, с полной поддержкой функциональности <see cref="ListArray{TItem}"/>
    /// с учетом особенности реализации двусторонней очереди
    /// </remarks>
    /// <typeparam name="TItem">Тип элемента очереди.</typeparam>
    [Serializable]
    public class DequeArray<TItem> : ListArray<TItem>
    {
        #region Fields
        // Основные параметры
        protected internal int _startOffset;
        #endregion

        #region Properties
        //
        // ОСНОВНЫЕ ПАРАМЕТРЫ
        //
        /// <summary>
        /// Начальное смещение для формирование очереди.
        /// </summary>
        public int StartOffset
        {
            get { return _startOffset; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор инициализирует данные двусторонней очереди предустановленными данными.
        /// </summary>
        public DequeArray()
            : base(INIT_MAX_COUNT)
        {
            _startOffset = INIT_MAX_COUNT / 2;
        }

        /// <summary>
        /// Конструктор инициализирует данные двусторонней очереди указанными данными.
        /// </summary>
        /// <param name="maxCount">Максимальное количество элементов.</param>
        public DequeArray(int maxCount)
            : base(maxCount)
        {
            _startOffset = maxCount / 2;
        }
        #endregion

        #region Indexer
        /// <summary>
        /// Индексация элементов очереди.
        /// </summary>
        /// <param name="index">Индекс элемента.</param>
        /// <returns>Элемент очереди.</returns>
        new public TItem this[int index]
        {
            get { return _arrayOfItems[(_startOffset + index) % _maxCount]!; }
            set
            {
                _arrayOfItems[(_startOffset + index) % _maxCount] = value;
            }
        }
        #endregion

        #region Main methods
        /// <summary>
        /// Получение элемента очереди по индексу.
        /// </summary>
        /// <param name="index">Индекс элемента очереди.</param>
        /// <returns>Элемент очереди.</returns>
        public TItem GetElement(int index)
        {
            return _arrayOfItems[(_startOffset + index) % _maxCount]!;
        }

        /// <summary>
        /// Добавление элемента в начало очереди.
        /// </summary>
        /// <param name="item">Элемент.</param>
        public void AddFront(in TItem item)
        {
            // Если текущие количество элементов равно максимально возможному
            if (_count == _maxCount)
            {
                _maxCount *= 2;
                var items = new TItem[_maxCount];
                Array.Copy(_arrayOfItems, items, _count);
                _arrayOfItems = items;
            }

            // Нет возможности добавить в начало очереди
            if (_startOffset == 0)
            {
                _maxCount *= 2;
                var items = new TItem[_maxCount];
                _startOffset = _maxCount / 2;
                Array.Copy(_arrayOfItems, 0, items, _startOffset, _count);
                _arrayOfItems = items;
            }

            // Уменьшаем индекс начало очереди
            _startOffset--;
            _arrayOfItems[_startOffset] = item;
            _count++;
        }

        /// <summary>
        /// Добавление элемента в конец очереди.
        /// </summary>
        /// <param name="item">Элемент.</param>
        public void AddBack(in TItem item)
        {
            // Если текущие количество элементов равно максимально возможному
            if (_count == _maxCount)
            {
                _maxCount *= 2;
                var items = new TItem[_maxCount];
                Array.Copy(_arrayOfItems, items, _count);
                _arrayOfItems = items;
            }

            _arrayOfItems[_startOffset + _count] = item;
            _count++;
        }

        /// <summary>
        /// Взятие и удаление элемента из начала очереди.
        /// </summary>
        /// <returns>Элемент.</returns>
        public TItem? RemoveFront()
        {
            if (_count > 0)
            {
                var item = _arrayOfItems[_startOffset];
                _arrayOfItems[_startOffset] = default;
                _startOffset++;
                _count--;
                return item;
            }
            else
            {
#if UNITY_2017_1_OR_NEWER
				UnityEngine.Debug.LogError("Not element in deque!!!");
#else
                XLogger.LogError("Not element in deque!!!");
#endif
                return default;
            }
        }

        /// <summary>
        /// Взятие и удаление элемента из конца очереди.
        /// </summary>
        /// <returns>Элемент.</returns>
        public TItem? RemoveBack()
        {
            if (_count > 0)
            {
                _count--;
                var item = _arrayOfItems[_count];
                _arrayOfItems[_count] = default;

                return item;
            }
            else
            {
#if UNITY_2017_1_OR_NEWER
				UnityEngine.Debug.LogError("Not element in deque!!!");
#else
                XLogger.LogError("Not element in deque!!!");
#endif
                return default;
            }
        }

        /// <summary>
        /// Взятие элемента начала очереди (без его удаления).
        /// </summary>
        /// <returns>Элемент.</returns>
        public TItem? PeekFront()
        {
            if (_count > 0)
            {
                return _arrayOfItems[_startOffset];
            }
            else
            {
#if UNITY_2017_1_OR_NEWER
				UnityEngine.Debug.LogError("Not element in deque!!!");
#else
                XLogger.LogError("Not element in deque!!!");
#endif
                return default;
            }

        }

        /// <summary>
        /// Взятие элемента конца очереди (без его удаления).
        /// </summary>
        /// <returns>Элемент.</returns>
        public TItem? PeekBack()
        {
            if (_count > 0)
            {
                return _arrayOfItems[_count - 1];
            }
            else
            {
#if UNITY_2017_1_OR_NEWER
				UnityEngine.Debug.LogError("Not element in deque!!!");
#else
                XLogger.LogError("Not element in deque!!!");
#endif
                return default;
            }

        }

        /// <summary>
        /// Проверка на наличие элемента в очереди.
        /// </summary>
        /// <param name="item">Элемент.</param>
        /// <returns>Статус наличия.</returns>
        public new bool Contains(in TItem item)
        {
            var index = _startOffset;
            var count = _count;

            while (count-- > 0)
            {
                if (_arrayOfItems[index]!.Equals(item))
                {
                    return true;
                }
                index = (index + 1) % _maxCount;
            }

            return false;
        }

        /// <summary>
        /// Очистка очереди.
        /// </summary>
        public new void Clear()
        {
            Array.Clear(_arrayOfItems, _startOffset, _count);
            _startOffset = _maxCount / 2;
            _count = 0;
        }
        #endregion
    }
    /**@}*/
}