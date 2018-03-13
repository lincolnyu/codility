using codility.TestFramework;
using System.Collections.Generic;

namespace codility.Lessons.Lesson2
{
    class OddOccurrencesInArray : ITestee
    {
        public int Solve(int[] A)
        {
            var s = 0;
            foreach (var v in A)
            {
                s ^= v;
            }
            return s;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<OddOccurrencesInArray>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 9, 3, 9, 3, 9, 7, 9 }, 7);
                yield return CreateSingleInputSet(new[] { 5, 4, 3, 2, 2, 3, 4, 5, 6, 7, 6 }, 7);
            }
        }
    }
}
