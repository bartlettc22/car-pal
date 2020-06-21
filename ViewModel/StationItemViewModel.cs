using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using car_pal.Models;

namespace car_pal.ViewModel
{
    public class StationItemViewModel
    {

        private String _title;
        private String _address;
        private String _phoneNumber;
        private double _distance;
        private int _itemNumber;

        public int ItemNumber
        {
            get
            {
                return _itemNumber;
            }
            set
            {
                _itemNumber = value;
            }

        }

        public String Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public String Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        public String PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
            }
        }

        public String Distance
        {
            get
            {
                return String.Format("{0:0.00}", _distance) + " mi";
            }
            set
            {
                _distance = double.Parse(value);
            }
        }

    }
}
