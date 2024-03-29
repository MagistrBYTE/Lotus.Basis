using System.Linq;

namespace Lotus.Repository
{
    /** \addtogroup RepositoryFilter
	*@{*/
    /// <summary>
    /// Статический класс реализующий методы расширений для работы с интерфейсом <see cref="IQueryable"/>.
    /// </summary>
    public static class XQueryableExtensionFilter
    {
        /// <summary>
        /// Фильтрация данных запроса по указанным параметрам.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности.</typeparam>
        /// <param name="query">Запрос.</param>
        /// <param name="properties">Параметры фильтрации свойств.</param>
        /// <returns>Запрос.</returns>
        public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> query,
            params FilterByProperty[]? properties)
        {
            if (properties == null || properties.Length == 0)
            {
                return query;
            }

            foreach (var property in properties)
            {
                if ((property.Function == TFilterFunction.IncludeAny
                    || property.Function == TFilterFunction.IncludeAll
                    || property.Function == TFilterFunction.IncludeEquals
                    || property.Function == TFilterFunction.IncludeNone)
                    && (property.Values == null || property.Values.Length == 0))
                {
                    continue;
                }

                var filter = XExpressionFilters.GetFilter<TEntity>(property);
                query = query.Where(filter);
            }

            return query;
        }
    }
    /**@}*/
}