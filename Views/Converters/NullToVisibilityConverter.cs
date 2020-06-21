using System;
using System.Windows;
using System.Windows.Data;

namespace car_pal.Views.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null) //Not invert parameter passed
            {
                return (value == null) ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (parameter.ToString() == "Inverse")
            {
                return (value == null) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
