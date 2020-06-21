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
using System.Windows.Navigation;
using car_pal.Models;

namespace car_pal
{
    public partial class GaragePage : PhoneApplicationPage
    {

        private GarageModel _garage;

        public GaragePage()
        {
            InitializeComponent();
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

            // Delete temporary storage to avoid unnecessary storage costs.
            State.Clear();
        }

        private void InitializePageState()
        {
            DataContext = _garage = DataStore.Garage;
        }

        private void VehicleList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        	//Get the data object that represents the current selected item
            VehicleModel vehicle = (sender as ListBox).SelectedItem as VehicleModel;

            if (vehicle != null)
            {
                _garage = DataStore.Garage;
                _garage.DefaultVehicleIndex = _garage.getVehicleIndexByName(vehicle.Name);
                DataStore.SaveGarage();

                NavigationService.GoBack();
            }
        }

        private void AddVehicleAppBarButton_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/EditVehicle.xaml", UriKind.Relative));
        }

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            VehicleModel vehicle = (sender as Button).DataContext as VehicleModel;
            NavigationService.Navigate(new Uri(string.Format("//Views/EditVehicle.xaml?vehicleName={0}", 
                Uri.EscapeUriString(vehicle.Name)), UriKind.Relative));
        }

        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	VehicleModel vehicle = (sender as Button).DataContext as VehicleModel;
            if (MessageBox.Show("Are you sure?", "Delete \"" + vehicle.Name + "\"", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                _garage.deleteVehicle(vehicle);
                DataStore.SaveGarage();
                InitializePageState();
            }
        }
    }
}
