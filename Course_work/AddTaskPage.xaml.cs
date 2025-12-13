using System;
using System.Globalization;
using System.ComponentModel;
using Course_work.Models;

namespace Course_work;

public partial class AddTaskPage : ContentPage, INotifyPropertyChanged
{
    private string selectedPriority = "Low";
    public string SelectedPriority
    {
        get => selectedPriority;
        set
        {
            if (selectedPriority != value)
            {
                selectedPriority = value;
                OnPropertyChanged(nameof(SelectedPriority));
            }
        }
    }

    private DateTime? selectedDate = null;
    private TimeSpan? selectedTime = null;
    private int? reminderMinutes = null;

    private PropertyChangedEventHandler? propertyChanged;

    event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
    {
        add => propertyChanged += value;
        remove => propertyChanged -= value;
    }

    public AddTaskPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnCloseTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void OnPriorityClicked(object sender, EventArgs e)
    {
        PopupOverlay.IsVisible = true;
        PriorityPopup.IsVisible = true;
        DatePopupOverlay.IsVisible = false;
        ReminderPopup.IsVisible = false;
        CustomReminderPopup.IsVisible = false;
    }

    private void OnClosePriorityPopup(object sender, EventArgs e)
    {
        PriorityPopup.IsVisible = false;
        PopupOverlay.IsVisible = false;
    }

    private void OnPrioritySelected(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string param)
            SelectedPriority = param;

