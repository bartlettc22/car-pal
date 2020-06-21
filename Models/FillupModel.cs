using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace car_pal.Models
{
    public class FillupModel : INotifyPropertyChanged
    {

        private DateTime _date;
        private float _fuelVolume;
        private float _odometerReading;
        private float _pricePerUnit;

        public FillupModel()
        {
            _date = DateTime.Now;
            _date.AddDays(2.0);
        }

        public DateTime FillupDate
        {
            get { return _date; }
            set
            {
                if (_date == value) return;
                _date = value;
                NotifyPropertyChanged("FillupDate");
            }
        }

        public float OdometerReading
        {
            get { return _odometerReading; }
            set
            {
                var roundedValue = (float)Math.Round(value, 0);
                if (_odometerReading.Equals(roundedValue)) return;
                _odometerReading = roundedValue;
                NotifyPropertyChanged("OdometerReading");
            }
        }

        public float FuelVolume
        {
            get { return _fuelVolume; }
            set
            {
                var roundedValue = (float)Math.Round(value, 1);
                if (_fuelVolume.Equals(roundedValue)) return;
                _fuelVolume = roundedValue;
                NotifyPropertyChanged("FuelVolume");
            }
        }

        public float PricePerUnit
        {
            get { return _pricePerUnit; }
            set
            {
                var roundedValue = (float)Math.Round(value, 2);
                if (_pricePerUnit.Equals(roundedValue)) return;
                _pricePerUnit = roundedValue;
                NotifyPropertyChanged("PricePerUnit");
            }
        }

        public float MPG
        {
            get
            {
                if (_fuelVolume != null && _odometerReading != null && _fuelVolume > 0 && _odometerReading > 0)
                {
                    return _odometerReading / _fuelVolume;
                }

                return 0;
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
