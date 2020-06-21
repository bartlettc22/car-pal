using System;
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
    public class GarageModel
    {
        private ObservableCollection<VehicleModel> _vehicles;
        //private VehicleModel _defaultVehicle;
        private int _defaultVehicleIndex = 0;

        public ObservableCollection<VehicleModel> Vehicles
        {
            get { return _vehicles; }
            set
            {
                if (_vehicles == value) return;
                _vehicles = value;
                if (_vehicles != null)
                {
                    _vehicles.CollectionChanged += delegate
                    {
                        //NotifyPropertyChanged("AverageFuelEfficiency");
                        //NotifyPropertyChanged("LastFillup");
                    };
                }
                //NotifyPropertyChanged("FillupHistory");
                //NotifyPropertyChanged("AverageFuelEfficiency");

            }
        }

        public int DefaultVehicleIndex
        {
            get { return _defaultVehicleIndex; }
            set
            {
                _defaultVehicleIndex = value;
            }
        }

        public VehicleModel DefaultVehicle
        {
            get { return _vehicles[DefaultVehicleIndex]; }
        }

        public void addVehicle(VehicleModel vehicle) 
        {
            _vehicles.Insert(0, vehicle);
            DataStore.SaveGarage();
        }

        public void deleteVehicle(VehicleModel vehicle)
        {
            //_vehicles.Insert(0, vehicle);
            _vehicles.Remove(vehicle);
            DataStore.SaveGarage();
        }

        public int GarageSize
        {
            get { return _vehicles.Count; }
        }

        public Int16 getVehicleIndexByName(String name)
        {
            Int16 i = 0;
            foreach (VehicleModel v in _vehicles)
            {
                if (v.Name == name)
                {
                    return i;
                }
                i++;
            }

            return 0;
        }
    }
}
