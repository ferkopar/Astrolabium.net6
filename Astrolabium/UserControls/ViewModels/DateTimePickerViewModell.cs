using Astrolabium.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrolabium.UserControls.ViewModels
{
    public class DateTimePickerEventArgs : EventArgs
    {
        public DateTime Selected{get;}
        public DateTimePickerEventArgs(DateTime selectedDateTime)
        {
            Selected = selectedDateTime;
        }
    }
    public class DateTimePickerViewModell:ViewModelBase
    {
        private int year;
        private int month;
        private int day;
        private int hour;
        private int minute;
        private int second;

        public DateTimePickerViewModell(DateTime dateTime)
        {
            SelectedDateTime = dateTime;
        }

        public event EventHandler<DateTimePickerEventArgs> RaiseDateTimePickerButtonClicked;
        public int Year
        {
            get { return year; }
            set { year = value;
                OnPropertyChanged(nameof(SelectedDateTime));
                
            }
        }
        public int Month
        {
            get { return month; }
            set { month = value;
                OnPropertyChanged(nameof(SelectedDateTime));
               
            }
        }
        public int Day
        {
            get { return day; }
            set { day = value;
                OnPropertyChanged(nameof(SelectedDateTime));
                
            }
        }
        public int Hour
        {
            get { return hour; }
            set { hour = value;
                OnPropertyChanged(nameof(SelectedDateTime));
               
            }
        }
        public int Minute
        {
            get { return minute; }
            set { minute = value;
                OnPropertyChanged(nameof(SelectedDateTime));
               
            }
        }
        public int Second
        {
            get { return second; }
            set { second = value;
                OnPropertyChanged(nameof(SelectedDateTime));
                OnRaiseDateTimePickerButtonClicked(new DateTimePickerEventArgs(SelectedDateTime));
            }
        }
        public DateTime SelectedDateTime
        {

            get
            {
                return new DateTime(Year, Month, Day, Hour, Minute, Second);
            }

            set
            {
                Year = value.Year;
                Month = value.Month;
                Day = value.Day;
                Hour = value.Hour;
                Minute = value.Minute;
                Second = value.Second;
                OnPropertyChanged(nameof(SelectedDateTime));
                OnRaiseDateTimePickerButtonClicked(new DateTimePickerEventArgs(SelectedDateTime));
            }

        }
        protected virtual void OnRaiseDateTimePickerButtonClicked(DateTimePickerEventArgs e) => RaiseDateTimePickerButtonClicked?.Invoke(this, e);

    }
}
