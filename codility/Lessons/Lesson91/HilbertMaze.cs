using System;
using System.Collections.Generic;
using System.Linq;
using codility.TestFramework;

namespace codility.Lessons.Lesson91
{
    public class HilbertMaze : ITestee
    {
        public enum Direction
        {
            Down = 0,
            Right,
            Up,
            Left
        }

        public class Cross
        {
            public int CenterRow;
            public int CenterCol;
            public Direction Dir;

            public bool Covers(int row, int col)
                => CenterRow == row || CenterCol == col;

            public void GetExit(int n, int row, int col, out int steps, out int orow, out int ocol)
            {
                var d = 1 << n;
                switch (Dir)
                {
                    case Direction.Down:
                        ocol = CenterCol;
                        steps = Math.Abs(ocol - col);
                        if (row < CenterRow)
                        {
                            orow = CenterRow - d;
                            steps += row - orow;
                        }
                        else 
                        {
                            orow = CenterRow + d;
                            steps += row > CenterRow ? orow - row : Math.Abs(CenterCol - ocol) + d;
                        }
                        break;
                    case Direction.Right:
                        orow = CenterRow;
                        steps = Math.Abs(orow - row);
                        if (col < CenterCol)
                        {
                            ocol = CenterCol - d;
                            steps += col - ocol;
                        }
                        else
                        {
                            ocol = CenterCol + d;
                            steps += col > CenterCol ? ocol - col : Math.Abs(CenterRow - orow) + d;
                        }
                        break;
                    case Direction.Up:
                        ocol = CenterCol;
                        steps = Math.Abs(ocol - col);
                        if (row > CenterRow)
                        {
                            orow = CenterRow + d;
                            steps += orow - row;
                        }
                        else
                        {
                            orow = CenterRow - d;
                            steps += row < CenterRow ? row - orow : Math.Abs(CenterCol - ocol) + d;
                        }
                        break;
                    case Direction.Left:
                        orow = CenterRow;
                        steps = Math.Abs(orow - row);
                        if (col > CenterCol)
                        {
                            ocol = CenterCol + d;
                            steps += ocol - col;
                        }
                        else
                        {
                            ocol = CenterCol - d;
                            steps += col < CenterCol ? col - ocol : Math.Abs(CenterRow - orow) + d;
                        }
                        break;
                    default:
                        throw new ArgumentException("Unexpected direction value");
                }
            }

            public Cross GetNext(int n, int row, int col)
            {
                int quarter;
                if (row < CenterRow)
                {
                    quarter = col > CenterCol ? 0 : 1;
                }
                else
                {
                    quarter = col > CenterCol ? 3 : 2;
                }

                var d = 1 << n;

                var dir = GetNext(Dir, quarter);
                return new Cross
                {
                    Dir = dir,
                    CenterRow = row < CenterRow? CenterRow-d : CenterCol + d,
                    CenterCol = col < CenterCol? CenterCol-d : CenterCol + d
                };
            }

            public int GetDist(int row1, int col1, int row2, int col2)
            {
                if (row1 == row2)
                {
                    return Math.Abs(col1 - col2) - 1;
                }
                else if (col1 == col2)
                {
                    return Math.Abs(row1 - row2) - 1;
                }
                else
                {
                    return Math.Abs(row1 - CenterRow + col1 - CenterCol)
                        + Math.Abs(row2 - CenterRow + col2 - CenterCol) - 1;
                }
            }

            private Direction GetNext(Direction currDir, int quarter)
            {
                Direction b;
                if (quarter < 2) b = Direction.Down;
                else if (quarter == 2) b = Direction.Left;
                else b = Direction.Right;
                var i = (int)b + (int)currDir;
                return (Direction)(i % 4);
            }

            private static int GetDistOnBorder0(int n, int col1, int row2, int col2)
            {
                var end = 1 << (n + 1);
                if (row2 == 0)
                {
                    return Math.Abs(col2 - col1) - 1;
                }
                else if (row2 == end)
                {
                    return Math.Min(col2 + col1, end * 2 - (col2 + col1)) + end - 1;
                }
                else if (col2 == 0)
                {
                    return col1 + row2 - 1;
                }
                else
                {
                    return end - col1 + row2 - 1;
                }
            }

