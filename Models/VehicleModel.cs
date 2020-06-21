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
using System.Collections.ObjectModel;

namespace car_pal.Models
{
    public class VehicleModel : INotifyPropertyChanged
    {
        private string _name;
        private ObservableCollection<FillupModel> _fillupHistory;

        public VehicleModel()
        {
            _fillupHistory = new ObservableCollection<FillupModel>();
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public ObservableCollection<FillupModel> FillupHistory
        {
            get { return _fillupHistory; }
            set
            {
                if (_fillupHistory == value) return;
                _fillupHistory = value;
                if (_fillupHistory != null)
                {
                    _fillupHistory.CollectionChanged += delegate
                    {
                        //NotifyPropertyChanged("AverageFuelEfficiency");
                        //NotifyPropertyChanged("LastFillup");
                    };
                }
                //NotifyPropertyChanged("FillupHistory");
                //NotifyPropertyChanged("AverageFuelEfficiency");

            }
        }

        public Boolean isDefaultVehicle
        {
            get
            {
                if (DataStore.Garage.getVehicleIndexByName(this.Name) == DataStore.Garage.DefaultVehicleIndex)
                {
                    return true;
                }
                return false;
            }
        }

        public void addFillup(FillupModel fillup)
        {

            _fillupHistory.Insert(0, fillup);
            DataStore.SaveGarage(); 
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
