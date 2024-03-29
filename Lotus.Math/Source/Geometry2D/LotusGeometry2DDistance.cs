using System;

namespace Lotus.Maths
{
    /** \addtogroup MathGeometry2D
	*@{*/
    /// <summary>
    /// Статический класс реализующий методы вычисление дистанции между основными геометрическими телами/примитивами.
    /// </summary>
    public static class XDistance2D
    {
        #region Point - Line 
        /// <summary>
        /// Вычисление расстояния между линией и ближайшей точки.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="line">Линия.</param>
        /// <returns>Расстояние.</returns>
        public static float PointLine(in Vector2Df point, in Line2Df line)
        {
            return Vector2Df.Distance(in point, XClosest2D.PointLine(in point, in line));
        }

        /// <summary>
        /// Вычисление расстояния между линией и ближайшей точки.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="linePos">Позиция линии.</param>
        /// <param name="lineDir">Направление линии.</param>
        /// <returns>Расстояние.</returns>
        public static float PointLine(in Vector2Df point, in Vector2Df linePos, in Vector2Df lineDir)
        {
            return Vector2Df.Distance(point, XClosest2D.PointLine(point, linePos, lineDir));
        }
        #endregion

        #region Point - Ray 
        /// <summary>
        /// Вычисление расстояние до самой близкой точки на луче.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="ray">Луч.</param>
        /// <returns>Расстояние.</returns>
        public static float PointRay(in Vector2Df point, in Ray2Df ray)
        {
            return Vector2Df.Distance(in point, XClosest2D.PointRay(in point, in ray));
        }

        /// <summary>
        /// Вычисление расстояние до самой близкой точки на луче.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="rayPos">Позиция луча.</param>
        /// <param name="rayDir">Направление луча.</param>
        /// <returns>Расстояние.</returns>
        public static float PointRay(in Vector2Df point, in Vector2Df rayPos, in Vector2Df rayDir)
        {
            return Vector2Df.Distance(in point, XClosest2D.PointRay(in point, in rayPos, in rayDir));
        }
        #endregion

        #region Point - Segment 
        /// <summary>
        /// Вычисление расстояние до самой близкой точки на отрезке.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="segment">Отрезок.</param>
        /// <returns>Расстояние.</returns>
        public static float PointSegment(in Vector2Df point, in Segment2Df segment)
        {
            return Vector2Df.Distance(in point, XClosest2D.PointSegment(in point, in segment));
        }

        /// <summary>
        /// Вычисление расстояние до самой близкой точки на отрезке.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="start">Начало отрезка.</param>
        /// <param name="end">Конец отрезка.</param>
        /// <returns>Расстояние.</returns>
        public static float PointSegment(in Vector2Df point, in Vector2Df start, in Vector2Df end)
        {
            return Vector2Df.Distance(in point, XClosest2D.PointSegment(in point, in start, in end));
        }

        /// <summary>
        /// Вычисление расстояние до самой близкой точки на отрезке.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="start">Начало отрезка.</param>
        /// <param name="end">Конец отрезка.</param>
        /// <param name="segment_direction">Направление отрезка.</param>
        /// <param name="segment_length">Длина отрезка.</param>
        /// <returns>Расстояние.</returns>
        private static float PointSegment(in Vector2Df point, in Vector2Df start, in Vector2Df end,
            in Vector2Df segment_direction, float segment_length)
        {
            var point_projection = Vector2Df.Dot(in segment_direction, point - start);
            if (point_projection < -XGeometry2D.Eplsilon_f)
            {
                return Vector2Df.Distance(in point, in start);
            }
            if (point_projection > segment_length + XGeometry2D.Eplsilon_f)
            {
                return Vector2Df.Distance(in point, in end);
            }
            return Vector2Df.Distance(in point, start + (segment_direction * point_projection));
        }
        #endregion

        #region Point - Circle 
        /// <summary>
        /// Вычисление расстояние до самой близкой точки на окружности.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="circle">Окружность.</param>
        /// <returns>Расстояние.</returns>
        public static float PointCircle(in Vector2Df point, in Circle2Df circle)
        {
            return PointCircle(in point, in circle.Center, circle.Radius);
        }

        /// <summary>
        /// Вычисление расстояние до самой близкой точки на окружности.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="circleCenter">Центр окружности.</param>
        /// <param name="circleRadius">Радиус окружности.</param>
        /// <returns>Расстояние.</returns>
        public static float PointCircle(in Vector2Df point, in Vector2Df circleCenter, float circleRadius)
        {
            return (circleCenter - point).Length - circleRadius;
        }
        #endregion

