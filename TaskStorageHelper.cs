using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CyberSecurityChatbot_PART_2
{
    public class TaskStorageHelper
    {
        private const string FilePath = "tasks.json";

        // Loads all tasks from tasks.json
        public List<CyberTask> LoadTasks()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return new List<CyberTask>();
                }

                string json = File.ReadAllText(FilePath);

                return JsonConvert.DeserializeObject<List<CyberTask>>(json)
                       ?? new List<CyberTask>();
            }
            catch
            {
                return new List<CyberTask>();
            }
        }

        // Saves all tasks to tasks.json
        public void SaveTasks(List<CyberTask> tasks)
        {
            try
            {
                string json = JsonConvert.SerializeObject(
                    tasks,
                    Formatting.Indented);

                File.WriteAllText(FilePath, json);
            }
            catch
            {
                // Prevent crash if file write fails
            }
        }

        // Add new task
        public void AddTask(
            string title,
            string description,
            string reminder)
        {
            List<CyberTask> tasks = LoadTasks();

            int newId = 1;

            if (tasks.Count > 0)
            {
                newId = tasks[tasks.Count - 1].Id + 1;
            }

            CyberTask task = new CyberTask
            {
                Id = newId,
                Title = title,
                Description = description,
                Reminder = reminder,
                IsComplete = false,
                CreatedAt = DateTime.Now
                    .ToString("yyyy-MM-dd HH:mm")
            };

            tasks.Add(task);

            SaveTasks(tasks);
        }

        // Mark task complete
        public void MarkAsComplete(int id)
        {
            List<CyberTask> tasks = LoadTasks();

            CyberTask task =
                tasks.Find(t => t.Id == id);

            if (task != null)
            {
                task.IsComplete = true;

                SaveTasks(tasks);
            }
        }

        // Delete task
        public void DeleteTask(int id)
        {
            List<CyberTask> tasks = LoadTasks();

            tasks.RemoveAll(t => t.Id == id);

            SaveTasks(tasks);
        }

        // Update an existing task by id
        public void UpdateTask(int id, string title, string description, string reminder, bool isComplete)
        {
            List<CyberTask> tasks = LoadTasks();

            var task = tasks.Find(t => t.Id == id);

            if (task != null)
            {
                task.Title = title;
                task.Description = description;
                task.Reminder = reminder;
                task.IsComplete = isComplete;

                SaveTasks(tasks);
            }
        }
    }
}