using System.Collections.ObjectModel;
using Course_work.Models;

namespace Course_work
{
    public static class AppData
    {
        // Активные задачи (All)
        public static ObservableCollection<TodoItem> ActiveTasks { get; set; }
            = new ObservableCollection<TodoItem>();

        // Выполненные задачи (Completed)
        public static ObservableCollection<TodoItem> CompletedTasks { get; set; }
            = new ObservableCollection<TodoItem>();
    }
}