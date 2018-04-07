using codility.TestFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace codility.Lessons.Lesson7
{
    class StoneWall : ITestee
    {
        int Solve(int[] H)
        {
            var stack = new Stack<int>();
            var total = 0;
            foreach (var h in H)
            {
                var found = false;
                for (; stack.Count > 0; )
                {
                    var t = stack.Peek();
                    if (t > h)
                    {
                        stack.Pop();
                    }
                    else
                    {
                        found = t == h;
                        break;
                    }
                }
                if (!found)
                {
                    total++;
                    stack.Push(h);
                }
            }
            return total;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<StoneWall>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 8, 8, 5, 7, 9, 8, 7, 4, 8 }, 7);
            }
        }
    }
}
