using System;
using System.Linq;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson17
{
    class MinAbsSum : ITestee
    {
        int Solve(int[] A) => Solve1(A);

        int GetGcf(int a, int b)
        {
            for (; ; )
            {
                var r = a % b;
                if (r == 0) return b;
                a = b;
                b = r;
            }
        }

        void GetATraits(int[] A, out int aamax, out int aasum, out int[] aaacc, 
            out int gcf)
        {
            aamax = 0;
            aasum = 0;
            aaacc = new int[A.Length];
            var i = A.Length - 1;
            for (; i >= 0; i--)
            {
                var a = A[i];
                var aa = Math.Abs(a);
                if (aa > aamax) aamax = aa;
                aasum += aa;
                if (i > 0)
                {
                    aaacc[i - 1] = aa + aaacc[i];
                }
            }
            gcf = 0;
            for (i = 0; i < A.Length && A[i] == 0; i++) ;
            if (i < A.Length)
            {
                gcf = Math.Abs(A[i]);
                for (i++; i < A.Length; i++)
                {
                    if (A[i] != 0)
                    {
                        gcf = GetGcf(Math.Abs(A[i]), gcf);
                    }
                }
            }
        }

        int Solve1(int[] A)
        {
            if (A.Length == 0) return 0;
            GetATraits(A, out int aamax, out int aasum, out int[] aaacc, out int gcf);
            if (gcf == 0) return 0;
            var upperlimit =  Math.Min(aamax * aamax, aasum);
            var possibleBuffers = new bool[2][]
            {
                new bool[upperlimit+1],
                new bool[upperlimit+1]
            };
            var pointer = 0;
            possibleBuffers[1 - pointer][0] = true;
            for (var k = 0; k < A.Length; k++)
            {
                var a = A[k];
                var aa = Math.Abs(a);
                var prevset = possibleBuffers[1 - pointer];
                var currset = possibleBuffers[pointer];
                var ilimit = Math.Min(prevset.Length, aaacc[k] + 101 + aa);
                for (var i = 0; i < ilimit; i += gcf)
                {
                    if (prevset[i])
                    {
                        var p = Math.Abs(i + aa);
                        var q = Math.Abs(i - aa);
                        if (q <= upperlimit)
                        {
                            currset[q] = true;
                            if (p <= upperlimit)
                            {
                                currset[p] = true;
                            }
                        }
                        prevset[i] = false;
                    }
                }
                pointer = 1 - pointer;
            }
            for (var i = 0; i < possibleBuffers[1 - pointer].Length; i++)
            {
                if (possibleBuffers[1 - pointer][i]) return i;
            }
            return -1;
        }

        int Solve2(int[] A)
        {
            if (A.Length == 0) return 0;
            var possibleBuffers = new HashSet<int>[2]
            {
                new HashSet<int>(),
                new HashSet<int>()
            };
            var amax = A.Max(x=>Math.Abs(x));
            var upperlimit = amax*amax;
            var pointer = 0;
            possibleBuffers[1 - pointer].Add(0);
            foreach (var a in A)
            {
                var aa = Math.Abs(a);
                var prevset = possibleBuffers[1 - pointer];
                var currset = possibleBuffers[pointer];
                foreach (var x in prevset)
                {
                    var p = Math.Abs(x + aa);
                    var q = Math.Abs(x - aa);
                    if (q <= upperlimit)
                    {
                        currset.Add(q);
                        if (p <= upperlimit)
                        {
                            currset.Add(p);
                        }
                    }
                }
                prevset.Clear();
                pointer = 1 - pointer;
            }
            return possibleBuffers[1 - pointer].Min();
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<MinAbsSum>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 100, 0, 0, 0, 0, 0 }, 100);
                yield return CreateSingleInputSet(new int[] { }, 0);
                yield return CreateSingleInputSet(new[] { 0, 100, 0, 0, 0, 0 }, 100);
                yield return CreateSingleInputSet(new[] { 91, 92, 93, 94, 95, 96, 97 }, 82);
                yield return CreateSingleInputSet(new[] { 91, 92, 92, 92, 97, 97, 97 }, 76);
                yield return CreateSingleInputSet(new[] { 91, 92, 92, 92, 97, 97, 97 }, 76);
                yield return CreateSingleInputSet(new[] { 1, 5, 2, -2 }, 0);
                {
                    var l = new int [201];
                    l[0] = 100;
                    for (var i = 1; i < 101; i++) l[i] = 99;
                    for (var i = 101; i < 201; i++) l[i] = 100;
                    yield return CreateSingleInputSet(l, 0);
                }
                {
                    const int count = 10000;
                    var l = new int[count + 1];
                    l[0] = 100;
                    for (var c = 0; c < count/200; c++)
                    {
                        for (var i = 1; i < 101; i++) l[c * 200 + i] = 99;
                        for (var i = 101; i < 201; i++) l[c * 200 + i] = 100;
                    }
                    yield return CreateSingleInputSet(l, 0);
                }
            }
        }
    }
}
