using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson6
{
    class NumberOfDiscIntersections : ITestee
    {
        int Solve(int[] A)
        {
            throw new System.NotImplementedException();
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<NumberOfDiscIntersections>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 1, 5, 2, 1, 4, 0 }, 11);
            }
        }
    }
}
