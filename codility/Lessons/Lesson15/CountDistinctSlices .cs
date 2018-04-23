using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson15
{
    class CountDistinctSlices : ITestee
    {
        int Solve(int M, int[] A)
        {
            var map = new int[M + 1];
            for (var i = 0; i < M + 1; i++) map[i] = -1;
            var begin = 0;
            var aBegin = A[begin];
            map[aBegin] = begin;
            var total = 1;
            for (var end = 1; end < A.Length; end++)
            {
                var aEnd = A[end];
                if (map[aEnd] >= 0)
                {
                    var newBegin = map[aEnd] + 1;
                    for (var j = begin; j < newBegin; j++)
                    {
                        map[A[j]] = -1;
                    }
                    begin = newBegin;
                }
                total += end - begin + 1;
                if (total >= 1000000000) return 1000000000;
                map[aEnd] = end;
            }
            return total;
        }

        public object Run(params object[] args)
            => Solve((int)args[0], (int[])args[1]);

        public class Tester : BaseSelfTester<CountDistinctSlices>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(6, new[] { 3, 4, 5, 1, 5, 2 }, 15);
                yield return Create2InputSet(6, new[] { 3, 4, 5, 5, 2 }, 9);
            }
        }
    }
}
