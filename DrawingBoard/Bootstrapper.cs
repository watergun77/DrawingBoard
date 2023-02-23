using Caliburn.Micro;
using DrawingBoard.ViewModels;
using System.Windows;

namespace DrawingBoard
{
    internal class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<ShellViewModel>();
        }
    }
}
