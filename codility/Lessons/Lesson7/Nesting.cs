using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson7
{
    class Nesting : ITestee
    {
        int Solve(string S)
        {
            var opencount = 0;
            foreach (var c in S)
            {
                if (c == '(')
                {
                    opencount++;
                }
                else
                {
                    if (opencount == 0) return 0;
                    opencount--;
                }
            }
            return opencount == 0? 1 : 0;
        }

        public object Run(params object[] args)
            => Solve((string)args[0]);

        public class Tester : BaseSelfTester<Nesting>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet("(()(())())", 1);
                yield return CreateSingleInputSet("())", 0);
            }
        }
    }
}
