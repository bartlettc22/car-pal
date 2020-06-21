using System;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using car_pal.BingService;
using car_pal.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;

namespace car_pal.Views
{
    public partial class StationPage : PhoneApplicationPage
    {

        GeoCoordinateWatcher watcher;

        GeoPositionStatus _gpsStatus;
        double _currentLatitude;
        double _currentLongitude;
        GeoCoordinate _currentLocation;

        public StationPage()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SearchLoading.Text = "Loading...";
            SearchResults.Visibility = Visibility.Collapsed;
            SearchLoading.Visibility = Visibility.Visible;
            refresh();
        }

        private void StartWatcher()
        {
            if (watcher == null)
            {
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                watcher.MovementThreshold = 20; // use MovementThreshold to ignore noise in the signal

                watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
                //watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            }
            
            watcher.Start();
        }

        private bool InternetIsAvailable()
        {
             if (!NetworkInterface.GetIsNetworkAvailable())
             {
                 return false;
             }
             return true;
        }

        // Event handler for the GeoCoordinateWatcher.StatusChanged event.
        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {

            _gpsStatus = e.Status;

            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    // The Location Service is disabled or unsupported.
                    // Check to see whether the user has disabled the Location Service.
                    if (watcher.Permission == GeoPositionPermission.Denied)
                    {
                        // The user has disabled the Location Service on their device.
                        MessageText.Text = "Location services disabled. To use this feature, turn on location from the phone settings menu.";
                        MapPanel.Visibility = Visibility.Collapsed;
                        AppLocationDisabledMessage.Visibility = Visibility.Collapsed;
                        MessagePanel.Visibility = Visibility.Visible;
                        MessageBox.Show("Location services disabled.");
                        watcher.Stop();
                    }
                    else
                    {
                        // Location service is not functioning (not supported?)
                        MessageText.Text = "Location services disabled. To use this feature, turn on location from the phone settings menu.";
                        MapPanel.Visibility = Visibility.Collapsed;
                        AppLocationDisabledMessage.Visibility = Visibility.Collapsed;
                        MessagePanel.Visibility = Visibility.Visible;
                        MessageBox.Show("Location services disabled.");
                        watcher.Stop();
                    }
                    break;

                case GeoPositionStatus.Initializing:
                    // The Location Service is initializing.
                    break;

                case GeoPositionStatus.NoData:
                    // The Location Service is working, but it cannot get location data.
                    MessageText.Text = "Location cannot be aquired. Please try again.";
                    MapPanel.Visibility = Visibility.Collapsed;
                    AppLocationDisabledMessage.Visibility = Visibility.Collapsed;
                    MessagePanel.Visibility = Visibility.Visible;
                    MessageBox.Show("Location cannot be aquired.");
                    watcher.Stop();
                    break;

                case GeoPositionStatus.Ready:
                    // The Location Service is working and is receiving location data.
                    refreshData();
                    break;
            }
        }

       /* void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //Debug.WriteLine("GPS Position Changed (" + e.Position.Location.Latitude.ToString("0.000") + "," + e.Position.Location.Longitude.ToString("0.000") + ")");
            //latText.Text = e.Position.Location.Latitude.ToString("0.000");
            //longText.Text = e.Position.Location.Longitude.ToString("0.000");
            //double zoomLevel = double.Parse(s[2], NumberStyles.Float, CultureInfo.InvariantCulture);

            _currentLatitude = e.Position.Location.Latitude;
            _currentLongitude = e.Position.Location.Longitude;

            if (!_initialized)
            {
                _currentLocation = new GeoCoordinate(_currentLatitude, _currentLongitude);
                miniMap.SetView(_currentLocation, 12);
                _initialized = true;
                refresh();                
            }
            
        }*/
        private void refreshData()
        {
            MessagePanel.Visibility = Visibility.Collapsed;
            MapPanel.Visibility = Visibility.Visible;
            GeoPosition<GeoCoordinate> geoPosition = watcher.Position;
            _currentLocation = geoPosition.Location;
            _currentLatitude = geoPosition.Location.Latitude;
            _currentLongitude = geoPosition.Location.Longitude;
            miniMap.SetView(geoPosition.Location, 12);
            search();
        }

