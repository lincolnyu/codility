using codility.TestFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace codility.Lessons.Lesson13
{
    class FibFrog : ITestee
    {
        int Solve(int[] A)
        {
            throw new NotImplementedException();
        }

        int SolveSlow(int[] A)
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
            for (var split = end-1; split > start; split--)
            {
                var s = steps[end] - steps[split];
                if (!fibs[s]) continue;
                var j = GetJumps(steps, fibs, start, split);
                if (j == -1) continue;
                j++;
                if (j < min) min = j;
            }
            return min == int.MaxValue ? -1 : min;
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

        public object Run(params object[] args)
            => SolveSlow((int[])args[0]);

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
