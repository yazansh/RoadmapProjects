namespace RoadMap.CLI.TaskTracker
{
    /// <summary>
    /// Task URL: https://roadmap.sh/projects/task-tracker
    /// </summary>
    public class Task
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = Enums.Status.Todo;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        public override string ToString()
        {
            return $"Id=> {Id}. Description=> {Description}. Status=> {Status}";
        }
    }
}
