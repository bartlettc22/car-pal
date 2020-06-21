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
using System.Device.Location;
using Microsoft.Phone.Controls.Maps.Platform;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps;
using car_pal.BingService;
using car_pal.ViewModel;
using System.Diagnostics;

namespace car_pal.Views
{
    public partial class StationPage : PhoneApplicationPage
    {

        GeoCoordinateWatcher watcher;
        bool _initialized = false;

        GeoPositionStatus _gpsStatus;
        double _currentLatitude;
        double _currentLongitude;
        GeoCoordinate _currentLocation;
        StationItemViewModel _currentlySelectedItem;

        public StationPage()
        {
            InitializeComponent();
            
            // The watcher variable was previously declared as type GeoCoordinateWatcher. 
            if (watcher == null)
            {
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                watcher.MovementThreshold = 20; // use MovementThreshold to ignore noise in the signal

                watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            }

            watcher.Start();
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
                        Debug.WriteLine("Location service disabled.");
                    }
                    else
                    {
                        Debug.WriteLine("Location service not functioning.");
                    }
                    break;

                case GeoPositionStatus.Initializing:
                    // The Location Service is initializing.
                    Debug.WriteLine("Location service intitializing.");
                    break;

                case GeoPositionStatus.NoData:
                    // The Location Service is working, but it cannot get location data.
                    Debug.WriteLine("Location service data not currently available.");
                    break;

                case GeoPositionStatus.Ready:
                    // The Location Service is working and is receiving location data.
                    Debug.WriteLine("Location service data available.");
                    break;
            }
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Debug.WriteLine("GPS Position Changed (" + e.Position.Location.Latitude.ToString("0.000") + "," + e.Position.Location.Longitude.ToString("0.000") + ")");
            //latText.Text = e.Position.Location.Latitude.ToString("0.000");
            //longText.Text = e.Position.Location.Longitude.ToString("0.000");
            //double zoomLevel = double.Parse(s[2], NumberStyles.Float, CultureInfo.InvariantCulture);

            _currentLatitude = e.Position.Location.Latitude;
            _currentLongitude = e.Position.Location.Longitude;

            if (!_initialized)
            {
                _initialized = true;
                refresh();                
            }
        }

        private void refresh()
        {
            Debug.WriteLine("Refreshing gas station display");
            _currentLocation = new GeoCoordinate(_currentLatitude, _currentLongitude);
            miniMap.SetView(_currentLocation, 12);
            search();
        }

        // Click the event handler for the “Start Location” button.
        private void stopLocationButton_Click(object sender, RoutedEventArgs e)
        {
            watcher.Stop();
        }

        private void search()
        {
            miniMap.Focus();

            SearchRequest request = new SearchRequest();
            request.AppId = "FA93274CE521C318F2046C747A9E70B70111E284";
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
            bingClient.SearchAsync(request);
       
            /*string query_string = "1670 Broadway";
            //GeocodeServiceClient _svc = new GeocodeServiceClient();

            GeocodeRequest geocodeRequest = new GeocodeRequest
            {
                Credentials = new Credentials
                {
                    
                    ApplicationId =
                        "ApnhK03M5oUXUrgPWrlLdFh_Tjkss5XkQusvWd7KPt9q5qJIrQx400CVgu9Im1TS"
                },
                Query = query_string
            };

            var filters = new FilterBase[1];
            filters[0] = new ConfidenceFilter { MinimumConfidence = Confidence.High };
            var geocodeOptions = new GeocodeOptions { Filters = new ObservableCollection<FilterBase>(filters) };
            geocodeRequest.Options = geocodeOptions;
            var geocodeService = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            geocodeService.GeocodeCompleted += doneSearching;
            geocodeService.GeocodeAsync(geocodeRequest);*/

            //Geocoder geocoder = new Geocoder(BingToken);
        }

        private void doneSearching(object sender, SearchCompletedEventArgs e)
        {
            /*ObservableCollection<GeocodeResult> r = new ObservableCollection<GeocodeResult>();
            
            GeocodeResponse geocodeResponse = e.Result;
            r = geocodeResponse.Results;
            SearchResults.ItemsSource = r;*/

            miniMap.Children.Clear();

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
            if (_currentlySelectedItem != null)
            {
                _currentlySelectedItem.TitleColor = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
            }
            station.TitleColor = new SolidColorBrush(Color.FromArgb(0xFF, 0x64, 0x3C, 0x77));
            _currentlySelectedItem = station;

            miniMap.SetView(station.Coordinates, 16);
        }
    }
}