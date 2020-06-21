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
        private bool _editMode = false;
        private VehicleModel _editVehicle;

        public EditVehicle()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _vehicle = new VehicleModel();

            // Editing vehicle...
            if (NavigationContext.QueryString.ContainsKey("vehicleName"))
            {
                _editVehicle = DataStore.Garage.getVehicle(NavigationContext.QueryString["vehicleName"]);
                if(_editVehicle != null)
                {
                    _editMode = true;
                    _vehicle.Name = _editVehicle.Name;
                }
            }

            DataContext = _vehicle;
        }

        private void VehicleCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.GoBack();
        }

        private void VehicleSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _vehicle.Name = (_vehicle.Name == null)?_vehicle.Name:_vehicle.Name.Trim();

            if (_vehicle.Name == null || _vehicle.Name == "")
            {
                MessageBox.Show("Vehicle name is required.");
                return;
            }
            else if((!_editMode && DataStore.Garage.vehicleExists(_vehicle.Name)) ||
                (_editMode && _vehicle.Name != _editVehicle.Name && DataStore.Garage.vehicleExists(_vehicle.Name)))
            {
                MessageBox.Show("Vehicle name already exists.");
                return;
            }
            

            if (_editMode == false)
            {
                DataStore.Garage.addVehicle(_vehicle);
            }
            else
            {
                _editVehicle.Name = _vehicle.Name;
            }

            DataStore.SaveGarage();
            NavigationService.GoBack();
        }
    }
}