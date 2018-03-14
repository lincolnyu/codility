using System.Collections.Generic;
using codility.Helpers;
using codility.TestFramework;

namespace codility.Lessons.Lesson2
{
    class CyclicRotation : ITestee
    {
        public int[] Solve(int[] A, int K)
        {
            int N = A.Length;
            if (N == 0) return A;
            K %= N;
            if (K == 0) return A;
            var C = GetCommonFactor(N, K);
            var M = N / C;
            for (int p = 0; p < C; p++)
            {
                int nextP;
                var last = A[p];
                for (var t = 0; t < M; p = nextP, t++)
                {
                    nextP = (p + K) % N;
                    var v = A[nextP];
                    A[nextP] = last;
                    last = v;
                }
            }
            return A;
        }

        private int GetCommonFactor(int big, int small)
        {
            while (true)
            {
                var q = big / small;
                var r = big - q * small;
                if (r == 0)
                {
                    return small;
                }
                big = small;
                small = r;
            }
        }

        public object Run(params object[] args) 
            => Solve((int[])args[0], (int)args[1]);

        public class Tester : BaseSelfTester<CyclicRotation>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(new[] { 3, 8, 9, 7, 6 }, 3, new[] { 9, 7, 6, 3, 8 });
                yield return Create2InputSet(new[] { 0, 0, 0 }, 1, new[] { 0, 0, 0 });
                yield return Create2InputSet(new[] { 1, 2, 3, 4 }, 4, new[] { 1, 2, 3, 4 });
                yield return Create2InputSet(new int[] { }, 1, new int[] { });
            }

            public override bool ResultsEqual(object a, object b)
                => ResultsHelper.ResultsEqual((int[])a, (int[])b);

            public override string ResultToString(object r)
                => ResultsHelper.ResultToString((int[])r);
        }
    }
}
