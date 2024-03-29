﻿using System;
using System.Collections.Generic;

namespace Poly2Tri.Utility
{
    public class MathUtil
    {
        public const double EPSILON = 1e-12;

        public static bool AreValuesEqual(double val1, double val2, double tolerance = EPSILON)
        {
            if (val1 >= (val2 - tolerance) && val1 <= (val2 + tolerance))
            {
                return true;
            }

            return false;
        }


        public static bool IsValueBetween(double val, double min, double max, double tolerance)
        {
            if (min > max)
            {
                var tmp = min;
                min = max;
                max = tmp;
            }
            if ((val + tolerance) >= min && (val - tolerance) <= max)
            {
                return true;
            }

            return false;
        }


        public static double RoundWithPrecision(double f, double precision)
        {
            if (precision < 0.0)
            {
                return f;
            }

            var mul = Math.Pow(10.0, precision);
            var fTemp = Math.Floor(f * mul) / mul;

            return fTemp;
        }


        public static double Clamp(double a, double low, double high)
        {
            return Math.Max(low, Math.Min(a, high));
        }


        public static void Swap<T>(ref T a, ref T b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }


        public static uint Jenkins32Hash(IEnumerable<byte> data, uint nInitialValue)
        {
            foreach (var b in data)
            {
                nInitialValue += b;
                nInitialValue += (nInitialValue << 10);
                nInitialValue += (nInitialValue >> 6);
            }

            nInitialValue += (nInitialValue << 3);
            nInitialValue ^= (nInitialValue >> 11);
            nInitialValue += (nInitialValue << 15);

            return nInitialValue;
        }
    }
}