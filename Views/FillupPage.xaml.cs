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
    public partial class FillupPage : PhoneApplicationPage
    {

        private VehicleModel _currentVehicle;
        private FillupModel _fillup;

        public FillupPage()
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
                Welcome_Panel.Visibility = Visibility.Collapsed;
                Form_Panel.Visibility = Visibility.Visible;
            }
            else
            {
                Form_Panel.Visibility = Visibility.Collapsed;
                Welcome_Panel.Visibility = Visibility.Visible;
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
        	// Commit any uncommitted changes. Changes in a bound text box are 
            // normally committed to the data source only when the text box 
            // loses focus. However, application bar buttons do not receive or 
            // change focus because they are not Silverlight controls. 
			// TODO: ONLY NEEDED IF SAVE BUTTON CHANGED TO APPLICATION BAR
            //CommitTextBoxWithFocus();
            if (FillupDate.Value == null)
            {
                MessageBox.Show("Date value required.");
                return;
            }
            if (FillupTime.Value == null)
            {
                MessageBox.Show("Time value required.");
                return;
            }
			if (string.IsNullOrWhiteSpace(FillupOdoInput.Text))
            {
                MessageBox.Show("The odometer reading is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(FillupVolInput.Text))
            {
                MessageBox.Show("The gallons value is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(FillupPriceInput.Text))
            {
                MessageBox.Show("The price per gallon value is required.");
                return;
            }

            float val;
            if (!float.TryParse(FillupOdoInput.Text, out val))
            {
                MessageBox.Show("The odometer reading could not be converted to a number.");
                return;
            };

            if (!float.TryParse(FillupVolInput.Text, out val))
            {
                MessageBox.Show("The gallons value could not be converted to a number.");
                return;
            };

            if (!float.TryParse(FillupPriceInput.Text, out val))
            {
                MessageBox.Show("The price per gallon value could not be converted to a number.");
                return;
            };

            // Everything checks out, lets do it!
            DateTime dT = (DateTime)FillupDate.Value;
            dT = dT.AddHours(FillupTime.Value.Value.Hour);
            dT = dT.AddMinutes(FillupTime.Value.Value.Minute);

            FillupModel _newFillup = new FillupModel();
            _newFillup.FillupDate = (DateTime)dT;
            _newFillup.PriceReading = double.Parse(FillupPriceInput.Text);
            _newFillup.VolReading = double.Parse(FillupVolInput.Text);
            _newFillup.OdoReading = double.Parse(FillupOdoInput.Text);

            // Need to add logic for editing...
            App.ViewModel.DefaultVehicle.Fillups.Add(_newFillup);
            App.ViewModel.SaveChangesToDB();

            //Debug.WriteLine("Date: " + dT.Value.Month + "/" + dT.Value.Day + "/" + dT.Value.Year + " " + dT.Value.Hour + ":" + dT.Value.Minute + ":" + dT.Value.Second);


            //_currentVehicle.addFillup(_currentFillup);
			
			
			/*
			SaveResult result = CarDataStore.SaveFillup(_currentFillup,
                delegate
                {
                    MessageBox.Show("There is not enough space on your phone to " +
                    "save your fill-up data. Free some space and try again.");
                });

            if (result.SaveSuccessful)
            {
                Microsoft.Phone.Shell.PhoneApplicationService.Current
                    .State[Constants.FILLUP_SAVED_KEY] = true;
                NavigationService.GoBack();
            }
            else
            {
                string errorMessages = String.Join(
                    Environment.NewLine + Environment.NewLine,
                    result.ErrorMessages.ToArray());
                if (!String.IsNullOrEmpty(errorMessages))
                {
                    MessageBox.Show(errorMessages,
                        "Warning: Invalid Values", MessageBoxButton.OK);
                }
            }*/
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