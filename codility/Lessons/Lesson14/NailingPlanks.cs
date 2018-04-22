using codility.Lessons.Lesson14.Helper;
using codility.TestFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace codility.Lessons.Lesson14
{
    class NailingPlanks : ITestee
    {
        class Pair : IComparable<Pair>
        {
            public int Step;
            public int NailPos;
            public int CompareTo(Pair other)
            {
                var c = NailPos.CompareTo(other.NailPos);
                if (c != 0) return c;
                return Step.CompareTo(other.Step);
            }
        }

        IEnumerable<Pair> Cleanup(IEnumerable<Pair> pairs)
        {
            var lastPos = -1;
            foreach (var p in pairs)
            {
                if (p.NailPos != lastPos)
                {
                    yield return p;
                    lastPos = p.NailPos;
                }
            }
        }
        
        public int Solve(int[] A, int[] B, int[] C)
        {
            var pairs = new Pair[C.Length];
            for (var i = 0; i < C.Length; i++)
            {
                var pair = new Pair { Step = i+1, NailPos = C[i] };
                pairs[i] = pair;
            }
            Array.Sort(pairs);
            pairs = Cleanup(pairs).ToArray();

            var minNeeded = 0;
            for (var i = 0; i < A.Length; i++)
            {
                var a = A[i];
                var b = B[i];
                var bin = BSHelper.Generate(0, pairs.Length - 1);
                int index = -1;

                foreach (var z in bin)
                {
                    var p = pairs[z.Index];
                    if (a <= p.NailPos && p.NailPos <= b)
                    {
                        index = z.Index;
                        break;
                    }
                    else if (b < p.NailPos )
                    {
                        z.Dir = -1;
                    }
                    else if (p.NailPos < a)
                    {
                        z.Dir = 1;
                    }
                }
                if (index < 0) return -1;
                var step = pairs[index].Step;
                if (step > minNeeded)
                {
                    for (var j = index + 1; j < pairs.Length && pairs[j].NailPos <= b; j++)
                    {
                        if (pairs[j].Step < step)
                        {
                            step = pairs[j].Step;
                            if (step <= minNeeded) break;
                        }
                    }
                }
                if (step > minNeeded)
                {
                    for (var j = index - 1; j >= 0 && pairs[j].NailPos >= a; j--)
                    {
                        if (pairs[j].Step < step)
                        {
                            step = pairs[j].Step;
                            if (step <= minNeeded) break;
                        }
                    }
                }
                if (step > minNeeded) minNeeded = step;
            }
            return minNeeded;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0], (int[])args[1], (int[])args[2]);

        public class Tester : BaseSelfTester<NailingPlanks>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create3InputSet(new[] { 1, 4, 5, 8 }, new[] { 4, 5, 9, 10 }, new[] { 4, 6, 7, 10, 2 }, 4);
            }
        }
    }
}
