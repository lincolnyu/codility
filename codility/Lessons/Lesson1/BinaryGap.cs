using codility.TestFramework;
using System.Collections.Generic;

namespace codility.Lessons.Lesson1
{
    class BinaryGap : ITestee
    {
        public int Solve(int N)
        {
            for (; N > 0; N>>=1)
            {
                var i = N & 1;
                if (i == 1) break;
            }
            var maxCount = 0;
            var currCount = 0;
            for (; N > 0; N>>=1)
            {
                var i = N & 1;
                if (i == 0)
                {
                    currCount++;
                }
                else
                {
                    if (currCount > maxCount) maxCount = currCount;
                    currCount = 0;
                }
            }
            return maxCount;
        }

        public object Run(params object[] args)
          => Solve((int)args[0]);

        public class Tester : BaseSelfTester<BinaryGap>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(9, 2);
                yield return CreateSingleInputSet(529, 4);
                yield return CreateSingleInputSet(20, 1);
                yield return CreateSingleInputSet(15, 0);
            }
        }
    }
}