        #region Line - Line 
        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на линиях.
        /// </summary>
        /// <param name="lineA">Первая линия.</param>
        /// <param name="lineB">Вторая линия.</param>
        /// <returns>Расстояние.</returns>
        public static float LineLine(in Line2Df lineA, in Line2Df lineB)
        {
            return LineLine(lineA.Position, lineA.Direction, lineB.Position, lineB.Direction);
        }

        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на линиях.
        /// </summary>
        /// <param name="posA">Позиция первой линии.</param>
        /// <param name="dirA">Направление первой линии.</param>
        /// <param name="posB">Позиция второй линии.</param>
        /// <param name="dirB">Направление второй линии.</param>
        /// <returns>Расстояние.</returns>
        public static float LineLine(in Vector2Df posA, in Vector2Df dirA, in Vector2Df posB,
            in Vector2Df dirB)
        {
            if (Math.Abs(Vector2Df.DotPerp(in dirA, in dirB)) < XGeometry2D.Eplsilon_f)
            {
                // Parallel
                var pos_b_to_a = posA - posB;
                if (Math.Abs(Vector2Df.DotPerp(in dirA, in pos_b_to_a)) > XGeometry2D.Eplsilon_f ||
                    Math.Abs(Vector2Df.DotPerp(in dirB, in pos_b_to_a)) > XGeometry2D.Eplsilon_f)
                {
                    // Not collinear
                    var origin_b_projection = Vector2Df.Dot(in dirA, in pos_b_to_a);
                    var distance_sqr = pos_b_to_a.SqrLength - (origin_b_projection * origin_b_projection);

                    // distanceSqr can be negative
                    return distance_sqr <= 0 ? 0 : XMath.Sqrt(distance_sqr);
                }

                // Collinear
                return 0;
            }

            // Not parallel
            return 0;
        }
        #endregion

        #region Line - Ray 
        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на линии и луче.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="ray">Луч.</param>
        /// <returns>Расстояние.</returns>
        public static float LineRay(in Line2Df line, in Ray2Df ray)
        {
            return LineRay(in line.Position, in line.Direction, in ray.Position, in ray.Direction);
        }

        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на линии и луче.
        /// </summary>
        /// <param name="linePos">Позиция линии.</param>
        /// <param name="lineDir">Направление линии.</param>
        /// <param name="rayPos">Позиция луча.</param>
        /// <param name="rayDir">Направление луча.</param>
        /// <returns>Расстояние.</returns>
        public static float LineRay(in Vector2Df linePos, in Vector2Df lineDir, in Vector2Df rayPos, in Vector2Df rayDir)
        {
            var ray_pos_to_line_pos = linePos - rayPos;
            var denominator = Vector2Df.DotPerp(in lineDir, in rayDir);
            var perp_dot_a = Vector2Df.DotPerp(in lineDir, in ray_pos_to_line_pos);

            if (Math.Abs(denominator) < XGeometry2D.Eplsilon_f)
            {
                // Parallel
                var perp_dot_b = Vector2Df.DotPerp(in rayDir, in ray_pos_to_line_pos);
                if (Math.Abs(perp_dot_a) > XGeometry2D.Eplsilon_f || Math.Abs(perp_dot_b) > XGeometry2D.Eplsilon_f)
                {
                    // Not collinear
                    var ray_pos_projection = Vector2Df.Dot(in lineDir, in ray_pos_to_line_pos);
                    var distanceSqr = ray_pos_to_line_pos.SqrLength - (ray_pos_projection * ray_pos_projection);
                    // distanceSqr can be negative
                    return distanceSqr <= 0 ? 0 : XMath.Sqrt(distanceSqr);
                }
                // Collinear
                return 0;
            }

            // Not parallel
            var ray_distance = perp_dot_a / denominator;
            if (ray_distance < -XGeometry2D.Eplsilon_f)
            {
                // No intersection
                var ray_pos_projection = Vector2Df.Dot(in lineDir, in ray_pos_to_line_pos);
                var line_point = linePos - (lineDir * ray_pos_projection);
                return Vector2Df.Distance(in line_point, in rayPos);
            }
            // Point intersection
            return 0;
        }
        #endregion

        #region Line - Segment 
        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на линии и отрезке.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="segment">Отрезок.</param>
        /// <returns>Расстояние.</returns>
        public static float LineSegment(in Line2Df line, in Segment2Df segment)
        {
            return LineSegment(in line.Position, in line.Direction, in segment.Start, in segment.End);
        }

        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на линии и отрезке.
        /// </summary>
        /// <param name="linePos">Позиция линии.</param>
        /// <param name="lineDir">Направление линии.</param>
        /// <param name="start">Начало отрезка.</param>
        /// <param name="end">Конец отрезка.</param>
        /// <returns>Расстояние.</returns>
        public static float LineSegment(in Vector2Df linePos, in Vector2Df lineDir, in Vector2Df start, in Vector2Df end)
        {
            var segment_start_to_pos = linePos - start;
            var segment_direction = end - start;
            var denominator = Vector2Df.DotPerp(in lineDir, in segment_direction);
            var perp_dot_a = Vector2Df.DotPerp(in lineDir, in segment_start_to_pos);

            if (Math.Abs(denominator) < XGeometry2D.Eplsilon_f)
            {
                // Parallel
                // Normalized Direction gives more stable results 
                var perp_dot_b = Vector2Df.DotPerp(segment_direction.Normalized, segment_start_to_pos);
                if (Math.Abs(perp_dot_a) > XGeometry2D.Eplsilon_f || Math.Abs(perp_dot_b) > XGeometry2D.Eplsilon_f)
                {
                    // Not collinear
                    var segment_start_projection = Vector2Df.Dot(in lineDir, in segment_start_to_pos);
                    var distanceSqr = segment_start_to_pos.SqrLength - (segment_start_projection * segment_start_projection);
                    // distanceSqr can be negative
                    return distanceSqr <= 0 ? 0 : XMath.Sqrt(distanceSqr);
                }
                // Collinear
                return 0;
            }

            // Not parallel
            var segment_distance = perp_dot_a / denominator;
            if (segment_distance < -XGeometry2D.Eplsilon_f || segment_distance > 1 + XGeometry2D.Eplsilon_f)
            {
                // No intersection
                var segment_point = start + (segment_direction * XMath.Clamp01(segment_distance));
                var segmentPointProjection = Vector2Df.Dot(lineDir, segment_point - linePos);
                var line_point = linePos + (lineDir * segmentPointProjection);
                return Vector2Df.Distance(in line_point, in segment_point);
            }
            // Point intersection
            return 0;
        }
        #endregion

