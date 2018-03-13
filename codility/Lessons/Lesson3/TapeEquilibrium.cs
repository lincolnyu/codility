using System.Collections.Generic;
using System.Linq;
using codility.TestFramework;

namespace codility.Lessons.Lesson3
{
    class TapeEquilibrium : ITestee
    {
        public int Solve(int[] A)
        {
            var sum = A.Sum();
            var min = int.MaxValue;
            for (var p = A.Length-1; p > 0; p--)
            {
                sum -= A[p] * 2;
                var asum = sum < 0 ? -sum : sum;
                if (asum < min)
                {
                    min = asum;
                }
            }
            return min;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<TapeEquilibrium>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 3, 1, 2, 4, 3 }, 1);
            }
        }
    }
}
