using Microsoft.Maui.Controls;

namespace Course_work;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnAllTasksClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ContentPage
        {
            Title = "All Tasks",
            Content = new Label
            {
                Text = "All Tasks page (placeholder)",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            }
        });
    }

    private async void OnTodayTasksClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ContentPage
        {
            Title = "Today's Tasks",
            Content = new Label
            {
                Text = "Today's Tasks page (placeholder)",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            }
        });
    }

    private async void OnCompletedTasksClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ContentPage
        {
            Title = "Completed Tasks",
            Content = new Label
            {
                Text = "Completed Tasks page (placeholder)",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            }
        });
    }

    private async void OnAddTaskClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Add Task",
            "Popup for adding a task will appear here later.",
            "OK");
    }
}