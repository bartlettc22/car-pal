using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Diagnostics;

namespace car_pal.Models
{
    public class DatabaseContext : DataContext, INotifyPropertyChanged
    {
        // AppSetting info
        //private const string DEFAULT_VEHICLE_KEY = "car_pal.DefaultVehicle";
        //private readonly IsolatedStorageSettings _appSettings = IsolatedStorageSettings.ApplicationSettings;
        //private int _defaultVehicleId = -1;

        // Specify the connection string as a static, used in main page and app.xaml.
        public static string DBConnectionString = "Data Source=isostore:/carpal.sdf";

        // Pass the connection string to the base class.
        public DatabaseContext(string connectionString) : base(connectionString) 
        { 
            /*if (_appSettings.Contains(DEFAULT_VEHICLE_KEY))
            {
                _defaultVehicleId = (int)_appSettings[DEFAULT_VEHICLE_KEY];
            }*/

            PropertyChanged += delegate(object sender, PropertyChangedEventArgs arg) { Debug.WriteLine("DatabaseContext Property Changed!: " + arg.PropertyName); };
        }

        // DB table models
        public Table<VehicleModel> Vehicles;
        public Table<FillupModel> Fillups;

        /*public int DefaultVehicleId
        {
            get
            {
                return _defaultVehicleId;
            }
            set
            {
                try
                {
                    _appSettings[DEFAULT_VEHICLE_KEY] = value;
                    _appSettings.Save();
                    _defaultVehicleId = value;
                }
                catch (IsolatedStorageException)
                {
                    MessageBox.Show("Error saving data to device.");
                }
            }
        }*/

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    // Define the vehicle database table.
    [Table]
    public class VehicleModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define Vehicle ID: private field, public property and database column.
        private int _vehicleId;
        //private bool _isDefault = false;

        /*public bool isDefaultVehicle
        {
            get
            {
                return _isDefault;
            }
            set
            {
                NotifyPropertyChanging("isDefaultVehicle");
                _isDefault = value;
                NotifyPropertyChanged("isDefaultVehicle");
            }
        }*/


        /*public bool isDefaultVehicle
        {
            get
            {
                if (_vehicleId == DatabaseContext.DefaultVehicleId)
                {
                    return true;
                }

                return false;
            }
        }

        public void setAsDefaultVehicle()
        {
            if (_vehicleId != DatabaseContext.DefaultVehicleId)
            {
                DatabaseContext.DefaultVehicleId = _vehicleId;
            }
        }*/

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int VehicleId
        {
            get
            {
                return _vehicleId;
            }
            set
            {
                if (_vehicleId != value)
                {
                    NotifyPropertyChanging("VehicleId");
                    _vehicleId = value;
                    NotifyPropertyChanged("VehicleId");
                }
            }
        }

        // Define Vehicle name: private field, public property and database column.
        private string _vehicleName;

        [Column]
        public string VehicleName
        {
            get
            {
                return _vehicleName;
            }
            set
            {
                if (_vehicleName != value)
                {
                    NotifyPropertyChanging("VehicleName");
                    _vehicleName = value;
                    NotifyPropertyChanged("VehicleName");
                }
            }
        }

        // Define Vehicle default: private field, public property and database column.
        private bool _isDefaultVehicle = false;

        [Column]
        public bool IsDefaultVehicle
        {
            get
            {
                return _isDefaultVehicle;
            }
            set
            {
                if (_isDefaultVehicle != value)
                {
                    NotifyPropertyChanging("IsDefaultVehicle");
                    _isDefaultVehicle = value;
                    NotifyPropertyChanged("IsDefaultVehicle");
                }
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        // Define the entity set for the collection side of the relationship.
        private EntitySet<FillupModel> _fillups;

        [Association(Storage = "_fillups", OtherKey = "_vehicleId", ThisKey = "VehicleId")]
        public EntitySet<FillupModel> Fillups
        {
            get { return this._fillups; }
            set { this._fillups.Assign(value); }
        }

        // Assign handlers for the add and remove operations, respectively.
        public VehicleModel()
        {
            _fillups = new EntitySet<FillupModel>(
                new Action<FillupModel>(this.attach_Fillup),
                new Action<FillupModel>(this.detach_Fillup)
                );
            _fillups.CollectionChanged += delegate { Debug.WriteLine("Fillups Changed!"); NotifyPropertyChanged("Fillups"); };
            PropertyChanged += delegate(object sender, PropertyChangedEventArgs arg) { Debug.WriteLine("VehicleModel Property Changed!: " + arg.PropertyName); };
        }

        // Called during an add operation
        private void attach_Fillup(FillupModel fillup)
        {
            NotifyPropertyChanging("Vehicle");
            fillup.Vehicle = this;
        }

        // Called during a remove operation
        private void detach_Fillup(FillupModel fillup)
        {
            NotifyPropertyChanging("Vehicle");
            fillup.Vehicle = null;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

    // Define the fillup database table.
    [Table]
    public class FillupModel : INotifyPropertyChanged, INotifyPropertyChanging
    {

        public FillupModel()
        {
            PropertyChanged += delegate(object sender, PropertyChangedEventArgs arg) { Debug.WriteLine("FillupModel Property Changed!: " + arg.PropertyName); };
        }

        // Define Fillup ID: private field, public property and database column.
        private int _fillupId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int FillupId
        {
            get
            {
                return _fillupId;
            }
            set
            {
                NotifyPropertyChanging("FillupId");
                _fillupId = value;
                NotifyPropertyChanged("FillupId");
            }
        }

        private DateTime _fillupDate;
        [Column]
        public DateTime FillupDate
        {
            get
            {
                return _fillupDate;
            }
            set
            {
                if (_fillupDate != value)
                {
                    NotifyPropertyChanging("FillupDate");
                    _fillupDate = value;
                    NotifyPropertyChanged("FillupDate");
                }
            }
        }

        private double _priceReading;
        [Column]
        public double PriceReading
        {
            get
            {
                return _priceReading;
            }
            set
            {
                if (_priceReading != value)
                {
                    NotifyPropertyChanging("PriceReading");
                    _priceReading = value;
                    NotifyPropertyChanged("PriceReading");
                }
            }
        }

        private double _odoReading;
        [Column]
        public double OdoReading
        {
            get
            {
                return _odoReading;
            }
            set
            {
                if (_odoReading != value)
                {
                    NotifyPropertyChanging("OdoReading");
                    _odoReading = value;
                    NotifyPropertyChanged("OdoReading");
                }
            }
        }

        private double _volReading;
        [Column]
        public double VolReading
        {
            get
            {
                return _volReading;
            }
            set
            {
                if (_volReading != value)
                {
                    NotifyPropertyChanging("VolReading");
                    _volReading = value;
                    NotifyPropertyChanged("VolReading");
                }
            }
        }

        // Internal column for the associated ToDoCategory ID value
        [Column]
        internal int _vehicleId;

        // Entity reference, to identify the ToDoCategory "storage" table
        private EntityRef<VehicleModel> _vehicle;

        // Association, to describe the relationship between this key and that "storage" table
        [Association(Storage = "_vehicle", ThisKey = "_vehicleId", OtherKey = "VehicleId", IsForeignKey = true)]
        public VehicleModel Vehicle
        {
            get { return _vehicle.Entity; }
            set
            {
                NotifyPropertyChanging("Vehicle");
                _vehicle.Entity = value;

                if (value != null)
                {
                    _vehicleId = value.VehicleId;
                }

                NotifyPropertyChanging("Vehicle");
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}
