using HelixToolkit.Wpf;
using PropertyTools.DataAnnotations;
using SharpDxf.Entities;
using System;
using sysComponent = System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
        Color SelectedColor { get; set; }
        IEntityObject ToDxfEntity();
    }

    [Serializable]
    public abstract class DxfVisualElement : ModelVisual3D, IDxfVisualElement, ICloneable
    {
        // Using a DependencyProperty as the backing store for ColorProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(DxfVisualElement), new UIPropertyMetadata(Colors.Transparent, (s, e) =>
            {
                ((DxfVisualElement)s).colorChanged(e);
            }));

        // Using a DependencyProperty as the backing store for ColorProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(DxfVisualElement), new UIPropertyMetadata(Colors.Yellow, (s, e) =>
            {
                ((DxfVisualElement)s).colorChanged(e);
            }));

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(DxfVisualElement), new UIPropertyMetadata(false, (s, e) =>
            {
                var self = (DxfVisualElement)s;
                if ((bool)e.NewValue)
                {
                    var c = new DependencyPropertyChangedEventArgs(IsSelectedProperty, self.Color, self.SelectedColor);
                    self.colorChanged(c);
                    self.drawActiveHandle();
                }
                else
                {
                    var c = new DependencyPropertyChangedEventArgs(IsSelectedProperty, self.SelectedColor, self.Color);
                    self.colorChanged(c);
                    self.clearActiveHandle();
                }
            }));

        /// <summary>
        /// 获取或设置当前对象呈现的颜色
        /// </summary>
        [Category("Common")]
        [Browsable(true)]
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// 获取或设置选中对象呈现的颜色
        /// </summary>
        [Category("Common")]
        [Browsable(true)]
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
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
        public abstract IEntityObject ToDxfEntity();
        /// <summary>
        /// 深拷贝对象
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();
        /// <summary>
        /// 颜色发生变化时触发更新
        /// </summary>
        /// <param name="e"></param>
        protected virtual void colorChanged(DependencyPropertyChangedEventArgs e)
        {
            var model = this.Content as GeometryModel3D;
            model.BackMaterial = new DiffuseMaterial(new SolidColorBrush((Color)e.NewValue));
            model.Material = new DiffuseMaterial(new SolidColorBrush((Color)e.NewValue));
        }
    }
}
