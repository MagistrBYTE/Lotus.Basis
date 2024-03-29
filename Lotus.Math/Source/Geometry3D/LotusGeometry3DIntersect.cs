using System;

namespace Lotus.Maths
{
    /** \addtogroup MathGeometry3D
	*@{*/
    /// <summary>
    /// Тип пересечения в 3D пространстве.
    /// </summary>
    public enum TIntersectType3D
    {
        /// <summary>
        /// Пересечения нет.
        /// </summary>
        None,

        /// <summary>
        /// Пересечения представляет собой точку.
        /// Обычно пересечения луча с геометрическими объектами.
        /// </summary>
        Point,

        /// <summary>
        /// Пересечения представляет собой сегмент.
        /// </summary>
        Segment,

        /// <summary>
        /// Пересечения представляет собой луч.
        /// </summary>
        Ray,

        /// <summary>
        /// Пересечения представляет собой линию.
        /// </summary>
        Line,

        /// <summary>
        /// Пересечения представляет собой полигон.
        /// </summary>
        Polygon,

        /// <summary>
        /// Пересечения представляет собой плоскость.
        /// </summary>
        Plane,

        /// <summary>
        /// Другой тип пересечения.
        /// </summary>
        Other
    }

    /// <summary>
    /// Структура для хранения информации о пересечении в 3D пространстве.
    /// </summary>
    public struct TIntersectHit3D
    {
        #region Fields
        /// <summary>
        /// Дистанция.
        /// </summary>
        public double Distance;

        /// <summary>
        /// Барицентрические координаты.
        /// </summary>
        public Vector3D BarycentricCoordinate;

        /// <summary>
        /// Первая точка попадания.
        /// </summary>
        public Vector3D Point1;

        /// <summary>
        /// Вторая точка попадания.
        /// </summary>
        public Vector3D Point2;

        /// <summary>
        /// Тип пересечения.
        /// </summary>
        public TIntersectType3D IntersectType;
        #endregion
    }

    /// <summary>
    /// Структура для хранения информации о пересечении в 3D пространстве.
    /// </summary>
    public struct TIntersectHit3Df
    {
        #region Static methods
        /// <summary>
        /// Нет пересечения.
        /// </summary>
        /// <returns>Информация о пересечении.</returns>
        public static TIntersectHit3Df None()
        {
            var hit = new TIntersectHit3Df
            {
                IntersectType = TIntersectType3D.None
            };
            return hit;
        }

        /// <summary>
        /// Пересечения представляет собой точку.
        /// </summary>
        /// <param name="point">Точка пересечения.</param>
        /// <returns>Информация о пересечении.</returns>
        public static TIntersectHit3Df Point(in Vector3Df point)
        {
            var hit = new TIntersectHit3Df
            {
                IntersectType = TIntersectType3D.Point,
                Point1 = point
            };
            return hit;
        }

        /// <summary>
        /// Пересечения представляет собой точку.
        /// </summary>
        /// <param name="point">Точка пересечения.</param>
        /// <param name="distance">Дистанция пересечения.</param>
        /// <returns>Информация о пересечении.</returns>
        public static TIntersectHit3Df Point(in Vector3Df point, float distance)
        {
            var hit = new TIntersectHit3Df
            {
                IntersectType = TIntersectType3D.Point,
                Point1 = point,
                Distance = distance
            };
            return hit;
        }

        /// <summary>
        /// Пересечения представляет собой отрезок.
        /// </summary>
        /// <param name="point1">Первая точка пересечения.</param>
        /// <param name="point2">Вторая точка пересечения.</param>
        /// <returns>Информация о пересечении.</returns>
        public static TIntersectHit3Df Segment(in Vector3Df point1, in Vector3Df point2)
        {
            var hit = new TIntersectHit3Df
            {
                IntersectType = TIntersectType3D.Segment,
                Point1 = point1,
                Point2 = point2
            };
            return hit;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Тип пересечения.
        /// </summary>
        public TIntersectType3D IntersectType;

        /// <summary>
        /// Дистанция.
        /// </summary>
        public float Distance;

        /// <summary>
        /// Барицентрические координаты.
        /// </summary>
        public Vector3Df BarycentricCoordinate;

        /// <summary>
        /// Первая точка попадания.
        /// </summary>
        public Vector3Df Point1;

        /// <summary>
        /// Вторая точка попадания.
        /// </summary>
        public Vector3Df Point2;
        #endregion
    }

