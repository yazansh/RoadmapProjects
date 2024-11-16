using RoadMap.CLI.TaskTracker.Utils;

namespace RoadMap.CLI.TaskTracker
{
    public class TasksService
    {
        private readonly string[] _validStatuses = ["todo", "in-progress", "done"];
        public readonly string[] _validOperations = ["add", "update", "delete"];
        
        private readonly TasksFileManager _tasksFileManager;


        public TasksService()
        {
            _tasksFileManager = new TasksFileManager();
        }

        public bool IsAddUpdateDeleteOperation(string opertiona)
        {
            return _validOperations.Contains(opertiona);
        }

        public bool ValidateStatus(string? status)
        {
            return _validStatuses.Contains(status);
        }

        public string AddTask(string description)
        {
            try
            {
                var task = _tasksFileManager.AddTask(description);
                return $"Task added successfully (ID: {task.Id})";
            }
            catch (Exception e)
            {
                return $"Error: {e.Message}";
            }
        }

        public void UpdateTask(int id, string description)
        {
            _tasksFileManager.UpdateTask(id, description);
        }

        public void DeleteTask(int id)
        {
            _tasksFileManager.DeleteTask(id);
        }

        internal void SetStatus(int id, string status)
        {
            _tasksFileManager.SetStatus(id, status);
        }

        internal List<Task> GetTasks(string? status)
        {
            return _tasksFileManager.GetTasks(status);
        }
    }
}
