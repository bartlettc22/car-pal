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
using System.Windows.Controls.Primitives;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;
using System.ComponentModel;

namespace car_pal
{
    public partial class MainPage : PhoneApplicationPage
    {   

        bool _fillupAlt = false;

        public MainPage()
        {
            InitializeComponent();

            // Set the page DataContext property to the Main ViewModel.
            DataContext = App.ViewModel;

            // Notify us when the fillup history changes so we can update the UI alternating colors
            //App.ViewModel.PropertyChanged += UpdateFillupHistoryVisual;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Reset alternating fillup colors
            _fillupAlt = false;

            Loaded += new RoutedEventHandler(OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ShellTile TileFindFillup = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("FillupPage"));
            ShellTile TileFindStation = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("StationPage"));
            ShellTile TileFindLogbook = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("LogbookPage"));
            if (TileFindFillup != null)
            {
                PinLinkFillup.Header = "remove from start";
            }
            if (TileFindStation != null)
            {
                PinLinkStation.Header = "remove from start";
            }
            if (TileFindLogbook != null)
            {
                PinLinkLogbook.Header = "remove from start";
            }

            if (App.ViewModel.AllVehicles.Count > 0)
            {
                Welcome_Panel.Visibility = Visibility.Collapsed;
                DashboardVehicleNameDisplay.Visibility = Visibility.Visible;
                Dashboard_Panel.Visibility = Visibility.Visible;
                FillupVehicleNameDisplay.Visibility = Visibility.Visible;
            }
            else
            {
                DashboardVehicleNameDisplay.Visibility = Visibility.Collapsed;
                Dashboard_Panel.Visibility = Visibility.Collapsed;
                FillupVehicleNameDisplay.Visibility = Visibility.Collapsed;
                Welcome_Panel.Visibility = Visibility.Visible;
            }
        }

        private void PinTile_Click(object sender, System.EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            if ((string)item.Tag == "fillup")
            {
                ShellTile TileFindFillup = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("FillupPage"));
                if (TileFindFillup == null)
                {
                    StandardTileData NewTileData = new StandardTileData
                    {
                        BackgroundImage = new Uri("/Images/PinnedTileFillupBg.png", UriKind.Relative),
                        Title = "Fill-up",
                        BackTitle = "car-pal+",
                        BackBackgroundImage = new Uri("/Images/PinnedTileFillupBg.png", UriKind.Relative)
                    };

                    ShellTile.Create(new Uri("/Views/FillupPage.xaml", UriKind.Relative), NewTileData);
                }
                else
                {
                    TileFindFillup.Delete();
                    PinLinkFillup.Header = "pin to start";
                }
            }
            else if ((string)item.Tag == "gas_station")
            {
                ShellTile TileFindStation = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("StationPage"));
                if (TileFindStation == null)
                {
                    StandardTileData NewTileData = new StandardTileData
                    {
                        BackgroundImage = new Uri("/Images/PinnedTileStationBg.png", UriKind.Relative),
                        Title = "Find Gas Station",
                        BackTitle = "car-pal+",
                        BackBackgroundImage = new Uri("/Images/PinnedTileStationBg.png", UriKind.Relative)
                    };

                    ShellTile.Create(new Uri("/Views/StationPage.xaml", UriKind.Relative), NewTileData);
                }
                else
                {
                    TileFindStation.Delete();
                    PinLinkStation.Header = "pin to start";
                }
            }
            else if ((string)item.Tag == "logbook")
            {
                ShellTile TileFindLogbook = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("LogbookPage"));
                if (TileFindLogbook == null)
                {
                    StandardTileData NewTileData = new StandardTileData
                    {
                        BackgroundImage = new Uri("/Images/PinnedTileLogbookBg.png", UriKind.Relative),
                        Title = "Logbook",
                        BackTitle = "car-pal+",
                        BackBackgroundImage = new Uri("/Images/PinnedTileLogbookBg.png", UriKind.Relative)
                    };

                    ShellTile.Create(new Uri("/Views/LogbookPage.xaml", UriKind.Relative), NewTileData);
                }
                else
                {
                    TileFindLogbook.Delete();
                    PinLinkLogbook.Header = "pin to start";
                }
            }
        }

        private void HistoryItem_Loaded(object sender, RoutedEventArgs arg)
        {

            Grid ItemRef = sender as Grid;    

            if (_fillupAlt)
            {
                //ItemRef.Background.Opacity -= 0.2;
                ItemRef.Background = new SolidColorBrush(Color.FromArgb(145, 231, 121, 54));
            }
            else
            {
                ItemRef.Background = new SolidColorBrush(Color.FromArgb(200, 231, 121, 54));
            }

            _fillupAlt = !_fillupAlt;
        }

        private void FillupList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FillupModel fillup = (sender as ListBox).SelectedItem as FillupModel;
            if (fillup != null)
            {
                // unset the selection and bring user to edit fillup
                (sender as ListBox).SelectedItem = null;
                NavigationService.Navigate(new Uri(string.Format("//Views/FillupPage.xaml?fillupId={0}",
                    Uri.EscapeUriString(fillup.FillupId.ToString())), UriKind.Relative));
            }
        }

        private void FillupLink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("//Views/FillupPage.xaml", UriKind.Relative));
        }

        private void GasStationLink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("//Views/StationPage.xaml", UriKind.Relative));
        }

        private void LogbookLink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("//Views/LogbookPage.xaml", UriKind.Relative));
        }

        private void WelcomeAddVehicleButton_Click(object sender, RoutedEventArgs e)
        {
        	NavigationService.Navigate(new Uri("//Views/EditVehicle.xaml", UriKind.Relative));
        }

        private void GarageAppBarButton_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("//Views/GaragePage.xaml", UriKind.Relative));
        }

        private void SettingsBarMenuItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("//Views/SettingsPage.xaml", UriKind.Relative));
        }
    }
}