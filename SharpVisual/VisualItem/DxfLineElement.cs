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
using PropertyTools.DataAnnotations;
using System.Windows.Documents;
using SharpDxf.Entities;

namespace SharpDxf.Visual
{
    public class DxfLineElement : DxfVisualElement
    {
        // Using a DependencyProperty as the backing store for StartPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point3D), typeof(DxfLineElement), new UIPropertyMetadata(new Point3D(0, 0, 0), (s, e) => ((DxfLineElement)s).updateElement()));

        // Using a DependencyProperty as the backing store for EndPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(Point3D), typeof(DxfLineElement), new UIPropertyMetadata(new Point3D(10, 10, 0), (s, e) => ((DxfLineElement)s).updateElement()));

        [Category("Special")]
        [Browsable(true)]
        [FormatString("0.000")]
        public Point3D StartPoint
        {
            get { return (Point3D)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }
        [Browsable(true)]
        [FormatString("0.000")]
        public Point3D EndPoint
        {
            get { return (Point3D)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        private LinesVisual3D lineVisual = new LinesVisual3D();
        private Line dxfline = new Line();

        public DxfLineElement()
        {
            //active handle number
            numberofHandle = 2;
            activeHandle = -1;

            var ps = new Point3DCollection();
            ps.Add(StartPoint);
            ps.Add(EndPoint);
            lineVisual.Points = ps;
            lineVisual.Thickness = 0.2;
            this.Content = lineVisual.Content;
            this.Color = Colors.Black;
        }
        public DxfLineElement(SharpDxf.Entities.Line line)
        {
            numberofHandle = 2;
            activeHandle = -1;
            dxfline = line;

            StartPoint = new Point3D(dxfline.StartPoint.X, dxfline.StartPoint.Y, dxfline.StartPoint.Z);
            EndPoint = new Point3D(dxfline.EndPoint.X, dxfline.EndPoint.Y, dxfline.EndPoint.Z);

            lineVisual.Points.Add(StartPoint);
            lineVisual.Points.Add(EndPoint);
            lineVisual.Thickness = 0.2;

            this.Content = lineVisual.Content;
            this.Color = dxfline.Color.ToMediaColor();
        }

        protected override void updateElement()
        {
            var ps = new Point3DCollection();
            ps.Add(StartPoint);
            ps.Add(EndPoint);
            lineVisual.Points = ps;

            base.updateElement();
        }
        protected override void drawActiveHandle()
        {
            this.Children.Clear();
            this.Children.Add(new PointsVisual3D() { Points = lineVisual.Points, Size = lineVisual.Thickness * 20, Color = Colors.Red });
        }
        protected override void clearActiveHandle()
        {
            this.activeHandle = -2;
            this.Children.Clear();
        }
        public override void UpdateActiveHandle(Point3D currentPos)
        {
            var distance = new double[numberofHandle];
            distance[0] = currentPos.DistanceTo(StartPoint);
            distance[1] = currentPos.DistanceTo(EndPoint);

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
                        StartPoint = (StartPoint.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        EndPoint = (EndPoint.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        break;
                    }
                case 0:
                    {
                        StartPoint = (StartPoint.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        break;
                    }
                case 1:
                    {
                        EndPoint = (EndPoint.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        break;
                    }
                default: break;
            }
        }
        public override IEntityObject ToDxfEntity()
        {
            dxfline.StartPoint = new Vector3f((float)StartPoint.X, (float)StartPoint.Y, (float)StartPoint.Z);
            dxfline.EndPoint = new Vector3f((float)EndPoint.X, (float)EndPoint.Y, (float)EndPoint.Z);
            dxfline.Color = new AciColor(Color);
            return dxfline;
        }
        public override object Clone()
        {
            return new DxfLineElement()
            {
                StartPoint = this.StartPoint,
                EndPoint = this.EndPoint,
                IsSelected = this.IsSelected,
                Color = this.Color,
                SelectedColor = this.SelectedColor

            };
        }
    }
}
