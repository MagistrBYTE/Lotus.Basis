using System;
using System.ComponentModel;

namespace Lotus.Core
{
    /**
     * \defgroup CoreDateTime Подсистема работы с датой и временем
     * \ingroup Core
     * \brief Подсистема работы с датой и временем обеспечивает дополнительные функциональные возможности для работы
        с временным интервалом и временным периодом. 
     * @{
     */
    /// <summary>
    /// Статический класс реализующий дополнительные методы для работы с типом <see cref="DateTime"/>.
    /// </summary>
    public static class XDateTime
    {
        /// <summary>
        /// Получение текущей даты в тестовом формате UTC.
        /// </summary>
        /// <returns>Дата в текстовом формате UTC.</returns>
        public static string? GetStrDateUTC()
        {
            return DateTime.Now.ToStrDateUTC();
        }

        /// <summary>
        /// Получение текущей даты/времени в тестовом формате.
        /// </summary>
        /// <returns>Дата/время в текстовом формате.</returns>
        public static string? GetStrDateTime()
        {
            return DateTime.Now.ToStrDateTime();
        }

        /// <summary>
        /// Получение текущей даты/времени в тестовом формате.
        /// </summary>
        /// <returns>Дата/время в текстовом формате.</returns>
        public static string? GetStrDateTimeShort()
        {
            return DateTime.Now.ToStrDateTimeShort();
        }

        /// <summary>
        /// Проверка на вхождение даты в указанный диапазон.
        /// </summary>
        /// <param name="check">Проверяемая дата.</param>
        /// <param name="beginRange">Начало диапазона.</param>
        /// <param name="endRange">Окончание диапазона.</param>
        /// <returns>Статус проверки.</returns>
#if UNITY_2017_1_OR_NEWER
		public static Boolean CheckDateInRange(DateTime check, DateTime beginRange, DateTime endRange)
		{
			return check >= beginRange && check <= endRange;
		}
#else
        public static bool CheckDateInRange(DateOnly check, DateOnly beginRange, DateOnly endRange)
        {
            return check >= beginRange && check <= endRange;
        }
#endif
        /// <summary>
        /// Проверка на вхождение/пересечение диапазонов дат.
        /// </summary>
        /// <param name="beginCheck">Начало проверяемого диапазона.</param>
        /// <param name="endCheck">Окончание проверяемого диапазона.</param>
        /// <param name="beginRange">Начало диапазона.</param>
        /// <param name="endRange">Окончание диапазона.</param>
        /// <returns>Статус проверки.</returns>
#if UNITY_2017_1_OR_NEWER
		public static Boolean CheckDateRangeInRange(DateTime beginCheck, DateTime? endCheck,
			DateTime beginRange, DateTime endRange)
#else
        public static bool? CheckDateRangeInRange(DateOnly beginCheck, DateOnly? endCheck,
            DateOnly beginRange, DateOnly endRange)
#endif
        {
            if (endCheck != null)
            {
                var endDate = endCheck.GetValueOrDefault();

                // Целиком в диапазоне
                var all = CheckDateInRange(beginCheck, beginRange, endRange) &&
                    CheckDateInRange(endDate, beginRange, endRange);

                // Пересечение начальной даты
                var begin = beginCheck <= beginRange && CheckDateInRange(endDate, beginRange, endRange);

                // Пересечение конечной даты
                var end = CheckDateInRange(beginCheck, beginRange, endRange) && endDate >= endRange;

                return all || begin || end;
            }
            else
            {
                return beginCheck <= endRange;
            }
        }
    }

    /// <summary>
    /// Временной интервал.
    /// </summary>
    /// <remarks>
    /// Временной интервал это некая абстракция для выполнения действие через фиксированные интервал времени.
    /// Он может характеризовать, как и реальные временные интервалы, так и некие абстрактные игровые интервалы 
    /// привязанные к настоящим временным интервалам 
    /// </remarks>
    [TypeConverter(typeof(EnumToStringConverter<TTimeInterval>))]
    public enum TTimeInterval
    {
        /// <summary>
        /// Минутный.
        /// </summary>
        [Description("Минутный")]
        Minutely,

        /// <summary>
        /// Часовой.
        /// </summary>
        [Description("Часовой")]
        Hourly,

        /// <summary>
        /// Дневной.
        /// </summary>
        [Description("Дневной")]
        Daily,

        /// <summary>
        /// Недельный.
        /// </summary>
        [Description("Недельный")]
        Weekly,

        /// <summary>
        /// Месячный.
        /// </summary>
        [Description("Месячный")]
        Monthly,

        /// <summary>
        /// Годовой.
        /// </summary>
        [Description("Годовой")]
        Yearly
    }

    /// <summary>
    /// Интерфейс для объектов которые имеют временной признак.
    /// </summary>
    public interface ILotusDateTimeable : ICloneable
    {
        /// <summary>
        /// Дата.
        /// </summary>
        DateTime Date { get; set; }
    }
    /**@}*/
}