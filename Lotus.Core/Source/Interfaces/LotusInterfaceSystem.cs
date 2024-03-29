namespace Lotus.Core
{
    /**
     * \defgroup CoreInterfaces Подсистема интерфейсов
     * \ingroup Core
     * \brief Подсистема интерфейсов содержит базовые интерфейсы которые определяют различные аспекты работы с объектами.
     * @{
     */
    /// <summary>
    /// Определение интерфейса для объектов поддерживающих копирование параметров с других объектов.
    /// </summary>
    public interface ILotusCopyParameters
    {
        /// <summary>
        /// Копирование параметров с указанного объекта.
        /// </summary>
        /// <param name="sourceObject">Объект-источник с которого будут скопированы параметры.</param>
        /// <param name="parameters">Параметры контекста копирования параметров.</param>
        void CopyParameters(object sourceObject, CParameters? parameters);
    }

    /// <summary>
    /// Определение интерфейса для информирование о начале процесса загрузки.
    /// </summary>
    public interface ILotusBeforeLoad
    {
        /// <summary>
        /// Метод вызывается непосредственно перед загрузкой всех данных.
        /// </summary>
        /// <remarks>
        /// Реализация данного метода может предусматривать дополнительные действия, например по подготовки формата
        /// загрузки данных или очистку внутренних данных
        /// </remarks>
        /// <param name="parameters">Параметры контекста.</param>
        void OnBeforeLoad(CParameters? parameters);
    }

    /// <summary>
    /// Определение интерфейса для информирование о завершении процесса загрузки.
    /// </summary>
    public interface ILotusAfterLoad
    {
        /// <summary>
        /// Метод вызывается непосредственно после загрузки всех данных.
        /// </summary>
        /// <remarks>
        /// Реализация данного метода может предусматривать дополнительные действия, например по динамическому
        /// связыванию, которые можно/нужно осуществить после полной загрузки всех данных
        /// </remarks>
        /// <param name="parameters">Параметры контекста.</param>
        void OnAfterLoad(CParameters? parameters);
    }

    /// <summary>
    /// Определение интерфейса для информирование о начале процесса сохранения.
    /// </summary>
    public interface ILotusBeforeSave
    {
        /// <summary>
        /// Метод вызывается непосредственно перед сохранением всех данных.
        /// </summary>
        /// <param name="parameters">Параметры контекста.</param>
        void OnBeforeSave(CParameters? parameters);
    }

    /// <summary>
    /// Определение интерфейса для информирование о завершении процесса сохранения.
    /// </summary>
    public interface ILotusAfterSave
    {
        /// <summary>
        /// Метод вызывается непосредственно после сохранения всех данных.
        /// </summary>
        /// <param name="parameters">Параметры контекста.</param>
        void OnAfterSave(CParameters? parameters);
    }
    /**@}*/
}