    /// <summary>
    /// Статический класс реализующий методы для работы с пересечением в 3D.
    /// </summary>
    public static class XIntersect3D
    {
        #region Point - Line 
        /// <summary>
        /// Проверка нахождения точки на линии.
        /// </summary>
        /// <param name="point">Проверяемая точка.</param>
        /// <param name="line">Линия.</param>
        /// <returns>Статус нахождения.</returns>
        public static bool PointLine(Vector3Df point, Line3Df line)
        {
            return PointLine(point, line.Position, line.Direction);
        }

        /// <summary>
        /// Проверка нахождения точки на линии.
        /// </summary>
        /// <param name="point">Проверяемая точка.</param>
        /// <param name="linePos">Позиция линии.</param>
        /// <param name="lineDir">Направление линии.</param>
        /// <returns>Статус нахождения.</returns>
        public static bool PointLine(Vector3Df point, Vector3Df linePos, Vector3Df lineDir)
        {
            return XDistance3D.PointLine(point, linePos, lineDir) < XGeometry3D.Eplsilon_f;
        }
        #endregion

        #region Point - Ray 
        /// <summary>
        /// Проверка на пересечении/нахождении некоторой точки на луче.
        /// </summary>
        /// <param name="point">Проверяемая точка.</param>
        /// <param name="ray">Луч.</param>
        /// <param name="length">Длина до этой точке, если она находится на луче.</param>
        /// <returns>Тип пересечения.</returns>
        public static TIntersectType3D PointRay(in Vector3D point, in Ray3D ray, out double length)
        {
            var delta = point - ray.Position;
            var tim = new Vector3D(delta.X / ray.Direction.X, delta.Y / ray.Direction.Y,
                delta.Z / ray.Direction.Z);

            // Сравниваем по компонентно
            if (XMath.Approximately(tim.X, tim.Y) && XMath.Approximately(tim.Y, tim.Z))
            {
                length = tim.X;
                return TIntersectType3D.None;
            }
            else
            {
                length = 0;
                return TIntersectType3D.Point;
            }
        }

        /// <summary>
        /// Проверка на нахождение точки на луче.
        /// </summary>
        /// <param name="point">Проверяемая точка.</param>
        /// <param name="ray">Луч.</param>
        /// <returns>Статус нахождения.</returns>
        public static bool PointRay(Vector3Df point, Ray3Df ray)
        {
            return PointRay(point, ray.Position, ray.Direction);
        }

        /// <summary>
        /// Проверка на нахождение точки на луче.
        /// </summary>
        /// <param name="point">Проверяемая точка.</param>
        /// <param name="rayPos">Позиция луча.</param>
        /// <param name="rayDir">Направление луча.</param>
        /// <returns>Статус нахождения.</returns>
        public static bool PointRay(Vector3Df point, Vector3Df rayPos, Vector3Df rayDir)
        {
            return XDistance3D.PointRay(point, rayPos, rayDir) < XGeometry3D.Eplsilon_f;
        }
        #endregion

        #region Point - Segment 
        /// <summary>
        /// Проверка нахождения точки на отрезке.
        /// </summary>
        /// <param name="point">Проверяемая точка.</param>
        /// <param name="segment">Отрезок.</param>
        /// <returns>Статус нахождения.</returns>
        public static bool PointSegment(in Vector3Df point, in Segment3Df segment)
        {
            return PointSegment(in point, in segment.Start, in segment.End);
        }

        /// <summary>
        /// Проверка нахождения точки на отрезке.
        /// </summary>
        /// <param name="point">Проверяемая точка.</param>
        /// <param name="start">Начало отрезка.</param>
        /// <param name="end">Конец отрезка.</param>
        /// <returns>Статус нахождения.</returns>
        public static bool PointSegment(in Vector3Df point, in Vector3Df start, in Vector3Df end)
        {
            return XDistance3D.PointSegment(point, start, end) < XGeometry3D.Eplsilon_f;
        }
        #endregion

