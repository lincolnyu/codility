using System.Collections.Generic;
using System.Linq;
using codility.TestFramework;

namespace codility.Lessons.Lesson8
{
    class Dominator : ITestee
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
            var count = 0;
            int? firstOccur = null;
            for (var i = 0; i < n; i++)
            {
                if (A[i] == cand)
                {
                    if (firstOccur == null) firstOccur = i;
                    count++;
                    if (count > n / 2) return firstOccur.Value;
                }
            }
            return -1;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<Dominator>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 3, 4, 3, 2, 3, -1, 3, 3 }, 0);
                yield return CreateSingleInputSet(new[] { 2, 1, 1, 3, 4 }, -1);
            }
        }
    }
}
