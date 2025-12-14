using System.Collections.ObjectModel;
using System.ComponentModel;
using Course_work.Models;

namespace Course_work
{
    public static class AppData
    {
        private static ObservableCollection<TodoItem> _activeTasks = new ObservableCollection<TodoItem>();
        private static ObservableCollection<TodoItem> _completedTasks = new ObservableCollection<TodoItem>();

        public static ObservableCollection<TodoItem> ActiveTasks
        {
            get => _activeTasks;
            set
            {
                if (_activeTasks != value)
                {
                    if (_activeTasks != null)
                    {
                        _activeTasks.CollectionChanged -= OnActiveTasksCollectionChanged;
                        foreach (var item in _activeTasks)
                            UnsubscribeFromItemChanges(item);
                    }
                    
                    _activeTasks = value;
                    
                    if (_activeTasks != null)
                    {
                        foreach (var item in _activeTasks)
                            SubscribeToItemChanges(item);
                        
                        _activeTasks.CollectionChanged += OnActiveTasksCollectionChanged;
                    }
                }
            }
        }

        public static ObservableCollection<TodoItem> CompletedTasks
        {
            get => _completedTasks;
            set
            {
                if (_completedTasks != value)
                {
                    if (_completedTasks != null)
                    {
                        _completedTasks.CollectionChanged -= OnCompletedTasksCollectionChanged;
                        foreach (var item in _completedTasks)
                            UnsubscribeFromItemChanges(item);
                    }
                    
                    _completedTasks = value;
                    
                    if (_completedTasks != null)
                    {
                        foreach (var item in _completedTasks)
                            SubscribeToItemChanges(item);
                        
                        _completedTasks.CollectionChanged += OnCompletedTasksCollectionChanged;
                    }
                }
            }
        }

        private static void OnActiveTasksCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DataManager.OnDataChanged();
            
            if (e.NewItems != null)
            {
                foreach (TodoItem newItem in e.NewItems)
                    SubscribeToItemChanges(newItem);
            }
            
            if (e.OldItems != null)
            {
                foreach (TodoItem oldItem in e.OldItems)
                    UnsubscribeFromItemChanges(oldItem);
            }
        }

        private static void OnCompletedTasksCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DataManager.OnDataChanged();
            
            if (e.NewItems != null)
            {
                foreach (TodoItem newItem in e.NewItems)
                    SubscribeToItemChanges(newItem);
            }
            
            if (e.OldItems != null)
            {
                foreach (TodoItem oldItem in e.OldItems)
                    UnsubscribeFromItemChanges(oldItem);
            }
        }

        private static void SubscribeToItemChanges(TodoItem item)
        {
            item.PropertyChanged -= OnItemPropertyChanged;
            item.PropertyChanged += OnItemPropertyChanged;
        }

        private static void UnsubscribeFromItemChanges(TodoItem item)
        {
            item.PropertyChanged -= OnItemPropertyChanged;
        }

        private static void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TodoItem.Title) || 
                e.PropertyName == nameof(TodoItem.Description) ||
                e.PropertyName == nameof(TodoItem.Priority) ||
                e.PropertyName == nameof(TodoItem.Date) ||
                e.PropertyName == nameof(TodoItem.Time) ||
                e.PropertyName == nameof(TodoItem.ReminderMinutes))
            {
                DataManager.OnDataChanged();
            }
        }
        
        public static void SaveAllData()
        {
            DataManager.ForceSave();
        }
    }
}