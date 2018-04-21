using System.Collections.Generic;

namespace codility.Lessons.Lesson14.Helper
{
    class BSHelper
    {
        public class BSBox
        {
            public int Index { get; set; }
            public int Dir { get; set; }
        }

        public static IEnumerable<BSBox> Generate(int begin, int end)
        {
            for (; begin <= end;)
            {
                var mid = (begin + end) / 2;
                var pod = new BSBox { Index = mid };
                yield return pod;
                if (pod.Dir == 0) yield break;
                if (pod.Dir > 0)
                {
                    begin = mid + 1;
                }
                else
                {
                    end = mid - 1;
                }
            }
        }
    }
}
