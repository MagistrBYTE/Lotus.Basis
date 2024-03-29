namespace Lotus.Core
{
    /** \addtogroup CoreInterfaces
	*@{*/
    /// <summary>
    /// Определение интерфейса для объектов поддерживающих адаптацию к указанному типу.
    /// </summary>
    public interface ILotusAdapter
    {
        /// <summary>
        /// Проверка возможности адаптации объекта к указанному типу.
        /// </summary>
        /// <typeparam name="TType">Тип к корому нужно адаптироваться.</typeparam>
        /// <param name="parameters">Параметры адаптации.</param>
        /// <returns>Статус адаптации.</returns>
        bool CheckAdaptedObject<TType>(CParameters? parameters = null);

        /// <summary>
        /// Получение объекта адаптированного к указанному типу.
        /// </summary>
        /// <typeparam name="TType">Тип к корому нужно адаптироваться.</typeparam>
        /// <param name="parameters">Параметры адаптации.</param>
        /// <returns>Объект.</returns>
        TType GetAdaptedObject<TType>(CParameters? parameters = null);
    }

    /// <summary>
    /// Определение интерфейса для объектов поддерживающих дублирование.
    /// </summary>
    /// <typeparam name="TType">Тип объекта.</typeparam>
    public interface ILotusDuplicate<out TType>
    {
        /// <summary>
        /// Получение дубликата объекта.
        /// </summary>
        /// <param name="parameters">Параметры дублирования объекта.</param>
        /// <returns>Дубликат объекта.</returns>
        TType Duplicate(CParameters? parameters = null);
    }

    /// <summary>
    /// Определение интерфейса для идентификации типов объектов которые поддерживают иерархию.
    /// </summary>
    public interface ILotusDiscriminator
    {
        /// <summary>
        /// Дискриминатор типа.
        /// </summary>
        /// <remarks>
        /// Должен быть уникальным в пределах иерархии типов и одинаковым для каждого экземпляра типа.
        /// </remarks>
        string Discriminator { get; set; }
    }
    /**@}*/
}