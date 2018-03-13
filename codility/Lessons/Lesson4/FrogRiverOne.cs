using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson4
{
    class FrogRiverOne : ITestee
    {
        public int Solve(int X, int[] A)
        {
            var filled = new bool[X];
            var filledCount = 0;
            for (var i = 0; i < A.Length; i++)
            {
                var index = A[i] - 1;
                if (!filled[index])
                {
                    filledCount++;
                    if (filledCount == X) return i;
                    filled[index] = true;
                }
            }
            return -1; // unexpected
        }

        public object Run(params object[] args)
            => Solve((int)args[0], (int[])args[1]);

        public class Tester : BaseSelfTester<FrogRiverOne>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(5, new[] { 1, 3, 1, 4, 2, 3, 5, 4 }, 6);
            }
        }
    }
}
