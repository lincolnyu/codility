using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson91
{
    // req: O(N*log(N)), O(N)
    class RectangleBuilderGreaterArea : ITestee
    {
        public int solution(int[] A, int X)
        {
            throw new System.NotImplementedException();
        }

        public object Run(params object[] args)
            => solution((int[])args[0], (int)args[1]);

        public class Tester : BaseSelfTester<RectangleBuilderGreaterArea>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(new[] { 1, 2, 5, 1, 1, 2, 3, 5, 1 }, 5, 2);
            }
        }
    }
}
