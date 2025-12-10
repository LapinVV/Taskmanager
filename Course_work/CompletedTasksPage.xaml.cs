using Course_work.Models;

namespace Course_work.Pages;

public partial class CompletedTasksPage : ContentPage
{
    public CompletedTasksPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        CompletedList.ItemsSource = null;
        CompletedList.ItemsSource = AppData.CompletedTasks;
    }
}