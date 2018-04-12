using codility.TestFramework;
using System.Collections.Generic;

namespace codility.Lessons.Lesson12
{
    class ChocolatesByNumbers : ITestee
    {
        /// <summary>
        ///  N chocolates in total arranged in a circle consumed one every M chocolates
        /// </summary>
        /// <param name="N">Total number of chocolates</param>
        /// <param name="M">Step of each consumption</param>
        /// <returns>The number of chocolates eaten before hitting an empty wrap</returns>
        int Solve(int N, int M)
        {
            var gcd = N < M ? Gcd(M, N) : Gcd(N, M);
            return N / gcd;
        }

        /// <summary>
        ///  Get the common factor of A and B where A is no smaller than B
        /// </summary>
        /// <param name="A">The first number which is the larger of the two if unequal</param>
        /// <param name="B">The secnod number</param>
        /// <returns>The common factor</returns>
        int Gcd(int A, int B)
        {
            while (true)
            {
                var r = A % B;
                if (r == 0) return B;
                A = B;
                B = r;
            }
        }

        public object Run(params object[] args)
            => Solve((int) args[0], (int) args[1]);

        public class Tester : BaseSelfTester<ChocolatesByNumbers>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(10, 4, 5);
                yield return Create2InputSet(6, 10, 3);
            }
        }
    }
}
