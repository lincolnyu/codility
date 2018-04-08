using codility.TestFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace codility.Lessons.Lesson9
{
    class MaxProfit : ITestee
    {
        int Solve(int[] A)
        {
            var n = A.Length;
            var maxSlice = 0;
            var maxEnding = 0;
            for (var i = 1; i < n; i++)
            {
                var d = A[i] - A[i - 1];
                maxEnding = Math.Max(0, maxEnding + d);
                maxSlice = Math.Max(maxSlice, maxEnding);
            }
            return maxSlice;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<MaxProfit>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 23171, 21011, 21123, 21366, 21013, 21367 }, 356);
                yield return CreateSingleInputSet(new int[] { }, 0);
            }
        }
    }
}
