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
        private ReminderModel _editReminder;

        public EditLogbook()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!State.ContainsKey("WentToDatePicker") && NavigationContext.QueryString.ContainsKey("reminderId"))
            {
                _editReminder = App.ViewModel.DefaultVehicle.Reminders.First(r => r.ReminderId == int.Parse(NavigationContext.QueryString["reminderId"]));
                if (_editReminder != null)
                {
                    ReminderName.Text = _editReminder.ReminderType;
                    ReminderDate.IsChecked = _editReminder.RemindDate;
                    ReminderDateValue.Value = _editReminder.RemindDateValue;
                    ReminderOdo.IsChecked = _editReminder.RemindOdo;
                    ReminderOdoValue.Text = (_editReminder.RemindOdoValue == 0)?"":_editReminder.RemindOdoValue.ToString();
                }
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

        private void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_editReminder != null)
            {
                if ((bool)ReminderDate.IsChecked && ReminderDateValue.Value == null)
                {
                    MessageBox.Show("Date value required.");
                    return;
                }
                if ((bool)ReminderOdo.IsChecked && string.IsNullOrWhiteSpace(ReminderOdoValue.Text))
                {
                    MessageBox.Show("Mileage Due is required.");
                    return;
                }

                double val;
                if ((bool)ReminderOdo.IsChecked && !double.TryParse(ReminderOdoValue.Text, out val))
                {
                    MessageBox.Show("The Mileage Due could not be converted to a number.");
                    return;
                };

                _editReminder.RemindDate = (bool)ReminderDate.IsChecked;
                _editReminder.RemindDateValue = (DateTime)ReminderDateValue.Value;
                _editReminder.RemindOdo = (bool)ReminderOdo.IsChecked;
                if (_editReminder.RemindOdo)
                {
                    _editReminder.RemindOdoValue = double.Parse(ReminderOdoValue.Text);
                }

                App.ViewModel.saveDefault();
            }
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

    }
}