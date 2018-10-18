using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SharpDxf.Visual.Controls
{
    public class Manipulation3DEventArgs : EventArgs
    {
        /// <summary>
        /// 鼠标在3D空间中的当前坐标位置
        /// </summary>
        public Point3D CurrentPosition { get; set; }
        public Manipulation3DEventArgs(Point3D point)
        {
            CurrentPosition = point;
        }
    }

    /// <summary>
    /// Provides event data for the VisualSelected event of the <see cref="DxfViusalElementSelectionCommand"/>
    /// </summary>
    public class VisualSelectedEventArgs : EventArgs
    {
        public Visual3D SelectedVisual { get; private set; }
        public VisualSelectedEventArgs(Visual3D visual)
        {
            SelectedVisual = visual;
        }
    }


    public class DxfViusalElementSelectionCommand : ICommand
    {
        /// <summary>
        /// The viewport of the command.
        /// </summary>
        protected readonly HelixViewport3D Viewport;

        /// <summary>
        /// Keeps track of the old cursor.
        /// </summary>
        private Cursor oldCursor;


        /// <summary>
        /// Keeps track of the old cursor.
        /// </summary>
        private DxfVisualElement selectedElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionCommand"/> class.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <param name="eventHandlerModels">The selection event handler for models.</param>
        /// <param name="eventHandlerVisuals">The selection event handler for visuals.</param>
        public DxfViusalElementSelectionCommand(HelixViewport3D viewport, EventHandler<VisualSelectedEventArgs> eventHandlerVisuals)
        {
            this.Viewport = viewport;
            this.VisualsSelected = eventHandlerVisuals;
        }

        /// <summary>
        /// Occurs when <see cref="CanExecute" /> is changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Occurs when visuals are selected.
        /// </summary>
        private event EventHandler<VisualSelectedEventArgs> VisualsSelected;

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
            this.OnMouseDown(this.Viewport);
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
            if (selectedElement != null)
            {
                selectedElement.IsSelected = false;
            }
            var selectionHits = this.Viewport.Viewport.FindHits(Mouse.GetPosition(this.Viewport));
            if (selectionHits != null)
            {
                var visuals = selectionHits.Where(x => x.Visual is DxfVisualElement).Select(x => x.Visual);
                selectedElement = visuals.FirstOrDefault() as DxfVisualElement;
                this.OnVisualsSelected(new VisualSelectedEventArgs(selectedElement));
                if (selectedElement != null)
                {
                    selectedElement.IsSelected = true;
                    selectedElement.UpdateActiveHandle(e.CurrentPosition);
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
            selectedElement?.moveByHandle(offset.ToPoint3D());
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
        /// Raises the <see cref="E:VisualsSelected" /> event.
        /// </summary>
        /// <param name="e">The <see cref="VisualSelectedEventArgs"/> instance containing the event data.</param>
        protected void OnVisualsSelected(VisualSelectedEventArgs e)
        {
            var handler = this.VisualsSelected;
            if (handler != null)
            {
                handler(this.Viewport, e);
            }
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
        protected void OnMouseDown(object sender)
        {
            this.Viewport.Focus();
            this.Viewport.CaptureMouse();

            this.Started(new Manipulation3DEventArgs(this.Viewport.CursorPosition.Value));
            if (selectedElement == null)
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

        /// <summary>
        /// Called when the condition of execution is changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected void OnCanExecutedChanged(object sender, EventArgs e)
        {
            var handler = this.CanExecuteChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }


    }
}
