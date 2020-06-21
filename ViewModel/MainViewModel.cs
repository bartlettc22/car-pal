using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using car_pal.Models;
using System;
using System.IO.IsolatedStorage;
using System.Windows;

namespace car_pal.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database. Also, init local datastore
        private DatabaseContext _db;

        // Class constructor, create the data context object.
        public MainViewModel()
        {
            _db = new DatabaseContext(DatabaseContext.DBConnectionString);
            PropertyChanged += delegate(object sender, PropertyChangedEventArgs arg) { Debug.WriteLine("MainViewModel Property Changed!: "+arg.PropertyName); };
        }

        // Vehicle List
        private ObservableCollection<VehicleModel> _allVehicles;
        public ObservableCollection<VehicleModel> AllVehicles
        {
            get 
            {
                /*if (_allVehicles != null)
                {
                    Debug.WriteLine("Number of Vehicles: " + _allVehicles.Count());
                }
                else
                {
                    Debug.WriteLine("No vehicles found.");
                }*/
                return _allVehicles; 
            }
            set
            {
                _allVehicles = value;
                NotifyPropertyChanged("AllVehicles");
            }
        }

        public VehicleModel DefaultVehicle
        {
            get
            {
                if (AllVehicles.Count > 0)
                {
                    if (AllVehicles.Count(v => v.IsDefaultVehicle == true) == 1)
                    {
                        return AllVehicles.First(v => v.IsDefaultVehicle == true);
                    }
                    else
                    {
                        // reset to 1 default???
                        DefaultVehicle = AllVehicles.First();
                        return AllVehicles.First();
                    }
                    
                }

                return null;
            }
            set
            {
                Debug.WriteLine("Changing Default Vehicle from \"" + this.DefaultVehicle.VehicleName +"\" to \"" + value.VehicleName +"\"");
                
                foreach (VehicleModel vehicle in (from v in _db.Vehicles select v))
                {
                    if(vehicle.VehicleId == value.VehicleId)
                    {
                        vehicle.IsDefaultVehicle = true;
                    }
                    else
                    {
                        vehicle.IsDefaultVehicle = false;
                    }
                }

                // Submit the changes to the database.
                try
                {
                    _db.SubmitChanges();
                }
                catch (Exception e)
                {
                    // Provide for exceptions.
                }

                NotifyPropertyChanged("DefaultVehicle");
                
            }
        }

        // Add vehicle to database
        public void AddVehicle(VehicleModel vehicle)
        {

            // Check if this is first vehicle... if so, make it default
            if (_db.Vehicles.Count() == 0)
            {
                vehicle.IsDefaultVehicle = true;
            }

            _db.Vehicles.InsertOnSubmit(vehicle);
            _db.SubmitChanges();

            // Add the vehicle to the "all" observable collection.
            AllVehicles.Add(vehicle);
            Debug.WriteLine("Added Vehicle: (" + vehicle.VehicleId + ", \"" + vehicle.VehicleName + "\", "+ vehicle.IsDefaultVehicle +")");
        }

        public void DeleteVehicle(VehicleModel vehicle)
        {
            // If this is the default vehicle, change it
            if (vehicle.IsDefaultVehicle && AllVehicles.Count > 1)
            {
                VehicleModel NewDefaultVehicle = AllVehicles.First(v => v.VehicleId != vehicle.VehicleId);
                NewDefaultVehicle.IsDefaultVehicle = true;
            }

            // Remove the vehicle from the "all" observable collection.
            Debug.WriteLine("Removed Vehicle: (" + vehicle.VehicleId + ", \"" + vehicle.VehicleName + "\")");
            AllVehicles.Remove(vehicle);
            
            _db.Vehicles.DeleteOnSubmit(vehicle);
            _db.SubmitChanges();
        }

        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {
            // Specify the query for all to-do items in the database.
            var vehiclesInDB = from VehicleModel vehicle in _db.Vehicles
                               select vehicle;

            // Query the database and load all to-do items.
            AllVehicles = new ObservableCollection<VehicleModel>(vehiclesInDB);
        }
        //
        // TODO: Add collections, list, and methods here.
        //

        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            _db.SubmitChanges();
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
