using codility.TestFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace codility.Lessons.Lesson92
{
    class SocksLaundering : ITestee
    {
        // return list of (value, count)
        private IEnumerable<Tuple<int, int>> Count(IEnumerable<int> cs)
        {
            var count = 0;
            int? last = null;
            foreach (var c in cs)
            {
                if (last != null && c == last.Value)
                {
                    count++;
                }
                else
                {
                    if (last != null)
                    {
                        yield return new Tuple<int, int>(last.Value, count);
                    }
                    count = 1;
                }
                last = c;
            }
            if (count > 0)
            {
                yield return new Tuple<int, int>(last.Value, count);
            }
        }

        private IEnumerable<int> Generate(int[] source, bool[] removed)
        {
            for (var i = 0; i < source.Length; i++)
            {
                if (!removed[i])
                {
                    yield return source[i];
                }
            }
        }

        public int solution(int K, int[] C, int[] D)
        {
            var cs = C.ToArray();
            Array.Sort(cs);
            var odds = new List<int>();
            var total = 0;
            foreach (var cp in Count(cs))
            {
                var c = cp.Item1;
                var count = cp.Item2;
                if (count % 2 == 1)
                {
                    odds.Add(c);
                }
                total += count / 2;
            }
            var ds = D.ToArray();
            var removed = new bool[ds.Length];
            Array.Sort(ds);
            var i = 0;
            var j = 0;
            var cd = K;
            for (; i < odds.Count && cd > 0; i++)
            {
                var c = odds[i];
                for (; j < ds.Length && ds[j] < c; j++) ;
                if (j < ds.Length && ds[j] == c)
                {
                    removed[j] = true;
                    cd--;
                    total++;
                }
            }
            if (cd > 1)
            {
                var cdpair = cd / 2;
                var dg =Generate(ds, removed);
                foreach (var dp in Count(dg))
                {
                    var d = dp.Item1;
                    var count = dp.Item2;
                    if (count > 1)
                    {
                        var co = count / 2;
                        if (cdpair > co)
                        {
                            cdpair -= co;
                            total += co;
                        }
                        else
                        {
                            total += cdpair;
                            break;
                        }
                    }
                }
            }
            return total;
        }

        public object Run(params object[] args)
            => solution((int)args[0], (int[])args[1], (int[])args[2]);

        public class Tester : BaseSelfTester<SocksLaundering>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create3InputSet(2, new[] { 1 }, new[] { 3, 2, 5, 5 }, 1);
                yield return Create3InputSet(2, new[] { 1, 2, 1, 1 }, new[] { 1, 4, 3, 2, 4 }, 3);
            }
        }
    }
}
