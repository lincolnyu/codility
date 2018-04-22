using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson15
{
    class CountTriangles : ITestee
    {
        public int Solve(int[] A)
        {
            throw new System.NotImplementedException();
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<CountDistinctSlices>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 10, 2, 5, 1, 8, 12 }, 4);
            }
        }
    }
}
