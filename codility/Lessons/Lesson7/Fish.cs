using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson7
{
    public class Fish : ITestee
    {
        int Solve(int[] A, int[] B)
        {
            var dsstack = new Stack<int>();
            var n = A.Length;
            var usalive = 0;
            for (var i = 0; i < n; i++)
            {
                var a = A[i];
                var b = B[i];
                if (b == 1)
                {
                    dsstack.Push(a);
                }
                else
                {
                    var eaten = false;
                    for (; dsstack.Count >0 ; )
                    {
                        var p = dsstack.Peek();
                        if (p > a)
                        {
                            eaten = true;
                            break;
                        }
                        else
                        {
                            dsstack.Pop();
                        }
                    }
                    if (!eaten) usalive++;
                }
            }
            return usalive + dsstack.Count;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0], (int[])args[1]);

        public class Tester : BaseSelfTester<Fish>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create2InputSet(new[] { 4, 3, 2, 1, 5 }, new[] { 0, 1, 0, 0, 0 }, 2);
            }
        }
    }
}
