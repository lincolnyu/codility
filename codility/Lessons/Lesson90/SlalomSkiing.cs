#define TEST

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
<<<<<<< HEAD
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
=======
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
>>>>>>> 25d53787cd726a1218d9d4ecd41b8cef5fca42c3
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
                const int size = 32768;
                var rand = new Random(123);
                var seq = rand.GenerateRandomSequence(size);
                yield return BaseTester.CreateInputSet(null, seq);
            }
        }
    }
#endif
}
