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

namespace ZoomInOut3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double bodyHeight = 150;
        private const double bodyWidth = 150;
        private const double interfaceWidth = 15;
        private bool isMovingAlgoBox = false;
        private Polyline? polyline;
        private bool isLineDrawing = false;

        private Dictionary<Polygon, Point> snappingPoints = new();
        private Canvas? selectedGroup = null;

        public MainWindow()
        {
            InitializeComponent();

            Canvas[] algoBoxes =
            {
                drawDspAlgo(10, 10, 1, 2),
                drawDspAlgo(500, 20, 2, 1),
                drawDspAlgo(10, 10, 1, 2),
                drawDspAlgo(500, 20, 2, 1),
            };

            foreach (var algoBox in algoBoxes)
            {
                algoBox.MouseLeftButtonDown += AlgoBox_MouseLeftButtonDown;
                algoBox.MouseLeftButtonUp += AlgoBox_MouseLeftButtonUp;
                algoBox.MouseMove += AlgoBox_MouseMove;
                MainCanvas.Children.Add(algoBox);
            }
        }

        #region Drag to change AlgoBox position
        private void AlgoBox_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas? box = sender as Canvas;
            if (box != null && isMovingAlgoBox && e.LeftButton == MouseButtonState.Pressed)
            {
                // get the position of the mouse relative to the main Canvas
                var mousePos = e.GetPosition(MainCanvas);
                //Debug.WriteLine($"mousePos : X={mousePos.X} Y={mousePos.Y}");

                // center the rect on the mouse
                double left = mousePos.X - (bodyWidth / 2);
                double top = mousePos.Y - (bodyHeight / 2);

                Canvas.SetLeft(box, left);
                Canvas.SetTop(box, top);
                e.Handled = true;
            }
        }

        private void AlgoBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("AlgoBox_MouseLeftButtonUp");
            Canvas box = sender as Canvas;
            if (box != null)
            {
                // To remove line if mouse is not release in the correct position
                if (isLineDrawing)
                {
                    MainCanvas.Children.Remove(polyline);
                    isLineDrawing = false;
                }

                box.ReleaseMouseCapture();
                isMovingAlgoBox = false;
                e.Handled = true;
            }
        }

        private void AlgoBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas? box = sender as Canvas;
            if (box != null)
            {
                box.CaptureMouse();
                isMovingAlgoBox = true;
                e.Handled = true;
            }
        }
        #endregion

        private void Interface_MouseLeave(object sender, MouseEventArgs e)
        {
            Polygon? inOut = sender as Polygon;
            if (inOut != null)
            {
                inOut.Fill = Brushes.Gray;
                e.Handled = true;
            }
        }
        private void Interface_MouseEnter(object sender, MouseEventArgs e)
        {
            Polygon? inOut = sender as Polygon;
            if (inOut != null)
            {
                inOut.Fill = Brushes.LightGray;
                selectedGroup = (Canvas)inOut.Parent;
                e.Handled = true;
            }
        }
        private void Body_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle? rect = sender as Rectangle;
            if (rect != null)
            {
                rect.StrokeThickness = 0;
                e.Handled = true;
            }
        }
        private void Body_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle? rect = sender as Rectangle;
            if (rect != null)
            {                
                rect.Stroke = Brushes.Red;
                rect.StrokeThickness = 2;                
                e.Handled = true;
            }
        }
        private Rectangle drawBody()
        {
            Rectangle body = new Rectangle();

            body.Fill = Brushes.LightBlue;
            body.StrokeThickness = 0;
            body.Height = bodyHeight;
            body.Width = bodyWidth;

            body.MouseEnter += Body_MouseEnter;
            body.MouseLeave += Body_MouseLeave;
            return body;
        }
        private Canvas drawDspAlgo(double left = 0, double top = 0, int totalInput = 1, int totalOutput = 1)
        {
            Canvas group = new Canvas();

            // Body creation
            Rectangle body = drawBody();
            group.Children.Add(body);

            // Draw text label
            TextBlock textBlock = new TextBlock();
            textBlock.Width = bodyWidth;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.FontFamily = new FontFamily("Courier New");
            textBlock.Text = "Hello World";
            textBlock.RenderTransform = new TranslateTransform { Y = bodyHeight * 0.5 - 10 };
            group.Children.Add(textBlock);

            // Calculate gaps between interfaces
            double yGapInput = bodyHeight / (totalInput + 1);
            double yGapOutput = bodyHeight / (totalOutput + 1);
            double xGap = bodyWidth - interfaceWidth;

            // Draw input interfaces
            for (int i = 1; i <= totalInput; i++)
            {
                Polygon input = drawInterface(0, i * yGapInput);
                input.MouseEnter += Interface_MouseEnter;
                input.MouseLeave += Interface_MouseLeave;
                group.Children.Add(input);

                //capture input interface snapping point
                Point snapPoint = new Point(input.Points[0].X, input.Points[2].Y);
                snappingPoints.Add(input, snapPoint);               
            }

            // Draw output interfaces
            for (int j = 1; j <= totalOutput; j++)
            {
                // Draw output interface
                Polygon output = drawInterface(xGap, j * yGapOutput);
                output.MouseEnter += Interface_MouseEnter;
                output.MouseLeave += Interface_MouseLeave;
                group.Children.Add(output);

                //capture output interface snapping point
                Point snapPoint = new Point(output.Points[2].X, output.Points[2].Y);
                snappingPoints.Add(output, snapPoint);
            }

            // Finalise position of the entire group.
            Canvas.SetTop(group, top);
            Canvas.SetLeft(group, left);

            return group;
        }
        private Polygon drawInterface(double x = 0, double y = 0)
        {
            Polygon triangle = new Polygon();
            triangle.StrokeThickness = 0;
            triangle.Fill = Brushes.Gray;

            Point Point1 = new Point(x, -1 * (interfaceWidth * 0.5) + y);
            Point Point2 = new Point(x, interfaceWidth * 0.5 + y);
            Point Point3 = new Point(interfaceWidth + x, 0 + y);

            PointCollection myPointCollection = new PointCollection
            {
                Point1,
                Point2,
                Point3
            };
            triangle.Points = myPointCollection;
            triangle.MouseLeftButtonDown += InOut_MouseLeftButtonDown;
            triangle.MouseLeftButtonUp += InOut_MouseLeftButtonUp;

            return triangle;
        }
        private void InOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("InOut_MouseLeftButtonDown");
            MainCanvas.MouseMove += MainCanvas_MouseMove;
            MainCanvas.MouseLeftButtonUp += MainCanvas_MouseLeftButtonUp;

            Point outPoint = snappingPoints[(Polygon)sender];
            Point startPoint = selectedGroup.TranslatePoint(outPoint, MainCanvas); //snap to correct starting point (of algobox)

            polyline = new Polyline();
            polyline.MouseEnter += Polyline_MouseEnter;
            polyline.MouseLeave += Polyline_MouseLeave;
            polyline.StrokeThickness = 3;
            polyline.Stroke = Brushes.White;
            polyline.Points = new PointCollection
            {
                startPoint
            };

            MainCanvas.Children.Add(polyline);
            isLineDrawing = true;

            e.Handled = true;
        }
        private void MainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("MainCanvas_MouseLeftButtonUp");

            // To remove line if mouse is not release in the correct position
            if (isLineDrawing)
            {
                MainCanvas.Children.Remove(polyline);
                MainCanvas.MouseMove -= MainCanvas_MouseMove;
                MainCanvas.MouseLeftButtonUp -= MainCanvas_MouseLeftButtonUp;
                isLineDrawing = false;
                e.Handled = true;
            }
        }
        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("MainCanvas_MouseMove");
            if (isLineDrawing && e.LeftButton == MouseButtonState.Pressed)
            {
                Point endPoint = e.GetPosition(MainCanvas);
                endPoint.X -= 1;
                endPoint.Y -= 1;
                Point startPoint = polyline!.Points[0];

                PointCollection midPoints = CalculateMidLine(startPoint, endPoint);
                polyline.Points = new PointCollection(4)
                {
                    startPoint,
                    midPoints[0],
                    midPoints[1],
                    endPoint
                };

                e.Handled = true;
            }
        }
        private void InOut_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //todo : snap to correct ending point (of algobox)
            Debug.WriteLine("InOut_MouseLeftButtonUp");
            MainCanvas.MouseMove -= MainCanvas_MouseMove;
            MainCanvas.MouseLeftButtonUp -= MainCanvas_MouseLeftButtonUp;
            isLineDrawing = false;

            Point inPoint = snappingPoints[(Polygon)sender];
            Point endPoint = selectedGroup.TranslatePoint(inPoint, MainCanvas); //snap to correct ending point (of algobox)


            Point startPoint = polyline!.Points[0];
            PointCollection midPoints = CalculateMidLine(startPoint, endPoint);
            polyline.Points = new PointCollection(4)
                {
                    startPoint,
                    midPoints[0],
                    midPoints[1],
                    endPoint
                };

            e.Handled = true;
        }
        private void Polyline_MouseLeave(object sender, MouseEventArgs e)
        {
            Polyline? polyline = sender as Polyline;
            if (polyline != null)
            {
                polyline.Stroke = Brushes.White;
                polyline.MouseDown -= Polyline_MouseDown;
                polyline.MouseMove -= Polyline_MouseMove;
                polyline.MouseUp -= Polyline_MouseUp;
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
            }
        }
        private void Polyline_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Polyline_MouseUp");
            //if (isLineDrawing)
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
