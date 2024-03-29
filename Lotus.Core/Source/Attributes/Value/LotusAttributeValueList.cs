using System;

namespace Lotus.Core
{
    /** \addtogroup CoreAttribute
	*@{*/
    /// <summary>
    /// Атрибут для определения набора значений величины.
    /// </summary>
    /// <remarks>
    /// В зависимости от способа задания, значение распространяется либо на весь тип, либо к каждому экземпляру.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
#if UNITY_2017_1_OR_NEWER
	public sealed class LotusListValuesAttribute : UnityEngine.PropertyAttribute
#else
    public sealed class LotusListValuesAttribute : Attribute
#endif
    {
        #region Fields
        internal readonly object _listValues;
        internal readonly string _memberName;
        internal readonly TInspectorMemberType _memberType;
        internal string _formatMethodName = "";
        #endregion

        #region Properties
        /// <summary>
        /// Набор значений величины.
        /// </summary>
        public object ListValues
        {
            get { return _listValues; }
        }

        /// <summary>
        /// Имя члена объекта содержащий набор значений величины.
        /// </summary>
        public string MemberName
        {
            get { return _memberName; }
        }

        /// <summary>
        /// Тип члена объекта.
        /// </summary>
        public TInspectorMemberType MemberType
        {
            get { return _memberType; }
        }

        /// <summary>
        /// Имя метода который осуществляет преобразование объекта в текстовое представление.
        /// </summary>
        /// <remarks>
        /// Метод должен иметь один аргумент типа <see cref="object"/> и возвращать значение строкового типа.
        /// </remarks>
        public string FormatMethodName
        {
            get { return _formatMethodName; }
            set { _formatMethodName = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор инициализирует объект класса указанными параметрами.
        /// </summary>
        /// <param name="listValues">Набор значений величины.</param>
        public LotusListValuesAttribute(params object[] listValues)
        {
            _listValues = listValues;
        }

        /// <summary>
        /// Конструктор инициализирует объект класса указанными параметрами.
        /// </summary>
        /// <param name="memberName">Имя члена объекта содержащий набор значений величины.</param>
        /// <param name="memberType">Тип члена объекта.</param>
        public LotusListValuesAttribute(string memberName, TInspectorMemberType memberType)
        {
            _memberName = memberName;
            _memberType = memberType;
        }

        /// <summary>
        /// Конструктор инициализирует объект класса указанными параметрами.
        /// </summary>
        /// <param name="type">Тип содержащий набор значений величины.</param>
        /// <param name="memberName">Имя члена объекта содержащий набор значений величины.</param>
        /// <param name="memberType">Тип члена объекта.</param>
        public LotusListValuesAttribute(Type type, string memberName, TInspectorMemberType memberType)
        {
            _listValues = type;
            _memberName = memberName;
            _memberType = memberType;
        }
        #endregion
    }
    /**@}*/
}