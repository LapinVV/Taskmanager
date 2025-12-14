using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Course_work.Models
{
    [XmlRoot("TaskData")]
    public class DataStorage
    {
        [XmlArray("ActiveTasks")]
        [XmlArrayItem("Task")]
        public List<TodoItem> ActiveTasks { get; set; } = new List<TodoItem>();

        [XmlArray("CompletedTasks")]
        [XmlArrayItem("Task")]
        public List<TodoItem> CompletedTasks { get; set; } = new List<TodoItem>();

        [XmlElement("LastSave")]
        public DateTime LastSaveTime { get; set; } = DateTime.Now;
        
        [XmlElement("Version")]
        public string Version { get; set; } = "1.0";
    }
}