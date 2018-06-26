using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson91
{
    class TreeProduct : BaseTestee
    {
        class Link
        {
            public TreeNode Node1;
            public TreeNode Node2;
        }
        class TreeNode
        {
            public List<Link> Links { get; } = new List<Link>();
        }

        // req: O(N*log(N)), O(N)
        public int solution(int[] A, int[] B)
        {
            throw new System.NotImplementedException();
        }

        public class Tester : BaseSelfTester<TreeProduct>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateInputSet(18, new[] { 0, 1, 1, 3, 3, 6, 7 }, new[] { 1, 2, 3, 4, 5, 3, 5 });
            }
        }
    }
}