        PriorityPopup.IsVisible = false;
        PopupOverlay.IsVisible = false;
    }

    private void OnDateClicked(object sender, EventArgs e)
    {
        PopupOverlay.IsVisible = true;
        DatePopupOverlay.IsVisible = true;
        PriorityPopup.IsVisible = false;
        ReminderPopup.IsVisible = false;
        CustomReminderPopup.IsVisible = false;
    }

    private void OnCloseDatePopup(object sender, EventArgs e)
    {
        DatePopupOverlay.IsVisible = false;
        PopupOverlay.IsVisible = false;
    }

    private void OnSelectToday(object sender, EventArgs e)
    {
        selectedDate = DateTime.Today;
        ApplySelectedDate();
    }

    private void OnSelectTomorrow(object sender, EventArgs e)
    {
        selectedDate = DateTime.Today.AddDays(1);
        ApplySelectedDate();
    }

    private void OnSelectNextWeek(object sender, EventArgs e)
    {
        selectedDate = DateTime.Today.AddDays(7);
        ApplySelectedDate();
    }

    private void OnSelectNoDate(object sender, EventArgs e)
    {
        selectedDate = null;
        selectedTime = null;
        reminderMinutes = null;

        PickedDateLabel.IsVisible = false;
        UpdateReminderButtonText();

        DatePopupOverlay.IsVisible = false;
        PopupOverlay.IsVisible = false;
    }

    private void OnCustomDatePicked(object sender, DateChangedEventArgs e)
    {
        selectedDate = e.NewDate;
        ApplySelectedDate();
    }

    private void OnCustomTimePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(TimePicker.Time))
            return;

        if (sender is TimePicker tp)
        {
            selectedTime = tp.Time;

            if (selectedDate != null)
                ApplySelectedDate();
        }
    }

    private void ApplySelectedDate()
    {
        if (selectedDate == null)
        {
            PickedDateLabel.IsVisible = false;
            return;
        }

        var ru = new CultureInfo("ru-RU");
        string result;

        if (selectedTime != null)
        {
            var dt = selectedDate.Value.Date + selectedTime.Value;
            result = "Дата: " + dt.ToString("d MMMM HH:mm yyyy", ru);
        }
        else
        {
            result = "Дата: " + selectedDate.Value.ToString("d MMMM yyyy", ru);
        }

        PickedDateLabel.Text = result;
        PickedDateLabel.IsVisible = true;

        DatePopupOverlay.IsVisible = false;
        PopupOverlay.IsVisible = false;
    }

    private void OnReminderClicked(object sender, EventArgs e)
    {
        if (selectedDate == null)
        {
            DisplayAlert("Внимание", "Чтобы настроить напоминание, выберите дату.", "Ок");
            return;
        }

        if (selectedTime == null)
        {
            DisplayAlert("Внимание", "Чтобы настроить напоминание, выберите точное время.", "Ок");
            return;
        }

        DateTime eventTime = selectedDate.Value.Date + selectedTime.Value;

        if (eventTime <= DateTime.Now)
        {
            DisplayAlert("Ошибка", "Нельзя установить напоминание на прошедшее время.", "Ок");
            return;
        }

        if (EventDateTimeLabel != null)
        {
            EventDateTimeLabel.Text = $"Событие: {eventTime:dd.MM.yyyy} в {eventTime:HH:mm}";
        }

        if (TimeUnitPicker != null && TimeUnitPicker.Items.Count > 0)
        {
            TimeUnitPicker.SelectedIndex = 0;
        }

        PopupOverlay.IsVisible = true;
        ReminderPopup.IsVisible = true;
        PriorityPopup.IsVisible = false;
        DatePopupOverlay.IsVisible = false;
        CustomReminderPopup.IsVisible = false;
    }

    private void OnCloseReminderPopup(object sender, EventArgs e)
    {
        ReminderPopup.IsVisible = false;
        PopupOverlay.IsVisible = false;
    }

    private void OnReminderPresetSelected(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string param)
        {
            if (param == "None")
            {
                reminderMinutes = null;
            }
            else if (int.TryParse(param, out int minutes))
            {
                reminderMinutes = minutes;
            }

            UpdateReminderButtonText();
            OnCloseReminderPopup(sender, e);
        }
    }

    private void OnOpenCustomReminder(object sender, EventArgs e)
    {
        ReminderPopup.IsVisible = false;
        CustomReminderPopup.IsVisible = true;

        if (CustomMinutesEntry != null)
        {
            CustomMinutesEntry.Text = "";
        }
        
        UpdateTimePlaceholder();
    }

    private void OnCloseCustomReminderPopup(object sender, EventArgs e)
    {
        CustomReminderPopup.IsVisible = false;
        ReminderPopup.IsVisible = true;
    }

    private void UpdateTimePlaceholder()
    {
        if (TimeUnitPicker?.SelectedItem == null || CustomMinutesEntry == null)
            return;
        
        string unit = TimeUnitPicker.SelectedItem.ToString()?.ToLower() ?? "минут";
        
        CustomMinutesEntry.Placeholder = unit switch
        {
            "минут" => "1-59",
            "часов" => "1-23",
            "дней" => "1-30",
            _ => "Число"
        };
    }

    private void OnTimeUnitChanged(object sender, EventArgs e)
    {
        UpdateTimePlaceholder();
    }

    private string? ValidateTimeInput(int value, string unit)
    {
        return unit switch
        {
            "минут" => value switch
            {
                <= 0 => "Минуты должны быть больше 0",
                > 59 => "Максимум 59 минут",
                _ => null
            },
            "часов" => value switch
            {
                <= 0 => "Часы должны быть больше 0",
                > 23 => "Максимум 23 часа",
                _ => null
            },
            "дней" => value switch
            {
                <= 0 => "Дни должны быть больше 0",
                > 30 => "Максимум 30 дней",
                _ => null
            },
            _ => "Неизвестная единица времени"
        };
    }

    private void OnConfirmCustomReminder(object sender, EventArgs e)
    {
        if (CustomMinutesEntry == null || TimeUnitPicker == null)
        {
            DisplayAlert("Ошибка", "Ошибка загрузки интерфейса", "OK");
            return;
        }

        string inputText = CustomMinutesEntry.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(inputText))
        {
            DisplayAlert("Ошибка", "Введите количество времени", "OK");
            return;
        }

        if (!int.TryParse(inputText, out int value))
        {
            DisplayAlert("Ошибка", "Введите число", "OK");
            return;
        }

        object selectedItem = TimeUnitPicker.SelectedItem;
        string unit = selectedItem?.ToString()?.Trim()?.ToLower() ?? "минут";

        string? validationError = ValidateTimeInput(value, unit);
        if (validationError != null)
        {
            DisplayAlert("Ошибка", validationError, "OK");
            return;
        }

        int multiplier = unit switch
        {
            "часов" => 60,
            "дней" => 1440,
            _ => 1
        };

        reminderMinutes = value * multiplier;
        
        UpdateReminderButtonText();

        OnCloseCustomReminderPopup(sender, e);
        OnCloseReminderPopup(sender, e);
    }

    private void UpdateReminderButtonText()
    {
        string buttonText = "Уведомления";

        if (reminderMinutes.HasValue)
        {
            int minutes = reminderMinutes.Value;
            
            buttonText = minutes switch
            {
                0 => "В начале",
                < 60 => $"{minutes} мин",
                < 1440 => $"{minutes / 60} ч",
                _ => $"{minutes / 1440} д"
            };
        }

        if (ReminderButton != null)
        {
            ReminderButton.Text = buttonText;
        }
    }

    private async void OnAddTaskConfirm(object sender, EventArgs e)
    {
        var titleText = TitleEntry?.Text?.Trim() ?? string.Empty;
        var desc = DescriptionEditor?.Text?.Trim();

        if (string.IsNullOrWhiteSpace(titleText))
        {
            await DisplayAlert("Внимание", "Нужно добавить название", "OK");
            return;
        }

        var item = new TodoItem
        {
            Title = titleText,
            Description = desc,
            Priority = SelectedPriority,
            Date = selectedDate,
            Time = selectedTime,
            ReminderMinutes = reminderMinutes
        };

        item.UpdateDateDisplay();

        AppData.ActiveTasks.Add(item);

        if (reminderMinutes.HasValue && selectedDate.HasValue && selectedTime.HasValue)
        {
            DateTime eventTime = selectedDate.Value.Date + selectedTime.Value;
            DateTime reminderTime = eventTime.AddMinutes(-reminderMinutes.Value);
            Console.WriteLine($"Напоминание для '{titleText}' в {reminderTime:HH:mm}");
        }

        await DisplayAlert("Готово", "Задача добавлена", "OK");
        await Navigation.PopAsync();
    }

   protected new virtual void OnPropertyChanged(string propertyName)
{
    propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
}