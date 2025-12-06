using Course_work.Models;

namespace Course_work.Pages;

public partial class AllTasksPage : ContentPage
{
    public AllTasksPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Обновляем источник данных
        TasksList.ItemsSource = null;
        TasksList.ItemsSource = AppData.Tasks;
    }
}