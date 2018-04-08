using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson10
{
    class Peaks : ITestee
    {
        int Solve(int[] A)
        {
            var n = A.Length;
            var peaks = new int[n / 2];
            var peaksCount = 0;
            bool check(int blockSize)
            {
                var pp = 0;
                var b = 0;
                for (b = 0; b < n; b += blockSize)
                {
                    var oldpp = pp;
                    for (; pp < peaksCount && peaks[pp] < b + blockSize; pp++)
                    {
                    }
                    if (oldpp == pp) break;
                }
                return b == n;
            }
            for (var i = 1; i < n - 1; i++)
            {
                var a = A[i];
                if (a > A[i - 1] && a > A[i + 1])
                {
                    peaks[peaksCount++] = i;
                    i++;
                }
            }
            if (peaksCount == 0) return 0;
            for (var bs = 3; bs <= n / 2; bs++)
            {
                var bn = n / bs;
                if (bn * bs != n) continue;
                if (peaksCount >= bn)
                {
                    if (check(bs))
                    {
                        return bn;
                    }
                }
            }
            return 1;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<Peaks>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 1, 2, 3, 4, 3, 4, 1, 2, 3, 4, 6, 2 }, 3);
                yield return CreateSingleInputSet(new[] { 5 }, 0);
            }
        }
    }
}
