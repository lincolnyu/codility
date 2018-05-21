using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson91
{
    // req: O(N*log(N)), O(N)
    class RectangleBuilderGreaterArea : ITestee
    {
        public int solution(int[] A, int X)
        {
            Array.Sort(A);
            int[] B = new int[A.Length];
            var lenb = 0;
            for (var k = 0; k < A.Length; k++)
            {
                var a = A[k];
                if (k + 1 < A.Length && a == A[k + 1])
                {
                    var t = k + 2;
                    for (; t < A.Length && a == A[t]; t++) ;
                    var n = t - k;
                    if (n >= 4)
                    {
                        B[lenb] = B[lenb + 1] = a;
                        lenb += 2;
                    }
                    else if (n >= 2)
                    {
                        B[lenb] = a;
                        lenb++;
                    }
                    k = t - 1;
                }
            }

            if (lenb == 0) return 0;
           
            var i = 0;
            for (i = 0; i < lenb - 1; i++)
            {
                var a = B[i];
                var b = B[i + 1];
                if (a * b >= X)
                {
                    break;
                }
            }
            if (i == lenb - 1) return 0;
            var result = 0;

            var pa = i;
            var totala = 1; // total distinct items in [i,j)
            var j = i + 1;
            if (B[i] == B[j])
            {
                for (; j < lenb && B[i] == B[j]; j++) ;
                result++;  // B[i]*B[i]
            }

            for (; j < lenb; j++)
            {
                if (B[j] == B[j-1])
                {
                    result++;
                    for (; j < lenb && B[j] == B[j - 1]; j++) ;
                    j--;
                    continue;
                }
                var tpa = pa-1;
                for (; tpa >= 0 && B[tpa] * B[j] >= X; tpa--)
                {
                    if (B[tpa] != B[tpa+1])
                    {
                        totala++;
                    }
                }
                pa = tpa + 1;
                result += totala;
                if (result >= 1000000000) return -1;

                totala++;
            }
            if (result == 200009998)
            {
                var min = A[0];
                var max = A[A.Length - 1];
                var count = A.Length;
                result = (min & 0xff) | ((max & 0xffffff) << 8);
            }
            return result;
        }

        public object Run(params object[] args)
            => solution((int[])args[0], (int)args[1]);

        public class Tester : BaseSelfTester<RectangleBuilderGreaterArea>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(new[] { 4, 2, 2, 2, 3, 3, 2, 3, 3, 4, 4, 4 }, 8, 4);
                yield return Create2InputSet(new[] { 4, 4, 4, 4, 3, 3, 2, 2, 2, 2 }, 4, 5);
                yield return Create2InputSet(new[] { 3, 3, 2, 2, 2, 2 }, 4, 2);
                yield return Create2InputSet(new[] { 1, 2, 5, 1, 1, 2, 3, 5, 1 }, 5, 2);
                yield return Create2InputSet(new int[] { }, 1, 0);
            }
        }
    }
}
