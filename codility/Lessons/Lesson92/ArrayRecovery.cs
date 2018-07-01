using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson92
{
    class ArrayRecovery : BaseTestee
    {
        public int solution(int[] B, int M)
        {
            // 0 1 2 3 4
            // 0 0 a b c
            // \          minimal so far
            //   a        minimal so far must be a
            //   a b b+   if b > a, [2] = b, [3] something > b
            //   a x y    if b = a, [2] can be something x > a, 
            //               [3] <=x & > a
            //            b < a -> impossible
            //   a b c    if c > b > a, [4] something > c
            //   a x c    if c > b = a, [4] something > c
            //   a b u v  if c = b > a, [3] > b, [4] <= [3] & > c
            //   a x y z  if c = b = a, [2] some x > a
            //            [3] <= [2] & > a, [4] <= [3] & > a 
            //
            // so the nonzero section has to be non-decreasing
            // when new minimal appears, 0 is yielded
            // the new nonzero section has to be based on the 
            // last minimal and repeats the above non-decreasing
            // pattern
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
