using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace HelixToolKit.Extension
{
    public class XXXVisual3D : UIElement3D
    {
        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ColorProperty =
        //    DependencyProperty.Register("ColorProperty", typeof(Brush), typeof(XXXVisual3D), new UIPropertyMetadata(Brushes.Black, colorChanged));

        //public Brush Color
        //{
        //    get { return (Brush)GetValue(ColorProperty); }
        //    set { SetValue(ColorProperty, value); }
        //}

        private BoundingBoxVisual3D box = new BoundingBoxVisual3D();
        private Model3DGroup model = new Model3DGroup();
        public XXXVisual3D()
        {
            this.Visual3DModel = model;
            var s = new SphereVisual3D()
            {
                Radius = 20,
                Fill = Brushes.Red,
                Center = new Point3D(0, 0, 0)
            };
            box.BoundingBox = s.FindBounds(Transform3D.Identity);
            box.Diameter = 0.2;
           
            model.Children.Add(s.Content);
        }

        //private static void colorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (e.NewValue != e.OldValue)
        //        ((XXXVisual3D)d).colorChanged();

        //}

        //private void colorChanged()
        //{
        //    model.Material = MaterialHelper.CreateMaterial(Color);
        //    model.BackMaterial = MaterialHelper.CreateMaterial(Color);
        //}

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.GetViewport().Children.Add(box);
            }
        }
    }
}
