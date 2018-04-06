using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson6
{
    class Triangle : ITestee
    {
        int Solve(int[] A)
        {
            Array.Sort(A); // assuming it's O(n log n)
            for (var i = A.Length - 3; i >= 0; i--)
            {
                var a = A[i];
                var b = A[i + 1];
                var c = A[i + 2];
                if (a > c - b) return 1;
            }
            return 0;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<Triangle>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 10, 2, 5, 1, 8, 20 }, 1);
                yield return CreateSingleInputSet(new[] { 10, 50, 5, 1 }, 0);
            }
        }
    }
}