        #region Point - Sphere 
        /// <summary>
        /// Проверка на попадание точки в область сферы.
        /// </summary>
        /// <param name="point">Проверяемая точка.</param>
        /// <param name="sphere">Сфера.</param>
        /// <returns>Статус попадания.</returns>
        public static bool PointSphere(in Vector3Df point, in Sphere3Df sphere)
        {
            return PointSphere(in point, in sphere.Center, sphere.Radius);
        }

        /// <summary>
        /// Проверка на попадание точки в область сферы.
        /// </summary>
        /// <param name="point">Проверяемая точка.</param>
        /// <param name="sphereCenter">Центр сферы.</param>
        /// <param name="sphereRadius">Радиус сферы.</param>
        /// <returns>Статус попадания.</returns>
        public static bool PointSphere(in Vector3Df point, in Vector3Df sphereCenter, float sphereRadius)
        {
            // For points on the sphere's surface magnitude is more stable than SqrLength
            return (point - sphereCenter).Length < (sphereRadius * sphereRadius) + XGeometry3D.Eplsilon_f;
        }
        #endregion

        #region Line - Line 
        /// <summary>
        /// Проверка пересечения двух линий.
        /// </summary>
        /// <param name="lineA">Первая линия.</param>
        /// <param name="lineB">Вторая линия.</param>
        /// <returns>Статус пересечения линий.</returns>
        public static bool LineLine(in Line3Df lineA, in Line3Df lineB)
        {
            return LineLine(in lineA.Position, in lineA.Direction, in lineB.Position, in lineB.Direction, out _);
        }

        /// <summary>
        /// Проверка пересечения двух линий.
        /// </summary>
        /// <param name="lineA">Первая линия.</param>
        /// <param name="lineB">Вторая линия.</param>
        /// <param name="hit">Точка пересечения линий если они пересекаются.</param>
        /// <returns>Статус пересечения линий.</returns>
        public static bool LineLine(in Line3Df lineA, in Line3Df lineB, out Vector3Df hit)
        {
            return LineLine(in lineA.Position, in lineA.Direction, in lineB.Position, in lineB.Direction, out hit);
        }

        /// <summary>
        /// Проверка пересечения двух линий.
        /// </summary>
        /// <param name="posA">Позиция первой линии.</param>
        /// <param name="dirA">Направление первой линии.</param>
        /// <param name="posB">Позиция второй линии.</param>
        /// <param name="dirB">Направление второй линии.</param>
        /// <returns>Статус пересечения линий.</returns>
        public static bool LineLine(in Vector3Df posA, in Vector3Df dirA, in Vector3Df posB, in Vector3Df dirB)
        {
            return LineLine(in posA, in dirA, in posB, in dirB, out _);
        }

        /// <summary>
        /// Проверка пересечения двух линий.
        /// </summary>
        /// <param name="posA">Позиция первой линии.</param>
        /// <param name="dirA">Направление первой линии.</param>
        /// <param name="posB">Позиция второй линии.</param>
        /// <param name="dirB">Направление второй линии.</param>
        /// <param name="hit">Точка пересечения линий если они пересекаются.</param>
        /// <returns>Статус пересечения линий.</returns>
        public static bool LineLine(in Vector3Df posA, in Vector3Df dirA, in Vector3Df posB, in Vector3Df dirB,
            out Vector3Df hit)
        {
            var sqr_length_a = dirA.SqrLength;
            var sqr_length_b = dirB.SqrLength;
            var dot_a_b = Vector3Df.Dot(in dirA, in dirB);

            var denominator = (sqr_length_a * sqr_length_b) - (dot_a_b * dot_a_b);
            var pos_b_to_a = posA - posB;
            var a = Vector3Df.Dot(in dirA, in pos_b_to_a);
            var b = Vector3Df.Dot(in dirB, in pos_b_to_a);

            Vector3Df closest_point_a;
            Vector3Df closest_point_b;
            if (Math.Abs(denominator) < XGeometry3D.Eplsilon_f)
            {
                // Parallel
                var distance_b = dot_a_b > sqr_length_b ? a / dot_a_b : b / sqr_length_b;

                closest_point_a = posA;
                closest_point_b = posB + (dirB * distance_b);
            }
            else
            {
                // Not parallel
                var distance_a = ((sqr_length_a * b) - (dot_a_b * a)) / denominator;
                var distance_b = ((dot_a_b * b) - (sqr_length_b * a)) / denominator;

                closest_point_a = posA + (dirA * distance_a);
                closest_point_b = posB + (dirB * distance_b);
            }

            if ((closest_point_b - closest_point_a).SqrLength < XGeometry3D.Eplsilon_f)
            {
                hit = closest_point_a;
                return true;
            }
            hit = Vector3Df.Zero;
            return false;
        }
        #endregion

