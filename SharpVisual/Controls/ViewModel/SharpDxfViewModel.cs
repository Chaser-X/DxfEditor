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

namespace SharpDxf.Visual.Controls
{
    public class SharpDxfViewModel : NotifyPropertyChangeBase
    {
        //Dxf对象
        private DxfDocument dxfDoc;
        //选中的图元
        private object selectedObject;
        //图层编辑使能
        private bool enableEdit;
        //图元存储空间
        private ObservableCollection<DxfVisualElement> entityObjects = new ObservableCollection<DxfVisualElement>();

        private Layer editLayer = new Layer("userLayer");

        #region public property
        public ObservableCollection<DxfVisualElement> EntityObjects
        {
            get
            {
                return entityObjects;
            }
            set
            {
                SetProperty(ref entityObjects, value);
            }
        } 

        public object SelectedObject
        {
            get {
                return selectedObject;
            }
            set {
                SetProperty(ref selectedObject, value);
            }
        }
        #endregion


        //构造函数
        public SharpDxfViewModel()
        {
            EntityObjects.Add(new DxfLineElement());
        }


        public void loadDxf(string filename)
        {
            dxfDoc = new DxfDocument();
            dxfDoc.Load(filename);
            
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
            dxfDoc.Save(filename, dxfDoc.Version);
        }
    }
}
