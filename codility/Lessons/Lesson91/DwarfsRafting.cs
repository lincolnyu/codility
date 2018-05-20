using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson91
{
    class DwarfsRafting : ITestee
    {
        private void GetRowCol(string s, out int row, out int col)
        {
            row = int.Parse(s.Substring(0, s.Length - 1))-1;
            col = s[s.Length - 1] - 'A';
        }

        public int solution(int N, string S, string T)
        {
            int[,] d = new int[2, 2];
            int[,] b = new int[2, 2];
            var ss = string.IsNullOrWhiteSpace(S)? new string[0]: S.Split(' ');
            var st = string.IsNullOrWhiteSpace(T)? new string[0]: T.Split(' ');
            var hn = N / 2;
            var quarterSize = hn * hn;
            foreach (var s in ss)
            {
                GetRowCol(s, out int r, out int c);
                b[r / hn, c / hn]++;
            }
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    b[i, j] = quarterSize - b[i, j];
                }
            }
            var ab = Math.Min(b[0, 0], b[1, 1]);
            var cd = Math.Min(b[0, 1], b[1, 0]);
            foreach (var t in st)
            {
                GetRowCol(t, out int r, out int c);
                d[r / hn, c / hn]++;
            }
            var a00 = ab - d[0, 0];
            if (a00 < 0) return -1;
            var a11 = ab - d[1, 1];
            if (a11 < 0) return -1;
            var a01 = cd - d[0, 1];
            if (a01 < 0) return -1;
            var a10 = cd - d[1, 0];
            if (a10 < 0) return -1;
            return a00 + a11 + a01 + a10;
        }

        public object Run(params object[] args)
            => solution((int)args[0], (string)args[1], (string)args[2]);

        public class Tester : BaseSelfTester<DwarfsRafting>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create3InputSet(4, "1B 1C 4B 1D 2A", "3B 2D", 6);
                yield return Create3InputSet(2, "", "", 4);
                yield return Create3InputSet(4, "1B 1A 2A", "3C 4C", -1);
            }
        }
    }
}
