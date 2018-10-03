using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HelixToolkit.Wpf;
/*--------------------------------------------*/
/*A Spline 3D Visual Extension Type for HelixToolKit */
/*------------------------------------------*/
namespace HelixToolKit.Extension
{
    public class SplineVisual3D : ScreenSpaceVisual3D
    {
        /// <summary>
        /// Identifies the <see cref="Thickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
            "Thickness", typeof(double), typeof(SplineVisual3D), new UIPropertyMetadata(1.0, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="IsClosed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(
            "IsClosed", typeof(bool), typeof(SplineVisual3D), new UIPropertyMetadata(false, GeometryChanged));

        /// <summary>
        /// The builder.
        /// </summary>
        private readonly SplineGeometryBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref = "SplineVisual3D" /> class.
        /// </summary>
        public SplineVisual3D()
        {
            this.builder = new SplineGeometryBuilder(this);
        }

        /// <summary>
        /// Gets or sets the thickness of the lines.
        /// </summary>
        /// <value>
        /// The thickness.
        /// </value>
        public double Thickness
        {
            get
            {
                return (double)this.GetValue(ThicknessProperty);
            }

            set
            {
                this.SetValue(ThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets if the spline is Closed .
        /// </summary>
        /// <value>
        /// The IsClosed.
        /// </value>
        public bool IsClosed
        {
            get
            {
                return (bool)this.GetValue(IsClosedProperty);
            }

            set
            {
                this.SetValue(IsClosedProperty, value);
            }
        }

        /// <summary>
        /// Updates the geometry.
        /// </summary>
        protected override void UpdateGeometry()
        {
            if (this.Points == null)
            {
                this.Mesh.Positions = null;
                return;
            }

            int n = this.Points.Count;
            if (n > 0)
            {
                this.Mesh.Positions = this.builder.CreatePositions(this.Points, this.Thickness, this.DepthOffset, null,
                    0.5, 0.25, this.IsClosed);
                var nn = this.Mesh.Positions.Count;
                if (this.Mesh.TriangleIndices.Count != nn * 3)
                {
                    this.Mesh.TriangleIndices = this.builder.CreateIndices(nn);
                }
            }
            else
            {
                this.Mesh.Positions = null;
            }
        }

        /// <summary>
        /// Updates the transforms.
        /// </summary>
        /// <returns>
        /// True if the transform is updated.
        /// </returns>
        protected override bool UpdateTransforms()
        {
            return this.builder.UpdateTransforms();
        }
    }
}
