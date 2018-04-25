using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson16
{
    class MaxNonoverlappingSegments : ITestee
    {
        int Solve(int[] A, int[] B)
        {
            var total = 0;
            for (var i = B.Length - 1; i >= 0; total++)
            {
                var maxStart = A[i];
                var end = B[i];

                for (var j = i - 1; j >= 0 && B[j] > maxStart; j--)
                {
                    if (A[j] > maxStart)
                    {
                        maxStart = A[j];
                        i = j;
                    }
                }
               
                for (i--; i >= 0 && B[i] >= maxStart; i--) ;
            }
            return total;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0], (int[])args[1]);

        public class Tester : BaseSelfTester<MaxNonoverlappingSegments>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(new[] { 1, 3, 7, 9, 9 }, new[] { 5, 6, 8, 9, 10 }, 3);
            }
        }
    }
}
