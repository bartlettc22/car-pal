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
using car_pal.Models;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;

namespace car_pal
{
    public partial class MainPage : PhoneApplicationPage
    {

        bool _fillupAlt = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the page DataContext property to the ViewModel.
            DataContext = App.ViewModel;

            // Keep the splash screen up for a bit
            //System.Threading.Thread.Sleep(2000);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ShellTile TileFindFillup = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("FillupPage"));
            ShellTile TileFindStation = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("StationPage"));
            ShellTile TileFindLogbook = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("LogbookPage"));
            if (TileFindFillup != null)
            {
                PinLinkFillup.Header = "remove from start";
            }
            if (TileFindStation != null)
            {
                PinLinkStation.Header = "remove from start";
            }
            if (TileFindLogbook != null)
            {
                PinLinkLogbook.Header = "remove from start";
            }

            if (App.ViewModel.AllVehicles.Count > 0)
            {
                //DataContext = DataStore.Garage.DefaultVehicle;
                Welcome_Panel.Visibility = Visibility.Collapsed;
                DashboardVehicleNameDisplay.Visibility = Visibility.Visible;
                Dashboard_Panel.Visibility = Visibility.Visible;
                FillupVehicleNameDisplay.Visibility = Visibility.Visible;
            }
            else
            {
                DashboardVehicleNameDisplay.Visibility = Visibility.Collapsed;
                Dashboard_Panel.Visibility = Visibility.Collapsed;
                FillupVehicleNameDisplay.Visibility = Visibility.Collapsed;
                Welcome_Panel.Visibility = Visibility.Visible;
            }

            /*GarageModel garage = DataStore.Garage;
            _fillupAlt = false;
            if (garage.GarageSize > 0)
            {
                DataContext = DataStore.Garage.DefaultVehicle;
                Welcome_Panel.Visibility = Visibility.Collapsed;
                Dashboard_Panel.Visibility = Visibility.Visible;
            }
            else
            {
                Dashboard_Panel.Visibility = Visibility.Collapsed;
                Welcome_Panel.Visibility = Visibility.Visible;
            }

            // Initialize the page state only if it is not already initialized,
            // and not when the application was deactivated but not tombstoned (returning from being dormant).
            if (DataContext == null)
            {
                //InitializePageState();
            }*/
        }

        private void InitializePageState()
        {

        }

        void Garage_DefaultVehicleChanged(object sender, EventArgs e)
        {
            //DataContext = DataStore.Garage.DefaultVehicle;
        }

        private void FillupLink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/FillupPage.xaml", UriKind.Relative));
        }

        private void GarageAppBarButton_Click(object sender, System.EventArgs e)
        {
		    NavigationService.Navigate(new Uri("//Views/GaragePage.xaml", UriKind.Relative));
			
			/*
			// Create a popup.
			Popup p = new Popup();
		
			// Set the Child property of Popup to an instance of MyControl.
			p.Child = new VehicleSelectionPopup();
		
			// Set where the popup will show up on the screen.
			p.VerticalOffset = 200;
			p.HorizontalOffset = 200;
		
			// Open the popup.
			p.IsOpen = true; */
        }

        private void GasStationLink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("//Views/StationPage.xaml", UriKind.Relative));
        }

        private void LogbookLink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/LogbookPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/DebugPage.xaml", UriKind.Relative));
        }

        private void PinTile_Click(object sender, System.EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            if ((string)item.Tag == "fillup")
            {
                ShellTile TileFindFillup = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("FillupPage"));
                if (TileFindFillup == null)
                {
                    StandardTileData NewTileData = new StandardTileData
                    {
                        BackgroundImage = new Uri("/Images/TileFillupBackground.png", UriKind.Relative),
                        Title = "Fill-up",
                        BackTitle = "car-pal+",
                        BackBackgroundImage = new Uri("/Images/TileFillupBackground.png", UriKind.Relative)
                    };

                    ShellTile.Create(new Uri("/Views/FillupPage.xaml", UriKind.Relative), NewTileData);
                }
                else
                {
                    TileFindFillup.Delete();
                    PinLinkFillup.Header = "pin to start";
                }
            }
            else if ((string)item.Tag == "gas_station")
            {
                ShellTile TileFindStation = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("StationPage"));
                if (TileFindStation == null)
                {
                    StandardTileData NewTileData = new StandardTileData
                    {
                        BackgroundImage = new Uri("/Images/TileStationBackground.png", UriKind.Relative),
                        Title = "Find Gas Station",
                        BackTitle = "car-pal+",
                        BackBackgroundImage = new Uri("/Images/TileStationBackground.png", UriKind.Relative)
                    };

                    ShellTile.Create(new Uri("/Views/StationPage.xaml", UriKind.Relative), NewTileData);
                }
                else
                {
                    TileFindStation.Delete();
                    PinLinkStation.Header = "pin to start";
                }
            }
            else if ((string)item.Tag == "logbook")
            {
                ShellTile TileFindLogbook = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("LogbookPage"));
                if (TileFindLogbook == null)
                {
                    StandardTileData NewTileData = new StandardTileData
                    {
                        BackgroundImage = new Uri("/Images/TileLogbookBackground.png", UriKind.Relative),
                        Title = "Logbook",
                        BackTitle = "car-pal+",
                        BackBackgroundImage = new Uri("/Images/TileLogbookBackground.png", UriKind.Relative)
                    };

                    ShellTile.Create(new Uri("/Views/LogbookPage.xaml", UriKind.Relative), NewTileData);
                }
                else
                {
                    TileFindLogbook.Delete();
                    PinLinkLogbook.Header = "pin to start";
                }
            }
        }

        private void HistoryItem_Loaded(object sender, RoutedEventArgs e)
        {

            Grid ItemRef = sender as Grid;    

            //SolidColorBrush brush1 = new SolidColorBrush(Color.FromArgb(200,221,121,100));

            if (_fillupAlt)
            {
                ItemRef.Background.Opacity -= 0.2;
            }

            _fillupAlt = !_fillupAlt;
        }

        private void SettingsBarMenuItem_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/SettingsPage.xaml", UriKind.Relative));
        }

        private void FillupList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FillupModel fillup = (sender as Button).DataContext as FillupModel;
            NavigationService.Navigate(new Uri(string.Format("//Views/FillupPage.xaml?fillupId={0}", 
                Uri.EscapeUriString(fillup.FillupId.ToString())), UriKind.Relative));
        }
    }
}