using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson12
{
    public class CommonPrimeDivisors : ITestee
    {
        int Solve(int[] A, int[] B)
        {
            var z = A.Length;
            var res = 0;
            for (var i = 0; i < z; i++)
            {
                var a = A[i];
                var b = B[i];
                if (HasCommonPrimes(a, b)) res++;
            }
            return res;
        }

        bool HasCommonPrimes(int a, int b)
        {
            if (a == b) return true;
            var c = Gcd(a, b);
            if (c == 1) return false;
            var ha = Check(a/c, c);
            if (!ha) return false;
            var hb = Check(b/c, c);
            return hb;
        }

        bool Check(int x, int c)
        {
            if (x == 1) return true;
            var d = Gcd(x, c);
            if (d == 1) return false;
            return Check(x / d, c);
        }

        int Gcd(int A, int B)
        {
            if (A < B)
            {
                var t = A;
                A = B;
                B = t;
            }
            while (true)
            {
                var r = A % B;
                if (r == 0) return B;
                A = B;
                B = r;
            }
        }

        public object Run(params object[] args)
            => Solve((int[])args[0], (int[])args[1]);

        public class Tester : BaseSelfTester<CommonPrimeDivisors>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(new[] { 15, 10, 3 }, new[] { 75, 30, 5 }, 1);
                yield return Create2InputSet(new[] { 1 }, new[] { 1 }, 1);
            }
        }
    }
}
