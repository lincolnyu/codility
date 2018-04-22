using codility.TestFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace codility.Lessons.Lesson15
{
    class AbsDistinct : ITestee
    {
        int Solve(int[] A)
        {
            throw new NotImplementedException();
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<AbsDistinct>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { -5, -3, -1, 0, 3, 6 }, 5);
            }
        }
    }
}
