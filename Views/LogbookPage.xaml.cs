using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using car_pal.Models;
using car_pal.ViewModel;
using Microsoft.Phone.Controls;

namespace car_pal
{
    public partial class LogbookPage : PhoneApplicationPage
    {
        public LogbookPage()
        {
            InitializeComponent();

            // Set the page DataContext property to the Main ViewModel.
            DataContext = App.ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            if (App.ViewModel.AllVehicles.Count > 0)
            {
                Welcome_Panel.Visibility = Visibility.Collapsed;
                Form_Panel.Visibility = Visibility.Visible;
                InitializePage();
            }
            else
            {
                Form_Panel.Visibility = Visibility.Collapsed;
                Welcome_Panel.Visibility = Visibility.Visible;
                DefaultHelp.Visibility = Visibility.Collapsed;
            }
        }

        private void InitializePage()
        {

            if (App.ViewModel.DefaultVehicle.Reminders.Count(r => (r.RemindDate && r.RemindOdo)) > 0)
            {
                DefaultHelp.Visibility = Visibility.Visible;
            }
            else
            {
                DefaultHelp.Visibility = Visibility.Collapsed;
            }

            ObservableCollection<ReminderItemViewModel> resultsModel = new ObservableCollection<ReminderItemViewModel>();
            foreach(ReminderModel r in App.ViewModel.DefaultVehicle.Reminders)
            {
                ReminderItemViewModel ReminderView = new ReminderItemViewModel(r);
                
                resultsModel.Add(ReminderView);
            }
            ResultsList.ItemsSource = resultsModel;
        }

        private void HomeAppBarButton_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/MainPage.xaml", UriKind.Relative));
        }

        private void GarageAppBarButton_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/GaragePage.xaml", UriKind.Relative));
        }

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ReminderItemViewModel reminder = (sender as Button).DataContext as ReminderItemViewModel;

            if (reminder != null)
            {
                NavigationService.Navigate(new Uri(string.Format("//Views/EditLogbook.xaml?reminderId={0}",
                    Uri.EscapeUriString(reminder.ReminderId.ToString())), UriKind.Relative));
            }
        }

        private void ResultsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ReminderItemViewModel reminder = (sender as ListBox).SelectedItem as ReminderItemViewModel;
            if (reminder != null)
            {
                // unset the selection
                (sender as ListBox).SelectedItem = null;
                reminder.ShowAlt();
            }
        }
    }
}
