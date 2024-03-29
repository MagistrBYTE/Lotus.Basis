using System;

namespace Lotus.Core.Serialization
{
    /** \addtogroup CoreSerialization
	*@{*/
    /// <summary>
    /// Атрибут для указания того что тип самостоятельно предоставит данные для сериализации.
    /// </summary>
    /// <remarks>
    /// Тип помеченный данным атрибутом должен обязательно реализовать статический метод
    /// с именем <see cref="LotusSerializeDataAttribute.GET_SERIALIZE_DATA"/> который возвращает данные сериализации.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class LotusSerializeDataAttribute : Attribute
    {
        #region Const
        /// <summary>
        /// Имя статического метода типа который представляет данные для сериализации.
        /// </summary>
        public const string GET_SERIALIZE_DATA = "GetSerializeData";
        #endregion
    }
    /**@}*/
}