        #region Line - Sphere 
        /// <summary>
        /// Проверка на пересечения линии и сферы.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="sphere">Сфера.</param>
        /// <returns>Статус пересечения.</returns>
        public static bool LineSphere(in Line3Df line, in Sphere3Df sphere)
        {
            return LineSphere(in line.Position, in line.Direction, in sphere.Center, sphere.Radius, out _);
        }

        /// <summary>
        /// Проверка на пересечения линии и сферы.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="sphere">Сфера.</param>
        /// <param name="hit">Информация о пересечении.</param>
        /// <returns>Статус пересечения.</returns>
        public static bool LineSphere(in Line3Df line, in Sphere3Df sphere, out TIntersectHit3Df hit)
        {
            return LineSphere(in line.Position, in line.Direction, in sphere.Center, sphere.Radius, out hit);
        }

        /// <summary>
        /// Проверка на пересечения линии и сферы.
        /// </summary>
        /// <param name="linePos">Позиция линии.</param>
        /// <param name="lineDir">Направление линии.</param>
        /// <param name="sphereCenter">Центр сферы.</param>
        /// <param name="sphereRadius">Радиус сферы.</param>
        /// <returns>Статус пересечения.</returns>
        public static bool LineSphere(in Vector3Df linePos, in Vector3Df lineDir, in Vector3Df sphereCenter, float sphereRadius)
        {
            return LineSphere(in linePos, in lineDir, in sphereCenter, sphereRadius, out _);
        }

        /// <summary>
        /// Проверка на пересечения линии и сферы.
        /// </summary>
        /// <param name="linePos">Позиция линии.</param>
        /// <param name="lineDir">Направление линии.</param>
        /// <param name="sphereCenter">Центр сферы.</param>
        /// <param name="sphereRadius">Радиус сферы.</param>
        /// <param name="hit">Информация о пересечении.</param>
        /// <returns>Статус пересечения.</returns>
        public static bool LineSphere(in Vector3Df linePos, in Vector3Df lineDir, in Vector3Df sphereCenter, float sphereRadius,
            out TIntersectHit3Df hit)
        {
            var pos_to_center = sphereCenter - linePos;
            var center_projection = Vector3Df.Dot(in lineDir, in pos_to_center);
            var sqr_distance_to_line = pos_to_center.SqrLength - (center_projection * center_projection);

            var sqr_distance_to_intersection = (sphereRadius * sphereRadius) - sqr_distance_to_line;
            if (sqr_distance_to_intersection < -XGeometry3D.Eplsilon_f)
            {
                hit = TIntersectHit3Df.None();
                return false;
            }
            if (sqr_distance_to_intersection < XGeometry3D.Eplsilon_f)
            {
                hit = TIntersectHit3Df.Point(linePos + (lineDir * center_projection));
                return true;
            }

            var distance_to_intersection = XMath.Sqrt(sqr_distance_to_intersection);
            var distance_a = center_projection - distance_to_intersection;
            var distance_b = center_projection + distance_to_intersection;

            var point_a = linePos + (lineDir * distance_a);
            var point_b = linePos + (lineDir * distance_b);
            hit = TIntersectHit3Df.Segment(point_a, point_b);
            return true;
        }
        #endregion Line-Sphere

