using System.Collections.Generic;
using System.Linq;
using codility.TestFramework;

namespace codility.Lessons.Lesson8
{
    class EquiLeader : ITestee
    {
        int Solve(int[] A)
        {
            var repCount = 0;
            var cand = 0;
            var n = A.Length;
            for (var i = 0; i < n; i++)
            {
                var a = A[i];
                if (repCount > 0)
                {
                    if (cand != a)
                    {
                        repCount--;
                    }
                    else
                    {
                        repCount++;
                    }
                }
                else
                {
                    repCount++;
                    cand = a;
                }
            }
            if (repCount == 0) return 0;
            var totalLeaders = A.Count(a => a == cand);
            var isLeader = totalLeaders > n / 2;
            if (!isLeader) return 0;
            var leftLeaderCount = 0;
            var equiCount = 0;
            for (var i = 0; i < n-1; i++)
            {
                if (A[i] == cand)
                {
                    leftLeaderCount++;
                }
                var leftTotal = i + 1;
                if (leftLeaderCount > leftTotal / 2
                    && (totalLeaders - leftLeaderCount) > (n - leftTotal) / 2)
                {
                    equiCount++;
                }
            }
            return equiCount;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<EquiLeader>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 4, 3, 4, 4, 4, 2 }, 2);
                yield return CreateSingleInputSet(new[] { 0, 0 }, 1);
            }
        }
    }
}
