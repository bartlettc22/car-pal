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

        private bool _editMode = false;
        private FillupModel _editFillup;

        public FillupPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (App.ViewModel.AllVehicles.Count > 0)
            {
                Welcome_Panel.Visibility = Visibility.Collapsed;
                Form_Panel.Visibility = Visibility.Visible;

                if (!State.ContainsKey("WentToDatePicker") && NavigationContext.QueryString.ContainsKey("fillupId"))
                {
                    _editFillup = App.ViewModel.DefaultFillups.First(f => f.FillupId == int.Parse(NavigationContext.QueryString["fillupId"]));
                    if (_editFillup != null)
                    {
                        // Editing vehicle...
                        _editMode = true;
                        PageTitle_Edit.Visibility = Visibility.Visible;
                        PageTitle_Add.Visibility = Visibility.Collapsed;
                        FillupDeleteButton.Visibility = Visibility.Visible;
                        FillupDate.Value = new DateTime(_editFillup.FillupDate.Year, _editFillup.FillupDate.Month, _editFillup.FillupDate.Day);
                        FillupTime.Value = new DateTime(_editFillup.FillupDate.Year, _editFillup.FillupDate.Month, _editFillup.FillupDate.Day, _editFillup.FillupDate.Hour, _editFillup.FillupDate.Minute, _editFillup.FillupDate.Second);
                        FillupPriceInput.Text = _editFillup.PriceReading.ToString();
                        FillupVolInput.Text = _editFillup.VolReading.ToString();
                        FillupOdoInput.Text = _editFillup.OdoReading.ToString();
                    }
                }
            }
            else
            {
                Form_Panel.Visibility = Visibility.Collapsed;
                Welcome_Panel.Visibility = Visibility.Visible;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.Content is DatePickerPage || e.Content is TimePickerPage)
            {
                State["WentToDatePicker"] = true;
            }

            base.OnNavigatedFrom(e);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Focus hack?
        }

        private void fillupCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.GoBack();
        }

        private void fillupSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
            DateTime dT = (DateTime)FillupDate.Value;
            dT.Subtract(dT.TimeOfDay);
            dT = dT.AddHours(FillupTime.Value.Value.Hour);
            dT = dT.AddMinutes(FillupTime.Value.Value.Minute);

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

            int editId = _editMode ? _editFillup.FillupId : -1;
            if ((from f in App.ViewModel.DefaultFillups where f.FillupDate == dT && f.FillupId != editId select f.FillupId).Count() > 0)
            {
                MessageBox.Show("An entry for this time already exists");
                return;
            }
            if ((from f in App.ViewModel.DefaultFillups where f.FillupDate <= dT && f.OdoReading >= double.Parse(FillupOdoInput.Text) && f.FillupId != editId select f.FillupId).Count() > 0)
            {
                MessageBox.Show("A entry exists for a previous date with a higher odometer reading.  Please check entry and/or fillup history.");
                return;
            }
            if ((from f in App.ViewModel.DefaultFillups where f.FillupDate >= dT && f.OdoReading <= double.Parse(FillupOdoInput.Text) && f.FillupId != editId select f.FillupId).Count() > 0)
            {
                MessageBox.Show("A entry exists for a future date with a lower odometer reading.  Please check entry and/or fillup history.");
                return;
            }

            // Everything checks out, lets do it!
            if (!_editMode)
            {
                FillupModel _newFillup = new FillupModel();
                _newFillup.FillupDate = dT;
                _newFillup.PriceReading = double.Parse(FillupPriceInput.Text);
                _newFillup.VolReading = double.Parse(FillupVolInput.Text);
                _newFillup.OdoReading = double.Parse(FillupOdoInput.Text);

                App.ViewModel.AddFillup(_newFillup);
            }
            else
            {
                _editFillup.FillupDate = dT;
                _editFillup.PriceReading = double.Parse(FillupPriceInput.Text);
                _editFillup.VolReading = double.Parse(FillupVolInput.Text);
                _editFillup.OdoReading = double.Parse(FillupOdoInput.Text);
                App.ViewModel.EditFillup(_editFillup);
            }

            // Return to the main page.
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void GarageAppBarButton_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/GaragePage.xaml", UriKind.Relative));
        }

        private void HomeAppBarButton_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/MainPage.xaml", UriKind.Relative));
        }

        private void FillupDeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_editMode && _editFillup != null && MessageBox.Show("Are you sure?", "Delete fill-up", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {

                App.ViewModel.DeleteFillup(_editFillup);
                
                // Return to the main page.
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
        }
    }
}