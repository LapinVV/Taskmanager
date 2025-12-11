using System;
using Course_work.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Course_work.Pages.Popups
{
    public partial class ViewTaskPopup : ContentView
    {
        public event EventHandler<TodoItem>? CompleteRequested;
        public event EventHandler<TodoItem>? DeleteRequested;
        public event EventHandler<TodoItem>? EditRequested;
        public event EventHandler? Closed;

        public ViewTaskPopup()
        {
            InitializeComponent();
        }

        public void Show(TodoItem item)
        {
            if (item == null) return;

            BindingContext = item;

            // Заголовок
            TitleLabel.Text = item.Title ?? string.Empty;
            TitleReadonly.Text = item.Title ?? string.Empty;

            // Описание
            if (string.IsNullOrWhiteSpace(item.Description))
            {
                DescriptionLabel.IsVisible = false;
                DescriptionLabel.Text = string.Empty;
            }
            else
            {
                DescriptionLabel.IsVisible = true;
                DescriptionLabel.Text = item.Description;
            }

            // Форматированная дата
            if (item.Date.HasValue)
            {
                string date = item.Date.Value.ToString("d MMMM yyyy");

                if (item.Time.HasValue)
                    date += " " + item.Time.Value.ToString(@"hh\:mm");

                DateTimeLabel.Text = $"Дата: {date}";
                DateTimeLabel.IsVisible = true;
            }
            else
            {
                DateTimeLabel.IsVisible = false;
                DateTimeLabel.Text = string.Empty;
            }

            // Цвет рамки по приоритету
            var priority = item.Priority ?? "Low";
            var color = Colors.Transparent;

            switch (priority)
            {
                case "HighUrgent":
                    color = Color.FromArgb("#d55043");
                    break;
                case "High":
                    color = Color.FromArgb("#FFF09515");
                    break;
                case "Urgent":
                    color = Color.FromArgb("#FFBD59");
                    break;
                default:
                    color = Color.FromArgb("#CCCCCC");
                    break;
            }

            TitleBorder.Stroke = color;

            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void OnCloseClicked(object? sender, EventArgs e)
        {
            Hide();
        }

        private void OnCompleteClicked(object? sender, EventArgs e)
        {
            if (BindingContext is TodoItem item)
                CompleteRequested?.Invoke(this, item);

            Hide();
        }

        private void OnDeleteClicked(object? sender, EventArgs e)
        {
            if (BindingContext is TodoItem item)
                DeleteRequested?.Invoke(this, item);

            Hide();
        }

        private void OnEditClicked(object? sender, EventArgs e)
        {
            if (BindingContext is TodoItem item)
                EditRequested?.Invoke(this, item);
        }
    }
}