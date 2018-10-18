using System.Windows.Controls;
using System.Windows.Input;

namespace SharpDxf.Visual.Controls
{
    /// <summary>
    /// SharpDxfView.xaml 的交互逻辑
    /// </summary>
    public partial class SharpDxfView : UserControl
    {
        public SharpDxfView()
        {
            InitializeComponent();
            DxfEngine = new SharpDxfEngine(this.canves);
        }
        public SharpDxfEngine DxfEngine { get; set; }
        public HelixToolkit.Wpf.HelixViewport3D ViewPort
        {
            get {
                return this.canves;
            }
        }
    }
}
