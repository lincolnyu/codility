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
            for (var i = B.Length - 1; i >= 0; )
            {
                var end = B[i];
                var start = A[i];

                int maxStart2 = start;
                int start2;
                var j = i-1;
                int ii = i;
                for (j = i - 1; j >= 0 && (start2 = A[j]) > start; j--)
                {
                    if (start2 > maxStart2)
                    {
                        maxStart2 = start2;
                        ii = j;
                    }
                }

                total++;
                i = ii - 1;

                for (; i >= 0 && B[i] >= maxStart2; i--) ;
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
