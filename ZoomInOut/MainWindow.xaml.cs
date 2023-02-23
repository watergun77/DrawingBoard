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

namespace ZoomInOut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ZoomViewbox.Width = 100;
            ZoomViewbox.Height = 100;
        }

        private void MainWindow_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            UpdateViewBox((e.Delta > 0) ? 5 : -5);
        }

        private void UpdateViewBox(int newValue)
        {
            if ((ZoomViewbox.Width >= 0) && ZoomViewbox.Height >= 0)
            {
                ZoomViewbox.Width += newValue;
                ZoomViewbox.Height += newValue;
            }
        }
    }
}
