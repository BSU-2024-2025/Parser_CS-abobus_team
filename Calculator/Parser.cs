using System;

namespace Calculator;

public static class Parser
{

  public static void Main()
  {
    //Console.WriteLine(Parser.Parse("1+2"));
    //Console.WriteLine(Parser.Parse("return;"));
    //Console.WriteLine(Parser.Parse("return 1;"));
    Console.WriteLine(Parser.Parse("return 1 + 2;"));
  }

  private static string expression = "";
  private static int curIndex;
  private static object? result = null;
  private static CmdList CmdList = new CmdList();

  public static bool GetBinaryOperation(out Operation op)
  {
    char s = GetCurrentChar();
    op = 0;
    if (s == '>')
    {
      if (IsNotEnd() && expression[curIndex + 1] == '=')
      {
        curIndex += 2;
        op = Operation.MoreOrEqual;
        return true;
      }
      else
      {
        curIndex++;
        op = (Operation)s;
        return true;
      }
    }

    if (s == '<')
    {
      if (IsNotEnd() && expression[curIndex + 1] == '=')
      {
        curIndex += 2;
        op = Operation.LessOrEqual;
        return true;
      }
      else
      {
        curIndex++;
        op = (Operation)s;
        return true;
      }
    }

    if (ParseString("&&"))
    {
      op = Operation.LogicalAnd; return true;
    }
    if (ParseString("||"))
    {
      op = Operation.LogicalOr; return true;
    }

    switch ((Operation)s)
    {
      case Operation.Add:
      case Operation.Subtract:
      case Operation.Multiply:
      case Operation.Divide:
      case Operation.MoreThan: 
      case Operation.LessThan:
        op = (Operation)s;
        curIndex++;
        return true;
      case Operation.Negation:
        if (IsNotEnd() && expression[curIndex + 1] == '=')
        {
          curIndex += 2;
          // TODO Add "!="
        }
        else
        {
          op = (Operation)s;
        }
        curIndex++;
        return true;
      default:
        return false;
    }
  }


  public static bool IsSymbol(char s)
  {
    switch (s)
    {
      case (char)Symbol.Space:
      case (char)Symbol.Tab:
      case (char)Symbol.NewLine:
      case (char)Symbol.CarriageReturn:
      case (char)Symbol.FormFeed:
        return true;
      default:
        return false;
    }
  }

  public static bool GetUnaryOperation(char s, out Operation op)
  {
    op = 0;
    switch (s)
    {
      case (char)Operation.Add:
      case (char)Operation.Subtract:
      case (char)Operation.Negation:
        op = (Operation)s;
        return true;
      default:
        return false;
    }
  }

  public static object? CompileExpression(string s)
  {
    expression = s;
    curIndex = 0;
    try
    {
      var res = ParseExpr();
      if (IsNotEnd())
      {
        return 555;
      }

      if (!res)
      {
        return 222;
      }

      return Compiler.GetResult();
    }
    catch (Exception)
    {
      return 333;
    }
  }

  public static object? Compile(string s)
  {
    expression = s;
    curIndex = 0;
    try
    {
      var res = ParseOperators();
      if (IsNotEnd() && result == null)
      {
        return 555;
      }
      if (!res)
      {
        return 222;
      }
      return result;
    }
    catch (Exception)
    {
      return 333;
    }
  }

  public static bool Parse(string s)
  {
    Variable.ClearVariables();
    CmdList.Clear();
    expression = s;
    curIndex = 0;
    try
    {
      var res = ParseOperators();
      if (IsNotEnd())
      {
        return false;
      }
      return res;
    }
    catch (Exception)
    {
      return false;
    }
  }

  public static bool ParseExpression(string s)
  {
    expression = s;
    curIndex = 0;
    try
    {
      var res = ParseExpr();
      if (IsNotEnd())
      {
        return false;
      }
      return res;
    }
    catch (Exception)
    {
      return false;
    }
  }

  public static bool ParseOperators()
  {
    while (IsNotEnd())
    {
      if (ParseReturn())
      {
        return true;
      }
      ParseAssign();
    }
    if (!IsNotEnd())
    {
      return true;
    }

    return false;
  }

  public static bool ParseAssign()
  {
    var name = ParseName();
    if (name == "")
    {
      return false;
    }

    if (!ParseChar('='))
    {
      return false;
    }

    ParseExpr();

    if (!ParseChar(';'))
    {
      throw new ParserException("Error");
    }

    //Variable.AddVariable(name);
    //Variable.SetVariable(name, Compiler.GetResult());
    CmdList.AddVar(curIndex, name);
    CmdList.AddAssign(curIndex, Operation.Assign);

    return true;
  }

  public static bool ParseReturn()
  {
    if (!ParseString("return"))
    {
      return false;
    }

    if (ParseChar(';'))
    {
      result = 0;
      CmdList.AddReturn(curIndex);
      return true;
    }

    ParseExpr();

    if (!ParseChar(';'))
    {
      throw new ParserException("Error");
    }
    CmdList.AddReturn(curIndex);

    //result = Compiler.GetResult();
    return true;
  }

  public static string ParseName()
  {
    Skip();

    var prevInd = curIndex;


    if (!IsNotEnd() || !(char.IsAsciiLetter(GetCurrentChar()) || GetCurrentChar() == '_'))
    {
      return "";
    }

    while (IsNotEnd() && (char.IsAsciiLetterOrDigit(GetCurrentChar()) || GetCurrentChar() == '_'))
    {
      curIndex++;
    }

    if (curIndex > prevInd)
    {
      return expression[prevInd..curIndex];
    }

    return "";
  }


