using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson15
{
    class AbsDistinct : ITestee
    {
        int Solve(int[] A)
        {
            // Can be optimized with binary search but meh
            var ileft = -1;
            for (; ileft < A.Length - 1; ileft++)
            {
                if ((ileft < 0 || A[ileft] < 0) && A[ileft + 1] >= 0) break;
            }
            var iright = ileft + 1;
            for (; iright < A.Length; iright++)
            {
                if (A[iright] > 0) break;
            }

            // ileft -- last negative
            // iright -- first positive
            var count = iright - ileft > 1 ? 1 : 0; // has zero
            for (; iright < A.Length && ileft >= 0; )
            {
                var left = A[ileft];
                var right = A[iright];
                var sum = left + right;
                if (sum > 0) // abs(left) < abs(right)
                {
                    do { ileft--; } while (ileft >= 0 && A[ileft] == left);
                }
                else if (sum < 0) // abs(left) > abs(right)
                {
                    do { iright++; } while (iright < A.Length && A[iright] == right);
                }
                else
                {
                    // get out of here
                    for (var aright = A[iright]; iright < A.Length && A[iright] == aright; iright++) ;
                    for (var aleft = A[ileft]; ileft >= 0 && A[ileft] == aleft; ileft--) ;
                }
                count++;
            }

            var last = 0;
            for (; iright < A.Length; iright++ )
            {
                var a = A[iright];
                if (a != last)
                {
                    count++;
                    last = a;
                }
            }
            for (; ileft >= 0; ileft--)
            {
                var a = A[ileft];
                if (a != last)
                {
                    count++;
                    last = a;
                }
            }

            return count;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<AbsDistinct>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                var rand = new Random(123);
                for (var c = 0; c < 100; c++)
                {
                    var r = GetRandomArray(rand, 10000, int.MinValue, int.MaxValue);
                    var e = RefSolve(r);
                    yield return CreateSingleInputSet(r, e);
                }
                yield return CreateSingleInputSet(new[] { 0 }, 1);
                yield return CreateSingleInputSet(new[] { 1 }, 1);
                yield return CreateSingleInputSet(new[] { 0,1 }, 2);
                yield return CreateSingleInputSet(new[] { -1, 0 }, 2);
                yield return CreateSingleInputSet(new[] { -2147483648, 0 }, 2);
                yield return CreateSingleInputSet(new[] { -4, -3, -1, 0, 1, 1, 2, 2, 5 }, 6);
                yield return CreateSingleInputSet(new[] { -2, -2, -1, 1, 2 }, 2);
                yield return CreateSingleInputSet(new[] { 0, 0, 0 }, 1);
                yield return CreateSingleInputSet(new[] { -1 }, 1);
                yield return CreateSingleInputSet(new[] { -5, -3, -1, 0, 3, 6 }, 5);
            }

            private int[] GetRandomArray(Random rand, int n, int min = int.MinValue, int max = int.MaxValue)
            {
                var g = GenerateRandom(rand, min, max);
                var res = new int[n];
                var i = 0;
                foreach (var v in g)
                {
                    res[i++] = v;
                    if (i == n) break;
                }
                Array.Sort(res);
                for (i = 1; i < n; i++)
                {
                    if (res[i-1] > res[i])
                    {
                        throw new Exception("wrong sort");
                    }
                }
                return res;
            }

            private int RefSolve(int[] l)
            {
                var h = new HashSet<int>();
                var c = 0;
                var minint = false;
                foreach (var v in l)
                {
                    if (v == int.MinValue)
                    {
                        if (!minint)
                        {
                            c++;
                            minint = true;
                        }
                    }
                    else
                    {
                        var a = Math.Abs(v);
                        if (!h.Contains(a))
                        {
                            c++;
                            h.Add(a);
                        }
                    }
                }
                return c;
            }

            private IEnumerable<int> GenerateRandom(Random rand, int min = int.MinValue, int max = int.MaxValue)
            {
                for (; ;)
                {
                    yield return rand.Next(min, max);
                }
            }
        }
    }
}
