using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace car_pal
{
	public partial class VehicleSelectionPopup : UserControl
	{
		public VehicleSelectionPopup()
		{
			// Required to initialize variables
			InitializeComponent();
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			this.Visibility = Visibility.Collapsed;
		}
	}
}