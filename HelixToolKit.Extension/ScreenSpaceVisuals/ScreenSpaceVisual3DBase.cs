﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace HelixToolKit.Extension
{
    public abstract class ScreenSpaceVisual3DBase : RenderingModelVisual3D
    {
        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(ScreenSpaceVisual3DBase), new UIPropertyMetadata(Colors.Black, ColorChanged));

        /// <summary>
        /// Identifies the <see cref="DepthOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DepthOffsetProperty = DependencyProperty.Register(
            "DepthOffset", typeof(double), typeof(ScreenSpaceVisual3DBase), new UIPropertyMetadata(0.0, GeometryChanged));

        /// <summary>
        /// The is rendering flag.
        /// </summary>
        private bool isRendering;

    
        /// <summary>
        /// Initializes a new instance of the <see cref = "ScreenSpaceVisual3D" /> class.
        /// </summary>
        protected ScreenSpaceVisual3DBase()
        {
            this.Mesh = new MeshGeometry3D();
            this.Model = new GeometryModel3D { Geometry = this.Mesh };
            this.Content = this.Model;
            this.ColorChanged();
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color
        {
            get
            {
                return (Color)this.GetValue(ColorProperty);
            }

            set
            {
                this.SetValue(ColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the depth offset.
        /// A small positive number (0.0001) will move the visual slightly in front of other objects.
        /// </summary>
        /// <value>
        /// The depth offset.
        /// </value>
        public double DepthOffset
        {
            get
            {
                return (double)this.GetValue(DepthOffsetProperty);
            }

            set
            {
                this.SetValue(DepthOffsetProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is being rendered.
        /// When the visual is removed from the visual tree, this property should be set to false.
        /// </summary>
        public bool IsRendering
        {
            get
            {
                return this.isRendering;
            }

            set
            {
                if (value != this.isRendering)
                {
                    this.isRendering = value;
                    if (this.isRendering)
                    {
                        this.SubscribeToRenderingEvent();
                    }
                    else
                    {
                        this.UnsubscribeRenderingEvent();
                    }
                }
            }
        }
        /// <summary>
        /// Gets or sets the clipping object.
        /// </summary>
        protected CohenSutherlandClipping Clipping { get; set; }

        /// <summary>
        /// Gets or sets the mesh.
        /// </summary>
        protected MeshGeometry3D Mesh { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        protected GeometryModel3D Model { get; set; }

        /// <summary>
        /// Called when geometry properties have changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        protected static void GeometryChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((ScreenSpaceVisual3DBase)sender).UpdateGeometry();
        }

        /// <summary>
        /// The composition target_ rendering.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnCompositionTargetRendering(object sender, RenderingEventArgs e)
        {
            if (this.isRendering)
            {
                if (!Visual3DHelper.IsAttachedToViewport3D(this))
                {
                    return;
                }

                if (this.UpdateTransforms())
                {
                    this.UpdateClipping();
                    this.UpdateGeometry();
                }
            }
        }

        /// <summary>
        /// Called when the parent of the 3-D visual object is changed.
        /// </summary>
        /// <param name="oldParent">
        /// A value of type <see cref="T:System.Windows.DependencyObject"/> that represents the previous parent of the <see cref="T:System.Windows.Media.Media3D.Visual3D"/> object. If the <see cref="T:System.Windows.Media.Media3D.Visual3D"/> object did not have a previous parent, the value of the parameter is null.
        /// </param>
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            var parent = VisualTreeHelper.GetParent(this);
            this.IsRendering = parent != null;
        }

        /// <summary>
        /// Updates the geometry.
        /// </summary>
        protected abstract void UpdateGeometry();

        /// <summary>
        /// Updates the transforms.
        /// </summary>
        /// <returns>
        /// True if the transform is updated.
        /// </returns>
        protected abstract bool UpdateTransforms();

        /// <summary>
        /// Changes the material when the color changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private static void ColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((ScreenSpaceVisual3DBase)sender).ColorChanged();
        }

        /// <summary>
        /// Handles changes in the <see cref="Points" /> collection.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void HandlePointsChanged(object sender, EventArgs e)
        {
            this.UpdateGeometry();
        }

        /// <summary>
        /// Changes the material when the color changed.
        /// </summary>
        private void ColorChanged()
        {
            var mg = new MaterialGroup();
            mg.Children.Add(new DiffuseMaterial(Brushes.Black));
            mg.Children.Add(new EmissiveMaterial(new SolidColorBrush(this.Color)));
            mg.Freeze();
            this.Model.Material = mg;
        }

        /// <summary>
        /// Updates the clipping object.
        /// </summary>
        private void UpdateClipping()
        {
            var vp = this.GetViewport3D();
            if (vp == null)
            {
                return;
            }

            this.Clipping = new CohenSutherlandClipping(10, vp.ActualWidth - 20, 10, vp.ActualHeight - 20);
        }
    }
}
