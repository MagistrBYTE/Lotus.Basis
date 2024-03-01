using System;

using Lotus.Core;

namespace Lotus.Repository
{
    /**
     * \defgroup RepositoryBase Базовая подсистема
     * \ingroup Repository
     * \brief Базовая подсистема определяет основные сущности репозитория.
     * @{
     */
    /// <summary>
    /// Интерфейс для определение базовой сущности репозитория.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа(идентификатора).</typeparam>
    public interface ILotusRepositoryEntity<TKey> : ILotusIdentifierId<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Дата создания сущности.
        /// </summary>
        DateTime Created { get; set; }

        /// <summary>
        /// Дата последней модификации сущности.
        /// </summary>
        DateTime Modified { get; set; }
    }

    /// <summary>
    /// Определение базовой сущности репозитория.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа(идентификатора).</typeparam>
    public abstract class RepositoryEntityBase<TKey> : ILotusRepositoryEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Идентификатор сущности.
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// Дата создания сущности.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Дата последней модификации сущности.
        /// </summary>
        public DateTime Modified { get; set; }
    }

    /// <summary>
    /// Определение базовой сущности репозитория с поддержкой глобального уникального идентификатора.
    /// </summary>
    public abstract class RepositoryEntity : RepositoryEntityBase<Guid>
    {
    }
    /**@}*/
}