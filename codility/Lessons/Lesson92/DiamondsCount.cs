using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson92
{
    class DiamondsCount : BaseTestee
    {
#if CLASS_POINT
        class 
#else
        struct
#endif
        Point
        {
            public int X;
            public int Y;
#if LINEARITY_CHECK && CLASS_POINT
            public int IX;
            public int IY;
#endif
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

#if LINEARITY_CHECK
        bool IsLinear(Point p1, Point p2)
            => Math.Abs(p2.IX - p1.IX) == Math.Abs(p2.IY - p1.IY);
#endif

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
            for (var i = 0; i < N; i++)
            {
                arrY[i] = arrX[i];
            }

            Array.Sort(arrX, xcomp);
            Array.Sort(arrY, ycomp);
#if LINEARITY_CHECK
            for (var i = 0; i < N; i++)
            {
                arrX[i].IX = i;
                arrY[i].IY = i;
            }
#endif

            var midsX = new Point[N * N / 2];
            var midsY = new Point[N * N / 2];
            var px = 0;
            var py = 0;

            var llx = new LinkedList<Tuple<int, int>>();
            var lly = new LinkedList<Tuple<int, int>>();

            for (var i = 0; i < arrX.Length - 1; i++)
            {
                var pi = arrX[i];
                var jend = i;

                FibbInc(ref jend, t => t < arrX.Length && arrX[t].X == pi.X);
#if BOUNARY_EXCLUSION
                if (arrX[i].X == arrX[0].X)
                {
                    i = jend - 1;
                    continue;
                }
                if (arrX[i].X == arrX[arrX.Length - 1].X)
                {
                    break;
                }
#endif
                if (jend > i+1)
                {
                    var pbegin = py;
                    for (; i < jend-1; i++)
                    {
                        pi = arrX[i];
                        var parityi = pi.Y % 2;
                        for (var j = i + 1; j < jend; j++)
                        {
                            if (arrX[j].Y % 2 == parityi
#if LINEARITY_CHECK
                                && !IsLinear(arrX[j], arrX[i])
#endif
                                )
                            {
                                midsY[py++] = new Point { X = pi.X, Y = (pi.Y + arrX[j].Y) / 2 };
                            }
                        }
                    }
                    if (py > pbegin + 1)
                    {
                        lly.AddLast(new Tuple<int, int>(pbegin, py - pbegin));
                    }
                }
            }
            
            for (var i = 0; i < arrY.Length - 1; i++)
            {
                var pi = arrY[i];
                var jend = i;

                FibbInc(ref jend, t => t < arrY.Length && arrY[t].Y == pi.Y);
#if BOUNARY_EXCLUSION
                if (arrY[i].Y == arrY[0].Y)
                {
                    i = jend - 1;
                    continue;
                }
                if (arrY[i].Y == arrY[arrY.Length - 1].Y)
                {
                    break;
                }
#endif
                if (jend > i + 1)
                {
                    var pbegin = px;
                    for (; i < jend - 1; i++)
                    {
                        pi = arrY[i];
                        var parityi = pi.X % 2;
                        for (var j = i + 1; j < jend; j++)
                        {
                            if (arrY[j].X % 2 == parityi
#if LINEARITY_CHECK
                                && !IsLinear(arrY[j], arrY[i])
#endif
                                )
                            {
                                midsX[px++] = new Point { X = (pi.X + arrY[j].X) / 2, Y = pi.Y };
                            }
                        }
                    }
                    if (px > pbegin + 1)
                    {
                        llx.AddLast(new Tuple<int, int>(pbegin, px - pbegin));
                    }
                }
            }        

            IComparer<Point> sel;
            if (px < py)
            {
                sel = xcomp;
                Array.Sort(midsX, 0, px, xcomp);
                foreach (var t in lly)
                {
                    Array.Sort(midsY, t.Item1, t.Item2, xcomp);
                }
            }
            else
            {
                sel = ycomp;
                Array.Sort(midsY, 0, py, ycomp);
                foreach(var t in llx)
                {
                    Array.Sort(midsX, t.Item1, t.Item2, xcomp);
                }
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
                    FibbInc(ref j, t => t < py && sel.Compare(midsY[t], my) == 0);
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
