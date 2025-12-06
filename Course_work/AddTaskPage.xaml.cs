using System;
using Course_work.Models;

namespace Course_work;

public partial class AddTaskPage : ContentPage
{
    private string selectedPriority = "Low";

    public AddTaskPage()
    {
        InitializeComponent();
    }

    private async void OnCloseTapped(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // Открыть popup
    private void OnPriorityClicked(object? sender, EventArgs e)
    {
        PopupOverlay.IsVisible = true;
    }

    // Закрыть popup (крестик)
    private void OnClosePriorityPopup(object? sender, EventArgs e)
    {
        PopupOverlay.IsVisible = false;
    }

    // Выбрать приоритет (CommandParameter передаёт строку)
    private void OnPrioritySelected(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string param)
        {
            selectedPriority = param;
        }
        else if (sender is Button b && b.BindingContext is string ctx)
        {
            selectedPriority = ctx;
        }

        PopupOverlay.IsVisible = false;
    }

    // Добавление задачи
    private async void OnAddTaskConfirm(object? sender, EventArgs e)
    {
        var title = TitleEntry?.Text?.Trim() ?? string.Empty;
        var desc = DescriptionEditor?.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(title))
        {
            await DisplayAlert("Внимание", "Нужно добавить название", "OK");
            return;
        }

        var item = new TodoItem
        {
            Title = title,
            Description = desc,
            Priority = selectedPriority
        };

        // добавляем в глобальный список — убедись, что AppData.Tasks существует
        Course_work.AppData.Tasks.Add(item);

        await DisplayAlert("Готово", "Задача добавлена", "OK");

        // Закрываем страницу (возврат к списку)
        await Navigation.PopAsync();
    }
}