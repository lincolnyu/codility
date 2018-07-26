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

        public abstract int CompareTo(Node other);
    }

    public abstract class Node<T> : Node where T : Node<T>, IComparable<T>
    {
        public override int CompareTo(Node other)
            => ((IComparable<T>)this).CompareTo((T)other);
    }

    public static class Algo<T> where T : Node
    {
        public delegate void MarkDelegate(T n, bool subtree = false);
        public delegate void UnmarkSubtreeDelegate(T n);

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
            => Treeify(sorted, 0, sorted.Count);

        public static T Treeify(IList<T> sorted, int start, int len)
        {
            if (len == 0) return null;
            if (len == 1) return sorted[start];

            var l = Log2(len);
            var m = (1 << (l - 1)) - 1;
            var root = sorted[start + m];
            root.Left = Treeify(sorted, start, m);
            root.Right = Treeify(sorted, start + m + 1, len - m - 1);
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

        public static void MarkRange(T left, T right,
            MarkDelegate mark, UnmarkSubtreeDelegate unmarkSubtree)
        {
            // left (inclusive) to common root (exclusive)
            //   for every 'right parent' (or left itself) :
            //      single-mark it
            //      subtree-mark its right child if existent
            // similar to right

            // NOTE The purpose is not to maximizing subtree marking
            //      but minimizing the marking operations itself

            Node lastp;

            var p = left;

            if (p != right)
            {
                lastp = p.Left;
                for (; ; )
                {
                    var pp = p.Parent;
                    if (pp != null && pp.CompareTo(right) < 0)
                    {
                        if (p.Left == lastp)
                        {
                            mark(p, false);
                            if (p.Right != null)
                            {
                                mark((T)p.Right, true);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                    lastp = p;
                    p = (T)pp;
                }
            }

            // p is now common root

            if (p == left)
            {
                mark(left, false);
            }
            else if (p == right)
            {
                mark(right, false);
            }
            else
            {
                mark(p, false);
            }

            lastp = right.Right;
            for (var pr = right; pr != p; pr = (T)pr.Parent)
            {
                if (pr.Right == lastp)
                {
                    mark(pr, false);
                    if (pr.Left != null)
                    {
                        mark((T)pr.Left, true);
                    }
                }
                lastp = pr;
            }

            for (; p != null; p = (T)p.Parent)
            {
                unmarkSubtree(p);
            }
        }
    }
}
