using System;
using System.Collections.Generic;
using codility.TestFramework;

namespace codility.Lessons.Lesson10
{
    class Flags : ITestee
    {
        int Solve(int[] A)
        {
            var n = A.Length;
            var pd = new int[n / 2];
            var lastPeak = -1;
            var pdcount = 0;
            var mpd = 0;
            for (var i = 1; i < n-1; i++)
            {
                var a = A[i];
                if (a > A[i-1] && a > A[i+1])
                {
                    if (lastPeak >= 0)
                    {
                        var d = i - lastPeak;
                        pd[pdcount++] = d;
                        mpd += d;
                    }
                    lastPeak = i;
                    i++;
                }
            }
            if (pdcount == 0) return lastPeak >= 0 ? 1 : 0;
            if (pdcount == 1) return 2;
            var sqrtMpd = (int)Math.Floor(Math.Sqrt(mpd));
            var mpdRestrict = sqrtMpd * (sqrtMpd + 1) <= mpd ? sqrtMpd + 1 : sqrtMpd;
            var maxFlags = Math.Min(pdcount + 1, mpdRestrict);

            for (var fc = maxFlags; fc > 2; fc--)
            {
                var nreq = fc - 1;
                var stride = 0;
                for (var i = 0; i < pdcount && nreq > 0; i++)
                {
                    stride += pd[i];
                    if (stride >= fc)
                    {
                        nreq--;
                        stride = 0;
                    }
                }
                if (nreq == 0)
                {
                    return fc;
                }
                else if (nreq == 1)
                {
                    return fc - 1;
                }
            }
            return 2;
        }

        public object Run(params object[] args)
            => Solve((int[])args[0]);

        public class Tester : BaseSelfTester<Flags>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateSingleInputSet(new[] { 1, 5, 3, 4, 3, 4, 1, 2, 3, 4, 6, 2 }, 3);
                yield return CreateSingleInputSet(new[] { 1, 3, 2 }, 1);
                yield return CreateSingleInputSet(new[] { 5 }, 0);
            }
        }
    }
}
