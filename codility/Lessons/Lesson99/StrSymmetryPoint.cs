using codility.TestFramework;
using System.Collections.Generic;

namespace codility.Lessons.Lesson99
{
    public class StrSymmetryPoint : BaseTestee
    {
        public int solution(string S)
        {
            if (S.Length % 2 == 0) return -1;
            if (S.Length == 1) return 0;
            var m = S.Length / 2;
            for (var i = 0; i < m; i++)
            {
                var j = S.Length - i - 1;
                if (S[i] != S[j]) return -1;
            }
            return m;
        }

        public class Tester : BaseSelfTester<StrSymmetryPoint>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateInputSet(3, "racecar");
                yield return CreateInputSet(0, "X");
            }
        }
    }
}
