using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharpDxf.Visual.Controls
{
    public class SharpDxfViewModel : NotifyPropertyChangeBase
    {
        #region view operation
        
        private HelixToolkit.Wpf.HelixViewport3D viewPort;
        /// <summary>
        /// 视图显示窗口
        /// </summary>
        public HelixToolkit.Wpf.HelixViewport3D ViewPort
        {
            get
            {
                return viewPort;
            }
        }

        private bool is3DVisualize = false;
        public bool Is3DVisualize
        {
            get
            {
                return is3DVisualize;
            }
            set
            {
                viewPort.ResetCamera();
                viewPort.IsRotationEnabled = value;
                viewPort.ShowViewCube = value;
                SetProperty(ref is3DVisualize, value);
            }
        } 
        #endregion
        private DxfVisualElement addOject = null;
        /// <summary>
        /// 获取或设置待添加的DXF可视化对象
        /// </summary>
        public DxfVisualElement AddObject
        {
            get
            {
                return addOject;
            }
            set
            {
                SetProperty(ref addOject, value);
            }
        }
        internal bool EditMode
        {
            get
            {
                return addOject != null;
            }
        }
        /// <summary>
        /// DXF增删改功能引擎对象
        /// </summary>
        public SharpDxfEngine Subject { get; private set; }
        #region Commands
        public DxfViusalElementSelectionCommand SelectionCommand { get; private set; }
        public DxfVisualElementDeletCommand DeletCommand { get; private set; }
        #endregion

        public SharpDxfViewModel(HelixToolkit.Wpf.HelixViewport3D viewport)
        {
            viewPort = viewport;
            Is3DVisualize = false;
            Subject = new SharpDxfEngine();

            viewPort.Children.Add(Subject.ShowModel);

            //ViewPort上增删改的命令初始化
            SelectionCommand = new DxfViusalElementSelectionCommand(this);
            DeletCommand = new DxfVisualElementDeletCommand(this);
        }
    }
}
