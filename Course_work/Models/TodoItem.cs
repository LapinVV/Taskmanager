using System;
using System.Globalization;

namespace Course_work.Models
{
    public class TodoItem
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;

        public string Priority { get; set; } = "Low";

        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        
        // 🔔 ДОБАВЬТЕ ЭТО СВОЙСТВО ДЛЯ НАПОМИНАНИЙ
        public int? ReminderMinutes { get; set; }

        public string? DateDisplay { get; set; }
        public string DateColor { get; set; } = "Red";
        
        // 🔔 (ОПЦИОНАЛЬНО) Можно добавить свойство для отображения
        public string ReminderDisplayText
        {
            get
            {
                if (!ReminderMinutes.HasValue) return "Нет";
                
                int minutes = ReminderMinutes.Value;
                return minutes switch
                {
                    0 => "Во время начала",
                    < 60 => $"За {minutes} мин",
                    < 1440 => $"За {minutes / 60} час",
                    _ => $"За {minutes / 1440} дн"
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
                DateDisplay =
                    $"Дата: {Date.Value.Day} {mon} {Time.Value:hh\\:mm} {Date.Value:yyyy}";
            else
                DateDisplay =
                    $"Дата: {Date.Value.Day} {mon} {Date.Value:yyyy}";

            if (Date.Value.Date == DateTime.Today)
                DateColor = "#468966";
            else if (Date.Value.Date == DateTime.Today.AddDays(1))
                DateColor = "#F0AD4E";
            else
                DateColor = "Red";
        }
    }
}