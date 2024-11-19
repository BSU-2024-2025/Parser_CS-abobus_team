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
      case Operation.Negation:
        {
          var operand = PopNumber();

          if (operand is bool)
          {
            var res = !(bool)operand;
            PushNumber(!(bool)operand);
          }
          else
          {
            throw new InvalidOperationException("Unsupported operand types");
          }
          break;
        }
      case Operation.MoreThan:
        {
          var operand2 = PopNumber();
          var operand1 = PopNumber();
          PushNumber((int?)operand1 > (int?)operand2);
          break;
        }
      case Operation.MoreOrEqual:
        {
          var operand2 = PopNumber();
          var operand1 = PopNumber();
          PushNumber((int?)operand1 >= (int?)operand2);
          break;
        }
      case Operation.LessThan:
        {
          var operand2 = PopNumber();
          var operand1 = PopNumber();
          PushNumber((int?)operand1 < (int?)operand2);
          break;
        }
      case Operation.LessOrEqual:
        {
          var operand2 = PopNumber();
          var operand1 = PopNumber();
          PushNumber((int?)operand1 <= (int?)operand2);
          break;
        }
      case Operation.LogicalAnd:
        {
          var operand2 = PopNumber();
          var operand1 = PopNumber();
          if (operand1 is bool && operand2 is bool)
          {
            PushNumber((bool)operand1 && (bool)operand2);
          }
          break;
        }
      case Operation.LogicalOr:
        {
          var operand2 = PopNumber();
          var operand1 = PopNumber();
          if (operand1 is bool && operand2 is bool)
          {
            PushNumber((bool)operand1 || (bool)operand2);
          }
          break;
        }
      default:
        break;

    }
  }



  public static int GetPriority(Operation op)
  {
    switch (op)
    {
      case Operation.Negation:
      case Operation.UnaryMinus:
        return 400;
      case Operation.Multiply:
      case Operation.Divide:
        return 300;
      case Operation.Add:
      case Operation.Subtract:
        return 100;
      case Operation.MoreThan:
      case Operation.MoreOrEqual:
      case Operation.LessThan:
      case Operation.LessOrEqual:
        return 50;
      case Operation.LogicalAnd:
        return 40;
      case Operation.LogicalOr:
        return 20;
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
  public static void ExecuteMany(Operation oper)
  {
    int currentPriority = GetPriority(oper);
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

  public static bool IsFalseable(this object value)
  {
    return value is sbyte
            || value is byte
            || value is short
            || value is ushort
            || value is int
            || value is uint
            || value is long
            || value is ulong
            || value is float
            || value is double
            || value is decimal;
  }
}

//public static int GetPriority(char op)
//{
//  switch (op)
//  {
//    case (char)Operation.Add:
//    case (char)Operation.Subtract:
//      return 100;
//    case (char)Operation.Multiply:
//    case (char)Operation.Divide:
//      return 300;
//    case (char)Operation.LeftBracket:
//      return 0;
//    case (char)Operation.End:
//      return 0;
//    case (char)Operation.Negation:
//      return 10;
//    default:
//      return 0;
//  }
//}