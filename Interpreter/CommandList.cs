using System.Xml.Linq;

namespace Interpreter;

// public class IndexedValue
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
  public void AddReturn0(int index)
  {
    Add(index, CommandType.Return0);
  }

  public void AddPopStack(int index)
  {
    Add(index, CommandType.PopStack);
  }
  //public void AddVariable(int index, object value)
  //{
  //  Add(index, CommandType.Variable, value);
  //}
  public void AddConstant(int index, object value)
  {
    Add(index, CommandType.Constant, value);
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
  public void Add(int index, CommandType type)
  {
    commands.Add(new Command(index, type));
  }
  public List<Command> GetCommands()
  {
    return commands;
  }

  public void GetLocalVariable(int currentIndex, int offset, int dim = 0)
  {
    if (dim == 0)
    {
      commands.Add(new Command(currentIndex, CommandType.GetLocal, offset));
    }
    else
    {
      commands.Add(new Command(currentIndex, CommandType.Constant, dim));
      commands.Add(new Command(currentIndex, CommandType.GetLocalIndexed, offset));
    }
  }

  public void SetLocalVariable(int currentIndex, int offset, int dim = 0)
  {
    if (dim == 0)
    {
      commands.Add(new Command(currentIndex, CommandType.SetLocal, offset));
    }
    else
    {
      commands.Add(new Command(currentIndex, CommandType.Constant, dim));
      commands.Add(new Command(currentIndex, CommandType.SetLocalIndexed, offset));
    }

  }

  public void GetGlobalVariable(int currentIndex, string name, int dim = 0)
  {
    if (dim == 0)
    {
      commands.Add(new Command(currentIndex, CommandType.GetGlobal, name));
    }
    else
    {
      commands.Add(new Command(currentIndex, CommandType.Constant, dim));
      commands.Add(new Command(currentIndex, CommandType.GetGlobalIndexed, name));
      //commands.Add(new Command(currentIndex, CommandType.GetGlobalIndexed, new GlobalIndexed(name, dim)); 
    }
  }
  public void SetGlobalVariable(int currentIndex, string name, int dim = 0)
  {
    if (dim == 0)
    {
      commands.Add(new Command(currentIndex, CommandType.SetGlobal, name));
    }
    else
    {
      commands.Add(new Command(currentIndex, CommandType.Constant, dim));
      commands.Add(new Command(currentIndex, CommandType.SetGlobalIndexed, name));
    }
  }

  public void AddCallFunction(int currentIndex, string name)
  {
    commands.Add(new Command(currentIndex, CommandType.CallFunction, name));
  }

  public void AddNewArray(int currentIndex, int size)
  {
    commands.Add(new Command(currentIndex, CommandType.NewArray, size));
  }
}

//  public void SetArrayLocal(int currentIndex, int name)
//  {
//    commands.Add(new Command(currentIndex, CommandType.SetArrayLocal, name));
//  }

//  public void SetArrayGlobal(int currentIndex, string name)
//  {
//    commands.Add(new Command(currentIndex, CommandType.SetArrayGlobal, name));
//  }

//  public void GetArrayGlobal(int currentIndex, string name)
//  {
//    commands.Add(new Command(currentIndex, CommandType.GetArrayGlobal, name));
//  }
//}

//public class GlobalIndexed
//{
//  private string name;
//  private int dim;

//  public GlobalIndexed(string name, int dim)
//  {
//    this.name = name;
//    this.dim = dim;
//  }
//}