        #region Line - Circle 
        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на линии и окружности.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="circle">Окружность.</param>
        /// <returns>Расстояние.</returns>
        public static float LineCircle(in Line2Df line, in Circle2Df circle)
        {
            return LineCircle(in line.Position, in line.Direction, in circle.Center, circle.Radius);
        }

        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на линии и окружности.
        /// </summary>
        /// <param name="linePos">Позиция линии.</param>
        /// <param name="lineDir">Направление линии.</param>
        /// <param name="circleCenter">Центр окружности.</param>
        /// <param name="circleRadius">Радиус окружности.</param>
        /// <returns>Расстояние.</returns>
        public static float LineCircle(in Vector2Df linePos, in Vector2Df lineDir,
            in Vector2Df circleCenter, float circleRadius)
        {
            var pos_to_center = circleCenter - linePos;
            var center_projection = Vector2Df.Dot(in lineDir, in pos_to_center);
            var sqr_distance_to_line = pos_to_center.SqrLength - (center_projection * center_projection);
            var sqr_distance_to_intersection = (circleRadius * circleRadius) - sqr_distance_to_line;
            if (sqr_distance_to_intersection < -XGeometry2D.Eplsilon_f)
            {
                // No intersection
                return XMath.Sqrt(sqr_distance_to_line) - circleRadius;
            }
            return 0;
        }
        #endregion

        #region Ray - Ray 
        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на лучах.
        /// </summary>
        /// <param name="rayA">Первый луч.</param>
        /// <param name="rayB">Второй луч.</param>
        /// <returns>Расстояние.</returns>
        public static float RayRay(in Ray2Df rayA, in Ray2Df rayB)
        {
            return RayRay(in rayA.Position, in rayA.Direction, in rayB.Position, in rayB.Direction);
        }

        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на лучах.
        /// </summary>
        /// <param name="posA">Позиция первого луча.</param>
        /// <param name="dirA">Направление первого луча.</param>
        /// <param name="posB">Позиция второго луча.</param>
        /// <param name="dirB">Направление второго луча.</param>
        /// <returns>Расстояние.</returns>
        public static float RayRay(in Vector2Df posA, in Vector2Df dirA, in Vector2Df posB,
            in Vector2Df dirB)
        {
            var pos_b_to_a = posA - posB;
            var denominator = Vector2Df.DotPerp(in dirA, in dirB);
            var perp_dot_a = Vector2Df.DotPerp(in dirA, in pos_b_to_a);
            var perp_dot_b = Vector2Df.DotPerp(in dirB, in pos_b_to_a);

            var codirected = Vector2Df.Dot(in dirA, in dirB) > 0;
            if (Math.Abs(denominator) < XGeometry2D.Eplsilon_f)
            {
                // Parallel
                var pos_b_projection = -Vector2Df.Dot(dirA, pos_b_to_a);
                if (Math.Abs(perp_dot_a) > XGeometry2D.Eplsilon_f || Math.Abs(perp_dot_b) > XGeometry2D.Eplsilon_f)
                {
                    // Not collinear
                    if (!codirected && pos_b_projection < XGeometry2D.Eplsilon_f)
                    {
                        return Vector2Df.Distance(posA, posB);
                    }
                    var distanceSqr = pos_b_to_a.SqrLength - (pos_b_projection * pos_b_projection);
                    // distanceSqr can be negative
                    return distanceSqr <= 0 ? 0 : XMath.Sqrt(distanceSqr);
                }
                // Collinear

                if (codirected)
                {
                    // Ray intersection
                    return 0;
                }
                else
                {
                    if (pos_b_projection < XGeometry2D.Eplsilon_f)
                    {
                        // No intersection
                        return Vector2Df.Distance(in posA, in posB);
                    }
                    else
                    {
                        // Segment intersection
                        return 0;
                    }
                }
            }

            // Not parallel
            var distance_a = perp_dot_b / denominator;
            var distance_b = perp_dot_a / denominator;
            if (distance_a < -XGeometry2D.Eplsilon_f || distance_b < -XGeometry2D.Eplsilon_f)
            {
                // No intersection
                if (codirected)
                {
                    var pos_a_projection = Vector2Df.Dot(dirB, pos_b_to_a);
                    if (pos_a_projection > -XGeometry2D.Eplsilon_f)
                    {
                        var ray_point_a = posA;
                        var ray_point_b = posB + (dirB * pos_a_projection);
                        return Vector2Df.Distance(in ray_point_a, in ray_point_b);
                    }
                    var pos_b_projection = -Vector2Df.Dot(dirA, pos_b_to_a);
                    if (pos_b_projection > -XGeometry2D.Eplsilon_f)
                    {
                        var ray_point_a = posA + (dirA * pos_b_projection);
                        var ray_point_b = posB;
                        return Vector2Df.Distance(in ray_point_a, in ray_point_b);
                    }
                    return Vector2Df.Distance(in posA, in posB);
                }
                else
                {
                    if (distance_a > -XGeometry2D.Eplsilon_f)
                    {
                        var pos_b_projection = -Vector2Df.Dot(dirA, pos_b_to_a);
                        if (pos_b_projection > -XGeometry2D.Eplsilon_f)
                        {
                            var ray_point_a = posA + (dirA * pos_b_projection);
                            var ray_point_b = posB;
                            return Vector2Df.Distance(in ray_point_a, in ray_point_b);
                        }
                    }
                    else if (distance_b > -XGeometry2D.Eplsilon_f)
                    {
                        var pos_a_projection = Vector2Df.Dot(in dirB, in pos_b_to_a);
                        if (pos_a_projection > -XGeometry2D.Eplsilon_f)
                        {
                            var ray_point_a = posA;
                            var ray_point_b = posB + (dirB * pos_a_projection);
                            return Vector2Df.Distance(in ray_point_a, in ray_point_b);
                        }
                    }
                    return Vector2Df.Distance(in posA, in posB);
                }
            }
            // Point intersection
            return 0;
        }
        #endregion

