using System.Collections.ObjectModel;
using Course_work.Models;

namespace Course_work
{
    public static class AppData
    {
        public static ObservableCollection<TodoItem> ActiveTasks { get; set; }
            = new ObservableCollection<TodoItem>();

        public static ObservableCollection<TodoItem> CompletedTasks { get; set; }
            = new ObservableCollection<TodoItem>();
    }
}