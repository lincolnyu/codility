using System;
using System.Collections.Generic;
using codility.Lessons.Lesson14.Helper;
using codility.TestFramework;

namespace codility.Lessons.Lesson14
{
    class MinMaxDivision : ITestee
    {
        public int Solve(int K, int M, int[] A)
        {
            var (upper, max) = Init(A, K);
            var aims = BSHelper.Generate(max, upper-1);
            int lastAim = upper;
            foreach (var aim in aims)
            {
                if (TryOne(A, aim.Index)<=K)
                {
                    lastAim = aim.Index;
                    aim.Dir = -1;
                }
                else
                {
                    aim.Dir = 1;
                }
            }
            return lastAim;
        }

        (int,int) Init(int[] A, int K)
        {
            var maxA = 0;
            var maxSum = 0;
            var meanBlkSz = (A.Length + K - 1) / K;
            for (var i = 0; i < A.Length; i += meanBlkSz)
            {
                var sum = 0;
                for (var j = i; j < Math.Min(i + meanBlkSz, A.Length); j++)
                {
                    var a = A[j];
                    sum += a;
                    if (a > maxA) maxA = a;
                }
                if (sum > maxSum) maxSum = sum;
            }
            return (maxSum, maxA);
        }

        int TryOne (int[]A, int aim)
        {
            var blocksum = 0;
            var blockcount = 0;
            for (var i = 0; i < A.Length; i++)
            {
                var a = A[i];
                if (blocksum + a <= aim)
                {
                    blocksum += a;
                }
                else
                {
                    blockcount++;
                    blocksum = a;
                }
            }
            if (blocksum > 0)
            {
                blockcount++;
            }
            return blockcount;
        }

        public object Run(params object[] args)
            => Solve((int)args[0], (int)args[1], (int[])args[2]);

        public class Tester : BaseSelfTester<MinMaxDivision>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create3InputSet(3, 5, new[] { 5, 3 }, 5);
                yield return Create3InputSet(3, 5, new[] { 2, 1, 5, 1, 2, 2, 2 }, 6);
            }

            private IEnumerable<int> GetZeroOnes(Random rand, int n, double thr = 0.1)
            {
                for (var i = 0; i < n; i++)
                {
                    var r = rand.NextDouble();
                    if (r > thr)
                    {
                        yield return 1;
                    }
                    else
                    {
                        yield return 0;
                    }
                }
            }
        }
    }
}
