using codility.TestFramework;
using System.Collections.Generic;

namespace codility.Lessons.Lesson3
{
    class PermMissingElem : ITestee
    {
        public int Solve(int[] A)
        {
            var N = A.Length;
            var s = 0;
            for (var i = 0; i < N; i++)
            {
                s ^= A[i] ^ (i + 1);
            }
            return s ^ (N + 1);
        }

        public int SolveSuboptimal(int[] A)
        {
            var N = A.Length;
            long N1 = N + 1;
            var expectedTotal = (N1 + 1) * N1 / 2;
            long total = 0;
            foreach (var a in A) total += a;
            return (int)(expectedTotal - total);
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<PermMissingElem>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new int[] { 2, 3, 1, 5 }, 4);
            }
        }
    }
}
