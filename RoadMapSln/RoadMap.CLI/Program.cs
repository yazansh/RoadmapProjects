// See https://aka.ms/new-console-template for more information

using RoadMap.CLI.TaskTracker;

var tasksService = new TasksService();
//var validOperations = tasksService.ValidateOpertion(input);

var input = "";

while (!input!.Equals("Exit"))
{
    input = Console.ReadLine();

    var arguments = input?.Split(" ");

    var operation = arguments?[0] ?? ""; // add,delete,update / mark-{} / list
    if (!operation.StartsWith("mark") && !operation.StartsWith("list"))
    {
        try
        {
            switch (operation)
            {
                case "add":
                    {
                        var description = string.Join(" ", arguments[1..]);
                        tasksService.AddTask(description.Trim('"'));
                        break;
                    }
                case "update":
                    {
                        var description = string.Join(" ", arguments[2..]);

                        if (int.TryParse(arguments[1], out var id))
                            tasksService.UpdateTask(int.Parse(arguments[1]), description.Trim('"'));
                        else
                            Console.WriteLine($"Invalid id argument: {arguments[1]}");

                        break;
                    }
                case "delete":
                    {
                        if (int.TryParse(arguments[1], out var id))
                            tasksService.DeleteTask(id);
                        else
                            Console.WriteLine($"Invalid id argument: {arguments[1]}");
                        break;
                    }
                default:
                    Console.WriteLine($"Invalid Operation: {operation}");
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
    // else if mark
    // TODO:

    // TODO:
    // else if list
}


