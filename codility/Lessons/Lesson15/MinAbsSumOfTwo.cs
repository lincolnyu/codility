using codility.Lessons.Lesson14.Helper;
using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson15
{
    class MinAbsSumOfTwo : ITestee
    {
        int Solve(int[] A)
        {
            if (A.Length < 2) return Math.Abs(A[0])*2;
            Array.Sort(A);
            var bin = BSHelper.Generate(0, A.Length - 1);
            var lastIndex = -1;
            foreach (var b in bin)
            {
                var a = A[b.Index];
                lastIndex = b.Index;
                if (a < 0)
                {
                    b.Dir = 1;
                }
                else if (a > 0)
                {
                    b.Dir = -1;
                }
                else
                {
                    break;
                }
            }
            if (A[lastIndex] == 0 && lastIndex > 0 && A[lastIndex - 1] == 0
                || lastIndex < A.Length - 1 && A[lastIndex + 1] == 0) return 0;
            int i, j;
            if (A[lastIndex] < 0)
            {
                i = lastIndex;
                j = lastIndex + 1;
                if (j >= A.Length) return -A[i]*2;
            }
            else
            {
                i = lastIndex - 1;
                j = lastIndex;
                if (i < 0) return A[j]*2;
            }
            var min = int.MaxValue;
            for (; ; )
            {
                var sum = A[i] + A[j];
                var abssum = Math.Abs(sum);
                if (abssum < min) min = abssum;
                if (i > 0 && j < A.Length - 1)
                {
                    var newLeft = Math.Abs(A[i - 1] + A[j]);
                    var newRight = Math.Abs(A[i] + A[j + 1]);
                    if (newLeft < newRight)
                    {
                        i--;
                    }
                    else
                    {
                        j++;
                    }
                }
                else if (i > 0)
                {
                    if (sum > 0) i--;
                    else break;
                }
                else if (j < A.Length - 1)
                {
                    if (sum < 0) j++;
                    else break;
                }
                else break;
            }
            return min;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<MinAbsSumOfTwo>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 8, 5, 3, 4, 6, 8 }, 6);
                yield return CreateSingleInputSet(new[] { -1000000000 }, 2000000000);
                yield return CreateSingleInputSet(new[] { -1,1}, 0);
                yield return CreateSingleInputSet(new[] { 1, 4, -3 }, 1);
                yield return CreateSingleInputSet(new[] { 1, 4, 3 }, 2);
                yield return CreateSingleInputSet(new[] { -8, 4, 5, -10, 3 }, 3);
            }
        }
    }
}
