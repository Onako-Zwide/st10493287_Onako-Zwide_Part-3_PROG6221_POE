using System.Collections.Generic;

namespace CyberSecurityChatbot_PART_2
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;

        public TaskManager()
        {
            _storage = new TaskStorageHelper();
        }

        public string AddTask(
            string title,
            string description,
            string reminder)
        {
            _storage.AddTask(
                title,
                description,
                reminder);

            ActivityLogger.Log(
                $"Task added: '{title}'");

            return $"Task added: '{title}'.";
        }

        public List<CyberTask> GetAllTasks()
        {
            return _storage.LoadTasks();
        }

        public void ToggleComplete(int id)
        {
            var tasks = _storage.LoadTasks();

            var task = tasks.Find(t => t.Id == id);

            if (task != null)
            {
                task.IsComplete = !task.IsComplete;
                _storage.SaveTasks(tasks);

                ActivityLogger.Log($"Task {(task.IsComplete ? "marked complete" : "marked incomplete")} (ID {id})");
            }
        }

        public void UpdateTask(int id, string title, string description, string reminder, bool isComplete)
        {
            _storage.UpdateTask(id, title, description, reminder, isComplete);

            ActivityLogger.Log($"Task updated (ID {id})");
        }

        public void SetReminder(int id, string reminder)
        {
            var tasks = _storage.LoadTasks();

            var task = tasks.Find(t => t.Id == id);

            if (task != null)
            {
                task.Reminder = reminder;
                _storage.SaveTasks(tasks);

                ActivityLogger.Log($"Reminder set for task ID {id}: {reminder}");
            }
        }

        public void MarkAsComplete(int id)
        {
            _storage.MarkAsComplete(id);

            ActivityLogger.Log(
                $"Task marked complete (ID {id})");
        }

        public void DeleteTask(int id)
        {
            _storage.DeleteTask(id);

            ActivityLogger.Log(
                $"Task deleted (ID {id})");
        }
    }
}