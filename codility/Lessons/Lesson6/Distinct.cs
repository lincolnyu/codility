using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson6
{
    class Distinct : ITestee
    {
        int Solve(int[] A)
        {
            if (A.Length == 0) return 0;
            Array.Sort(A);
            var count = 1;
            var lastA = A[0];
            for (var i = 1; i < A.Length; i++)
            {
                var a = A[i];
                if (a != lastA)
                {
                    lastA = a;
                    count++;
                }
            }
            return count;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<Distinct>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 2, 1, 1, 2, 3, 1 }, 3);
            }
        }
    }
}
