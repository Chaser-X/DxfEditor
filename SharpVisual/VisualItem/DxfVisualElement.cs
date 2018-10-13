using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SharpDxf.Visual
{
    public interface IDxfVisualElement
    {
        bool IsSelected { get; set; }
        Color SelectedColor { get; set; }
        Color Color { get; set; }
        Dictionary<string,Point3D> ControlPoint { get; }
    }

    public class DxfVisualElement : UIElement3D ,IDxfVisualElement
    {
        public bool IsSelected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color SelectedColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color Color { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dictionary<string, Point3D> ControlPoint => throw new NotImplementedException();
    }
}
