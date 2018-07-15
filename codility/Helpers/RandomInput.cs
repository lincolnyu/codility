using System;
using System.Collections.Generic;

namespace codility.Helpers
{
    static class RandomInput
    {
        // Generates random numbers ranging 1 to 'len' with each appearing exactly once
        public static int[] GenerateRandomSequence(this Random rand, int len)
        {
            var buf = new int[len];
            for(var i = 0; i < len; i++)
            {
                buf[i] = i;
            }
            int shuffle = len;
            var d = rand.Next(len);
            for (var i = 0; i < shuffle; i++)
            {
                var newd = rand.Next(len);
                var temp = buf[d];
                buf[d] = buf[newd];
                buf[newd] = temp;
                d = newd;
            }
            return buf;
        }
    }
}
