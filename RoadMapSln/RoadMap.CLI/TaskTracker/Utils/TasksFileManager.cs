using System.Reflection;
using System.Text.Json;

namespace RoadMap.CLI.TaskTracker.Utils
{
    public class TasksFileManager
    {

        private static readonly string _tasksFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\tasks.txt";
        private static readonly JsonSerializerOptions options = new() { WriteIndented = true };

        public TasksFileManager()
        {
            if (!File.Exists(_tasksFilePath))
                File.Create(_tasksFilePath);
        }

        public Task AddTask(string desciprtions)
        {
            var tasks = GetTasksInternal();

            var latestId = tasks.LastOrDefault()?.Id ?? 0;

            var task = new Task() { Id = ++latestId, Description = desciprtions };

            
            tasks.Add(task);

            var tasksSerialized = JsonSerializer.Serialize(tasks, options);


            File.WriteAllText(_tasksFilePath, tasksSerialized);

            return task;
        }

        public void UpdateTask(int id, string description)
        {
            var tasks = GetTasksInternal();

            var taskToUpdate = tasks.FirstOrDefault(t => t.Id.Equals(id)) ?? throw new Exception("Task Not found!");
            
            taskToUpdate.Description = description;
            taskToUpdate.UpdatedAt = DateTime.Now;

            
            File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(tasks, options));
        }


        public void DeleteTask(int id)
        {
            var tasks = GetTasksInternal();

            var tasksToDelete = tasks.FirstOrDefault(task => task.Id.Equals(id)) ?? throw new Exception("Task Not found!");

            tasks.Remove(tasksToDelete);

            File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(tasks, options));
        }

        public void SetStatus(int id, string status)
        {
            var tasks = GetTasksInternal();
            var taskToUpdate = tasks.FirstOrDefault(t => t.Id.Equals(id)) ?? throw new Exception("Task Not found!");

            taskToUpdate.Status = status;
            taskToUpdate.UpdatedAt = DateTime.Now;

            File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(tasks, options));
        }

        public List<Task> GetTasks(string? status)
        {
            var tasks = GetTasksInternal();

            if (string.IsNullOrEmpty(status)) return tasks;

            return tasks.Where(t => t.Status == status).ToList();
        }

        private static List<Task> GetTasksInternal()
        {
            var tasksText = File.ReadAllText(_tasksFilePath);

            var tasks = JsonSerializer.Deserialize<List<Task>>(tasksText) ?? throw new Exception("Error while reading tasks from json file!");
            return tasks;
        }
    }
}
