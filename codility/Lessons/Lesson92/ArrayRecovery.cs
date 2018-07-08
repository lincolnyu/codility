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

        /// <summary>
        ///  Get the inverse of a in terms of prime modulo n using extended euclidean algo
        /// </summary>
        /// <param name="a">The number to get inverse of</param>
        /// <param name="n">The characteristic (modulo)</param>
        /// <returns>The iverse of a in the prime field with characteristic n</returns>
        /// <remarks>
        ///  https://en.wikipedia.org/wiki/Extended_Euclidean_algorithm
        /// </remarks>
        private int GetInverse(int a, int n)
        {
            var t = 0;
            var newt = 1;
            var r = n;
            var newr = a;
            while (newr != 0)
            {
                var q = r / newr;
                var oldt = t;
                t = newt;
                newt = oldt - q * newt;
                var oldr = r;
                r = newr;
                newr = oldr - q * newr;
            }
            if (t < 0) t += n;
            return t;
        }

        private int Combination(int n, int m, int pm)
        {
            var num = 1UL;
            var denom = 1UL;
            var len = Math.Min(n - m, m);

            for (var i = 0; i < len; i++)
            {
                num *= (ulong)(n - i);
                denom *= (ulong)(1 + i);
                if (num >= (ulong)pm) num %= (ulong)pm;
                if (denom >= (ulong)pm) denom %= (ulong)pm;
            }

            var inv = GetInverse((int)denom, pm);
            return (int)((num * (ulong)inv) % (ulong)pm);
        }

        private void MulNonincreasingCombinations(ref int total, int n, int m, int primeMod)
        {
            // n balls to be put in m boxes
            //(n+m-1)!/n!*(m-1)! ways?  See stars (n) and bars (m-1) method
            // throw new NotImplementedException();
            var c = Combination((n + m - 1), n, primeMod);
            ulong tmp = (ulong)total * (ulong)c;
            tmp %= (ulong)primeMod;
            total = (int)tmp;
        }

        public int solution(int[] B, int M)
        {
            // B[n] = 0      means A[n] is no more than prev minimal
            // B[n+1] > B[n] means A[n] = B[n+1]
            //               and A[n+1] > A[n]
            // B[n+1] = B[n] means A[n+1] <= A[n]
            // B[n+1] < B[n] Then B[n+1] must be one stair that appeared before
            //               up to the recent 0 or 0 (new minimal). 
            //               Suppose it's x suppose the one just one step
            //               above B[k] is y, then A[n+1] > x and <= y
            //               The search table should also be updated by having y 
            //               and above removed (See line 160)
            const int modulo = 1000000007;
            var N = B.Length;
            var record = new int[N];
            var rcsize = 0;
            var eqCount = 1;
            var currMax = M; // <=
            var currBase = 0; // >
            var total = 1;
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
                    // eqCount items (currBase, currMax]
                    MulNonincreasingCombinations(ref total, eqCount, currMax - currBase, modulo);

                    if (b == 0) // new min
                    {
                        currMax = record[0];
                        currBase = 0;
                        eqCount = 1;
                        rcsize = 0; //clear record
                    }
                    else
                    {
                        var ib = BinarySearch(record, rcsize, b);
                        // update rcsize
                        var ib1 = ib + 1;
                        rcsize = ib1;
                        // A[i] > record[ib] && A[i] <= record[ib1]

                        currBase = record[ib]; // >
                        currMax = record[ib1]; // <=
                        eqCount = 1;
                    }
                }
            }

            MulNonincreasingCombinations(ref total, eqCount, currMax - currBase, modulo);

            return (total % modulo);
        }

        public class Tester : BaseSelfTester<ArrayRecovery>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateInputSet(1000000006, new[] { 0, 328193, 0, 1 }, 604469);
                yield return CreateInputSet(91, new[] { 0, 0 }, 999999993);
                yield return CreateInputSet(44, new[] { 0, 11, 0 }, 15);
                yield return CreateInputSet(49965, new[] { 0, 0 }, 100000);
                yield return CreateInputSet(3, new[] { 0, 2, 2 }, 4);
                yield return CreateInputSet(4, new[] { 0, 3, 5, 6 }, 10);
            }
        }
    }
}
