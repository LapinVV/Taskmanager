using System.Collections.Generic;
using Course_work.Models;

namespace Course_work
{
    public static class AppData
    {
        public static List<TodoItem> Tasks { get; } = new List<TodoItem>();
    }
}