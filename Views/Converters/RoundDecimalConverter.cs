using System;
using System.Windows.Data;

namespace car_pal.Views.Converters
{
    public class RoundDecimalConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int size = 2;
            int.TryParse(parameter.ToString(), out size);
            return String.Format("0:0.00", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