        #region Ray - Segment 
        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на луче и сегменте.
        /// </summary>
        /// <param name="ray">Луч.</param>
        /// <param name="segment">Отрезок.</param>
        /// <returns>Расстояние.</returns>
        public static float RaySegment(in Ray2Df ray, in Segment2Df segment)
        {
            return RaySegment(in ray.Position, in ray.Direction, in segment.Start, in segment.End);
        }

        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на луче и сегменте.
        /// </summary>
        /// <param name="rayPos">Позиция луча.</param>
        /// <param name="rayDir">Направление луча.</param>
        /// <param name="start">Начало отрезка.</param>
        /// <param name="end">Конец отрезка.</param>
        /// <returns>Расстояние.</returns>
        public static float RaySegment(in Vector2Df rayPos, in Vector2Df rayDir, in Vector2Df start, in Vector2Df end)
        {
            var start_copy = start;
            var end_copy = end;
            var segment_start_to_pos = rayPos - start_copy;
            var segment_direction = end_copy - start_copy;
            var denominator = Vector2Df.DotPerp(in rayDir, in segment_direction);
            var perp_dot_a = Vector2Df.DotPerp(in rayDir, in segment_start_to_pos);

            // Normalized Direction gives more stable results 
            var perp_dot_b = Vector2Df.DotPerp(segment_direction.Normalized, in segment_start_to_pos);

            if (Math.Abs(denominator) < XGeometry2D.Eplsilon_f)
            {
                // Parallel
                var segment_start_projection = -Vector2Df.Dot(in rayDir, in segment_start_to_pos);
                var originToSegmentB = end_copy - rayPos;
                var segment_end_projection = Vector2Df.Dot(in rayDir, in originToSegmentB);
                if (Math.Abs(perp_dot_a) > XGeometry2D.Eplsilon_f || Math.Abs(perp_dot_b) > XGeometry2D.Eplsilon_f)
                {
                    // Not collinear
                    if (segment_start_projection > -XGeometry2D.Eplsilon_f)
                    {
                        var distanceSqr = segment_start_to_pos.SqrLength - (segment_start_projection * segment_start_projection);
                        // distanceSqr can be negative
                        return distanceSqr <= 0 ? 0 : XMath.Sqrt(distanceSqr);
                    }
                    if (segment_end_projection > -XGeometry2D.Eplsilon_f)
                    {
                        var distanceSqr = originToSegmentB.SqrLength - (segment_end_projection * segment_end_projection);
                        // distanceSqr can be negative
                        return distanceSqr <= 0 ? 0 : XMath.Sqrt(distanceSqr);
                    }

                    if (segment_start_projection > segment_end_projection)
                    {
                        return Vector2Df.Distance(in rayPos, in start_copy);
                    }
                    return Vector2Df.Distance(in rayPos, in end_copy);
                }
                // Collinear
                if (segment_start_projection > -XGeometry2D.Eplsilon_f || segment_end_projection > -XGeometry2D.Eplsilon_f)
                {
                    // Point or segment intersection
                    return 0;
                }
                // No intersection
                return segment_start_projection > segment_end_projection ? -segment_start_projection : -segment_end_projection;
            }

            // Not parallel
            var ray_distance = perp_dot_b / denominator;
            var segment_distance = perp_dot_a / denominator;
            if (ray_distance < -XGeometry2D.Eplsilon_f ||
                segment_distance < -XGeometry2D.Eplsilon_f || segment_distance > 1 + XGeometry2D.Eplsilon_f)
            {
                // No intersection
                var codirected = Vector2Df.Dot(in rayDir, in segment_direction) > 0;
                Vector2Df segment_end_to_pos;
                if (!codirected)
                {
                    XMath.Swap(ref start_copy, ref end_copy);
                    segment_direction = -segment_direction;
                    segment_end_to_pos = segment_start_to_pos;
                    segment_start_to_pos = rayPos - start_copy;
                    segment_distance = 1 - segment_distance;
                }
                else
                {
                    segment_end_to_pos = rayPos - end_copy;
                }

                var segment_start_projection = -Vector2Df.Dot(in rayDir, in segment_start_to_pos);
                var segment_end_projection = -Vector2Df.Dot(in rayDir, in segment_end_to_pos);
                var segment_start_on_ray = segment_start_projection > -XGeometry2D.Eplsilon_f;
                var segment_end_on_ray = segment_end_projection > -XGeometry2D.Eplsilon_f;
                if (segment_start_on_ray && segment_end_on_ray)
                {
                    if (segment_distance < 0)
                    {
                        var ray_point = rayPos + (rayDir * segment_start_projection);
                        var segment_point = start_copy;
                        return Vector2Df.Distance(in ray_point, in segment_point);
                    }
                    else
                    {
                        var ray_point = rayPos + (rayDir * segment_end_projection);
                        var segment_point = end_copy;
                        return Vector2Df.Distance(in ray_point, in segment_point);
                    }
                }
                else if (!segment_start_on_ray && segment_end_on_ray)
                {
                    if (segment_distance < 0)
                    {
                        var ray_point = rayPos;
                        var segment_point = start_copy;
                        return Vector2Df.Distance(in ray_point, in segment_point);
                    }
                    else if (segment_distance > 1 + XGeometry2D.Eplsilon_f)
                    {
                        var ray_point = rayPos + (rayDir * segment_end_projection);
                        var segment_point = end_copy;
                        return Vector2Df.Distance(in ray_point, in segment_point);
                    }
                    else
                    {
                        var ray_point = rayPos;
                        var pos_projection = Vector2Df.Dot(in segment_direction, in segment_start_to_pos);
                        var segment_point = start_copy + (segment_direction * pos_projection / segment_direction.SqrLength);
                        return Vector2Df.Distance(in ray_point, in segment_point);
                    }
                }
                else
                {
                    // Not on ray
                    var ray_point = rayPos;
                    var pos_projection = Vector2Df.Dot(in segment_direction, in segment_start_to_pos);
                    var sqr_segment_length = segment_direction.SqrLength;
                    if (pos_projection < 0)
                    {
                        return Vector2Df.Distance(in ray_point, in start_copy);
                    }
                    else if (pos_projection > sqr_segment_length)
                    {
                        return Vector2Df.Distance(in ray_point, in end_copy);
                    }
                    else
                    {
                        var segment_point = start_copy + (segment_direction * pos_projection / sqr_segment_length);
                        return Vector2Df.Distance(in ray_point, in segment_point);
                    }
                }
            }
            // Point intersection
            return 0;
        }
        #endregion