        #region Ray - Ray 
        /// <summary>
        /// Проверка на пересечении/совпадения лучей.
        /// </summary>
        /// <param name="ray1">Первый луч.</param>
        /// <param name="ray2">Второй луч.</param>
        /// <param name="point">Точка пересечения лучей, если они пересекаются.</param>
        /// <returns>Тип пересечения.</returns>
        public static TIntersectType3D RayToRay(in Ray3D ray1, in Ray3D ray2, out Vector3D point)
        {
            point = Vector3D.Zero;
            return TIntersectType3D.None;
        }
        #endregion

        #region Ray - Plane 
        /// <summary>
        /// Проверка на пересечение луча и плоскости.
        /// </summary>
        /// <param name="ray">Луч.</param>
        /// <param name="plane">Плоскость.</param>
        /// <param name="point">Точка пересечения, если они пересекаются.</param>
        /// <returns>Тип пересечения.</returns>
        public static TIntersectType3D RayToPlane(in Ray3D ray, in Plane3D plane, out Vector3D point)
        {
            var dot = Vector3D.Dot(in ray.Direction, in plane.Normal);
            var distance = plane.GetDistanceToPoint(in ray.Position);

            // Не должно быть равным нулю
            if (XMath.Approximately(dot, 0) == false)
            {
                // Направление луча не параллельно плоскости. Пересечение есть
                // Точка пересечения
                point = ray.Position + (ray.Direction * (distance / dot));

                return TIntersectType3D.Point;
            }

            point = Vector3D.Zero;

            // А может на луч лежит плоскости
            if (XMath.Approximately(distance, XGeometry3D.Eplsilon_d))
            {
                return TIntersectType3D.Line;
            }

            return TIntersectType3D.None;
        }
        #endregion

        #region Ray - Sphere 
        /// <summary>
        /// Проверка на пересечение луча и сферы. Используется аналитическое решение.
        /// </summary>
        /// <param name="ray">Луч.</param>
        /// <param name="position">Позиция центра сферы.</param>
        /// <param name="radius">Радиус сферы.</param>
        /// <param name="point">Точка пересечения, если они пересекаются.</param>
        /// <returns>Тип пересечения.</returns>
        public static TIntersectType3D RayToSphere(in Ray3D ray, in Vector3D position, float radius, out Vector3D point)
        {
            point = Vector3D.Zero;

            var l = ray.Direction.X;
            var m = ray.Direction.Y;
            var n = ray.Direction.Z;

            var dx = ray.Position.X - position.X;
            var dy = ray.Position.Y - position.Y;
            var dz = ray.Position.Z - position.Z;

            var a = (l * l) + (m * m) + (n * n);
            var b = (2 * dx * l) + (2 * dy * m) + (2 * dz * n);
            var c = (dx * dx) + (dy * dy * dz * dz) - (radius * radius);

            var result = XMathSolverEquations.SolveQuadraticEquation(a, b, c, out var t1, out var t2);
            if (result == -1)
            {
                return TIntersectType3D.None;
            }
            else
            {
                if (result == 0)
                {
                    point = ray.GetPoint(t1);
                    return TIntersectType3D.Point;
                }
                else
                {
                    double value = 0;
                    if (t1 > 0)
                    {
                        if (t2 < 0)
                        {
                            value = t1;
                        }
                        else
                        {
                            if (t1 < t2)
                            {
                                value = t1;
                            }
                            else
                            {
                                value = t2;
                            }
                        }
                    }
                    if (t2 > 0)
                    {
                        if (t1 < 0)
                        {
                            value = t2;
                        }
                        else
                        {
                            if (t2 < t1)
                            {
                                value = t2;
                            }
                            else
                            {
                                value = t1;
                            }
                        }
                    }
                    point = ray.GetPoint(value);
                    return TIntersectType3D.Point;
                }
            }
        }

