using codility.TestFramework;
using System.Collections.Generic;

namespace codility.Lessons.Lesson7
{
    class Brackets : ITestee
    {
        int Solve(string S)
        {
            var stack = new Stack<char>();
            bool match(char open, char close)
            {
                if (close == ')') return open == '(';
                if (close == ']') return open == '[';
                if (close == '}') return open == '{';
                return false;
            }
            foreach (var c in S)
            {
                switch (c)
                {
                    case '(':
                    case '[':
                    case '{':
                        stack.Push(c);
                        break;
                    default: // assuming closing ones
                        if (stack.Count == 0) return 0;
                        var pc = stack.Pop();
                        if (!match(pc, c)) return 0;
                        break;
                }
            }
            return stack.Count == 0? 1 : 0;
        }

        public object Run(params object[] args)
            => Solve((string)args[0]);

        public class Tester : BaseSelfTester<Brackets>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet("{[()()]}", 1);
                yield return CreateSingleInputSet("([)()]", 0);
            }
        }
    }
}
