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

            var midsX = new Point[N*N/2];
            var midsY = new Point[N*N/2];
            var px = 0;
            var py = 0;

            for (var i = 0; i < N-1; i++)
            {
                var pix = X[i];
                var piy = Y[i];
                for (var j = i+1; j < N; j++)
                {
                    var pjx = X[j];
                    var pjy = Y[j];
                    if (piy == pjy)
                    {
                        var sumx = pix + pjx;
                        if (sumx %2 ==0)
                        {
                            midsX[px++] = new Point{X = sumx/2, Y = piy};
                        }
                    }
                    else if (pix == pjx)
                    {
                        var sumy = piy + pjy;
                        if (sumy %2 ==0)
                        {
                            midsY[py++] = new Point{X = pix, Y = sumy/2};
                        }
                    }
                }
            }

            Array.Sort(midsX, 0, px);
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
