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
        class Property : IComparable<Property>
        {
            public int XPos;
            public int YPos;

            public bool Done;

            public int?[] Candidates = new int?[3];
            public int[] Max = new int[3];

            public int? One
            {
                get => Candidates[0]; 
                set => Candidates[0] = value; 
            }
            public int? Two
            {
                get => Candidates[1];
                set => Candidates[1] = value;
            }
            public int? Three
            {
                get => Candidates[2];
                set => Candidates[2] = value;
            }

            public int MaxOneY
            {
                get => Max[0];
                set => Max[0] = value;
            }
            public int MaxTwoY
            {
                get => Max[1];
                set => Max[1] = value;
            }
            public int MaxThreeY
            {
                get => Max[2];
                set => Max[2] = value;
            }

            public int CompareTo(Property other)
                => XPos.CompareTo(other.XPos);
        }

        public int solution(int[] A)
        {

            var N = A.Length;
            var props = A.Select(x => new Property { XPos = x }).ToArray();
            for (var i = 0; i <N; i++)
            {
                props[i].YPos = i;
            }
            Array.Sort(props);

            var aindex = new int[N];
            for (var i = 0; i < N; i++)
            {
                aindex[props[i].YPos] = i;
            }

            Func<int, Property> getprop = y => props[aindex[y]];
            Func<Property, int, int> getmax = (prop, comp) =>
                getprop(prop.Max[comp]).Candidates[comp].Value;
            Func<int, int, int> dist = (ya, yb) =>
            {
                var ia = aindex[ya];
                var ib = aindex[yb];
                var total = 0;
                for (var i = ia + 1; i < ib; i++)
                {
                    if (!props[i].Done)
                    {
                        total++;
                    }
                }
                return total;
            };

            var first = getprop(0);
            first.One = first.Two = first.Three = 1;
            first.MaxOneY = first.MaxTwoY = first.MaxThreeY = 0;

            for (var i = 1; i < N; i++)
            {
                var a = A[i];
                var target = new Property { XPos = a };
                var index = aindex[i];
                var prop = props[index];

                var lastprop = getprop(i - 1);
                var maxone = getmax(lastprop, 0);

                if (a > A[lastprop.MaxOneY])
                {
                    prop.One = maxone + 1;
                    prop.MaxOneY = i;
                }
                else
                {
                    var maxonex = A[lastprop.MaxOneY];
                    // uncomputed items between a and maxonex
                    var d = dist(i, lastprop.MaxOneY);
                    var max = 0;
                    for (var j = i - 1; j + 1 > max; j--)
                    {
                        var p = getprop(j);
                        if (a > p.XPos && p.One.HasValue && p.One + 1 + d > maxone
                            && p.One + 1 > max)
                        {
                            max = p.One.Value + 1;
                        }
                    }
                    if (max > 0)
                    {
                        prop.One = max;
                        if (max > maxonex)
                        {
                            prop.MaxOneY = i;
                        }
                        else
                        {
                            prop.MaxOneY = lastprop.MaxOneY;
                        }
                    }
                }

                var maxtwo = getmax(lastprop, 1);
                if (a < A[lastprop.MaxTwoY])
                {
                    prop.Two = maxtwo + 1;
                    prop.MaxTwoY = i;
                }
                else
                {
                    // TODO ...
                }

                var maxthree = getmax(lastprop, 2);
                if (a > A[lastprop.MaxThreeY])
                {
                    prop.Three = maxthree + 1;
                    prop.MaxThreeY = i;
                }
                else
                {
                    // TODO...
                }

                prop.Done = true;
            }

            var endprop = props[aindex[N - 1]];
            return Enumerable.Range(0, 3).Select(c => getmax(endprop, c)).Max();

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
                const int size = 32768;
                var rand = new Random(123);
                var seq = rand.GenerateRandomSequence(size);
                yield return BaseTester.CreateInputSet(null, seq);
            }
        }
    }
#endif
}
