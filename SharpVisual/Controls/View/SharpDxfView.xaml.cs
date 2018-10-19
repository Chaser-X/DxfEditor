using System.Windows.Controls;
using System.Windows.Input;

namespace SharpDxf.Visual.Controls
{
    /// <summary>
    /// SharpDxfView.xaml 的交互逻辑
    /// </summary>
    public partial class SharpDxfView : UserControl
    {

        public SharpDxfViewModel ViewModel { get; }
        public HelixToolkit.Wpf.HelixViewport3D ViewPort
        {
            get
            {
                return this.canves;
            }
        }
        public SharpDxfView()
        {
            InitializeComponent();
            ViewModel = new SharpDxfViewModel(this.canves);
            DataContext = ViewModel;
        }

       
        
    }
}
