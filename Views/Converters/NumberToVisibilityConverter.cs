using System;
using System.Windows;
using System.Windows.Data;

namespace car_pal.Views.Converters
{
    public class NumberToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null)
            {
                return ((int)value == 0) ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (parameter.ToString() == "Inverse")
            {
                return ((int)value == 0) ? Visibility.Visible : Visibility.Collapsed;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
