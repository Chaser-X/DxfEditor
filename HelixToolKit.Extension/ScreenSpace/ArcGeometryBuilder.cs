using System;
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
    public class ArcGeometryBuilder : ScreenGeometryBuilder
    {
        public ArcGeometryBuilder(Visual3D visual) : base(visual)
        {
        }

        public Int32Collection CreateIndices(int n)
        {
            var indices = new Int32Collection(n * 3);

            for (int i = 0; i < n / 2; i++)
            {
                var i4 = i * 4;
                indices.Add(i4 + 2);
                indices.Add(i4 + 1);
                indices.Add(i4 + 0);

                indices.Add(i4 + 2);
                indices.Add(i4 + 3);
                indices.Add(i4 + 1);
            }

            indices.Freeze();
            return indices;
        }

        public Point3DCollection CreatePositions(
            Point3D center,
            double radius,
            double startAngle,
            double endAngle,
            double thetadiv = 32,
            double thickness = 1.0,
            double depthOffset = 0.0,
            CohenSutherlandClipping clipping = null
            )
        {
            var halfThickness = thickness * 0.5;
            var spoints = new Point3DCollection();
            for (int i = 0; i <= thetadiv; i++)
            {
                double x = radius * Math.Sin(i * (endAngle - startAngle) / thetadiv + startAngle) + center.X;
                double y = radius * Math.Cos(i * (endAngle - startAngle) / thetadiv + startAngle) + center.Y;
                spoints.Add(new Point3D(x, y, center.Z));
            }


            var segmentCount = spoints.Count - 1;

            var positions = new Point3DCollection(segmentCount * 4);

            for (int i = 0; i < segmentCount; i++)
            {
                //   int startIndex = i * 2;

                var startPoint = spoints[i];
                var endPoint = spoints[i + 1];

                // Transform the start and end points to screen space
                var s0 = (Point4D)startPoint * this.visualToScreen;
                var s1 = (Point4D)endPoint * this.visualToScreen;

                if (clipping != null)
                {
                    // Apply a clipping rectangle
                    var x0 = s0.X / s0.W;
                    var y0 = s0.Y / s0.W;
                    var x1 = s1.X / s1.W;
                    var y1 = s1.Y / s1.W;

                    if (!clipping.ClipLine(ref x0, ref y0, ref x1, ref y1))
                    {
                        continue;
                    }

                    s0.X = x0 * s0.W;
                    s0.Y = y0 * s0.W;
                    s1.X = x1 * s1.W;
                    s1.Y = y1 * s1.W;
                }

                var lx = (s1.X / s1.W) - (s0.X / s0.W);
                var ly = (s1.Y / s1.W) - (s0.Y / s0.W);
                var l2 = (lx * lx) + (ly * ly);

                var p00 = s0;
                var p01 = s0;
                var p10 = s1;
                var p11 = s1;

                if (l2.Equals(0))
                {
                    // coinciding points (in world space or screen space)
                    var dz = halfThickness;

                    // TODO: make a square with the thickness as side length
                    p00.X -= dz * p00.W;
                    p00.Y -= dz * p00.W;

                    p01.X -= dz * p01.W;
                    p01.Y += dz * p01.W;

                    p10.X += dz * p10.W;
                    p10.Y -= dz * p10.W;

                    p11.X += dz * p11.W;
                    p11.Y += dz * p11.W;
                }
                else
                {
                    var m = halfThickness / Math.Sqrt(l2);

                    // the normal (dx,dy)
                    var dx = -ly * m;
                    var dy = lx * m;

                    // segment start points
                    p00.X += dx * p00.W;
                    p00.Y += dy * p00.W;
                    p01.X -= dx * p01.W;
                    p01.Y -= dy * p01.W;

                    // segment end points
                    p10.X += dx * p10.W;
                    p10.Y += dy * p10.W;
                    p11.X -= dx * p11.W;
                    p11.Y -= dy * p11.W;
                }

                if (!depthOffset.Equals(0))
                {
                    // Adjust the z-coordinate by the depth offset
                    p00.Z -= depthOffset;
                    p01.Z -= depthOffset;
                    p10.Z -= depthOffset;
                    p11.Z -= depthOffset;

                    // Transform from screen space to world space
                    p00 *= this.screenToVisual;
                    p01 *= this.screenToVisual;
                    p10 *= this.screenToVisual;
                    p11 *= this.screenToVisual;

                    positions.Add(new Point3D(p00.X / p00.W, p00.Y / p00.W, p00.Z / p00.W));
                    positions.Add(new Point3D(p01.X / p00.W, p01.Y / p01.W, p01.Z / p01.W));
                    positions.Add(new Point3D(p10.X / p00.W, p10.Y / p10.W, p10.Z / p10.W));
                    positions.Add(new Point3D(p11.X / p00.W, p11.Y / p11.W, p11.Z / p11.W));
                }
                else
                {
                    // Transform from screen space to world space
                    p00 *= this.screenToVisual;
                    p01 *= this.screenToVisual;
                    p10 *= this.screenToVisual;
                    p11 *= this.screenToVisual;

                    positions.Add(new Point3D(p00.X, p00.Y, p00.Z));
                    positions.Add(new Point3D(p01.X, p01.Y, p01.Z));
                    positions.Add(new Point3D(p10.X, p10.Y, p10.Z));
                    positions.Add(new Point3D(p11.X, p11.Y, p11.Z));
                }
            }

            positions.Freeze();
            return positions;
            //  return null;
        }
    }
}
