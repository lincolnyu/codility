using codility.TestFramework;
using System;
using System.Collections.Generic;

namespace codility.Lessons.Lesson99
{
    class TreeHeight : BaseTestee
    {
        public class Tree
        {
            public int x;
            public Tree l;
            public Tree r;
        }

        public int solution(Tree T)
        {
            if (T == null) return -1;
            return Math.Max(solution(T.l), solution(T.r)) + 1;
        }

        public class Tester : BaseSelfTester<TreeHeight>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateInputSet(0, new Tree { x = 1 });
                yield return CreateInputSet(-1, (Tree)null);
                yield return CreateInputSet(2,
                    new Tree
                    {
                        x = 5,
                        l = new Tree
                        {
                            x = 3,
                            l = new Tree
                            {
                                x = 20
                            },
                            r = new Tree
                            {
                                x = 21
                            }
                        },
                        r = new Tree
                        {
                            x = 10,
                            l = new Tree
                            {
                                x = 1
                            }
                        }
                    });
            }
        }
    }
}
