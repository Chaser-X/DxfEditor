using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDxf.Entities;
using PropertyTools.DataAnnotations;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Windows.Media;
using System.Windows;
using HelixToolKit.Extension;

namespace SharpDxf.Visual
{
    [Serializable]
    public class DxfCircleElement : DxfVisualElement
    {
        // Using a DependencyProperty as the backing store for CenterPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterPointProperty =
            DependencyProperty.Register("CenterPoint", typeof(Point3D), typeof(DxfCircleElement), new UIPropertyMetadata(new Point3D(0, 0, 0), (s, e) => ((DxfCircleElement)s).updateElement()));

        // Using a DependencyProperty as the backing store for EndPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(DxfCircleElement), new UIPropertyMetadata(10.0, (s, e) => ((DxfCircleElement)s).updateElement()));

        [Category("Special")]
        [Browsable(true)]
        [FormatString("0.000")]
        public Point3D CenterPoint
        {
            get { return (Point3D)GetValue(CenterPointProperty); }
            set { SetValue(CenterPointProperty, value); }
        }

        [Browsable(true)]
        [FormatString("0.000")]
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        private ArcVisual3D circleVisual = new ArcVisual3D() { StartAngle = 0, EndAngle = Math.PI * 2, ThetaDiv = 64 };
        private Circle dxfcircle = new Circle();

        public DxfCircleElement()
        {
            //active handle number
            numberofHandle = 2;
            activeHandle = -1;

            circleVisual.Center = CenterPoint;
            circleVisual.Radius = Radius;
            circleVisual.Thickness = 0.2;

            this.Content = circleVisual.Content;
            Color = Colors.Black;
        }
        public DxfCircleElement(SharpDxf.Entities.Circle circle)
        {
            numberofHandle = 2;
            activeHandle = -1;
            dxfcircle = circle;

            CenterPoint = new Point3D(dxfcircle.Center.X, dxfcircle.Center.Y, dxfcircle.Center.Z);
            Radius = dxfcircle.Radius;

            circleVisual.Center = CenterPoint;
            circleVisual.Radius = Radius;
            circleVisual.Thickness = 0.2;

            this.Content = circleVisual.Content;
            this.Color = circle.Color.ToMediaColor();
        }

        protected override void updateElement()
        {
            circleVisual.Center = CenterPoint;
            circleVisual.Radius = Radius;

            base.updateElement();
        }
        protected override void drawActiveHandle()
        {
            var ps = new Point3DCollection();
            ps.Add(CenterPoint);
            ps.Add(new Point3D(CenterPoint.X + Radius, CenterPoint.Y, CenterPoint.Z));

            this.Children.Clear();
            this.Children.Add(new PointsVisual3D() { Points = ps, Size = circleVisual.Thickness * 20, Color = Colors.Red });
        }
        protected override void clearActiveHandle()
        {
            this.activeHandle = -2;
            this.Children.Clear();
        }
        public override void UpdateActiveHandle(Point3D currentPos)
        {
            var distance = new double[numberofHandle];
            distance[0] = currentPos.DistanceTo(CenterPoint);
            distance[1] = currentPos.DistanceTo(new Point3D(CenterPoint.X + Radius, CenterPoint.Y, CenterPoint.Z));

            activeHandle = distance[0] >= distance[1] ? 1 : 0;
            if (distance[activeHandle] > 5)
                activeHandle = -1;
        }
        public override void moveByHandle(Point3D offset)
        {
            switch (activeHandle)
            {
                case -1:
                    {
                        CenterPoint = (CenterPoint.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        break;
                    }
                case 0:
                    {
                        CenterPoint = (CenterPoint.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        break;
                    }
                case 1:
                    {
                        var point = new Point3D(CenterPoint.X + Radius, CenterPoint.Y, CenterPoint.Z);
                        point = (point.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        Radius = point.DistanceTo(CenterPoint);
                        break;
                    }
                default: break;
            }
        }
        public override IEntityObject ToDxfEntity()
        {
            dxfcircle.Center = new Vector3f((float)CenterPoint.X, (float)CenterPoint.Y, (float)CenterPoint.Z);
            dxfcircle.Radius = (float)Radius;
            dxfcircle.Color = new AciColor(Color);
            return dxfcircle;
        }
        public override object Clone()
        {
            return new DxfCircleElement()
            {
                CenterPoint = this.CenterPoint,
                Radius = this.Radius,
                IsSelected = this.IsSelected,
                Color = this.Color,
                SelectedColor = this.SelectedColor
            };
        }
    }
}
