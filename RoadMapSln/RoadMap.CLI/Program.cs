// See https://aka.ms/new-console-template for more information
using RoadMap.CLI.TaskTracker;


var tasksService = new TasksService();

var input = "";

while (!input!.Equals("Exit"))
{
    input = Console.ReadLine();

    var arguments = input?.Split(" ");

    var operation = arguments?[0] ?? ""; // add,delete,update / mark-{} / list
    if (tasksService.IsAddUpdateDeleteOperation(operation))
    {
        try
        {
            HandleAddUpdateDeleteOperations(tasksService, arguments, operation);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
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
        }
    }
    else if (operation.StartsWith("list"))
    {

    }
    else
    {
        Console.WriteLine($"Invalid operation: {operation}");
    }
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

static void HandleAddUpdateDeleteOperations(TasksService tasksService, string[]? arguments, string operation)
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