using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson5
{
    class CountDiv : ITestee
    {
        public int Solve(int A, int B, int K)
        {
            var qa = A / K;
            var ra = A - K * qa;
            var na = ra != 0 ? qa + 1 : qa;
            var nb = B / K;
            return na <= nb ? nb - na + 1 : 0;
        }

        public object Run(params object[] args)
            => Solve((int) args[0], (int) args[1], (int) args[2]);

        public class Tester : BaseSelfTester<CountDiv>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create3InputSet(6, 11, 2, 3);
            }
        }
    }
}
