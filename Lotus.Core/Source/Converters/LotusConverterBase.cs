using System;

namespace Lotus.Core
{
    /**
	 * \defgroup CoreConverters Подсистема конвертации данных
	 * \ingroup Core
	 * \brief Подсистема конвертации и преобразования данных обеспечивает единый механизм и точку входа для 
		преобразования объекта в нужный тип. 
	 * \details Преобразование происходит на основе детальной информации об объекте который надо преобразовать в нужный тип, 
		поддерживается конвертация в том числе из строки или путём преобразования из объектов смежного типа.
	 * @{
	 */
    /// <summary>
    /// Статический класс реализующий конвертацию в базовые типы.
    /// </summary>
    /// <remarks>
    /// Класс является прежде всего аккумулятором всех методов преобразования которые реализуют другие классы.
    /// </remarks>
    public static class XConverter
    {
        #region ToBoolean 
        /// <summary>
        /// Преобразование текста в логическое значение.
        /// </summary>
        /// <param name="text">Текстовое значение.</param>
        /// <returns>Значение.</returns>
        public static bool ToBoolean(string text)
        {
            return XBooleanHelper.Parse(text);
        }
        #endregion

        #region ToInt 
        /// <summary>
        /// Преобразование текста в целочисленное значение.
        /// </summary>
        /// <param name="text">Текстовое значение.</param>
        /// <param name="defaultValue">Значение по умолчанию если преобразовать не удалось.</param>
        /// <returns>Значение.</returns>
        public static int ToInt(string text, int defaultValue = 0)
        {
            return XNumberHelper.ParseInt(text, defaultValue);
        }
        #endregion

        #region ToSingle 
        /// <summary>
        /// Преобразование текста в вещественное значение одинарной точности.
        /// </summary>
        /// <param name="text">Текстовое значение.</param>
        /// <param name="defaultValue">Значение по умолчанию если преобразовать не удалось.</param>
        /// <returns>Значение.</returns>
        public static float ToSingle(string text, float defaultValue = 0)
        {
            return XNumberHelper.ParseSingle(text, defaultValue);
        }
        #endregion

        #region ToDouble 
        /// <summary>
        /// Преобразование текста в вещественное значение двойной точности.
        /// </summary>
        /// <param name="text">Текстовое значение.</param>
        /// <param name="defaultValue">Значение по умолчанию если преобразовать не удалось.</param>
        /// <returns>Значение.</returns>
        public static double ToDouble(string text, double defaultValue = 0)
        {
            return XNumberHelper.ParseDouble(text, defaultValue);
        }
        #endregion

        #region ToDecimal 
        /// <summary>
        /// Преобразование текста в десятичное число с плавающей запятой.
        /// </summary>
        /// <param name="text">Текстовое значение.</param>
        /// <param name="defaultValue">Значение по умолчанию если преобразовать не удалось.</param>
        /// <returns>Значение.</returns>
        public static decimal ParseDecimal(string text, decimal defaultValue = 0)
        {
            return XNumberHelper.ParseDecimal(text, defaultValue);
        }
        #endregion

        #region ToNumber 
        /// <summary>
        /// Преобразование вещественного значения двойной точности в числовой тип указанного типа.
        /// </summary>
        /// <param name="targetType">Целевой числовой тип.</param>
        /// <param name="value">Значение.</param>
        /// <returns>Числовое значение.</returns>
        public static object ToNumber(Type targetType, double value)
        {
            var type_name = targetType.Name;
            switch (type_name)
            {
                case nameof(Byte):
                    {
                        return Convert.ToByte(value);
                    }
                case nameof(SByte):
                    {
                        return Convert.ToSByte(value);
                    }
                case nameof(Char):
                    {
                        return Convert.ToChar(value);
                    }
                case nameof(Int16):
                    {
                        return Convert.ToInt16(value);
                    }
                case nameof(UInt16):
                    {
                        return Convert.ToUInt16(value);
                    }
                case nameof(Int32):
                    {
                        return Convert.ToInt32(value);
                    }
                case nameof(UInt32):
                    {
                        return Convert.ToUInt32(value);
                    }
                case nameof(Int64):
                    {
                        return Convert.ToInt64(value);
                    }
                case nameof(UInt64):
                    {
                        return Convert.ToUInt64(value);
                    }
                case nameof(Single):
                    {
                        return Convert.ToSingle(value);
                    }
                case nameof(Decimal):
                    {
                        return Convert.ToDecimal(value);
                    }
            }

