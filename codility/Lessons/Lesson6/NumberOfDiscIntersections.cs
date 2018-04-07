using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson6
{
    class NumberOfDiscIntersections : ITestee
    {
        int Solve(int[] A)
        {
            var n = A.Length;
            var total = 0;
            var accuIntersects = new int[n];
            for (var i = 0; i < n - 1; i++)
            {
                var a = A[i];
                var distToEnd = n - 1 - i;
                var rightCover = Math.Min(a, distToEnd);
                total += rightCover;
                if (total > 10000000) return -1;
                accuIntersects[i + rightCover]++;
            }

            var accu = 0;
            for (var i = 0; i < n; i++)
            {
                var a = accuIntersects[i];
                accuIntersects[i] += accu;
                accu += a;
            }

            for (var i = n - 1; i > 0; i--)
            {
                var end = accuIntersects[i - 1];
                var a = A[i];
                var start = i - 1 >= a ? accuIntersects[i - a - 1] : 0;
                var backCatch = end - start;
                total += backCatch;
                if (total > 10000000) return -1;
            }
            return total;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<NumberOfDiscIntersections>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 1, 5, 2, 1, 4, 0 }, 11);
                yield return CreateSingleInputSet(new[] { 1, 2147483647, 0 }, 2);
            }
        }
    }
}
