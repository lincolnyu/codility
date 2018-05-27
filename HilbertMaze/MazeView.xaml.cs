using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HilbertMaze
{
    /// <summary>
    /// Interaction logic for MazeView.xaml
    /// </summary>
    public partial class MazeView : Window
    {
        private double _aw;
        private double _ah;
        private double _sizeSquare;
        private double _tep;

        public MazeView(int n)
        {
            N = n;
            InitializeComponent();
        }

        public int N { get; }

        public int MazeSize => GetMazeSize(N);

        private int GetMazeSize(int n) => 2 * (1 << n) + 1;

        public int SizeLength => Math.Max(200, Math.Min(800, MazeSize * 10));

        private void WindowOnLoaded(object sender, RoutedEventArgs e)
        {
            var targetSize = SizeLength;
            Width = targetSize;
            Height = targetSize;
            var dh = targetSize - DrawingCanvas.ActualHeight;
            var dw = targetSize - DrawingCanvas.ActualWidth;
            Width += dw;
            Height += dh;
        }

        private void ReDraw()
        {
            DrawingCanvas.Children.Clear();
            DrawMaze();
            DrawGrid();
        }

        private void DrawMaze()
        {
            var h = MazeSize / 2;
            DrawStructure(0, N, h, h);
        }

        private IEnumerable<(int, (int, int))> GetSubOrient(int orient)
        {
            // 0: (1,0) or [1,0;0,1]
            // 1: (0,1) or [0,-1;1,0] (90 degree)
            // 2: (-1,0) or [-1,0;0,-1] (180 degree)
            // 3: (0,-1) or [0,1;-1,0] (-90 degree)
            var map = new[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
            (int, int) go((int, int) m, (int, int) orig)
            => (m.Item1 != 0) ? (orig.Item1 * m.Item1, orig.Item2 * m.Item1)
                    : (-orig.Item2 * m.Item2, orig.Item1 * m.Item2);

            var mo = map[orient];
            var or1 = (orient + 1) % 4;
            //(-1,1) * 
            yield return (or1, go(mo, (-1, 1)));
            var or2 = orient;
            // (-1,-1)
            yield return (or2, go(mo, (-1, -1)));
            var or3 = orient;
            // (1,-1)
            yield return (or3, go(mo, (1, -1)));
            var or4 = (orient + 3) % 4;
            // (1,1)
            yield return (or4, go(mo, (1, 1)));
        }

        // orient: 0 up; 1 right; 2 down; 3 left
        private void DrawStructure(int orient, int n, int row, int col)
        {
            if (n > 1)
            {
                var ms = GetMazeSize(n);
                var c = ms / 4;
                var nn = n - 1;
                var subo = GetSubOrient(orient);
                foreach (var (so, (sc, sr)) in subo)
                {
                    DrawStructure(so, nn, row + sr * c, col + sc * c);
                }
                var cross = new[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
                var crosso = cross[orient];
                var jointBrush = Brushes.Green;
                DrawDot(row + crosso.Item2, col + crosso.Item1, jointBrush);
                if (orient % 2 == 0)
                {
                    DrawDot(row, col - ms / 2 + 1, jointBrush);
                    DrawDot(row, col + ms / 2 - 1, jointBrush);
                }
                else
                {
                    DrawDot(row - ms / 2 + 1, col, jointBrush);
                    DrawDot(row + ms / 2 - 1, col, jointBrush);
                }
            }
            else
            {
                var opening = new[] { (1, 0), (0, -1), (-1, 0), (0, 1) };
                var oo = opening[orient];
                for (var i = -1; i <= 1; i++)
                {
                    for (var j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0 || i == oo.Item1 && j == oo.Item2) continue;
                        DrawDot(row + i, col + j);
                    }
                }
            }
        }

        private void DrawDot(int row, int col)
            => DrawDot(row, col, Brushes.Blue);

        private void DrawDot(int row, int col, SolidColorBrush brush)
        {
            var y = (_ah - _sizeSquare) / 2 + row * _tep;
            var x = (_aw - _sizeSquare) / 2 + col * _tep;

            var rect = new Rectangle
            {
                Fill = brush,
                Width = _tep,
                Height = _tep,
            };
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
            DrawingCanvas.Children.Add(rect);
        }

        private void DrawGrid()
        {
            for (var i = 0; i <= MazeSize; i++)
            {
                var line = new Line
                {
                    Stroke = Brushes.Black,
                    X1 = (_aw - _sizeSquare) / 2,
                    X2 = (_aw + _sizeSquare) / 2,
                };
                line.Y2 = line.Y1 = (_ah - _sizeSquare) / 2 + i * _tep;
                DrawingCanvas.Children.Add(line);

                var line2 = new Line
                {
                    Stroke = Brushes.Black,
                    Y1 = (_ah - _sizeSquare) / 2,
                    Y2 = (_ah + _sizeSquare) / 2
                };
                line2.X1 = line2.X2 = (_aw - _sizeSquare) / 2 + i * _tep;
                DrawingCanvas.Children.Add(line2);
            }
        }

        private void DrawingCanvasOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _aw = DrawingCanvas.ActualWidth;
            _ah = DrawingCanvas.ActualHeight;
            _sizeSquare = Math.Min(_aw, _ah);
            _tep = _sizeSquare / MazeSize;

            ReDraw();
        }
    }
}
