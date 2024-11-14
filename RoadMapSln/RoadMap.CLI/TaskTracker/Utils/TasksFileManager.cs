using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace RoadMap.CLI.TaskTracker.Utils
{
    public class TasksFileManager
    {

        private static readonly string _tasksFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\tasks.txt";
        //private static int _id = 0;

        public TasksFileManager()
        {
            if (!File.Exists(_tasksFilePath))
            {
                File.Create(_tasksFilePath);
                //var tasksJson = JsonSerializer.Serialize(new List<Task>());
                //File.WriteAllText(_tasksFilePath, tasksJson);
            }

        }

        public Task AddTask(string desciprtions)
        {
            var tasksText = File.ReadAllText(_tasksFilePath);
            var tasks = !string.IsNullOrEmpty(tasksText) ?
                JsonSerializer.Deserialize<List<Task>>(tasksText) ?? throw new Exception("Error while reading tasks from json file!")
                : new List<Task>();

            var latestId = tasks.LastOrDefault()?.Id ?? 0;

            var task = new Task() { Id = ++latestId, Description = desciprtions };

            
            tasks.Add(task);

            var tasksSerialized = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });


            File.WriteAllText(_tasksFilePath, tasksSerialized);

            return task;
        }

        public void UpdateTask(int id, string description)
        {
            var tasksText = File.ReadAllText(_tasksFilePath);

            var tasks = JsonSerializer.Deserialize<List<Task>>(tasksText) ?? throw new Exception("Error while reading tasks from json file!");
            var taskToUpdate = tasks.FirstOrDefault(t => t.Id.Equals(id)) ?? throw new Exception("Task Not found!");
            
            taskToUpdate.Description = description;
            taskToUpdate.UpdatedAt = DateTime.Now;

            File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true }));
        }


        public void DeleteTask(int id)
        {
            var tasksText = File.ReadAllText(_tasksFilePath);
            
            var tasks = JsonSerializer.Deserialize<List<Task>>(tasksText) ?? throw new Exception("Error while reading tasks from json file!");
            var tasksToDelete = tasks.FirstOrDefault(task => task.Id.Equals(id)) ?? throw new Exception("Task Not found!");

            tasks.Remove(tasksToDelete);

            File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
