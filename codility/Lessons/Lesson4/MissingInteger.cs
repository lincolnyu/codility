using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson4
{
    /// <summary>
    ///  Find the smallest missing positive integer
    /// </summary>
    /// <remarks>
    ///  Problem Link: https://app.codility.com/programmers/lessons/4-counting_elements/missing_integer/
    /// </remarks>
    class MissingInteger : ITestee
    {
        public int Solve(int[] A)
        {
            var minPos = int.MaxValue;
            var maxPos = int.MinValue;
            var posCount = 0;
            foreach (var i in A)
            {
                if (i > 0)
                {
                    if (i < minPos) minPos = i;
                    if (i > maxPos) maxPos = i;
                    posCount++;
                }
            }
            if (posCount == 0 || minPos > 1) return 1;
            const int flag = int.MinValue;
            for (var i = 0; i < A.Length; i++)
            {
                var v = A[i];
                if (v <= 0) continue;
                var index = v - 1;
                while (index < A.Length)
                {
                    var av = A[index];
                    A[index] = flag;
                    if (av > 0) index = av - 1;
                    else break;
                }
            }
            for (var i = 0; i < A.Length; i++)
            {
                var v = A[i];
                if (v != flag) return i + 1;
            }
            return A.Length + 1;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseTester
        {
            private MissingInteger _mi = new MissingInteger();

            public IEnumerable<string> TestAndShowResults()
            {
                return TestAndShowResults(_mi);
            }

            private TestSet CreateSet(int[] input, int expected)
            {
                return new TestSet
                {
                    Input = new object[] { input },
                    ExpectedOutput = expected
                };
            }

            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSet(new int[] { 1, 3, 6, 4, 1, 2}, 5);
                yield return CreateSet(new int[] { 1, 2, 3 }, 4);
                yield return CreateSet(new int[] { -1, -3 }, 1);

                yield return CreateSet(new int[] { -1, -2, 0, 2, 4}, 1);
                yield return CreateSet(new int[] { 2,1,4,5,3 }, 6);
            }
        }
    }
}