        #region Ray - Circle 
        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на луче и окружности.
        /// </summary>
        /// <param name="ray">Луч.</param>
        /// <param name="circle">Окружность.</param>
        /// <returns>Расстояние.</returns>
        public static float RayCircle(in Ray2Df ray, in Circle2Df circle)
        {
            return RayCircle(in ray.Position, in ray.Direction, in circle.Center, circle.Radius);
        }

        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на луче и окружности.
        /// </summary>
        /// <param name="rayPos">Позиция луча.</param>
        /// <param name="rayDir">Направление луча.</param>
        /// <param name="circleCenter">Центр окружности.</param>
        /// <param name="circleRadius">Радиус окружности.</param>
        /// <returns>Расстояние.</returns>
        public static float RayCircle(in Vector2Df rayPos, in Vector2Df rayDir, in Vector2Df circleCenter,
                float circleRadius)
        {
            var pos_to_center = circleCenter - rayPos;
            var center_projection = Vector2Df.Dot(in rayDir, in pos_to_center);
            if (center_projection + circleRadius < -XGeometry2D.Eplsilon_f)
            {
                // No intersection
                return XMath.Sqrt(pos_to_center.SqrLength) - circleRadius;
            }

            var sqr_distance_to_pos = pos_to_center.SqrLength;
            var sqr_distance_to_line = sqr_distance_to_pos - (center_projection * center_projection);
            var sqr_distance_to_intersection = (circleRadius * circleRadius) - sqr_distance_to_line;
            if (sqr_distance_to_intersection < -XGeometry2D.Eplsilon_f)
            {
                // No intersection
                if (center_projection < -XGeometry2D.Eplsilon_f)
                {
                    return XMath.Sqrt(sqr_distance_to_pos) - circleRadius;
                }
                return XMath.Sqrt(sqr_distance_to_line) - circleRadius;
            }
            if (sqr_distance_to_intersection < XGeometry2D.Eplsilon_f)
            {
                if (center_projection < -XGeometry2D.Eplsilon_f)
                {
                    // No intersection
                    return XMath.Sqrt(sqr_distance_to_pos) - circleRadius;
                }
                // Point intersection
                return 0;
            }

            // Line intersection
            var distance_to_intersection = XMath.Sqrt(sqr_distance_to_intersection);
            var distance_a = center_projection - distance_to_intersection;
            var distance_b = center_projection + distance_to_intersection;

            if (distance_a < -XGeometry2D.Eplsilon_f)
            {
                if (distance_b < -XGeometry2D.Eplsilon_f)
                {
                    // No intersection
                    return XMath.Sqrt(sqr_distance_to_pos) - circleRadius;
                }

                // Point intersection;
                return 0;
            }

            // Two points intersection;
            return 0;
        }
        #endregion

