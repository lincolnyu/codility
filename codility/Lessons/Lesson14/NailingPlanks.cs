using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson14
{
    class NailingPlanks : ITestee
    {
        int Solve(int[] A, int[] B, int[] C)
        {
            throw new NotImplementedException();
        }

        public object Run(params object[] args)
            => Solve((int[])args[0], (int[])args[1], (int[])args[2]);

        public class Tester : BaseSelfTester<NailingPlanks>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create3InputSet(new[] { 1, 4, 5, 8 }, new[] { 4, 5, 9, 10 }, new[] { 4, 6, 7, 10, 2 }, 4);
            }
        }
    }
}
