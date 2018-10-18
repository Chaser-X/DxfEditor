using SharpDxf.Entities;
using SharpDxf.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDxf.Visual;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using HelixToolKit.Extension;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace SharpDxf.Visual.Controls
{
    public class SharpDxfViewModel : NotifyPropertyChangeBase
    {
        //视图窗口
        private HelixViewport3D viewPort;
        //Dxf对象
        private DxfDocument dxfDoc;
        //选中的图元
        private DxfVisualElement selectedObject = null;
        //图层编辑使能
        private bool enableEdit;
        //坐标系空间
        private CoordinateSystemVisual3D coordinateSystem;
        private HelixToolKit.Extension.GridLinesVisual3D backGroundGrid;
        private LightSetup light;

        //图元存储空间
        private ObservableCollection<Visual3D> entityObjects = new ObservableCollection<Visual3D>();

        private Layer editLayer = new Layer("userLayer");

        #region public property
        public ObservableCollection<Visual3D> EntityObjects
        {
            get
            {
                return entityObjects;
            }
            internal set
            {
                SetProperty(ref entityObjects, value);
            }
        }

        public DxfVisualElement SelectedObject
        {
            get
            {
                return selectedObject;
            }
            internal set
            {
                SetProperty(ref selectedObject, value);
            }
        }

        public DxfViusalElementSelectionCommand SelectionCommand { get; private set; }
        #endregion

        //构造函数
        public SharpDxfViewModel(HelixViewport3D view)
        {
            viewPort = view;

            coordinateSystem = new CoordinateSystemVisual3D()
            {
                ArrowLengths = 5
            };
            light = new SunLight() { ShowLights = false };
            backGroundGrid = new HelixToolKit.Extension.GridLinesVisual3D()
            {
                Center = new Point3D(0, 0, 0),
                MajorDistance = 50,
                Length = 400,
                Width = 400,
                MinorDistance = 5,
                MajorLineThickness = 1,
                MinorLineThickness = 0.5
            };

            EntityObjects.Add(coordinateSystem);
            EntityObjects.Add(light);
            EntityObjects.Add(backGroundGrid);

            SelectionCommand = new DxfViusalElementSelectionCommand(this.viewPort, HandleSelectionVisualsEvent);
        }

        public void LoadDxf(string filename)
        {
            EntityObjects.Clear();
            EntityObjects.Add(coordinateSystem);
            EntityObjects.Add(light);
            EntityObjects.Add(backGroundGrid);

            dxfDoc = new DxfDocument();
            dxfDoc.Load(filename);

            foreach (var item in dxfDoc.EntityCollection)
            {
                switch (item.Type)
                {
                    case EntityType.Line:
                        this.EntityObjects.Add(new DxfLineElement(item as Line));
                        break;
                    case EntityType.Point:break;
                    case EntityType.Arc:break;
                    case EntityType.Circle:break;
                    default:break;
                }
            }
        }
        ////添加图元
        //public void AddEntity(IEntityObject obj)
        //{
        //    if (!enableEdit) return;
        //    obj.Layer = editLayer;
        //    EntityObjects.Add(obj);
        //    SelectedObject = obj;
        //}

        //public void DeletEntity(IEntityObject obj)
        //{
        //    if (!enableEdit) return;
        //    if (obj.Layer != editLayer)
        //        throw new NotImplementedException("Can not delete a unuserlayer entity");
        //    if (EntityObjects.Contains(obj))
        //        EntityObjects.Remove(obj);
        //    SelectedObject = null;
        //}
        public void SaveDxf(string filename)
        {
            dxfDoc = new DxfDocument();
            EntityObjects.Where(x => x is DxfVisualElement).ToList().ForEach(
                (x) => {
                    dxfDoc.AddEntity(((DxfVisualElement)x).ToDxfEntity());
                    }
                );
            dxfDoc.Save(filename, dxfDoc.Version);
        }
        private void HandleSelectionVisualsEvent(object sender, VisualSelectedEventArgs args)
        {
            this.SelectedObject = args.SelectedVisual as DxfVisualElement;
        }
    }
}
