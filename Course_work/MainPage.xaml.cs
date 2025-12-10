using Microsoft.Maui.Controls;
using Course_work.Pages;

namespace Course_work
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnAllTasksClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllTasksPage());
        }

        private async void OnTodayTasksClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NowTasksPage());
        }

        private async void OnCompletedTasksClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CompletedTasksPage());
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTaskPage());
        }
    }
}