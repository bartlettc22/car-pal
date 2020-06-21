using System;
using System.Linq;
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
        private ObservableCollection<zVehicleModel> _vehicles;
        private int _defaultVehicleIndex = -1;

        public static event EventHandler DefaultVehicleChanged;

        public GarageModel()
        {
            _vehicles = new ObservableCollection<zVehicleModel>();
        }

        public ObservableCollection<zVehicleModel> Vehicles
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
                NotifyDefaultVehicleUpdated();
            }
        }

        public zVehicleModel DefaultVehicle
        {
            get 
            {
                if (_defaultVehicleIndex >= 0)
                {
                    return _vehicles[DefaultVehicleIndex];
                }

                return null;
            }
        }

        public void addVehicle(zVehicleModel vehicle) 
        {
            _vehicles.Insert(0, vehicle);
            DataStore.SaveGarage();
        }

        public void deleteVehicle(zVehicleModel vehicle)
        {

            Boolean defaultChanged = false;

            // If this is the only vehicle, empty the garage
            if (GarageSize == 1)
            {
                _defaultVehicleIndex = -1;
                defaultChanged = true;
            }
            // If we're deleting the default vehicle, change the default
            else if (vehicle == DefaultVehicle)
            {
                _defaultVehicleIndex = 0;
                defaultChanged = true;
            }

            _vehicles.Remove(vehicle);
            DataStore.SaveGarage();

            if (defaultChanged == true)
            {
                NotifyDefaultVehicleUpdated();
            }
        }

        public int GarageSize
        {
            get { return _vehicles.Count; }
        }

        public Int16 getVehicleIndexByName(String name)
        {
            Int16 i = 0;
            foreach (zVehicleModel v in _vehicles)
            {
                if (v.Name == name)
                {
                    return i;
                }
                i++;
            }

            return 0;
        }

        public zVehicleModel getVehicle(String name)
        {
            if (vehicleExists(name))
            {
               return (from v in Vehicles
                         where v.Name.ToUpper() == name.ToUpper()
                         select v).First();
            }

            return null;
        }

        public bool vehicleExists(String name)
        {
            int count = (from v in Vehicles
                         where v.Name.ToUpper() == name.ToUpper()
                              select v).Count();
            return (count > 0) ? true : false;
        }

        private static void NotifyDefaultVehicleUpdated()
        {
            var handler = DefaultVehicleChanged;
            if (handler != null) handler(null, null);
        }
    }
}
