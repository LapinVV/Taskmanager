using System;
using System.Globalization;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Course_work.Models
{
    public class TodoItem : INotifyPropertyChanged
    {
        private string _title = string.Empty;
        private string? _description;
        private bool _isCompleted = false;
        private string _priority = "Low";
        private DateTime? _date;
        private TimeSpan? _time;
        private int? _reminderMinutes;
        private string? _dateDisplay;
        private string _dateColor = "Red";

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public string? Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }

        public string Priority
        {
            get => _priority;
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    OnPropertyChanged(nameof(Priority));
                }
            }
        }
        
        [XmlIgnore]
        public DateTime? Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    UpdateDateDisplay();
                    OnPropertyChanged(nameof(Date));
                }
            }
        }
        
        [XmlElement("Date")]
        public string DateXml
        {
            get => _date?.ToString("yyyy-MM-ddTHH:mm:ss") ?? string.Empty;
            set => _date = string.IsNullOrEmpty(value) ? null : DateTime.Parse(value);
        }
        
        [XmlIgnore]
        public TimeSpan? Time
        {
            get => _time;
            set
            {
                if (_time != value)
                {
                    _time = value;
                    UpdateDateDisplay();
                    OnPropertyChanged(nameof(Time));
                }
            }
        }
        
        [XmlElement("Time")]
        public string TimeXml
        {
            get => _time?.ToString(@"hh\:mm\:ss") ?? string.Empty;
            set => _time = string.IsNullOrEmpty(value) ? null : TimeSpan.Parse(value);
        }
        
        public int? ReminderMinutes
        {
            get => _reminderMinutes;
            set
            {
                if (_reminderMinutes != value)
                {
                    _reminderMinutes = value;
                    OnPropertyChanged(nameof(ReminderMinutes));
                }
            }
        }
        
        [XmlIgnore]
        public string? DateDisplay
        {
            get => _dateDisplay;
            private set
            {
                if (_dateDisplay != value)
                {
                    _dateDisplay = value;
                    OnPropertyChanged(nameof(DateDisplay));
                }
            }
        }
        
        [XmlIgnore]
        public string DateColor
        {
            get => _dateColor;
            private set
            {
                if (_dateColor != value)
                {
                    _dateColor = value;
                    OnPropertyChanged(nameof(DateColor));
                }
            }
        }

        [XmlIgnore]
        public string ReminderDisplayText
        {
            get
            {
                if (!ReminderMinutes.HasValue)
                    return "Напоминание: не напоминать";
                
                int minutes = ReminderMinutes.Value;
                
                return minutes switch
                {
                    0 => "Напоминание: во время начала",
                    < 60 => $"Напоминание: за {minutes} минут до конца срока выполнения",
                    < 1440 => $"Напоминание: за {minutes / 60} часов до конца срока выполнения",
                    _ => $"Напоминание: за {minutes / 1440} дней до конца срока выполнения"
                };
            }
        }

        public void UpdateDateDisplay()
        {
            if (Date is null)
            {
                DateDisplay = null;
                DateColor = "Red";
                return;
            }

            var ru = new CultureInfo("ru-RU");
            string mon = Date.Value.ToString("MMM", ru)
                               .ToLowerInvariant()
                               .TrimEnd('.');

            if (mon.Length >= 3)
                mon = mon.Substring(0, 3) + ".";

            if (Time is not null)
                DateDisplay = $"Дата: {Date.Value.Day} {mon} {Time.Value:hh\\:mm} {Date.Value:yyyy}";
            else
                DateDisplay = $"Дата: {Date.Value.Day} {mon} {Date.Value:yyyy}";

            if (Date.Value.Date == DateTime.Today)
                DateColor = "#468966";
            else if (Date.Value.Date == DateTime.Today.AddDays(1))
                DateColor = "#F0AD4E";
            else
                DateColor = "Red";
        }
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public TodoItem() { }
    }
}