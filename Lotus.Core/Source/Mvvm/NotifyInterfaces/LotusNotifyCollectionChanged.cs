using System;

namespace Lotus.Core.Inspector
{
    /** \addtogroup CoreNotifyInterfaces
	*@{*/
    /// <summary>
    /// Основные типы действий по изменению коллекций.
    /// </summary>
    public enum TNotifyCollectionChangedAction
    {
        /// <summary>
        /// Элемент/элементы был добавлен/вставлен в коллекцию.
        /// </summary>
        /// <remarks>
        /// Описание аргументов:
        /// - Объект - элемент который добавился, если список элементов по передается как IList
        /// - Индекс элемента - если добавляется то указывается последний индекс, иначе указывается индекс вставки
        /// - Количество элементов - либо 1 либо количество элементов в списке
        /// </remarks>
        Add,

        /// <summary>
        /// Элемент был перемещен в пределах коллекции.
        /// </summary>
        /// <remarks>
        /// Описание аргументов:
        /// - Объект - элемент который переместился, может быть null
        /// - Индекс элемента - указывается предыдущаю позиция элемента
        /// - Количество элементов - указывается новая позицяю элемента
        /// </remarks>
        Move,

        /// <summary>
        /// Элемент был удален из коллекции.
        /// </summary>
        /// <remarks>
        /// Описание аргументов:
        /// - Объект - элемент который удалили, удаляемый элемент может быть null
        /// - Индекс элемента - указывается индекс элемента которого удалили
        /// - Количество элементов - указывается количество удаляемых элементов
        /// </remarks>
        Remove,

        /// <summary>
        /// Элемент был заменен в коллекции.
        /// </summary>
        /// <remarks>
        /// Описание аргументов:
        /// - Объект - новый элемент
        /// - Индекс элемента - указывается индекс элемента которого заменили
        /// - Количество элементов - указывается 1
        /// </remarks>
        Replace,

        /// <summary>
        /// Свойства(данные) элемента в коллекции были изменены.
        /// </summary>
        /// <remarks>
        /// Применятся только для элементов которые имеют сложный тип
        /// Описание аргументов:
        /// - Объект - Имя свойства которое изменилось (тип String)
        /// - Индекс элемента - указывается индекс элемента свойства которого изменились
        /// - Количество элементов - указывается 1
        /// </remarks>
        ModifyItem,

        /// <summary>
        /// Содержимое коллекции было переустановлено.
        /// </summary>
        /// <remarks>
        /// Описание аргументов:
        /// - Объект - коллекция новых элементов передается как IList
        /// - Индекс элемента - не используется (указывается 0)
        /// - Количество элементов - указывается количество элементов
        /// </remarks>
        Reset,

        /// <summary>
        /// Содержимое коллекции было удалено.
        /// </summary>
        /// <remarks>
        /// Описание аргументов:
        /// - Объект - не используется (указывается null)
        /// - Индекс элемента - не используется (указывается 0)
        /// - Количество элементов - не используется (указывается 0)
        /// </remarks>
        Clear
    }

    /// <summary>
    /// Определение дополнительного интерфейса для нотификации изменения коллекции.
    /// </summary>
    /// <remarks>
    /// Реализация данного интерфейса любой коллекции определяет так называемую наблюдаемую коллекцию.
    /// Наблюдаемая коллекция - коллекция которая информирует о всех изменения которые происходят с ней
    /// </remarks>
    public interface ILotusNotifyCollectionChanged
    {
        /// <summary>
        /// Событие для нотификации о любом изменении коллекции.
        /// </summary>
        /// <remarks>
        /// Аргументы:
        /// - Тип происходящего действия
        /// - Объект - элемент над которым производится действия, либо коллекция
        /// - Индекс элемента
        /// - Количество элементов
        /// Интерпретацию передаваемых аргументов смотреть в комментариях к перечислению <see cref="TNotifyCollectionChangedAction"/>
        /// </remarks>
        Action<TNotifyCollectionChangedAction, object, int, int> OnCollectionChanged { get; set; }
    }
    /**@}*/
}