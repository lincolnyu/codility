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

        public int Solve(int[] A)
        {
            var n = A.Length;
            var fibs = GenFibs(n + 1).ToArray();
            return SolveSteps(fibs, A, -1);
        }

        private int SolveSteps(int[] fibs, int[] A, int start)
        {
            var i = FindFib(A.Length - start, fibs);
            for (; i >= 0; i--)
            {
                var fib = fibs[i];
                var newStart = start + fib;
                if (newStart == A.Length) return 1;
                if (A[newStart] == 1)
                {
                    var steps = SolveSteps(fibs, A, newStart);
                    if (steps > 0) return steps + 1;
                }
            }
            return -1;
        }


        public int Solve2(int[] A)
        {
            var n = A.Length;
            var fibs = GenFibs(n + 1).ToArray();
            var stack = new Stack<int>();
            var dist = n + 1;

            var c = 0;
            var p = -1;
            var i = FindFib(dist, fibs);
            while (true)
            {
                for (; i >= 0; i--)
                {
                    var fib = fibs[i];
                    if (p + fib == A.Length) return c + 1;
                    if (A[p + fib] == 1)
                    {
                        stack.Push(i);
                        c++;
                        p += fib;
                        dist -= fib;
                        if (dist == 0) return c;
                        break;
                    }
                }
                if (i < 0)
                {
                    if (stack.Count == 0) return -1;
                    i = stack.Pop();
                    var f = fibs[i];
                    p -= f;
                    dist += f;
                    i--;
                    c--;
                }
                else
                {
                    i = FindFib(dist, fibs);
                }
            }
        }

        public int Solve3(int[] A)
        {
            var n = A.Length;
            if (n == 0) return 1;
            var fibs = GenFibs(n + 1).ToArray();
            return GetJumps3(fibs, A, -1, A.Length).Item1;
        }
        public Tuple<int, bool> GetJumps3(int[] fibs, int[] A, int start, int end)
        {
            var n = end - start;
            var min = int.MaxValue;
            int i;
            var ini = FindFib(n, fibs);
            var chain = false;
            for (i = ini; i >= 0; i--)
            {
                var fib = fibs[i];
                if (start + fib == end)
                {
                    return new Tuple<int, bool>(1, true);
                }
                if (A[start + fib] == 1)
                {
                    var t = GetJumps3(fibs, A, start + fib, end);
                    var j = t.Item1;
                    if (j > 0 && j < min) min = j;
                    if (t.Item2 || j == 1)
                    {
                        chain = t.Item2;
                        break;
                    }
                }
            }
            if (min == int.MaxValue) return new Tuple<int, bool>(-1, false);
            return new Tuple<int,bool>(min + 1, ini==i && chain);
        }

        public void TestFindFib()
        {
            var fibs = GenFibs(56).ToArray();
            var i = FindFib(56, fibs);
            Console.WriteLine($"{fibs[i]}");
        }

        public int Solve1(int[] A)
        {
            var n = A.Length;
            var steps = Steps(A).ToArray();
            var fibs = MarkFibs(n+1);
            return GetJumps(steps, fibs, 0, steps.Length-1);
        }

        int GetJumps(int[] steps, bool[] fibs, int start, int end)
        {
            var d = steps[end] - steps[start];
            if (fibs[d]) return 1;
            var min = int.MaxValue;
            for (var split = start+1; split < end; split++)
            {
                var s = steps[end] - steps[split];
                if (!fibs[s]) continue;
                var j = GetJumps(steps, fibs, start, split);
                if (j == -1) continue;
                if (j < min)
                {
                    min = j;
                    if (j == 1) break;
                }
                //if (split == start + 1) break;
            }
            return min == int.MaxValue ? -1 : min + 1;
        }

        IEnumerable<int> Steps(int[] A)
        {
            yield return 0;
            for (var i = 0; i < A.Length; i++)
            {
                if (A[i] != 0) yield return i + 1;
            }
            yield return A.Length + 1;
        }

        bool[] MarkFibs(int max)
        {
            var res = new bool[max + 1];
            var fibs = GenFibs(max);
            foreach (var fib in fibs)
            {
                res[fib] = true;
            }
            return res;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<FibFrog>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0 }, 3);
                yield return CreateSingleInputSet(new int[] { }, 1);
            }
        }
    }
}
