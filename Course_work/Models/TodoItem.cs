namespace Course_work.Models
{
    public class TodoItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;

        // Приоритет: "HighUrgent", "High", "Urgent", "Low"
        public string Priority { get; set; } = "Low";
    }
}