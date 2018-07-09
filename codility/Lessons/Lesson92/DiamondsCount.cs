using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson92
{
    class DiamondsCount : BaseTestee
    {
        struct Point
        {
            public int X;
            public int Y;

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

        class YComp : IComparer<Point>
        {
            public int Compare(Point x, Point y)
            {
                var c = x.Y.CompareTo(y.Y);
                if (c != 0) return c;
                return x.X.CompareTo(y.X);
            }
        }

        void FibbInc(ref int i, Predicate<int> pred)
        {
            var a = 1;
            var b = 1;
            while (true)
            {
                i += b;
                if (!pred(i))
                {
                    if (b == 1)
                    {
                        break;
                    }
                    else
                    {
                        i -= b;
                        a = 1;
                        b = 1;
                        continue;
                    }
                }
                var c = a + b;
                a = b;
                b = c;
            }
        }

        public int solution(int[] X, int[] Y)
        {
            var xcomp = new XComp();
            var ycomp = new YComp();

            var N = X.Length;
            var arrX = new Point[N];
            var arrY = new Point[N];
            Point.Load(arrX, X, Y);
            Point.Load(arrY, X, Y);
            Array.Sort(arrX, xcomp);
            Array.Sort(arrY, ycomp);

            var midsX = new Point[N * N / 2];
            var midsY = new Point[N * N / 2];
            var px = 0;
            var py = 0;

            for (var i = 0; i < arrX.Length - 1; i++)
            {
                var pi = arrX[i];
                var jend = i;

                FibbInc(ref jend, t => t < arrX.Length && arrX[t].X == pi.X);
                if (jend > i+1)
                {
                    var pbegin = py;
                    for (; i < jend-1; i++)
                    {
                        pi = arrX[i];
                        for (var j = i + 1; j < jend; j++)
                        {
                            var sumy = pi.Y + arrX[j].Y;
                            if (sumy % 2 == 0)
                            {
                                midsY[py++] = new Point { X = pi.X, Y = sumy / 2 };
                            }
                        }
                    }
                    Array.Sort(midsY, pbegin, py - pbegin, xcomp);
                }
            }

            for (var i = 0; i < arrY.Length - 1; i++)
            {
                var pi = arrY[i];
                var jend = i;

                FibbInc(ref jend, t => t < arrY.Length && arrY[t].Y == pi.Y);
                if (jend > i + 1)
                {
                    var pbegin = px;
                    for (; i < jend - 1; i++)
                    {
                        pi = arrY[i];
                        for (var j = i + 1; j < jend; j++)
                        {
                            var sumx = pi.X + arrY[j].X;
                            if (sumx % 2 == 0)
                            {
                                midsX[px++] = new Point { X = sumx / 2, Y = pi.Y };
                            }
                        }
                    }
                    Array.Sort(midsX, pbegin, px - pbegin, ycomp);
                }
            }

            IComparer<Point> sel;
            if (px < py)
            {
                sel = xcomp;
                Array.Sort(midsX, 0, px, xcomp);
            }
            else
            {
                sel = ycomp;
                Array.Sort(midsY, 0, py, ycomp);
            }

            var total = 0;
            for (int i = 0, j = 0; i < px && j < py;)
            {
                var mx = midsX[i];
                var my = midsY[j];
                var c = sel.Compare(mx, my);
                if (c == 0)
                {
                    var oldi = i;
                    var oldj = j;
                    FibbInc(ref i, t => t < px && sel.Compare(midsX[t], mx) == 0);
                    FibbInc(ref j, t => t < py && sel.Compare(midsY[t], mx) == 0);
                    total += (i - oldi) * (j - oldj);
                }
                else if (c < 0)
                {
                    FibbInc(ref i, t => t < px && sel.Compare(midsX[t], my) < 0);
                }
                else
                {
                    FibbInc(ref j, t => t < py && sel.Compare(mx, midsY[t]) > 0);
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
