using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Interpreter;

public class Compiler(string input)
{
  private string input = input;
  private List<Command> commands = new();
  private readonly MyStack<string> operations = new();

  private readonly MyStack<object?> data = new();

  //private readonly Dictionary<string, object?> variables = new();
  //private readonly Dictionary<string, LocalDictionary> functions = new();
  private int nestLevel = 0;
  private LocalDictionary? context = null;
  private Parser parser = null;


  //public Compiler(List<Command> commands)
  //{
  //	this.commands = commands;
  //}
  //
  //public Compiler(string input)
  //{
  //  var p = new Parser(input);
  //  this.commands = p.Parse();
  //}

  public object? Compile()
  {
    parser = new Parser(input);
    commands.Clear();
    commands = parser.Parse();
    int bp = 0;
    string? funcName = null;
    int dim = 0;
    int index1 = 0;
    int index2 = 0;

    void GetIndexes()
    {
      if (dim > 0)
      {
        index1 = (int)PopData();
      }
      if (dim > 1)
      {
        index2 = (int)PopData();
      }
    }

    for (var i = 0; i < commands.Count; i++)
    {
      var command = commands[i];
      switch (command.CommandType)
      {
        case CommandType.Constant:
          {
            PushData(command.Value!);
            break;
          }
        case CommandType.GetGlobalIndexed:
        case CommandType.GetGlobal:
          {
            dim = 0;
            if (command.CommandType == CommandType.GetGlobalIndexed)
            {
              dim = (int)PopData();
              GetIndexes();
            }

            if (parser.HasGlobalVariable((string)command.Value!))
            {
              var varValue = parser.variables[(string)command.Value!];
              if (varValue != null)
              {
                if (dim > 0)
                {
                  varValue = GetVariableIndexed(varValue, dim, index1, index2);
                }
                PushData(varValue!);
              }
              else
              {
                throw new ApplicationException($"Global variable '{command.Value}' is not defined.");
              }
            }
            else
            {
              throw new ApplicationException($"Global variable '{command.Value}' is not found.");
            }

            break;
          }

        case CommandType.GetLocalIndexed:
        case CommandType.GetLocal:
          {
            dim = 0;
            if (command.CommandType == CommandType.GetLocalIndexed)
            {
              dim = (int)PopData();
              GetIndexes();
            }

            var varValue = data.PeekByIndex(bp - (int)command.Value!)!;
            if (varValue != null)
            {
              if (dim > 0)
              {
                varValue = GetVariableIndexed(varValue, dim, index1, index2);
              }
              PushData(varValue!);
            }
            break;
          }
        case CommandType.SetLocalIndexed:
        case CommandType.SetLocal:
          {
            dim = 0;
            if (command.CommandType == CommandType.SetGlobalIndexed)
            {
              dim = (int)PopData();
            }
            var v = PopData();
            var stackIndex = bp - (int)command.Value!;
            if (dim > 0)
            {
              GetIndexes();
              var arr = data.PeekByIndex(stackIndex);
              if (arr != null)
              {
                SetIndexedVariable(arr, v, dim, index1, index2);
              }
              else throw new ApplicationException("Local array is not defined");
            }
            else
            {
              data.SetByIndex(stackIndex, v);
            }
            break;
          }
        case CommandType.Operator:
          {
            if (command.Value != null)
            {
              switch ((string)command.Value)
              {
                case "(":
                  PushOperator("(");
                  break;
                case ")":
                  ExecuteParenthesis();
                  break;
                default:
                  ExecuteOperators((string)command.Value);
                  PushOperator((string)command.Value);
                  break;
              }
            }

            break;
          }

        case CommandType.SetGlobalIndexed:
        case CommandType.SetGlobal:
          {
            dim = 0;
            if (command.CommandType == CommandType.SetGlobalIndexed)
            {
              dim = (int)PopData();
            }
            var v = PopData();
            var name = (string)command.Value!;
            if (dim > 0)
            {
              GetIndexes();
              var arr = parser.variables[name];
              if (arr != null)
              {
                SetIndexedVariable(arr, v, dim, index1, index2);
              }
              else throw new ApplicationException($"Global array {name} is not defined");

            }
            else 
            {
              parser.variables[name] = v;
            }
            break;
          }
        case CommandType.NewArray:
          {
            var size = (int)command.Value!;
            var a = new ArrayList(size);
            for (var j = size - 1; j >= 0; j--)
            {
              a.Add(data.Peek(j));
            }
            PopData(size);
            PushData(a);
            break;
          }
        //case CommandType.SetArrayGlobal:
        //  {
        //    var size = (int)PopData();
        //    var a = new ArrayList();
        //    for (var j = 0; j < size; j++)
        //    {
        //      a.Add(PopData());
        //    }
        //    SetArrayGlobal((string)command.Value!, a);
        //    bp = data.Count;
        //    break;
        //  }
        //case CommandType.GetArrayGlobal:
        //  {
        //    var index = (int)PopData();
        //    var x = (string)command.Value!;
        //    var d = (int)GetGlobalVariable(x)!;
        //    PushData(data.PeekByIndex(bp - d + index)!);
        //    break;
        //  }
        case CommandType.Return:
        case CommandType.Return0:
          if (nestLevel == 0 && GetOperatorsLength() != 0)
          {
            throw new Exception($"Not empty operators stack.");
          }

          if (nestLevel == 0)
          {
            return (GetDataLength() != 0 ? PeekData() : 0) ?? 0;
          }
          else
          {
            var result = (command.CommandType == CommandType.Return0 ? 0 : data.Pop());

            i = ReturnFunc(ref funcName!, ref bp, result);
            nestLevel--;

          }

          break;
        case CommandType.EndExpression:
          ExecuteOperators(Operator.End);
          break;
        case CommandType.If:
          if (!(bool)PopData())
          {
            i = (int)command.Value! - 1; // increment in for
          }

          break;
        case CommandType.Jump:
          i = (int)command.Value! - 1; // increment in for
          break;
        case CommandType.CallFunction:
          PushOperator("(");
          data.Push(i);
          data.Push(funcName);
          data.Push(bp);
          bp = data.Count;
          nestLevel++;
          funcName = (string)command.Value!;
          i = CallFunc((string)command.Value!) - 1; // increment in for
          break;
        case CommandType.PopStack:
          PopData();
          break;
        default: 
          throw new ApplicationException("Unknown command type");
      }
    }


