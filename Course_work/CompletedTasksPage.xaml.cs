using Course_work.Models;
using Course_work.Pages.Popups;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Course_work.Pages;

public partial class CompletedTasksPage : ContentPage
{
    public ICommand OpenTaskCommand { get; }

    public CompletedTasksPage()
    {
        InitializeComponent();

        OpenTaskCommand = new Command<TodoItem>(ExecuteOpenTaskPopup);
        BindingContext = this;

        try
        {
            if (this.FindByName("TaskPopup") is ViewCompletedTaskPopup popup)
            {
                popup.ReturnRequested += OnPopupReturnRequested;
                popup.DeleteRequested += OnPopupDeleteRequested;
                popup.EditRequested += OnPopupEditRequested;
                popup.Closed += OnPopupClosed;
            }
        }
        catch { }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshTasks();
    }

    private void RefreshTasks()
    {
        CompletedList.ItemsSource = null;
        CompletedList.ItemsSource = AppData.CompletedTasks;
    }

    private void ExecuteOpenTaskPopup(TodoItem item)
    {
        if (item == null) return;

        if (this.FindByName("TaskPopup") is ViewCompletedTaskPopup popup)
            popup.Show(item);
    }

    private void OnPopupReturnRequested(object? sender, TodoItem item)
    {
        if (item == null) return;

        // Возвращаем задачу в активные
        AppData.CompletedTasks.Remove(item);
        AppData.ActiveTasks.Add(item);
        item.IsCompleted = false;

        RefreshTasks();
    }

    private void OnPopupDeleteRequested(object? sender, TodoItem item)
    {
        if (item == null) return;

        AppData.CompletedTasks.Remove(item);
        RefreshTasks();
    }

    private async void OnPopupEditRequested(object? sender, TodoItem item)
{
    if (item == null) return;

    // Закрываем popup перед открытием редактора
    if (this.FindByName("TaskPopup") is ViewCompletedTaskPopup popup)
        popup.Hide();
    
    // Открываем страницу редактирования
    await Navigation.PushAsync(new EditTaskPage(item));
}

    private void OnPopupClosed(object? sender, EventArgs e)
    {
    }
}