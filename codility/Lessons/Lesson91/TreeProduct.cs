using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson91
{
    class TreeProduct : ITestee
    {
        // req: O(N*log(N)), O(N)
        public string solution(int[] A, int[] B)
        {
            throw new System.NotImplementedException();
        }

        public object Run(params object[] args)
            => solution((int[])args[0], (int[])args[1]);

        public class Tester : BaseSelfTester<TreeProduct>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(new[] { 0, 1, 1, 3, 3, 6, 7 }, new[] { 1, 2, 3, 4, 5, 3, 5 }, 18);
            }
        }
    }
}