            return value;
        }

        /// <summary>
        /// Преобразование десятичного числа с плавающей запятой в числовой тип указанного типа.
        /// </summary>
        /// <param name="targetType">Целевой числовой тип.</param>
        /// <param name="value">Значение.</param>
        /// <returns>Числовое значение.</returns>
        public static object ToNumber(Type targetType, decimal value)
        {
            var type_name = targetType.Name;
            switch (type_name)
            {
                case nameof(Byte):
                    {
                        return Convert.ToByte(value);
                    }
                case nameof(SByte):
                    {
                        return Convert.ToSByte(value);
                    }
                case nameof(Char):
                    {
                        return Convert.ToChar(value);
                    }
                case nameof(Int16):
                    {
                        return Convert.ToInt16(value);
                    }
                case nameof(UInt16):
                    {
                        return Convert.ToUInt16(value);
                    }
                case nameof(Int32):
                    {
                        return Convert.ToInt32(value);
                    }
                case nameof(UInt32):
                    {
                        return Convert.ToUInt32(value);
                    }
                case nameof(Int64):
                    {
                        return Convert.ToInt64(value);
                    }
                case nameof(UInt64):
                    {
                        return Convert.ToUInt64(value);
                    }
                case nameof(Single):
                    {
                        return Convert.ToSingle(value);
                    }
                case nameof(Double):
                    {
                        return Convert.ToDouble(value);
                    }
            }

            return value;
        }
        #endregion

        #region ToEnum 
        /// <summary>
        /// Преобразование в объект указанного типа перечисления строкового значения.
        /// </summary>
        /// <typeparam name="TEnum">Тип перечисления.</typeparam>
        /// <param name="text">Текстовое значение.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns>Объект перечисления.</returns>
        public static TEnum ToEnum<TEnum>(string text, TEnum? defaultValue = default(TEnum)) where TEnum : Enum
        {
            return XEnumHelper.ToEnum<TEnum>(text, defaultValue);
        }

        /// <summary>
        /// Преобразование в объект указанного типа перечисления целочисленного значения.
        /// </summary>
        /// <typeparam name="TEnum">Тип перечисления.</typeparam>
        /// <param name="value">Значение.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns>Объект перечисления.</returns>
        public static TEnum ToEnum<TEnum>(int value, TEnum? defaultValue = default(TEnum)) where TEnum : Enum
        {
            return XEnumHelper.ToEnum<TEnum>(value, defaultValue);
        }

        /// <summary>
        /// Преобразование в объект указанного типа перечисления обобщенного значения.
        /// </summary>
        /// <typeparam name="TEnum">Тип перечисления.</typeparam>
        /// <param name="value">Значение.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns>Объект перечисления.</returns>
        public static TEnum ToEnum<TEnum>(object value, TEnum? defaultValue) where TEnum : Enum
        {
            return ToEnum<TEnum>(Convert.ToString(value)!, defaultValue);
        }

        /// <summary>
        /// Преобразование в объект перечисления обобщенного значения.
        /// </summary>
        /// <param name="enumType">Тип перечисления.</param>
        /// <param name="value">Значение.</param>
        /// <returns>Объект перечисления.</returns>
        public static Enum ToEnumOfType(Type enumType, object value)
        {
            return XEnumHelper.ToEnumOfType(enumType, value)!;
        }

