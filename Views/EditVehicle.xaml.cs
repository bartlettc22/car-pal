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

namespace car_pal.Views
{
    public partial class EditVehicle : PhoneApplicationPage
    {

        private VehicleModel _vehicle;

        public EditVehicle()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (DataContext == null)
            {
                InitializePageState();
            }
        }

        private void InitializePageState()
        {
            _vehicle = new VehicleModel();
            DataContext = _vehicle;
        }

        private void VehicleCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.GoBack();
        }

        private void VehicleSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GarageModel garage = DataStore.Garage;
            garage.addVehicle(_vehicle);
            DataStore.Garage = garage;

            DataStore.SaveGarage();
            NavigationService.GoBack();
        }
    }
}