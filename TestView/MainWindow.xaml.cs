using SharpDxf.Control;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestView
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var model = view.DataContext as SharpDxfViewModel;
            model.Load(@"C:\Users\SHZBG\Desktop\3DVisual\ducky.obj");
        }

        private void view_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
