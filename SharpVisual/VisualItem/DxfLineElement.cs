using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using HelixToolKit;
using HelixToolkit.Wpf;

namespace SharpDxf.Visual
{
    public class DxfLineElement : DxfVisualElement
    {
        public DxfLineElement()
        {
            var l = new LinesVisual3D();
            var ps = new Point3DCollection();
            ps.Add(new Point3D(0, 0, 0));
            ps.Add(new Point3D(50, 40, 0));
            l.Points = ps;
            this.Visual3DModel = l.Content;
        }
    }
}
