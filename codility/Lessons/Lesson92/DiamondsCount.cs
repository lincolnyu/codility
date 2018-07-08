using codility.TestFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace codility.Lessons.Lesson92
{
    class DiamondsCount : BaseTestee
    {
        class Point :IComparable<Point>
        {
            public int X;
            public int Y;

            public int CompareTo(Point other)
            {
                var c = Y.CompareTo(other.Y);
                if (c!=0) return c;
                return X.CompareTo(other.X);
            }

            public static IEnumerable<Point> Load(int[] X, int[] Y)
            {
                for (var i = 0; i < X.Length; i++)
                {
                    yield return new Point{X = X[i], Y = Y[i]};
                }
            }
        }

        public int solution(int[] X, int[] Y)
        {
            var N = X.Length;

            var points = Point.Load(X, Y).ToArray();

            var midsX = new List<Point>();
            var midsY = new List<Point>();

            for (var i = 0; i < points.Length-1; i++)
            {
                var pi = points[i];
                for (var j = i+1; j < points.Length; j++)
                {
                    var pj = points[j];
                    if (pi.Y == pj.Y)
                    {
                        var sumx = pi.X + pj.X;
                        if (sumx %2 ==0)
                        {
                            midsX.Add(new Point{X = sumx/2, Y = pi.Y});
                        }
                    }
                    else if (pi.X == pj.X)
                    {
                        var sumy = pi.Y + pj.Y;
                        if (sumy %2 ==0)
                        {
                            midsY.Add(new Point{X = pi.X, Y = sumy/2});
                        }
                    }
                }
            }

            midsX.Sort();
            midsY.Sort();

            var total = 0;
            for (int i = 0, j = 0; i < midsX.Count && j < midsY.Count; )
            {
                var mx = midsX[i];
                var my = midsY[j];
                var c = mx.CompareTo(my);
                if (c == 0)
                {
                    int cx = 0, cy = 0;
                    for (; i+cx < midsX.Count && midsX[i+cx].CompareTo(mx) == 0 ; cx++);
                    for (; j+cy < midsY.Count && midsY[j+cy].CompareTo(mx) == 0 ; cy++);
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