        #region Segment - Segment 
        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на отрезках.
        /// </summary>
        /// <param name="segment1">Первый отрезок.</param>
        /// <param name="segment2">Второй отрезок.</param>
        /// <returns>Расстояние.</returns>
        public static float SegmentSegment(in Segment2Df segment1, in Segment2Df segment2)
        {
            return SegmentSegment(in segment1.Start, in segment1.End, in segment2.Start, in segment2.End);
        }

        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на отрезках.
        /// </summary>
        /// <param name="segment1Start">Начало первого отрезка.</param>
        /// <param name="segment1End">Окончание первого отрезка.</param>
        /// <param name="segment2Start">Начало второго отрезка.</param>
        /// <param name="segment2End">Окончание второго отрезка.</param>
        /// <returns>Расстояние.</returns>
        public static float SegmentSegment(in Vector2Df segment1Start, in Vector2Df segment1End,
            in Vector2Df segment2Start, in Vector2Df segment2End)
        {
            var segment2_start_copy = segment2Start;
            var segment2_end_copy = segment2End;
            var from_2start_to_1start = segment1Start - segment2_start_copy;
            var direction1 = segment1End - segment1Start;
            var direction2 = segment2_end_copy - segment2_start_copy;
            var segment_1length = direction1.Length;
            var segment_2length = direction2.Length;

            var segment1IsAPoint = segment_1length < XGeometry2D.Eplsilon_f;
            var segment2IsAPoint = segment_2length < XGeometry2D.Eplsilon_f;
            if (segment1IsAPoint && segment2IsAPoint)
            {
                return Vector2Df.Distance(in segment1Start, in segment2_start_copy);
            }
            if (segment1IsAPoint)
            {
                direction2.Normalize();
                return PointSegment(in segment1Start, in segment2_start_copy, in segment2_end_copy, in direction2, segment_2length);
            }
            if (segment2IsAPoint)
            {
                direction1.Normalize();
                return PointSegment(in segment2_start_copy, in segment1Start, in segment1End, in direction1, segment_1length);
            }

            direction1.Normalize();
            direction2.Normalize();
            var denominator = Vector2Df.DotPerp(in direction1, in direction2);
            var perpDot1 = Vector2Df.DotPerp(in direction1, in from_2start_to_1start);
            var perpDot2 = Vector2Df.DotPerp(in direction2, in from_2start_to_1start);

            if (Math.Abs(denominator) < XGeometry2D.Eplsilon_f)
            {
                // Parallel
                if (Math.Abs(perpDot1) > XGeometry2D.Eplsilon_f || Math.Abs(perpDot2) > XGeometry2D.Eplsilon_f)
                {
                    // Not collinear
                    var segment2AProjection = -Vector2Df.Dot(in direction1, in from_2start_to_1start);
                    if (segment2AProjection > -XGeometry2D.Eplsilon_f &&
                        segment2AProjection < segment_1length + XGeometry2D.Eplsilon_f)
                    {
                        var distanceSqr = from_2start_to_1start.SqrLength - (segment2AProjection * segment2AProjection);
                        // distanceSqr can be negative
                        return distanceSqr <= 0 ? 0 : XMath.Sqrt(distanceSqr);
                    }

                    var from1ATo2B = segment2_end_copy - segment1Start;
                    var segment2BProjection = Vector2Df.Dot(in direction1, in from1ATo2B);
                    if (segment2BProjection > -XGeometry2D.Eplsilon_f &&
                        segment2BProjection < segment_1length + XGeometry2D.Eplsilon_f)
                    {
                        var distanceSqr = from1ATo2B.SqrLength - (segment2BProjection * segment2BProjection);
                        // distanceSqr can be negative
                        return distanceSqr <= 0 ? 0 : XMath.Sqrt(distanceSqr);
                    }

                    if (segment2AProjection < 0 && segment2BProjection < 0)
                    {
                        if (segment2AProjection > segment2BProjection)
                        {
                            return Vector2Df.Distance(in segment1Start, in segment2_start_copy);
                        }
                        return Vector2Df.Distance(in segment1Start, in segment2_end_copy);
                    }
                    if (segment2AProjection > 0 && segment2BProjection > 0)
                    {
                        if (segment2AProjection < segment2BProjection)
                        {
                            return Vector2Df.Distance(in segment1End, in segment2_start_copy);
                        }
                        return Vector2Df.Distance(in segment1End, in segment2_end_copy);
                    }
                    var segment1AProjection = Vector2Df.Dot(in direction2, in from_2start_to_1start);
                    var segment2Point = segment2_start_copy + (direction2 * segment1AProjection);
                    return Vector2Df.Distance(in segment1Start, in segment2Point);
                }
                // Collinear

                var codirected = Vector2Df.Dot(in direction1, in direction2) > 0;
                if (codirected)
                {
                    // Codirected
                    var segment2AProjection = -Vector2Df.Dot(in direction1, in from_2start_to_1start);
                    if (segment2AProjection > -XGeometry2D.Eplsilon_f)
                    {
                        // 1A------1B
                        //     2A------2B
                        return SegmentSegmentCollinear(in segment1Start, in segment1End, in segment2_start_copy);
                    }
                    else
                    {
                        //     1A------1B
                        // 2A------2B
                        return SegmentSegmentCollinear(in segment2_start_copy, in segment2_end_copy, in segment1Start);
                    }
                }
                else
                {
                    // Contradirected
                    var segment2BProjection = Vector2Df.Dot(in direction1, segment2_end_copy - segment1Start);
                    if (segment2BProjection > -XGeometry2D.Eplsilon_f)
                    {
                        // 1A------1B
                        //     2B------2A
                        return SegmentSegmentCollinear(in segment1Start, in segment1End, in segment2_end_copy);
                    }
                    else
                    {
                        //     1A------1B
                        // 2B------2A
                        return SegmentSegmentCollinear(in segment2_end_copy, in segment2_start_copy, in segment1Start);
                    }
                }
            }

            // Not parallel
            var distance1 = perpDot2 / denominator;
            var distance2 = perpDot1 / denominator;
            if (distance1 < -XGeometry2D.Eplsilon_f || distance1 > segment_1length + XGeometry2D.Eplsilon_f ||
                distance2 < -XGeometry2D.Eplsilon_f || distance2 > segment_2length + XGeometry2D.Eplsilon_f)
            {
                // No intersection
                var codirected = Vector2Df.Dot(in direction1, in direction2) > 0;
                Vector2Df from1ATo2B;
                if (!codirected)
                {
                    XMath.Swap(ref segment2_start_copy, ref segment2_end_copy);
                    direction2 = -direction2;
                    from1ATo2B = -from_2start_to_1start;
                    from_2start_to_1start = segment1Start - segment2_start_copy;
                    distance2 = segment_2length - distance2;
                }
                else
                {
                    from1ATo2B = segment2_end_copy - segment1Start;
                }
                Vector2Df segment1Point;
                Vector2Df segment2Point;

                var segment2AProjection = -Vector2Df.Dot(in direction1, in from_2start_to_1start);
                var segment2BProjection = Vector2Df.Dot(in direction1, in from1ATo2B);

                var segment2AIsAfter1A = segment2AProjection > -XGeometry2D.Eplsilon_f;
                var segment2BIsBefore1B = segment2BProjection < segment_1length + XGeometry2D.Eplsilon_f;
                var segment2AOnSegment1 = segment2AIsAfter1A && segment2AProjection < segment_1length + XGeometry2D.Eplsilon_f;
                var segment2BOnSegment1 = segment2BProjection > -XGeometry2D.Eplsilon_f && segment2BIsBefore1B;
                if (segment2AOnSegment1 && segment2BOnSegment1)
                {
                    if (distance2 < -XGeometry2D.Eplsilon_f)
                    {
                        segment1Point = segment1Start + (direction1 * segment2AProjection);
                        segment2Point = segment2_start_copy;
                    }
                    else
                    {
                        segment1Point = segment1Start + (direction1 * segment2BProjection);
                        segment2Point = segment2_end_copy;
                    }
                }
                else if (!segment2AOnSegment1 && !segment2BOnSegment1)
                {
                    if (!segment2AIsAfter1A && !segment2BIsBefore1B)
                    {
                        segment1Point = distance1 < -XGeometry2D.Eplsilon_f ? segment1Start : segment1End;
                    }
                    else
                    {
                        // Not on segment
                        segment1Point = segment2AIsAfter1A ? segment1End : segment1Start;
                    }
                    var segment1PointProjection = Vector2Df.Dot(in direction2, segment1Point - segment2_start_copy);
                    segment1PointProjection = XMath.Clamp(segment1PointProjection, 0, segment_2length);
                    segment2Point = segment2_start_copy + (direction2 * segment1PointProjection);
                }
                else if (segment2AOnSegment1)
                {
                    if (distance2 < -XGeometry2D.Eplsilon_f)
                    {
                        segment1Point = segment1Start + (direction1 * segment2AProjection);
                        segment2Point = segment2_start_copy;
                    }
                    else
                    {
                        segment1Point = segment1End;
                        var segment1PointProjection = Vector2Df.Dot(in direction2, segment1Point - segment2_start_copy);
                        segment1PointProjection = XMath.Clamp(segment1PointProjection, 0, segment_2length);
                        segment2Point = segment2_start_copy + (direction2 * segment1PointProjection);
                    }
                }
                else
                {
                    if (distance2 > segment_2length + XGeometry2D.Eplsilon_f)
                    {
                        segment1Point = segment1Start + (direction1 * segment2BProjection);
                        segment2Point = segment2_end_copy;
                    }
                    else
                    {
                        segment1Point = segment1Start;
                        var segment1PointProjection = Vector2Df.Dot(in direction2, segment1Point - segment2_start_copy);
                        segment1PointProjection = XMath.Clamp(segment1PointProjection, 0, segment_2length);
                        segment2Point = segment2_start_copy + (direction2 * segment1PointProjection);
                    }
                }
                return Vector2Df.Distance(in segment1Point, in segment2Point);
            }

            // Point intersection
            return 0;
        }

        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на отрезках.
        /// </summary>
        /// <param name="left_a">Начало отрезка слева.</param>
        /// <param name="left_b">Конец отрезка слева.</param>
        /// <param name="right_a">Начало отрезка справа.</param>
        /// <returns>Расстояние.</returns>
        private static float SegmentSegmentCollinear(in Vector2Df left_a, in Vector2Df left_b, in Vector2Df right_a)
        {
            var left_direction = left_b - left_a;
            var right_a_projection = Vector2Df.Dot(left_direction.Normalized, right_a - left_b);
            if (Math.Abs(right_a_projection) < XGeometry2D.Eplsilon_f)
            {
                // LB == RA
                // LA------LB
                //         RA------RB

                // Point intersection
                return 0;
            }
            if (right_a_projection < 0)
            {
                // LB > RA
                // LA------LB
                //     RARB
                //     RA--RB
                //     RA------RB

                // Segment intersection
                return 0;
            }
            // LB < RA
            // LA------LB
            //             RA------RB

            // No intersection
            return right_a_projection;
        }
        #endregion

