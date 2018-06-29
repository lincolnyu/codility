using System;
using System.Collections.Generic;
using System.Linq;
using codility.TestFramework;

namespace codility.Lessons.Lesson91
{
    class TreeProduct : BaseTestee
    {
        class Link
        {
            public TreeNode Node1;
            public TreeNode Node2;

            public TreeNode OtherNode(TreeNode oneNode)
            {
                return Node1 == oneNode ? Node2 : Node1;
            }

            public static Link CreateLink(TreeNode node1, TreeNode node2)
            {
                var link = new Link
                {
                    Node1 = node1,
                    Node2 = node2
                };
                link.Node1.Links.Add(link);
                link.Node2.Links.Add(link);
                return link;
            }
        }
        class TreeNode
        {
            public List<Link> Links { get; } = new List<Link>();
            public int SubtreeSize; // number of nodes of sub tree starting from this node

            public IEnumerable<Link> GetSublinks(Link parentLink)
                => Links.Where(x => x != parentLink);

            public IEnumerable<TreeNode> GetSubnodes(Link parentLink)
                => GetSublinks(parentLink).Select(x => x.OtherNode(this));
        }

        class SubtreeMeasurer
        {
            public void Measure(TreeNode node, Link parentLink)
            {
                node.SubtreeSize = node.GetSubnodes(parentLink).Sum(x => x.SubtreeSize) + 1;
            }
        }

        class SingleBridgeBurner
        {
            public readonly int TotalNodes;
            public int Max = 0;

            public SingleBridgeBurner(int totalNodes)
            {
                TotalNodes = totalNodes;
            }

            public void Update(TreeNode node, Link parentLink)
            {
                if (parentLink != null)
                {
                    var c1 = node.SubtreeSize;
                    var c2 = TotalNodes - c1;
                    var m = c1 * c2;
                    if (m > Max)
                    {
                        Max = m;
                    }
                }
            }
        }

        private TreeNode BuildTree(int[] A, int[] B)
        {
            var N = A.Length; // number of bridges
            var nodes = new TreeNode[N + 1];
            for (var i = 0; i < N; i++)
            {
                var na = A[i];
                var nb = B[i];
                if (nodes[na] == null)
                {
                    nodes[na] = new TreeNode();
                }
                if (nodes[nb] == null)
                {
                    nodes[nb] = new TreeNode();
                }
                Link.CreateLink(nodes[na], nodes[nb]);
            }
            var root = nodes[0];
            return root;
        }

        private delegate void VisitNode(TreeNode node, Link parentLink);

        private void TraverseTreePreOrder(TreeNode node, Link parentLink, VisitNode visit)
        {
            visit(node, parentLink);
            foreach (var link in node.GetSublinks(parentLink))
            {
                var subnode = link.OtherNode(node);
                TraverseTreePreOrder(subnode, link, visit);
            }
        }

        private void TraverseTreePostOrder(TreeNode node, Link parentLink, VisitNode visit)
        {
            foreach (var link in node.GetSublinks(parentLink))
            {
                var subnode = link.OtherNode(node);
                TraverseTreePostOrder(subnode, link, visit);
            }
            visit(node, parentLink);
        }

        private int GetIdeal3(int nodeCount)
        {
            var n1 = (nodeCount + 1) / 3;
            var n2 = nodeCount - n1 * 2;
            return n1 * n1 * n2;
        }

        private int GetDoubleBridgesBurnMax(TreeNode root)
        {
            throw new NotImplementedException();
        }

        // req: O(N*log(N)), O(N)
        public int solution(int[] A, int[] B)
        {
            var N = A.Length;
            var root = BuildTree(A, B);
            TraverseTreePostOrder(root, null, new SubtreeMeasurer().Measure);
            var sol0 = N+1;
            var sbb = new SingleBridgeBurner(N + 1);
            TraverseTreePreOrder(root, null, sbb.Update);
            var sol1 = sbb.Max;
            var ideal3 = GetIdeal3(sol0);
            var max01 = Math.Max(sol0, sol1);
            if (max01 >= ideal3) return max01;
            var sol2 = GetDoubleBridgesBurnMax(root);
            return Math.Max(max01, sol2);
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
