using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace CashGame.Apresentation.Wpf.Conversoes
{
    public class SomenteNumeros : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return Regex.Replace(value.ToString(), @"[^0-9]+", "");
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var t = Regex.Replace(value.ToString(), @"[^0-9]+", "");
            return string.IsNullOrWhiteSpace(t) ? "0" : t;
        }
    }
}
