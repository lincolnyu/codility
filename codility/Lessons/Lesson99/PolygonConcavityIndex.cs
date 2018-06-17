using codility.TestFramework;
using System.Collections.Generic;

namespace codility.Lessons.Lesson99
{
    class PolygonConcavityIndex : BaseTestee
    {
        enum Dir
        {
            Straight,
            Left,
            Right
        }

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
            Dir? dominantDir = null;
            for (var i = 0; i < A.Length; i++)
            {
                var i0 = i - 1;
                if (i0 < 0) i0 += A.Length;
                var i2 = i + 1;
                if (i2 >= A.Length) i2 -= A.Length;
                var p0 = A[i0];
                var p = A[i];
                var p2 = A[i2];
                var dir = GetDir(p0, p, p2);
                if (dominantDir.HasValue)
                {
                    if (dir != Dir.Straight && dominantDir.Value != dir)
                    {
                        return i;
                    }
                }
                else if (dir != Dir.Straight)
                {
                    dominantDir = dir;
                }
            }
            return -1;
        }

        private Dir GetDir(Point2D p0, Point2D p1, Point2D p2)
        {
            var dx1 = p1.x - p0.x;
            var dy1 = p1.y - p0.y;
            var dx2 = p2.x - p1.x;
            var dy2 = p2.y - p1.y;

            //    dx2 + dy2 j
            //   -------------
            //    dx1 + dy1 j
            //var diff_x = dx2 * dx1 + dy2 * dy1;
            var diff_y = dx1 * dy2 - dx2 * dy1;
            if (diff_y > 0) return Dir.Left;
            if (diff_y < 0) return Dir.Right;
            return Dir.Straight;
        }

        public class Tester : BaseSelfTester<PolygonConcavityIndex>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return CreateInputSet(-1, (object)new[] {
                   new Point2D( -1, 3 ),
                   new Point2D(1,2),
                   new Point2D(3,1),
                   new Point2D(0,-1),
                   new Point2D(-2,1)
                });
                yield return CreateInputSet(2, (object)new[] {
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