        /// <summary>
        /// Попытка преобразования в объект указанного типа перечисления обобщенного значения.
        /// </summary>
        /// <typeparam name="TEnum">Тип перечисления.</typeparam>
        /// <param name="value">Значение.</param>
        /// <param name="result">Объект перечисления.</param>
        /// <returns>Статус успешности преобразования.</returns>
        public static bool TryToEnum<TEnum>(object value, out TEnum result) where TEnum : Enum
        {
            return XEnumHelper.TryToEnum<TEnum>(value, out result);
        }
        #endregion

        #region ToPrimitive 
        /// <summary>
        /// Преобразование объекта к примитивному типу по указанному коду типа.
        /// </summary>
        /// <remarks>
        /// К примитивными данным относятся все числовые типы, строковой тип, логический тип и перечисление.
        /// </remarks>
        /// <param name="typeCode">Код типа.</param>
        /// <param name="value">Значение.</param>
        /// <returns>Значение примитивного типа.</returns>
        public static TPrimitive? ToPrimitive<TPrimitive>(TypeCode typeCode, object value)
        {
            return default;
        }

        /// <summary>
        /// Преобразование текста к примитивному типу по указанному коду типа.
        /// </summary>
        /// <remarks>
        /// К примитивными данным относятся все числовые типы, строковой тип, логический тип и перечисление.
        /// </remarks>
        /// <param name="typeCode">Код типа.</param>
        /// <param name="text">Текстовое значение.</param>
        /// <returns>Значение примитивного типа.</returns>
        public static TPrimitive? ToPrimitive<TPrimitive>(TypeCode typeCode, string text)
        {
            object? result = null;

            switch (typeCode)
            {
                case TypeCode.Empty:
                    {
                    }
                    break;
                case TypeCode.Object:
                    {
                    }
                    break;
                case TypeCode.DBNull:
                    {
                    }
                    break;
                case TypeCode.Boolean:
                    {
                        result = XBooleanHelper.Parse(text);
                    }
                    break;
                case TypeCode.Char:
                    {
                        result = text[0];
                    }
                    break;
                case TypeCode.SByte:
                    {
                        result = (sbyte)XNumberHelper.ParseInt(text);
                    }
                    break;
                case TypeCode.Byte:
                    {
                        result = (byte)XNumberHelper.ParseInt(text);
                    }
                    break;
                case TypeCode.Int16:
                    {
                        result = (short)XNumberHelper.ParseInt(text);
                    }
                    break;
                case TypeCode.UInt16:
                    {
                        result = (ushort)XNumberHelper.ParseInt(text);
                    }
                    break;
                case TypeCode.Int32:
                    {
                        result = XNumberHelper.ParseInt(text);
                    }
                    break;
                case TypeCode.UInt32:
                    {
                        result = (uint)XNumberHelper.ParseInt(text);
                    }
                    break;
                case TypeCode.Int64:
                    {
                        result = XNumberHelper.ParseLong(text);
                    }
                    break;
                case TypeCode.UInt64:
                    {
                        result = (ulong)XNumberHelper.ParseLong(text);
                    }
                    break;
                case TypeCode.Single:
                    {
                        result = XNumberHelper.ParseSingle(text);
                    }
                    break;
                case TypeCode.Double:
                    {
                        result = XNumberHelper.ParseDouble(text);
                    }
                    break;
                case TypeCode.Decimal:
                    {
                        result = XNumberHelper.ParseDecimal(text);
                    }
                    break;
                case TypeCode.DateTime:
                    {
                        result = XDateTimeHelper.Parse(text);
                    }
                    break;
                case TypeCode.String:
                    {
                        result = text;
                    }
                    break;
                default:
                    break;
            }

            if (result == null)
            {
                return default;
            }

            return (TPrimitive)result;
        }
        #endregion
    }
    /**@}*/
}