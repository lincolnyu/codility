using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson4
{
    class PermCheck : ITestee
    {
        public int Solve(int[] A)
        {
            var N = A.Length;
            var b = new bool[N];
            for (var i = 0; i < N; i++)
            {
                var index = A[i]-1;
                if (index >= N) return 0;
                if (b[index]) return 0;
                b[index] = true;
            }
            return 1;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<PermCheck>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 4, 1, 3, 2 }, 1);
                yield return CreateSingleInputSet(new[] { 4, 1, 3 }, 0);
                yield return CreateSingleInputSet(new[] { 1, 1 }, 0);
            }
        }
    }
}
