using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System;

namespace car_pal.Database
{
    public class DatabaseContext : DataContext
    {
        // Specify the connection string as a static, used in main page and app.xaml.
        public static string DBConnectionString = "Data Source=isostore:/carpal.sdf";

        // Pass the connection string to the base class.
        public DatabaseContext(string connectionString): base(connectionString) { }

        public Table<TblVehicles> Vehicles;
        public Table<TblVehicles> Fillups;
    }

    // Define the vehicle database table.
    [Table]
    public class TblVehicles : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define Vehicle ID: private field, public property and database column.
        private int _vehicleId;

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

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        // Define the entity set for the collection side of the relationship.
        private EntitySet<TblFillups> _fillups;

        [Association(Storage = "_fillups", OtherKey = "FillupId", ThisKey = "_vehicleId")]
        public EntitySet<TblFillups> Fillups
        {
            get { return this._fillups; }
            set { this._fillups.Assign(value); }
        }

        // Assign handlers for the add and remove operations, respectively.
        public TblVehicles()
        {
            _fillups = new EntitySet<TblFillups>(
                new Action<TblFillups>(this.attach_Fillup),
                new Action<TblFillups>(this.detach_Fillup)
                );
        }

        // Called during an add operation
        private void attach_Fillup(TblFillups fillup)
        {
            NotifyPropertyChanging("Vehicle");
            fillup.Vehicle = this;
        }

        // Called during a remove operation
        private void detach_Fillup(TblFillups fillup)
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
    public class TblFillups : INotifyPropertyChanged, INotifyPropertyChanging
    {
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

        // Define Vehicle name: private field, public property and database column.
        private double _vehicleOdo;

        [Column]
        public double VehicleOdo
        {
            get
            {
                return _vehicleOdo;
            }
            set
            {
                if (_vehicleOdo != value)
                {
                    NotifyPropertyChanging("VehicleOdo");
                    _vehicleOdo = value;
                    NotifyPropertyChanged("VehicleOdo");
                }
            }
        }

        // Internal column for the associated ToDoCategory ID value
        [Column]
        internal int _vehicleId;

        // Entity reference, to identify the ToDoCategory "storage" table
        private EntityRef<TblVehicles> _vehicle;

        // Association, to describe the relationship between this key and that "storage" table
        [Association(Storage = "_vehicle", ThisKey = "_vehicleId", OtherKey = "VehicleId", IsForeignKey = true)]
        public TblVehicles Vehicle
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