        /// <summary>
        /// Проверка на пересечения луча и сферы.
        /// </summary>
        /// <param name="ray">Луч.</param>
        /// <param name="sphere">Сфера.</param>
        /// <returns>Статус пересечения.</returns>
        public static bool RaySphere(in Ray3Df ray, in Sphere3Df sphere)
        {
            return RaySphere(in ray.Position, in ray.Direction, in sphere.Center, sphere.Radius, out _);
        }

        /// <summary>
        /// Проверка на пересечения луча и сферы.
        /// </summary>
        /// <param name="ray">Луч.</param>
        /// <param name="sphere">Сфера.</param>
        /// <param name="hit">Информация о пересечении.</param>
        /// <returns>Статус пересечения.</returns>
        public static bool RaySphere(in Ray3Df ray, in Sphere3Df sphere, out TIntersectHit3Df hit)
        {
            return RaySphere(in ray.Position, in ray.Direction, in sphere.Center, sphere.Radius, out hit);
        }

        /// <summary>
        /// Проверка на пересечения луча и сферы.
        /// </summary>
        /// <param name="rayPos">Позиция луча.</param>
        /// <param name="rayDir">Направление луча.</param>
        /// <param name="sphereCenter">Центр сферы.</param>
        /// <param name="sphereRadius">Радиус сферы.</param>
        /// <returns>Статус пересечения.</returns>
        public static bool RaySphere(in Vector3Df rayPos, in Vector3Df rayDir, in Vector3Df sphereCenter, float sphereRadius)
        {
            return RaySphere(in rayPos, in rayDir, in sphereCenter, sphereRadius, out _);
        }

        /// <summary>
        /// Проверка на пересечения луча и сферы.
        /// </summary>
        /// <param name="rayPos">Позиция луча.</param>
        /// <param name="rayDir">Направление луча.</param>
        /// <param name="sphereCenter">Центр сферы.</param>
        /// <param name="sphereRadius">Радиус сферы.</param>
        /// <param name="hit">Информация о пересечении.</param>
        /// <returns>Статус пересечения.</returns>
        public static bool RaySphere(in Vector3Df rayPos, in Vector3Df rayDir, in Vector3Df sphereCenter, float sphereRadius,
            out TIntersectHit3Df hit)
        {
            var pos_to_center = sphereCenter - rayPos;
            var center_projection = Vector3Df.Dot(in rayDir, in pos_to_center);
            if (center_projection + sphereRadius < -XGeometry3D.Eplsilon_f)
            {
                hit = TIntersectHit3Df.None();
                return false;
            }

            var sqr_distance_to_line = pos_to_center.SqrLength - (center_projection * center_projection);
            var sqr_distance_to_intersection = (sphereRadius * sphereRadius) - sqr_distance_to_line;
            if (sqr_distance_to_intersection < -XGeometry3D.Eplsilon_f)
            {
                hit = TIntersectHit3Df.None();
                return false;
            }
            if (sqr_distance_to_intersection < XGeometry3D.Eplsilon_f)
            {
                if (center_projection < -XGeometry3D.Eplsilon_f)
                {
                    hit = TIntersectHit3Df.None();
                    return false;
                }
                hit = TIntersectHit3Df.Point(rayPos + (rayDir * center_projection));
                return true;
            }

            // Line hit
            var distance_to_intersection = XMath.Sqrt(sqr_distance_to_intersection);
            var distance_a = center_projection - distance_to_intersection;
            var distance_b = center_projection + distance_to_intersection;

            if (distance_a < -XGeometry3D.Eplsilon_f)
            {
                if (distance_b < -XGeometry3D.Eplsilon_f)
                {
                    hit = TIntersectHit3Df.None();
                    return false;
                }
                hit = TIntersectHit3Df.Point(rayPos + (rayDir * distance_b));
                return true;
            }

            var point_a = rayPos + (rayDir * distance_a);
            var point_b = rayPos + (rayDir * distance_b);
            hit = TIntersectHit3Df.Segment(point_a, point_b);
            return true;
        }
        #endregion

