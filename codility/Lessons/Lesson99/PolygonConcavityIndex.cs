using codility.TestFramework;
using System.Collections.Generic;

namespace codility.Lessons.Lesson99
{
    class PolygonConcavityIndex : BaseTestee
    {
        public class Point2D
        {
            public int x;
            public int y;
            public Point2D(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public int solution(Point2D[] A)
        {
            var signedArea2 = GetSignedArea2(A);
            for (var i = 0; i < A.Length; i++)
            {
                var dir = TurnRate(A, i);
                if (dir < 0 && signedArea2 > 0 || dir > 0 && signedArea2 < 0)
                {
                    return i;
                }
            }
            return -1;
        }

        private int GetSignedArea2(Point2D[] A)
        {
            var area = 0;
            for (var i = 0; i < A.Length; i++)
            {
                var xi = A[i].x;
                var yi = A[i].y;
                var i1 = i + 1;
                if (i1 == A.Length) i1 = 0;
                var xi1 = A[i1].x;
                var yi1 = A[i1].y;
                var d = xi * yi1 - xi1 * yi;
                area += d;
            }
            return area;
        }

        private int TurnRate(Point2D[] A, int i)
        {
            var i0 = i - 1;
            if (i0 < 0) i0 += A.Length;
            var i2 = i + 1;
            if (i2 >= A.Length) i2 -= A.Length;
            var p0 = A[i0];
            var p1 = A[i];
            var p2 = A[i2];
            var dx1 = p1.x - p0.x;
            var dy1 = p1.y - p0.y;
            var dx2 = p2.x - p1.x;
            var dy2 = p2.y - p1.y;

            //    dx2 + dy2 j
            //   -------------
            //    dx1 + dy1 j
            //var diff_x = dx2 * dx1 + dy2 * dy1;
            var diff_y = dx1 * dy2 - dx2 * dy1;
            return diff_y;
        }

        public class Tester : BaseSelfTester<PolygonConcavityIndex>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateInputSet(0, (object)new []
                {
                    new Point2D(0,0),
                    new Point2D(-1,1),
                    new Point2D(1,1),
                    new Point2D(-1,-1)
                });
                yield return CreateInputSet(-1, (object)new [] {
                   new Point2D( -1, 3 ),
                   new Point2D(1,2),
                   new Point2D(3,1),
                   new Point2D(0,-1),
                   new Point2D(-2,1)
                });
                yield return CreateInputSet(2, (object)new [] {
                    new Point2D(-1,3),
                    new Point2D(1,2),
                    new Point2D(1,1),
                    new Point2D(3,1),
                    new Point2D(0,-1),
                    new Point2D(-2,1),
                    new Point2D(-1,2)
                });
            }
        }
    }
}
