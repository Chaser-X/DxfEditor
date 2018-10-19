using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharpDxf.Visual.Controls
{
    public class FitViewCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private HelixToolkit.Wpf.HelixViewport3D viewPort;

        public FitViewCommand()
        {
           
        }



        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewPort = parameter as HelixToolkit.Wpf.HelixViewport3D;
            viewPort.ResetCamera();
        }
    }
}
