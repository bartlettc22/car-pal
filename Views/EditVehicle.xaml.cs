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

        private bool _editMode = false;
        private VehicleModel _editVehicle;

        public EditVehicle()
        {
            InitializeComponent();
            Loaded += SetPageFocus;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            
            if (NavigationContext.QueryString.ContainsKey("vehicleId"))
            {
                _editVehicle = App.ViewModel.AllVehicles.First(v => v.VehicleId == int.Parse(NavigationContext.QueryString["vehicleId"]));
                if(_editVehicle != null)
                {
                    // Editing vehicle...
                    _editMode = true;
                    PageTitle_Edit.Visibility = Visibility.Visible;
                    PageTitle_Add.Visibility = Visibility.Collapsed;
                    VehicleNameInput.Text = _editVehicle.VehicleName;
                    VehicleEditContainer.Visibility = Visibility.Visible;
                    VehicleEditName.Text = _editVehicle.VehicleName;
                }
            }
        }

        private void SetPageFocus(object sender, RoutedEventArgs e)
        {
            VehicleNameInput.Focus();
        }

        private void VehicleCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.GoBack();
        }

        private void VehicleSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            VehicleNameInput.Text = (VehicleNameInput.Text == null) ? VehicleNameInput.Text : VehicleNameInput.Text.Trim();
            bool vehicle_name_exists = (App.ViewModel.AllVehicles.Count(v => v.VehicleName.ToLower() == VehicleNameInput.Text.ToLower()) > 0);


            if (VehicleNameInput.Text == null || VehicleNameInput.Text == "")
            {
                MessageBox.Show("Vehicle name is required.");
                return;
            }
            else if ((!_editMode && vehicle_name_exists) ||
                (_editMode && VehicleNameInput.Text != _editVehicle.VehicleName && vehicle_name_exists))
            {
                MessageBox.Show("Vehicle name already exists.");
                return;
            }

            if (!_editMode)
            {
                // Create a new vehicle
                VehicleModel newVehicle = new VehicleModel
                {
                    VehicleName = VehicleNameInput.Text
                };

                // Add the item to the ViewModel.
                App.ViewModel.AddVehicle(newVehicle);
            }
            else
            {
                _editVehicle.VehicleName = VehicleNameInput.Text;
                App.ViewModel.SaveChangesToDB();
            }

            // Return to the main page.
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
         
        }
    }
}