namespace Lotus.Core
{
    /**
     * \defgroup CoreResultsSystem Подсистема результата операции
     * \ingroup Core
     * \brief Подсистема результата операции реализует общую схему ответа/результата выполнения различных 
		операций(вызов метода, дополнительные данные в ответе и т.д) от различных подсистем и определяет 
		успешность/не успешность ответа, а также вспомогательную информацию.
     * @{
     */
    /// <summary>
    /// Определение интерфейса для представления ответа/результата операции.
    /// </summary>
    /// <remarks>
    /// Помимо основного назначения - характеристики успешности или неуспешности выполнения, 
    /// может также нести дополнительную информацию.
    /// </remarks>
    public interface ILotusResult
    {
        /// <summary>
        /// Статус успешности выполнения операции.
        /// </summary>
        bool Succeeded { get; }

        /// <summary>
        /// Код ответа.
        /// </summary>
        /// <remarks>
        /// В зависимости от подсистемы коды могут по-разному интерпретироваться.
        /// </remarks>
        int Code { get; }

        /// <summary>
        /// Сообщение о результате выполнения операции.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Значение результата выполнения операции.
        /// </summary>
        object? Value { get; set; }
    }

    /// <summary>
    /// Определение интерфейса для представления ответа/результата операции 
    /// с типизированным значением результата выполнения операции.
    /// </summary>
    /// <typeparam name="TValue">Тип значения результата операции.</typeparam>
    public interface ILotusResult<TValue> : ILotusResult
    {
        /// <summary>
        /// Значение результата выполнения операции.
        /// </summary>
        new TValue? Value { get; set; }
    }
    /**@}*/
}