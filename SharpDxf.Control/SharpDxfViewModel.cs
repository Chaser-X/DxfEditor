using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Caliburn;
using Caliburn.Micro;
using System.Windows.Media;
using System.Windows;

using HelixToolkit.Wpf;
using HelixToolKit.Extension;
using SharpDxf.Entities;

namespace SharpDxf.Control
{
    public class SharpDxfViewModel : PropertyChangedBase
    {
        public SharpDxfViewModel() { }

        Model3DGroup model3d = new Model3DGroup();
        public Model3DGroup Model3D
        {
            get
            {
                return model3d;
            }
            set
            {
                model3d = value;
                NotifyOfPropertyChange(() => Model3D);
            }
        }



        public void Load(string filename)
        {
            // var l = new System.Windows.Shapes.Line();
            //ObjReader CurrentHelixObjReader = new ObjReader();
            ////相对路径，新建了一个objFile文件夹，添加了mtl，obj 和纹理图片
            //Model3D = CurrentHelixObjReader.Read(filename);
            //var l = new LinesVisual3D();
            //var ls = new HelixToolkit.Wpf.LineSegment(new System.Windows.Point(0, 0), new System.Windows.Point(10, 190));

            SharpDxf.DxfDocument doc = new DxfDocument();
            doc.Load(@"C:\Users\SHZBG\Desktop\3DVisual\metal-part-01.dxf");
            foreach (var l in doc.Lines)
            {
                var l3d = new LinesVisual3D();
                l3d.Points.Add(new Point3D(l.StartPoint.X, l.StartPoint.Y, l.StartPoint.Z));
                l3d.Points.Add(new Point3D(l.EndPoint.X, l.EndPoint.Y, l.EndPoint.Z));
                l3d.Thickness = 0.1;
                l3d.Color = Colors.Red;
                Model3D.Children.Add(l3d.Content);
            }

            foreach (var a in doc.Circles)
            {
                var arc = new ArcVisual3D(new Point3D(a.Center.X, a.Center.Y, a.Center.Z), a.Radius, 0, 2 * Math.PI, 64, 0.1);
                arc.Color = Colors.Red;
                Model3D.Children.Add(arc.Content);
            }

            foreach (var a in doc.Arcs)
            {
                var arc = new ArcVisual3D(new Point3D(a.Center.X, a.Center.Y, a.Center.Z), a.Radius, a.StartAngle * Math.PI / 180, a.EndAngle * Math.PI / 180, 64, 0.1);
                arc.Color = Colors.Red;

                Model3D.Children.Add(arc.Content);
            }

            foreach (var p in doc.Polylines)
            {
                var mesh = new MeshBuilder();
                var pp = p as PolyfaceMesh;


                var points = pp.Vertexes.Select(x => new Point3D(x.Location.X, x.Location.Y, x.Location.Z));

                var p3d = new PointsVisual3D();

                p3d.Points = new Point3DCollection(points);

                mesh.Normals = null;
                mesh.TextureCoordinates = null;

                foreach (var x in points)
                    mesh.Positions.Add(x);

                foreach (var x in pp.Faces)
                {
                    mesh.AddTriangle(x.VertexIndexes);
                }
                var model = new GeometryModel3D(mesh.ToMesh(), new DiffuseMaterial(Brushes.Green));
                model.BackMaterial = new DiffuseMaterial(Brushes.Green);


                Model3D.Children.Add(model);

            }
            NotifyOfPropertyChange(() => Model3D);

        }
    }
}
