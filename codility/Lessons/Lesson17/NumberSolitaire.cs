using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson17
{
    class NumberSolitaire : ITestee
    {
        int Solve(int[] A)
        {
            var n = A.Length;
            var dp = new LinkedList<int>();
            dp.AddLast(A[0]);
            var maxi = 0;
            var max = A[0];
            for (var k = 1; k < n; k++)
            {
                var newv = A[k] + max;
                dp.AddLast(newv);

                var newIsMax = newv > max;
                if (dp.Count > 6)
                {
                    dp.RemoveFirst();
                    if (!newIsMax)
                    {
                        if (maxi == 0)
                        {
                            max = int.MinValue;
                            var i = 0;
                            foreach (var v in dp)
                            {
                                if (v > max)
                                {
                                    max = v;
                                    maxi = i;
                                }
                                i++;
                            }
                        }
                        else
                        {
                            maxi--;
                        }
                    }
                }      
                if (newIsMax)
                {
                    maxi = dp.Count - 1;
                    max = newv;
                }
            }
            return dp.Last.Value;
        }

        int SolveSlightlySlow(int[] A)
        {
            var n = A.Length;
            var dp = new int[n]; // dp[i] max can be achieved for length i
            dp[0] = A[0];
            for (var k = 1; k < n; k++)
            {
                var max = int.MinValue;
                for (var j = 1; j <= 6 && k-j>=0; j++)
                {
                    if (dp[k - j] > max)
                    {
                        max = dp[k - j];
                    }
                }
                dp[k] = A[k] + max;
            }
            return dp[n - 1];
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<NumberSolitaire>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 1, -2, 0, 9, -1, -2 }, 8);
            }
        }
    }
}
