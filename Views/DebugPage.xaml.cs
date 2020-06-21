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
using System.Collections.ObjectModel;

namespace car_pal
{
    public partial class DebugPage : PhoneApplicationPage
    {
        public DebugPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataStore.Garage.DefaultVehicle.FillupHistory = new ObservableCollection<zFillupModel>();
            DataStore.SaveGarage();
        }
    }
}
