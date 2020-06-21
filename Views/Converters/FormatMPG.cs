using System;
using System.Windows.Data;

namespace car_pal.Views.Converters
{
    public class FormatMPG : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((double)value == 0)
            {
                return "-";
            }
            return String.Format("{0:0}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