  public static bool ParseExpr()
  {
    ParseUnary();
    if (!ParseOperand())
    {
      return false;
    }

    while (ParseBinary())
    {
      if (!ParseOperand())
      {
        return false;
      }
    }

    //Compiler.ExecuteMany(Operation.End);

    CmdList.AddEndExpr(curIndex);
    return true;
  }

  public static bool ParseOperand()
  {
    if (ParseNum(out object? num))
    {
      if (num != null)
      {
        //Compiler.PushNumber(num);
        CmdList.AddConst(curIndex, num);
      }
      return true;
    }
    else if (ParseTypeStr(out object? str))
    {
      if (str != null)
      {
        //Compiler.PushData(str);
        CmdList.AddConst(curIndex, str);
      }
      return true;
    }
    else if (ParseBool(out object? boolean))
    {
      if (boolean != null)
      {
        //Compiler.PushData(boolean);
        CmdList.AddConst(curIndex, boolean);
      }
      return true;
    }
    else
    {
      if (ParseVar())
      {
        return true;
      }
    }

    if (ParseChar((char)Operation.LeftBracket))
    {
      //Compiler.PushOperation(Operation.LeftBracket);
      CmdList.AddOperation(curIndex, Operation.LeftBracket);
      ParseExpr();
      if (ParseChar((char)Operation.RightBracket))
      {
        //Compiler.ExecuteParanthesis();
        CmdList.AddOperation(curIndex, Operation.RightBracket);
        return true;
      }

      return false;
    }

    return false;
  }

  public static bool ParseVar()
  {
    var name = ParseName();
    if (name == "")
    {
      return false;
    }

    if (!Variable.HasVariable(name))
    {
      throw new ParserException($"Variable not found: {name}");
    }

    //Compiler.PushNumber(Variable.GetVariable(name));
    CmdList.AddVar(curIndex, name);

    return true;
  }

  public static bool ParseNum(out object? a)
  {
    Skip();

    var prevInd = curIndex;

    while (IsNotEnd() && char.IsDigit(GetCurrentChar()))
    {
      curIndex++;
    }

    a = curIndex > prevInd ? int.Parse(expression[prevInd..curIndex]) : null;

    return curIndex > prevInd;
  }

  public static bool ParseTypeStr(out object? a)
  {
    Skip();

    var prevInd = curIndex;

    if (IsNotEnd() && GetCurrentChar().Equals('"'))
    {
      curIndex++;
      a = string.Empty;
    }
    else
    {
      a = null;
      return false;
    }

    while (IsNotEnd())
    {
      if (GetCurrentChar().Equals('"'))
      {
        curIndex++;
        break;
      }
      a += GetCurrentChar().ToString();
      curIndex++;
    }

    return curIndex > prevInd;
  }

  public static bool ParseBool(out object? a)
  {
    Skip();

    var prevInd = curIndex;
    a = null;

    if (IsNotEnd()) 
    {
      if (ParseString("true"))
      {
        a = true;
      }
      else if (ParseString("false"))
      {
        a = false;
      }
      else
      {
        prevInd = curIndex;
      }
    }

    return curIndex > prevInd;
  }


  public static bool ParseChar(char symbol)
  {
    Skip();

    if (IsNotEnd() && GetCurrentChar().Equals(symbol))
    {
      curIndex++;
      return true;
    }

    return false;
  }

  public static bool ParseString(string str)
  {
    Skip();

    if (curIndex + str.Length < expression.Length && expression.Substring(curIndex, str.Length).Equals(str))
    {
      curIndex += str.Length;
      return true;
    }

    return false;
  }


  public static bool ParseBinary()
  {
    Skip();
    Operation oper = 0;

    if (IsNotEnd() && GetBinaryOperation(out oper))
    {
      //Compiler.ExecuteMany(oper);
      //Compiler.PushOperation(oper);
      //curIndex++;
      CmdList.AddOperation(curIndex, oper);
      return true;
    }

    return false;
  }


  private static void Skip()
  {
    while (IsNotEnd() && IsSymbol(GetCurrentChar()))
    {
      curIndex++;
    }

    if (curIndex < expression.Length - 1
        && GetCurrentChar().Equals((char)Operation.Divide)
        && expression[curIndex + 1].Equals((char)Operation.Divide))
    {
      curIndex += 2;
      while (IsNotEnd() && !GetCurrentChar().Equals((char)Symbol.NewLine))
      {
        curIndex++;
      }

      if (IsNotEnd())
      {
        curIndex++;
        Skip();
      }
    }
  }


  public static void ParseUnary()
  {
    Skip();
    Operation oper = 0;

    if (IsNotEnd() && GetUnaryOperation(GetCurrentChar(), out oper))
    {
      CmdList.AddOperation(curIndex, oper);
      //if (GetCurrentChar() == (char)Operation.Subtract)
      //{
      //  Compiler.PushOperation(Operation.UnaryMinus);
      //}
      //else if (GetCurrentChar() == (char)Operation.Negation)
      //{
      //  Compiler.PushOperation(Operation.Negation);
      //}
      curIndex++;
    }
  }

  private static bool IsNotEnd()
  {
    return curIndex < expression.Length;
  }

  public static char GetCurrentChar()
  {
    return expression[curIndex];
  }
}