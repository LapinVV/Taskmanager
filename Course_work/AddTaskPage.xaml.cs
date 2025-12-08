using System;
using System.Globalization;
using System.ComponentModel;
using Course_work.Models;

namespace Course_work;

public partial class AddTaskPage : ContentPage
{
    private string selectedPriority = "Low";
    private DateTime? selectedDate = null;
    private TimeSpan? selectedTime = null;

    public AddTaskPage()
    {
        InitializeComponent();
    }

    private async void OnCloseTapped(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void OnPriorityClicked(object? sender, EventArgs e)
    {
        PopupOverlay.IsVisible = true;
        PriorityPopup.IsVisible = true;
        DatePopupOverlay.IsVisible = false;
    }

    private void OnClosePriorityPopup(object? sender, EventArgs e)
    {
        PriorityPopup.IsVisible = false;
        PopupOverlay.IsVisible = false;
    }

    private void OnPrioritySelected(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string param)
            selectedPriority = param;

        PriorityPopup.IsVisible = false;
        PopupOverlay.IsVisible = false;
    }

    private void OnDateClicked(object? sender, EventArgs e)
    {
        PopupOverlay.IsVisible = true;
        DatePopupOverlay.IsVisible = true;
        PriorityPopup.IsVisible = false;
    }

    private void OnCloseDatePopup(object? sender, EventArgs e)
    {
        DatePopupOverlay.IsVisible = false;
        PopupOverlay.IsVisible = false;
    }

    private void OnSelectToday(object? sender, EventArgs e)
    {
        selectedDate = DateTime.Today;
        ApplySelectedDate();
    }

    private void OnSelectTomorrow(object? sender, EventArgs e)
    {
        selectedDate = DateTime.Today.AddDays(1);
        ApplySelectedDate();
    }

    private void OnSelectNextWeek(object? sender, EventArgs e)
    {
        selectedDate = DateTime.Today.AddDays(7);
        ApplySelectedDate();
    }

    private void OnSelectNoDate(object? sender, EventArgs e)
    {
        selectedDate = null;
        selectedTime = null;

        PickedDateLabel.IsVisible = false;

        DatePopupOverlay.IsVisible = false;
        PopupOverlay.IsVisible = false;
    }

    private void OnCustomDatePicked(object? sender, DateChangedEventArgs e)
    {
        selectedDate = e.NewDate;
        ApplySelectedDate();
    }

    private void OnCustomTimePropertyChanged(object? sender, PropertyChangedEventArgs e)
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
        string mon = selectedDate.Value.ToString("MMM", ru);
        mon = mon.ToLowerInvariant().TrimEnd('.').Substring(0, Math.Min(3, mon.Length)) + ".";

        string result;
        if (selectedTime != null)
            result = $"Дата: {selectedDate.Value.Day} {mon} {selectedTime.Value:hh\\:mm}";
        else
            result = $"Дата: {selectedDate.Value.Day} {mon}";

        PickedDateLabel.Text = result;
        PickedDateLabel.IsVisible = true;

        DatePopupOverlay.IsVisible = false;
        PopupOverlay.IsVisible = false;
    }

    private async void OnAddTaskConfirm(object? sender, EventArgs e)
    {
        var titleText = TitleEntry?.Text?.Trim() ?? string.Empty;
        var desc = DescriptionEditor?.Text?.Trim();

        if (string.IsNullOrWhiteSpace(titleText))
        {
            await DisplayAlert("Внимание", "Нужно добавить название", "OK");
            return;
        }

        string? display = null;

        if (selectedDate != null)
        {
            var ru = new CultureInfo("ru-RU");
            string mon = selectedDate.Value.ToString("MMM", ru);
            mon = mon.ToLowerInvariant().TrimEnd('.').Substring(0, Math.Min(3, mon.Length)) + ".";

            if (selectedTime != null)
                display = $"Дата: {selectedDate.Value.Day} {mon} {selectedTime.Value:hh\\:mm}";
            else
                display = $"Дата: {selectedDate.Value.Day} {mon}";
        }

        var item = new TodoItem
        {
            Title = titleText,
            Description = desc,
            Priority = selectedPriority,
            Date = selectedDate,
            Time = selectedTime,
            DateDisplay = display
        };

        AppData.Tasks.Add(item);

        await DisplayAlert("Готово", "Задача добавлена", "OK");
        await Navigation.PopAsync();
    }
}