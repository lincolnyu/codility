using System.Collections.Generic;
using System.Linq;
using codility.Helpers;
using codility.TestFramework;

namespace codility.Lessons.Lesson13
{
    class Ladder : ITestee
    {
        class IntWrapper
        {
            public int Value;
            public IntWrapper(int v) { Value = v; }
            public static IntWrapper operator+(IntWrapper a, IntWrapper b)
            {
                return new IntWrapper(a.Value + b.Value);
            }
        }

        int[] Solve(int[] A, int[] B)
        {
            int n = A.Length;
            var res = new int[n];
            var maxa = A.Max();
            var maxb = B.Max();
            var maxmod = 1 << maxb;
            var fibs = GetFibs(maxa, maxmod).ToArray();
            for (var i = 0; i < n; i++)
            {
                var a = A[i];
                var b = B[i];
                var mod = 1 << b;
                res[i] = fibs[a-1] % mod;
            }
            return res;
        }

        IEnumerable<int> GetFibs(int n, int mod)
        {
            var fibs = GenFibs();
            var i = 0;
            foreach (var fib in fibs)
            {
                fib.Value %= mod;
                yield return fib.Value;
                i++;
                if (i == n)
                {
                    break;
                }
            }
        }

        IEnumerable<IntWrapper> GenFibs()
        {
            var a = new IntWrapper(1);
            var b = new IntWrapper(2);
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
            => Solve((int[])args[0], (int[])args[1]);

        public class Tester : BaseSelfTester<Ladder>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(new[] { 4, 4, 5, 5, 1 }, new[] { 3, 2, 4, 3, 1 }, new[] { 5, 1, 8, 0, 1 });
            }

            public override bool ResultsEqual(object a, object b)
                => ResultsHelper.ResultsEqual((int[])a, (int[])b);

            public override string ResultToString(object r)
                => ResultsHelper.ResultToString((int[])r);
        }
    }
}
