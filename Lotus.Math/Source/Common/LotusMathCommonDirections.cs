using System;

namespace Lotus.Maths
{
    /** \addtogroup MathCommon
	*@{*/
    /// <summary>
    /// Направление в двухмерном пространстве.
    /// </summary>
    public enum TDirection2D
    {
        /// <summary>
        /// Направление влево.
        /// </summary>
        /// <remarks>
        /// Значение по оси X уменьшается.
        /// </remarks>
        Left = 1,

        /// <summary>
        /// Направление вправо.
        /// </summary>
        /// <remarks>
        /// Значение по оси X увеличивается.
        /// </remarks>
        Right = 2,

        /// <summary>
        /// Направление вверх.
        /// </summary>
        /// <remarks>
        /// Значение по оси Y уменьшается.
        /// </remarks>
        Up = 4,

        /// <summary>
        /// Направление вниз.
        /// </summary>
        /// <remarks>
        /// Значение по оси Y увеличивается.
        /// </remarks>
        Down = 8
    }

    /// <summary>
    /// Выбор направления в двухмерном пространстве.
    /// </summary>
    public enum TDirectionSelect2D
    {
        /// <summary>
        /// Нет направления.
        /// </summary>
        None = 0,

        /// <summary>
        /// Направление влево.
        /// </summary>
        /// <remarks>
        /// Значение по оси X уменьшается.
        /// </remarks>
        Left = 1,

        /// <summary>
        /// Направление вправо.
        /// </summary>
        /// <remarks>
        /// Значение по оси X увеличивается.
        /// </remarks>
        Right = 2,

        /// <summary>
        /// Направление вверх.
        /// </summary>
        /// <remarks>
        /// Значение по оси Y уменьшается.
        /// </remarks>
        Up = 4,

        /// <summary>
        /// Направление вниз.
        /// </summary>
        /// <remarks>
        /// Значение по оси Y увеличивается.
        /// </remarks>
        Down = 8
    }

    /// <summary>
    /// Набор направлений в двухмерном пространстве.
    /// </summary>
    [Flags]
    public enum TDirectionSet2D
    {
        /// <summary>
        /// Нет направления.
        /// </summary>
        None = 0,

        /// <summary>
        /// Направление влево.
        /// </summary>
        /// <remarks>
        /// Значение по оси X уменьшается.
        /// </remarks>
        Left = 1,

        /// <summary>
        /// Направление вправо.
        /// </summary>
        /// <remarks>
        /// Значение по оси X увеличивается.
        /// </remarks>
        Right = 2,

        /// <summary>
        /// Направление вверх.
        /// </summary>
        /// <remarks>
        /// Значение по оси Y уменьшается.
        /// </remarks>
        Up = 4,

        /// <summary>
        /// Направление вниз.
        /// </summary>
        /// <remarks>
        /// Значение по оси Y увеличивается.
        /// </remarks>
        Down = 8,

        /// <summary>
        /// Направление по оси X.
        /// </summary>
        AxisX = Left | Right,

        /// <summary>
        /// Направление по оси Y.
        /// </summary>
        AxisY = Down | Up,

        /// <summary>
        /// Направление во все стороны.
        /// </summary>
        All = Left | Up | Right | Down
    }

    /// <summary>
    /// Направление в трехмерном пространстве.
    /// </summary>
    public enum TDirection3D
    {
        /// <summary>
        /// Направление влево.
        /// </summary>
        /// <remarks>
        /// Значение по оси X уменьшается.
        /// </remarks>
        Left = 1,

        /// <summary>
        /// Направление вправо.
        /// </summary>
        /// <remarks>
        /// Значение по оси X увеличивается.
        /// </remarks>
        Right = 2,

        /// <summary>
        /// Направление вверх.
        /// </summary>
        /// <remarks>
        /// Значение по оси Y увеличивается.
        /// </remarks>
        Up = 4,

        /// <summary>
        /// Направление вниз.
        /// </summary>
        /// <remarks>
        /// Значение по оси Y уменьшается.
        /// </remarks>
        Down = 8,

        /// <summary>
        /// Направление назад.
        /// </summary>
        /// <remarks>
        /// Значение по оси Z уменьшается.
        /// </remarks>
        Back = 16,

        /// <summary>
        /// Направление вперед.
        /// </summary>
        /// <remarks>
        /// Значение по оси Z увеличивается.
        /// </remarks>
        Forward = 32
    }

    /// <summary>
    /// Выбор направления в трехмерном пространстве.
    /// </summary>
    public enum TDirectionSelect3D
    {
        /// <summary>
        /// Нет направления.
        /// </summary>
        None = 0,

        /// <summary>
        /// Направление влево.
        /// </summary>
        /// <remarks>
        /// Значение по оси X уменьшается.
        /// </remarks>
        Left = 1,

        /// <summary>
        /// Направление вправо.
        /// </summary>
        /// <remarks>
        /// Значение по оси X увеличивается.
        /// </remarks>
        Right = 2,

        /// <summary>
        /// Направление вверх.
        /// </summary>
        /// <remarks>
        /// Значение по оси Y увеличивается.
        /// </remarks>
        Up = 4,

        /// <summary>
        /// Направление вниз.
        /// </summary>
        /// <remarks>
        /// Значение по оси Y уменьшается.
        /// </remarks>
        Down = 8,

        /// <summary>
        /// Направление назад.
        /// </summary>
        /// <remarks>
        /// Значение по оси Z уменьшается.
        /// </remarks>
        Back = 16,

        /// <summary>
        /// Направление вперед.
        /// </summary>
        /// <remarks>
        /// Значение по оси Z увеличивается.
        /// </remarks>
        Forward = 32
    }

    /// <summary>
    /// Набор направлений в трехмерном пространстве.
    /// </summary>
    [Flags]
    public enum TDirectionSet3D
    {
        /// <summary>
        /// Нет направления.
        /// </summary>
        None = 0,

        /// <summary>
        /// Направление влево.
        /// </summary>
        /// <remarks>
        /// Значение по оси X уменьшается.
        /// </remarks>
        Left = 1,

        /// <summary>
        /// Направление вправо.
        /// </summary>
        /// <remarks>
        /// Значение по оси X увеличивается.
        /// </remarks>
        Right = 2,

        /// <summary>
        /// Направление вверх.
        /// </summary>
        /// <remarks>
        /// Значение по оси Y увеличивается.
        /// </remarks>
        Up = 4,

        /// <summary>
        /// Направление вниз.
        /// </summary>
        /// <remarks>
        /// Значение по оси Y уменьшается.
        /// </remarks>
        Down = 8,

        /// <summary>
        /// Направление назад.
        /// </summary>
        /// <remarks>
        /// Значение по оси Z уменьшается.
        /// </remarks>
        Back = 16,

        /// <summary>
        /// Направление вперед.
        /// </summary>
        /// <remarks>
        /// Значение по оси Z увеличивается.
        /// </remarks>
        Forward = 32,

        /// <summary>
        /// Направление по оси X.
        /// </summary>
        AxisX = Left | Right,

        /// <summary>
        /// Направление по оси Y.
        /// </summary>
        AxisY = Down | Up,

        /// <summary>
        /// Направление по оси Z.
        /// </summary>
        AxisZ = Back | Forward,

        /// <summary>
        /// Направление во все стороны.
        /// </summary>
        All = Left | Right | Down | Up | Back | Forward
    }
    /**@}*/
}