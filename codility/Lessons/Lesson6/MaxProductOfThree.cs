using codility.TestFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace codility.Lessons.Lesson6
{
    class MaxProductOfThree : ITestee
    {
        int Solve(int[] A)
        {
            Array.Sort(A);
            var n = A.Length;
            var lastThree = A[n - 3] * A[n - 2] * A[n - 1];
            if (A[1] < 0 && A[n - 1] > 0)
            {
                return Math.Max(A[0] * A[1] * A[n - 1], lastThree);
            }
            else 
            {
                return lastThree;
            }
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<MaxProductOfThree>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { -3, 1, 2, -2, 5, 6 }, 60);
            }
        }
    }
}
