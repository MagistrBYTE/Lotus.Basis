﻿/* Poly2Tri
 * Copyright (c) 2009-2010, Poly2Tri Contributors
 * http://code.google.com/p/poly2tri/
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * * Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 * * Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 * * Neither the name of Poly2Tri nor the names of its contributors may be
 *   used to endorse or promote products derived from this software without specific
 *   prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */


/*
 * The Following notice applies to the Method SplitComplexPolygon and the 
 * class SplitComplexPolygonNode.   Both are altered only enough to convert to C#
 * and take advantage of some of C#'s language features.   Any errors
 * are thus mine from the conversion and not Eric's.
 * 
 * Copyright (c) 2007 Eric Jordan
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty.  In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 * */


/*
 * Portions of the following code (notably: the methods PolygonUnion, 
 * PolygonSubtract, PolygonIntersect, PolygonOperationContext.Init,
 * PolygonOperationContext.VerticesIntersect,
 * PolygonOperationContext.PointInPolygonAngle, and
 * PolygonOperationContext.VectorAngle are from the Farseer Physics Engine 3.0
 * and are covered under the Microsoft Permissive License V1.1
 * (http://farseerphysics.codeplex.com/license)
 * 
 * Microsoft Permissive License (Ms-PL)
 * 
 * This license governs use of the accompanying software. If you use the 
 * software, you accept this license. If you do not accept the license, do not
 * use the software.
 * 
 * 1. Definitions
 * 
 * The terms "reproduce," "reproduction," "derivative works," and
 * "distribution" have the same meaning here as under U.S. copyright law.
 * 
 * A "contribution" is the original software, or any additions or changes to
 * the software.
 * 
 * A "contributor" is any person that distributes its contribution under this 
 * license.
 *
 * "Licensed patents" are a contributor's patent claims that read directly on
 * its contribution.
 * 
 * 2. Grant of Rights
 * 
 * (A) Copyright Grant- Subject to the terms of this license, including the
 * license conditions and limitations in section 3, each contributor grants
 * you a non-exclusive, worldwide, royalty-free copyright license to reproduce
 * its contribution, prepare derivative works of its contribution, and
 * distribute its contribution or any derivative works that you create.
 * 
 * (B) Patent Grant- Subject to the terms of this license, including the
 * license conditions and limitations in section 3, each contributor grants
 * you a non-exclusive, worldwide, royalty-free license under its licensed
 * patents to make, have made, use, sell, offer for sale, import, and/or
 * otherwise dispose of its contribution in the software or derivative works
 * of the contribution in the software.
 * 
 * 3. Conditions and Limitations
 * 
 * (A) No Trademark License- This license does not grant you rights to use
 * any contributors' name, logo, or trademarks.
 * 
 * (B) If you bring a patent claim against any contributor over patents that
 * you claim are infringed by the software, your patent license from such
 * contributor to the software ends automatically.
 * 
 * (C) If you distribute any portion of the software, you must retain all
 * copyright, patent, trademark, and attribution notices that are present
 * in the software.
 * 
 * (D) If you distribute any portion of the software in source code form, you
 * may do so only under this license by including a complete copy of this
 * license with your distribution. If you distribute any portion of the
 * software in compiled or object code form, you may only do so under a
 * license that complies with this license.
 * 
 * (E) The software is licensed "as-is." You bear the risk of using it. The
 * contributors give no express warranties, guarantees or conditions. You may
 * have additional consumer rights under your local laws which this license
 * cannot change. To the extent permitted under your local laws, the
 * contributors exclude the implied warranties of merchantability, fitness for
 * a particular purpose and non-infringement. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Poly2Tri.Triangulation.Util;
using Poly2Tri.Utility;

#nullable disable

namespace Poly2Tri.Triangulation.Polygon
{
    public static class PolygonUtil
    {
        public enum PolyUnionError
        {
            None,
            NoIntersections,
            Poly1InsidePoly2,
            InfiniteLoop
        }

        [Flags]
        public enum PolyOperation : uint
        {
            None = 0,
            Union = 1 << 0,
            Intersect = 1 << 1,
            Subtract = 1 << 2,
        }


        public static Point2DList.WindingOrderType CalculateWindingOrder(IList<Point2D> l)
        {
            var area = 0.0;
            for (var i = 0; i < l.Count; i++)
            {
                var j = (i + 1) % l.Count;
                area += l[i].X * l[j].Y;
                area -= l[i].Y * l[j].X;
            }
            area /= 2.0f;

            // the sign of the 'area' of the polygon is all we are interested in.
            if (area < 0.0)
            {
                return Point2DList.WindingOrderType.Clockwise;
            }
            else if (area > 0.0)
            {
                return Point2DList.WindingOrderType.AntiClockwise;
            }

            // error condition - not even verts to calculate, non-simple poly, etc.
            return Point2DList.WindingOrderType.Unknown;
        }


        /// <summary>
        /// Check if the polys are similar to within a tolerance (Doesn't include reflections,
        /// but allows for the points to be numbered differently, but not reversed).
        /// </summary>
        /// <param name="poly1"></param>
        /// <param name="poly2"></param>
        /// <returns></returns>
        public static bool PolygonsAreSame2D(IList<Point2D> poly1, IList<Point2D> poly2)
        {
            var numVerts1 = poly1.Count;
            var numVerts2 = poly2.Count;

            if (numVerts1 != numVerts2)
            {
                return false;
            }
            const double EPSILON = 0.01;
            const double EPSILON_SQ = EPSILON * EPSILON;

            // Bounds the same to within tolerance, are there polys the same?
            var vdelta = new Point2D(0.0, 0.0);
            for (var k = 0; k < numVerts2; ++k)
            {
                // Look for a match in verts2 to the first vertex in verts1
                vdelta.Set(poly1[0].X, poly1[0].Y);
                vdelta.Subtract(poly2[k]);

                if (vdelta.MagnitudeSquared() < EPSILON_SQ)
                {
                    // Found match to the first point, now check the other points continuing round
                    // if the points don't match in the first direction we check, then it's possible
                    // that the polygons have a different winding order, so we check going round 
                    // the opposite way as well
                    var matchedVertIndex = k;
                    var bReverseSearch = false;
                    while (true)
                    {
                        var bMatchFound = true;
                        for (var i = 1; i < numVerts1; ++i)
                        {
                            if (!bReverseSearch)
                            {
                                ++k;
                            }
                            else
                            {
                                --k;
                                if (k < 0)
                                {
                                    k = numVerts2 - 1;
                                }
                            }

                            vdelta.Set(poly1[i].X, poly1[i].Y);
                            vdelta.Subtract(poly2[k % numVerts2]);
                            if (vdelta.MagnitudeSquared() >= EPSILON_SQ)
                            {
                                if (bReverseSearch)
                                {
                                    // didn't find a match going in either direction, so the polygons are not the same
                                    return false;
                                }
                                else
                                {
                                    // mismatch in the first direction checked, so check the other direction.
                                    k = matchedVertIndex;
                                    bReverseSearch = true;
                                    bMatchFound = false;
                                    break;
                                }
                            }
                        }

                        if (bMatchFound)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        public static bool PointInPolygon2D(IList<Point2D> polygon, Point2D p)
        {
            if (polygon == null || polygon.Count < 3)
            {
                return false;
            }

            var numVerts = polygon.Count;
            var p0 = polygon[numVerts - 1];
            var bYFlag0 = (p0.Y >= p.Y);

            var bInside = false;
            for (var j = 0; j < numVerts; ++j)
            {
                var p1 = polygon[j];
                var bYFlag1 = (p1.Y >= p.Y);
                if (bYFlag0 != bYFlag1)
                {
                    if (((p1.Y - p.Y) * (p0.X - p1.X) >= (p1.X - p.X) * (p0.Y - p1.Y)) == bYFlag1)
                    {
                        bInside = !bInside;
                    }
                }

                // Move to the next pair of vertices, retaining info as possible.
                bYFlag0 = bYFlag1;
                p0 = p1;
            }

            return bInside;
        }


        // Given two polygons and their bounding rects, returns true if the two polygons intersect.
        // This test will NOT determine if one of the two polygons is contained within the other or if the 
        // two polygons are similar - it will return false in all those cases.  The only case it will return
        // true for is if the two polygons actually intersect.
        public static bool PolygonsIntersect2D(IList<Point2D> poly1, Rect2D boundRect1,
                                                IList<Point2D> poly2, Rect2D boundRect2)
        {
            // do some quick tests first before doing any real work
            if (poly1 == null || poly1.Count < 3 || boundRect1.IsEmpty || poly2 == null || poly2.Count < 3 || boundRect2.IsEmpty)
            {
                return false;
            }

            if (!boundRect1.Intersects(boundRect2))
            {
                return false;
            }

            // We first check whether any edge of one poly intersects any edge of the other poly. If they do,
            // then the two polys intersect.

            // Make the epsilon a function of the size of the polys. We could take the heights of the rects 
            // also into consideration here if needed; but, that should not usually be necessary.
            var epsilon = Math.Max(Math.Min(boundRect1.Width, boundRect2.Width) * 0.001f, MathUtil.EPSILON);

            var numVerts1 = poly1.Count;
            var numVerts2 = poly2.Count;
            for (var i = 0; i < numVerts1; ++i)
            {
                var lineEndVert1 = i + 1;
                if (lineEndVert1 == numVerts1)
                {
                    lineEndVert1 = 0;
                }
                for (var j = 0; j < numVerts2; ++j)
                {
                    var lineEndVert2 = j + 1;
                    if (lineEndVert2 == numVerts2)
                    {
                        lineEndVert2 = 0;
                    }
                    Point2D tmp = null;
                    if (TriangulationUtil.LinesIntersect2D(poly1[i], poly1[lineEndVert1], poly2[j], poly2[lineEndVert2], ref tmp, epsilon))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks to see if poly1 contains poly2.  return true if so, false otherwise.
        ///
        /// If the polygons intersect, then poly1 cannot contain poly2 (or vice-versa for that matter)
        /// Since the poly intersection test can be somewhat expensive, we'll only run it if the user
        /// requests it.   If runIntersectionTest is false, then it is assumed that the user has already
        /// verified that the polygons do not intersect.  If the polygons DO intersect and runIntersectionTest
        /// is false, then the return value is meaningless.  Caveat emptor.
        /// 
        /// As an added bonus, just to cause more user-carnage, if runIntersectionTest is false, then the 
        /// boundRects are not used and can safely be passed in as nulls.   However, if runIntersectionTest
        /// is true and you pass nulls for boundRect1 or boundRect2, you will cause a program crash.
        /// 
        /// Finally, the polygon points are assumed to be passed in Clockwise winding order.   It is possible
        /// that CounterClockwise ordering would work, but I have not verified the behavior in that case. 
        /// 
        /// </summary>
        /// <param name="poly1">points of polygon1</param>
        /// <param name="boundRect1">bounding rect of polygon1.  Only used if runIntersectionTest is true</param>
        /// <param name="poly2">points of polygon2</param>
        /// <param name="boundRect2">bounding rect of polygon2.  Only used if runIntersectionTest is true</param>
        /// <param name="runIntersectionTest">see summary above</param>
        /// <returns>true if poly1 fully contains poly2</returns>
        public static bool PolygonContainsPolygon(IList<Point2D> poly1, Rect2D boundRect1,
                                                    IList<Point2D> poly2, Rect2D boundRect2,
                                                    bool runIntersectionTest = true)
        {
            // quick early-out tests
            if (poly1 == null || poly1.Count < 3 || poly2 == null || poly2.Count < 3)
            {
                return false;
            }

            if (runIntersectionTest)
            {
                // make sure the polygons are not actually the same...
                if (poly1.Count == poly2.Count)
                {
                    // Check if the polys are similar to within a tolerance (Doesn't include reflections,
                    // but allows for the points to be numbered differently)
                    if (PolygonsAreSame2D(poly1, poly2))
                    {
                        return false;
                    }
                }

                var bIntersect = PolygonsIntersect2D(poly1, boundRect1, poly2, boundRect2);
                if (bIntersect)
                {
                    return false;
                }
            }

            // Since we (now) know that the polygons don't intersect and they are not the same, we can just do a
            // single check to see if ANY point in poly2 is inside poly1.  If so, then all points of poly2
            // are inside poly1.  If not, then ALL points of poly2 are outside poly1.
            if (PointInPolygon2D(poly1, poly2[0]))
            {
                return true;
            }

            return false;
        }



        ////////////////////////////////////////////////////////////////////////////////
        // ClipPolygonToEdge2D
        //
        // This function clips a polygon against an edge. The portion of the polygon
        // which is to the left of the edge (while going from edgeBegin to edgeEnd) 
        // is returned in "outPoly". Note that the clipped polygon may have more vertices
        // than the input polygon. Make sure that outPolyArraySize is sufficiently large. 
        // Otherwise, you may get incorrect results and may be an assert (hopefully, no crash).
        // Pass in the actual size of the array in "outPolyArraySize".
        //
        // Read Sutherland-Hidgman algorithm description in Foley & van Dam book for 
        // details about this.
        //
        ///////////////////////////////////////////////////////////////////////////
        private static void ClipPolygonToEdge2D(Point2D edgeBegin,
                                                Point2D edgeEnd,
                                                IList<Point2D> poly,
                                                out List<Point2D> outPoly)
        {
            outPoly = null;
            if (edgeBegin == null ||
                edgeEnd == null ||
                poly == null ||
                poly.Count < 3)
            {
                return;
            }

            outPoly = new List<Point2D>();
            var lastVertex = poly.Count - 1;
            var edgeRayVector = new Point2D(edgeEnd.X - edgeBegin.X, edgeEnd.Y - edgeBegin.Y);
            // Note: >= 0 as opposed to <= 0 is intentional. We are 
            // dealing with x and z here. And in our case, z axis goes
            // downward while the x axis goes rightward.
            //bool bLastVertexIsToRight = TriangulationUtil.PointRelativeToLine2D(poly[lastVertex], edgeBegin, edgeEnd) >= 0;
            var bLastVertexIsToRight = TriangulationUtil.Orient2d(edgeBegin, edgeEnd, poly[lastVertex]) == Orientation.Clockwise;
            var tempRay = new Point2D(0.0, 0.0);

            for (var curVertex = 0; curVertex < poly.Count; curVertex++)
            {
                //bool bCurVertexIsToRight = TriangulationUtil.PointRelativeToLine2D(poly[curVertex], edgeBegin, edgeEnd) >= 0;
                var bCurVertexIsToRight = TriangulationUtil.Orient2d(edgeBegin, edgeEnd, poly[curVertex]) == Orientation.Clockwise;
                if (bCurVertexIsToRight)
                {
                    if (bLastVertexIsToRight)
                    {
                        outPoly.Add(poly[curVertex]);
                    }
                    else
                    {
                        tempRay.Set(poly[curVertex].X - poly[lastVertex].X, poly[curVertex].Y - poly[lastVertex].Y);
                        var ptIntersection = new Point2D(0.0, 0.0);
                        var bIntersect = TriangulationUtil.RaysIntersect2D(poly[lastVertex], tempRay, edgeBegin, edgeRayVector, ref ptIntersection);
                        if (bIntersect)
                        {
                            outPoly.Add(ptIntersection);
                            outPoly.Add(poly[curVertex]);
                        }
                    }
                }
                else if (bLastVertexIsToRight)
                {
                    tempRay.Set(poly[curVertex].X - poly[lastVertex].X, poly[curVertex].Y - poly[lastVertex].Y);
                    var ptIntersection = new Point2D(0.0, 0.0);
                    var bIntersect = TriangulationUtil.RaysIntersect2D(poly[lastVertex], tempRay, edgeBegin, edgeRayVector, ref ptIntersection);
                    if (bIntersect)
                    {
                        outPoly.Add(ptIntersection);
                    }
                }

                lastVertex = curVertex;
                bLastVertexIsToRight = bCurVertexIsToRight;
            }
        }


        public static void ClipPolygonToPolygon(IList<Point2D> poly, IList<Point2D> clipPoly, out List<Point2D> outPoly)
        {
            outPoly = null;
            if (poly == null || poly.Count < 3 || clipPoly == null || clipPoly.Count < 3)
            {
                return;
            }

            outPoly = new List<Point2D>(poly);
            var numClipVertices = clipPoly.Count;
            var lastVertex = numClipVertices - 1;

            // The algorithm keeps clipping the polygon against each edge of "clipPoly".
            for (var curVertex = 0; curVertex < numClipVertices; curVertex++)
            {
                List<Point2D> clippedPoly;
                var edgeBegin = clipPoly[lastVertex];
                var edgeEnd = clipPoly[curVertex];
                ClipPolygonToEdge2D(edgeBegin, edgeEnd, outPoly, out clippedPoly);
                outPoly.Clear();
                outPoly.AddRange(clippedPoly);

                lastVertex = curVertex;
            }
        }

        /// <summary>
        /// Merges two polygons, given that they intersect.
        /// </summary>
        /// <param name="polygon1">The first polygon.</param>
        /// <param name="polygon2">The second polygon.</param>
        /// <param name="union">The union of the two polygons</param>
        /// <returns>The error returned from union</returns>
        public static PolyUnionError PolygonUnion(Point2DList polygon1, Point2DList polygon2, out Point2DList union)
        {
            var ctx = new PolygonOperationContext();
            ctx.Init(PolyOperation.Union, polygon1, polygon2);
            PolygonUnionInternal(ctx);
            union = ctx.Union;
            return ctx.Error;
        }


        private static void PolygonUnionInternal(PolygonOperationContext ctx)
        {
            var union = ctx.Union;
            if (ctx.StartingIndex == -1)
            {
                switch (ctx.Error)
                {
                    case PolyUnionError.NoIntersections:
                    case PolyUnionError.InfiniteLoop:
                        return;
                    case PolyUnionError.Poly1InsidePoly2:
                        union.AddRange(ctx.OriginalPolygon2);
                        return;
                }
            }

            var currentPoly = ctx.Poly1;
            var otherPoly = ctx.Poly2;
            var currentPolyVectorAngles = ctx.Poly1VectorAngles;

            // Store the starting vertex so we can refer to it later.
            var startingVertex = ctx.Poly1[ctx.StartingIndex];
            var currentIndex = ctx.StartingIndex;
            var firstPoly2Index = -1;
            union.Clear();

            do
            {
                // Add the current vertex to the final union
                union.Add(currentPoly[currentIndex]);

                foreach (var intersect in ctx.Intersections)
                {
                    // If the current point is an intersection point
                    if (currentPoly[currentIndex].Equals(intersect.IntersectionPoint, currentPoly.Epsilon))
                    {
                        // Make sure we want to swap polygons here.
                        var otherIndex = otherPoly.IndexOf(intersect.IntersectionPoint);

                        // If the next vertex, if we do swap, is not inside the current polygon,
                        // then its safe to swap, otherwise, just carry on with the current poly.
                        var comparePointIndex = otherPoly.NextIndex(otherIndex);
                        var comparePoint = otherPoly[comparePointIndex];
                        bool bPointInPolygonAngle;
                        if (currentPolyVectorAngles[comparePointIndex] == -1)
                        {
                            bPointInPolygonAngle = PolygonOperationContext.PointInPolygonAngle(comparePoint, currentPoly);
                            currentPolyVectorAngles[comparePointIndex] = bPointInPolygonAngle ? 1 : 0;
                        }
                        else
                        {
                            bPointInPolygonAngle = (currentPolyVectorAngles[comparePointIndex] == 1);
                        }

                        if (!bPointInPolygonAngle)
                        {
                            // switch polygons
                            if (currentPoly == ctx.Poly1)
                            {
                                currentPoly = ctx.Poly2;
                                currentPolyVectorAngles = ctx.Poly2VectorAngles;
                                otherPoly = ctx.Poly1;
                                if (firstPoly2Index < 0)
                                {
                                    firstPoly2Index = otherIndex;
                                }
                            }
                            else
                            {
                                currentPoly = ctx.Poly1;
                                currentPolyVectorAngles = ctx.Poly1VectorAngles;
                                otherPoly = ctx.Poly2;
                            }

                            // set currentIndex
                            currentIndex = otherIndex;

                            // Stop checking intersections for this point.
                            break;
                        }
                    }
                }

                // Move to next index
                currentIndex = currentPoly.NextIndex(currentIndex);

                if (currentPoly == ctx.Poly1)
                {
                    if (currentIndex == 0)
                    {
                        break;
                    }
                }
                else
                {
                    if (firstPoly2Index >= 0 && currentIndex == firstPoly2Index)
                    {
                        break;
                    }
                }
            } while ((currentPoly[currentIndex].Equals(startingVertex) && (union.Count <= (ctx.Poly1.Count + ctx.Poly2.Count))));

            // If the number of vertices in the union is more than the combined vertices
            // of the input polygons, then something is wrong and the algorithm will
            // loop forever. Luckily, we check for that.
            if (union.Count > (ctx.Poly1.Count + ctx.Poly2.Count))
            {
                ctx.Error = PolyUnionError.InfiniteLoop;
            }
        }


        /// <summary>
        /// Finds the intersection between two polygons.
        /// </summary>
        /// <param name="polygon1">The first polygon.</param>
        /// <param name="polygon2">The second polygon.</param>
        /// <param name="intersectOut">The intersection of the two polygons</param>
        /// <returns>error code</returns>
        public static PolyUnionError PolygonIntersect(Point2DList polygon1, Point2DList polygon2, out Point2DList intersectOut)
        {
            var ctx = new PolygonOperationContext();
            ctx.Init(PolyOperation.Intersect, polygon1, polygon2);
            PolygonIntersectInternal(ctx);
            intersectOut = ctx.Intersect;
            return ctx.Error;
        }


        private static void PolygonIntersectInternal(PolygonOperationContext ctx)
        {
            var intersectOut = ctx.Intersect;
            if (ctx.StartingIndex == -1)
            {
                switch (ctx.Error)
                {
                    case PolyUnionError.NoIntersections:
                    case PolyUnionError.InfiniteLoop:
                        return;
                    case PolyUnionError.Poly1InsidePoly2:
                        intersectOut.AddRange(ctx.OriginalPolygon2);
                        return;
                }
            }

            var currentPoly = ctx.Poly1;
            var otherPoly = ctx.Poly2;
            var currentPolyVectorAngles = ctx.Poly1VectorAngles;

            // Store the starting vertex so we can refer to it later.            
            var currentIndex = ctx.Poly1.IndexOf(ctx.Intersections[0].IntersectionPoint);
            var startingVertex = ctx.Poly1[currentIndex];
            var firstPoly1Index = currentIndex;
            var firstPoly2Index = -1;
            intersectOut.Clear();

            do
            {
                // Add the current vertex to the final intersection
                if (intersectOut.Contains(currentPoly[currentIndex]))
                {
                    // This can happen when the two polygons only share a single edge, and neither is inside the other
                    break;
                }
                intersectOut.Add(currentPoly[currentIndex]);

                foreach (var intersect in ctx.Intersections)
                {
                    // If the current point is an intersection point
                    if (currentPoly[currentIndex].Equals(intersect.IntersectionPoint, currentPoly.Epsilon))
                    {
                        // Make sure we want to swap polygons here.
                        var otherIndex = otherPoly.IndexOf(intersect.IntersectionPoint);

                        // If the next vertex, if we do swap, is inside the current polygon,
                        // then its safe to swap, otherwise, just carry on with the current poly.
                        var comparePointIndex = otherPoly.NextIndex(otherIndex);
                        var comparePoint = otherPoly[comparePointIndex];
                        bool bPointInPolygonAngle;
                        if (currentPolyVectorAngles[comparePointIndex] == -1)
                        {
                            bPointInPolygonAngle = PolygonOperationContext.PointInPolygonAngle(comparePoint, currentPoly);
                            currentPolyVectorAngles[comparePointIndex] = bPointInPolygonAngle ? 1 : 0;
                        }
                        else
                        {
                            bPointInPolygonAngle = (currentPolyVectorAngles[comparePointIndex] == 1);
                        }

                        if (bPointInPolygonAngle)
                        {
                            // switch polygons
                            if (currentPoly == ctx.Poly1)
                            {
                                currentPoly = ctx.Poly2;
                                currentPolyVectorAngles = ctx.Poly2VectorAngles;
                                otherPoly = ctx.Poly1;
                                if (firstPoly2Index < 0)
                                {
                                    firstPoly2Index = otherIndex;
                                }
                            }
                            else
                            {
                                currentPoly = ctx.Poly1;
                                currentPolyVectorAngles = ctx.Poly1VectorAngles;
                                otherPoly = ctx.Poly2;
                            }

                            // set currentIndex
                            currentIndex = otherIndex;

                            // Stop checking intersections for this point.
                            break;
                        }
                    }
                }

                // Move to next index
                currentIndex = currentPoly.NextIndex(currentIndex);

                if (currentPoly == ctx.Poly1)
                {
                    if (currentIndex == firstPoly1Index)
                    {
                        break;
                    }
                }
                else
                {
                    if (firstPoly2Index >= 0 && currentIndex == firstPoly2Index)
                    {
                        break;
                    }
                }
            } while (currentPoly[currentIndex].Equals(startingVertex) && intersectOut.Count <= (ctx.Poly1.Count + ctx.Poly2.Count));

            // If the number of vertices in the union is more than the combined vertices
            // of the input polygons, then something is wrong and the algorithm will
            // loop forever. Luckily, we check for that.
            if (intersectOut.Count > (ctx.Poly1.Count + ctx.Poly2.Count))
            {
                ctx.Error = PolyUnionError.InfiniteLoop;
            }
        }


        /// <summary>
        /// Subtracts one polygon from another.
        /// </summary>
        /// <param name="polygon1">The base polygon.</param>
        /// <param name="polygon2">The polygon to subtract from the base.</param>
        /// <param name="subtract">The result of the polygon subtraction</param>
        /// <returns>error code</returns>
        public static PolyUnionError PolygonSubtract(Point2DList polygon1, Point2DList polygon2, out Point2DList subtract)
        {
            var ctx = new PolygonOperationContext();
            ctx.Init(PolyOperation.Subtract, polygon1, polygon2);
            PolygonSubtractInternal(ctx);
            subtract = ctx.Subtract;
            return ctx.Error;
        }

        private static void PolygonSubtractInternal(PolygonOperationContext ctx)
        {
            var subtract = ctx.Subtract;
            if (ctx.StartingIndex == -1)
            {
                switch (ctx.Error)
                {
                    case PolyUnionError.NoIntersections:
                    case PolyUnionError.InfiniteLoop:
                    case PolyUnionError.Poly1InsidePoly2:
                        return;
                }
            }

            var currentPoly = ctx.Poly1;
            var otherPoly = ctx.Poly2;
            var currentPolyVectorAngles = ctx.Poly1VectorAngles;

            // Store the starting vertex so we can refer to it later.
            var startingVertex = ctx.Poly1[ctx.StartingIndex];
            var currentIndex = ctx.StartingIndex;
            subtract.Clear();

            // Trace direction
            var forward = true;

            do
            {
                // Add the current vertex to the final union
                subtract.Add(currentPoly[currentIndex]);

                foreach (var intersect in ctx.Intersections)
                {
                    // If the current point is an intersection point
                    if (currentPoly[currentIndex].Equals(intersect.IntersectionPoint, currentPoly.Epsilon))
                    {
                        // Make sure we want to swap polygons here.
                        var otherIndex = otherPoly.IndexOf(intersect.IntersectionPoint);

                        //Point2D otherVertex;
                        if (forward)
                        {
                            // If the next vertex, if we do swap, is inside the current polygon,
                            // then its safe to swap, otherwise, just carry on with the current poly.
                            var compareIndex = otherPoly.PreviousIndex(otherIndex);
                            var compareVertex = otherPoly[compareIndex];
                            bool bPointInPolygonAngle;
                            if (currentPolyVectorAngles[compareIndex] == -1)
                            {
                                bPointInPolygonAngle = PolygonOperationContext.PointInPolygonAngle(compareVertex, currentPoly);
                                currentPolyVectorAngles[compareIndex] = bPointInPolygonAngle ? 1 : 0;
                            }
                            else
                            {
                                bPointInPolygonAngle = (currentPolyVectorAngles[compareIndex] == 1);
                            }

                            if (bPointInPolygonAngle)
                            {
                                // switch polygons
                                if (currentPoly == ctx.Poly1)
                                {
                                    currentPoly = ctx.Poly2;
                                    currentPolyVectorAngles = ctx.Poly2VectorAngles;
                                    otherPoly = ctx.Poly1;
                                }
                                else
                                {
                                    currentPoly = ctx.Poly1;
                                    currentPolyVectorAngles = ctx.Poly1VectorAngles;
                                    otherPoly = ctx.Poly2;
                                }

                                // set currentIndex
                                currentIndex = otherIndex;

                                // Reverse direction
                                forward = false;

                                // Stop checking ctx.mIntersections for this point.
                                break;
                            }
                        }
                        else
                        {
                            // If the next vertex, if we do swap, is outside the current polygon,
                            // then its safe to swap, otherwise, just carry on with the current poly.
                            var compareIndex = otherPoly.NextIndex(otherIndex);
                            var compareVertex = otherPoly[compareIndex];
                            bool bPointInPolygonAngle;
                            if (currentPolyVectorAngles[compareIndex] == -1)
                            {
                                bPointInPolygonAngle = PolygonOperationContext.PointInPolygonAngle(compareVertex, currentPoly);
                                currentPolyVectorAngles[compareIndex] = bPointInPolygonAngle ? 1 : 0;
                            }
                            else
                            {
                                bPointInPolygonAngle = (currentPolyVectorAngles[compareIndex] == 1);
                            }

                            if (!bPointInPolygonAngle)
                            {
                                // switch polygons
                                if (currentPoly == ctx.Poly1)
                                {
                                    currentPoly = ctx.Poly2;
                                    currentPolyVectorAngles = ctx.Poly2VectorAngles;
                                    otherPoly = ctx.Poly1;
                                }
                                else
                                {
                                    currentPoly = ctx.Poly1;
                                    currentPolyVectorAngles = ctx.Poly1VectorAngles;
                                    otherPoly = ctx.Poly2;
                                }

                                // set currentIndex
                                currentIndex = otherIndex;

                                // Reverse direction
                                forward = true;

                                // Stop checking intersections for this point.
                                break;
                            }
                        }
                    }
                }

                currentIndex = forward ? currentPoly.NextIndex(currentIndex) : currentPoly.PreviousIndex(currentIndex);
            } while ((currentPoly[currentIndex].Equals(startingVertex)) && (subtract.Count <= (ctx.Poly1.Count + ctx.Poly2.Count)));


            // If the number of vertices in the union is more than the combined vertices
            // of the input polygons, then something is wrong and the algorithm will
            // loop forever. Luckily, we check for that.
            if (subtract.Count > (ctx.Poly1.Count + ctx.Poly2.Count))
            {
                ctx.Error = PolyUnionError.InfiniteLoop;
            }
        }

        /// <summary>
        /// Performs one or more polygon operations on the 2 provided polygons
        /// </summary>
        /// <param name="operations"></param>
        /// <param name="polygon1">The first polygon.</param>
        /// <param name="polygon2">The second polygon</param>
        /// <param name="results">The result of the polygon subtraction</param>
        /// <returns>error code</returns>
        public static PolyUnionError PolygonOperation(PolyOperation operations, Point2DList polygon1, Point2DList polygon2, out Dictionary<uint, Point2DList> results)
        {
            var ctx = new PolygonOperationContext();
            ctx.Init(operations, polygon1, polygon2);
            results = ctx.Output;
            return PolygonOperation(ctx);
        }


        public static PolyUnionError PolygonOperation(PolygonOperationContext ctx)
        {
            if ((ctx.Operations & PolyOperation.Union) == PolyOperation.Union)
            {
                PolygonUnionInternal(ctx);
            }
            if ((ctx.Operations & PolyOperation.Intersect) == PolyOperation.Intersect)
            {
                PolygonIntersectInternal(ctx);
            }
            if ((ctx.Operations & PolyOperation.Subtract) == PolyOperation.Subtract)
            {
                PolygonSubtractInternal(ctx);
            }

            return ctx.Error;
        }

        ///  <summary>
        ///  Trace the edge of a non-simple polygon and return a simple polygon.
        ///  
        /// Method:
        /// Start at vertex with minimum y (pick maximum x one if there are two).  
        /// We aim our "lastDir" vector at (1.0, 0)
        /// We look at the two rays going off from our start vertex, and follow whichever
        /// has the smallest angle (in -Pi . Pi) wrt lastDir ("rightest" turn)
        /// 
        /// Loop until we hit starting vertex:
        /// 
        /// We add our current vertex to the list.
        /// We check the seg from current vertex to next vertex for intersections
        ///   - if no intersections, follow to next vertex and continue
        ///   - if intersections, pick one with minimum distance
        ///     - if more than one, pick one with "rightest" next point (two possibilities for each)
        ///     
        ///  </summary>
        ///  <param name="verts"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static IEnumerable<Point2DList> SplitComplexPolygon(Point2DList verts, double epsilon)
        {
            var numVerts = verts.Count;
            var nodes = verts.Select(t => new SplitComplexPolygonNode(new Point2D(t.X, t.Y))).ToList();

            //Add base nodes (raw outline)
            for (var i = 0; i < verts.Count; ++i)
            {
                var iplus = (i == numVerts - 1) ? 0 : i + 1;
                var iminus = (i == 0) ? numVerts - 1 : i - 1;
                nodes[i].AddConnection(nodes[iplus]);
                nodes[i].AddConnection(nodes[iminus]);
            }
            var nNodes = nodes.Count;

            //Process intersection nodes - horribly inefficient
            var dirty = true;
            while (dirty)
            {
                dirty = false;
                for (var i = 0; !dirty && i < nNodes; ++i)
                {
                    for (var j = 0; !dirty && j < nodes[i].NumConnected; ++j)
                    {
                        for (var k = 0; !dirty && k < nNodes; ++k)
                        {
                            if (k == i || nodes[k] == nodes[i][j])
                            {
                                continue;
                            }
                            for (var l = 0; !dirty && l < nodes[k].NumConnected; ++l)
                            {
                                if (nodes[k][l] == nodes[i][j] || nodes[k][l] == nodes[i])
                                {
                                    continue;
                                }
                                //Check intersection
                                var intersectPt = new Point2D();
                                //if (counter > 100) printf("checking intersection: %d, %d, %d, %d\n",i,j,k,l);
                                var crosses = TriangulationUtil.LinesIntersect2D(nodes[i].Position,
                                                                                    nodes[i][j].Position,
                                                                                    nodes[k].Position,
                                                                                    nodes[k][l].Position,
                                                                                    true, true, true,
                                                                                    ref intersectPt,
                                                                                    epsilon);
                                if (crosses)
                                {
                                    /*if (counter > 100) {
                                        printf("Found crossing at %f, %f\n",intersectPt.x, intersectPt.y);
                                        printf("Locations: %f,%f - %f,%f | %f,%f - %f,%f\n",
                                                        nodes[i].position.x, nodes[i].position.y,
                                                        nodes[i].connected[j].position.x, nodes[i].connected[j].position.y,
                                                        nodes[k].position.x,nodes[k].position.y,
                                                        nodes[k].connected[l].position.x,nodes[k].connected[l].position.y);
                                        printf("Memory addresses: %d, %d, %d, %d\n",(int)&nodes[i],(int)nodes[i].connected[j],(int)&nodes[k],(int)nodes[k].connected[l]);
                                    }*/
                                    dirty = true;
                                    //Destroy and re-hook connections at crossing point
                                    var intersectionNode = new SplitComplexPolygonNode(intersectPt);
                                    var idx = nodes.IndexOf(intersectionNode);
                                    if (idx >= 0 && idx < nodes.Count)
                                    {
                                        intersectionNode = nodes[idx];
                                    }
                                    else
                                    {
                                        nodes.Add(intersectionNode);
                                        nNodes = nodes.Count;
                                    }

                                    var nodei = nodes[i];
                                    var connij = nodes[i][j];
                                    var nodek = nodes[k];
                                    var connkl = nodes[k][l];
                                    connij.RemoveConnection(nodei);
                                    nodei.RemoveConnection(connij);
                                    connkl.RemoveConnection(nodek);
                                    nodek.RemoveConnection(connkl);
                                    if (!intersectionNode.Position.Equals(nodei.Position, epsilon))
                                    {
                                        intersectionNode.AddConnection(nodei);
                                        nodei.AddConnection(intersectionNode);
                                    }
                                    if (!intersectionNode.Position.Equals(nodek.Position, epsilon))
                                    {
                                        intersectionNode.AddConnection(nodek);
                                        nodek.AddConnection(intersectionNode);
                                    }
                                    if (!intersectionNode.Position.Equals(connij.Position, epsilon))
                                    {
                                        intersectionNode.AddConnection(connij);
                                        connij.AddConnection(intersectionNode);
                                    }
                                    if (!intersectionNode.Position.Equals(connkl.Position, epsilon))
                                    {
                                        intersectionNode.AddConnection(connkl);
                                        connkl.AddConnection(intersectionNode);
                                    }
                                }
                            }
                        }
                    }
                }
                //if (counter > 100) printf("Counter: %d\n",counter);
            }

            //    /*
            //    // Debugging: check for connection consistency
            //    for (int i=0; i<nNodes; ++i) {
            //        int nConn = nodes[i].nConnected;
            //        for (int j=0; j<nConn; ++j) {
            //            if (nodes[i].connected[j].nConnected == 0) Assert(false);
            //            SplitComplexPolygonNode* connect = nodes[i].connected[j];
            //            bool found = false;
            //            for (int k=0; k<connect.nConnected; ++k) {
            //                if (connect.connected[k] == &nodes[i]) found = true;
            //            }
            //            Assert(found);
            //        }
            //    }*/

            //Collapse duplicate points
            var foundDupe = true;
            var nActive = nNodes;
            var epsilonSquared = epsilon * epsilon;
            while (foundDupe)
            {
                foundDupe = false;
                for (var i = 0; i < nNodes; ++i)
                {
                    if (nodes[i].NumConnected == 0)
                    {
                        continue;
                    }
                    for (var j = i + 1; j < nNodes; ++j)
                    {
                        if (nodes[j].NumConnected == 0)
                        {
                            continue;
                        }
                        var diff = nodes[i].Position - nodes[j].Position;
                        if (diff.MagnitudeSquared() <= epsilonSquared)
                        {
                            if (nActive <= 3)
                            {
                                throw new Exception("Eliminated so many duplicate points that resulting polygon has < 3 vertices!");
                            }

                            //printf("Found dupe, %d left\n",nActive);
                            --nActive;
                            foundDupe = true;
                            var inode = nodes[i];
                            var jnode = nodes[j];
                            //Move all of j's connections to i, and remove j
                            var njConn = jnode.NumConnected;
                            for (var k = 0; k < njConn; ++k)
                            {
                                var knode = jnode[k];
                                //Debug.Assert(knode != jnode);
                                if (knode != inode)
                                {
                                    inode.AddConnection(knode);
                                    knode.AddConnection(inode);
                                }
                                knode.RemoveConnection(jnode);
                                //printf("knode %d on node %d now has %d connections\n",k,j,knode.nConnected);
                                //printf("Found duplicate point.\n");
                            }
                            jnode.ClearConnections();   // to help with garbage collection
                            nodes.RemoveAt(j);
                            --nNodes;
                        }
                    }
                }
            }

            //    /*
            //    // Debugging: check for connection consistency
            //    for (int i=0; i<nNodes; ++i) {
            //        int nConn = nodes[i].nConnected;
            //        printf("Node %d has %d connections\n",i,nConn);
            //        for (int j=0; j<nConn; ++j) {
            //            if (nodes[i].connected[j].nConnected == 0) {
            //                printf("Problem with node %d connection at address %d\n",i,(int)(nodes[i].connected[j]));
            //                Assert(false);
            //            }
            //            SplitComplexPolygonNode* connect = nodes[i].connected[j];
            //            bool found = false;
            //            for (int k=0; k<connect.nConnected; ++k) {
            //                if (connect.connected[k] == &nodes[i]) found = true;
            //            }
            //            if (!found) printf("Connection %d (of %d) on node %d (of %d) did not have reciprocal connection.\n",j,nConn,i,nNodes);
            //            Assert(found);
            //        }
            //    }//*/

            //Now walk the edge of the list

            //Find node with minimum y value (max x if equal)
            var minY = double.MaxValue;
            var maxX = -double.MaxValue;
            var minYIndex = -1;
            for (var i = 0; i < nNodes; ++i)
            {
                if (nodes[i].Position.Y < minY && nodes[i].NumConnected > 1)
                {
                    minY = nodes[i].Position.Y;
                    minYIndex = i;
                    maxX = nodes[i].Position.X;
                }
                else if (nodes[i].Position.Y == minY && nodes[i].Position.X > maxX && nodes[i].NumConnected > 1)
                {
                    minYIndex = i;
                    maxX = nodes[i].Position.X;
                }
            }

            var origDir = new Point2D(1.0f, 0.0f);
            var resultVecs = new List<Point2D>();
            var currentNode = nodes[minYIndex];
            var startNode = currentNode;
            //Debug.Assert(currentNode.nConnected > 0);
            var nextNode = currentNode.GetRightestConnection(origDir);
            if (nextNode == null)
            {
                // Borked, clean up our mess and return
                return SplitComplexPolygonCleanup(verts);
            }

            resultVecs.Add(startNode.Position);
            while (nextNode != startNode)
            {
                if (resultVecs.Count > (4 * nNodes))
                {
                    //printf("%d, %d, %d\n",(int)startNode,(int)currentNode,(int)nextNode);
                    //printf("%f, %f . %f, %f\n",currentNode.position.x,currentNode.position.y, nextNode.position.x, nextNode.position.y);
                    //verts.printFormatted();
                    //printf("Dumping connection graph: \n");
                    //for (int i=0; i<nNodes; ++i)
                    //{
                    //    printf("nodex[%d] = %f; nodey[%d] = %f;\n",i,nodes[i].position.x,i,nodes[i].position.y);
                    //    printf("//connected to\n");
                    //    for (int j=0; j<nodes[i].nConnected; ++j)
                    //    {
                    //        printf("connx[%d][%d] = %f; conny[%d][%d] = %f;\n",i,j,nodes[i].connected[j].position.x, i,j,nodes[i].connected[j].position.y);
                    //    }
                    //}
                    //printf("Dumping results thus far: \n");
                    //for (int i=0; i<nResultVecs; ++i)
                    //{
                    //    printf("x[%d]=map(%f,-3,3,0,width); y[%d] = map(%f,-3,3,height,0);\n",i,resultVecs[i].x,i,resultVecs[i].y);
                    //}
                    //Debug.Assert(false);
                    //nodes should never be visited four times apiece (proof?), so we've probably hit a loop...crap
                    throw new Exception("nodes should never be visited four times apiece (proof?), so we've probably hit a loop...crap");
                }
                resultVecs.Add(nextNode.Position);
                var oldNode = currentNode;
                currentNode = nextNode;
                //printf("Old node connections = %d; address %d\n",oldNode.nConnected, (int)oldNode);
                //printf("Current node connections = %d; address %d\n",currentNode.nConnected, (int)currentNode);
                nextNode = currentNode.GetRightestConnection(oldNode);
                if (nextNode == null)
                {
                    return SplitComplexPolygonCleanup(resultVecs);
                }
                // There was a problem, so jump out of the loop and use whatever garbage we've generated so far
                //printf("nextNode address: %d\n",(int)nextNode);
            }

            if (resultVecs.Count < 1)
            {
                // Borked, clean up our mess and return
                return SplitComplexPolygonCleanup(verts);
            }
            else
            {
                return SplitComplexPolygonCleanup(resultVecs);
            }
        }


        private static IEnumerable<Point2DList> SplitComplexPolygonCleanup(IEnumerable<Point2D> orig)
        {
            var l = new List<Point2DList>();
            var origP2DL = new Point2DList();
            origP2DL.AddRange(orig);
            l.Add(origP2DL);
            var listIdx = 0;
            var numLists = l.Count;
            while (listIdx < numLists)
            {
                var numPoints = l[listIdx].Count;
                for (var i = 0; i < numPoints; ++i)
                {
                    for (var j = i + 1; j < numPoints; ++j)
                    {
                        if (l[listIdx][i].Equals(l[listIdx][j], origP2DL.Epsilon))
                        {
                            // found a self-intersection loop - split it off into it's own list
                            var numToRemove = j - i;
                            var newList = new Point2DList();
                            for (var k = i + 1; k <= j; ++k)
                            {
                                newList.Add(l[listIdx][k]);
                            }
                            l[listIdx].RemoveRange(i + 1, numToRemove);
                            l.Add(newList);
                            ++numLists;
                            numPoints -= numToRemove;
                            j = i + 1;
                        }
                    }
                }
                l[listIdx].Simplify();
                ++listIdx;
            }

            return l;
        }

    }


    public class EdgeIntersectInfo
    {
        public EdgeIntersectInfo(Edge edgeOne, Edge edgeTwo, Point2D intersectionPoint)
        {
            EdgeOne = edgeOne;
            EdgeTwo = edgeTwo;
            IntersectionPoint = intersectionPoint;
        }

        public Edge EdgeOne { get; private set; }
        public Edge EdgeTwo { get; private set; }
        public Point2D IntersectionPoint { get; private set; }
    }


    public class SplitComplexPolygonNode
    {
        /*
         * Given sines and cosines, tells if A's angle is less than B's on -Pi, Pi
         * (in other words, is A "righter" than B)
         */
        private readonly List<SplitComplexPolygonNode> _connected = new();
        private Point2D _position = null;

        public int NumConnected { get { return _connected.Count; } }
        public Point2D Position { get { return _position; } set { _position = value; } }
        public SplitComplexPolygonNode this[int index]
        {
            get { return _connected[index]; }
        }


        public SplitComplexPolygonNode(Point2D pos)
        {
            _position = pos;
        }


        public override bool Equals(object obj)
        {
            var pn = obj as SplitComplexPolygonNode;
            if (pn != null)
                return Equals(pn);

            return false;
        }


        public bool Equals(SplitComplexPolygonNode pn)
        {
            if ((object)pn == null)
            {
                return false;
            }
            if (_position == null || pn.Position == null)
            {
                return false;
            }

            return _position.Equals(pn.Position);
        }


        public override int GetHashCode()
        {
            return _position.GetHashCode();
        }

        public static bool operator ==(SplitComplexPolygonNode lhs, SplitComplexPolygonNode rhs) { if ((object)lhs != null) { return lhs.Equals(rhs); } if ((object)rhs == null) { return true; } else { return false; } }
        public static bool operator !=(SplitComplexPolygonNode lhs, SplitComplexPolygonNode rhs) { if ((object)lhs != null) { return !lhs.Equals(rhs); } if ((object)rhs == null) { return false; } else { return true; } }

        public override string ToString()
        {
            var sb = new StringBuilder(256);
            sb.Append(_position);
            sb.Append(" -> ");
            for (var i = 0; i < NumConnected; ++i)
            {
                if (i != 0)
                {
                    sb.Append(", ");
                }
                sb.Append(_connected[i].Position);
            }

            return sb.ToString();
        }


        private bool IsRighter(double sinA, double cosA, double sinB, double cosB)
        {
            if (sinA < 0)
            {
                if (sinB > 0 || cosA <= cosB)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (sinB < 0 || cosA <= cosB)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void AddConnection(SplitComplexPolygonNode toMe)
        {
            // Ignore duplicate additions
            if (!_connected.Contains(toMe) && toMe != this)
            {
                _connected.Add(toMe);
            }
        }

        public void RemoveConnection(SplitComplexPolygonNode fromMe)
        {
            _connected.Remove(fromMe);
        }

        public void ClearConnections()
        {
            _connected.Clear();
        }

        public SplitComplexPolygonNode GetRightestConnection(SplitComplexPolygonNode incoming)
        {
            if (NumConnected == 0)
            {
                throw new Exception("the connection graph is inconsistent");
            }
            if (NumConnected == 1)
            {
                //b2Assert(false);
                // Because of the possibility of collapsing nearby points,
                // we may end up with "spider legs" dangling off of a region.
                // The correct behavior here is to turn around.
                return incoming;
            }
            var inDir = _position - incoming._position;

            var inLength = inDir.Magnitude();
            inDir.Normalize();

            if (inLength <= MathUtil.EPSILON)
            {
                throw new Exception("Length too small");
            }

            SplitComplexPolygonNode result = null;
            for (var i = 0; i < NumConnected; ++i)
            {
                if (_connected[i] == incoming)
                {
                    continue;
                }
                var testDir = _connected[i]._position - _position;
                var testLengthSqr = testDir.MagnitudeSquared();
                testDir.Normalize();
                /*
                if (testLengthSqr < COLLAPSE_DIST_SQR) {
                    printf("Problem with connection %d\n",i);
                    printf("This node has %d connections\n",nConnected);
                    printf("That one has %d\n",connected[i].nConnected);
                    if (this == connected[i]) printf("This points at itself.\n");
                }*/
                if (testLengthSqr <= (MathUtil.EPSILON * MathUtil.EPSILON))
                {
                    throw new Exception("Length too small");
                }

                var myCos = Point2D.Dot(inDir, testDir);
                var mySin = Point2D.Cross(inDir, testDir);
                if (result != null)
                {
                    var resultDir = result._position - _position;
                    resultDir.Normalize();
                    var resCos = Point2D.Dot(inDir, resultDir);
                    var resSin = Point2D.Cross(inDir, resultDir);
                    if (IsRighter(mySin, myCos, resSin, resCos))
                    {
                        result = _connected[i];
                    }
                }
                else
                {
                    result = _connected[i];
                }
            }

            //if (B2_POLYGON_REPORT_ERRORS && result != null)
            //{
            //    printf("nConnected = %d\n", nConnected);
            //    for (int i = 0; i < nConnected; ++i)
            //    {
            //        printf("connected[%d] @ %d\n", i, (int)connected[i]);
            //    }
            //}
            //Debug.Assert(result != null);

            return result;
        }

        public SplitComplexPolygonNode GetRightestConnection(Point2D incomingDir)
        {
            var diff = _position - incomingDir;
            var temp = new SplitComplexPolygonNode(diff);
            var res = GetRightestConnection(temp);
            //Debug.Assert(res != null);
            return res;
        }
    }


    public class PolygonOperationContext
    {
        public PolygonUtil.PolyOperation Operations;
        public Point2DList OriginalPolygon2;
        public Point2DList Poly1;
        public Point2DList Poly2;
        public List<EdgeIntersectInfo> Intersections;
        public int StartingIndex;
        public PolygonUtil.PolyUnionError Error;
        public List<int> Poly1VectorAngles;
        public List<int> Poly2VectorAngles;
        public Dictionary<uint, Point2DList> Output = new();

        public Point2DList Union
        {
            get
            {
                Point2DList l;
                if (!Output.TryGetValue((uint)PolygonUtil.PolyOperation.Union, out l))
                {
                    l = new Point2DList();
                    Output.Add((uint)PolygonUtil.PolyOperation.Union, l);
                }

                return l;
            }
        }
        public Point2DList Intersect
        {
            get
            {
                Point2DList l;
                if (!Output.TryGetValue((uint)PolygonUtil.PolyOperation.Intersect, out l))
                {
                    l = new Point2DList();
                    Output.Add((uint)PolygonUtil.PolyOperation.Intersect, l);
                }

                return l;
            }
        }
        public Point2DList Subtract
        {
            get
            {
                Point2DList l;
                if (!Output.TryGetValue((uint)PolygonUtil.PolyOperation.Subtract, out l))
                {
                    l = new Point2DList();
                    Output.Add((uint)PolygonUtil.PolyOperation.Subtract, l);
                }

                return l;
            }
        }

        public void Clear()
        {
            Operations = PolygonUtil.PolyOperation.None;
            OriginalPolygon2 = null;
            Poly1 = null;
            Poly2 = null;
            Intersections = null;
            StartingIndex = -1;
            Error = PolygonUtil.PolyUnionError.None;
            Poly1VectorAngles = null;
            Poly2VectorAngles = null;
            Output = new Dictionary<uint, Point2DList>();
        }


        public bool Init(PolygonUtil.PolyOperation operations, Point2DList polygon1, Point2DList polygon2)
        {
            Clear();

            Operations = operations;
            OriginalPolygon2 = polygon2;

            // Make a copy of the polygons so that we dont modify the originals, and
            // force vertices to integer (pixel) values.
            Poly1 = new Point2DList(polygon1)
            {
                WindingOrder = Point2DList.WindingOrderType.Default
            };
            Poly2 = new Point2DList(polygon2)
            {
                WindingOrder = Point2DList.WindingOrderType.Default
            };

            // Find intersection points
            if (!VerticesIntersect(Poly1, Poly2, out Intersections))
            {
                // No intersections found - polygons do not overlap.
                Error = PolygonUtil.PolyUnionError.NoIntersections;
                return false;
            }

            // make sure edges that intersect more than once are updated to have correct start points
            var numIntersections = Intersections.Count;
            for (var i = 0; i < numIntersections; ++i)
            {
                for (var j = i + 1; j < numIntersections; ++j)
                {
                    if (Intersections[i].EdgeOne.EdgeStart.Equals(Intersections[j].EdgeOne.EdgeStart) &&
                        Intersections[i].EdgeOne.EdgeEnd.Equals(Intersections[j].EdgeOne.EdgeEnd))
                    {
                        Intersections[j].EdgeOne.EdgeStart = Intersections[i].IntersectionPoint;
                    }
                    if (Intersections[i].EdgeTwo.EdgeStart.Equals(Intersections[j].EdgeTwo.EdgeStart) &&
                        Intersections[i].EdgeTwo.EdgeEnd.Equals(Intersections[j].EdgeTwo.EdgeEnd))
                    {
                        Intersections[j].EdgeTwo.EdgeStart = Intersections[i].IntersectionPoint;
                    }
                }
            }

            // Add intersection points to original polygons, ignoring existing points.
            foreach (var intersect in Intersections)
            {
                if (!Poly1.Contains(intersect.IntersectionPoint))
                {
                    Poly1.Insert(Poly1.IndexOf(intersect.EdgeOne.EdgeStart) + 1, intersect.IntersectionPoint);
                }

                if (!Poly2.Contains(intersect.IntersectionPoint))
                {
                    Poly2.Insert(Poly2.IndexOf(intersect.EdgeTwo.EdgeStart) + 1, intersect.IntersectionPoint);
                }
            }

            Poly1VectorAngles = new List<int>();
            for (var i = 0; i < Poly2.Count; ++i)
            {
                Poly1VectorAngles.Add(-1);
            }
            Poly2VectorAngles = new List<int>();
            for (var i = 0; i < Poly1.Count; ++i)
            {
                Poly2VectorAngles.Add(-1);
            }

            // Find starting point on the edge of polygon1 that is outside of
            // the intersected area to begin polygon trace.
            var currentIndex = 0;
            do
            {
                var bPointInPolygonAngle = PointInPolygonAngle(Poly1[currentIndex], Poly2);
                Poly2VectorAngles[currentIndex] = bPointInPolygonAngle ? 1 : 0;
                if (bPointInPolygonAngle)
                {
                    StartingIndex = currentIndex;
                    break;
                }
                currentIndex = Poly1.NextIndex(currentIndex);
            } while (currentIndex != 0);

            // If we don't find a point on polygon1 thats outside of the
            // intersect area, the polygon1 must be inside of polygon2,
            // in which case, polygon2 IS the union of the two.
            if (StartingIndex == -1)
            {
                Error = PolygonUtil.PolyUnionError.Poly1InsidePoly2;
                return false;
            }

            return true;
        }


        /// <summary>
        /// Check and return polygon intersections
        /// </summary>
        /// <param name="polygon1"></param>
        /// <param name="polygon2"></param>
        /// <param name="intersections"></param>
        /// <returns></returns>
        private static bool VerticesIntersect(Point2DList polygon1, Point2DList polygon2, out List<EdgeIntersectInfo> intersections)
        {
            intersections = new List<EdgeIntersectInfo>();
            var epsilon = Math.Min(polygon1.Epsilon, polygon2.Epsilon);

            // Iterate through polygon1's edges
            for (var i = 0; i < polygon1.Count; i++)
            {
                // Get edge vertices
                var p1 = polygon1[i];
                var p2 = polygon1[polygon1.NextIndex(i)];

                // Get intersections between this edge and polygon2
                for (var j = 0; j < polygon2.Count; j++)
                {
                    var point = new Point2D();

                    var p3 = polygon2[j];
                    var p4 = polygon2[polygon2.NextIndex(j)];

                    // Check if the edges intersect
                    if (TriangulationUtil.LinesIntersect2D(p1, p2, p3, p4, ref point, epsilon))
                    {
                        // Rounding is not needed since we compare using an epsilon.
                        //// Here, we round the returned intersection point to its nearest whole number.
                        //// This prevents floating point anomolies where 99.9999-> is returned instead of 100.
                        //point = new Point2D((float)Math.Round(point.X, 0), (float)Math.Round(point.Y, 0));
                        // Record the intersection
                        intersections.Add(new EdgeIntersectInfo(new Edge(p1, p2), new Edge(p3, p4), point));
                    }
                }
            }

            // true if any intersections were found.
            return (intersections.Count > 0);
        }


        /// <summary>
        /// * ref: http://ozviz.wasp.uwa.edu.au/~pbourke/geometry/insidepoly/  - Solution 2 
        /// * Compute the sum of the angles made between the test point and each pair of points making up the polygon. 
        /// * If this sum is 2pi then the point is an interior point, if 0 then the point is an exterior point. 
        /// </summary>
        public static bool PointInPolygonAngle(Point2D point, Point2DList polygon)
        {
            double angle = 0;

            // Iterate through polygon's edges
            for (var i = 0; i < polygon.Count; i++)
            {
                // Get points
                var p1 = polygon[i] - point;
                var p2 = polygon[polygon.NextIndex(i)] - point;

                angle += VectorAngle(p1, p2);
            }

            if (Math.Abs(angle) < Math.PI)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Return the angle between two vectors on a plane
        /// The angle is from vector 1 to vector 2, positive anticlockwise
        /// The result is between -pi -> pi
        /// </summary>
        public static double VectorAngle(Point2D p1, Point2D p2)
        {
            var theta1 = Math.Atan2(p1.Y, p1.X);
            var theta2 = Math.Atan2(p2.Y, p2.X);
            var dtheta = theta2 - theta1;
            while (dtheta > Math.PI)
            {
                dtheta -= (2.0 * Math.PI);
            }
            while (dtheta < -Math.PI)
            {
                dtheta += (2.0 * Math.PI);
            }

            return (dtheta);
        }

    }

}