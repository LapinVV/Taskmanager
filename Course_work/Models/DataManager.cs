using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Course_work.Models
{
    public static class DataManager
    {
        private static readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.xml");

        public static void SaveData()
        {
            try
            {
                var data = new DataStorage
                {
                    ActiveTasks = AppData.ActiveTasks.ToList(),
                    CompletedTasks = AppData.CompletedTasks.ToList(),
                    LastSaveTime = DateTime.Now
                };

                foreach (var task in data.ActiveTasks)
                    task.UpdateDateDisplay();
                
                foreach (var task in data.CompletedTasks)
                    task.UpdateDateDisplay();

                var serializer = new XmlSerializer(typeof(DataStorage));
                
                using (var writer = new StreamWriter(FilePath))
                {
                    serializer.Serialize(writer, data);
                }
            }
            catch
            {
            }
        }

        public static void LoadData()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    AppData.ActiveTasks = new ObservableCollection<TodoItem>();
                    AppData.CompletedTasks = new ObservableCollection<TodoItem>();
                    return;
                }

                var serializer = new XmlSerializer(typeof(DataStorage));
                DataStorage? data;
                
                using (var reader = new StreamReader(FilePath))
                {
                    data = serializer.Deserialize(reader) as DataStorage;
                }
                
                if (data == null)
                {
                    AppData.ActiveTasks = new ObservableCollection<TodoItem>();
                    AppData.CompletedTasks = new ObservableCollection<TodoItem>();
                    return;
                }
                
                AppData.ActiveTasks = new ObservableCollection<TodoItem>();
                AppData.CompletedTasks = new ObservableCollection<TodoItem>();
                
                if (data.ActiveTasks != null)
                {
                    foreach (var task in data.ActiveTasks)
                    {
                        task.UpdateDateDisplay();
                        AppData.ActiveTasks.Add(task);
                    }
                }
                
                if (data.CompletedTasks != null)
                {
                    foreach (var task in data.CompletedTasks)
                    {
                        task.UpdateDateDisplay();
                        AppData.CompletedTasks.Add(task);
                    }
                }
            }
            catch
            {
                AppData.ActiveTasks = new ObservableCollection<TodoItem>();
                AppData.CompletedTasks = new ObservableCollection<TodoItem>();
            }
        }

        public static void ForceSave()
        {
            SaveData();
        }

        public static void OnDataChanged()
        {
            SaveData();
        }
    }
}