using System;
using System.Collections.Generic;

namespace Lotus.Maths
{
    /** \addtogroup MathSpline
	*@{*/
    /// <summary>
    /// Квадратичная кривая Безье.
    /// </summary>
    /// <remarks>
    /// Квадратичная кривая Безье второго порядка создается тремя опорным точками.
    /// При этом кривая проходит только через начальную и конечную точку.
    /// Другая точка (будет назвать её управляющей) определяет лишь форму кривой.
    /// </remarks>
    [Serializable]
    public class BezierQuadratic2D : SplineBase2D
    {
        #region Static methods
        /// <summary>
        /// Вычисление точки на кривой Безье представленной с помощью трех контрольных точек.
        /// </summary>
        /// <param name="time">Положение точки от 0 до 1, где 0 соответствует крайней "левой" точки, 1 соответствует крайне
        /// "правой" конечной точки кривой.</param>
        /// <param name="start">Начальная точка.</param>
        /// <param name="handlePoint">Контрольная точка.</param>
        /// <param name="end">Конечная точка.</param>
        /// <returns>Позиция точки на кривой Безье.</returns>
        public static Vector2Df CalculatePoint(float time, Vector2Df start, Vector2Df handlePoint, Vector2Df end)
        {
            var u = 1 - time;
            var tt = time * time;
            var uu = u * u;

            return (uu * start) + (2 * time * u * handlePoint) + (tt * end);
        }

        /// <summary>
        /// Вычисление точки на кривой Безье представленной с помощью трех контрольных точек.
        /// </summary>
        /// <param name="time">Положение точки от 0 до 1, где 0 соответствует крайней "левой" точки, 1 соответствует крайне
        /// "правой" конечной точки кривой.</param>
        /// <param name="start">Начальная точка.</param>
        /// <param name="handlePoint">Контрольная точка.</param>
        /// <param name="end">Конечная точка.</param>
        /// <returns>Позиция точки на кривой Безье.</returns>
        public static Vector2Df CalculatePoint(float time, ref Vector2Df start, ref Vector2Df handlePoint, ref Vector2Df end)
        {
            var u = 1 - time;
            var tt = time * time;
            var uu = u * u;

            return (uu * start) + (2 * time * u * handlePoint) + (tt * end);
        }

        /// <summary>
        /// Вычисление первой производной точки на кривой Безье представленной с помощью трех контрольных точек.
        /// </summary>
        /// <remarks>
        /// Первая производная показывает скорость изменения функции в данной точки.
        /// Физическим смысл производной - скорость в данной точке.
        /// </remarks>
        /// <param name="time">Положение точки от 0 до 1, где 0 соответствует крайней "левой" точки, 1 соответствует крайне
        /// "правой" конечной точки кривой.</param>
        /// <param name="start">Начальная точка.</param>
        /// <param name="handlePoint">Контрольная точка.</param>
        /// <param name="end">Конечная точка.</param>
        /// <returns>Первая производная точки на кривой Безье.</returns>
        public static Vector2Df CalculateFirstDerivative(float time, Vector2Df start, Vector2Df handlePoint, Vector2Df end)
        {
            return (2f * (1f - time) * (handlePoint - start)) + (2f * time * (end - handlePoint));
        }

        /// <summary>
        /// Вычисление первой производной точки на кривой Безье представленной с помощью трех контрольных точек.
        /// </summary>
        /// <remarks>
        /// Первая производная показывает скорость изменения функции в данной точки.
        /// Физическим смысл производной - скорость в данной точке.
        /// </remarks>
        /// <param name="time">Положение точки от 0 до 1, где 0 соответствует крайней "левой" точки, 1 соответствует крайне
        /// "правой" конечной точки кривой.</param>
        /// <param name="start">Начальная точка.</param>
        /// <param name="handlePoint">Контрольная точка.</param>
        /// <param name="end">Конечная точка.</param>
        /// <returns>Первая производная точки на кривой Безье.</returns>
        public static Vector2Df CalculateFirstDerivative(float time, ref Vector2Df start, ref Vector2Df handlePoint, ref Vector2Df end)
        {
            return (2f * (1f - time) * (handlePoint - start)) + (2f * time * (end - handlePoint));
        }
        #endregion

