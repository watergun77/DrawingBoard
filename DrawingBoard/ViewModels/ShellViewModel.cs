using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.ViewModels
{
    internal class ShellViewModel
    {
        public ShellViewModel() { }
        public void AddAlgoBlock()
        {
            Debug.WriteLine("AddAlgoBlock");
        }
        public void RemoveAlgoBlock()
        {
            Debug.WriteLine("RemoveAlgoBlock");
        }
    }
}
