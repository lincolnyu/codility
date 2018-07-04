using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson92
{
    class ArrayRecovery : BaseTestee
    {
        public int BinarySearch(IList<int> a, int n, int v)
        {
            var begin = 0;
            var end = n - 1;
            // a[begin] <= m <= a[end]
            for (; ; )
            {
                var mi = (begin + end) / 2;
                var m = a[mi];
                if (v < m)
                {
                    end = mi;
                }
                else if (v > m)
                {
                    if (begin == mi)
                    {
                        return -end - 1;
                    }
                    begin = mi;
                }
                else
                {
                    return mi;
                }
            }
        }

        private void MulNonincreasingCombinations(ref int total, int n, int m, int modulo)
        {
            // 'count' balls to be put in 'space' boxes
            //(n+m-1)!/n!*(m-1)! ways?  See multinomial coefficent
            throw new NotImplementedException();
        }

        public int solution(int[] B, int M)
        {
            // B[n] = 0      means A[n] is no more than prev minimal
            // B[n+1] > B[n] means A[n] = B[n+1]
            //               and A[n+1] > A[n]
            // B[n+1] = B[n] means A[n+1] <= A[n]
            // B[n+1] < B[n] Then B[n+1] must be one B[x] that appeared before
            //               up to the recent 0 or 0 (new minimal). 
            //               Suppose it's B[k] and suppose the one just one step
            //               above B[k] is x, then A[n+1] > B[k] and <= x
            const int modulo = 1000000007;
            var N = B.Length;
            var record = new int[N];
            var rcsize = 0;
            var eqCount = 1;
            var currMax = M; // <=
            var currBase = 0; // >
            var total = 0;
            for (var i = 1; i < N; i++)
            {
                var b = B[i];
                if (b > B[i - 1])
                {
                    // A[i-1] = B[i] && A[i] > B[i]

                    // one item A[i-1] == B[i]
                    // *1  don't need to do anything

                    if (eqCount > 1)
                    {
                        // another eqCount-1 items >= B[i] and <= currMax
                        // and non-increasing monotonously
                        MulNonincreasingCombinations(ref total, eqCount - 1, currMax - B[i] + 1, modulo);
                    }

                    record[rcsize++] = b;

                    eqCount = 1;
                    currMax = M;
                    currBase = B[i];
                }
                else if (b == B[i - 1])
                {
                    // A[i] <= A[i-1] and A[i] > currBase
                    eqCount++;
                }
                else // b < B[i-1]
                {
                    if (b == 0) // new min
                    {
                        currMax = record[0] - 1;
                        eqCount = 1;
                        rcsize = 0; //clear record
                    }
                    else
                    {
                        var ib = BinarySearch(record, rcsize, b);
                        var ib1 = ib + 1;
                        // A[i] > record[ib] && A[i] <= record[ib1]

                        // eqCount items (currBase, currMax]
                        MulNonincreasingCombinations(ref total, eqCount, currMax - currBase, modulo);

                        currBase = record[ib]; // >
                        currMax = record[ib1]; // <=
                        eqCount = 1;
                    }
                }
            }

            MulNonincreasingCombinations(ref total, eqCount, currMax - currBase, modulo);

            return total;
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
