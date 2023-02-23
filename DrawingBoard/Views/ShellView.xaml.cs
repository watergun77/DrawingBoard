using DrawingBoard.ViewModels;
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
using System.Windows.Shapes;

namespace DrawingBoard.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        private ShellViewModel vm;
        public ShellView()
        {
            InitializeComponent();            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vm = (ShellViewModel)DataContext;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            vm.AddAlgoBlock();
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            vm.RemoveAlgoBlock();
        }


    }
}
