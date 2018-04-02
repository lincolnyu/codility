using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson5
{
    class PassingCars : ITestee
    {
        public int Solve(int[] A)
        {
            var currZeroCount = 0;
            var passingCarCount = 0;
            foreach (var a in A)
            {
                if (a == 0)
                {
                    currZeroCount++;
                }
                else
                {
                    passingCarCount += currZeroCount;
                    if (passingCarCount > 1000000000) return -1;
                }
            }
            return passingCarCount;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<PassingCars>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 0, 1, 0, 1, 1 }, 5);
            }
        }
    }
}
