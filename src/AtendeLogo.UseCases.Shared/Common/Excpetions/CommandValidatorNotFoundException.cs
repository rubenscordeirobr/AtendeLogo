namespace AtendeLogo.UseCases.Common.Excpetions;

public class CommandValidatorNotFoundException : Exception
{
    public List<Type> CommandTypes { get; }
    public override string Message
        => GetMessage();

    public CommandValidatorNotFoundException(List<Type> commandTypes)
    {
        CommandTypes = commandTypes;
    }

    private string GetMessage()
    {
        var commandNames = CommandTypes.Select(type => type.Name);
        var message = $"Validation not found for commands: {string.Join(", ", commandNames)}";
        return message;
    }

    public override string ToString()
    {
        return $"{GetType().Name}.{Message}";
    }
}
