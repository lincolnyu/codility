using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson5
{
    class MinAvgTwoSlice : ITestee
    {
        int Solve(int[] A)
        {
            var n = A.Length;
            int? minSum = null;
            var minC = 1;
            var minPos = -1;
            var lastLowPos = 0;
            void check(int i, int sum, int checkC)
            {
                if (minSum == null || sum * minC < minSum.Value * checkC)
                {
                    minSum = sum;
                    minC = checkC;
                    minPos = i - 1;
                }
            }
            for (var i = 1; i < n; i++)
            {
                var curr = A[i];
                var prev = A[i - 1];
                var sum = curr + prev;
                if (curr == prev)
                {
                    check(i, sum, 2);
                }
                else if (curr < prev)
                {
                    if (i < n - 1)
                    {
                        var next = A[i + 1];
                        if (next > curr)
                        {
                            check(i, sum, 2);
                            lastLowPos = i;
                        }
                    }
                    else
                    {
                        check(i, sum, 2);
                    }
                }
                else if (i == lastLowPos + 1) // curr > prev and prev is a min
                {
                    check(i, sum, 2);
                    if (i < n - 1)
                    {
                        var next = A[i + 1];
                        if (next < curr && (i >= n - 2 || A[i + 2] > next))
                        {
                            sum += next;
                            check(i, sum, 3);
                        }
                    }
                }
            }
            return minPos;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<MinAvgTwoSlice>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 4, 2, 2, 5, 1, 5, 8 }, 1);
                yield return CreateSingleInputSet(new[] { 5, 6, 3, 4, 9 }, 2);
                yield return CreateSingleInputSet(new[] { 1, 2, 3, 4 }, 0);
                yield return CreateSingleInputSet(new[] { 0, 4, 3 }, 0 );
            }
        }
    }
}
