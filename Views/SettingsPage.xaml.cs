using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.Diagnostics;

namespace car_pal
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public const string LOCATION_SWITCH_KEY = "car_pal.LocationSwitch";
        public static readonly IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

        public SettingsPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(InitializeSettings);
        }

        private void InitializeSettings(object sender, RoutedEventArgs e)
        {
            if(appSettings.Contains(LOCATION_SWITCH_KEY))
            {
                LocationSwitch.IsChecked = (bool?)appSettings[LOCATION_SWITCH_KEY];
                Debug.WriteLine("Init: loading value from storage:"+LocationSwitch.ToString());
            } else 
            {
                LocationSwitch.IsChecked = false;
                Debug.WriteLine("Init: no value... setting to default:" + LocationSwitch.ToString());
            }
        }

        private void LocationSwitch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			
            ToggleSwitch locationSwitch = sender as ToggleSwitch;

            bool? locationAllowed = false;
            if (locationSwitch.IsChecked != null && locationSwitch.IsChecked == true)
            {
                locationAllowed = true;
            }
            Debug.WriteLine("Saving value: " + LocationSwitch.ToString());

            try
            {
                appSettings[LOCATION_SWITCH_KEY] = locationAllowed;
                appSettings.Save();
            }
            catch (IsolatedStorageException)
            {
                MessageBox.Show("Error saving data to device.");
            }
        }
    }
}
