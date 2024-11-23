namespace Interpreter;

public class Command
{
    public Command()
    {
    }

    public Command(int index, CommandType type, object? value)
    {
        CommandType = type;
        IndexText = index;
        Value = value;
    }

    public Command(int index, CommandType type)
    {
        IndexText = index;
        CommandType = type;
    }

    public CommandType CommandType { get; set; }
    public int IndexText { get; set; }
    public object? Value { get; set; }
}