using System;
using System.Windows;
using System.Linq;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Windows.Media;
using PropertyTools.DataAnnotations;
using HelixToolKit.Extension;
using SharpDxf.Entities;

namespace SharpDxf.Visual
{
    public class DxfArcElement : DxfVisualElement
    {
        // Using a DependencyProperty as the backing store for CenterPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterPointProperty =
            DependencyProperty.Register("CenterPoint", typeof(Point3D), typeof(DxfArcElement), new UIPropertyMetadata(new Point3D(0, 0, 0), (s, e) => ((DxfArcElement)s).updateElement()));

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(DxfArcElement), new UIPropertyMetadata(10.0, (s, e) => ((DxfArcElement)s).updateElement()));

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(DxfArcElement), new UIPropertyMetadata(0.0, (s, e) => ((DxfArcElement)s).updateElement()));

        // Using a DependencyProperty as the backing store for EndAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(DxfArcElement), new UIPropertyMetadata(Math.PI, (s, e) => ((DxfArcElement)s).updateElement()));

        // Using a DependencyProperty as the backing store for EndAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectProperty =
            DependencyProperty.Register("Direct", typeof(bool), typeof(DxfArcElement), new UIPropertyMetadata(true, (s, e) => ((DxfArcElement)s).updateElement()));

        [Category("Special")]
        [Browsable(true)]
        [FormatString("0.000")]
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        [Browsable(true)]
        [FormatString("0.000")]
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

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

        [Browsable(true)]
        [Description("获取或设置圆弧方向，为True时\"CW\"，为发False时\"CCW\"")]
        public bool Direct
        {
            get { return (bool)GetValue(DirectProperty); }
            set { SetValue(DirectProperty, value); }
        }

        private ArcVisual3D arcVisual = new ArcVisual3D() { StartAngle = 0, EndAngle = Math.PI * 2, ThetaDiv = 64 };
        private Arc dxfarc = new Arc();

        public DxfArcElement()
        {
            //active handle number
            numberofHandle = 3;
            activeHandle = -1;

            arcVisual.Center = CenterPoint;
            arcVisual.Radius = Radius;
            arcVisual.StartAngle = StartAngle;
            arcVisual.EndAngle = EndAngle;
            arcVisual.Direct = Direct;

            arcVisual.Thickness = 0.2;

            this.Content = arcVisual.Content;
            Color = Colors.Black;
        }
        public DxfArcElement(SharpDxf.Entities.Arc arc)
        {
            numberofHandle = 3;
            activeHandle = -1;
            dxfarc = arc;

            CenterPoint = new Point3D(dxfarc.Center.X, dxfarc.Center.Y, dxfarc.Center.Z);
            Radius = dxfarc.Radius;
            StartAngle = dxfarc.StartAngle * MathHelper.DegToRad;
            EndAngle = dxfarc.EndAngle * MathHelper.DegToRad;

            arcVisual.Center = CenterPoint;
            arcVisual.Radius = Radius;
            arcVisual.StartAngle = StartAngle;
            arcVisual.EndAngle = EndAngle;

            arcVisual.Thickness = 0.2;

            this.Content = arcVisual.Content;
            Color = dxfarc.Color.ToMediaColor();
        }
        protected override void updateElement()
        {
            arcVisual.Center = CenterPoint;
            arcVisual.Radius = Radius;
            arcVisual.StartAngle = StartAngle;
            arcVisual.EndAngle = EndAngle;
            arcVisual.Direct = Direct;

            base.updateElement();
        }
        protected override void drawActiveHandle()
        {
            var ps = new Point3DCollection();
            ps.Add(CenterPoint);
            ps.Add(new Point3D(Math.Cos(StartAngle) * Radius + CenterPoint.X, Math.Sin(StartAngle) * Radius + CenterPoint.Y, CenterPoint.Z));
            ps.Add(new Point3D(Math.Cos(EndAngle) * Radius + CenterPoint.X, Math.Sin(EndAngle) * Radius + CenterPoint.Y, CenterPoint.Z));
            if (Direct)
                ps.Add(new Point3D(Math.Cos((EndAngle + StartAngle) / 2) * Radius + CenterPoint.X, Math.Sin((EndAngle + StartAngle) / 2) * Radius + CenterPoint.Y, CenterPoint.Z));
            else
                ps.Add(new Point3D(Math.Cos((EndAngle + StartAngle) / 2 + Math.PI) * Radius + CenterPoint.X, Math.Sin((EndAngle + StartAngle) / 2 + Math.PI) * Radius + CenterPoint.Y, CenterPoint.Z));

            this.Children.Clear();
            this.Children.Add(new PointsVisual3D() { Points = ps, Size = arcVisual.Thickness * 20, Color = Colors.Red });
        }
        protected override void clearActiveHandle()
        {
            this.activeHandle = -2;
            this.Children.Clear();
        }
        public override void UpdateActiveHandle(Point3D currentPos)
        {
            var ps = this.Children[0] as PointsVisual3D;
            var distance = new double[numberofHandle];
            distance[0] = currentPos.DistanceTo(ps.Points[1]);//起点
            distance[1] = currentPos.DistanceTo(ps.Points[2]);//终点
            distance[2] = currentPos.DistanceTo(ps.Points[3]);//中点

            var min = distance[0];
            for (int i = 0; i < numberofHandle; i++)
            {
                if (distance[i] <= min)
                {
                    min = distance[i];
                    activeHandle = i;
                }
            }
            if (min > 5)
                activeHandle = -1;
        }
        public override void moveByHandle(Point3D offset)
        {
            var sp = new Point3D(Math.Cos(StartAngle) * Radius + CenterPoint.X, Math.Sin(StartAngle) * Radius + CenterPoint.Y, CenterPoint.Z);
            var ep = new Point3D(Math.Cos(EndAngle) * Radius + CenterPoint.X, Math.Sin(EndAngle) * Radius + CenterPoint.Y, CenterPoint.Z);
            var mp = new Point3D(Math.Cos((EndAngle + StartAngle) / 2) * Radius + CenterPoint.X, Math.Sin((EndAngle + StartAngle) / 2) * Radius + CenterPoint.Y, CenterPoint.Z);

            switch (activeHandle)
            {
                case -1://move
                    {
                        CenterPoint = (CenterPoint.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        break;
                    }
                case 0://起点
                    {
                        // CenterPoint = (CenterPoint.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        var newsp = (sp.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        var detAngle = Math.Atan((newsp.Y - CenterPoint.Y) / (newsp.X - CenterPoint.X)) -
                             Math.Atan((sp.Y - CenterPoint.Y) / (sp.X - CenterPoint.X));
                        StartAngle += detAngle;
                        break;
                    }
                case 1://终点
                    {
                        var newep = (ep.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        var detAngle = Math.Atan((newep.Y - CenterPoint.Y) / (newep.X - CenterPoint.X)) -
                             Math.Atan((ep.Y - CenterPoint.Y) / (ep.X - CenterPoint.X));
                        EndAngle += detAngle;
                        break;
                    }
                case 2://中点
                    {
                        mp = (mp.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        Radius = mp.DistanceTo(CenterPoint);
                        break;
                    }
                default: break;
            }
        }
        public override IEntityObject ToDxfEntity()
        {
            dxfarc.Center = new Vector3f((float)CenterPoint.X, (float)CenterPoint.Y, (float)CenterPoint.Z);
            dxfarc.Radius = (float)Radius;
            dxfarc.StartAngle = (float)(StartAngle * MathHelper.RadToDeg);
            dxfarc.EndAngle = (float)(EndAngle * MathHelper.RadToDeg);
            dxfarc.Color = new AciColor(Color);
            return dxfarc;
        }
        public override object Clone()
        {
            return new DxfArcElement()
            {
                CenterPoint = this.CenterPoint,
                Radius = this.Radius,
                StartAngle = this.StartAngle,
                EndAngle = this.EndAngle,
                Direct = this.Direct,
                IsSelected = this.IsSelected,
                Color = this.Color,
                SelectedColor = this.SelectedColor
            };
        }
    }
}