    return null;
  }
  

  private void SetArrayGlobal(string name, ArrayList a)
  {
    parser.variables[name] = a.Count;
    for (var i = a.Count - 1; i >= 0; i--)
    {
      data.Push(a[i]);
    }
  }

  private int ReturnFunc(ref string funcName, ref int bp, object? result)
  {
    var func = parser.functions[funcName];

    PopData(func.localCount);

    bp = (int)data.Pop()!;
    funcName = (string)data.Pop()!;
    int curIndex = (int)data.Pop()!;

    PopData(func.paramCount);

    data.Push(result);
    PopOperator();  // pop '('
    return curIndex;
  }

  private int CallFunc(string funcName)
  {
    var func = parser.functions[funcName];

    for (int i = 0; i < func.localCount; i++)
    {
      data.Push(null);
    }

    return func.codeIndex;
  }


  private void Execute(string operation)
  {
    dynamic op2 = 0;
    dynamic op1 = 0;

    switch (operation)
    {
      case Operator.Add:
        {
          //var operand2 = PopData();
          //var operand1 = PopData();
          //if (operand1 is int && operand2 is int)
          //{
          //  PushData((int?)operand1 + (int?)operand2);
          //  break;
          //}

          //if (operand1 is float ||
          //    operand2 is float ||
          //    operand1 is double ||
          //    operand2 is double ||
          //    operand1 is decimal ||
          //    operand2 is decimal ||
          //    operand1 is int ||
          //    operand2 is int
          //   )
          //{
          //  PushData((double?)operand1 + (double?)operand2);
          //  break;
          //}

          //PushData((string?)operand1 + (string?)operand2);

          op2 = PopData();
          op1 = PopData();
          PushData(op1 + op2);

          break;
        }
      case Operator.Subtract:
        {
          op2 = PopData();
          op1 = PopData();
          PushData(op1 - op2);
          break;
        }
      case Operator.Multiply:
        {
          op2 = PopData();
          op1 = PopData();
          PushData(op1 * op2);
          break;
        }
      case Operator.Divide:
        op2 = PopData();
        op1 = PopData();
        PushData(op1 / op2);
        break;
      case Operator.UnaryMinus:
        PushData(-(dynamic)PopData());
        break;
      case Operator.Not:
        {
          var operand = PopData();
          if (operand is bool)
          {
            var res = !(bool)operand;
            PushData(res);
            break;
          }

          throw new InvalidOperationException("Not supported");
        }
      case Operator.MoreThan:
        {
          op2 = PopData();
          op1 = PopData();

          if (op2 is string && op1 is string)
          {
            PushData(((string)op1).CompareTo(op2) > 0);
            break;
          }

          PushData(op1 > op2);
          break;
        }
      case Operator.LessThan:
        {
          op2 = PopData();
          op1 = PopData();

          if (op2 is string && op1 is string)
          {
            PushData(((string)op1).CompareTo(op2) < 0);
            break;
          }

          PushData(op1 < op2);
          break;
        }
      case Operator.GreaterThanOrEqual:
        {
          op2 = PopData();
          op1 = PopData();

          if (op2 is string && op1 is string)
          {
            PushData( ((string)op1).CompareTo(op2) >= 0);
            break;
          }

          PushData(op1 >= op2);
          break;
        }
      case Operator.LessThanOrEqual:
        {
          op2 = PopData();
          op1 = PopData();

          if (op2 is string && op1 is string)
          {
            PushData(((string)op1).CompareTo(op2) <= 0);
            break;
          }

          PushData(op1 <= op2);
          break;
        }
      case Operator.Equal:
        {
          var operand1 = PopData();
          var operand2 = PopData();
          var res = operand1.Equals(operand2);
          PushData(res);
          break;
        }
      case Operator.And:
        {
          op2 = PopData();
          op1 = PopData();
          PushData(op1 && op2);

          //var operand1 = PopData();
          //var operand2 = PopData();
          //if (operand1 is bool && operand2 is bool)
          //{
          //  PushData((bool)operand2 && (bool)operand1);
          //}

          break;
        }
      case Operator.NotEqual:
        {
          var operand1 = PopData();
          var operand2 = PopData();
          var res = !(operand1.Equals(operand2));
          PushData(res);
          break;
        }
      case Operator.Or:
        {

          op2 = PopData();
          op1 = PopData();
          PushData(op1 || op2);

          //var operand1 = PopData();
          //var operand2 = PopData();
          //if (operand1 is bool && operand2 is bool)
          //{
          //  PushData((bool)operand2 || (bool)operand1);
          //}

          break;
        }
    }
  }

