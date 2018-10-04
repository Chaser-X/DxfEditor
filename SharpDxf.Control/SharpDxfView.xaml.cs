using HelixToolkit.Wpf;
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

namespace SharpDxf.Control
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class SharpDxfView : UserControl
    {
        public SharpDxfView()
        {
            InitializeComponent();
            this.DataContext = new SharpDxfViewModel();

        }

        private void canves_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(canves);
            PointHitTestParameters hitParams = new PointHitTestParameters(mousePos);
            VisualTreeHelper.HitTest(canves, null, ResultCallback, hitParams);
        }

        public HitTestResultBehavior ResultCallback(HitTestResult result)
        {
  
            // Did we hit 3D?
            RayHitTestResult rayResult = result as RayHitTestResult;
            if (rayResult != null)
            {
                // Did we hit a MeshGeometry3D?
                RayMeshGeometry3DHitTestResult rayMeshResult =
                    rayResult as RayMeshGeometry3DHitTestResult;

                //Ignore 2d to 3d Visual Contanier 
                if (rayMeshResult.VisualHit is Viewport2DVisual3D)
                    return HitTestResultBehavior.Stop;

                if (rayMeshResult != null)
                {
                    var visual3d = rayMeshResult.VisualHit as ScreenSpaceVisual3D;
                    if (visual3d != null)
                    {

                        //visual3d.Color = Colors.Red;
                        //Shape target = visual2d.Visual as Shape;
                        //target.Stroke = Brushes.Yellow;
                    }
                    // Yes we did!
                }
            }

            //Did Hit 2D 
            var resut2d = result.VisualHit as Shape;
            if(resut2d != null)
            {
                resut2d.Stroke = Brushes.Yellow;
                return HitTestResultBehavior.Stop;
            }
            return HitTestResultBehavior.Continue;
        }


    }
}
