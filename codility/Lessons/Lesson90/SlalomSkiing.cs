#define TEST
#define NEW_IMPLEMENTATION2
using System.Linq;
using System;
using codility.Helpers;

#if TEST
using System.Collections.Generic;
using codility.TestFramework;
using codility.Lib.SmartArray;

namespace codility.Lessons.Lesson90
{
    using Sa = Algo<SlalomSkiing.Pod>;

    class SlalomSkiing : ITestee
#else
    // TODO add the lib code here...

    class Solution
#endif
    {
#if NEW_IMPLEMENTATION2
        public class Pod : Node<Pod>, IComparable<Pod>
        {
            public int XPos;
            public int YPos;
            public int Index;

            public new Pod Parent => (Pod)base.Parent;
            public new Pod Left => (Pod)base.Left;
            public new Pod Right => (Pod)base.Right;

            // 0 being none
            public int[] Max = new int[3];
            public int[] MaxSt = new int[3];

            public int CompareTo(Pod other)
                => XPos.CompareTo(other.XPos);
        }

        private static Sa.MarkDelegate GenMark(int c, int m)
            => (p, mt) =>
            {
                p.Max[c] = m;
                switch(mt)
                {
                    case Sa.MarkType.LeftAndCenter:
                        if (p.Right == null || p.Right.MaxSt[c] == m)
                        {
                            p.MaxSt[c] = m;
                        }
                        else
                        {
                            if(p.Left != null)
                            {
                                p.Left.MaxSt[c] = m;
                            }
                            p.Max[c] = m;
                        }
                        break;
                    case Sa.MarkType.RightAndCenter:
                        if (p.Left == null || p.Left.MaxSt[c] == m)
                        {
                            p.MaxSt[c] = m;
                        }
                        else
                        {
                            if (p.Right == null)
                            {
                                p.Right.MaxSt[c] = m;
                            }
                            p.Max[c] = m;
                        }
                        break;
                    case Sa.MarkType.CenterAndCheck:
                        if ((p.Left == null || p.Left.MaxSt[c] == m)
                            && (p.Right == null || p.Right.MaxSt[c] == m))
                        {
                            p.MaxSt[c] = m;
                        }
                        else
                        {
                            p.Max[c] = m;
                        }
                        break;
                    case Sa.MarkType.CheckAll:
                        if ((p.Left == null || p.Left.MaxSt[c] == m)
                            && (p.Right == null || p.Right.MaxSt[c] == m)
                            && p.Max[c] == m)
                        {
                            p.MaxSt[c] = m;
                        }
                        break;
                }
            };
        
        private static void Mark(Pod left, Pod right, int c, int m)
            => Sa.MarkRange(left, right, GenMark(c, m));

        private static int GetMark(Pod root, int i, int c)
        {
            var p = Sa.FindMark(root, GetFinder(i), GetHasMark(c));
            if (p == null) return 0;
            if (p.MaxSt[c] > 0) return p.MaxSt[c];
            return p.Max[c];
        }

        private static Predicate<Pod> GetHasMark(int c)
            => pod => pod.Max[c] > 0 || pod.MaxSt[c] > 0;
            
        private static Func<Pod, int> GetFinder(int i)
            => p => i.CompareTo(p.Index);
        
        static int FindFirstNoLess(Pod root, int t, int c, bool rev, int len)
        {
            var lastp = root;
            for (var p = root; p != null;)
            {
                if (p.MaxSt[c] > 0)
                {
                    var cmp = t.CompareTo(p.MaxSt[c]);
                    if (rev)
                    {
                        if (cmp > 0)
                        {

                        }
                    }
                    else
                    {
                        if (cmp > 0)
                        {

                        }
                    }
                }
                else
                {
                    lastp = p;
                    var cmp = t.CompareTo(p.Max[c]);
                    if (rev)
                    {
                        p = cmp > 0 ? p.Left : p.Right;
                    }
                    else
                    {
                        p = cmp > 0 ? p.Right : p.Left;
                    }
                }
            }
            if (lastp.Max[c] <= t)
            {
                return rev ? lastp.Index - 1 : lastp.Index + 1;
            }
            return lastp.Index;
        }

        public int solution(int[] A)
        {
            var N = A.Length;
            var pods = Sa.Load(A, x => new Pod { XPos = x }).ToArray();
            for (var i = 0; i < N; i++)
            {
                pods[i].YPos = i;
            }
            Array.Sort(pods);
            for (var i = 0; i < N; i++)
            {
                pods[i].Index = i;
            }

            var ytopod = new int[N];
            for (var i = 0; i < N; i++)
            {
                ytopod[pods[i].YPos] = i;
            }

            var root = Sa.Treeify(pods);

            var max0 = 0;
            var max1 = 0;
            var max2 = 0;
            Console.WriteLine($"N = {N}");
            for (var i = 0; i < N; i++)
            {
                var podi = ytopod[i];
                var pod = pods[podi];

                var pod0 = i == 0 ? 0 : GetMark(root, podi, 0);
                var pod1 = i == 0 ? 0 : GetMark(root, podi, 1);
                var pod2 = i == 0 ? 0 : GetMark(root, podi, 2);

                Console.WriteLine($"{i}: {podi}: {pod0} {pod1} {pod2}");

                // 1
                var new0 = pod0 + 1;
                var end0 = i == 0 ? N : FindFirstNoLess(root, new0, 0, false, N);
                Console.WriteLine($" mark {pod.Index} to {end0 - 1} as {new0}");
                Mark(pod, pods[end0 - 1], 0, new0);

                // 2
                var new1 = Math.Max(pod1 + 1, max0 + 1);
                var end1 = i == 0 ? -1 : FindFirstNoLess(root, new1, 1, true, N);
                Console.WriteLine($" mark {end1 + 1} to {pod.Index} as {new1}");
                Mark(pods[end1 + 1], pod, 1, new1);


                // 3
                var new2 = Math.Max(pod2 + 1, max1 + 1); //todo also consider max0+1?
                var end2 = i == 0 ? N : FindFirstNoLess(root, new2, 2, false, N);
                Console.WriteLine($" mark {pod.Index} to {end2 - 1} as {new2}");
                Mark(pod, pods[end2 - 1], 2, new2);

                if (new0 > max0) max0 = new0;
                if (new1 > max1) max1 = new1;
                if (new2 > max2) max2 = new2;
            }
            return Math.Max(Math.Max(max0, max1), max2);
        }

#elif NEW_IMPLEMENTATION
        class Node : IComparable<Node>
        {
            public int XPos { get; set; }
            public int YPos { get; set; }

            public int CompareTo(Node other)
                => XPos.CompareTo(other.XPos);

            public Node[] Children { get; } = new Node[2];

#if TEST
            public Node Left { get => Children[0]; set => Children[0] = value; }
            public Node Right { get => Children[1]; set => Children[1] = value; }
#else
            public Node Left { get { return Children[0];} set { Children[0] = value; }}
            public Node Right { get { return Children[1];} set { Children[1] = value; } }
#endif
            public Node Parent { get; set; }

            public Node Prev { get; set; }
            public Node Next { get; set; }

            public int[] Max { get; private set; } 
            public bool ChildrenHasValue { get; private set; }

            public bool HasValue => Max != null;

            public bool SubtreeHasValue => HasValue || ChildrenHasValue;

            public void SetMax (int a, int b, int c)
            {
                Max = new int[] { a, b, c };
                for (var p = Parent; p != null; p = p.Parent)
                {
                    p.ChildrenHasValue = true;
                }
            }

            // Find the first child having value and with index next to this in the specified direction 
            // 0 - left, 1 - right
            private Node FindChildNeighbour(int d) 
            {
                for (var p = Children[d]; p?.SubtreeHasValue == true;)
                {
                    var c1 = p;
                    var c2 = p.Children[1-d];
                    if (c2?.ChildrenHasValue == true)
                    {
                        p = c2;
                    }
                    else if (c2?.HasValue == true)
                    {
                        if (!c2.SubtreeHasValue)
                        {
                            return c2;
                        }
                        p = c2;
                    }
                    else if (c1?.ChildrenHasValue == true)
                    {
                        p = c1.Children[d];
                    }
                    else if (c1?.HasValue == true)
                    {
                        return c1;
                    }
                }
                return null;
            }

            // Find the leftmost (d == 0) or rightmost (d == 1) valueful in the tree
            private Node FindMax(int d)
            {
                for (var p = this; p?.SubtreeHasValue == true; )
                {
                    var s = p.Children[d];
                    if (s?.SubtreeHasValue == true)
                    {
                        p = s;
                    }
                    else if (p.HasValue)
                    {
                        return p;
                    }
                    else
                    {
                        p = p.Children[1-d];
                    }
                }
                return null;
            }

            private Node FindNeighbour(int d = 0)
            {
                var p = FindChildNeighbour(d);
                if (p != null)
                {
                    return p;
                }

                var p1 = this;
                var p2 = p1.Parent; 
                for (; p2 != null; p2 = p2.Parent)
                {
                    if (p2.Children[1-d] == p1)
                    {
                        if (p2.HasValue) 
                        {
                            return p2;
                        }
                        var other = p2.Children[d];
                        if (other != null && other.SubtreeHasValue)
                        {
                            return other.FindMax(1-d);
                        }
                    }
                    p1 = p2;
                }
                return null;
            }

            public void Add()
            {
                var prev = FindNeighbour(0);
                if (prev != null)
                {
                    var next = prev.Next;
                    prev.Next = this;
                    Prev = prev;
                    Next = next;
                    if (next != null)
                    {
                        next.Prev = this;
                    }
                }
                else
                {
                    var next = FindNeighbour(1);
                    if (next != null)
                    {
                        next.Prev = this;
                        Next = next;
                    }
                }
            }
        }

        public int solution(int[] A)
        {
            var N = A.Length;
            var props = A.Select(x => new Node { XPos = x }).ToArray();
            for (var i = 0; i < N; i++)
            {
                props[i].XPos = A[i];
                props[i].YPos = i;
            }
            Array.Sort(props);

            var ytoprop = new int[N];
            for (var i = 0; i < N; i++)
            {
                ytoprop[props[i].YPos] = i;
            }

            var root = Treeify(props, 0, props.Length);

            var propfirst = props[ytoprop[0]];
            propfirst.SetMax(1, 1, 1);

            var global_max0 = 1;
            var global_max1 = 1;
            var global_max2 = 1;

            for (var i = 1; i < N; i++)
            {
                var prop = props[ytoprop[i]];

                prop.Add();

                int max0;
                int max1;
                int max2;

                // \
                if (prop.Prev != null)
                {
                    max0 = prop.Prev.Max[0] + 1;
                }
                else
                {
                    max0 = 1;
                }
              
                // \/
                if (prop.Next != null)
                {
                    max1 = prop.Next.Max[1] + 1;
                }
                else
                {
                    max1 = 1;
                }
                if (max1 < global_max0 + 1)
                {
                    max1 = global_max0 + 1;
                }
               
                // \/\
                if (prop.Prev != null)
                {
                    max2 = prop.Prev.Max[2] + 1;
                }
                else
                {
                    max2 = 1;
                }
                if (max2 < global_max1 + 1)
                {
                    max2 = global_max1 + 1;
                }

                prop.SetMax(max0, max1, max2);

                if (i < N - 1)
                {
                    for (var p = prop.Next; p != null && p.Max[0] < max0; p = p.Next)
                    {
                        p.Max[0] = max0;
                    }
                    for (var p = prop.Prev; p != null && p.Max[1] < max1; p = p.Prev)
                    {
                        p.Max[1] = max1;
                    }
                    for (var p = prop.Next; p != null && p.Max[2] < max2; p = p.Next)
                    {
                        p.Max[2] = max2;
                    }
                }

                if (max0 > global_max0)
                {
                    global_max0 = max0;
                }
                if (max1 > global_max1)
                {
                    global_max1 = max1;
                }
                if (max2 > global_max2)
                {
                    global_max2 = max2;
                }
            }
            return Math.Max(Math.Max(global_max0, global_max1), global_max2);
        }

        private Node Treeify(Node[] props, int start, int len)
        {
            if (len == 0) return null;
            if (len == 1) return props[start];

            var n = len;
            var l = 0;
            for (; n > 0; n >>= 1, l++) ;
            var m = 1 << (l - 1) - 1;
            var root = props[start + m];

            root.Left = Treeify(props, start, m);
            root.Right = Treeify(props, start + m + 1, len - m - 1);
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
#else
        const int NumTypes = 6;

    class Track
    {
        public enum Directions
        {
            Left = 0,
            Right
        }

        public int Passes;
        public int Turns;
        public Directions Direction;
        public int TypeIndex => GetTypeIndex(Direction, Turns);

        public static int GetTypeIndex(Directions dir, int turns)
            => (int)dir + turns * 2;

        public static Track First(Directions dir)
        {
            return new Track
            {
                Passes = 1,
                Turns = 0,
                Direction = dir
            };
        }

        public Track Follow()
            => new Track
            {
                Passes = Passes + 1,
                Turns = Turns,
                Direction = Direction,
            };

        public Track Turn()
            => new Track
            {
                Passes = Passes + 1,
                Turns = Turns + 1,
                Direction = 1 - Direction
            };
    }
        W
    bool Allowed(Track t)
        => t.Turns % 2 == 0 ? t.Direction == Track.Directions.Left
        : t.Direction == Track.Directions.Right;

    bool Allowed(int typeIndex)
    {
        var dir = (Track.Directions)(typeIndex % 2);
        var turns = typeIndex / 2;
        return turns % 2 == 0 ? dir == Track.Directions.Left
            : dir == Track.Directions.Right;
    }

    public int solution(int[] A)
    {
        var dp = new Track[A.Length][];
        for (var i = 0; i < A.Length; i++)
        {
            Solve(A, i, dp);
        }
        var max = 0;
        for (var i = A.Length - 1; i >= 0; i--)
        {
            if (max > i + 1) break;
            var d = dp[i];
            var p = d.Max(x => x?.Passes ?? 0);
            if (p > max) max = p;
        }
        return max;
    }

    void Solve(int[] A, int p, Track[][] dp)
    {
        dp[p] = new Track[NumTypes];
        var a = A[p];
        for (var dir = Track.Directions.Left; dir <= Track.Directions.Right; dir++)
        {
            var b = Track.First(dir);
            if (Allowed(b))
            {
                dp[p][b.TypeIndex] = b;
            }
            if (p == 0) continue;
            var badcomp = dir > 0 ? 1 : -1;
            for (var targetTurns = 0; targetTurns <= 2; targetTurns++)
            {
                var type = Track.GetTypeIndex(dir, targetTurns);
                if (!Allowed(type)) continue;
                var maxPasses = 0;
                for (var j = p - 1; j >= maxPasses; j--)
                {
                    var aj = A[j];
                    if (a.CompareTo(aj) == badcomp) continue;
                    var solj = dp[j][type];
                    if (solj == null) continue;
                    if (solj.Passes > maxPasses)
                    {
                        maxPasses = solj.Passes;
                        dp[p][type] = solj.Follow();
                    }
                }
                if (targetTurns > 0)
                {
                    var type2 = Track.GetTypeIndex(1 - dir, targetTurns - 1);
                    for (var j = p - 1; j >= maxPasses; j--)
                    {
                        var aj = A[j];
                        if (a.CompareTo(aj) == badcomp) continue;
                        var solj = dp[j][type2];
                        if (solj == null) continue;
                        if (solj.Passes > maxPasses)
                        {
                            maxPasses = solj.Passes;
                            dp[p][type] = solj.Turn();
                        }
                    }
                }
            }
        }
    }
#endif

#if TEST
            public object Run(params object[] args)
            => solution((int[])args[0]);

        public class Tester : BaseSelfTester<SlalomSkiing>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 15, 13, 5, 7, 4, 10, 12, 8, 2, 11, 6, 9, 3 }, 8);
                yield return CreateSingleInputSet(new[] { 1, 5 }, 2);
            }
        }

        public class Profiler : BaseSelfProfiler<SlalomSkiing>
        {
            public override IEnumerable<BaseTester.TestSet> GetProfilingTestSets()
            {
                const int size = 16000;
                var rand = new Random(123);
                var seq = rand.GenerateRandomSequence(size);
                yield return BaseTester.CreateInputSet(null, seq);
                var monotonic = Enumerable.Range(1, 90000).ToArray();
                yield return BaseTester.CreateInputSet(null, monotonic);
            }
        }
    }
#endif
}
