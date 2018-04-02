using codility.Helpers;
using codility.TestFramework;
using System.Collections.Generic;
using System.Linq;

namespace codility.Lessons.Lesson5
{
    class GenomicRangeQuery : ITestee
    {
        private int GetImpact(char c)
        {
            switch (c)
            {
                case 'A': return 1;
                case 'C': return 2;
                case 'G': return 3;
                default: /*T*/ return 4;
            }
        }

        private IEnumerable<int> Solve(string S, int[] P, int[] Q)
        {
            var n = S.Length;
            var m = P.Length;
            var a1 = new int[n];
            var a2 = new int[n];
            var a3 = new int[n];
            a1[0] = a2[0] = a3[0] = 0;
            var lastImpact = GetImpact(S[0]);
            for (var i = 1; i < n; i++)
            {
                var s = S[i];
                if (s == 'A')
                {
                    a1[i] = a1[i - 1] + 1;
                    a2[i] = a2[i - 1] + 1;
                    a3[i] = a3[i - 1] + 1;
                    lastImpact = 1;
                }
                else if (s == 'C')
                {
                    a1[i] = a1[i - 1];
                    if (lastImpact == 1) a1[i]++;
                    a2[i] = a2[i - 1] + 1;
                    a3[i] = a3[i - 1] + 1;
                    lastImpact = 2;
                }
                else if (s == 'G')
                {
                    a1[i] = a1[i - 1];
                    a2[i] = a2[i - 1];
                    if (lastImpact <= 2)
                    {
                        a2[i]++;
                        if (lastImpact == 1) a1[i]++;
                    }
                    a3[i] = a3[i - 1] + 1;
                    lastImpact = 3;
                }
                else // s == 'T'
                {
                    a1[i] = a1[i - 1];
                    a2[i] = a2[i - 1];
                    a3[i] = a3[i - 1];
                    if (lastImpact <= 3)
                    {
                        a3[i]++;
                        if (lastImpact <= 2)
                        {
                            a2[i]++;
                            if (lastImpact == 1) a1[i]++;
                        }
                    }
                    lastImpact = 4;
                }
            }
            
            for (var j = 0; j < m; j++)
            {
                var p = P[j];
                var q = Q[j];
                if (p == q)
                {
                    yield return GetImpact(S[p]);
                }
                else if (a1[p] != a1[q]) yield return 1;
                else if (a2[p] != a2[q]) yield return 2;
                else if (a3[p] != a3[q]) yield return 3;
                else yield return 4;
            }
        }

        public object Run(params object[] args)
            => Solve((string)args[0], (int[])args[1], (int[])args[2]).ToArray();

        public class Tester : BaseSelfTester<GenomicRangeQuery>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create3InputSet("CAGCCTA", new[] { 2, 5, 0 }, new[] { 4, 5, 6 }, new[] { 2, 4, 1 });
                yield return Create3InputSet("A", new[] { 0 }, new[] { 0 }, new[] { 1 });
                yield return Create3InputSet("AC", new[] { 0, 0, 1 }, new[] { 0, 1, 1 }, new[] { 1, 1, 2 });
            }

            public override bool ResultsEqual(object a, object b)
                => ResultsHelper.ResultsEqual((int[])a, (int[])b);

            public override string ResultToString(object r)
                => ResultsHelper.ResultToString((int[])r);
        }
    }
}
