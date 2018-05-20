using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson91
{
    class DwarfsRafting : ITestee
    {
        public int solution(int N, string S, string T)
        {
            throw new System.NotImplementedException();
        }

        public object Run(params object[] args)
            => solution((int)args[0], (string)args[1], (string)args[2]);

        public class Tester : BaseSelfTester<DwarfsRafting>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create3InputSet(4, "1B 1C 4B 1D 2A", "3B 2D", 6);
            }
        }
    }
}
