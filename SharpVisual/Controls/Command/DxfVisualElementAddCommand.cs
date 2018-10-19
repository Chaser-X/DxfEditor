using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace SharpDxf.Visual.Controls
{
    /// <summary>
    /// 可视化DXF图元种类
    /// </summary>
    public enum DxfVisualType
    {
        Point = 0,
        Line = 1,
        Arc = 2,
        Circle = 3,
        Other = 4
    }
    public class DxfVisualElementAddCommand : ICommand
    {
        private SharpDxfViewModel viewModel;
        private HelixToolkit.Wpf.HelixViewport3D Viewport;
        private DxfVisualType addType;
        private DxfVisualElement cacheVisual;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DxfVisualElementAddCommand(SharpDxfViewModel model)
        {
            viewModel = model;
            Viewport = model.ViewPort;
        }

        /// <summary>
        /// Keeps track of the old cursor.
        /// </summary>
        private Cursor oldCursor;

        /// <summary>
        /// Gets the mouse down point (2D screen coordinates).
        /// </summary>
        protected Point3D MouseDownPoint { get; private set; }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            if (!(parameter is DxfVisualType)) throw new NotSupportedException();

            addType = (DxfVisualType)parameter ;
      
            switch (addType)
            {
                case DxfVisualType.Line:
                    cacheVisual = new DxfLineElement();
                    break;
                default:break;
            }

            this.Viewport.MouseDown += this.OnMouseDown;

        }

        /// <summary>
        /// Checks whether the command can be executed.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// <c>true</c> if the command can be executed. Otherwise, it returns <c>false</c>.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Occurs when the manipulation is started.
        /// </summary>
        /// <param name="e">
        /// The <see cref="Manipulation3DEventArgs"/> instance containing the event data.
        /// </param>
        protected void Started(Manipulation3DEventArgs e)
        {
            this.MouseDownPoint = e.CurrentPosition;
            if (viewModel.Subject.SelectedObject != null)
            {
                viewModel.Subject.SelectedObject.IsSelected = false;
            }
            var selectionHits = this.Viewport.Viewport.FindHits(Mouse.GetPosition(this.Viewport));
            if (selectionHits != null)
            {
                var visuals = selectionHits.Where(x => x.Visual is DxfVisualElement).Select(x => x.Visual);
                viewModel.Subject.SelectedObject = visuals.FirstOrDefault() as DxfVisualElement;
                if (viewModel.Subject.SelectedObject != null)
                {
                    viewModel.Subject.SelectedObject.IsSelected = true;
                    viewModel.Subject.SelectedObject.UpdateActiveHandle(e.CurrentPosition);
                }
            }
        }

        /// <summary>
        /// Occurs when the position is changed during a manipulation.
        /// </summary>
        /// <param name="e">
        /// The <see cref="Manipulation3DEventArgs"/> instance containing the event data.
        /// </param>
        protected void Delta(Manipulation3DEventArgs e)
        {
            var offset = e.CurrentPosition - this.MouseDownPoint;
            this.MouseDownPoint = e.CurrentPosition;
            viewModel.Subject.SelectedObject?.moveByHandle(offset.ToPoint3D());
        }

        /// <summary>
        /// Occurs when the manipulation is completed.
        /// </summary>
        /// <param name="e">
        /// The <see cref="Manipulation3DEventArgs"/> instance containing the event data.
        /// </param>
        protected void Completed(Manipulation3DEventArgs e)
        {

        }

        /// <summary>
        /// Gets the cursor for the gesture.
        /// </summary>
        /// <returns>
        /// A cursor.
        /// </returns>
        protected Cursor GetCursor()
        {
            return Cursors.Pen;
        }

        /// <summary>
        /// Called when the mouse button is pressed down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        protected void OnMouseDown(object sender , MouseButtonEventArgs e)
        {
            this.Viewport.Focus();
            this.Viewport.CaptureMouse();

            this.Started(new Manipulation3DEventArgs(this.Viewport.CursorPosition.Value));
            if (viewModel.Subject.SelectedObject == null)
            {
                this.Viewport.ReleaseMouseCapture();
                return;
            }
            this.Viewport.MouseMove += this.OnMouseMove;
            this.Viewport.MouseUp += this.OnMouseUp;

            this.oldCursor = this.Viewport.Cursor;
            this.Viewport.Cursor = this.GetCursor();
        }

        /// <summary>
        /// Called when the mouse button is released.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Viewport.MouseMove -= this.OnMouseMove;
            this.Viewport.MouseUp -= this.OnMouseUp;
            this.Viewport.ReleaseMouseCapture();
            this.Viewport.Cursor = this.oldCursor;
            this.Completed(new Manipulation3DEventArgs(this.Viewport.CursorPosition.Value));
            e.Handled = true;
        }

        /// <summary>
        /// Called when the mouse is move on the control.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected void OnMouseMove(object sender, MouseEventArgs e)
        {
            this.Delta(new Manipulation3DEventArgs(this.Viewport.CursorPosition.Value));
            e.Handled = true;
        }
    }
}
