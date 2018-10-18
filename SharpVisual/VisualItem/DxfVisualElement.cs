using HelixToolkit.Wpf;
using PropertyTools.DataAnnotations;
using SharpDxf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SharpDxf.Visual
{
    public interface IDxfVisualElement
    {
        bool IsSelected { get; }
        Color Color { get; set; }
        IEntityObject ToDxfEntity();
    }

    public abstract class DxfVisualElement : ModelVisual3D, IDxfVisualElement
    {
        // Using a DependencyProperty as the backing store for ColorProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(DxfVisualElement), new UIPropertyMetadata(Colors.Red, (s, e) =>
            {
                var model = ((DxfVisualElement)s).Content as GeometryModel3D;
                model.BackMaterial = new DiffuseMaterial(new SolidColorBrush((Color)e.NewValue));
                model.Material = new DiffuseMaterial(new SolidColorBrush((Color)e.NewValue));
            }));

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(DxfVisualElement), new UIPropertyMetadata(false, (s, e) =>
            {
                var self = (DxfVisualElement)s;
                self.Color = (bool)e.NewValue == true ? Colors.DarkBlue : Colors.Black;
                if ((bool)e.NewValue)
                    self.drawActiveHandle();
                else
                    self.clearActiveHandle();
            }));


        /// <summary>
        /// 获取或设置当前对象呈现的颜色
        /// </summary>
        [Category("Common")]
        [Browsable(false)]
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        /// <summary>
        /// 获取当前对象是否被选中
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            internal set { SetValue(IsSelectedProperty, value); }
        }

        #region interactive visual object region
        protected int numberofHandle = 0;
        protected int activeHandle = -2;
        /// <summary>
        /// 获取激活的绘图句柄
        /// </summary>
        /// <param name="currentPos">当前鼠标位置</param>
        public virtual void UpdateActiveHandle(Point3D currentPos)
        {

        }
        /// <summary>
        /// 响应激活的绘图句柄动作
        /// </summary>
        /// <param name="currentPos">当前鼠标位置</param>
        public virtual void moveByHandle(Point3D offset)
        {
        }
        /// <summary>
        /// 绘图句柄可视化
        /// </summary>
        protected virtual void drawActiveHandle()
        {

        }

        /// <summary>
        /// 清除绘图句柄
        /// </summary>
        protected virtual void clearActiveHandle()
        {

        }
        /// <summary>
        /// 更新元素呈现
        /// </summary>
        protected virtual void updateElement()
        {
            if (IsSelected)
                drawActiveHandle();
        }
        #endregion

        /// <summary>
        /// 返回Dxf对象
        /// </summary>
        /// <returns>Dxf对象</returns>
        public virtual IEntityObject ToDxfEntity()
        {
            return null;
        }
    }
}
