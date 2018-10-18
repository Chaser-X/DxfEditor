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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            var vm = new SharpDxfViewModel(this.canves);
            DataContext = vm;
            this.canves.InputBindings.Add(new MouseBinding(vm.SelectionCommand, new MouseGesture(MouseAction.LeftClick)));
        }

        public SharpDxfViewModel ViewModel
        {
            get {
                return this.DataContext as SharpDxfViewModel;
            }
        }
    }
}
