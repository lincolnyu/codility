using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson10
{
    class MinPerimeterRectangle : ITestee
    {
        int Solve(int N)
        {
            var sqrtN = (int)Math.Floor(Math.Sqrt(N));
            var minPeri = int.MaxValue;
            for (var a = 1; a <= sqrtN; a++)
            {
                var b = N / a;
                var r = N - b * a;
                if (r != 0) continue;
                var peri = (a + b) * 2;
                if (peri < minPeri) minPeri = peri;
            }
            return minPeri;
        }

        public object Run(params object[] args)
            => Solve((int)args[0]);

        public class Tester : BaseSelfTester<MinPerimeterRectangle>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(30, 22);
            }
        }
    }
}
