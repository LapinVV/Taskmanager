using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Shapes;
using Course_work.Models;

namespace Course_work.Pages
{
    public partial class NowTasksPage : ContentPage
    {
        public ObservableCollection<TodoItem> TodayTasks { get; set; }
            = new ObservableCollection<TodoItem>();

        public Command<TodoItem> CompleteTaskCommand { get; }

        public NowTasksPage()
        {
            InitializeComponent();

            CompleteTaskCommand = new Command<TodoItem>(OnTaskCompleteRequest);

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshTasks();
        }

        private void RefreshTasks()
        {
            TodayTasks.Clear();

            DateTime today = DateTime.Today;

            var todayTasks = AppData.ActiveTasks
                .Where(t => t.Date.HasValue && t.Date.Value.Date == today)
                .ToList();

            foreach (var task in todayTasks)
                TodayTasks.Add(task);
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


        private async void OnTaskCompleteRequest(TodoItem item)
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

            // Перемещение задачи
            AppData.ActiveTasks.Remove(item);
            AppData.CompletedTasks.Add(item);
            item.IsCompleted = true;

            RefreshTasks();
        }
    }
}