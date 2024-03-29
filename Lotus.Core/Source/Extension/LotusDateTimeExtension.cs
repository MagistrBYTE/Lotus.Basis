using System;

namespace Lotus.Core
{
    /** \addtogroup CoreExtension
	*@{*/
    /// <summary>
    /// Статический класс реализующий методы расширения для типа <see cref="System.DateTime"/>.
    /// </summary>
    public static class XDateTimeExtension
    {
        /// <summary>
        /// Получение позавчерашнего дня.
        /// </summary>
        /// <param name="this">Дата/время.</param>
        /// <returns>Позавчерашний день.</returns>
        public static DateTime TwoDaysAgo(this DateTime @this)
        {
            return @this - TimeSpan.FromDays(2);
        }

        /// <summary>
        /// Получение вчерашнего дня.
        /// </summary>
        /// <param name="this">Дата/время.</param>
        /// <returns>Вчерашний день.</returns>
        public static DateTime Yesterday(this DateTime @this)
        {
            return @this - TimeSpan.FromDays(1);
        }

        /// <summary>
        /// Получение завтрашнего дня.
        /// </summary>
        /// <param name="this">Дата/время.</param>
        /// <returns>Завтрашний день.</returns>
        public static DateTime Tomorrow(this DateTime @this)
        {
            return @this + TimeSpan.FromDays(1);
        }

        /// <summary>
        /// Получение послезавтрашнего дня.
        /// </summary>
        /// <param name="this">Дата/время.</param>
        /// <returns>Послезавтрашний день.</returns>
        public static DateTime? DayAfterTomorrow(this DateTime @this)
        {
            return @this + TimeSpan.FromDays(2);
        }

        /// <summary>
        /// Получение интерполированное значение даты.
        /// </summary>
        /// <param name="this">Дата/время.</param>
        /// <param name="endDate">Конечная дата/время.</param>
        /// <param name="time">Фактор интерполяции от 0 до 1.</param>
        /// <returns>Дата.</returns>
        public static DateTime GetInterpolatedDate(this DateTime @this, DateTime endDate, float time)
        {
            var total_second = (endDate - @this).TotalSeconds;
            var result = @this + TimeSpan.FromSeconds(total_second * time);

            return result;
        }

        /// <summary>
        /// Преобразование в текстовый формат даты UTC: yyyy-MM-dd.
        /// </summary>
        /// <param name="this">Дата/время.</param>
        /// <returns>Дата в текстовом формате UTC.</returns>
        public static string ToStrDateUTC(this DateTime @this)
        {
            return @this.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Преобразование в текстовый формат даты/времени: "{yy}y_{mm}m_{dd}d_{hh}h_{mm}m_{ss}".
        /// </summary>
        /// <param name="this">Дата/время.</param>
        /// <returns>Дата/время в текстовом формате.</returns>
        public static string ToStrDateTime(this DateTime @this)
        {
            var result = string.Format("{0}y_{1}m_{2}d_{3}h_{4}m_{5}", @this.Year, @this.Month.ToString("D2"),
            @this.Day.ToString("D2"), @this.Hour.ToString("D2"), @this.Minute.ToString("D2"), @this.Second.ToString("D2"));

            return result;
        }

        /// <summary>
        /// Преобразование в текстовый формат даты/времени: "yy_mm_dd@hh_mm_ss".
        /// </summary>
        /// <param name="this">Дата/время.</param>
        /// <returns>Дата/время в текстовом формате.</returns>
        public static string ToStrDateTimeShort(this DateTime @this)
        {
            var result = string.Format("{0}_{1}_{2}@{3}_{4}_{5}", (@this.Year - 2000).ToString("D2"),
            @this.Month.ToString("D2"), @this.Day.ToString("D2"), @this.Hour.ToString("D2"),
            @this.Minute.ToString("D2"), @this.Second.ToString("D2"));

            return result;
        }
    }
    /**@}*/
}