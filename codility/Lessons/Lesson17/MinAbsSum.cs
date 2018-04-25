using System;
using System.Collections.Generic;
using System.Linq;
using codility.TestFramework;

namespace codility.Lessons.Lesson17
{
    class MinAbsSum : ITestee
    {
        int Solve(int[] A)
        {
            var possibleBuffers = new HashSet<int>[2]
            {
                new HashSet<int>(),
                new HashSet<int>()
            };
            var pointer = 0;
            possibleBuffers[1 - pointer].Add(0);
            foreach (var a in A)
            {
                var aa = Math.Abs(a);
                var prevset = possibleBuffers[1 - pointer];
                var currset = possibleBuffers[pointer];
                foreach (var x in prevset)
                {
                    currset.Add(x + aa);
                    currset.Add(x - aa);
                }
                prevset.Clear();
                pointer = 1 - pointer;
            }
            return possibleBuffers[1 - pointer].Min(x => Math.Abs(x));
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<MinAbsSum>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 91, 92, 93, 94, 95, 96, 97 }, 82);
                yield return CreateSingleInputSet(new[] { 1, 5, 2, -2 }, 0);
            }
        }
    }
}
