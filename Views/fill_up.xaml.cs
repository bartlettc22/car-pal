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

namespace car_pal.Views
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
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
    }
}