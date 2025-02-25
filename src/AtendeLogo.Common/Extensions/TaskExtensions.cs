namespace AtendeLogo.Common.Extensions;

public static class TaskExtensions
{
    public static object? GetResult(this Task task)
    {
        var taskType = task.GetType();
        if (taskType.IsGenericType)
        {
            var resultProperty = taskType.GetProperty("Result");
            return resultProperty?.GetValue(task);
        }
        return null;
    }
}
