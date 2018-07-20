#define TEST
#define NEW_IMPLEMENTATION
using System.Linq;
using System;
using codility.Helpers;

#if TEST
using System.Collections.Generic;
using codility.TestFramework;
namespace codility.Lessons.Lesson90
{
    class SlalomSkiing : ITestee
#else
class Solution
#endif
{
#if NEW_IMPLEMENTATION
        class Node : IComparable<Node>
        {
            public int XPos { get; set; }
            public int YPos { get; set; }

            public int CompareTo(Node other)
                => XPos.CompareTo(other.XPos);

            public Node[] Children { get; } = new Node[2];
            public Node Left { get => Children[0]; set => Children[0] = value; }
            public Node Right { get => Children[1]; set => Children[1] = value; }

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

            var root = Treeize(props, 0, props.Length);

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

        private Node Treeize(Node[] props, int start, int len)
        {
            if (len == 0) return null;
            if (len == 1) return props[start];

            var n = len;
            var l = 0;
            for (; n > 0; n >>= 1, l++) ;
            var m = 1 << (l - 1) - 1;
            var root = props[start + m];

            root.Left = Treeize(props, start, m);
            root.Right = Treeize(props, start + m + 1, len - m - 1);
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
            }
        }
    }
#endif
}
