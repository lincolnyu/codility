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
        }

        public enum MarkStmType
        {
            LeftStAndCenter,
            RightStAndCenter,
            CenterStOnly
        }

        // Return original subtree mark token if it is a subtree marked node
        // only required when called with LeftAndCenter or RightAndCenter
        public delegate void MarkDelegate<TStm>(T n, MarkType mt);

        public delegate void MarkStmelegate<TStm>(T n, TStm stm, MarkStmType mst);

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

        public static void MarkRange<TStm>(T left, T right, MarkDelegate<TStm> mark,
            MarkStmelegate<TStm> markstm, Func<T, TStm> getstm)
        {
            var lpl = left.Left;
            var pl = (Node)left;
            var lpr = right.Right;
            var pr = (Node)right;

            var leftStack = new Stack<Node>();
            var rightStack = new Stack<Node>();

            var leftStmStackIndex = 0;
            TStm leftStm = default(TStm);
            var rightStmStackIndex = 0;
            TStm rightStm = default(TStm);

#if LOCAL_FUNCTION
            void walkLeft()
#else
            Walk walkLeft = () =>
#endif
            {
                var stm = getstm((T)pl);
                if (pl.Left == lpl)
                {
                    // The implementing method must mark entire right subtree
                    // Howerver the implementing method is recommend to check if
                    // the left subtree is marked and decide if the whole subtree is
                    mark((T)pl, MarkType.RightAndCenter);
                }
                else
                {
                    // devil's skin and hair
                    leftStack.Push(pl);
                }
                if (!stm.Equals(default(TStm)))
                {
                    leftStm = stm;
                    leftStmStackIndex = leftStack.Count;
                }
                lpl = pl;
            };

#if LOCAL_FUNCTION
            void walkRight()
#else
            Walk walkRight = () =>
#endif
            {
                var stm = getstm((T)pr);
                if (pr.Right == lpr)
                {
                    // The implementing method must mark entire left subtree
                    // Howerver the implementing method is recommended to check if
                    // the right subtree is marked and decide if the whole subtree is
                    mark((T)pr, MarkType.LeftAndCenter);
                }
                else
                {
                    // devil's skin and hair
                    rightStack.Push(pr);
                }
                if (!stm.Equals(default(TStm)))
                {
                    rightStm = stm;
                    rightStmStackIndex = rightStack.Count;
                }
                lpr = pr;
            };

            for (; pl.Depth > right.Depth; pl = pl.Parent)
            {
                walkLeft();
            }
            for (; pr.Depth > left.Depth; pr = pr.Parent)
            {
                walkRight();
            }

            for (; pl != pr; pl = pl.Parent, pr = pr.Parent)
            {
                walkLeft();
                walkRight();
            }

            // process stm

            // Common ancestor
            // Mark the center and if both children are marked then mark the whole subtree
            var rootstm = getstm((T)pl); 
            mark((T)pl, MarkType.CenterAndCheck);
            if (!rootstm.Equals(default(TStm)))
            {
                leftStm = rightStm = rootstm;
                leftStmStackIndex = leftStack.Count;
                rightStmStackIndex = rightStack.Count;
            }

            Node stmparent = null;
            TStm stmparentstm = default(TStm);
            for (var p = pl.Parent; p != null; p = p.Parent)
            {
                var stm = getstm((T)p);
                if (!stm.Equals(default(TStm)))
                {
                    stmparent = p;
                    stmparentstm = stm;
                }
            }
            if (stmparent != null)
            {
                var lp = pl;
                for (var p = pl.Parent; lp != stmparent; p = p.Parent)
                {
                    markstm((T)p, stmparentstm, lp == p.Left ? MarkStmType.RightStAndCenter : MarkStmType.LeftStAndCenter);
                    lp = p;
                }
                leftStm = rightStm = stmparentstm;
                leftStmStackIndex = leftStack.Count;
                rightStmStackIndex = rightStack.Count;
            }

            for (; leftStmStackIndex < leftStack.Count(); )
            {
                var p = (T)leftStack.Pop();
                var oldstm = getstm(p);
                if (!oldstm.Equals(default(TStm)))
                {
                    markstm(p, oldstm, MarkStmType.LeftStAndCenter);
                }
            }
            for (; rightStmStackIndex < rightStack.Count(); )
            {
                var p = (T)rightStack.Pop();
                var oldstm = getstm(p);
                if (!oldstm.Equals(default(TStm)))
                {
                    markstm(p, oldstm, MarkStmType.RightStAndCenter);
                }
            }
            if (!leftStm.Equals(default(TStm)))
            {
                for (; leftStack.Count() > 0; )
                {
                    var n = leftStack.Pop();
                    markstm((T)n, leftStm, MarkStmType.LeftStAndCenter);
                }
                if (left.Left != null)
                {
                    markstm((T)left.Left, leftStm, MarkStmType.CenterStOnly);
                }
            }
            if (!rightStm.Equals(default(TStm)))
            {
                for (; rightStack.Count() > 0;)
                {
                    var n = rightStack.Pop();
                    markstm((T)n, rightStm, MarkStmType.RightStAndCenter);
                }
                if (right.Right != null)
                {
                    markstm((T)right.Right, rightStm, MarkStmType.CenterStOnly);
                }
            }
        }
    }
}