        private void refresh()
        {
            if (!SettingsPage.appSettings.Contains(SettingsPage.LOCATION_SWITCH_KEY))
            {
                if (MessageBox.Show("car-pal+ requires your location to provide nearby gas station locations.\n\nAllow car-pal+ to access and use your location?", "Allow this application to use your location?",
                MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    try
                    {
                        SettingsPage.appSettings[SettingsPage.LOCATION_SWITCH_KEY] = true;
                        SettingsPage.appSettings.Save();
                    }
                    catch (IsolatedStorageException)
                    {
                        MessageBox.Show("Error saving data to device.");
                    }
                }
            }
            if (!SettingsPage.appSettings.Contains(SettingsPage.LOCATION_SWITCH_KEY) ||
                (bool)SettingsPage.appSettings[SettingsPage.LOCATION_SWITCH_KEY] == false)
            {
                MessageText.Text = "Location services disabled in application settings. To use this feature, turn on location from the applications settings menu.";
                MapPanel.Visibility = Visibility.Collapsed;
                MessagePanel.Visibility = Visibility.Collapsed;
                AppLocationDisabledMessage.Visibility = Visibility.Visible;
                MessageBox.Show("Location services disabled in application settings.");
            }
            else
            {
                if (InternetIsAvailable())
                {
                    if (watcher != null && watcher.Status == GeoPositionStatus.Ready)
                    {
                        refreshData();
                    }
                    else
                    {
                        StartWatcher();
                    }
                }
                else
                {
                    MessageText.Text = "No internet connection available. Please connect to mobile or Wi-Fi network and try again.";
                    MapPanel.Visibility = Visibility.Collapsed;
                    AppLocationDisabledMessage.Visibility = Visibility.Collapsed;
                    MessagePanel.Visibility = Visibility.Visible;
                    MessageBox.Show("No internet connection available.  Try again later.");
                }
            }
        }

        private void search()
        {
            miniMap.Focus();

            SearchRequest request = new SearchRequest();
            request.AppId = "18125912B7049CCC5E6F484E5617E5D834983119";
            request.Sources = new SourceType[] { SourceType.Phonebook };
            request.Query = "gas station";

            request.Market = "en-us";
            request.UILanguage = "en";
            request.Longitude = _currentLocation.Longitude;
            request.LongitudeSpecified = true;
            request.Latitude = _currentLocation.Latitude;
            request.LatitudeSpecified = true;
            request.Radius = 10.0;
            request.RadiusSpecified = true;
            request.Phonebook = new PhonebookRequest();
            request.Phonebook.Count = 10;
            request.Phonebook.SortBy = PhonebookSortOption.Distance;

            BingPortTypeClient bingClient = new BingPortTypeClient();
            bingClient.SearchCompleted += doneSearching;
            SearchLoading.Text = "Loading...";
            SearchResults.Visibility = Visibility.Collapsed;
            SearchLoading.Visibility = Visibility.Visible;
            bingClient.SearchAsync(request);
        }

        private void doneSearching(object sender, SearchCompletedEventArgs e)
        {

            if (e.Result.Errors != null)
            {
                SearchLoading.Text = "Error fetching data...";
                return;
            }
            
            
            miniMap.Children.Clear();

            if (e.Result.Phonebook == null)
            {
                SearchLoading.Text = "No results nearby...";
                return;
            }

            ObservableCollection<StationItemViewModel> resultsModel = new ObservableCollection<StationItemViewModel>();
            PhonebookResult[] r = e.Result.Phonebook.Results;

            SolidColorBrush pinBrush = new SolidColorBrush(Color.FromArgb(255,85,44,105));

            // Add the pushpins
            for (int i = 0; i < r.Length; i++)
            {
                Pushpin pin = new Pushpin();
                pin.Location = new GeoCoordinate(r[i].Latitude, r[i].Longitude);
                pin.Background = pinBrush;
                pin.Content = i + 1;
                pin.Height = 60;
                pin.Width = 30;
                miniMap.Children.Add(pin);

                // Add it to our results model
                StationItemViewModel station = new StationItemViewModel();
                station.Title = r[i].Title;
                station.Address = r[i].Address + ", " + r[i].City + ", " + r[i].StateOrProvince;
                station.PhoneNumber = r[i].PhoneNumber;
                station.ItemNumber = i + 1;
                station.Coordinates = pin.Location;
                station.Distance = distanceApart(_currentLocation, pin.Location).ToString();
                resultsModel.Add(station);
            }

            SearchResults.ItemsSource = resultsModel;
            SearchLoading.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Visible;
        }

        private double distanceApart(GeoCoordinate p1, GeoCoordinate p2)
        {
            var R = 3963.1676; // km
            var dLat = degreesToRadians(p2.Latitude - p1.Latitude);
            var dLon = degreesToRadians(p2.Longitude - p1.Longitude);
            var lat1 = degreesToRadians(p1.Latitude);
            var lat2 = degreesToRadians(p2.Latitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;

            return d;
        }

        private double degreesToRadians(double angle)
        {
            return Math.PI * angle / 180;
        }

        private void RefreshButton_Click(object sender, System.EventArgs e)
        {
            refresh();
        }

        private void HomeAppBarButton_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/MainPage.xaml", UriKind.Relative));
        }

        private void SearchResults_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            StationItemViewModel station = (sender as ListBox).SelectedItem as StationItemViewModel;
            //ListBoxItem Item = (sender as ListBox).SelectedItem as ListBoxItem;

            if (station != null)
            {
                miniMap.SetView(station.Coordinates, 16);
            }
        }

        private void SettingsBarMenuItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("//Views/SettingsPage.xaml", UriKind.Relative));
        }
    }
}