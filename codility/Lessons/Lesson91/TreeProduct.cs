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
            public int SubtreeSize;// size of subtree from node2

            public TreeNode OtherNode(TreeNode oneNode) => Node1 == oneNode ? Node2 : Node1;

            public void SetSubtreeSize(TreeNode node, int size, int totalNodess)
            {
                SubtreeSize = (Node2 == node) ? size : totalNodess - size;
            }

            public int GetSubtreeSize(TreeNode node, int totalNodes)
                => Node2 == node ? SubtreeSize : totalNodes - SubtreeSize;

            public int GetSubtreeSizeOfOther(TreeNode node, int totalNodes)
                => Node2 == node ? totalNodes - SubtreeSize : SubtreeSize;

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
            public int ParentIndex;
            public List<Link> Links { get; } = new List<Link>();

            public IEnumerable<Link> GetSublinks(Link parentLink)
                => Links.Where(x => x != parentLink);

            public IEnumerable<TreeNode> GetSubnodes(Link parentLink)
                => GetSublinks(parentLink).Select(x => x.OtherNode(this));
        }

        class SubtreeMeasurer
        {
            public readonly int TotalNodes;
            public SubtreeMeasurer(int totalNodes)
            {
                TotalNodes = totalNodes;
            }

            public void Measure(TreeNode node, Link parentLink)
            {
                parentLink?.SetSubtreeSize(node, node.GetSublinks(parentLink).Sum(l =>
                    l.GetSubtreeSizeOfOther(node, TotalNodes)
                ) + 1, TotalNodes);
            }
        }

        class SingleBridgeBurner
        {
            public readonly int TotalNodes;
            public int Max = 0;
            public Link MaxLink = null;

            public SingleBridgeBurner(int totalNodes)
            {
                TotalNodes = totalNodes;
            }

            public void Update(TreeNode node, Link parentLink)
            {
                if (parentLink != null)
                {
                    var c1 = parentLink.GetSubtreeSize(node, TotalNodes);
                    var c2 = TotalNodes - c1;
                    var m = c1 * c2;
                    if (m > Max)
                    {
                        Max = m;
                        MaxLink = parentLink;
                    }
                }
            }
        }

        class SubtreeSizeCollector
        {
            private readonly LinkedList<int> List;
            private int TotalNodes;

            public SubtreeSizeCollector(LinkedList<int> list, int totalNodes)
            {
                List = list;
                TotalNodes = totalNodes;
            }

            public void Collect(TreeNode node, Link link)
            {
                var size = link.GetSubtreeSize(node, TotalNodes);
                List.AddLast(size);
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

        private long Mul(int a, int b, int c)
        {
            var d = (long)a * b;
            return d * c;
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

        private LinkedList<int> LaunderList(LinkedList<int> ll)
        {
            var a = ll.ToArray();
            Array.Sort(a);
            var r = new LinkedList<int>();
            var last = -1;
            foreach (var i in a)
            {
                if (i != last)
                {
                    r.AddLast(i);
                    last = i;
                }
            }
            return r;
        }

        private long GetDoubleBridgesBurnMax(Link startLink, int totalNodes)
        {
            // 'startLink' divide the tree to two subtrees with a and M-a nodes respectively
            // Then it follows that two bridges have to be one on each side of 'startLink' 
            // (or at most one being the 'startLink' itself) For otherwise they both on one
            // side (suppose the total number of nodes is M): 
            // 1. If one is on the subtree chopped out by the other:
            //    then the tree is divided by them into x, y, M-x-y
            //    we can prove that x, a-x, M-a is better than that division, because:
            //    (M-a)*a >= (M-x-y)*(x+y), so (M-a)(a-x+x) >= (M-x-y)*(x+y)
            //    (M-a)*(a-x) + (M-a)*x >= (M-x-y)*y + (M-x-y)*x
            //    Since x+y < a, i.e. M-a < M-x-y
            //    (M-a)*(a-x) > (M-x-y)*y, hence (M-a)*(a-x)*x > (M-x-y)*y*x
            // 2. If they are on different subtrees:
            //    The two subtrees are x, y respectively, the remainder of the tree of size a
            //    minus the two subtree is e (=a-x-y), let b=M-a
            //    The challenging product is x*y*(e+b). Now we can prove 
            //          (e+x)*y*b >= x*y*(e+b) or (e+y)*x*b >= x*y*(e+b)
            //    For otherwise, x>b and y>b, then we must have x*(M-x)>a*b and y*(M-y)>a*b
            //    (Not hard to prove)
            var list1 = new LinkedList<int>();
            var list2 = new LinkedList<int>();
            TraverseTreePreOrder(startLink.Node1, startLink, new SubtreeSizeCollector(list1, totalNodes).Collect);
            TraverseTreePreOrder(startLink.Node2, startLink, new SubtreeSizeCollector(list2, totalNodes).Collect);
            list1 = LaunderList(list1);
            list2 = LaunderList(list2);

            long maxm = 0;
            var maxmset = true;

            for (LinkedListNode<int> i = list1.First, j = list2.Last; 
                maxmset && i != null && j != null; i = i.Next)
            {
                var a = i.Value;
                maxmset = false;
                long lastM = 0;
                for (; j != null; j = j.Previous)
                {
                    var b = j.Value;
                    var c = totalNodes - a - b;
                    var m = Mul(a, b, c);
                    if (maxm < m)
                    {
                        maxm = m;
                        maxmset = true;
                    }
                    else if (m < lastM)
                    {
                        break;
                    }
                    lastM = m;
                }
                j = j.Next;
            }
            return maxm;
        }

        // req: O(N*log(N)), O(N)
        public string solution(int[] A, int[] B)
        {
            var N = A.Length;
            var sol0 = N + 1; // total nodes
            var root = BuildTree(A, B);
            TraverseTreePostOrder(root, null, new SubtreeMeasurer(sol0).Measure);
            var sbb = new SingleBridgeBurner(sol0);
            TraverseTreePreOrder(root, null, sbb.Update);
            var sol1 = sbb.Max;
            var ideal3 = GetIdeal3(sol0);
            var max01 = Math.Max(sol0, sol1);
            if (max01 >= ideal3) return max01.ToString();
            var sol2 = GetDoubleBridgesBurnMax(sbb.MaxLink, sol0);
            return Math.Max((long)max01, sol2).ToString();
        }

        public class Tester : BaseSelfTester<TreeProduct>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateInputSet("18", new [] { 0, 1, 1, 3, 3, 6, 7 },
                    new [] { 1, 2, 3, 4, 5, 3, 5 });
            }
        }
    }
}