        #region Ray - Triangle 
        /// <summary>
        /// Проверка на пересечение луча и треугольника.
        /// </summary>
        /// <param name="ray">Луч.</param>
        /// <param name="p1">Первая вершина треугольника.</param>
        /// <param name="p2">Вторая вершина треугольника.</param>
        /// <param name="p3">Третья вершина треугольника.</param>
        /// <param name="rayHit">Информация о пересечении.</param>
        /// <returns>Тип пересечения.</returns>
        public static TIntersectType3D RayToTriangle(in Ray3D ray, in Vector3D p1, in Vector3D p2, Vector3D p3,
            out TIntersectHit3D rayHit)
        {
            rayHit = new TIntersectHit3D();

            // Find vectors for two edges sharing vert0
            var edge1 = p2 - p1;
            var edge2 = p3 - p1;

            // Begin calculating determinant - also used to calculate U parameter
            var pvec = Vector3D.Cross(in ray.Direction, in edge2);

            // If determinant is near zero, ray lies in plane of triangle
            var det = Vector3D.Dot(in edge1, in pvec);

            Vector3D tvec;
            if (det > 0)
            {
                tvec = ray.Position - p1;
            }
            else
            {
                tvec = p1 - ray.Position;
                det = -det;
            }

            if (det < XGeometry3D.Eplsilon_d)
            {
                return TIntersectType3D.None;
            }

            // Calculate U parameter and test bounds
            var u = Vector3D.Dot(in tvec, in pvec);
            if (u < 0.0 || u > det)
            {
                return TIntersectType3D.None;
            }

            // Prepare to test V parameter
            var qvec = Vector3D.Cross(in tvec, in edge1);

            // Calculate V parameter and test bounds
            var v = Vector3D.Dot(in ray.Direction, in qvec);
            if (v < 0.0 || u + v > det)
            {
                return TIntersectType3D.None;
            }

            // Calculate t, scale parameters, ray intersects triangle
            var t = Vector3D.Dot(in edge2, in qvec);
            var invert_t = 1.0 / det;
            t *= invert_t;
            u *= invert_t;
            v *= invert_t;

            // Сохраняем данные
            rayHit.IntersectType = TIntersectType3D.Point;
            rayHit.Distance = t;
            rayHit.BarycentricCoordinate = new Vector3D(u, v, 0.0);
            rayHit.Point1 = ray.GetPoint(t);

            return TIntersectType3D.Point;
        }
        #endregion

        #region Segment - Sphere 
        /// <summary>
        /// Проверка на пересечения отрезка и сферы.
        /// </summary>
        /// <param name="segment">Отрезок.</param>
        /// <param name="sphere">Сфера.</param>
        /// <returns>Статус пересечения.</returns>
        public static bool SegmentSphere(in Segment3Df segment, in Sphere3Df sphere)
        {
            return SegmentSphere(in segment.Start, in segment.End, in sphere.Center, sphere.Radius, out _);
        }

        /// <summary>
        /// Проверка на пересечения отрезка и сферы.
        /// </summary>
        /// <param name="segment">Отрезок.</param>
        /// <param name="sphere">Сфера.</param>
        /// <param name="hit">Информация о пересечении.</param>
        /// <returns>Статус пересечения.</returns>
        public static bool SegmentSphere(Segment3Df segment, Sphere3Df sphere, out TIntersectHit3Df hit)
        {
            return SegmentSphere(in segment.Start, in segment.End, in sphere.Center, sphere.Radius, out hit);
        }

        /// <summary>
        /// Проверка на пересечения отрезка и сферы.
        /// </summary>
        /// <param name="start">Начало отрезка.</param>
        /// <param name="end">Конец отрезка.</param>
        /// <param name="sphereCenter">Центр сферы.</param>
        /// <param name="sphereRadius">Радиус сферы.</param>
        /// <returns>Статус пересечения.</returns>
        public static bool SegmentSphere(in Vector3Df start, in Vector3Df end, in Vector3Df sphereCenter, float sphereRadius)
        {
            return SegmentSphere(in start, in end, in sphereCenter, sphereRadius, out _);
        }

