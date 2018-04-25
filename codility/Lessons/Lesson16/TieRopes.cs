using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson16
{
    class TieRopes : ITestee
    {
        int Solve(int K, int[] A)
        {
            var total = 0;
            var rope = 0;
            foreach (var a in A)
            {
                rope += a;
                if (rope >= K)
                {
                    rope = 0;
                    total++;
                }
            }
            if (rope >= K)
            {
                total++;
            }
            return total;
        }

        public object Run(params object[] args)
            => Solve((int)args[0], (int[])args[1]);

        public class Tester : BaseSelfTester<TieRopes>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 1, 2, 3, 4, 1, 1, 3 }, 3);
            }
        }
    }
}
