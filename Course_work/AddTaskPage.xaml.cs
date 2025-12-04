namespace Course_work;

public partial class AddTaskPage : ContentPage
{
    public AddTaskPage()
    {
        InitializeComponent();
    }

    private async void OnCloseTapped(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnAddTaskConfirm(object? sender, EventArgs e)
    {
        await DisplayAlert("Task", "Task added!", "OK");
    }
}