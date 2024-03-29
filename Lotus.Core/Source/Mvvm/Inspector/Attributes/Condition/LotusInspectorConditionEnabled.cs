using System;

namespace Lotus.Core.Inspector
{
    /**
     * \defgroup CoreInspectorAttribute Атрибуты для инспектора свойств
     * \ingroup CoreInspector
     * \brief Атрибуты для инспектора свойств управляют отдельным аспектами отображения свойства и расширяют его управления.
     * @{
     */
    /// <summary>
    /// Атрибут доступности для редактирования(поля/свойства)в зависимости от логического условия равенства.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
#if UNITY_2017_1_OR_NEWER
	public sealed class LotusEnabledEqualityAttribute : UnityEngine.PropertyAttribute
#else
    public sealed class LotusEnabledEqualityAttribute : Attribute
#endif
    {
        #region Fields
        internal string _managingMemberName;
        internal TInspectorMemberType _memberType;
        internal bool _value;
        #endregion

        #region Properties
        /// <summary>
        /// Имя члена объекта от которого зависит доступность свойства для редактирования.
        /// </summary>
        public string ManagingMemberName
        {
            get { return _managingMemberName; }
            set { _managingMemberName = value; }
        }

        /// <summary>
        /// Тип члена объекта.
        /// </summary>
        public TInspectorMemberType MemberType
        {
            get { return _memberType; }
            set { _memberType = value; }
        }

        /// <summary>
        /// Целевое значение поля/свойства при котором доступно редактирование.
        /// </summary>
        public bool Value
        {
            get { return _value; }
            set { _value = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор инициализирует объект класса указанными параметрами.
        /// </summary>
        /// <param name="managingMemberName">Имя члена объекта от которого зависит доступность свойства для редактирования.</param>
        /// <param name="memberType">Тип члена объекта.</param>
        /// <param name="value">Целевое значение поля/свойства при котором доступно редактирование.</param>
        public LotusEnabledEqualityAttribute(string managingMemberName, TInspectorMemberType memberType, bool value)
        {
            _managingMemberName = managingMemberName;
            _memberType = memberType;
            _value = value;
        }
        #endregion
    }
    /**@}*/
}