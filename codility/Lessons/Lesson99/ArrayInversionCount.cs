using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson99
{
    class ArrayInversionCount : BaseTestee
    {
        class Pod : IComparable<Pod>
        {
            public int Val;
            public int Index;

            public int CompareTo(Pod other)
                => Val.CompareTo(other.Val);
        }

        public int solution(int[] A)
        {
            int n = A.Length;
            var pods = new Pod[n];
            for (var i = 0; i < n; i++)
            {
                pods[i] = new Pod { Val = A[i], Index = i };
            }
            Array.Sort(pods);
            var ranks = new int[n];
            for (var i = 0; i < n; i++)
            {
                if (i > 0 && pods[i].Val == pods[i-1].Val)
                {
                    ranks[pods[i].Index] = ranks[pods[i - 1].Index];
                }
                else
                {
                    ranks[pods[i].Index] = i;
                }
            }
            throw new NotImplementedException();
        }

        public class Tester : BaseSelfTester<ArrayInversionCount>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateInputSet(4, new[] { -1, 6, 3, 4, 7, 4 });
            }
        }
    }
}