            public static int GetDistOnBorder(int n, int row1, int col1, int row2, int col2)
            {
                var end = 1 << (n + 1);
                if (row1 == 0)
                {
                    return GetDistOnBorder0(n, col1, row2, col2);
                }
                else if (row1 == end)
                {
                    return GetDistOnBorder0(n, col1, end - row2, col2);
                }
                else if (col1 == 0)
                {
                    return GetDistOnBorder0(n, row1, col2, row2);
                }
                else // col1 == end
                {
                    return GetDistOnBorder0(n, row1, end - col2, row2);
                }
            }
        }

        // req: O(N), O(N)
        public int solution(int N, int A, int B, int C, int D)
        {
            var end = 1 << (N + 1);
            B = end - B;
            D = end - D;
            var crossesA = GetCrosses(N, B, A);
            var crossesC = GetCrosses(N, D, C);
            var distsA = GetDists(crossesA, N, B, A).Reverse().ToArray();
            var distsC = GetDists(crossesC, N, D, C).Reverse().ToArray();
            var i = 0;
            for (; i < distsA.Length && i < distsC.Length; i++)
            {
                if (distsA[i].Item4 == null || distsC[i].Item4 == null
                    || distsA[i].Item2 != distsC[i].Item2 || distsA[i].Item3 != distsC[i].Item3)
                {
                    break;
                }
            }
            int outd;
            if (i > 0)
            {
                var crossA = distsA[i - 1].Item4;
                outd = crossA.GetDist(distsA[i].Item2, distsA[i].Item3, distsC[i].Item2, distsC[i].Item3);
            }
            else
            {
                outd = Cross.GetDistOnBorder(N, distsA[i].Item2, distsA[i].Item3, distsC[i].Item2, distsC[i].Item3);
            }
            var sumA = 0;
            for (var j = i; j < distsA.Length; j++)
            {
                sumA += distsA[j].Item1;
            }
            var sumB = 0;
            for (var j = i; j < distsC.Length; j++)
            {
                sumB += distsC[j].Item1;
            }
            return outd + sumA + sumB + 1;
        }

        // distance to outlet, position of outlet
        public IEnumerable<Cross> GetCrosses(int n, int row, int col)
        {
            // len: 2(N+1)+1
            //var len = (1 << (N + 1)) + 1;
            var end = 1 << (n + 1);
            if (row == 0 || col == 0 || row == end || col == end)
            {
                yield break;
            }

            var cross = new Cross
            {
                Dir = Direction.Down,
                CenterRow = 1 << n,
                CenterCol = 1 << n
            };
            yield return cross;

            while (!cross.Covers(row, col))
            {
                cross = cross.GetNext(--n, row, col);
                yield return cross;
            }
        }

        public IEnumerable<Tuple<int, int, int, Cross>> GetDists(IEnumerable<Cross> crosses, int n, int row, int col)
        {
            yield return new Tuple<int, int, int, Cross>(0, row, col, null);
            var rcrosses = crosses.Reverse().ToArray();
            if (rcrosses.Length > 0)
            {
                var i = n - rcrosses.Length + 1;
                foreach (var cross in rcrosses)
                {
                    if (cross.Covers(row, col))
                    {
                        cross.GetExit(i, row, col, out var steps, out var orow, out var ocol);
                        yield return new Tuple<int, int, int, Cross>(steps, orow, ocol, cross);
                        row = orow;
                        col = ocol;
                    }
                    i++;
                }
            }
        }

        public object Run(params object[] args)
            => solution((int)args[0], (int)args[1], (int)args[2], (int)args[3], (int)args[4]);

        public class Tester : BaseSelfTester<HilbertMaze>
        {
            public override IEnumerable<TestSet> GetTestSets()
            {
                yield return Create5InputSet(1, 2, 1, 2, 1, 0);
                yield return Create5InputSet(1, 2, 0, 0, 0, 2);
                yield return Create5InputSet(1, 2, 1, 2, 2, 1);
                yield return Create5InputSet(1, 1, 0, 1, 0, 0);
                yield return Create5InputSet(1, 0, 0, 2, 1, 3);
                yield return Create5InputSet(1, 2, 0, 2, 1, 1);
                yield return Create5InputSet(1, 2, 0, 2, 0, 0);
                yield return Create5InputSet(1, 2, 1, 3, 4, 8);
                yield return Create5InputSet(2, 2, 5, 6, 6, 7);
                yield return Create5InputSet(3, 6, 6, 10, 13, 39);
            }
        }
    }
}
