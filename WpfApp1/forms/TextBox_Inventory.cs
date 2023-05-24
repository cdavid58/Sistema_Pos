using System;
using System.Globalization;

namespace WpfApp1.forms
{
    class TextBox_Inventory
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double height = (double)value;
            double fontSize = height * 0.5;
            return fontSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
