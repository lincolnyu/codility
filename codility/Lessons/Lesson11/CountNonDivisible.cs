using System.Collections.Generic;
using System.Linq;
using codility.Helpers;
using codility.TestFramework;

namespace codility.Lessons.Lesson11
{
    public class CountNonDivisible : ITestee
    {
        int[] Solve(int[] A)
        {
            var max = A.Max();
            var f = new int[max + 1];
            var oneCount = 0;
            foreach (var a in A)
            {
                if (a == 1) oneCount++;
                else
                {
                    for (var j = a; j < f.Length; j += a)
                    {
                        f[j]++;
                    }
                }
            }
            var res = new int[A.Length];
            var top = A.Length - oneCount;
            for (var i = 0; i < A.Length; i++)
            {
                var a = A[i];
                res[i] = top - f[a];
            }
            return res;
        }

        int[] SolveFast(int[] A)
        {
            var max = A.Max();
            var f = new int[max + 1];
            foreach (var a in A)
            {
                for (var i = a; i < f.Length; i += a)
                {
                    f[i]++;
                }
            }
            var res = new int[A.Length];
            for (var i = 0; i < A.Length; i++)
            {
                var a = A[i];
                res[i] = A.Length - f[a];
            }
            return res;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<CountNonDivisible>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 3, 1, 2, 3, 6 }, new[] { 2, 4, 3, 2, 0 });
            }

            public override bool ResultsEqual(object a, object b)
                => ResultsHelper.ResultsEqual((int[])a, (int[])b);

            public override string ResultToString(object r)
                => ResultsHelper.ResultToString((int[])r);
        }
    }
}
