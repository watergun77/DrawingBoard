using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LineDrawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainCanvas.MouseDown += MainCanvas_MouseDown;
            MainCanvas.MouseMove += MainCanvas_MouseMove;
            MainCanvas.MouseUp += MainCanvas_MouseUp;

            TestButton.Click += TestButton_Click;

        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private Point startPoint;
        private Line? line;
        private bool isLineDrawing = false;

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                MainCanvas.CaptureMouse();
                startPoint = e.GetPosition(MainCanvas);

                line = new Line();
                line.MouseEnter += Line_MouseEnter;
                line.MouseLeave += Line_MouseLeave;
                line.StrokeThickness = 3;
                line.Stroke = Brushes.White;
                line.X1 = startPoint.X;
                line.Y1 = startPoint.Y;
                line.X2 = startPoint.X;
                line.Y2 = startPoint.Y;
                MainCanvas.Children.Add(line);
                isLineDrawing = true;
            }
        }

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                MainCanvas.ReleaseMouseCapture();
                isLineDrawing = false;
            }
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {            
            if (isLineDrawing && e.LeftButton == MouseButtonState.Pressed)
            {
                Point endPoint = e.GetPosition(MainCanvas);
                line.X2 = endPoint.X;
                line.Y2 = endPoint.Y;
            }
        }

        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            Line? thisLine = sender as Line;
            thisLine.Stroke = Brushes.White;
        }

        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            Line? thisLine = sender as Line;
            thisLine.Stroke = Brushes.Red;
        }

    }
}
