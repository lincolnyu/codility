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
            {
                var c = Val.CompareTo(other.Val);
                if (c != 0) return c;
                return Index.CompareTo(other.Index);
            }
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
            throw new NotImplementedException();
        }

        public static int ref_solution(int[] A)
        {
            var total = 0;
            for (var i = 0; i < A.Length-1; i++)
            {
                for (var j = i+1; j < A.Length; j++)
                {
                    if (A[j] < A[i])
                    {
                        total++;
                    }
                }
            }
            return total;
        }

        public class Tester : BaseSelfTester<ArrayInversionCount>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                return GetDistinctNumberTestSets();
                //yield return Generate(new[] { 7, 1, 1, 2, 3, 4, 6, 5, 4, 3, 7, 8, 9 });
                //yield return Generate(new[] { -1, 6, 3, 4, 7, 4 });
                //yield return CreateInputSet(4, new[] { -1, 6, 3, 4, 7, 4 });
            }

            IEnumerable<TestSet> GetDistinctNumberTestSets()
            {
                var r = new Random(123);
                for (var t = 0; t < 10; t++)
                {
                    var max = r.Next(10000);
                    var seq = new int[max];
                    var used = new bool[max];
                    for (var i = 0; i < max; i++)
                    {
                        var n = r.Next(max - i);
                        var j = 0;
                        for (j= 0; j < max; j++)
                        {
                            if (!used[j])
                            {
                                if (n-- <= 0)
                                {
                                    used[j] = true;
                                    break;
                                }
                            }
                        }
                        seq[i] = j;
                    }
                    yield return Generate(seq);
                }
            }

            TestSet Generate(int[] A)
                => CreateInputSet(ref_solution(A), A);
        }
    }
}
