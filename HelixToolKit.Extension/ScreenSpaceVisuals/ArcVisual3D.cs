using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace HelixToolKit.Extension
{
    public class ArcVisual3D : ScreenSpaceVisual3DBase
    {
        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(double), typeof(ArcVisual3D), new UIPropertyMetadata(1.0,GeometryChanged));

        public static readonly DependencyProperty CenterProperty =
             DependencyProperty.Register("Center", typeof(Point3D), typeof(ArcVisual3D), new UIPropertyMetadata(new Point3D(0,0,0), GeometryChanged));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(ArcVisual3D), new UIPropertyMetadata(1.0, GeometryChanged));


        public static readonly DependencyProperty StartAngleProperty =
             DependencyProperty.Register("StartAngle", typeof(double), typeof(ArcVisual3D), new UIPropertyMetadata(0.0, GeometryChanged));

        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(ArcVisual3D), new UIPropertyMetadata(Math.PI, GeometryChanged));


        private ArcGeometryBuilder builder ;

        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        public Point3D Center
        {
            get { return (Point3D)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }


        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        public ArcVisual3D()
        {
            builder = new ArcGeometryBuilder(this);
        }

      

        protected override void UpdateGeometry()
        {
            this.Mesh.Positions = this.builder.CreatePositions(this.Center,this.Radius , this.StartAngle,
                this.EndAngle, 32, this.Thickness);
            var nn = this.Mesh.Positions.Count;
            if (this.Mesh.TriangleIndices.Count != nn * 3)
            {
                this.Mesh.TriangleIndices = this.builder.CreateIndices(nn);
            }
        }

        protected override bool UpdateTransforms()
        {
            return this.builder.UpdateTransforms();
        }
    }
}
