using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDxf.Entities;
using System.Windows;
using PropertyTools.DataAnnotations;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Windows.Media;

namespace SharpDxf.Visual
{
    [Serializable]
    public class DxfPointElement : DxfVisualElement
    {
        // Using a DependencyProperty as the backing store for StartPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointProperty =
            DependencyProperty.Register("Point", typeof(Point3D), typeof(DxfPointElement), new UIPropertyMetadata(new Point3D(0, 0, 0), (s, e) => ((DxfPointElement)s).updateElement()));

        // Using a DependencyProperty as the backing store for EndAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(DxfPointElement), new UIPropertyMetadata(5.0, (s, e) => ((DxfPointElement)s).updateElement()));


        [Category("Special")]
        [Browsable(true)]
        [FormatString("0.000")]
        public Point3D Point
        {
            get { return (Point3D)GetValue(PointProperty); }
            set { SetValue(PointProperty, value); }
        }

        [Browsable(true)]
        [FormatString("0.000")]
        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        private PointsVisual3D pointVisual = new PointsVisual3D();
        private Entities.Point dxfpoint = new Entities.Point();

        public DxfPointElement()
        {
            //active handle number
            numberofHandle = 1;
            activeHandle = -1;

            pointVisual.Points.Clear();
            pointVisual.Points.Add(Point);
            pointVisual.Size = Size;

            this.Content = pointVisual.Content;
            Color = Colors.Black;
        }
        public DxfPointElement(SharpDxf.Entities.Point point)
        {
            numberofHandle = 1;
            activeHandle = -1;
            dxfpoint = point;

            Point = new Point3D(dxfpoint.Location.X, dxfpoint.Location.Y, dxfpoint.Location.Z);
            this.Size = dxfpoint.Thickness;

            pointVisual.Points.Clear();
            pointVisual.Points.Add(Point);
            pointVisual.Size = dxfpoint.Thickness;
            this.Content = pointVisual.Content;

            this.Color = dxfpoint.Color.ToMediaColor();
        }

        protected override void updateElement()
        {
            pointVisual.Points.Clear();
            pointVisual.Points.Add(Point);
            pointVisual.Size = Size;

            base.updateElement();
        }
        protected override void drawActiveHandle()
        {
            //this.Children.Clear();
            //this.Children.Add(new PointsVisual3D() { Points = lineVisual.Points, Size = lineVisual.Thickness * 20, Color = Colors.Red });
        }
        protected override void clearActiveHandle()
        {
            this.activeHandle = -2;
            //this.Children.Clear();
        }
        public override void UpdateActiveHandle(Point3D currentPos)
        {
            var distance = new double[numberofHandle];
            distance[0] = currentPos.DistanceTo(Point);
            if (distance[0] < 3)
                activeHandle = -1;
        }
        public override void moveByHandle(Point3D offset)
        {
            switch (activeHandle)
            {
                case -1:
                    {
                        Point = (Point.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        break;
                    }
                default: break;
            }
        }
        public override IEntityObject ToDxfEntity()
        {
            dxfpoint.Location = new Vector3f((float)Point.X, (float)Point.Y, (float)Point.Z);
            dxfpoint.Thickness = (float)this.Size;
            dxfpoint.Color = new AciColor(Color);
            return dxfpoint;
        }
        public override object Clone()
        {
            return new DxfPointElement()
            {
                Point = this.Point,
                Size = this.Size,
                IsSelected = this.IsSelected,
                Color = this.Color,
                SelectedColor = this.SelectedColor

            };
        }
    }
}
