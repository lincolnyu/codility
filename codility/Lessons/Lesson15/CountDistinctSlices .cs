using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson15
{
    class CountDistinctSlices : ITestee
    {
        int Solve(int M, int[] A)
        {
            throw new NotImplementedException();
        }

        public object Run(params object[] args)
            => Solve((int)args[0], (int[])args[1]);

        public class Tester : BaseSelfTester<CountDistinctSlices>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(6, new[] { 3, 4, 5, 5, 2 }, 9);
            }
        }
    }
}
