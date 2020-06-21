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
using System.IO.IsolatedStorage;

namespace car_pal.Models
{
    public static class DataStore
    {
        private const string GARAGE_KEY = "car_pal.Garage";
        private static readonly IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        private static GarageModel _garage;

        //public static event EventHandler GarageUpdated;

        public static GarageModel Garage
        {
            get
            {
                if (_garage == null)
                {
                    if (appSettings.Contains(GARAGE_KEY))
                    {
                        _garage = (GarageModel)appSettings[GARAGE_KEY];
                    }
                    else
                    {
                        _garage = new GarageModel(); ;
                    }
                }
                return _garage;
            }
            set
            {
                _garage = value;
                //NotifyCarUpdated();
            }
        }

        public static void SaveGarage()
        {
            try
            {
                appSettings[GARAGE_KEY] = Garage;
                appSettings.Save();
                //NotifyCarUpdated();
            }
            catch (IsolatedStorageException)
            {
                MessageBox.Show("Error saving data to device.");
            }
        }
    }
}
