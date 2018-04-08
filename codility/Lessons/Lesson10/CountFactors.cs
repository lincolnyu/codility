using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson10
{
    class CountFactors : ITestee
    {
        int Solve(int N)
        {
            var sqrtN = (int)Math.Floor(Math.Sqrt(N));
            var count = 0;
            for (var i = 1; i <= sqrtN; i++)
            {
                var d = N % i;
                if (d == 0)
                {
                    count += 2;
                }
            }
            if (sqrtN * sqrtN == N) count--;
            return count;
        }

        public object Run(params object[] args)
            => Solve((int)args[0]);

        public class Tester : BaseSelfTester<CountFactors>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(24, 8);
            }
        }
    }
}
