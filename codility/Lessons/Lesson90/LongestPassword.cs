using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson90
{
    class LongestPassword : ITestee
    {
        int Solve(string S)
        {
            var digitCount = 0;
            var letterCount = 0;
            var bad = false;
            var max = -1;
            for (var i = 0; i < S.Length + 1; i++)
            {
                var c = i < S.Length ? S[i] : ' ';
                if (char.IsWhiteSpace(c))
                {
                    if (!bad && letterCount % 2 == 0 && digitCount % 2 == 1)
                    {
                        var len = letterCount + digitCount;
                        if (len > max)
                        {
                            max = len;
                        }
                    }
                    bad = false;
                    letterCount = 0;
                    digitCount = 0;
                }
                else if (char.IsLetter(c) && !bad)
                {
                    letterCount++;
                }
                else if (char.IsDigit(c) && !bad)
                {
                    digitCount++;
                }
                else
                {
                    bad = true;
                }
            }
            return max;
        }

        public object Run(params object[] args)
            => Solve((string)args[0]);

        public class Tester : BaseSelfTester<LongestPassword>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet("test 5 a0A pass007 ?xy1", 7);
            }
        }
    }
}
