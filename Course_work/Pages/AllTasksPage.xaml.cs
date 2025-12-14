using Course_work.Models;
using Course_work.Pages.Popups;
using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;


namespace Course_work.Pages
{
    public partial class AllTasksPage : ContentPage
    {
        public ICommand CompleteTaskCommand { get; }
        public ICommand OpenTaskCommand { get; }

        public AllTasksPage()
        {
            InitializeComponent();

            CompleteTaskCommand = new Command<TodoItem>(ExecuteCompleteTaskCommand);
            OpenTaskCommand = new Command<TodoItem>(ExecuteOpenTaskPopup);

            BindingContext = this;

            try
            {
                if (this.FindByName("TaskPopup") is ViewTaskPopup popup)
                {
                    popup.CompleteRequested += OnPopupCompleteRequested;
                    popup.DeleteRequested += OnPopupDeleteRequested;
                    popup.EditRequested += OnPopupEditRequested;
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
            TasksList.ItemsSource = null;
            TasksList.ItemsSource = AppData.ActiveTasks;
        }

        private async Task<bool> ShowAutoClosingAlert(string title, string message, int timeoutMs = 2000)
        {
            var tcs = new TaskCompletionSource<bool>();

            var overlay = new Grid
            {
                BackgroundColor = Colors.Black.WithAlpha(0.35f),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            var alertBox = new Border
            {
                Stroke = Colors.LightGray,
                StrokeThickness = 1,
                BackgroundColor = Color.FromRgb(248, 248, 248),
                StrokeShape = new RoundRectangle { CornerRadius = 14 },
                Shadow = new Shadow
                {
                    Brush = Colors.Black,
                    Offset = new Point(0, 4),
                    Radius = 12,
                    Opacity = 0.25f
                },
                Padding = 20,
                WidthRequest = 300,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            alertBox.Content = new VerticalStackLayout
            {
                Spacing = 14,
                Children =
                {
                    new Label
                    {
                        Text = title,
                        FontSize = 20,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = Colors.Black
                    },
                    new Label
                    {
                        Text = message,
                        FontSize = 15,
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = Colors.Black
                    },
                    new Button
                    {
                        Text = "Отмена",
                        CornerRadius = 10,
                        BackgroundColor = Color.FromRgb(230, 230, 230),
                        TextColor = Colors.Black,
                        Command = new Command(() => tcs.TrySetResult(true))
                    }
                }
            };

            overlay.Children.Add(alertBox);

            var oldContent = this.Content;
            var wrapper = new Grid();
            if (oldContent != null)
                wrapper.Children.Add(oldContent);

            wrapper.Children.Add(overlay);

            this.Content = wrapper;

            _ = Task.Run(async () =>
            {
                await Task.Delay(timeoutMs);
                tcs.TrySetResult(false);
            });

            bool cancelled = await tcs.Task;

            this.Content = oldContent;

            return cancelled;
        }

        private async void ExecuteCompleteTaskCommand(TodoItem item)
        {
            if (item == null)
                return;

            bool cancelled = await ShowAutoClosingAlert(
                "Задача выполнена",
                $"Отменить выполнение задачи \"{item.Title}\"?",
                2000
            );

            if (cancelled)
                return;

            AppData.ActiveTasks.Remove(item);
            AppData.CompletedTasks.Add(item);
            item.IsCompleted = true;

            RefreshTasks();
        }

        private void ExecuteOpenTaskPopup(TodoItem item)
        {
            if (item == null)
                return;

            if (this.FindByName("TaskPopup") is ViewTaskPopup popup)
                popup.Show(item);
        }

        private async void OnPopupCompleteRequested(object? sender, TodoItem item)
        {
            if (item == null)
                return;

            bool cancelled = await ShowAutoClosingAlert(
                "Задача выполнена",
                $"Отменить выполнение задачи \"{item.Title}\"?",
                2000
            );

            if (cancelled)
                return;

            AppData.ActiveTasks.Remove(item);
            AppData.CompletedTasks.Add(item);
            item.IsCompleted = true;

            RefreshTasks();
        }

        private void OnPopupDeleteRequested(object? sender, TodoItem item)
        {
            if (item == null)
                return;

            AppData.ActiveTasks.Remove(item);
            RefreshTasks();
        }

       private async void OnPopupEditRequested(object? sender, TodoItem item)
{
    if (item == null)
        return;

    // Закрываем popup перед открытием редактора
    if (this.FindByName("TaskPopup") is ViewTaskPopup popup)
        popup.Hide();
    
    // Открываем страницу редактирования
    await Navigation.PushAsync(new EditTaskPage(item));
}
    }
}