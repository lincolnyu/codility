using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson90
{
    public class FloodDepth : ITestee
    {
        public int Solve(int[] A)
        {
            throw new System.NotImplementedException();
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<FloodDepth>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 5, 8 }, 0);
                yield return CreateSingleInputSet(new[] { 1, 3, 2, 1, 2, 1, 5, 3, 3, 4, 2 }, 2);
            }
        }
    }
}
