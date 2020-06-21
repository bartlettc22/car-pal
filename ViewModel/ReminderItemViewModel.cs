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
using System.ComponentModel;
using car_pal.Models;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace car_pal.ViewModel
{
    public class ReminderItemViewModel : INotifyPropertyChanged
    {
        private int _reminderId;
        private string _reminderTitle;
        private string _dueRemaining;
        private string _dueStatus;
        private string _dueDate;

        public ReminderItemViewModel(ReminderModel reminder)
        {
            ReminderTitle = reminder.ReminderType;
            ReminderId = reminder.ReminderId;

            if (reminder.RemindDate || reminder.RemindOdo)
            {
                // Default is date
                if (reminder.RemindDate)
                {
                    TimeSpan diffDays = reminder.RemindDateValue.Date.Subtract(DateTime.Now);
                    double totalDays = diffDays.TotalDays;
                    int monthsRemaining = (int)Math.Round(totalDays / 30, 0);
                    int daysRemaining = (int)Math.Round(totalDays - monthsRemaining * 30, 0);

                    DueRemaining = monthsRemaining.ToString() + " m " + daysRemaining.ToString() + " d";
                    DueDate = "Due on: " + String.Format("{0:MM/dd/yyyy}", reminder.RemindDateValue.Date);
                    if (totalDays < 0)
                    {
                        DueStatus = "overdue";
                    }
                    else
                    {
                        DueStatus = "remaining";
                    }

                }
                else
                {
                    if (App.ViewModel.DefaultFillups.Count < 1)
                    {
                        DueRemaining = "TBD";
                    }
                    else
                    {

                        double totalOdo = reminder.RemindOdoValue - App.ViewModel.DefaultFillups[0].OdoReading;

                        DueRemaining = String.Format("{0:#,0}", totalOdo) + " miles";
                        DueDate = "Due at: " + String.Format("{0:#,0}", reminder.RemindOdoValue) + " miles";
                        if (totalOdo < 0)
                        {
                            DueStatus = "overdue";
                        }
                        else
                        {
                            DueStatus = "remaining";
                        }
                    }
                }
            }
            else
            {
                DueRemaining = "Off";
                DueStatus = " ";
                DueDate = " ";
            }
        }

        public int ReminderId
        {
            get
            {
                return _reminderId;
            }
            set
            {
                if (_reminderId != value)
                {
                    _reminderId = value;
                    NotifyPropertyChanged("ReminderId");
                }
            }
        }

        public string ReminderTitle
        {
            get
            {
                return _reminderTitle;
            }
            set
            {
                if (_reminderTitle != value)
                {
                    _reminderTitle = value;
                    NotifyPropertyChanged("ReminderTitle");
                }
            }
        }

        public string DueRemaining
        {
            get
            {
                return _dueRemaining;
            }
            set
            {
                if (_dueRemaining != value)
                {
                    _dueRemaining = value;
                    NotifyPropertyChanged("DueRemaining");
                }
            }
        }

        public string DueStatus
        {
            get
            {
                return _dueStatus;
            }
            set
            {
                if (_dueStatus != value)
                {
                    _dueStatus = value;
                    NotifyPropertyChanged("DueStatus");
                }
            }
        }

        public string DueDate
        {
            get
            {
                return _dueDate;
            }
            set
            {
                if (_dueDate != value)
                {
                    _dueDate = value;
                    NotifyPropertyChanged("DueDate");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