        #region Segment - Circle 
        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на отрезке и окружности.
        /// </summary>
        /// <param name="segment">Отрезок.</param>
        /// <param name="circle">Окружность.</param>
        /// <returns>Расстояние.</returns>
        public static float SegmentCircle(in Segment2Df segment, in Circle2Df circle)
        {
            return SegmentCircle(in segment.Start, in segment.End, in circle.Center, circle.Radius);
        }

        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на отрезке и окружности.
        /// </summary>
        /// <param name="start">Начало отрезка.</param>
        /// <param name="end">Конец отрезка.</param>
        /// <param name="circleCenter">Центр окружности.</param>
        /// <param name="circleRadius">Радиус окружности.</param>
        /// <returns>Расстояние.</returns>
        public static float SegmentCircle(in Vector2Df start, in Vector2Df end, in Vector2Df circleCenter, float circleRadius)
        {
            var segment_start_to_center = circleCenter - start;
            var from_start_to_end = end - start;
            var segment_length = from_start_to_end.Length;
            if (segment_length < XGeometry2D.Eplsilon_f)
            {
                return segment_start_to_center.Length - circleRadius;
            }

            var segment_direction = from_start_to_end.Normalized;
            var center_projection = Vector2Df.Dot(in segment_direction, in segment_start_to_center);
            if (center_projection + circleRadius < -XGeometry2D.Eplsilon_f ||
                center_projection - circleRadius > segment_length + XGeometry2D.Eplsilon_f)
            {
                // No intersection
                if (center_projection < 0)
                {
                    return XMath.Sqrt(segment_start_to_center.SqrLength) - circleRadius;
                }
                return (circleCenter - end).Length - circleRadius;
            }

            var sqr_distance_to_a = segment_start_to_center.SqrLength;
            var sqr_distance_to_line = sqr_distance_to_a - (center_projection * center_projection);
            var sqr_distance_to_intersection = (circleRadius * circleRadius) - sqr_distance_to_line;
            if (sqr_distance_to_intersection < -XGeometry2D.Eplsilon_f)
            {
                // No intersection
                if (center_projection < -XGeometry2D.Eplsilon_f)
                {
                    return XMath.Sqrt(sqr_distance_to_a) - circleRadius;
                }
                if (center_projection > segment_length + XGeometry2D.Eplsilon_f)
                {
                    return (circleCenter - end).Length - circleRadius;
                }
                return XMath.Sqrt(sqr_distance_to_line) - circleRadius;
            }

            if (sqr_distance_to_intersection < XGeometry2D.Eplsilon_f)
            {
                if (center_projection < -XGeometry2D.Eplsilon_f)
                {
                    // No intersection
                    return XMath.Sqrt(sqr_distance_to_a) - circleRadius;
                }
                if (center_projection > segment_length + XGeometry2D.Eplsilon_f)
                {
                    // No intersection
                    return (circleCenter - end).Length - circleRadius;
                }
                // Point intersection
                return 0;
            }

            // Line intersection
            var distance_to_intersection = XMath.Sqrt(sqr_distance_to_intersection);
            var distance_a = center_projection - distance_to_intersection;
            var distance_b = center_projection + distance_to_intersection;

            var point_a_is_after_segment_start = distance_a > -XGeometry2D.Eplsilon_f;
            var point_b_is_before_segment_end = distance_b < segment_length + XGeometry2D.Eplsilon_f;

            if (point_a_is_after_segment_start && point_b_is_before_segment_end)
            {
                // Two points intersection
                return 0;
            }
            if (!point_a_is_after_segment_start && !point_b_is_before_segment_end)
            {
                // The segment is inside, but no intersection
                distance_b = -(distance_b - segment_length);
                return distance_a > distance_b ? distance_a : distance_b;
            }

            var point_a_is_before_segment_end = distance_a < segment_length + XGeometry2D.Eplsilon_f;
            if (point_a_is_after_segment_start && point_a_is_before_segment_end)
            {
                // Point A intersection
                return 0;
            }
            var point_b_is_after_segment_start = distance_b > -XGeometry2D.Eplsilon_f;
            if (point_b_is_after_segment_start && point_b_is_before_segment_end)
            {
                // Point B intersection
                return 0;
            }

            // No intersection
            if (center_projection < 0)
            {
                return XMath.Sqrt(sqr_distance_to_a) - circleRadius;
            }
            return (circleCenter - end).Length - circleRadius;
        }
        #endregion

