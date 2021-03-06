﻿using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace car_pal.Models
{
    public class DatabaseContext : DataContext, INotifyPropertyChanged
    {
        // Specify the connection string as a static, used in main page and app.xaml.
        public static string DBConnectionString = "Data Source=isostore:/carpal.sdf";

        // Pass the connection string to the base class.
        public DatabaseContext(string connectionString) : base(connectionString) 
        { 
            //PropertyChanged += delegate(object sender, PropertyChangedEventArgs arg) { Debug.WriteLine("DatabaseContext Property Changed!: " + arg.PropertyName); };
        }

        // DB table models
        public Table<VehicleModel> Vehicles;
        public Table<FillupModel> Fillups;
        public Table<ReminderModel> Reminders;

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

        // Assign handlers for the add and remove operations, respectively.
        public VehicleModel()
        {
            _fillups = new EntitySet<FillupModel>(
                new Action<FillupModel>(this.attach_Fillup),
                new Action<FillupModel>(this.detach_Fillup)
                );
            _fillups.CollectionChanged += delegate { NotifyPropertyChanged("Fillups"); };

            _reminders = new EntitySet<ReminderModel>(
                new Action<ReminderModel>(this.attach_Reminder),
                new Action<ReminderModel>(this.detach_Reminder)
                );
            _reminders.CollectionChanged += delegate { NotifyPropertyChanged("Reminders"); };
        }

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

        // Define the entity set for the collection side of the relationship.
        private EntitySet<ReminderModel> _reminders;

        [Association(Storage = "_reminders", OtherKey = "_vehicleId", ThisKey = "VehicleId")]
        public EntitySet<ReminderModel> Reminders
        {
            get { return this._reminders; }
            set { this._reminders.Assign(value); }
        }

        // Called during an add operation
        private void attach_Reminder(ReminderModel reminder)
        {
            NotifyPropertyChanging("Vehicle");
            reminder.Vehicle = this;
        }

        // Called during a remove operation
        private void detach_Reminder(ReminderModel reminder)
        {
            NotifyPropertyChanging("Vehicle");
            reminder.Vehicle = null;
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
        private double _fillupMPG;
        public double FillupMPG
        {
            get
            {
                return _fillupMPG;
            }
            set
            {
                if(_fillupMPG != value)
                {
                    NotifyPropertyChanging("FillupMPG");
                    _fillupMPG = value;
                    NotifyPropertyChanged("FillupMPG");
                }
            }
        }

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

        [Column]
        internal int _vehicleId;

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

    [Table]
    public class ReminderModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _reminderId;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ReminderId
        {
            get
            {
                return _reminderId;
            }
            set
            {
                NotifyPropertyChanging("ReminderId");
                _reminderId = value;
                NotifyPropertyChanged("ReminderId");
            }
        }

        private string _reminderType = "";
        [Column]
        public string ReminderType
        {
            get
            {
                return _reminderType;
            }
            set
            {
                if (_reminderType != value)
                {
                    NotifyPropertyChanging("ReminderType");
                    _reminderType = value;
                    NotifyPropertyChanged("ReminderType");
                }
            }
        }

        private bool _remindDate;
        [Column]
        public bool RemindDate
        {
            get
            {
                return _remindDate;
            }
            set
            {
                if (_remindDate != value)
                {
                    NotifyPropertyChanging("RemindDate");
                    _remindDate = value;
                    NotifyPropertyChanged("RemindDate");
                }
            }
        }

        private DateTime _remindDateValue;
        [Column]
        public DateTime RemindDateValue
        {
            get
            {
                return _remindDateValue;
            }
            set
            {
                if (_remindDateValue != value)
                {
                    NotifyPropertyChanging("RemindDateValue");
                    _remindDateValue = value;
                    NotifyPropertyChanged("RemindDateValue");
                }
            }
        }

        private bool _remindOdo;
        [Column]
        public bool RemindOdo
        {
            get
            {
                return _remindOdo;
            }
            set
            {
                if (_remindOdo != value)
                {
                    NotifyPropertyChanging("RemindOdo");
                    _remindOdo = value;
                    NotifyPropertyChanged("RemindOdo");
                }
            }
        }

        private double _remindOdoValue;
        [Column]
        public double RemindOdoValue
        {
            get
            {
                return _remindOdoValue;
            }
            set
            {
                if (_remindOdoValue != value)
                {
                    NotifyPropertyChanging("RemindOdoValue");
                    _remindOdoValue = value;
                    NotifyPropertyChanged("RemindOdoValue");
                }
            }
        }

        [Column]
        internal int _vehicleId;

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
