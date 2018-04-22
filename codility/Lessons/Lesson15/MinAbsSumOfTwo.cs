using codility.TestFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace codility.Lessons.Lesson15
{
    class MinAbsSumOfTwo : ITestee
    {
        int Solve(int[] A)
        {
            throw new NotImplementedException();
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<MinAbsSumOfTwo>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 1, 4, -3 }, 1);
                yield return CreateSingleInputSet(new[] { -8, 4, 5, -10, 3 }, 3);
            }
        }
    }
}
