namespace Interpreter;

public class CommandList
{
    private readonly List<Command> commands = [];
    
    public int GetCommandCount() => commands.Count;
    public void AddOperator(int index, object value)
    {
        Add(index, CommandType.Operator, value);
    }

    public void AddIf(int index, out Command command)
    {
        command = new Command(index, CommandType.If);
        Add(command);
    }
    
    public void AddFunction(int index, object name)
    {
        var command = new Command(index, CommandType.Function, (name, GetCommandCount()));
        // ToDo Add function to dictionary

        Add(command);
    }
    private void Add(Command command)
    {
        commands.Add(command);
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


    public void AddPopStack(int index)
    {
        Add(index, CommandType.PopStack);
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
  public void AddJump(int index, out Command command)
  {
    command = new Command(index, CommandType.Jump);
    Add(command);
  }
  private void Add(int index, CommandType type, object value)
    {
        commands.Add(new Command(index, type, value));
    }
    private void Add(int index, CommandType type)
    {
        commands.Add(new Command(index, type));
    }
    public List<Command> GetCommands()
    {
        return commands;
    }

    public void AddLocalVariable(int currentIndex, string s)
    {
        commands.Add(new Command(currentIndex, CommandType.LocalVariable, s));
    }


  public void AddCallFunction(int currentIndex, string name)
  {
    commands.Add(new Command(currentIndex, CommandType.CallFunction, name));
  }
}