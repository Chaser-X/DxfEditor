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
using System.Windows.Input;

namespace SharpDxf.Visual.Controls
{
    public class SharpDxfEngine : NotifyPropertyChangeBase
    {
        #region private vairable section
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
        private ObservableCollection<Visual3D> entityObjects;
        //需要显示的DXF对像
        private ModelVisual3D showModels = new ModelVisual3D();

        private Layer editLayer = new Layer("userLayer");
        #endregion

        #region public property
        /// <summary>
        /// 所有可视化对象集合
        /// </summary>
        public ObservableCollection<Visual3D> EntityObjects
        {
            get
            {
                return entityObjects;
            }
            internal set
            {
                if (entityObjects != null)
                    entityObjects.CollectionChanged -= EntityObjects_CollectionChanged;
                SetProperty(ref entityObjects, value);
                entityObjects.CollectionChanged += EntityObjects_CollectionChanged;

            }
        }
        /// <summary>
        /// 当前选中的Dxf对象
        /// </summary>
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
        /// <summary>
        /// 选中命令
        /// </summary>
        public DxfViusalElementSelectionCommand SelectionCommand { get; private set; }
        #endregion

        //构造函数
        public SharpDxfEngine(HelixViewport3D view)
        {
            viewPort = view;
            EntityObjects = new ObservableCollection<Visual3D>();

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

            viewPort.Children.Add(coordinateSystem);
            viewPort.Children.Add(light);
            viewPort.Children.Add(backGroundGrid);
            viewPort.Children.Add(showModels);

            SelectionCommand = new DxfViusalElementSelectionCommand(this.viewPort, HandleSelectionVisualsEvent);
            viewPort.InputBindings.Add(new MouseBinding(SelectionCommand, new MouseGesture(MouseAction.LeftClick)));
        }

        /// <summary>
        /// 加载Dxf文档
        /// </summary>
        /// <param name="filename">文档路径</param>
        public void LoadDxf(string filename)
        {
            EntityObjects.Clear();
            dxfDoc = new DxfDocument();
            dxfDoc.Load(filename);

            foreach (var item in dxfDoc.EntityCollection)
            {
                switch (item.Type)
                {
                    case EntityType.Line:
                        this.EntityObjects.Add(new DxfLineElement(item as Line));
                        break;
                    case EntityType.Point: break;
                    case EntityType.Arc: break;
                    case EntityType.Circle: break;
                    default: break;
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
        /// <summary>
        /// 保存DXF文档
        /// </summary>
        /// <param name="filename">保存路径</param>
        public void SaveDxf(string filename)
        {
            dxfDoc = new DxfDocument();
            EntityObjects.Where(x => x is DxfVisualElement).ToList().ForEach(
                (x) =>
                {
                    dxfDoc.AddEntity(((DxfVisualElement)x).ToDxfEntity());
                }
                );
            dxfDoc.Save(filename, dxfDoc.Version);
        }
        /// <summary>
        /// 视图元素列表更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntityObjects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            showModels.Children.Clear();
            EntityObjects?.ToList().ForEach(x => showModels.Children.Add(x));
        }
        /// <summary>
        /// 选中可视化物件时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void HandleSelectionVisualsEvent(object sender, VisualSelectedEventArgs args)
        {
            this.SelectedObject = args.SelectedVisual as DxfVisualElement;
        }
    }
}
