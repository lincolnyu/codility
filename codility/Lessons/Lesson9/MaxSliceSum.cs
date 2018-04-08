using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson9
{
    class MaxSliceSum : ITestee
    {
        int Solve(int[] A)
        {
            var (maxSlice, maxEnding) = (0, 0);
            var maxElement = int.MinValue;
            foreach (var a in A)
            {
                maxEnding = Math.Max(maxEnding + a, 0);
                maxSlice = Math.Max(maxSlice, maxEnding);
                if (a > maxElement) maxElement = a;
            }
            if (maxElement < 0) return maxElement;
            return maxSlice;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<MaxSliceSum>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 3, 2, -6, 4, 0 }, 5);
                yield return CreateSingleInputSet(new[] { -10 }, -10);
            }
        }
    }
}
