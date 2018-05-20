using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson91
{
    public class HilbertMaze : ITestee
    {
        // req: O(N), O(N)
        public int solution(int N, int A, int B, int C, int D)
        {
            throw new System.NotImplementedException();
        }

        public object Run(params object[] args)
            => solution((int)args[0], (int)args[1], (int)args[2], (int)args[3], (int)args[4]);

        public class Tester : BaseSelfTester<HilbertMaze>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create5InputSet(1, 2, 1, 3, 4, 8);
                yield return Create5InputSet(2, 2, 5, 6, 6, 7);
                yield return Create5InputSet(3, 6, 6, 10, 13, 39);
            }
        }
    }
}
