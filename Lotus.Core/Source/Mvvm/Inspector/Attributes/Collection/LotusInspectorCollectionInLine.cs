using System;

namespace Lotus.Core.Inspector
{
    /** \addtogroup CoreInspectorAttribute
	*@{*/
    /// <summary>
    /// Атрибут для отображения дочерних свойств объекта в одну линию при показе в списке.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Функциональность атрибута реализуется атрибутом <see cref="LotusReorderableAttribute"/>
    /// </para>
    /// <para>
    /// Должен применяться для сложных типов, при этом дочерние поля/свойства должны иметь атрибут <see cref="LotusColumnAttribute"/>
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class LotusInLineAttribute : Attribute
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Main methods
#if UNITY_2017_1_OR_NEWER
		/// <summary>
		/// Получение массива прямоугольников в соответствии с заданной разбивкой.
		/// </summary>
		/// <remarks>
		/// Специальный метод для разбивки прямоугольников для отображения полей объекта в одну линию.
		/// </remarks>
		/// <param name="count">Количество элементов.</param>
		/// <param name="percents">Список процентов ширины для каждого поля.</param>
		/// <returns>Массив прямоугольников.</returns>
		public UnityEngine.Rect[] GetRectsFromPercent(Int32 count, params Single[] percents)
		{
			UnityEngine.Rect[] rects = null;
			if (count > 0)
			{
				rects = new UnityEngine.Rect[percents.Length + 1];

				// Считать будем от ширины с минусом по единицы на каждую колонку и минус ширина для вывода индекса
				Single index_width = 12;
				if (count > 9)
				{
					index_width = 16;
				}

				rects[0].x = 0;
				rects[0].y = 0;
				rects[0].width = index_width;
				rects[0].height = 18;
				for (var i = 0; i < percents.Length; i++)
				{
					rects[0].x = 0;
				}

			}
			else
			{
				rects = new UnityEngine.Rect[percents.Length];
			}

			return (rects);
		}
#endif
        #endregion
    }
    /**@}*/
}