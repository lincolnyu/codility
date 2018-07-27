#define LOCAL_FUNCTION

using System;
using System.Collections.Generic;
using System.Linq;

namespace codility.Lib.SmartArray
{
    public abstract class Node : IComparable<Node>
    {
        public Node Parent { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public int Depth { get; set; }

        // Assumption: Items are distinct with respect to this comparison
        public abstract int CompareTo(Node other);
    }

    public abstract class Node<T> : Node where T : Node<T>, IComparable<T>
    {
        public override int CompareTo(Node other)
            => ((IComparable<T>)this).CompareTo((T)other);
    }

    public static class Algo<T> where T : Node
    {
        public enum MarkType
        {
            LeftAndCenter,
            RightAndCenter,
            CenterAndCheck,
            CheckAll
        }

        public delegate void MarkDelegate(T n, MarkType mt);
        public delegate void UnmarkSubtreeDelegate(T n);

#if !NEW_CSHARP
        delegate void Walk();
#endif

        // the depth of the tree
        private static int Log2(int a)
        {
            var i = 0;
            for (; a > 0; i++, a >>= 1) ;
            return i;
        }

        public static IEnumerable<T> Load<T1>(IEnumerable<T1> t, Func<T1, T> create)
            => t.Select(x => create(x));

        public static T Treeify(IList<T> sorted)
            => Treeify(sorted, 0, sorted.Count, 0);

        public static T Treeify(IList<T> sorted, int start, int len, int depth)
        {
            if (len == 0) return null;
            if (len == 1)
            {
                sorted[start].Depth = depth;
                return sorted[start];
            }

            var l = Log2(len);
            var m = (1 << (l - 1)) - 1;
            var root = sorted[start + m];
            root.Depth = depth;
            root.Left = Treeify(sorted, start, m, depth+1);
            root.Right = Treeify(sorted, start + m + 1, len - m - 1, depth+1);
            if (root.Left != null)
            {
                root.Left.Parent = root;
            }
            if (root.Right != null)
            {
                root.Right.Parent = root;
            }
            return root;
        }

        // comp returns the value indicating target.compareTo(p)
        public static IEnumerable<T> Search(T root, Func<T, int> comp)
        {
            var last = root;
            for (var p = root; p != null;)
            {
                yield return p;
                var c = comp(p);
                if (c == 0)
                {
                    yield break;
                }
                p = (T)((c < 0) ? p.Left : p.Right);
            }
        }

        public static T Find(T root, Func<T, int> comp)
        {
            return FindMark(root, comp, x => comp(x) == 0);
        }

        public static T FindMark(T root, Func<T, int> comp, Predicate<T> hasMark)
        {
            var path = Search(root, comp);
            foreach (var n in path)
            {
                if (hasMark(n))
                {
                    return n;
                }
            }
            return null;
        }

        public static void MarkRange(T left, T right, MarkDelegate mark)
        {
            var lpl = left.Left;
            var pl = left;
            var lpr = right.Right;
            var pr = right;

#if LOCAL_FUNCTION
            void walkLeft()
#else
            Walk walkLeft = () =>
#endif
            {
                if (pl.Left == lpl)
                {
                    // Howerver the implementing method is recommend to check if
                    // the left subtree is marked and decide if the whole subtree is
                    mark(pl, MarkType.RightAndCenter);
                }
                lpl = pl;
            };

#if LOCAL_FUNCTION
            void walkRight()
#else
            Walk walkRight = () =>
#endif
            {
                if (pr.Right == lpr)
                {
                    // Howerver the implementing method is recommended to check if
                    // the right subtree is marked and decide if the whole subtree is
                    mark(pl, MarkType.LeftAndCenter);
                }
                lpr = pr;
            };

            for (; pl.Depth > right.Depth; pl = (T)pl.Parent)
            {
                walkLeft();
            }
            for (; pr.Depth > left.Depth; pr = (T)pr.Parent)
            {
                walkRight();
            }

            for (; pl != pr; pl = (T)pl.Parent, pr = (T)pr.Parent)
            {
                walkLeft();
                walkRight();
            }

            // Mark the center and if both children are marked then mark the whole subtree
            mark(pl, MarkType.CenterAndCheck);
            
            for (pl = (T)pl.Parent; pl != null; pl = (T)pl.Parent)
            {
                // If only the current and both children are marked then mark the whole subtree
                mark(pl, MarkType.CheckAll);
            }
        }
    }
}
