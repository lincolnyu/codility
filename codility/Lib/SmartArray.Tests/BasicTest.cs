using System;
using System.Diagnostics;
using System.Linq;

namespace codility.Lib.SmartArray.Tests
{
    using A = Algo<Pod>;

    class Pod : Node<Pod>, IComparable<Pod>
    {
        public int Value;
        public int? MarkSelf;
        public int? MarkSubtree;

        public int CompareTo(Pod other)
            => Value.CompareTo(other.Value);
    }

    class BasicTest
    {
        public void Test()
        {
            var raw = new int[]
            {
                2,5,6,3,7,13,12,1,9,4,8
            };

            var ilist = A.Load(raw, i => new Pod { Value = i }).ToArray();
            Array.Sort(ilist);

            var root = A.Treeify(ilist);
            // 1,2,3,4,5,6,7,8,9,12,13

            var genmark = new Func<int, A.MarkDelegate<int?>>(
                m => new A.MarkDelegate<int?>
                    (
                        (p, mt) =>
                        {
                            switch (mt)
                            {
                                case A.MarkType.LeftAndCenter:
                                    p.MarkSelf = m;
                                    if (p.Left != null)
                                    {
                                        ((Pod)p.Left).MarkSubtree = m;
                                    }
                                    break;
                                case A.MarkType.RightAndCenter:
                                    p.MarkSelf = m;
                                    if (p.Left != null)
                                    {
                                        ((Pod)p.Right).MarkSubtree = m;
                                    }
                                    break;
                                case A.MarkType.CenterAndCheck:
                                    if ((p.Left == null || ((Pod)p.Left).MarkSubtree == m)
                                        && (p.Right == null || ((Pod)p.Right).MarkSubtree == m))
                                    {
                                        p.MarkSubtree = m;
                                    }
                                    else
                                    {
                                        p.MarkSelf = m;
                                    }
                                    break;
                            }
                        }
                    )
            );
            
            var getfinder = new Func<int, Func<Pod, int>>(
                 i => new Func<Pod, int>(p =>
                         i.CompareTo(p.Value))
                );

            var gethasmark = new Func<int, int, Predicate<Pod>>(
                (i, m) => new Predicate<Pod>(pod =>
                {
                    if (pod.Value == i)
                    {
                        return pod.MarkSelf == m;
                    }
                    return pod.MarkSubtree == m;
                }));

            var gethasanymark = new Func<int, Predicate<Node>>(
                i => new Predicate<Node>(n =>
                {
                    var pod = (Pod)n;
                    if (pod.Value == i)
                    {
                        return pod.MarkSubtree.HasValue || pod.MarkSelf.HasValue;
                    }
                    return pod.MarkSubtree.HasValue;
                }));

            void markstm(Pod pod, int? stm, A.MarkStmType mst)
            {
                switch (mst)
                {
                    case A.MarkStmType.LeftStAndCenter:
                        pod.MarkSelf = stm;
                        if (pod.Left != null)
                        {
                            ((Pod)pod.Left).MarkSubtree = stm;
                        }
                        break;
                    case A.MarkStmType.RightStAndCenter:
                        pod.MarkSelf = stm;
                        if (pod.Right != null)
                        {
                            ((Pod)pod.Right).MarkSubtree = stm;
                        }
                        break;
                    case A.MarkStmType.CenterStOnly:
                        pod.MarkSubtree = stm;
                        break;
                }
            }

            int? getstm(Pod pod) => pod.MarkSubtree;

            var p3 = A.Find(root, getfinder(3));
            var p6 = A.Find(root, getfinder(6));
            
            A.MarkRange<int?>(p3, p6, genmark(5), markstm, getstm);

            for (var i = 0; i < ilist.Length; i++)
            {
                var finder = getfinder(i);
                if (i >= 3 && i <= 6)
                {
                    var r = A.FindMark(root, finder, gethasmark(i, 5));
                    Debug.Assert(r != null);
                }
                else
                {
                    var r = A.FindMark(root, finder, gethasanymark(i));
                    Debug.Assert(r == null);
                }
            }

            var p5 = A.Find(root, getfinder(5));
            var p9 = A.Find(root, getfinder(9));
            A.MarkRange<int?>(p5, p9, genmark(3), markstm, getstm);

            for (var i = 0; i < ilist.Length; i++)
            {
                var finder = getfinder(i);
                if (i >= 3 && i <= 4)
                {
                    var r = A.FindMark(root, finder, gethasmark(i, 5));
                    Debug.Assert(r != null);
                }
                else if (i >= 5 && i <= 9)
                {
                    var r = A.FindMark(root, finder, gethasmark(i, 3));
                    Debug.Assert(r != null);
                }
                else
                {
                    var r = A.FindMark(root, finder, gethasanymark(i));
                    Debug.Assert(r == null);
                }
            }

            Console.WriteLine("Successful!");
        }
    }
}
