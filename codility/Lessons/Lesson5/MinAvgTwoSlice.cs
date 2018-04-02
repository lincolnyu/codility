using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson5
{
    class MinAvgTwoSlice : ITestee
    {
        int Solve(int[] A)
        {
            throw new NotImplementedException();
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<MinAvgTwoSlice>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 4, 2, 2, 5, 1, 5, 8 }, 1);
            }
        }
    }
}
