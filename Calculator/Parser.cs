using System;

namespace Calculator;

public static class Parser
{

    public static void Main()
    {
        Console.WriteLine(Parser.Parse("1+2"));
    }

    private static string expression = "";
    private static int curIndex;
    private static object? result = null;

    public static bool IsBinaryOperation(char s)
    {

        switch (s)
        {
            case (char)Operation.Add:
            case (char)Operation.Subtract:
            case (char)Operation.Multiply:
            case (char)Operation.Divide:
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

    public static bool IsUnaryOperation(char s)
    {
        switch (s)
        {
            case (char)Operation.Add:
            case (char)Operation.Subtract:
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

        Variable.AddVariable(name);
        Variable.SetVariable(name, Compiler.GetResult());

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
            return true;
        }

        ParseExpr();

        if (!ParseChar(';'))
        {
            throw new ParserException("Error");
        }

        result = Compiler.GetResult();
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

        Compiler.ExecuteMany((char)Operation.End);
        return true;
    }

    public static bool ParseOperand()
    {
        if (ParseNum(out object? a))
        {
            if (a != null)
            {
                Compiler.PushNumber(a);
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
            Compiler.PushOperation(Operation.LeftBracket);
            ParseExpr();
            if (ParseChar((char)Operation.RightBracket))
            {
                Compiler.ExecuteParanthesis();
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

        Compiler.PushNumber(Variable.GetVariable(name));

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

        if (IsNotEnd() && IsBinaryOperation(GetCurrentChar()))
        {
            Compiler.ExecuteMany(GetCurrentChar());
            Compiler.PushOperation(GetCurrentChar());
            curIndex++;
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

        if (IsNotEnd() && IsUnaryOperation(GetCurrentChar()))
        {
            if (GetCurrentChar() == (char)Operation.Subtract)
            {
                Compiler.PushOperation(Operation.UnaryMinus);
            }
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

    private static void ClearData()
    {
        curIndex = 0;
        
    }
}