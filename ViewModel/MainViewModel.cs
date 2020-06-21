using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using car_pal.Models;

namespace car_pal.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database. Also, init local datastore
        private DatabaseContext _db;
        private VehicleModel _defaultVehicle;
        private ObservableCollection<FillupModel> _defaultFillups;
        private int _statSampleSize;
        private double _overallMPG;
        private double _overallCostMile;
        private double _overallMileFill;
        private double _overallDaysFill;

        // Class constructor, create the data context object.
        public MainViewModel()
        {
            _db = new DatabaseContext(DatabaseContext.DBConnectionString);
            //PropertyChanged += delegate(object sender, PropertyChangedEventArgs arg) { Debug.WriteLine("MainViewModel Property Changed!: "+arg.PropertyName); };
        }

        // Vehicle List
        private ObservableCollection<VehicleModel> _allVehicles;
        public ObservableCollection<VehicleModel> AllVehicles
        {
            get
            {
                return _allVehicles;
            }
            set
            {
                _allVehicles = value;
                NotifyPropertyChanged("AllVehicles");
            }
        }

        // Get all vehicles from db
        public void LoadCollectionsFromDatabase()
        {
            // Specify the query for all to-do items in the database.
            var vehiclesInDB = from VehicleModel vehicle in _db.Vehicles
                               select vehicle;

            // Query the database and load all to-do items.
            AllVehicles = new ObservableCollection<VehicleModel>(vehiclesInDB);
            _allVehicles.CollectionChanged += delegate { NotifyPropertyChanged("AllVehicles"); };

            initDefault();
        }

        public VehicleModel DefaultVehicle
        {
            get
            {
                return _defaultVehicle;
            }
            set
            {
                if (_defaultVehicle != value)
                {
                    _defaultVehicle = value;
                    saveDefault();
                    calcDefault();
                    NotifyPropertyChanged("DefaultVehicle");
                }
            }
        }

        public ObservableCollection<FillupModel> DefaultFillups
        {
            get
            {
                return _defaultFillups;
            }
            set
            {
                if (_defaultFillups != value)
                {
                    _defaultFillups = value;
                    NotifyPropertyChanged("DefaultFillups");
                }
            }
        }

        public double OverallMPG
        {
            get
            {
                return _overallMPG;
            }
            set
            {
                _overallMPG = value;
                NotifyPropertyChanged("OverallMPG");
            }
        }

        public double OverallCostMile
        {
            get
            {
                return _overallCostMile;
            }
            set
            {
                _overallCostMile = value;
                NotifyPropertyChanged("OverallCostMile");
            }
        }

        public double OverallMileFill
        {
            get
            {
                return _overallMileFill;
            }
            set
            {
                _overallMileFill = value;
                NotifyPropertyChanged("OverallMileFill");
            }
        }

        public double OverallDaysFill
        {
            get
            {
                return _overallDaysFill;
            }
            set
            {
                _overallDaysFill = value;
                NotifyPropertyChanged("OverallDaysFill");
            }
        }

        public int OverallFillupCount
        {
            get
            {
                return _statSampleSize;
            }
            set
            {
                _statSampleSize = value;
                NotifyPropertyChanged("OverallFillupCount");
            }
        }

        // Initializes and cleans up default vehicle
        public void initDefault()
        {
            if (DefaultVehicle == null)
            {
                if (AllVehicles.Count > 0)
                {
                    if (AllVehicles.Count(v => v.IsDefaultVehicle == true) == 1)
                    {
                        DefaultVehicle = AllVehicles.First(v => v.IsDefaultVehicle == true);
                    }
                    else
                    {
                        DefaultVehicle = AllVehicles.First();
                    }
                }
            }
        }

        // Save default car to database
        public void saveDefault()
        {
            foreach (VehicleModel vehicle in AllVehicles)
            {
                if (vehicle.VehicleId == _defaultVehicle.VehicleId)
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
        }

        // Called when default vehicle is set, calculates MPGs and other stats
        public void calcDefault()
        {

            OverallMPG = 0;
            OverallCostMile = 0;
            OverallMileFill = 0;
            OverallDaysFill = 0;
            
            FillupModel previousFillup = new FillupModel();

            DefaultFillups = new ObservableCollection<FillupModel>();
            int i = 0;
            double statMileDiff = 0;
            double statTotalVol = 0;
            TimeSpan statDayDiff = new TimeSpan();
            double statTotalCost = 0;
            foreach(FillupModel f in (from fu in DefaultVehicle.Fillups orderby fu.FillupDate descending select fu))
            {
                DefaultFillups.Add(f);
                if (previousFillup.OdoReading != 0.0D && previousFillup.VolReading != 0)
                {
                    previousFillup.FillupMPG = (previousFillup.OdoReading - f.OdoReading) / previousFillup.VolReading;

                    if (i <= 7)
                    {
                        statMileDiff += (previousFillup.OdoReading - f.OdoReading);
                        statTotalVol += (f.VolReading);
                        statDayDiff += (previousFillup.FillupDate - f.FillupDate);
                        statTotalCost += (previousFillup.PriceReading * previousFillup.VolReading);
                        i++;
                    }
                }

                previousFillup = f;
            }

            if (statTotalVol > 0 && statMileDiff > 0)
            {
                OverallMPG = statMileDiff / statTotalVol;
                OverallCostMile = statTotalCost / statMileDiff;
                OverallMileFill = statMileDiff / i;
                OverallDaysFill = statDayDiff.Days / i;
            }
            OverallFillupCount = i;
        }

        // Add vehicle to database
        public void AddVehicle(VehicleModel vehicle)
        {
            // Check if this is first vehicle... if so, make it default
            if (_db.Vehicles.Count() == 0)
            {
                vehicle.IsDefaultVehicle = true; DefaultVehicle = vehicle;
            }

            // Add default reminders
            ReminderModel reminderOil = new ReminderModel();
            reminderOil.ReminderType = "oil change";
            reminderOil.RemindDate = false;
            reminderOil.RemindDateValue = System.DateTime.Now;
            reminderOil.RemindOdo = false;
            vehicle.Reminders.Add(reminderOil);

            ReminderModel reminderRotate = new ReminderModel();
            reminderRotate.ReminderType = "rotate tires";
            reminderRotate.RemindDate = false;
            reminderRotate.RemindDateValue = System.DateTime.Now;
            reminderRotate.RemindOdo = false;
            vehicle.Reminders.Add(reminderRotate);

            ReminderModel reminderFluids = new ReminderModel();
            reminderFluids.ReminderType = "check fluids";
            reminderFluids.RemindDate = false;
            reminderFluids.RemindDateValue = System.DateTime.Now;
            reminderFluids.RemindOdo = false;
            vehicle.Reminders.Add(reminderFluids);

            _db.Vehicles.InsertOnSubmit(vehicle);
            _db.SubmitChanges();

            // Add the vehicle to the "all" observable collection.
            AllVehicles.Add(vehicle);
        }

        public void DeleteVehicle(VehicleModel vehicle)
        {
            // If this is the default vehicle, change it
            if (vehicle.IsDefaultVehicle && AllVehicles.Count > 1)
            {
                VehicleModel NewDefaultVehicle = AllVehicles.First(v => v.VehicleId != vehicle.VehicleId);
                NewDefaultVehicle.IsDefaultVehicle = true;
                DefaultVehicle = NewDefaultVehicle;
            }

            // Check if the vehicle has fillups and remove those
            foreach (FillupModel f in vehicle.Fillups)
            {
                _db.Fillups.DeleteOnSubmit(f);
            }

            // Check if the vehicle has reminders and remove those
            foreach (ReminderModel r in vehicle.Reminders)
            {
                _db.Reminders.DeleteOnSubmit(r);
            }

            // Remove the vehicle from the "all" observable collection.
            AllVehicles.Remove(vehicle);
            
            _db.Vehicles.DeleteOnSubmit(vehicle);
            _db.SubmitChanges();
            calcDefault();
        }

        public void AddFillup(FillupModel fillup)
        {
            App.ViewModel.DefaultVehicle.Fillups.Add(fillup);
            App.ViewModel.SaveChangesToDB();
            calcDefault();
            NotifyPropertyChanged("DefaultVehicle");
        }

        public void EditFillup(FillupModel fillup)
        {
            App.ViewModel.SaveChangesToDB();
            calcDefault();
            NotifyPropertyChanged("DefaultVehicle");
        }

        public void DeleteFillup(FillupModel fillup)
        {
            _db.Fillups.DeleteOnSubmit(fillup);
            App.ViewModel.SaveChangesToDB();
            calcDefault();
            NotifyPropertyChanged("DefaultVehicle");
        }

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
