#define WORM

using codility.TestFramework;
using System;
using System.Linq;
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
#if WORM
            public int Count;
#endif
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

        private IEnumerable<int> Worm(IList<Point> list, Func<Point, int> access,
            int start, Predicate<int> pred)
        {
            var b = start;

            var i = b;
            var j = i+1;
            var curr = 0;
            var curr2 = 0;
            bool found = false;
            for (; pred(j); j++)
            {
                var vi = access(list[i]);
                var vj = access(list[j]);
                if (vi % 2 == vj % 2)
                {
                    curr2 = vi + vj;
                    curr = curr2 / 2;
                    found = true;
                    yield return curr;
                    break;
                }
            }

            if (!found)
            {
                if (j <= i + 2)
                {
                    yield break;
                }
                i++;
                j = i + 1;
                curr2 = access(list[i]) + access(list[j]);
                curr = curr2 / 2;
                yield return curr;
            }

            while (true)
            {
                var go = 0;
                int a1 = 0;
                if (pred(j + 1))
                {
                    go = 1;
                    a1 = access(list[j + 1]) - access(list[j]);
                }
                if (i + 1 < j && (go == 0 || access(list[i + 1])-access(list[i]) < a1))
                {
                    go = 2;
                }

                if (go==1)
                {
                    j++;
                    var di = 0;
                    System.Diagnostics.Debug.Assert(i >= b && access(list[j]) + access(list[i]) >= curr2);
                    FibbInc(ref di, x => i >= b + x && access(list[j]) + access(list[i - x]) >= curr2);
                    di--;
                    i -= di;
                }
                else if (go==2)
                {
                    i++;
                    b = i;
                    var dj = 0;
                    System.Diagnostics.Debug.Assert(j > i && access(list[j]) + access(list[i]) >= curr2);
                    FibbInc(ref dj, x => j > i + x && access(list[j - x]) + access(list[i]) >= curr2);
                    dj--;
                    j -= dj;
                }
                else
                {
                    break;
                }

                var vi = access(list[i]);
                var vj = access(list[j]);
                if (vi % 2 == vj % 2)
                {
                    curr2 = vi + vj;
                    curr = curr2 / 2;
                    yield return curr;
                }
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

#if !WORM
            var llx = new LinkedList<Tuple<int, int>>();
            var lly = new LinkedList<Tuple<int, int>>();
#endif
            for (var i = 0; i < arrX.Length - 1;
#if !WORM
                i++
#endif
                )
            {
                var pi = arrX[i];

#if WORM
                var mids = Worm(arrX, t=>t.Y, i,
                    t =>
                    {
                        var res = t < arrX.Length && arrX[t].X == pi.X;
                        if (!res)
                        {
                            i = t;
                        }
                        return res;
                    });
                int? last = null;
                var count = 0;
                foreach (var mid in mids)
                {
                    if (last.HasValue && mid == last.Value)
                    {
                        count++;
                    }
                    else
                    {
                        if (count > 0)
                        {
                            midsY[py++] = new Point { X = pi.X, Y = last.Value, Count = count };
                        }
                        last = mid;
                        count = 1;
                    }
                }
                if (count > 0)
                {
                    midsY[py++] = new Point { X = pi.X, Y = last.Value, Count = count };
                }
#else
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
#endif
            }

            for (var i = 0; i < arrY.Length - 1;
#if !WORM
                i++
#endif
                )
            {
                var pi = arrY[i];

#if WORM
                var mids = Worm(arrY, t => t.X, i, t =>
                      {
                          var res = t < arrX.Length && arrY[t].Y == pi.Y;
                          if (!res)
                          {
                              i = t;
                          }
                          return res;
                      });
                int? last = null;
                var count = 0;
                foreach (var mid in mids)
                {
                    if (last.HasValue && mid == last.Value)
                    {
                        count++;
                    }
                    else
                    {
                        if (count > 0)
                        {
                            midsX[px++] = new Point { X = last.Value, Y = pi.Y, Count = count };
                        }
                        last = mid;
                        count = 1;
                    }
                }
                if (count > 0)
                {
                    midsX[px++] = new Point { X = last.Value, Y = pi.Y, Count = count };
                }
#else
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
#endif
            }        

            IComparer<Point> sel;
            if (px < py)
            {
                sel = xcomp;
                Array.Sort(midsX, 0, px, xcomp);
#if !WORM
                foreach (var t in lly)
                {
                    Array.Sort(midsY, t.Item1, t.Item2, xcomp);
                }
#endif
            }
            else
            {
                sel = ycomp;
                Array.Sort(midsY, 0, py, ycomp);
#if !WORM
                foreach(var t in llx)
                {
                    Array.Sort(midsX, t.Item1, t.Item2, xcomp);
                }
#endif
            }

            var total = 0;
            for (int i = 0, j = 0; i < px && j < py;)
            {
                var mx = midsX[i];
                var my = midsY[j];
                var c = sel.Compare(mx, my);

                if (c == 0)
                {
#if WORM
                    total += mx.Count * my.Count;
                    i++;
                    j++;
#else

                    var oldi = i;
                    var oldj = j;
                    FibbInc(ref i, t => t < px && sel.Compare(midsX[t], mx) == 0);
                    FibbInc(ref j, t => t < py && sel.Compare(midsY[t], my) == 0);
                    total += (i - oldi) * (j - oldj);
#endif
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

        public class Profiler : BaseSelfProfiler<DiamondsCount>
        {
            private IEnumerable<int> GenerateLine(int n, Random rand, int dyn = 5)
            {
                var c = 0;
                for (var i = 0; i < n; i++)
                {
                    yield return c;
                    var d = rand.Next(5) + 1;
                    c += d;
                }
            }

            private IEnumerable<(int, int)> GenerateSpotsLessHLine(int n, Random rand, int minx, int miny, int maxx, int maxy)
            {
                for (var i = 0; i < n; i++)
                {
                    var x = rand.Next(minx, maxx + 1);
                    do
                    {
                        var y = rand.Next(miny, maxy + 1);
                        if (y == 0) continue;
                        yield return (x, y);
                    } while (false);
                }
            }

            (int[], int[]) ShuffleAndSplit(IList<(int, int)> src, Random rand, double shuffleRate = 0.5)
            {
                var n = src.Count;
                var shuffleCount = (int)Math.Ceiling(n * shuffleRate);
                var p = 0;
                for (var i = 0; i < shuffleCount; i++)
                {
                    var d = rand.Next(n);
                    var t = src[p];
                    var dstp = (p + d)%n;
                    src[p] = src[dstp];
                    src[dstp] = t;
                    p = dstp;
                }
                var x = new int[n];
                var y = new int[n];
                for (var i = 0; i < n; i++)
                {
                    x[i] = src[i].Item1;
                    y[i] = src[i].Item2;
                }
                return (x, y);
            }

            public override IEnumerable<BaseTester.TestSet> GetProfilingTestSets()
            {
                const int lineCount = 1300;
                const int spotsCount = 700;
                var rand = new Random(123);
                var lineMain = GenerateLine(lineCount, rand).ToArray();
                var lineMax = lineMain[lineMain.Length-1];
                var boxMinX = -10;
                var boxMaxX = lineMax + 10;
                var boxMinY = - 5;
                var boxMaxY = 5;
                var spots = GenerateSpotsLessHLine(spotsCount, rand, boxMinX, boxMinY, boxMaxX, boxMaxY);

                var input = ShuffleAndSplit(lineMain.Select(x=>(x,0)).Concat(spots).ToArray(), rand);
                yield return BaseTester.CreateInputSet(null, input.Item1, input.Item2);
            }
        }
    }
}
