using System;
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
            throw new NotImplementedException();
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseTester
        {
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
            }
        }
    }
}
