using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson92
{
    class ArrayRecovery : BaseTestee
    {
        public int solution(int[] B, int M)
        {
            throw new NotImplementedException();
        }

        public class Tester : BaseSelfTester<ArrayRecovery>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateInputSet(3, new[] { 0, 2, 2 }, 4);
                yield return CreateInputSet(4, new[] { 0, 3, 5, 6 }, 10);
                yield return CreateInputSet(49965, new[] { 0, 0 }, 100000);
            }
        }
    }
}
