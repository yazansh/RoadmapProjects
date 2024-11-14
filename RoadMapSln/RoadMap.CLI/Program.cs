// See https://aka.ms/new-console-template for more information

using RoadMap.CLI.TaskTracker;
using System.Text;

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
                        var message = tasksService.AddTask(description.Trim('"'));
                        Console.WriteLine(message);
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
            continue;
        }
    }
    // else if mark
    // TODO:
    else if (operation.StartsWith("mark"))
    {
        try
        {
            var status = operation["mark-".Length..];

            var validationMessages = ValidateInputs(tasksService, arguments, status, out int id);
            if (validationMessages?.Any() ?? false)
            {
                foreach (var validationMessage in validationMessages)
                    Console.WriteLine(validationMessage);

                continue;
            }

            tasksService.SetStatus(id, status);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            continue;
        }
    }

    // TODO:
    // else if list
}

static List<string> ValidateInputs(TasksService tasksService, string[]? arguments, string status, out int id)
{
    var validationMessage = new List<string>();

    var idArgument = arguments?.Length > 1 ? arguments[1] : null;

    if (string.IsNullOrEmpty(idArgument))
        validationMessage.Add($"Invalid id ARgument: {arguments?[1]}");

    if (!int.TryParse(idArgument, out id))
        validationMessage.Add($"Invalid id argument: {arguments?[1]}");

    if (!tasksService.ValidationStatus(status))
        validationMessage.Add($"Invalid status: {status}");

    return validationMessage;
}