﻿using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson92
{
    class DiamondsCount : BaseTestee
    {
        struct Point :IComparable<Point>
        {
            public int X;
            public int Y;

            public int CompareTo(Point other)
            {
                var c = Y.CompareTo(other.Y);
                if (c!=0) return c;
                return X.CompareTo(other.X);
            }

            public static void Load(Point[] points, int[] X, int[] Y)
            {
                for (var i = 0; i < X.Length; i++)
                {
                    points[i] = new Point{X = X[i], Y = Y[i]};
                }
            }
        }

        class XComp : IComparer<Point>
        {
            public int Compare(Point x, Point y)
            {
                var c = x.X.CompareTo(y.X);
                if (c != 0) return c;
                return x.Y.CompareTo(y.Y);
            }
        }

        public int solution(int[] X, int[] Y)
        {
            var N = X.Length;

            var arrX = new Point[N];
            var arrY = new Point[N];
            Point.Load(arrX, X, Y);
            Point.Load(arrY, X, Y);
            Array.Sort(arrX, new XComp());
            Array.Sort(arrY);

            var midsX = new Point[N*N/2];
            var midsY = new Point[N*N/2];
            var px = 0;
            var py = 0;

            for (var i = 0; i < arrX.Length - 1; i++)
            {
                var pi = arrX[i];
                for (var j = i+1; j < arrX.Length && arrX[j].X == pi.X; j++)
                {
                    var sumy = pi.Y + arrX[j].Y;
                    if (sumy % 2 == 0)
                    {
                        midsY[py++] = new Point{X = pi.X, Y = sumy/2};
                    }
                }
            }

            for (var i = 0; i < arrY.Length - 1; i++)
            {
                var pi = arrY[i];
                var jend = i+1;

                for (; jend < arrY.Length && arrY[jend].Y == pi.Y; jend++);
                if (jend > i+1)
                {
                    var pbegin = px;
                    for ( ; i < jend - 1; i++)
                    {
                        pi = arrY[i];
                        for (var j = i+1; j < jend; j++)
                        {
                            var sumx = pi.X + arrY[j].X;
                            if (sumx % 2 == 0)
                            {
                                midsX[px++] = new Point{X = sumx/2, Y = pi.Y};
                            }
                        }
                    }
                    Array.Sort(midsX, pbegin, px-pbegin);
                }
            }

            Array.Sort(midsY, 0, py);

            var total = 0;
            for (int i = 0, j = 0; i < px && j < py; )
            {
                var mx = midsX[i];
                var my = midsY[j];
                var c = mx.CompareTo(my);
                if (c == 0)
                {
                    int cx = 0, cy = 0;
                    for (; i+cx < px && midsX[i+cx].CompareTo(mx) == 0 ; cx++);
                    for (; j+cy < py && midsY[j+cy].CompareTo(mx) == 0 ; cy++);
                    total += cx*cy;
                    i += cx;
                    j += cy;
                }
                else if (c < 0)
                {
                    i++;
                }
                else
                {
                    j++;
                }
            }

            return total;
        }
        public class Tester : BaseSelfTester<DiamondsCount>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateInputSet(2, new[] { 1, 1, 2, 2, 2, 3, 3 }, new[] { 3, 4, 1, 3, 5, 3, 4 });
                yield return CreateInputSet(0, new[] { 1, 2, 3, 3, 2, 1 }, new[] { 1, 1, 1, 2, 2, 2 });
            }
        }
    }
}
