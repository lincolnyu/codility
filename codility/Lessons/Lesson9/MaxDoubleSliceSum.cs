using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson9
{
    public class MaxDoubleSliceSum : ITestee
    {
        int Solve(int[] A)
        {
            var n = A.Length;
            var maxFromLeft = new int[n];
            var past = 0;
            for (var i = 1; i < n-1; i++)
            {
                var a = A[i];
                maxFromLeft[i] = past + a;
                if (maxFromLeft[i] > 0)
                {
                    past = maxFromLeft[i];
                }
                else
                {
                    past = 0;
                }
            }
            var maxFromRight = new int[n];
            past = 0;
            for (var i = n - 2; i > 0; i--)
            {
                var a = A[i];
                maxFromRight[i] = past + a;
                if (maxFromRight[i] > 0)
                {
                    past = maxFromRight[i];
                }
                else
                {
                    past = 0;
                }
            }
            if (n <= 3) return 0;
            var maxDoubleSlice = int.MinValue;
            for (var i = 1; i < n-1; i++)
            {
                var maxLeft = maxFromLeft[i - 1];
                var maxRight = maxFromRight[i + 1];
                var v = Math.Max(maxLeft, 0) + Math.Max(maxRight, 0);
                if (v > maxDoubleSlice)
                {
                    maxDoubleSlice = v;
                }
            }
            return maxDoubleSlice;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<MaxDoubleSliceSum>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 3, 2, 6, -1, 4, 5, -1, 2 }, 17);
                yield return CreateSingleInputSet(new[] { 5, 5, 5 }, 0);
                yield return CreateSingleInputSet(new[] { 5, 17, 0, 3 }, 17);
                yield return CreateSingleInputSet(new[] { -1, -1, -1 }, 0);
                yield return CreateSingleInputSet(new[] { -1, -1, -1, -1, -1 }, 0);
                yield return CreateSingleInputSet(new[] { 0, 10, -5, -2, 0 }, 10);
            }
        }
    }
}
