using System.Collections.Generic;
using System.Linq;
using codility.TestFramework;

namespace codility.Lessons.Lesson90
{
    class SlalomSkiing : ITestee
    {
        class Solution
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

            public static Solution First(Directions dir)
            {
                return new Solution
                {
                    Passes = 1,
                    Turns = 0,
                    Direction = dir
                };
            }

            public Solution Follow()
                => new Solution
                {
                    Passes = Passes + 1,
                    Turns = Turns,
                    Direction = Direction,
                };

            public Solution Turn()
                => new Solution
                {
                    Passes = Passes + 1,
                    Turns = Turns + 1,
                    Direction = 1 - Direction
                };

            private static int Compare(Solution a, Solution b)
            {
                // a.Turns >= b.Turns
                var diffPasses = a.Passes - b.Passes;
                if (a.Turns + b.Passes > b.Turns + a.Passes)
                {
                    return 1;
                }
                return 0;
            }

            public int CompareTo(Solution other)
            {
                if (Direction != other.Direction) return 0;
                if (Turns > other.Turns) return Compare(this, other);
                if (other.Turns > Turns) return Compare(other, this);
                if (Compare(this, other) == 1) return 1;
                if (Compare(other, this) == 1) return -1;
                return 0;
            }
        }

        const int NumTypes = 6;

        int Solve(int[] A)
        {
            var dp = new Solution[A.Length][];
            for (var i = 0; i <A.Length; i++)
            {
                Solve(A, i, dp);
            }
            var max = 0;
            for (var i = A.Length - 1; i>=0; i--)
            {
                if (max > i + 1) break;
                var d = dp[i];
                var p = d.Max(x => x?.Passes ?? 0);
                if (p > max) max = p;
            }
            return max;
        }

        void Solve(int[] A, int p, Solution[][] dp)
        {
            dp[p] = new Solution[NumTypes];
            var a = A[p];
            for (var dir = Solution.Directions.Left; dir <= Solution.Directions.Right; dir++)
            {
                var b = Solution.First(dir);
                dp[p][b.TypeIndex] = b;
                if (p == 0) continue;
                Solution last = null;
                var badcomp = dir > 0 ? -1 : 1;
                for (var targetTurns = 0; targetTurns <= 2; targetTurns++)
                {
                    var type = Solution.GetTypeIndex(dir, targetTurns);
                    int? maxPasses = null;
                    for (var j = p - 1; j >= 0; j--)
                    {
                        var aj = A[j];
                        if (a.CompareTo(aj) == badcomp) continue;
                        var solj = dp[j][type];
                        if (solj == null) continue;
                        maxPasses = solj.Passes;
                        dp[p][type] = solj.Follow();
                        break;
                    }
                    if (targetTurns > 0)
                    {
                        var type2 = Solution.GetTypeIndex(1 - dir, targetTurns - 1);
                        for (var j = p - 1; j >= 0; j--)
                        {
                            var aj = A[j];
                            if (a.CompareTo(aj) == badcomp) continue;
                            var solj = dp[j][type2];
                            if (solj == null) continue;
                            if (maxPasses == null || solj.Passes > maxPasses.Value)
                            {
                                dp[p][type] = solj.Turn();
                            }
                            break;
                        }
                    }
                    if (dp[p][type] != null && last != null)
                    {
                        if (last.Passes >= dp[p][type].Passes)
                        {
                            dp[p][type] = null;
                        }
                    }
                    if (dp[p][type] != null)
                    {
                        last = dp[p][type];
                    }
                }
            }
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<SlalomSkiing>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 1, 5 }, 2);
                yield return CreateSingleInputSet(new[] { 15, 13, 5, 7, 4, 10, 12, 8, 2, 11, 6, 9, 3 }, 8);
            }
        }
    }
}
