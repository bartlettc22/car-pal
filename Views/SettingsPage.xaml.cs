using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.Controls;

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
            } else 
            {
                LocationSwitch.IsChecked = false;
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
