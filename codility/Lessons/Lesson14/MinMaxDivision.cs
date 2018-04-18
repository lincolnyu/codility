using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson14
{
    class MinMaxDivision : ITestee
    {
        int Solve(int K, int M, int[] A)
        {
            throw new System.NotImplementedException();
        }

        public object Run(params object[] args)
            => Solve((int)args[0], (int)args[1], (int[])args[2]);

        public class Tester : BaseSelfTester<MinMaxDivision>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create3InputSet(3, 5, new[] { 2, 1, 5, 1, 2, 2, 2 }, 6);
            }
        }
    }
}
