using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson99
{
    class ArrayInversionCount : BaseTestee
    {
        class Pod : IComparable<Pod>
        {
            public int Val;
            public int Index;

            public int CompareTo(Pod other)
            {
                var c = Val.CompareTo(other.Val);
                if (c != 0) return c;
                return Index.CompareTo(other.Index);
            }
        }

        interface IPickable
        {
            int LeftCount { get; set; }
            bool Picked { get; set; }
        }

        class Node : IPickable
        {
            public int LeftCount { get; set; }
            public bool Picked { get; set; }
        }

        public int solution(int[] A)
        {
            int n = A.Length;
            var pods = new Pod[n];
            for (var i = 0; i < n; i++)
            {
                pods[i] = new Pod { Val = A[i], Index = i };
            }
            Array.Sort(pods);

            var nodes = new Node[n];
            for (var i = 0; i < n; i++)
            {
                nodes[i] = new Node { };
            }

            var total = 0;
            var count = 0;
            for (var i = n - 1; i >= 0; i--, count++)
            {
                var pod = pods[i];
                var offset = GetIndexAndUpdate(nodes, pod.Index);
                total += i - pod.Index + offset;
                if (total > 1000000000) return -1;
            }
            return total;
        }

        private int GetIndexAndUpdate(IPickable[] pickable, int t)
        {
            var n = pickable.Length;
            var index = 0;
            var begin = 0;
            var end = pickable.Length-1;
            var stack = new Stack<int>();
            while(begin <= end)
            {
                var p = (begin + end) / 2;
                var c = pickable[p];
                if (p > t)
                {
                    end = p - 1;
                }
                else if (p < t)
                {
                    begin = p + 1;
                    index += c.LeftCount;
                    if (c.Picked) index++;
                }
                else
                {
                    index += c.LeftCount;
                    c.Picked = true;
                    while (stack.Count > 0)
                    {
                        var i = stack.Pop();
                        if (i > p)
                        {
                            pickable[i].LeftCount++;
                        }
                    }
                    break;
                }
                stack.Push(p);
            }
            return index;
        }
        

        public static int ref_solution(int[] A)
        {
            var total = 0;
            for (var i = 0; i < A.Length-1; i++)
            {
                for (var j = i+1; j < A.Length; j++)
                {
                    if (A[j] < A[i])
                    {
                        total++;
                    }
                }
            }
            return total;
        }

        public class Tester : BaseSelfTester<ArrayInversionCount>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                return GetDistinctNumberTestSets();
                /*
                yield return CreateInputSet(6, new[] { 4, 3, 2, 1 });
                yield return Generate(new[] { 7, 1, 1, 2, 3, 4, 6, 5, 4, 3, 7, 8, 9 });
                yield return Generate(new[] { -1, 6, 3, 4, 7, 4 });
                yield return CreateInputSet(4, new[] { -1, 6, 3, 4, 7, 4 });*/
            }

            IEnumerable<TestSet> GetDistinctNumberTestSets()
            {
                var r = new Random(123);
                for (var t = 0; t < 10; t++)
                {
                    var max = r.Next(10000);
                    var seq = new int[max];
                    var used = new bool[max];
                    for (var i = 0; i < max; i++)
                    {
                        var n = r.Next(max - i);
                        var j = 0;
                        for (j= 0; j < max; j++)
                        {
                            if (!used[j])
                            {
                                if (n-- <= 0)
                                {
                                    used[j] = true;
                                    break;
                                }
                            }
                        }
                        seq[i] = j;
                    }
                    yield return Generate(seq);
                }
            }

            TestSet Generate(int[] A)
                => CreateInputSet(ref_solution(A), A);
        }
    }
}