        /// <summary>
        /// Проверка на пересечения отрезка и сферы.
        /// </summary>
        /// <param name="start">Начало отрезка.</param>
        /// <param name="end">Конец отрезка.</param>
        /// <param name="sphereCenter">Центр сферы.</param>
        /// <param name="sphereRadius">Радиус сферы.</param>
        /// <param name="hit">Информация о пересечении.</param>
        /// <returns>Статус пересечения.</returns>
        public static bool SegmentSphere(in Vector3Df start, in Vector3Df end, in Vector3Df sphereCenter, float sphereRadius,
            out TIntersectHit3Df hit)
        {
            var segment_start_to_center = sphereCenter - start;
            var from_start_to_end = end - start;
            var segment_length = from_start_to_end.Length;
            if (segment_length < XGeometry3D.Eplsilon_f)
            {
                var distanceToPoint = segment_start_to_center.Length;
                if (distanceToPoint < sphereRadius + XGeometry3D.Eplsilon_f)
                {
                    if (distanceToPoint > sphereRadius - XGeometry3D.Eplsilon_f)
                    {
                        hit = TIntersectHit3Df.Point(start);
                        return true;
                    }
                    hit = TIntersectHit3Df.None();
                    return true;
                }
                hit = TIntersectHit3Df.None();
                return false;
            }

            var segment_direction = from_start_to_end.Normalized;
            var center_projection = Vector3Df.Dot(segment_direction, segment_start_to_center);
            if (center_projection + sphereRadius < -XGeometry3D.Eplsilon_f ||
                center_projection - sphereRadius > segment_length + XGeometry3D.Eplsilon_f)
            {
                hit = TIntersectHit3Df.None();
                return false;
            }

            var sqr_distance_to_line = segment_start_to_center.SqrLength - (center_projection * center_projection);
            var sqr_distance_to_intersection = (sphereRadius * sphereRadius) - sqr_distance_to_line;
            if (sqr_distance_to_intersection < -XGeometry3D.Eplsilon_f)
            {
                hit = TIntersectHit3Df.None();
                return false;
            }

            if (sqr_distance_to_intersection < XGeometry3D.Eplsilon_f)
            {
                if (center_projection < -XGeometry3D.Eplsilon_f ||
                    center_projection > segment_length + XGeometry3D.Eplsilon_f)
                {
                    hit = TIntersectHit3Df.None();
                    return false;
                }
                hit = TIntersectHit3Df.Point(start + (segment_direction * center_projection));
                return true;
            }

            // Line hit
            var distance_to_intersection = XMath.Sqrt(sqr_distance_to_intersection);
            var distance_a = center_projection - distance_to_intersection;
            var distance_b = center_projection + distance_to_intersection;

            var point_a_is_after_segment_start = distance_a > -XGeometry3D.Eplsilon_f;
            var point_b_is_before_segment_end = distance_b < segment_length + XGeometry3D.Eplsilon_f;

            if (point_a_is_after_segment_start && point_b_is_before_segment_end)
            {
                var point_a = start + (segment_direction * distance_a);
                var point_b = start + (segment_direction * distance_b);
                hit = TIntersectHit3Df.Segment(in point_a, in point_b);
                return true;
            }
            if (!point_a_is_after_segment_start && !point_b_is_before_segment_end)
            {
                // The segment is inside, but no hit
                hit = TIntersectHit3Df.None();
                return true;
            }

            var point_a_is_before_segment_end = distance_a < segment_length + XGeometry3D.Eplsilon_f;
            if (point_a_is_after_segment_start && point_a_is_before_segment_end)
            {
                // Point A hit
                hit = TIntersectHit3Df.Point(start + (segment_direction * distance_a));
                return true;
            }
            var point_b_is_after_segment_start = distance_b > -XGeometry3D.Eplsilon_f;
            if (point_b_is_after_segment_start && point_b_is_before_segment_end)
            {
                // Point B hit
                hit = TIntersectHit3Df.Point(start + (segment_direction * distance_b));
                return true;
            }

            hit = TIntersectHit3Df.None();
            return false;
        }
        #endregion
    }
    /**@}*/
}