using codility.TestFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace codility.Lessons.Lesson92
{
    class DiamondsCount : BaseTestee
    {
        public int solution(int[] X, int[] Y)
        {
            var N = X.Length;
            var xs = X.Distinct().OrderBy(x => x).ToArray();
            var ys = Y.Distinct().OrderBy(x => x).ToArray();
            var map = new bool[xs.Length, ys.Length];
            var posArray = new Tuple<int, int>[N];

            for (var i = 0; i < N; i++)
            {
                var x = X[i];
                var y = Y[i];
                var ix = Array.BinarySearch(xs, x);
                var iy = Array.BinarySearch(ys, y);
                map[ix, iy] = true;
                posArray[i] = new Tuple<int, int>(ix, iy);
            }

            var dxs = new List<Tuple<int,int>>[xs.Length];
            for (var i = 1; i < xs.Length-1; i++)
            {
                for (int j1 = i-1, j2 =i+1; j1 >= 0 && j2 < xs.Length; )
                {
                    var d1 = xs[i] - xs[j1];
                    var d2 = xs[j2] - xs[i];
                    if (d1==d2)
                    {
                        if (dxs[i] == null) dxs[i] = new List<Tuple<int, int>>();
                        dxs[i].Add(new Tuple<int, int>(j1, j2));
                        j1--;
                        j2++;
                    }
                    else if (d1 < d2)
                    {
                        j1--;
                    }
                    else
                    {
                        j2++;
                    }
                }
            }

            var total = 0;
            for (var i = 0; i < N; i++)
            {
                var t = posArray[i];
                var ix = t.Item1;
                var iy = t.Item2;
                // x, y as top vertex
                if (iy >= ys.Length - 2) continue;
                if (ix == 0 || ix == xs.Length - 1) continue;
                if (dxs[ix] == null || dxs[ix].Count == 0) continue;
                
                for (var py = iy + 1; py < ys.Length; py++)
                {
                    if (map[ix, py])
                    {
                        var sumy = ys[iy] + ys[py];
                        if (sumy % 2 != 0) continue;
                        var my = sumy / 2;
                        var imy = Array.BinarySearch(ys, my);
                        if (imy < 0) continue;
                        foreach (var dx in dxs[ix])
                        {
                            if (map[dx.Item1, imy] && map[dx.Item2, imy])
                            {
                                total++;
                            }
                        }
                    }
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
