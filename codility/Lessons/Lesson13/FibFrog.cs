using codility.TestFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace codility.Lessons.Lesson13
{
    class FibFrog : ITestee
    {
        IEnumerable<int> GenFibs(int max)
            => GenFibs().TakeWhile(x => x <= max);

        IEnumerable<int> GenFibs()
        {
            var a = 1;
            var b = 2;
            yield return a;
            yield return b;
            for (; ; )
            {
                var c = a + b;
                yield return c;
                a = b;
                b = c;
            }
        }

        int FindFib(int n, int[] fibs)
            => FindFib(n, fibs, 0, fibs.Length);

        int FindFib(int n, int[] fibs, int start, int length)
        {
            if (length == 1) return start;
            var midd = length / 2;
            var mid = start + midd;
            // [start, mid), [mid, start+length)
            if (fibs[mid] <= n) return FindFib(n, fibs, mid, length - midd);
            else return FindFib(n, fibs, start, midd);
        }

        private int[] GenFibTable(int[] fibs, int N)
        {
            var res = new int[N+1];
            for (var i = 1; i < fibs.Length; i++)
            {
                var lastFib = fibs[i - 1];
                var fib = fibs[i];
                for (var j = lastFib; j < fib; j++)
                {
                    res[j] = i-1;
                }
            }
            for (var j = fibs[fibs.Length-1]; j <= N; j++)
            {
                res[j] = fibs.Length - 1;
            }
            return res;
        }

        public int Solve(int[] A) => Solve3(A);
        
        public int Solve3(int[] A)
        {
            var n = A.Length;
            var dp = new int[n];
            var fibs = GenFibs(n + 1).ToArray();
            int r = -1;
            for (var i = A.Length-1; i >= -1; i--)
            {
                var a = i < 0 || A[i] == 1;
                if (a)
                {
                    var min = int.MaxValue;
                    foreach (var fib in fibs)
                    {
                        if (i + fib > A.Length) break;
                        if (i + fib == A.Length)
                        {
                            min = 0;
                            break;
                        }
                        else if (A[i + fib] == 1)
                        {
                            var d = dp[i + fib];
                            if (d > 0 && d < min) min = d;
                        }
                    }
                    r = min != int.MaxValue ? min + 1 : -1;
                    if (i < 0) break;
                    dp[i] = r;
                }
            }
            return r;
        }

        public int Solve2(int[] A)
        {
            var n = A.Length;
            var fibs = GenFibs(n + 1).ToArray();
            var fibTable = GenFibTable(fibs, n+1);
            var dp = new int[n];
            return SolveRange2(fibs, fibTable, A, dp, -1, int.MaxValue);
        }

        int SolveRange2(int[] fibs, int[] fibTable, int[] A, int[] dp, int start, int quit)
        {
            var min = quit - 1;
            if (min < 1) return -1;
            var i = fibTable[A.Length - start];
            var fib = fibs[i];
            var newStart = start + fib;
            if (newStart == A.Length) return 1;
            for (; i >= 0; i--)
            {
                fib = fibs[i];
                newStart = start + fib;
                if (A[newStart] == 1)
                {
                    var steps = dp[newStart];
                    if (steps == 0)
                    {
                        steps = SolveRange2(fibs, fibTable, A, dp, newStart, min);
                        dp[newStart] = steps;
                    }
                    if (steps > 0 && steps < min)
                    {
                        min = steps;
                        if (min == 1) break;
                    }
                }
            }
            return min + 1 < quit ? min + 1 : -1;
        }

        public int Solve1(int[] A)
        {
            var n = A.Length;
            var fibs = GenFibs(n + 1).ToArray();
            return SolveRange1(fibs, A, -1, A.Length, int.MaxValue);
        }

        int SolveRange1(int[] fibs, int[] A, int start, int end, int quit)
        {
            var min = quit - 1;
            if (min < 1) return -1;
            var i = FindFib(end - start, fibs);
            var fib = fibs[i];
            var newStart = start + fib;
            if (newStart == end) return 1;
            for (; i >= 0; i--)
            {
                fib = fibs[i];
                newStart = start + fib;
                if (A[newStart] == 1)
                {
                    var steps = SolveRange1(fibs, A, newStart, end, min);
                    if (steps > 0 && steps < min)
                    {
                        min = steps;
                        if (min == 1) break;
                    }
                }
            }
            return min + 1 < quit ? min + 1 : -1;
        }
        
        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<FibFrog>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0 }, 3);
                var lvs = new[] { 55, 34, 89, 144, 145, 148 /*182*/};
                {
                    var i = new int[181];
                    foreach (var lf in lvs) i[lf-1] = 1;
                    yield return CreateSingleInputSet(i, 4);
                }

                var ff = new FibFrog();
                {
                    const int count = 100;
                    const int maxLen = 1000;
                    const int dyn = 20;
                    var seed = 123;
                    var rand = new Random(seed);
                    for (var c = 0; c < count;)
                    {
                        var len = rand.Next(1, maxLen);
                        var a = new int[len];
                        var p = -1;
                        for (; ; )
                        {
                            var leap = rand.Next(1, dyn);
                            p += leap;
                            if (p >= a.Length)
                            {
                                break;
                            }
                            a[p] = 1;
                        }
                        var expected = ff.Solve1(a);
                        if (expected != -1)
                        {
                            yield return CreateSingleInputSet(a, expected);
                            c++;
                        }
                    }
                }

                {
                    // 1 2 3 5 8 13 ...
                    //     4 6 9
                    var r = ff.GenFibs(40).ToArray();
                    var n = r[r.Length - 1];
                    var a = new int[n];
                    for (var i = 2; i < r.Length-1; i++)
                    {
                        a[r[i]-1] = 1;
                    }
                    var expected = ff.Solve1(a);
                    yield return CreateSingleInputSet(a, expected);
                }

                {
                    var leaves = new[] { 2, 91, 146, 148 };
                    var a = CreateTest(leaves);
                    var expected = ff.Solve1(a);
                    yield return CreateSingleInputSet(a, expected);
                }

                {
                    const int len = 100000;
                    var a = new int[len];
                    for (var i = 0; i < len; i++) a[i] = i % 3 == 0 ? 1 : 0;
                    yield return CreateSingleInputSet(a, 8);
                }
            }

            int[] CreateTest(int[] leaps)
            {
                var last = leaps[leaps.Length - 1];
                var n = last - 1;
                var a = new int[n];
                for (var i = 0; i < leaps.Length-1; i++)
                {
                    a[leaps[i]-1] = 1;
                }
                return a;
            }
        }
    }
}
