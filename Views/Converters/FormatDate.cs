using System;
using System.Windows.Data;

namespace car_pal.Views.Converters
{
    public class FormatDate : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return String.Format("{0:dd - MMM}", value) + "\n" + String.Format("{0:yyyy}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
