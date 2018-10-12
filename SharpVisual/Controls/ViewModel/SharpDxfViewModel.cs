using SharpDxf.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDxf.Visual.Controls
{
    public class SharpDxfViewModel
    {
        //Dxf对象
        private DxfDocument dxfDoc;
        //选中的图元
        private object selectedObject;
        //图层编辑使能
        private Dictionary<Layer, bool> layerEditSwitch;
        //图元存储空间
        private ObservableCollection<Entities.IEntityObject> entityObjects;


        //构造函数
        public SharpDxfViewModel()
        {
            entityObjects.CollectionChanged += EntityObjects_CollectionChanged;
        }

        private void EntityObjects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            
        }

        //添加图层
        public void AddLayer(string layerName , bool canEdit)
        {
            var lay = new Layer(layerName);
            layerEditSwitch.Add(lay, canEdit);
            dxfDoc.AddEntity
        }
 
    }
}
