using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson16
{
    class MaxNonoverlappingSegments : ITestee
    {
        int Solve(int[] A, int[] B)
        {
            throw new NotImplementedException();
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
