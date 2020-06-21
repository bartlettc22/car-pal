using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using car_pal.Models;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace car_pal
{
    public partial class GaragePage : PhoneApplicationPage
    {

        //private GarageModel _garage;

        public GaragePage()
        {
            InitializeComponent();

            // Set the page DataContext property to the ViewModel.
            DataContext = App.ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (App.ViewModel.AllVehicles.Count > 0)
            {
                GarageEmptyNotice.Visibility = Visibility.Collapsed;
                //App.ViewModel.DefaultVehicle.IsDefaultVehicle = true;
                //Debug.WriteLine("Default Vehicle: " + App.ViewModel.DefaultVehicle.VehicleName);
            }
        }

        private void VehicleList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        	//Get the data object that represents the current selected item
            VehicleModel vehicle = (sender as ListBox).SelectedItem as VehicleModel;

            if (vehicle != null)
            {
                App.ViewModel.DefaultVehicle = vehicle;
                //NavigationService.GoBack();
            }
        }

        private void AddVehicleAppBarButton_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/EditVehicle.xaml", UriKind.Relative));
        }

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            VehicleModel vehicle = (sender as Button).DataContext as VehicleModel;
            NavigationService.Navigate(new Uri(string.Format("//Views/EditVehicle.xaml?vehicleId={0}", 
                Uri.EscapeUriString(vehicle.VehicleId.ToString())), UriKind.Relative));
        }

        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	VehicleModel vehicle = (sender as Button).DataContext as VehicleModel;
            if (MessageBox.Show("Are you sure?", "Delete \"" + vehicle.VehicleName + "\"", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                App.ViewModel.DeleteVehicle(vehicle);
                if (App.ViewModel.AllVehicles.Count == 0)
                {
                    GarageEmptyNotice.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
