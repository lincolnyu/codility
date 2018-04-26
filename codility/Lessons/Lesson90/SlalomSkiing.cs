using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson90
{
    class SlalomSkiing : ITestee
    {
        int Solve(int[] A)
        {
            throw new System.NotImplementedException();
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<SlalomSkiing>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 1, 5 }, 2);
                yield return CreateSingleInputSet(new[] {15,13,5,7,4,10,12,8,2,11,6,9,3 }, 13);
            }
        }
    }
}