  private void ExecuteParenthesis()
  {
    var cur = PopOperator();
    while (!cur.Equals(Operator.LeftParenthesis))
    {
      Execute(cur);
      cur = PopOperator();
    }
  }

  private void ExecuteOperators(string operation)
  {
    var curPriority = GetPriority(operation);
    while (GetOperatorsLength() > 0 && curPriority < GetPriority(PeekOperator()))
    {
      Execute(PopOperator());
    }
  }

  private static int GetPriority(string operation)
  {
    return operation switch
    {
      Operator.Not or Operator.UnaryMinus or Operator.UnaryPlus => 400,
      Operator.Multiply or Operator.Divide => 300,
      Operator.Add or Operator.Subtract => 100,
      Operator.LessThan or Operator.LessThanOrEqual or Operator.Equal or Operator.NotEqual
          or Operator.GreaterThanOrEqual or Operator.MoreThan => 50,
      Operator.And => 40,
      Operator.Or => 20,
      Operator.LeftParenthesis => 0,
      Operator.End => 0,
      _ => 0
    };
  }

  private object? PeekData()
  {
    return data.Peek();
  }

  private object PopData()
  {
    return data.Pop()!;
  }

  private void PopData(int size)
  {
    for (var j = 0; j < size; j++)
    {
      PopData();
    }
  }

  private void PushData(object data)
  {
    this.data.Push(data);
  }

  private int GetDataLength()
  {
    return data.Count;
  }

  private int GetOperatorsLength()
  {
    return operations.Count;
  }

  private void PushOperator(string data)
  {
    operations.Push(data);
  }

  private string PopOperator()
  {
    return operations.Pop();
  }

  private string PeekOperator()
  {
    return operations.Peek();
  }

  private object? GetVariableIndexed(object v, int dim = 0, int index1 = 0, int index2 = 0)
  {
    if (dim == 0)
      return v;
    else if (dim == 1)
      return (v as ArrayList)![index1];
    else if (dim == 2)
      return ((v as ArrayList)![index1] as ArrayList)![index2];
    else
      throw new ApplicationException($"Invalid Array dimension {dim}.");
  }

  private object? GetGlobalVariable(string name, int dim = 0, int index1 = 0, int index2 = 0)
  {
    if (dim == 0)
      return parser.variables[name];
    else if (dim == 1)
      return (parser.variables[name] as ArrayList)![index1];
    else if (dim == 2)
      return ((parser.variables[name] as ArrayList)![index1] as ArrayList)![index2];
    else
      throw new ApplicationException($"Invalid Array dimension {dim}.");
  }


  //private void AddGlobalVariable(string name)
  //{
  //    if (!HasVariable(name))
  //    {
  //        parser.variables.Add(name, null);
  //    }
  //}

  //private void SetGlobalVariable(string name, object? value, int dim = 0, int index1 = 0, int index2 = 0)
  //{
  //  if (dim == 0)
  //    parser.variables[name] = value;
  //  else if (dim == 1)
  //    (parser.variables[name] as ArrayList)![index1] = value;
  //  else if (dim == 2)
  //    ((parser.variables[name] as ArrayList)![index1] as ArrayList)![index2] = value;
  //  else
  //    throw new ApplicationException($"Invalid Array dimension {dim}.");
  //}

  private void SetIndexedVariable(object o, object? value, int dim = 0, int index1 = 0, int index2 = 0)
  {
    if (dim == 0)
      o = value!;
    else if (dim == 1)
      (o as ArrayList)![index1] = value;
    else if (dim == 2)
      ((o as ArrayList)![index1] as ArrayList)![index2] = value;
    else
      throw new ApplicationException($"Invalid Array dimension {dim}.");
  }
}