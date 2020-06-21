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

        private bool _altAvailable = false;
        private bool _altShowing = false;
        private bool _showDateRemaining = true;
        private bool _showMileageRemaining = true;
        private bool _showAltRemaining = true;

        private string _dateDueAltRemaining = "";
        private string _dateDueMonthsRemaining;
        private string _dateDueDaysRemaining;
        private SolidColorBrush _dateDueRemainingColor;
        private string _dateDueDate;
        private string _dateDueStatus;

        private string _mileageDueAltRemaining;
        private string _mileageDueMileageRemaining;
        private SolidColorBrush _mileageDueRemainingColor;
        private string _mileageDueDate;
        private string _mileageDueStatus;


        private string _altRemaining;
        private string _dueStatus;
        private string _dueDate;

        public ReminderItemViewModel(ReminderModel reminder)
        {
            ReminderTitle = reminder.ReminderType;
            ReminderId = reminder.ReminderId;

            SolidColorBrush colorRed = new SolidColorBrush(Color.FromArgb(255, 183, 21, 58));
            SolidColorBrush colorGreen = new SolidColorBrush(Color.FromArgb(255, 0, 150, 76));

            if (reminder.RemindDate && reminder.RemindOdo)
            {
                _altAvailable = true;
                _altShowing = false;
                ReminderTitle = ReminderTitle + "*";
            } else if (!reminder.RemindDate && !reminder.RemindOdo)
            {
                ShowMileageRemaining = false;
                ShowDateRemaining = false;
                ShowAltRemaining = true;
                AltRemaining = "off";
                DueStatus = " ";
                DueDate = " ";
            }

            if (reminder.RemindOdo)
            {
                if (!reminder.RemindDate)
                {
                    ShowAltRemaining = false;
                    ShowDateRemaining = false;
                    ShowMileageRemaining = true;
                }

                _mileageDueAltRemaining = "";
                if (App.ViewModel.DefaultFillups.Count < 1)
                {
                    AltRemaining = _mileageDueAltRemaining = "TBD";
                    DueStatus = "Available after first fill-up";
                    DueDate = " ";
                    ShowDateRemaining = false;
                    ShowMileageRemaining = false;
                    ShowAltRemaining = true;
                }
                else
                {
                    double totalOdo = reminder.RemindOdoValue - App.ViewModel.DefaultFillups[0].OdoReading;

                    MileageDueMileageRemaining = String.Format("{0:#,0}", Math.Abs(totalOdo));
                    DueDate = _mileageDueDate = "Due at: " + String.Format("{0:#,0}", reminder.RemindOdoValue) + " miles";
                    if (totalOdo < 0)
                    {
                        MileageDueRemainingColor = colorRed;
                        DueStatus = _mileageDueStatus = "overdue";
                    }
                    else
                    {
                        MileageDueRemainingColor = colorGreen;
                        DueStatus = _mileageDueStatus = "remaining";
                    }
                }
            }

            if (reminder.RemindDate)
            {
                ShowMileageRemaining = false;
                ShowAltRemaining = false;
                ShowDateRemaining = true;

                TimeSpan diffDays = reminder.RemindDateValue.Date.Subtract(DateTime.Now);
                double totalDays = diffDays.TotalDays;
                int monthsRemaining = (int)Math.Round(totalDays / 30, 0);
                int daysRemaining = (int)Math.Round(totalDays - monthsRemaining * 30, 0);

                DateDueMonthsRemaining = Math.Abs(monthsRemaining).ToString();
                DateDueDaysRemaining = Math.Abs(daysRemaining).ToString();
                DueDate = _dateDueDate = "Due on: " + String.Format("{0:MM/dd/yyyy}", reminder.RemindDateValue.Date);
                if (totalDays < 0)
                {
                    DateDueRemainingColor = colorRed;
                    DueStatus = _dateDueStatus = "overdue";
                }
                else
                {
                    DateDueRemainingColor = colorGreen;
                    DueStatus = _dateDueStatus = "remaining";
                }

            }


        }

        public void ShowAlt()
        {
            if (_altAvailable)
            {
                if (_altShowing)
                {
                    ShowMileageRemaining = false;
                    ShowDateRemaining = false;
                    ShowAltRemaining = false;
                    if (_dateDueAltRemaining != "")
                    {
                        AltRemaining = _dateDueAltRemaining;
                        ShowAltRemaining = true;
                    }
                    else
                    {
                        ShowDateRemaining = true;
                    }

                    DueDate = _dateDueDate;
                    DueStatus = _dateDueStatus;
                    _altShowing = false;
                }
                else
                {
                    ShowMileageRemaining = false;
                    ShowDateRemaining = false;
                    ShowAltRemaining = false;
                    if (_mileageDueAltRemaining != "")
                    {
                        AltRemaining = _mileageDueAltRemaining;
                        ShowAltRemaining = true;
                    }
                    else
                    {
                        ShowMileageRemaining = true;
                    }

                    DueDate = _mileageDueDate;
                    DueStatus = _mileageDueStatus;
                    _altShowing = true;
                }
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

        #region Class Members

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

        public bool ShowDateRemaining
        {
            get
            {
                return _showDateRemaining;
            }
            set
            {
                if (_showDateRemaining != value)
                {
                    _showDateRemaining = value;
                    NotifyPropertyChanged("ShowDateRemaining");
                }
            }
        }

        public bool ShowMileageRemaining
        {
            get
            {
                return _showMileageRemaining;
            }
            set
            {
                if (_showMileageRemaining != value)
                {
                    _showMileageRemaining = value;
                    NotifyPropertyChanged("ShowMileageRemaining");
                }
            }
        }

        public bool ShowAltRemaining
        {
            get
            {
                return _showAltRemaining;
            }
            set
            {
                if (_showAltRemaining != value)
                {
                    _showAltRemaining = value;
                    NotifyPropertyChanged("ShowAltRemaining");
                }
            }
        }

        public string DateDueMonthsRemaining
        {
            get
            {
                return _dateDueMonthsRemaining;
            }
            set
            {
                if (_dateDueMonthsRemaining != value)
                {
                    _dateDueMonthsRemaining = value;
                    NotifyPropertyChanged("DateDueMonthsRemaining");
                }
            }
        }

        public string DateDueDaysRemaining
        {
            get
            {
                return _dateDueDaysRemaining;
            }
            set
            {
                if (_dateDueDaysRemaining != value)
                {
                    _dateDueDaysRemaining = value;
                    NotifyPropertyChanged("DateDueDaysRemaining");
                }
            }
        }

        public SolidColorBrush DateDueRemainingColor
        {
            get
            {
                return _dateDueRemainingColor;
            }
            set
            {
                if (_dateDueRemainingColor != value)
                {
                    _dateDueRemainingColor = value;
                    NotifyPropertyChanged("DateDueRemainingColor");
                }
            }
        }

        public string MileageDueMileageRemaining
        {
            get
            {
                return _mileageDueMileageRemaining;
            }
            set
            {
                if (_mileageDueMileageRemaining != value)
                {
                    _mileageDueMileageRemaining = value;
                    NotifyPropertyChanged("MileageDueMileageRemaining");
                }
            }
        }

        public SolidColorBrush MileageDueRemainingColor
        {
            get
            {
                return _mileageDueRemainingColor;
            }
            set
            {
                if (_mileageDueRemainingColor != value)
                {
                    _mileageDueRemainingColor = value;
                    NotifyPropertyChanged("MileageDueRemainingColor");
                }
            }
        }

        public string AltRemaining
        {
            get
            {
                return _altRemaining;
            }
            set
            {
                if (_altRemaining != value)
                {
                    _altRemaining = value;
                    NotifyPropertyChanged("AltRemaining");
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

        #endregion

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
