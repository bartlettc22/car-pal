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
using System.Windows.Navigation;
using System.Windows.Shapes;
using car_pal.Models;
using Microsoft.Phone.Controls;
using System.Diagnostics;

namespace car_pal.Views
{
    public partial class EditLogbook : PhoneApplicationPage
    {

        private VehicleModel _currentVehicle;
        private FillupModel _fillup;

        public EditLogbook()
        {
            InitializeComponent();

            // Set the page DataContext property to the ViewModel.
            DataContext = App.ViewModel;

            _fillup = new FillupModel();
        }

        /// <summary>
        /// Called when navigating to this page; loads the car data from storage 
        /// and then initializes the page state.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);


            if (App.ViewModel.AllVehicles.Count > 0)
            {
                //Welcome_Panel.Visibility = Visibility.Collapsed;
                //Form_Panel.Visibility = Visibility.Visible;
            }
            else
            {
                //Form_Panel.Visibility = Visibility.Collapsed;
                //Welcome_Panel.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// Initializes the view and its data context. 
        /// </summary>
        private void InitializePageState()
        {
            //GarageModel garage = DataStore.Garage;
            //_currentVehicle = garage.DefaultVehicle;
            //VehicleName.DataContext = _currentVehicle;
            //DataContext = _currentFillup = new zFillupModel();

            //_hasUnsavedChanges = State.ContainsKey(HAS_UNSAVED_CHANGES_KEY) && (bool)State[HAS_UNSAVED_CHANGES_KEY];

        }

        private void fillup_cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.GoBack();
        }

        private void fillup_save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			NavigationService.GoBack();
        }

        private void GarageAppBarButton_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/GaragePage.xaml", UriKind.Relative));
        }

        private void FillupPriceInput_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            // Format currency field
        }

        private void HomeAppBarButton_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/MainPage.xaml", UriKind.Relative));
        }
    }
}