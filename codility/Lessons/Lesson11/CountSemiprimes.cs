using codility.Helpers;
using codility.TestFramework;
using System.Collections.Generic;

namespace codility.Lessons.Lesson11
{
    class CountSemiprimes : ITestee
    {
        int[] Solve(int N, int[] P, int[] Q)
        {
            var F = new int[N + 1];
            for (var i = 2; i * i <= N; i++)
            {
                if (F[i] == 0)
                {
                    for (var k = i * i; k <= N; k += i)
                    {
                        F[k] = 2;
                    }
                }
            }
            for (var i = 2; i * i <= N; i++)
            {
                if (F[i] == 0)
                {
                    var j = i;
                    for (var k = i * i; k <= N; k += i, j++)
                    {
                        if (F[j] == 0) F[k] = 1;
                    }
                }
            }
            var acc = 0;
            for (var i = 1; i <= N; i++)
            {
                if (F[i] == 1)
                {
                    acc++;
                }
                F[i] = acc;
            }
            var m = P.Length;
            var res = new int[m];
            for (var i = 0; i < m; i++)
            {
                var p = P[i];
                var q = Q[i];
                res[i] = F[q] - F[p-1];
            }
            return res;
        }

        public object Run(params object[] args)
            => Solve((int)args[0], (int[])args[1], (int[])args[2]);


        public class Tester : BaseSelfTester<CountSemiprimes>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create3InputSet(26, new[] { 1, 4, 16 }, new[] { 26, 10, 20 }, new[] { 10, 4, 0 });
            }

            public override bool ResultsEqual(object a, object b)
                => ResultsHelper.ResultsEqual((int[])a, (int[])b);

            public override string ResultToString(object r)
                => ResultsHelper.ResultToString((int[])r);
        }
    }
}
