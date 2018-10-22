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
    public class DxfTextElement : DxfVisualElement
    {
        // Using a DependencyProperty as the backing store for StartPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(String), typeof(DxfTextElement), new UIPropertyMetadata("New Text", (s, e) => ((DxfTextElement)s).updateElement()));

        // Using a DependencyProperty as the backing store for StartPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(Point3D), typeof(DxfTextElement), new UIPropertyMetadata(new Point3D(0, 0, 0), (s, e) => ((DxfTextElement)s).updateElement()));

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(DxfTextElement), new UIPropertyMetadata(10.0, (s, e) => ((DxfTextElement)s).updateElement()));

        [Category("Special")]
        [Browsable(true)]
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        [Browsable(true)]
        [FormatString("0.000")]
        public Point3D Location
        {
            get { return (Point3D)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        [Browsable(true)]
        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        private BillboardTextVisual3D textVisual = new BillboardTextVisual3D();
        private PointsVisual3D pointVisual = new PointsVisual3D() { Size= 10 };

        private Entities.Text dxfText = new Entities.Text();

        public DxfTextElement()
        {
            //active handle number
            numberofHandle = 1;
            activeHandle = -1;

            var p = pointVisual.Content as GeometryModel3D;
            p.Material = new DiffuseMaterial(Brushes.Transparent);
            p.BackMaterial = new DiffuseMaterial(Brushes.Transparent);

            pointVisual.Points.Add(Location);
            textVisual.Position = Location;
            textVisual.Text = Text;
            textVisual.FontSize = Size;

            this.Content = pointVisual.Content;
            this.Children.Add(textVisual);
            Color = Colors.Black;
        }
        public DxfTextElement(SharpDxf.Entities.Text text)
        {
            numberofHandle = 1;
            activeHandle = -1;
            dxfText = text;

            var p = pointVisual.Content as GeometryModel3D;
            p.Material = new DiffuseMaterial(Brushes.Transparent);
            p.BackMaterial = new DiffuseMaterial(Brushes.Transparent);

            pointVisual.Points.Add(Location);
            textVisual.Position = Location;
            textVisual.Text = Text;
            textVisual.FontSize = Size;
             
            Location = new Point3D(text.BasePoint.X, text.BasePoint.Y, text.BasePoint.Z);
            Size = text.Height;
            Text = text.Value;

            this.Content = pointVisual.Content;
            this.Children.Add(textVisual);
            this.Color = dxfText.Color.ToMediaColor();
        }

        protected override void updateElement()
        {
            textVisual.Position = Location;
            textVisual.Text = Text;
            textVisual.FontSize = Size;

            pointVisual.Points.Clear();
            pointVisual.Points.Add(Location);

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
            distance[0] = currentPos.DistanceTo(Location);
            if (distance[0] < 20)
                activeHandle = -1;
        }
        public override void moveByHandle(Point3D offset)
        {
            switch (activeHandle)
            {
                case -1:
                    {
                        Location = (Location.ToVector3D() + offset.ToVector3D()).ToPoint3D();
                        break;
                    }
                default: break;
            }
        }
        public override IEntityObject ToDxfEntity()
        {
            dxfText.BasePoint = new Vector3f((float)Location.X, (float)Location.Y, (float)Location.Z);
            dxfText.Value = this.Text;
            dxfText.Height = (float)this.Size;
            dxfText.Color = new AciColor(Color);
            return dxfText;
        }

        protected override void colorChanged(DependencyPropertyChangedEventArgs e)
        {
            var model = this.Content as GeometryModel3D;
            

            textVisual.Foreground = new SolidColorBrush((Color)e.NewValue);
            textVisual.FontWeight = FontWeights.Normal;
            textVisual.Background = Brushes.Transparent;

            //model.BackMaterial = new DiffuseMaterial(Brushes.Transparent);
            //model.Material = new DiffuseMaterial(Brushes.Transparent);
        }
        public override object Clone()
        {
            return new DxfTextElement()
            {
                Location = this.Location,
                Text = this.Text,
                Size = this.Size,
                IsSelected = this.IsSelected,
                Color = this.Color,
                SelectedColor = this.SelectedColor

            };
        }
    }
}
