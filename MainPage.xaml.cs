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

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
            appSettings.Clear();

            GarageModel garage = DataStore.Garage;
            DataStore.SaveGarage();
            if (garage.GarageSize > 0)
            {
                VehicleModel vehicle = garage.DefaultVehicle;
                DataContext = vehicle;
            }

            // Initialize the page state only if it is not already initialized,
            // and not when the application was deactivated but not tombstoned (returning from being dormant).
            if (DataContext == null)
            {

            }
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!App.ViewModel.IsDataLoaded)
            //{
            //    App.ViewModel.LoadData();
           // }
        }

        private void FillupLink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.Navigate(
                new Uri("//Views/FillupPage.xaml", UriKind.Relative));
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
    }
}