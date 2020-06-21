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
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Keep the splash screen up for a bit
            //System.Threading.Thread.Sleep(2000);

            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
            //appSettings.Clear();

            // Notify this page when the default car is changed
            GarageModel.DefaultVehicleChanged += Garage_DefaultVehicleChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Initialize the page state only if it is not already initialized,
            // and not when the application was deactivated but not tombstoned (returning from being dormant).
            if (DataContext == null)
            {
                InitializePageState();
            }
        }

        private void InitializePageState()
        {
            GarageModel garage = DataStore.Garage;
            if (garage.GarageSize > 0)
            {
                DataContext = DataStore.Garage.DefaultVehicle;
            }
        }

        void Garage_DefaultVehicleChanged(object sender, EventArgs e)
        {
            DataContext = DataStore.Garage.DefaultVehicle;
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

        private void TileLink_Click(object sender, System.EventArgs e)
        {
            // Look to see if the tile already exists and if so, don't try to create again.
            ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("DefaultTitle=FromTile"));

            // Create the tile if we didn't find it already exists.
            if (TileToFind == null)
            {
                // Create the tile object and set some initial properties for the tile.
                // The Count value of 12 will show the number 12 on the front of the Tile. Valid values are 1-99.
                // A Count value of 0 will indicate that the Count should not be displayed.
                StandardTileData NewTileData = new StandardTileData
                {
                    BackgroundImage = new Uri("Red.jpg", UriKind.Relative),
                    Title = "Secondary Tile",
                    Count = 12,
                    BackTitle = "Back of Tile",
                    BackContent = "Welcome to the back of the Tile",
                    BackBackgroundImage = new Uri("Blue.jpg", UriKind.Relative), 
                };

                // Create the tile and pin it to Start. This will cause a navigation to Start and a deactivation of our application.
                ShellTile.Create(new Uri("/FillupPage.xaml", UriKind.Relative), NewTileData);
            }
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

    }
}