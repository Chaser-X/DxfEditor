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


using HelixToolkit.Wpf;
using HelixToolKit.Extension;

namespace HelixToolKitTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #region spline
            var ps = new Point3DCollection();
            ps.Add(new Point3D(0, 0, 0));
            ps.Add(new Point3D(10, 0, 0));
            ps.Add(new Point3D(10, 10, 0));
            ps.Add(new Point3D(0, 10, 0));
            var spline = new SplineVisual3D();
            spline.Points = ps;
            spline.Thickness = 1;
            spline.Color = Colors.Green;
            spline.IsClosed = false;
            #endregion

            #region arc
            var arc1 = new ArcVisual3D();
            arc1.StartAngle = 0;
            arc1.EndAngle = Math.PI / 2;
            arc1.Radius = 20;
            arc1.Color = Colors.Red;
            arc1.Thickness = 2;

            var arc2 = new ArcVisual3D();
            arc2.Center = new Point3D(10, 10, 10);
            arc2.StartAngle = 0;
            arc2.EndAngle = Math.PI * 2;
            arc2.Radius = 20;
            arc2.Color = Colors.Red;
            arc2.Thickness = 2;
            #endregion
            canves.Children.Add(spline);
            canves.Children.Add(arc1);
            canves.Children.Add(arc2);


        }
    }
}
