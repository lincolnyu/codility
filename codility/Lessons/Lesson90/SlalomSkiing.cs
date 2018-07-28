#define TEST
//#define PROFILING
//#define DEBUG_PROGRAM
//#define PRINT_PROGRAM

using System.Linq;
using System;
using System.Collections.Generic;

#if TEST

using codility.Lib.SmartArray;
using codility.Helpers;
using codility.TestFramework;

namespace codility.Lessons.Lesson90
{
    using Sa = Algo<SlalomSkiing.Pod>;

    class SlalomSkiing : ITestee
#else
    using Sa = Algo<Solution.Pod>;

// TODO add the lib code here...

    class Solution
#endif
    {
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

        private static Sa.MarkDelegate<int> GenMark(int c, int m)
            => (p, mt) =>
            {
                switch(mt)
                {
                case Sa.MarkType.LeftAndCenter:
                    if (p.Right == null || p.Right.MaxSt[c] == m)
                    {
                        p.MaxSt[c] = m;
                    }
                    else
                    {
                        if (p.Left != null)
                        {
                            p.Left.MaxSt[c] = m;
                        }
                        p.Max[c] = m;
                        p.MaxSt[c] = 0;
                    }
                    break;
                case Sa.MarkType.RightAndCenter:
                    if (p.Left == null || p.Left.MaxSt[c] == m)
                    {
                        p.MaxSt[c] = m;
                    }
                    else
                    {
                        if (p.Right != null)
                        {
                            p.Right.MaxSt[c] = m;
                        }
                        p.Max[c] = m;
                        p.MaxSt[c] = 0;
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
                        p.MaxSt[c] = 0;
                    }
                    break;
                }
            };

        private static Sa.MarkStmelegate<int> GenMarkStm(int c)
            => (n, stm, mst) =>
            {
                switch (mst )
                {
                    case Sa.MarkStmType.LeftStAndCenter:
                        n.MaxSt[c] = 0;
                        n.Max[c] = stm;
                        if (n.Left != null)
                        {
                            n.Left.MaxSt[c] = stm;
                        }
                        break;
                    case Sa.MarkStmType.RightStAndCenter:
                        n.MaxSt[c] = 0;
                        n.Max[c] = stm;
                        if (n.Right != null)
                        {
                            n.Right.MaxSt[c] = stm;
                        }
                        break;
                    case Sa.MarkStmType.CenterStOnly:
                        n.MaxSt[c] = stm;
                        break;
                }
            };

        private static Func<Pod, int> GenGetStm(int c)
            => p => p.MaxSt[c];

        private static void Mark(Pod left, Pod right, int c, int m)
            => Sa.MarkRange<int>(left, right, GenMark(c, m), GenMarkStm(c), GenGetStm(c));

        private static int GetMark(Pod root, int i, int c)
        {
            var p = Sa.FindMark(root, GetFinder(i), GetHasMark(i, c));
            if (p == null) return 0;
            if (p.MaxSt[c] > 0) return p.MaxSt[c];
            return p.Max[c];
        }

        private static Predicate<Pod> GetHasMark(int i, int c)
            => pod => (pod.Index == i && pod.Max[c] > 0) || pod.MaxSt[c] > 0;
            
        private static Func<Pod, int> GetFinder(int i)
            => p => i.CompareTo(p.Index);

        private static Pod GetLeftmost(Pod pod)
        {
            var pold = pod;
            for (pod = pod.Left; pod != null; pod = pod.Left)
            {
                pold = pod;
            }
            return pold;
        }

        private static Pod GetRightmost(Pod pod)
        {
            var pold = pod;
            for (pod = pod.Right; pod != null; pod = pod.Right)
            {
                pold = pod;
            }
            return pold;
        }

        static int FindFirstNoLess(Pod root, int t, int c, bool rev, int len)
        {
            var lastp = root;
            int lastMax = 0;
            for (var p = root; p != null;)
            {
                if (p.MaxSt[c] > 0)
                {
                    lastMax = p.MaxSt[c];
                    var cmp = t.CompareTo(lastMax);
                    lastp = (rev ^ (cmp > 0)) ? GetRightmost(p) : GetLeftmost(p);
                    break;
                }
                else
                {
                    lastMax = p.Max[c];
                    var cmp = t.CompareTo(lastMax);
                    lastp = p;
                    p = (rev ^ (cmp > 0)) ? p.Right : p.Left;
                }
            }

            return (lastMax < t) ? (rev ? lastp.Index - 1 : lastp.Index + 1) : lastp.Index;
        }

#if PROFILING
        class SpeedProfiler
        {
            public class Entry
            {
                public TimeSpan TotalDuration;
                public DateTime Start;
            }

            public Dictionary<string, Entry> DurationMap = new Dictionary<string, Entry>();

            public void Start(string name)
            {
                if (!DurationMap.TryGetValue(name, out var entry))
                {
                    entry = new Entry();
                    DurationMap[name] = entry;
                }
                entry.Start = DateTime.UtcNow;
            }
            public void Stop(string name)
            {
                if (DurationMap.TryGetValue(name, out var entry))
                {
                    entry.TotalDuration += DateTime.UtcNow - entry.Start;
                }
            }
            public void Display()
            {
                foreach (var kvp in DurationMap)
                {
                    var key = kvp.Key;
                    var val = kvp.Value;
                    Console.WriteLine($"{key} : {val.TotalDuration.TotalMilliseconds} ms");
                }
            }
        }
#endif

        public int solution(int[] A)
        {
#if PROFILING
            var profiler = new SpeedProfiler();
            profiler.Start("Loading and Sorting");
#endif

            var N = A.Length;
#if PRINT_PROGRAM
            Console.WriteLine("A:");
            for (var i = 0; i < N; i++)
            {
                Console.Write($" {A[i]}");
            }
            Console.WriteLine();
#endif
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
#if PROFILING
            profiler.Stop("Loading and Sorting");
            profiler.Start("Treefication");
#endif
            var root = Sa.Treeify(pods);
#if PROFILING
            profiler.Stop("Treefication");
            profiler.Start("DP");
#endif
            var max0 = 0;
            var max1 = 0;
            var max2 = 0;

#if DEBUG_PROGRAM
            Console.WriteLine($"N = {N}");
            void SnapShot(out int[] p0, out int[] p1, out int[] p2)
            {
                p0 = new int[N];
                p1 = new int[N];
                p2 = new int[N];
                for (var i = 0; i < N; i++)
                {
                    p0[i] = GetMark(root, i, 0);
                    p1[i] = GetMark(root, i, 1);
                    p2[i] = GetMark(root, i, 2);
                }
            }
            void Print(int c)
            {
                for (var i = 0; i < N; i++)
                {
                    var m = GetMark(root, i, c);
                    Console.Write($" {m}");
                }
                Console.WriteLine();
            }
            void PrintSnapshot(int[] snapshot)
            {
                for (var i = 0; i < N; i++)
                {
                    Console.Write($" {snapshot[i]}");
                }
                Console.WriteLine();
            }
            bool CheckMonotonity(int c, bool decrease)
            {
                int? last = null;
                for (var i = 0; i < N; i++)
                {
                    var m = GetMark(root, i, c);
                    if (!decrease && m < last)
                    {
                        return false;
                    }
                    if (decrease && m > last)
                    {
                        return false;
                    }
                    last = m;
                }
                return true;
            }
#endif

            for (var i = 0; i < N; i++)
            {
                var podi = ytopod[i];
                var pod = pods[podi];

#if PROFILING
                profiler.Start("Get Mark");
#endif

                var pod0 = i == 0 ? 0 : GetMark(root, podi, 0);
                var pod1 = i == 0 ? 0 : GetMark(root, podi, 1);
                var pod2 = i == 0 ? 0 : GetMark(root, podi, 2);

#if PROFILING
                profiler.Stop("Get Mark");
#endif

#if PRINT_PROGRAM
                Console.WriteLine($"{i}:");
#endif
#if DEBUG_PROGRAM
                SnapShot(out var oldp0, out var oldp1, out var oldp2);
#endif

                // 0
                var new0 = pod0 + 1;

#if PROFILING
                profiler.Start("Find First");
#endif
                var end0 = i == 0 ? N : FindFirstNoLess(root, new0, 0, false, N);
#if PROFILING
                profiler.Stop("Find First");
                profiler.Start("Mark");
#endif
                Mark(pod, pods[end0 - 1], 0, new0);
#if PROFILING
                profiler.Stop("Mark");
#endif
#if DEBUG_PROGRAM
                var cr = CheckMonotonity(0, false);
#if !PRINT_PROGRAM
                if (!cr)
#endif
                {
#endif
#if DEBUG_PROGRAM || PRINT_PROGRAM
                    Console.WriteLine($" {i} cmp 0 mark {pod.Index} to {end0 - 1} as {new0}:");
                    PrintSnapshot(oldp0);
                    Print(0);
#endif
#if DEBUG_PROGRAM
                    System.Diagnostics.Debug.Assert(cr);
                }
#endif
                // 1
#if PROFILING
                profiler.Start("Find First");
#endif
                var new1 = Math.Max(pod1 + 1, max0 + 1);
                var end1 = i == 0 ? -1 : FindFirstNoLess(root, new1, 1, true, N);
#if PROFILING
                profiler.Stop("Find First");
                profiler.Start("Mark");
#endif
                Mark(pods[end1 + 1], pod, 1, new1);
#if PROFILING
                profiler.Stop("Mark");
#endif
#if DEBUG_PROGRAM
                cr = CheckMonotonity(1, true);
#if !PRINT_PROGRAM
                if (!cr)
#endif
                {
#endif
#if DEBUG_PROGRAM || PRINT_PROGRAM
                    Console.WriteLine($" {i} cmp 1 mark {end1 + 1} to {pod.Index} as {new1}:");
                    PrintSnapshot(oldp1);
                    Print(1);
#endif
#if DEBUG_PROGRAM
                    System.Diagnostics.Debug.Assert(cr);
                }
#endif
                // 2
#if PROFILING
                profiler.Start("Find First");
#endif
                var new2 = Math.Max(pod2 + 1, max1 + 1); //todo also consider max0+1?
                var end2 = i == 0 ? N : FindFirstNoLess(root, new2, 2, false, N);
#if PROFILING
                profiler.Stop("Find First");
                profiler.Start("Mark");
#endif
                Mark(pod, pods[end2 - 1], 2, new2);
#if PROFILING
                profiler.Stop("Mark");
#endif
#if DEBUG_PROGRAM
                cr = CheckMonotonity(2, false);
#if !PRINT_PROGRAM
                if (!cr)
#endif
                {
#endif
#if DEBUG_PROGRAM || PRINT_PROGRAM
                    Console.WriteLine($" {i} cmp 2 mark {pod.Index} to {end2 - 1} as {new2}:");
                    PrintSnapshot(oldp2);
                    Print(2);
#endif
#if DEBUG_PROGRAM
                    System.Diagnostics.Debug.Assert(cr);
                }
#endif

                if (new0 > max0) max0 = new0;
                if (new1 > max1) max1 = new1;
                if (new2 > max2) max2 = new2;
            }
#if PROFILING
            profiler.Stop("DP");
            profiler.Display();
#endif
            return Math.Max(Math.Max(max0, max1), max2);
        }

        class RefSolution
        {
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
        }

#if TEST
        public object Run(params object[] args)
            => solution((int[])args[0]);

        public class Tester : BaseSelfTester<SlalomSkiing>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                {
                    var rand = new Random(1);
                    var seq = rand.GenerateRandomSequence(9);
                    var rs = new RefSolution();
                    var expected = rs.solution(seq);
                    yield return CreateSingleInputSet(seq, expected);
                }

                {
                    var rand = new Random(18);
                    var seq = rand.GenerateRandomSequence(8);
                    var rs = new RefSolution();
                    var expected = rs.solution(seq);
                    yield return CreateSingleInputSet(seq, expected);
                }

                {
                    var rand = new Random(123);
                    var seq = rand.GenerateRandomSequence(16000);
                    var rs = new RefSolution();
                    var expected = rs.solution(seq);
                    yield return CreateSingleInputSet(seq, expected);
                }

                yield return CreateSingleInputSet(new[] { 5, 6, 7, 8, 9, 10, 4, 3, 1, 2 }, 10);
                yield return CreateSingleInputSet(new[] { 1, 10, 4, 2, 7, 5, 9, 8, 6, 3 }, 7);
                yield return CreateSingleInputSet(new[] { 15, 13, 5, 7, 4, 10, 12, 8, 2, 11, 6, 9, 3 }, 8);
                yield return CreateSingleInputSet(new[] { 1, 5 }, 2);
            }
        }

        public class Profiler : BaseSelfProfiler<SlalomSkiing>
        {
            public override IEnumerable<BaseTester.TestSet> GetProfilingTestSets()
            {
                const int size = 100000;
                var rand = new Random(123);
                var seq = rand.GenerateRandomSequence(size);
                yield return BaseTester.CreateInputSet(null, seq);
                var monotonic = Enumerable.Range(1, 90000).ToArray();
                yield return BaseTester.CreateInputSet(null, monotonic);
            }
        }
#endif
    } // class Solution or SlalomSkiing

#if TEST
}
#endif
