using System;
using System.ComponentModel;
using System.Device.Location;
using System.Windows.Media;

namespace car_pal.ViewModel
{
    public class StationItemViewModel : INotifyPropertyChanged
    {

        private String _title;
        private String _address;
        private String _phoneNumber;
        private double _distance;
        private int _itemNumber;
        private GeoCoordinate _coordinates;
        private SolidColorBrush _titleColor;

        public StationItemViewModel()
        {
            TitleColor = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
        }

        public int ItemNumber
        {
            get
            {
                return _itemNumber;
            }
            set
            {
                _itemNumber = value;
            }

        }

        public String Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public String Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        public String PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
            }
        }

        public String Distance
        {
            get
            {
                return String.Format("{0:0.00}", _distance) + " mi";
            }
            set
            {
                _distance = double.Parse(value);
            }
        }

        public GeoCoordinate Coordinates
        {
            get
            {
                return _coordinates;
            }
            set
            {
                _coordinates = value;
            }
        }


        public SolidColorBrush TitleColor
        {
            get
            {
                return _titleColor;
            }
            set
            {
                _titleColor = value;
                NotifyPropertyChanged("TitleColor");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
