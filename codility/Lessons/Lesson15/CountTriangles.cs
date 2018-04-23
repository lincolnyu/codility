using System.Linq;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson15
{
    class CountTriangles : ITestee
    {
        public int Solve(int[] A)
        {
            var sorted = A.OrderBy(x => x).ToArray();
            var n = sorted.Length;
            var total = 0;
            for (var i = 0; i < n - 2; i++)
            {
                var k = i + 2;
                for (var j = i + 1; j < n; j++)
                {
                    for (;  k < n && sorted[i] + sorted[j] > sorted[k]; k++)
                    { }
                    total += k - j - 1;
                }
            }
            return total;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<CountTriangles>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 10, 2, 5, 1, 8, 12 }, 4);
            }
        }
    }
}
