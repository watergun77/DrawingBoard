using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Xml.Linq;

namespace Boxes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double bodyHeight = 150;
        private const double bodyWidth = 150;
        //private const double interfaceWidth = 10;
        private const double interfaceWidth = 20;
        private bool isMovingAlgoBox = false;

        public MainWindow()
        {
            InitializeComponent();

            Canvas[] algoBoxes = 
            {
                drawDspAlgo(10, 10, 1, 8),
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

        #region Drag to change position
        private void AlgoBox_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas? box = sender as Canvas;
            if (box != null && isMovingAlgoBox && e.LeftButton == MouseButtonState.Pressed)
            {
                // get the position of the mouse relative to the main Canvas
                var mousePos = e.GetPosition(MainCanvas);
                //Debug.WriteLine($"mousePos : X={mousePos.X} Y={mousePos.Y}");

                // center the rect on the mouse
                double left = mousePos.X;
                double top = mousePos.Y;

                Canvas.SetLeft(box, left);
                Canvas.SetTop(box, top);
                e.Handled = true;
            }
        }

        private void AlgoBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas box = sender as Canvas;
            if (box != null)
            {
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
                Point boxPosition = box.PointToScreen(new Point(0, 0));
                SetCursorPos((int)boxPosition.X, (int)boxPosition.Y);

                box.CaptureMouse();

                isMovingAlgoBox = true;
                e.Handled = true;
            }
        }
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        #endregion

        private void Interface_MouseLeave(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("Interface_MouseLeave");
            Polygon? inOut = sender as Polygon;
            if (inOut != null) 
            { 
                inOut.Fill = Brushes.Gray; 
            }
        }
        private void Interface_MouseEnter(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("Interface_MouseEnter");
            Polygon? inOut = sender as Polygon;
            if (inOut != null)
            {
                inOut.Fill = Brushes.LightGray;
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
        private Polygon drawInterface(double x=0, double y=0)
        {
            Polygon triangle = new Polygon();
            triangle.StrokeThickness = 0;

            //Point Point1 = new Point(x, -5 + y);
            //Point Point2 = new Point(x, 5 + y);
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
            triangle.Fill = Brushes.Gray;
            triangle.MouseLeftButtonDown += InOut_MouseLeftButtonDown;

            return triangle;
        }

        private Canvas drawDspAlgo(double left=0, double top = 0, int totalInput=1, int totalOutput=1)
        {
            Canvas group = new Canvas();

            // Body creation
            Rectangle body = drawBody();
            group.Children.Add(body);

            //Draw text label
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
            }

            // Draw output interfaces
            for (int j = 1; j <= totalOutput; j++)
            {
                // Draw output interface
                Polygon output = drawInterface(xGap, j * yGapOutput);
                output.MouseEnter += Interface_MouseEnter;
                output.MouseLeave += Interface_MouseLeave;
                group.Children.Add(output);
            }


            // Finalise position of the entire group.
            Canvas.SetTop(group, top);
            Canvas.SetLeft(group, left);

            return group;
        }

        private void InOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //todo
            e.Handled = true;
        }

    }
}
