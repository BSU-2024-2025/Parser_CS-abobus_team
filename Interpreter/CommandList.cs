namespace Interpreter;

public class CommandList
{
    private readonly List<Command> _commands = [];
    public void AddOperator(int index, object value)
    {
        Add(index, CommandType.Operator, value);
    }
    
    public void AddEndExpression(int index)
    {
        Add(index, CommandType.EndExpression);
    }
    public void AddReturn(int index)
    {
        Add(index, CommandType.Return);
    }
    public void AddAssign(int index, object value)
    {
        Add(index, CommandType.Assign,value);
    }
    public void AddVariable(int index, object value)
    {
        Add(index, CommandType.Variable, value);
    }
    public void AddConstant(int index, object value)
    {
        Add(index, CommandType.Constant, value);
    }
    public void AddConstVariable(int index,object value)
    {
        Add(index, CommandType.ConstVariable, value);
    }
    private void Add(int index, CommandType type, object value)
    {
        _commands.Add(new Command(index, type, value));
    }
    private void Add(int index, CommandType type)
    {
        _commands.Add(new Command(index, type));
    }
    public List<Command> GetCommands()
    {
        return _commands;
    }
}