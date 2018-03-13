using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson3
{
    class FrogJump : ITestee
    {
        public int Solve(int X, int Y, int D)
            => ((Y - X) + D - 1) / D;
        
        public object Run(params object[] args)
            => Solve((int)args[0], (int)args[1], (int)args[2]);

        public class Tester : BaseSelfTester<FrogJump>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create3InputSet(10, 85, 30, 3);
            }
        }
    }
}