        #region Circle - Circle 
        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на окружностях.
        /// </summary>
        /// <param name="circleA">Первая окружность.</param>
        /// <param name="circleB">Вторая окружность.</param>
        /// <returns>
        /// Положительное значение, если окружности не пересекаются, отрицательное иначе
        /// Отрицательная величина может быть интерпретирована как глубина проникновения
        /// </returns>
        public static float CircleCircle(in Circle2Df circleA, in Circle2Df circleB)
        {
            return CircleCircle(in circleA.Center, circleA.Radius, in circleB.Center, circleB.Radius);
        }

        /// <summary>
        /// Вычисление расстояние между самыми близкими точками на окружностях.
        /// </summary>
        /// <param name="centerA">Центр первой окружности.</param>
        /// <param name="radiusA">Радиус первой окружности.</param>
        /// <param name="centerB">Центр второй окружности.</param>
        /// <param name="radiusB">Радиус второй окружности.</param>
        /// <returns>
        /// Положительное значение, если окружности не пересекаются, отрицательное иначе
        /// Отрицательная величина может быть интерпретирована как глубина проникновения
        /// </returns>
        public static float CircleCircle(in Vector2Df centerA, float radiusA, in Vector2Df centerB,
                float radiusB)
        {
            return Vector2Df.Distance(in centerA, in centerB) - radiusA - radiusB;
        }
        #endregion
    }
    /**@}*/
}