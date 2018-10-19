using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharpDxf.Visual.Controls
{
    public class DxfVisualElementDeletCommand : ICommand
    {
        private SharpDxfViewModel viewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DxfVisualElementDeletCommand(SharpDxfViewModel model)
        {
            viewModel = model;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.Subject.SelectedObject != null;
        }

        public void Execute(object parameter)
        {
            viewModel.Subject.EntityObjects.Remove(viewModel.Subject.SelectedObject);
            viewModel.Subject.SelectedObject = null;
        }
    }
}
