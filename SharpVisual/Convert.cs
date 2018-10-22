using System;
using System.Globalization;
using System.Windows.Data;

namespace SharpDxf.Visual
{
    public class AddDxfVisualConvert : IValueConverter
    {
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var obj = value as DxfVisualElement;
            var par = parameter.ToString();
            if (obj == null)
                return false;
            switch (obj.GetType().Name)
            {
                case nameof(DxfLineElement):
                    return par == "Line";
                case nameof(DxfArcElement):
                    return par == "Arc";
                case nameof(DxfPointElement):
                    return par == "Point";
                case nameof(DxfCircleElement):
                    return par == "Circle";
                case nameof(DxfTextElement):
                    return par == "Text";
                default: return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var obj = (bool)value;
            if (obj == false)
                return null;
            var par = parameter.ToString();
            switch (par)
            {
                case "Point": return new DxfPointElement();
                case "Line": return new DxfLineElement();
                case "Arc":return new DxfArcElement();
                case "Circle": return new DxfCircleElement();
                case "Text": return new DxfTextElement();
                default: return null;
            }
        }
    }
}
