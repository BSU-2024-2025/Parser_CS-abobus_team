using System;

namespace Calculator;

public static class Compiler
{

  private static readonly Stack<Operation> operations = new();
  private static readonly Stack<object?> data = new();


  public static void PushNumber(object? number)
  {
    data.Push(number);
  }

  public static void PushData(object? obj)
  {
    data.Push(obj);
  }

  public static void PushOperation(char symbol)
  {
    operations.Push(GetOperation(symbol));
  }

  public static void PushOperation(Operation op)
  {
    operations.Push(op);
  }

  public static Operation GetOperation(char symbol)
  {
    switch (symbol)
    {
      case (char)Operation.Add:
        return Operation.Add;
      case (char)Operation.Subtract:
        return Operation.Subtract;
      case (char)Operation.Multiply:
        return Operation.Multiply;
      case (char)Operation.Divide:
        return Operation.Divide;
      default:
        return Operation.End;
    }
  }

  public static object? PopNumber()
  {
    return data.Pop();
  }


  public static Operation PopOperation()
  {
    return operations.Pop();
  }


  public static object? GetResult()
  {
    return PopNumber();
  }


  public static void Execute(Operation op)
  {
    switch (op)
    {
      case Operation.Add:
        {
          var operand2 = PopNumber();
          var operand1 = PopNumber();

          if (operand1 is int && operand2 is int) 
          {
            PushNumber((int)operand1 + (int)operand2); 
          } 
          else if (operand1 is string && operand2 is string) 
          { 
            PushNumber((string)operand1 + (string)operand2); 
          } 
          else 
          { 
            throw new InvalidOperationException("Unsupported operand types"); 
          }
          break;
        }

      case Operation.Subtract:
        PushNumber(-(int?)PopNumber() + (int?)PopNumber());
        break;
      case Operation.Multiply:
        PushNumber((int?)PopNumber() * (int?)PopNumber());
        break;
      case Operation.Divide:
        PushNumber(1 / (int?)PopNumber() * (int?)PopNumber());
        break;
      case Operation.UnaryMinus:
        PushNumber(-(int?)PopNumber());
        break;
      default:
        break;

    }
  }

  public static int GetPriority(char op)
  {
    switch (op)
    {
      case (char)Operation.Add:
      case (char)Operation.Subtract:
        return 1;
      case (char)Operation.Multiply:
      case (char)Operation.Divide:
        return 3;
      case (char)Operation.LeftBracket:
        return 0;
      case (char)Operation.End:
        return 0;
      default:
        return 0;
    }
  }

  public static int GetPriority(Operation op)
  {
    switch (op)
    {
      case Operation.Add:
      case Operation.Subtract:
        return 1;
      case Operation.Multiply:
      case Operation.Divide:
      case Operation.UnaryMinus:
        return 3;
      case Operation.LeftBracket:
        return 0;
      case Operation.End:
        return 0;
      default:
        return 0;
    }
  }

  public static Operation PeekOperation()
  {
    return operations.Peek();
  }
  public static void ExecuteMany(char s)
  {
    int currentPriority = GetPriority(s);
    while (operations.Count > 0 && currentPriority < GetPriority(PeekOperation()))
    {
      Execute(PopOperation());
    }
  }

  public static void ExecuteParanthesis()
  {
    var a = PopOperation();
    while (a != Operation.LeftBracket)
    {
      Execute(a);
      a = PopOperation();
    }
  }
}