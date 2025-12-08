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

        public string? DateDisplay { get; set; }
        public string DateColor { get; set; } = "Red";

        public void UpdateDateDisplay()
        {
            if (Date is null)
            {
                DateDisplay = null;
                DateColor = "Red";
                return;
            }

            var ru = new CultureInfo("ru-RU");
            string mon = Date.Value.ToString("MMM", ru);
            mon = mon.ToLowerInvariant().TrimEnd('.');
            if (mon.Length >= 3)
                mon = mon.Substring(0, 3) + ".";

            if (Time is not null)
                DateDisplay = $"Дата: {Date.Value.Day} {mon} {Time.Value:hh\\:mm}";
            else
                DateDisplay = $"Дата: {Date.Value.Day} {mon}";

            if (Date.Value.Date == DateTime.Today)
                DateColor = "#468966";
            else if (Date.Value.Date == DateTime.Today.AddDays(1))
                DateColor = "#F0AD4E";
            else
                DateColor = "Red";
        }
    }
}