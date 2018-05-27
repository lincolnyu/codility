using System.Windows;

namespace HilbertMaze
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawBtnOnClick(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(Input.Text, out var n) && n >= 1 && n <= 20)
            {
                var mazeView = new MazeView(n);
                mazeView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                mazeView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Invalid input for integer N");
            }
        }
    }
}