        #region Properties
        /// <summary>
        /// Управляющая точка.
        /// </summary>
        public Vector2Df HandlePoint
        {
            get { return _controlPoints[1]; }
            set { _controlPoints[1] = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию инициализирует объект класса предустановленными значениями.
        /// </summary>
        public BezierQuadratic2D()
            : base(3)
        {
        }

        /// <summary>
        /// Конструктор инициализирует объект класса указанными параметрами.
        /// </summary>
        /// <param name="startPoint">Начальная точка.</param>
        /// <param name="endPoint">Конечная точка.</param>
        public BezierQuadratic2D(Vector2Df startPoint, Vector2Df endPoint)
            : base(startPoint, endPoint)
        {
        }
        #endregion

        #region ILotusSpline2D methods
        /// <summary>
        /// Вычисление точки на сплайне.
        /// </summary>
        /// <param name="time">Положение точки от 0 до 1.</param>
        /// <returns>Позиция точки на сплайне.</returns>
        public override Vector2Df CalculatePoint(float time)
        {
            var u = 1 - time;
            var tt = time * time;
            var uu = u * u;

            return (uu * _controlPoints[0]) + (2 * time * u * _controlPoints[1]) + (tt * _controlPoints[2]);
        }
        #endregion

        #region Main methods
        /// <summary>
        /// Вычисление первой производной точки на кривой Безье.
        /// </summary>
        /// <remarks>
        /// Первая производная показывает скорость изменения функции в данной точки.
        /// Физическим смысл производной - скорость на данной точке 
        /// </remarks>
        /// <param name="time">Положение точки от 0 до 1, где 0 соответствует крайней "левой" точки, 1 соответствует крайне
        /// "правой" конечной точки кривой</param>
        /// <returns>Первая производная точки на кривой Безье.</returns>
        public Vector2Df CalculateFirstDerivative(float time)
        {
            return (2f * (1f - time) * (HandlePoint - StartPoint)) + (2f * time * (EndPoint - HandlePoint));
        }

        /// <summary>
        /// Проверка на управляющую точку.
        /// </summary>
        /// <param name="index">Позиция(индекс) контрольной точки.</param>
        /// <returns>Статус управляющей точки.</returns>
        public bool IsHandlePoint(int index)
        {
            return index == 1;
        }
        #endregion
    }

    /// <summary>
    /// Кубическая кривая Безье.
    /// </summary>
    /// <remarks>
    /// Кубическая кривая Безье третьего порядка создается четырьмя опорным точками.
    /// При этом кривая проходит только через начальную и конечную точку.
    /// Другие две точки (будет назвать их управляющими) определяет лишь форму кривой.
    /// </remarks>
    [Serializable]
    public class BezierCubic2D : SplineBase2D
    {
        #region Static methods
        /// <summary>
        /// Вычисление точки на кривой Безье представленной с помощью четырех контрольных точек.
        /// </summary>
        /// <param name="time">Положение точки от 0 до 1, где 0 соответствует крайней "левой" точки, 1 соответствует крайне
        /// "правой" конечной точки кривой.</param>
        /// <param name="start">Начальная точка.</param>
        /// <param name="handlePoint1">Первая управляющая точка.</param>
        /// <param name="handlePoint2">Вторая управляющая точка.</param>
        /// <param name="end">Конечная точка.</param>
        /// <returns>Позиция точки на кривой Безье.</returns>
        public static Vector2Df CalculatePoint(float time, Vector2Df start, Vector2Df handlePoint1, Vector2Df handlePoint2, Vector2Df end)
        {
            var u = 1 - time;
            var tt = time * time;
            var uu = u * u;
            var uuu = uu * u;
            var ttt = tt * time;

            var point = uuu * start;

            point += 3 * uu * time * handlePoint1;
            point += 3 * u * tt * handlePoint2;
            point += ttt * end;

            return point;
        }

        /// <summary>
        /// Вычисление точки на кривой Безье представленной с помощью четырех контрольных точек.
        /// </summary>
        /// <param name="time">Положение точки от 0 до 1, где 0 соответствует крайней "левой" точки, 1 соответствует крайне
        /// "правой" конечной точки кривой.</param>
        /// <param name="start">Начальная точка.</param>
        /// <param name="handlePoint1">Первая управляющая точка.</param>
        /// <param name="handlePoint2">Вторая управляющая точка.</param>
        /// <param name="end">Конечная точка.</param>
        /// <returns>Позиция точки на кривой Безье.</returns>
        public static Vector2Df CalculatePoint(float time, ref Vector2Df start, ref Vector2Df handlePoint1,
            ref Vector2Df handlePoint2, ref Vector2Df end)
        {
            var u = 1 - time;
            var tt = time * time;
            var uu = u * u;
            var uuu = uu * u;
            var ttt = tt * time;

            var point = uuu * start;

            point += 3 * uu * time * handlePoint1;
            point += 3 * u * tt * handlePoint2;
            point += ttt * end;

            return point;
        }

        /// <summary>
        /// Вычисление первой производной точки на кривой Безье представленной с помощью четырех контрольных точек.
        /// </summary>
        /// <remarks>
        /// Первая производная показывает скорость изменения функции в данной точки.
        /// Физическим смысл производной - скорость в данной точке.
        /// </remarks>
        /// <param name="time">Положение точки от 0 до 1, где 0 соответствует крайней "левой" точки, 1 соответствует крайне
        /// "правой" конечной точки кривой</param>
        /// <param name="start">Начальная точка.</param>
        /// <param name="handlePoint1">Первая управляющая точка.</param>
        /// <param name="handlePoint2">Вторая управляющая точка.</param>
        /// <param name="end">Конечная точка.</param>
        /// <returns>Первая производная точки на кривой Безье.</returns>
        public static Vector2Df CalculateFirstDerivative(float time, Vector2Df start, Vector2Df handlePoint1, Vector2Df handlePoint2, Vector2Df end)
        {
            var u = 1 - time;
            return (3f * u * u * (handlePoint1 - start)) +
                   (6f * u * time * (handlePoint2 - handlePoint1)) +
                   (3f * time * time * (end - handlePoint2));

        }

        /// <summary>
        /// Вычисление первой производной точки на кривой Безье представленной с помощью четырех контрольных точек.
        /// </summary>
        /// <remarks>
        /// Первая производная показывает скорость изменения функции в данной точки.
        /// Физическим смысл производной - скорость в данной точке.
        /// </remarks>
        /// <param name="time">Положение точки от 0 до 1, где 0 соответствует крайней "левой" точки, 1 соответствует крайне
        /// "правой" конечной точки кривой.</param>
        /// <param name="start">Начальная точка.</param>
        /// <param name="handlePoint1">Первая управляющая точка.</param>
        /// <param name="handlePoint2">Вторая управляющая точка.</param>
        /// <param name="end">Конечная точка.</param>
        /// <returns>Первая производная точки на кривой Безье.</returns>
        public static Vector2Df CalculateFirstDerivative(float time, ref Vector2Df start, ref Vector2Df handlePoint1,
            ref Vector2Df handlePoint2, ref Vector2Df end)
        {
            var u = 1 - time;
            return (3f * u * u * (handlePoint1 - start)) +
                   (6f * u * time * (handlePoint2 - handlePoint1)) +
                   (3f * time * time * (end - handlePoint2));
        }
        #endregion

        #region Properties
        /// <summary>
        /// Первая управляющая точка.
        /// </summary>
        public Vector2Df HandlePoint1
        {
            get { return _controlPoints[1]; }
            set { _controlPoints[1] = value; }
        }

        /// <summary>
        /// Вторая управляющая точка.
        /// </summary>
        public Vector2Df HandlePoint2
        {
            get { return _controlPoints[2]; }
            set { _controlPoints[2] = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию инициализирует объект класса предустановленными значениями.
        /// </summary>
        public BezierCubic2D()
            : base(4)
        {
        }

        /// <summary>
        /// Конструктор инициализирует объект класса указанными параметрами.
        /// </summary>
        /// <param name="startPoint">Начальная точка.</param>
        /// <param name="endPoint">Конечная точка.</param>
        public BezierCubic2D(Vector2Df startPoint, Vector2Df endPoint)
                            : base(4)
        {
            _controlPoints[0] = startPoint;
            _controlPoints[1] = (startPoint + endPoint) / 3;
            _controlPoints[2] = (startPoint + endPoint) / 3 * 2;
            _controlPoints[3] = endPoint;
        }
        #endregion

        #region ILotusSpline2D methods
        /// <summary>
        /// Вычисление точки на сплайне.
        /// </summary>
        /// <param name="time">Положение точки от 0 до 1.</param>
        /// <returns>Позиция точки на сплайне.</returns>
        public override Vector2Df CalculatePoint(float time)
        {
            var u = 1 - time;
            var tt = time * time;
            var uu = u * u;
            var uuu = uu * u;
            var ttt = tt * time;

            var point = uuu * _controlPoints[0];

            point += 3 * uu * time * _controlPoints[1];
            point += 3 * u * tt * _controlPoints[2];
            point += ttt * _controlPoints[3];

            return point;
        }
        #endregion

        #region Main methods
        /// <summary>
        /// Создание кубической кривой проходящий через заданные(опорные) точки на равномерно заданном времени.
        /// </summary>
        /// <param name="start">Начальная точка.</param>
        /// <param name="point1">Первая точка.</param>
        /// <param name="point2">Вторая точка.</param>
        /// <param name="end">Конечная точка.</param>
        public void CreateFromPivotPoint(Vector2Df start, Vector2Df point1, Vector2Df point2, Vector2Df end)
        {
            _controlPoints[0] = start;
            _controlPoints[1].X = ((-5 * start.X) + (18 * point1.X) - (9 * point2.X) + (2 * end.X)) / 6;
            _controlPoints[1].Y = ((-5 * start.Y) + (18 * point1.Y) - (9 * point2.Y) + (2 * end.Y)) / 6;
            _controlPoints[2].X = ((2 * start.X) - (9 * point1.X) + (18 * point2.X) - (5 * end.X)) / 6;
            _controlPoints[2].Y = ((2 * start.Y) - (9 * point1.Y) + (18 * point2.Y) - (5 * end.Y)) / 6;
            _controlPoints[3] = end;
        }

        /// <summary>
        /// Вычисление первой производной точки на кривой Безье.
        /// </summary>
        /// <remarks>
        /// Первая производная показывает скорость изменения функции в данной точки.
        /// Физическим смысл производной - скорость на данной точке 
        /// </remarks>
        /// <param name="time">Положение точки от 0 до 1, где 0 соответствует крайней "левой" точки, 1 соответствует крайне
        /// "правой" конечной точки кривой</param>
        /// <returns>Первая производная точки на кривой Безье.</returns>
        public Vector2Df CalculateFirstDerivative(float time)
        {
            var u = 1 - time;
            return (3f * u * u * (HandlePoint1 - StartPoint)) +
                   (6f * u * time * (HandlePoint2 - HandlePoint1)) +
                   (3f * time * time * (EndPoint - HandlePoint2));
        }

        /// <summary>
        /// Проверка на управляющую точку.
        /// </summary>
        /// <param name="index">Позиция(индекс) контрольной точки.</param>
        /// <returns>Статус управляющей точки.</returns>
        public bool IsHandlePoint(int index)
        {
            return index == 1 || index == 2;
        }
        #endregion
    }

    /// <summary>
    /// Режим редактирования управляющей точки.
    /// </summary>
    public enum TBezierHandleMode
    {
        /// <summary>
        /// Свободный режим.
        /// </summary>
        Free,

        /// <summary>
        /// Режим - при котором вторая управляющая точка(смежная по отношению к опорной) располагается симметрично.
        /// </summary>
        Aligned,

        /// <summary>
        /// Режим - при котором вторая управляющая точка(смежная по отношению к опорной) располагается симметрично и.
        /// на таком же расстоянии как и редактируемая точка.
        /// </summary>
        Mirrored
    }

    /// <summary>
    /// Путь состоящий из кривых Безье.
    /// </summary>
    /// <remarks>
    /// Реализация пути последовательно состоящего из кубических кривых Безье.
    /// Путь проходит через заданные опорные точки, управляющие точки определяют форму пути.
    /// </remarks>
    [Serializable]
    public class BezierPath2D : SplineBase2D
    {
        #region Fields
        // Основные параметры
#if UNITY_2017_1_OR_NEWER
		[UnityEngine.SerializeField]
#endif
        internal bool _isClosed;
#if UNITY_2017_1_OR_NEWER
		[UnityEngine.SerializeField]
#endif
        internal TBezierHandleMode[] _handleModes;
        #endregion

        #region Properties
        /// <summary>
        /// Статус замкнутости сплайна.
        /// </summary>
        public bool IsClosed
        {
            get { return _isClosed; }
            set
            {
                if (_isClosed != value)
                {
                    _isClosed = value;

                    if (_isClosed == true)
                    {
                        _handleModes[_handleModes.Length - 1] = _handleModes[0];
                        SetControlPoint(0, _controlPoints[0]);
                    }

                    OnUpdateSpline();
                }
            }
        }

        /// <summary>
        /// Количество кривых в пути.
        /// </summary>
        public int CurveCount
        {
            get { return (_controlPoints.Length - 1) / 3; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию инициализирует объект класса предустановленными значениями.
        /// </summary>
        public BezierPath2D()
            : base(4)
        {
            _handleModes = new TBezierHandleMode[4];
        }

        /// <summary>
        /// Конструктор осуществляет построение пути(связанных кривых) Безье на основе опорных точек пути.
        /// </summary>
        /// <remarks>
        /// Промежуточные управляющие точки генерируется автоматически.
        /// </remarks>
        /// <param name="pivotPoints">Опорные точки пути.</param>
        public BezierPath2D(params Vector2Df[] pivotPoints)
            : base(pivotPoints)
        {
            _handleModes = new TBezierHandleMode[4];
            CreateFromPivotPoints(pivotPoints);
        }
        #endregion

        #region ILotusSpline2D methods
        /// <summary>
        /// Вычисление точки на сплайне.
        /// </summary>
        /// <param name="time">Положение точки от 0 до 1.</param>
        /// <returns>Позиция точки на сплайне.</returns>
        public override Vector2Df CalculatePoint(float time)
        {
            int index_curve;
            if (time >= 1f)
            {
                time = 1f;
                index_curve = _controlPoints.Length - 4;
            }
            else
            {
                time = XMath.Clamp01(time) * CurveCount;
                index_curve = (int)time;
                time -= index_curve;
                index_curve *= 3;
            }

            var point = BezierCubic2D.CalculatePoint(time,
                ref _controlPoints[index_curve],
                ref _controlPoints[index_curve + 1],
                ref _controlPoints[index_curve + 2],
                ref _controlPoints[index_curve + 3]);

            return point;
        }

        /// <summary>
        /// Растеризация сплайна - вычисление точек отрезков для рисования сплайна.
        /// </summary>
        /// <remarks>
        /// Качество(степень) растеризации зависит от свойства <see cref="SplineBase2D.SegmentsSpline"/>.
        /// </remarks>
        public override void ComputeDrawingPoints()
        {
            _drawingPoints.Clear();

            for (var i = 0; i < CurveCount; i++)
            {
                var prev = CalculateCurvePoint(i, 0);
                _drawingPoints.Add(prev);
                for (var ip = 1; ip < SegmentsSpline; ip++)
                {
                    var time = (float)ip / SegmentsSpline;
                    var point = CalculateCurvePoint(i, time);

                    // Добавляем если длина больше 1,4
                    if ((point - prev).SqrLength > MinimalSqrLine)
                    {
                        _drawingPoints.Add(point);
                        prev = point;
                    }
                }
            }

            if (_isClosed)
            {
                CheckCorrectStartPoint();
            }
            else
            {
                CheckCorrectEndPoint();
            }
        }
        #endregion

        #region Main methods
        /// <summary>
        /// Установка режима редактирования управляющей точки.
        /// </summary>
        /// <param name="index">Позиция(индекс) контрольной точки.</param>
        private void SetHandleMode(int index)
        {
            var mode_index = (index + 1) / 3;
            var mode = _handleModes[mode_index];
            if (mode == TBezierHandleMode.Free || (!_isClosed && (mode_index == 0 || mode_index == _handleModes.Length - 1)))
            {
                return;
            }

            var middle_index = mode_index * 3;
            int fixed_index, enforced_index;
            if (index <= middle_index)
            {
                fixed_index = middle_index - 1;
                if (fixed_index < 0)
                {
                    fixed_index = _controlPoints.Length - 2;
                }
                enforced_index = middle_index + 1;
                if (enforced_index >= _controlPoints.Length)
                {
                    enforced_index = 1;
                }
            }
            else
            {
                fixed_index = middle_index + 1;
                if (fixed_index >= _controlPoints.Length)
                {
                    fixed_index = 1;
                }
                enforced_index = middle_index - 1;
                if (enforced_index < 0)
                {
                    enforced_index = _controlPoints.Length - 2;
                }
            }

            var middle = _controlPoints[middle_index];
            var enforced_tangent = middle - _controlPoints[fixed_index];
            if (mode == TBezierHandleMode.Aligned)
            {
                enforced_tangent = enforced_tangent.Normalized * Vector2Df.Distance(middle, _controlPoints[enforced_index]);
            }

            _controlPoints[enforced_index] = middle + enforced_tangent;
        }

        /// <summary>
        /// Создание пути Безье проходящего через заданные(опорные) точки.
        /// </summary>
        /// <remarks>
        /// Промежуточные управляющие точки генерируется автоматически.
        /// </remarks>
        /// <param name="pivotPoints">Опорные точки пути.</param>
        public void CreateFromPivotPoints(params Vector2Df[] pivotPoints)
        {
            // Если точек меньше двух выходим
            if (pivotPoints.Length < 2)
            {
                return;
            }

            var points = new List<Vector2Df>();
            for (var i = 0; i < pivotPoints.Length; i++)
            {
                // Первая точка
                if (i == 0)
                {
                    var p1 = pivotPoints[i];
                    var p2 = pivotPoints[i + 1];

                    // Расстояние
                    var distance = (p2 - p1).Length;
                    var q1 = p1 + (distance * 0.5f * Vector2Df.Right);

                    points.Add(p1);
                    points.Add(q1);
                }
                else if (i == pivotPoints.Length - 1) //last
                {
                    var p0 = pivotPoints[i - 1];
                    var p1 = pivotPoints[i];

                    // Расстояние
                    var distance = (p0 - p1).Length;
                    var q0 = p1 + (distance * 0.5f * Vector2Df.Right);

                    points.Add(q0);
                    points.Add(p1);
                }
                else
                {
                    var p0 = pivotPoints[i - 1];
                    var p1 = pivotPoints[i];
                    var p2 = pivotPoints[i + 1];

                    // Расстояние
                    var distance1 = (p1 - p0).Length;
                    var distance2 = (p2 - p1).Length;

                    var q0 = p1 + (distance1 * 0.5f * Vector2Df.Left);
                    var q1 = p1 + (distance2 * 0.5f * Vector2Df.Right);

                    points.Add(q0);
                    points.Add(p1);
                    points.Add(q1);
                }
            }

            // При необходимости изменяем размер массива
            if (_controlPoints.Length != points.Count)
            {
                Array.Resize(ref _controlPoints, points.Count);
            }

            // Копируем данные
            for (var i = 0; i < points.Count; i++)
            {
                _controlPoints[i] = points[i];
            }

            OnUpdateSpline();
        }

        /// <summary>
        /// Установка контрольной точки сплайна по индексу в локальных координатах.
        /// </summary>
        /// <param name="index">Позиция(индекс) точки.</param>
        /// <param name="point">Контрольная точка сплайна в локальных координатах.</param>
        /// <param name="updateSpline">Статус обновления сплайна.</param>
        public override void SetControlPoint(int index, Vector2Df point, bool updateSpline = false)
        {
            if (index % 3 == 0)
            {
                var delta = point - _controlPoints[index];
                if (_isClosed)
                {
                    if (index == 0)
                    {
                        _controlPoints[1] += delta;
                        _controlPoints[_controlPoints.Length - 2] += delta;
                        _controlPoints[_controlPoints.Length - 1] = point;
                    }
                    else if (index == _controlPoints.Length - 1)
                    {
                        _controlPoints[0] = point;
                        _controlPoints[1] += delta;
                        _controlPoints[index - 1] += delta;
                    }
                    else
                    {
                        _controlPoints[index - 1] += delta;
                        _controlPoints[index + 1] += delta;
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        _controlPoints[index - 1] += delta;
                    }
                    if (index + 1 < _controlPoints.Length)
                    {
                        _controlPoints[index + 1] += delta;
                    }
                }
            }
            _controlPoints[index] = point;
            SetHandleMode(index);

            if (updateSpline)
            {
                OnUpdateSpline();
            }
        }
        #endregion

        #region Curve methods
        /// <summary>
        /// Добавить кривую.
        /// </summary>
        public void AddCurve()
        {
            var point = _controlPoints[_controlPoints.Length - 1];
            Array.Resize(ref _controlPoints, _controlPoints.Length + 3);
            point.X += 100f;
            _controlPoints[_controlPoints.Length - 3] = point;
            point.X += 100f;
            _controlPoints[_controlPoints.Length - 2] = point;
            point.X += 100f;
            _controlPoints[_controlPoints.Length - 1] = point;

            Array.Resize(ref _handleModes, _handleModes.Length + 1);
            _handleModes[_handleModes.Length - 1] = _handleModes[_handleModes.Length - 2];
            SetHandleMode(_controlPoints.Length - 4);

            if (_isClosed)
            {
                _controlPoints[_controlPoints.Length - 1] = _controlPoints[0];
                _handleModes[_handleModes.Length - 1] = _handleModes[0];
                SetHandleMode(0);
            }

            OnUpdateSpline();
        }

        /// <summary>
        /// Удаление последней кривой.
        /// </summary>
        public void RemoveCurve()
        {
            if (CurveCount > 1)
            {
                Array.Resize(ref _controlPoints, _controlPoints.Length - 3);
                Array.Resize(ref _handleModes, _handleModes.Length - 1);
                SetHandleMode(_controlPoints.Length - 2, TBezierHandleMode.Free);
                if (_isClosed)
                {
                    _controlPoints[_controlPoints.Length - 1] = _controlPoints[0];
                    _handleModes[_handleModes.Length - 1] = _handleModes[0];
                    SetHandleMode(0);
                }

                OnUpdateSpline();
            }
        }

        /// <summary>
        /// Вычисление точки на отдельной кривой Безье.
        /// </summary>
        /// <param name="curveIndex">Индекс кривой.</param>
        /// <param name="time">Время.</param>
        /// <returns>Точка.</returns>
        public Vector2Df CalculateCurvePoint(int curveIndex, float time)
        {
            var node_index = curveIndex * 3;

            return BezierCubic2D.CalculatePoint(time,
                ref _controlPoints[node_index],
                ref _controlPoints[node_index + 1],
                ref _controlPoints[node_index + 2],
                ref _controlPoints[node_index + 3]);
        }

        /// <summary>
        /// Получение контрольной точки на отдельной кривой Безье.
        /// </summary>
        /// <param name="curveIndex">Индекс кривой.</param>
        /// <param name="pointIndex">Индекс контрольной точки.</param>
        /// <returns>Контрольная точка.</returns>
        public Vector2Df GetCurveControlPoint(int curveIndex, int pointIndex)
        {
            curveIndex *= 3;
            return _controlPoints[curveIndex + pointIndex];
        }

        /// <summary>
        /// Установка позиции контрольной точки на отдельной кривой Безье.
        /// </summary>
        /// <param name="curveIndex">Индекс кривой.</param>
        /// <param name="pointIndex">Индекс точки.</param>
        /// <param name="position">Позиция контрольной точки.</param>
        public void SetCurveControlPoint(int curveIndex, int pointIndex, Vector2Df position)
        {
            curveIndex *= 3;
            _controlPoints[curveIndex + pointIndex] = position;
        }
        #endregion

        #region Point methods
        /// <summary>
        /// Проверка на управляющую точку.
        /// </summary>
        /// <param name="index">Позиция(индекс) контрольной точки.</param>
        /// <returns>Статус управляющей точки.</returns>
        public bool IsHandlePoint(int index)
        {
            return index == 1 ||
                   index == 2 ||
                   index % 3 == 1 ||
                   index % 3 == 2;
        }

        /// <summary>
        /// Получения режима редактирования управляющей точки.
        /// </summary>
        /// <param name="index">Позиция(индекс) контрольной точки.</param>
        /// <returns>Режим редактирования управляющей точки.</returns>
        public TBezierHandleMode GetHandleMode(int index)
        {
            return _handleModes[(index + 1) / 3];
        }

        /// <summary>
        /// Установка режима редактирования управляющей точки.
        /// </summary>
        /// <param name="index">Позиция(индекс) контрольной точки.</param>
        /// <param name="mode">Режим редактирования управляющей точки.</param>
        public void SetHandleMode(int index, TBezierHandleMode mode)
        {
            var mode_index = (index + 1) / 3;
            _handleModes[mode_index] = mode;

            if (_isClosed)
            {
                if (mode_index == 0)
                {
                    _handleModes[_handleModes.Length - 1] = mode;
                }
                else if (mode_index == _handleModes.Length - 1)
                {
                    _handleModes[0] = mode;
                }
            }

            SetHandleMode(index);
        }
        #endregion
    }
    /**@}*/
}