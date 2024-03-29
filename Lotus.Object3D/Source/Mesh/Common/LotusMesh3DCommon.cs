using Lotus.Maths;

namespace Lotus.Object3D
{
    /**
     * \defgroup Object3DMesh Подсистема меша
     * \ingroup Object3D
     * \brief Подсистема трёхмерной полигональной сетки(меша) представляет собой совокупность данных предназначенных для 
		представления, хранения и генерации трёхмерной полигональной сетки.
     * \defgroup Object3DMeshCommon Общее данные
     * \ingroup Object3DMesh
     * \brief Общие данные содержат базовые представления структурных элементов трехмерной сетки.
     * @{
     */
    /// <summary>
    /// Тип структурного элемента меша.
    /// </summary>
    /// <remarks>
    /// Тип структурного элемента меша определяет различные классы данных которые содержат, как и сами данные так и
    /// отношения между ними
    /// </remarks>
    public enum TMeshElement
    {
        /// <summary>
        /// Вершина — это позиция вместе с другой информацией, такой как цвет, нормальный вектор и координаты текстуры.
        /// </summary>
        Vertex,

        /// <summary>
        /// Ребро — это соединение между двумя вершинами.
        /// </summary>
        Edge,

        /// <summary>
        /// Треугольник - это базовая грань которая содержит замкнутое множество трех рёбер.
        /// </summary>
        Triangle,

        /// <summary>
        /// Четырехугольник - это грань которая содержит замкнутое множество четырех рёбер.
        /// </summary>
        Quad,

        /// <summary>
        /// Трёхмерная полигональная сетка.
        /// </summary>
        Mesh
    }

    /// <summary>
    /// Набор операций которые поддерживает каждый структурный элемент меша.
    /// </summary>
    public interface ILotusMeshOperaiton
    {
        #region Properties
        /// <summary>
        /// Тип структурного элемента меша.
        /// </summary>
        TMeshElement MeshElement { get; }
        #endregion

        #region Main methods
        /// <summary>
        /// Смещение вершин.
        /// </summary>
        /// <param name="offset">Вектор смещения.</param>
        void Move(Vector3Df offset);

        /// <summary>
        /// Врашение вершин.
        /// </summary>
        /// <param name="rotation">Кватернион вращения.</param>
        void Rotate(Quaternion3Df rotation);

        /// <summary>
        /// Однородное масштабирование вершин.
        /// </summary>
        /// <param name="scale">Масштаб.</param>
        void Scale(float scale);

        /// <summary>
        /// Масштабирование вершин.
        /// </summary>
        /// <param name="scale">Масштаб.</param>
        void Scale(Vector3Df scale);

        /// <summary>
        /// Обратить нормали вершин.
        /// </summary>
        void FlipNormals();

        /// <summary>
        /// Обратить развёртку текстурных координат по горизонтали.
        /// </summary>
        /// <param name="channel">Канал текстурных координат.</param>
        void FlipUVHorizontally(int channel = 0);

        /// <summary>
        /// Обратить развёртку текстурных координат по вертикали.
        /// </summary>
        /// <param name="channel">Канал текстурных координат.</param>
        void FlipUVVertically(int channel = 0);
        #endregion
    }

    /// <summary>
    /// Набор операций которые поддерживает только грани меша и их производные структуры.
    /// </summary>
    public interface ILotusMeshFaceOperaiton
    {

    }

    /// <summary>
    /// Статический класс для хранителя настроек подсистемы меша.
    /// </summary>
    public static class XMeshSetting
    {
        /// <summary>
        /// Точность вещественного числа используемого при сравнении позиции вершин меша.
        /// </summary>
        /// <remarks>
        /// Будет использовать единицу как один метр реального мира, тогда при сравнении расстояний меньше 0,1 миллиметра будем считать их одинаковыми.
        /// </remarks>
        public static float Eplsilon_f = 0.0001f;
    }
    /**@}*/
}