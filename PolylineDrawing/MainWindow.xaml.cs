using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace PolylineDrawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Polyline? polyline;        
        private bool isLineDrawing = false;

        public MainWindow()
        {
            InitializeComponent();

            // temp : to be implemented with algobox interface
            MainCanvas.MouseDown += MainCanvas_MouseDown;
            MainCanvas.MouseMove += MainCanvas_MouseMove;
            MainCanvas.MouseUp += MainCanvas_MouseUp;
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
                Point startPoint = polyline!.Points[0];

                
                PointCollection midPoints = CalculateMidLine(startPoint, endPoint);
                PointCollection points = new PointCollection(4)
                {
                    startPoint,
                    midPoints[0],
                    midPoints[1],
                    endPoint
                };

                polyline.Points = points;
            }
        }

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                MainCanvas.CaptureMouse();
                Point startPoint = e.GetPosition(MainCanvas);

                polyline = new Polyline();
                polyline.MouseEnter += Polyline_MouseEnter;
                polyline.MouseLeave += Polyline_MouseLeave;
                polyline.StrokeThickness = 3;
                polyline.Stroke = Brushes.White;

                PointCollection points = new PointCollection();
                points.Add(startPoint);
                polyline.Points = points;

                MainCanvas.Children.Add(polyline);
                isLineDrawing = true;
            }
        }

        private void Polyline_MouseLeave(object sender, MouseEventArgs e)
        {
            Polyline? polyline = sender as Polyline;
            if(polyline != null)
            {
                polyline.Stroke = Brushes.White;
                polyline.MouseDown -= Polyline_MouseDown;
                polyline.MouseMove -= Polyline_MouseMove;
                polyline.MouseUp -= Polyline_MouseUp;

                // temp : to be implemented with algobox interface
                MainCanvas.MouseDown += MainCanvas_MouseDown;
                MainCanvas.MouseMove += MainCanvas_MouseMove;
                MainCanvas.MouseUp += MainCanvas_MouseUp;
            }
        }

        private void Polyline_MouseEnter(object sender, MouseEventArgs e)
        {
            Polyline? polyline = sender as Polyline;
            if (polyline != null)
            {
                polyline.Stroke = Brushes.Red;
                polyline.MouseDown += Polyline_MouseDown;
                polyline.MouseMove += Polyline_MouseMove;
                polyline.MouseUp += Polyline_MouseUp;

                // temp : to be implemented with algobox interface
                MainCanvas.MouseDown -= MainCanvas_MouseDown;
                MainCanvas.MouseMove -= MainCanvas_MouseMove;
                MainCanvas.MouseUp -= MainCanvas_MouseUp;
            }
        }

        private void Polyline_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Polyline? polyline = sender as Polyline;
            if (polyline != null)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    polyline.ReleaseMouseCapture();
                    //Debug.WriteLine("Polyline_MouseUp");
                }
            }
        }

        private void Polyline_MouseMove(object sender, MouseEventArgs e)
        {
            Polyline? polyline = sender as Polyline;
            if (polyline != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    //Debug.WriteLine("Polyline_MouseMove");                    
                    Point startPoint = polyline!.Points[0];
                    Point midPoint1 = polyline!.Points[1];
                    Point midPoint2 = polyline!.Points[2];
                    Point endPoint = polyline!.Points[3];

                    Point currentPosition = e.GetPosition(MainCanvas);
                    midPoint1.X = currentPosition.X;
                    midPoint2.X = currentPosition.X;

                    polyline.Points = new PointCollection(4)
                    {
                        startPoint,
                        midPoint1,
                        midPoint2,
                        endPoint
                    };

                }
            }
        }

        private void Polyline_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Polyline? polyline = sender as Polyline;
            if (polyline != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    polyline.CaptureMouse();
                    //Debug.WriteLine("Polyline_MouseDown");
                }
            }
        }

        private PointCollection CalculateMidLine(Point startPoint, Point endPoint)
        {
            PointCollection points = new PointCollection();

            Point midPoint1 = new Point(
                (endPoint.X - startPoint.X) * 0.5 + startPoint.X,
                startPoint.Y);
            Point midPoint2 = new Point(
                (endPoint.X - startPoint.X) * 0.5 + startPoint.X,
                endPoint.Y);

            points.Add(midPoint1);
            points.Add(midPoint2);

            return points;
        }
    }
}
