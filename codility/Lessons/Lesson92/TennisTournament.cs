using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson92
{
    class TennisTournament : ITestee
    {
        public int solution(int P, int C)
            => Math.Min(C, P / 2);

        public object Run(params object[] args)
            => solution((int)args[0], (int)args[1]);

        public class Tester : BaseSelfTester<TennisTournament>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(5, 3, 2);
                yield return Create2InputSet(10, 3, 3);
            }
        }
    }
}
