using codility.Helpers;
using codility.TestFramework;
using System.Collections.Generic;

namespace codility.Lessons.Lesson4
{
    class MaxCounters : ITestee
    {
        public int[] Solve(int N, int[] A)
        {
            var result = new int[N];
            var max = 0;
            var lastMax = 0;
            foreach (var x in A)
            {
                if (x <= N)
                {
                    var i = x - 1;
                    if (result[i] < lastMax)
                    {
                        result[i] = lastMax;
                    }
                    var v = ++result[x - 1];
                    if (v > max) max = v;
                }
                else
                {
                    lastMax = max;
                }
            }
            for (var j = 0; j < N; j++)
            {
                if (result[j] < lastMax)
                {
                    result[j] = lastMax;
                }
            }
            return result;
        }

        public object Run(params object[] args)
            => Solve((int)args[0], (int[])args[1]);

        public class Tester : BaseSelfTester<MaxCounters>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(5, new[] { 3, 4, 4, 6, 1, 4, 4 }, new[] { 3, 2, 2, 4, 2 });
            }

            public override bool ResultsEqual(object a, object b)
                   => ResultsHelper.ResultsEqual((int[])a, (int[])b);

            public override string ResultToString(object r)
                => ResultsHelper.ResultToString((int[])r);
        }
    }
}
